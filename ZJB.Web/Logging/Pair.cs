using System;
using System.Xml.Serialization;

namespace ZJB.Web.Logging
{
    [Serializable]
    public class Pair
    {
        private string name;
        private string value;

        [XmlIgnore]
        public string Name
        {
            get { return name; }
            set { this.name = value; }
        }

        [XmlIgnore]
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }

        [XmlElement("Pair")]
        public string PairString
        {
            get { return string.Format("{0}={1}", name, value); }
            set{}
        }
    }
}
