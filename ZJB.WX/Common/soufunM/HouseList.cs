using System;
using System.Xml.Serialization;

namespace ZJB.WX.Common.xms
{
    /// <summary>
    ///     序列化用，勿修改
    /// </summary>
    [XmlRoot("magent_interface", Namespace = "", IsNullable = true)]
    public class HouseListRoot
    {
        //        public object houseinfo { get; set; }
        [XmlElement("houselist")]
        public HouseList[] houselist { get; set; }
    }

    public class HouseList
    {
        public string houseid { get; set; }
        public string price { get; set; }
        public string pricetype { get; set; }
        public string boardtitle { get; set; }
        public string projname { get; set; }
        public string purpose { get; set; }
        public string houseurl { get; set; }
        public string room { get; set; }
        public string hall { get; set; }
        public DateTime registdate { get; set; }
    }
}