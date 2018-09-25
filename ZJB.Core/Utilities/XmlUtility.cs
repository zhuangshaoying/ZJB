using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ZJB.Core.Utilities
{
    public class XmlUtility
    {
        public static string Serialize(object o, Encoding encoding, string rootName)
        {
            XmlSerializer s = new XmlSerializer(o.GetType(), new XmlRootAttribute(rootName));

            using (MemoryStream ms = new MemoryStream())
            {
                s.Serialize(ms, o);
                return encoding.GetString(ms.ToArray());
            }
        }

        public static string Serialize(object o, Encoding encoding)
        {
            XmlSerializer s = new XmlSerializer(o.GetType());

            using (MemoryStream ms = new MemoryStream())
            {
                s.Serialize(ms, o);
                return encoding.GetString(ms.ToArray());
            }
        }


        public static T Deserialize<T>(string xml)
        {
            XmlSerializer s = new XmlSerializer(typeof(T));

            object obj = s.Deserialize(new StringReader(xml));

            return (T)obj;
        }

        public static T Deserialize<T>(string xml, string rootName)
        {
            XmlSerializer s = new XmlSerializer(typeof(T), new XmlRootAttribute(rootName));

            object obj = s.Deserialize(new StringReader(xml));

            return (T)obj;
        }
    }
}
