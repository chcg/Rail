using Rail.Tracks.Trigonometry;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rail.Misc
{
    public class JsonAngleConverter : JsonConverter<Angle>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            throw new NotImplementedException();
        }

        public override Angle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            //reader.GetInt32
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Angle value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
