using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZJB.Api.BLL;
using ZJB.Api.Entity;

namespace ZJB.WX.Common
{
    public static class PostDataHelper 
    {
        //
        // GET: /PostDataHelper/
        private static NCDBEntities WX = new NCBaseRule().CurrentEntities;
        public static int RMappingAreaID(string targetId, int siteId, int? uponId)
        {
            ObjectResult<P_Poster_RMappingRegion_Result> results = WX.P_Poster_RMappingRegion(targetId, uponId,
                siteId);
            P_Poster_RMappingRegion_Result[] tmp = results.ToArray();
            return tmp.Any() ? tmp.First().RegionID : 0;
        }
        public static int RMappingCommunityID(string targetId, int siteId)
        {
            ObjectResult<P_Poster_RMappingCommunity_Result> results = WX.P_Poster_RMappingCommunity(targetId,
                siteId);
            P_Poster_RMappingCommunity_Result[] tmp = results.ToArray();
            return tmp.Any() ? tmp.First().CommunityID : 0;
        }
        public static P_Poster_RMappingCommunity_Result RMappingCommunityModel(string targetId, int siteId)
        {
            ObjectResult<P_Poster_RMappingCommunity_Result> results = WX.P_Poster_RMappingCommunity(targetId,
                siteId);
            P_Poster_RMappingCommunity_Result[] tmp = results.ToArray();
            return tmp.Any() ? tmp.First():null;
        }
    }
}
