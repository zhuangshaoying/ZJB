using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Services.Description;
using Antlr.Runtime;
using ZJB.Api.BLL;
using ZJB.Api.Entity;
using ZJB.Api.Models;
using ZJB.Core.Utilities;
using ZJB.WX.Common;
using ZJB.WX.Filters;
using ZJB.WX.Models;
using ZJB.Web.Utilities;
namespace ZJB.WX.Controllers
{
    public class AjaxController : ApiController
    {
        private NCBaseRule ncBase = new NCBaseRule();

        #region 获取小区信息
        [System.Web.Http.HttpPost]
        [ValidateInput(false)]

        public CellResponse GetCellsByInput(string inputStr, int buildingType)
        {
            List<Community> conCommunities =
                ncBase.CurrentEntities.Community.Where(o => o.Name.Contains(inputStr)).Take(10).ToList();
            if (conCommunities.IsNoNull())
            {
                dynamic cellls = conCommunities.Select(com =>
               new
               {
                   address = com.Address,
                   area = 0,
                   avgPrice = com.SellPrice,
                   cellCode = com.CommunityID,
                   cellName = com.Name,
                   completionDate = 0,
                   district = com.Distrctid
               });
                return new CellResponse(cellls);
            }

            return new CellResponse(null);
        }
        #endregion

    }
}
