using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rail.Tracks
{
    //public class Person // TrackBase
    //{
    //    public string Name { get; set; }
    //}

    //public class Customer : Person // TrackStraight
    //{
    //    public decimal CreditLimit { get; set; }
    //}

    //public class Employee : Person // TrackCurved
    //{
    //    public string OfficeNumber { get; set; }
    //}



    public class JsonTrackConverter : JsonConverter<List<TrackBase>>
    {
        private Dictionary<string, Type> dict;
        public JsonTrackConverter(Dictionary<string, Type> dict)
        {
            this.dict = dict;
        }

        public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(List<TrackBase>);
        //typeof(TrackBase).IsAssignableFrom(typeToConvert);

        public override List<TrackBase> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }
            reader.Read();
            
            List<TrackBase> list = new List<TrackBase>();

            while (reader.TokenType == JsonTokenType.PropertyName)
            {
                string name = reader.GetString();
                Type type = this.dict[name];
                reader.Read();
                TrackBase track = (TrackBase)JsonSerializer.Deserialize(ref reader, type, options);
                list.Add(track);
            }

            reader.Read();
            return list;
        }

        public override void Write(Utf8JsonWriter writer, List<TrackBase> tracks, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var track in tracks)
            {
                string name = this.dict.First(d => d.Value == track.GetType()).Key;
                //string name = track.GetType().Name;
                writer.WritePropertyName(name);

                JsonSerializer.Serialize(writer, track, track.GetType(), options);
            }
            writer.WriteEndArray();
        }
    }

    //public sealed class JsonPropertyClassAttribute : JsonAttribute
    //{
    //    public JsonPropertyClassAttribute(string name, Type type)
    //    {
    //        this.Name = name;
    //        this.Type = type;
    //    }

    //    public string Name { get; }
    //    public Type Type { get; }
    //}
}