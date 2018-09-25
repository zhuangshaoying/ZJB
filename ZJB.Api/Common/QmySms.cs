namespace ZJB.Api.Common
{

    internal class QmySms
    {
        public string AccountSID { get; set; }
        public string AuthToken { get; set; }
        public string Version { get; set; }
        public string AppID { get; set; }
        public string EmailTemplateID { get; set; }
        public string To { get; set; }
        public string Param { get; set; }
    }

}
