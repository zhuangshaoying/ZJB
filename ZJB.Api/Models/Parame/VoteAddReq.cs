using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJB.Api.Entity;
namespace ZJB.Api.Models
{
    public class VoteAddReq
    {
        public string SurveyName { get; set; }
        public int SurveyType { get; set; }
        public DateTime EndTime { get; set; }
        public int ViewData { get; set; }
        public string OptionNameList { get; set; }
        public int UserId { get; set; }
        public int CityId { get; set; }
    }
}
