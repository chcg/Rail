using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Rail.Misc
{
    public class XmlNameOrValue
    {
        private string val;

        public XmlNameOrValue()
        {
            this.val = string.Empty;
        }

        public XmlNameOrValue(string str)
        {
            this.val = str;
        }

        public static implicit operator string(XmlNameOrValue nov)
        {
            return nov.val;
        }

        public static implicit operator XmlNameOrValue(string str)
        {
            return new XmlNameOrValue(str);
        }

        public static implicit operator double(XmlNameOrValue nov)
        {
            return double.Parse(nov.val, new CultureInfo("en-US"));
        }
    }
}
