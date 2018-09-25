using System;
using System.Xml.Serialization;

namespace ZJB.WX.Common.xms
{
    internal class HouseDetail
    {
    }


    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class magent_interface
    {

        private byte resultField;

        private string messageField;

        private string houseurlField;

        private byte videoidField;

        private object videoimgurlField;

        private object videourlField;

        private magent_interfaceHouse houseField;

        /// <remarks/>
        public byte result
        {
            get
            {
                return this.resultField;
            }
            set
            {
                this.resultField = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        public string houseurl
        {
            get
            {
                return this.houseurlField;
            }
            set
            {
                this.houseurlField = value;
            }
        }

        /// <remarks/>
        public byte videoid
        {
            get
            {
                return this.videoidField;
            }
            set
            {
                this.videoidField = value;
            }
        }

        /// <remarks/>
        public object videoimgurl
        {
            get
            {
                return this.videoimgurlField;
            }
            set
            {
                this.videoimgurlField = value;
            }
        }

        /// <remarks/>
        public object videourl
        {
            get
            {
                return this.videourlField;
            }
            set
            {
                this.videourlField = value;
            }
        }

        /// <remarks/>
        public magent_interfaceHouse house
        {
            get
            {
                return this.houseField;
            }
            set
            {
                this.houseField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class magent_interfaceHouse
    {

        private uint houseidField;

        private string districtField;

        private string comareaField;

        private string addressField;

        private string purposeField;

        private uint projcodeField;

        private string projnameField;

        private decimal buildingareaField;

        private decimal priceField;

        private string pricetypeField;

        private string boardtitleField;

        private object inneridField;

        private string photourlField;

        private object featureField;

        private object infocodeField;

        private System.DateTime inserttimeField;

        private System.DateTime modifydateField;

        private byte isvideoField;

        private byte imagecountField;

        private byte isbestField;

        private byte depositstatusField;

        private uint agentidField;

        private string agentnameField;

        private byte isorderField;

        private byte ispayField;

        private byte exhibitsortField;

        private byte tagField;

        private System.DateTime tagaddtimeField;

        private byte statusField;

        private byte isvalidField;

        private object sourceField;

        private byte isrealhouseField;

        private byte floorField;

        private byte totalfloorField;

        private byte liveareaField;

        private string forwardField;

        private int createtimeField;

        private string baseserviceField;

        private string fitmentField;

        private string lookhousetypeField;

        private string propertysubtypeField;

        private byte roomField;

        private byte hallField;

        private byte toiletField;

        private byte kitchenField;

        private byte balconyField;

        private string payinfoField;

        private byte isnewhouseField;

        private byte workshopareaField;

        private byte spaceareaField;

        private byte garageField;

        private byte parkingplaceField;

        private object shoptypeField;

        private byte propertyfeeField;

        private object propertycompanyField;

        private byte isdivisibilityField;

        private object aimoperastionField;

        private object propertygradeField;

        private byte floorareaField;

        private byte officeareaField;

        private object dormitoryField;

        private object spanField;

        private byte floorheightField;

        private byte loadbearingField;

        private object waterField;

        private byte switchingcapacity_nowField;

        private byte switchingcapacity_maxField;

        private string buildingtypeField;

        private string housestructureField;

        private string purposeTypeField;

        private string houseTypeField;

        private string boardcontentField;

        private object subwayinfoField;

        private object trafficinfoField;

        private System.DateTime registdateField;

        private string cityField;

        private string roomphotourlField;

        private string roomphotourlsField;

        private string[] housephotourlField;

        private string housephotourlsField;

        private string roompphotoidsField;

        private object housephotoidsField;

        private object autophotourlField;

        private object autophotourlsField;

        private object autophotoidsField;

        /// <remarks/>
        public uint houseid
        {
            get
            {
                return this.houseidField;
            }
            set
            {
                this.houseidField = value;
            }
        }

        /// <remarks/>
        public string district
        {
            get
            {
                return this.districtField;
            }
            set
            {
                this.districtField = value;
            }
        }

        /// <remarks/>
        public string comarea
        {
            get
            {
                return this.comareaField;
            }
            set
            {
                this.comareaField = value;
            }
        }

        /// <remarks/>
        public string address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }

        /// <remarks/>
        public string purpose
        {
            get
            {
                return this.purposeField;
            }
            set
            {
                this.purposeField = value;
            }
        }

        /// <remarks/>
        public uint projcode
        {
            get
            {
                return this.projcodeField;
            }
            set
            {
                this.projcodeField = value;
            }
        }

        /// <remarks/>
        public string projname
        {
            get
            {
                return this.projnameField;
            }
            set
            {
                this.projnameField = value;
            }
        }

        /// <remarks/>
        public decimal buildingarea
        {
            get
            {
                return this.buildingareaField;
            }
            set
            {
                this.buildingareaField = value;
            }
        }

        /// <remarks/>
        public decimal price
        {
            get
            {
                return this.priceField;
            }
            set
            {
                this.priceField = value;
            }
        }

        /// <remarks/>
        public string pricetype
        {
            get
            {
                return this.pricetypeField;
            }
            set
            {
                this.pricetypeField = value;
            }
        }

        /// <remarks/>
        public string boardtitle
        {
            get
            {
                return this.boardtitleField;
            }
            set
            {
                this.boardtitleField = value;
            }
        }

        /// <remarks/>
        public object innerid
        {
            get
            {
                return this.inneridField;
            }
            set
            {
                this.inneridField = value;
            }
        }

        /// <remarks/>
        public string photourl
        {
            get
            {
                return this.photourlField;
            }
            set
            {
                this.photourlField = value;
            }
        }

        /// <remarks/>
        public object feature
        {
            get
            {
                return this.featureField;
            }
            set
            {
                this.featureField = value;
            }
        }

        /// <remarks/>
        public object infocode
        {
            get
            {
                return this.infocodeField;
            }
            set
            {
                this.infocodeField = value;
            }
        }

        /// <remarks/>
        public System.DateTime inserttime
        {
            get
            {
                return this.inserttimeField;
            }
            set
            {
                this.inserttimeField = value;
            }
        }

        /// <remarks/>
        public System.DateTime modifydate
        {
            get
            {
                return this.modifydateField;
            }
            set
            {
                this.modifydateField = value;
            }
        }

        /// <remarks/>
        public byte isvideo
        {
            get
            {
                return this.isvideoField;
            }
            set
            {
                this.isvideoField = value;
            }
        }

        /// <remarks/>
        public byte imagecount
        {
            get
            {
                return this.imagecountField;
            }
            set
            {
                this.imagecountField = value;
            }
        }

        /// <remarks/>
        public byte isbest
        {
            get
            {
                return this.isbestField;
            }
            set
            {
                this.isbestField = value;
            }
        }

        /// <remarks/>
        public byte depositstatus
        {
            get
            {
                return this.depositstatusField;
            }
            set
            {
                this.depositstatusField = value;
            }
        }

        /// <remarks/>
        public uint agentid
        {
            get
            {
                return this.agentidField;
            }
            set
            {
                this.agentidField = value;
            }
        }

        /// <remarks/>
        public string agentname
        {
            get
            {
                return this.agentnameField;
            }
            set
            {
                this.agentnameField = value;
            }
        }

        /// <remarks/>
        public byte isorder
        {
            get
            {
                return this.isorderField;
            }
            set
            {
                this.isorderField = value;
            }
        }

        /// <remarks/>
        public byte ispay
        {
            get
            {
                return this.ispayField;
            }
            set
            {
                this.ispayField = value;
            }
        }

        /// <remarks/>
        public byte exhibitsort
        {
            get
            {
                return this.exhibitsortField;
            }
            set
            {
                this.exhibitsortField = value;
            }
        }

        /// <remarks/>
        public byte tag
        {
            get
            {
                return this.tagField;
            }
            set
            {
                this.tagField = value;
            }
        }

        /// <remarks/>
        public System.DateTime tagaddtime
        {
            get
            {
                return this.tagaddtimeField;
            }
            set
            {
                this.tagaddtimeField = value;
            }
        }

        /// <remarks/>
        public byte status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public byte isvalid
        {
            get
            {
                return this.isvalidField;
            }
            set
            {
                this.isvalidField = value;
            }
        }

        /// <remarks/>
        public object source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }

        /// <remarks/>
        public byte isrealhouse
        {
            get
            {
                return this.isrealhouseField;
            }
            set
            {
                this.isrealhouseField = value;
            }
        }

        /// <remarks/>
        public byte floor
        {
            get
            {
                return this.floorField;
            }
            set
            {
                this.floorField = value;
            }
        }

        /// <remarks/>
        public byte totalfloor
        {
            get
            {
                return this.totalfloorField;
            }
            set
            {
                this.totalfloorField = value;
            }
        }

        /// <remarks/>
        public byte livearea
        {
            get
            {
                return this.liveareaField;
            }
            set
            {
                this.liveareaField = value;
            }
        }

        /// <remarks/>
        public string forward
        {
            get
            {
                return this.forwardField;
            }
            set
            {
                this.forwardField = value;
            }
        }

        /// <remarks/>
        public int createtime
        {
            get
            {
                return this.createtimeField;
            }
            set
            {
                this.createtimeField = value;
            }
        }

        /// <remarks/>
        public string baseservice
        {
            get
            {
                return this.baseserviceField;
            }
            set
            {
                this.baseserviceField = value;
            }
        }

        /// <remarks/>
        public string fitment
        {
            get
            {
                return this.fitmentField;
            }
            set
            {
                this.fitmentField = value;
            }
        }

        /// <remarks/>
        public string lookhousetype
        {
            get
            {
                return this.lookhousetypeField;
            }
            set
            {
                this.lookhousetypeField = value;
            }
        }

        /// <remarks/>
        public string propertysubtype
        {
            get
            {
                return this.propertysubtypeField;
            }
            set
            {
                this.propertysubtypeField = value;
            }
        }

        /// <remarks/>
        public byte room
        {
            get
            {
                return this.roomField;
            }
            set
            {
                this.roomField = value;
            }
        }

        /// <remarks/>
        public byte hall
        {
            get
            {
                return this.hallField;
            }
            set
            {
                this.hallField = value;
            }
        }

        /// <remarks/>
        public byte toilet
        {
            get
            {
                return this.toiletField;
            }
            set
            {
                this.toiletField = value;
            }
        }

        /// <remarks/>
        public byte kitchen
        {
            get
            {
                return this.kitchenField;
            }
            set
            {
                this.kitchenField = value;
            }
        }

        /// <remarks/>
        public byte balcony
        {
            get
            {
                return this.balconyField;
            }
            set
            {
                this.balconyField = value;
            }
        }

        /// <remarks/>
        public string payinfo
        {
            get
            {
                return this.payinfoField;
            }
            set
            {
                this.payinfoField = value;
            }
        }

        /// <remarks/>
        public byte isnewhouse
        {
            get
            {
                return this.isnewhouseField;
            }
            set
            {
                this.isnewhouseField = value;
            }
        }

        /// <remarks/>
        public byte workshoparea
        {
            get
            {
                return this.workshopareaField;
            }
            set
            {
                this.workshopareaField = value;
            }
        }

        /// <remarks/>
        public byte spacearea
        {
            get
            {
                return this.spaceareaField;
            }
            set
            {
                this.spaceareaField = value;
            }
        }

        /// <remarks/>
        public byte garage
        {
            get
            {
                return this.garageField;
            }
            set
            {
                this.garageField = value;
            }
        }

        /// <remarks/>
        public byte parkingplace
        {
            get
            {
                return this.parkingplaceField;
            }
            set
            {
                this.parkingplaceField = value;
            }
        }

        /// <remarks/>
        public object shoptype
        {
            get
            {
                return this.shoptypeField;
            }
            set
            {
                this.shoptypeField = value;
            }
        }

        /// <remarks/>
        public byte propertyfee
        {
            get
            {
                return this.propertyfeeField;
            }
            set
            {
                this.propertyfeeField = value;
            }
        }

        /// <remarks/>
        public object propertycompany
        {
            get
            {
                return this.propertycompanyField;
            }
            set
            {
                this.propertycompanyField = value;
            }
        }

        /// <remarks/>
        public byte isdivisibility
        {
            get
            {
                return this.isdivisibilityField;
            }
            set
            {
                this.isdivisibilityField = value;
            }
        }

        /// <remarks/>
        public object aimoperastion
        {
            get
            {
                return this.aimoperastionField;
            }
            set
            {
                this.aimoperastionField = value;
            }
        }

        /// <remarks/>
        public object propertygrade
        {
            get
            {
                return this.propertygradeField;
            }
            set
            {
                this.propertygradeField = value;
            }
        }

        /// <remarks/>
        public byte floorarea
        {
            get
            {
                return this.floorareaField;
            }
            set
            {
                this.floorareaField = value;
            }
        }

        /// <remarks/>
        public byte officearea
        {
            get
            {
                return this.officeareaField;
            }
            set
            {
                this.officeareaField = value;
            }
        }

        /// <remarks/>
        public object dormitory
        {
            get
            {
                return this.dormitoryField;
            }
            set
            {
                this.dormitoryField = value;
            }
        }

        /// <remarks/>
        public object span
        {
            get
            {
                return this.spanField;
            }
            set
            {
                this.spanField = value;
            }
        }

        /// <remarks/>
        public byte floorheight
        {
            get
            {
                return this.floorheightField;
            }
            set
            {
                this.floorheightField = value;
            }
        }

        /// <remarks/>
        public byte loadbearing
        {
            get
            {
                return this.loadbearingField;
            }
            set
            {
                this.loadbearingField = value;
            }
        }

        /// <remarks/>
        public object water
        {
            get
            {
                return this.waterField;
            }
            set
            {
                this.waterField = value;
            }
        }

        /// <remarks/>
        public byte switchingcapacity_now
        {
            get
            {
                return this.switchingcapacity_nowField;
            }
            set
            {
                this.switchingcapacity_nowField = value;
            }
        }

        /// <remarks/>
        public byte switchingcapacity_max
        {
            get
            {
                return this.switchingcapacity_maxField;
            }
            set
            {
                this.switchingcapacity_maxField = value;
            }
        }

        /// <remarks/>
        public string buildingtype
        {
            get
            {
                return this.buildingtypeField;
            }
            set
            {
                this.buildingtypeField = value;
            }
        }

        /// <remarks/>
        public string housestructure
        {
            get
            {
                return this.housestructureField;
            }
            set
            {
                this.housestructureField = value;
            }
        }

        /// <remarks/>
        public string PurposeType
        {
            get
            {
                return this.purposeTypeField;
            }
            set
            {
                this.purposeTypeField = value;
            }
        }

        /// <remarks/>
        public string HouseType
        {
            get
            {
                return this.houseTypeField;
            }
            set
            {
                this.houseTypeField = value;
            }
        }

        /// <remarks/>
        public string boardcontent
        {
            get
            {
                return this.boardcontentField;
            }
            set
            {
                this.boardcontentField = value;
            }
        }

        /// <remarks/>
        public object subwayinfo
        {
            get
            {
                return this.subwayinfoField;
            }
            set
            {
                this.subwayinfoField = value;
            }
        }

        /// <remarks/>
        public object trafficinfo
        {
            get
            {
                return this.trafficinfoField;
            }
            set
            {
                this.trafficinfoField = value;
            }
        }

        /// <remarks/>
        public System.DateTime registdate
        {
            get
            {
                return this.registdateField;
            }
            set
            {
                this.registdateField = value;
            }
        }

        /// <remarks/>
        public string city
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        public string roomphotourl
        {
            get
            {
                return this.roomphotourlField;
            }
            set
            {
                this.roomphotourlField = value;
            }
        }

        /// <remarks/>
        public string roomphotourls
        {
            get
            {
                return this.roomphotourlsField;
            }
            set
            {
                this.roomphotourlsField = value;
            }
        }

        /// <remarks/>
        public string[] housephotourl
        {
            get
            {
                return this.housephotourlField;
            }
            set
            {
                this.housephotourlField = value;
            }
        }

        /// <remarks/>
        public string housephotourls
        {
            get
            {
                return this.housephotourlsField;
            }
            set
            {
                this.housephotourlsField = value;
            }
        }

        /// <remarks/>
        public string roompphotoids
        {
            get
            {
                return this.roompphotoidsField;
            }
            set
            {
                this.roompphotoidsField = value;
            }
        }

        /// <remarks/>
        public object housephotoids
        {
            get
            {
                return this.housephotoidsField;
            }
            set
            {
                this.housephotoidsField = value;
            }
        }

        /// <remarks/>
        public object autophotourl
        {
            get
            {
                return this.autophotourlField;
            }
            set
            {
                this.autophotourlField = value;
            }
        }

        /// <remarks/>
        public object autophotourls
        {
            get
            {
                return this.autophotourlsField;
            }
            set
            {
                this.autophotourlsField = value;
            }
        }

        /// <remarks/>
        public object autophotoids
        {
            get
            {
                return this.autophotoidsField;
            }
            set
            {
                this.autophotoidsField = value;
            }
        }
    }


  
}