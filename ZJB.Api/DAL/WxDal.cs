using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.Data;
using System.Text;
using log4net;
using ZJB.Api.BLL;
using ZJB.Api.Common;
using ZJB.Api.Entity;
using ZJB.Api.Model;
using ZJB.Api.Models;
using ZJB.Core.Utilities;
using ZJB.Web.Utilities;

namespace ZJB.Api.DAL
{
    public class WxDal : BaseDal
    {
        public WxDal()
            : base("WX")
        { }
        private readonly ICacheManager cache = CacheFactory.GetInstance();
        private ILog logger = LogManager.GetLogger("UserDal");
        private NCBaseRule ncBase = new NCBaseRule();

        /// <summary>
        /// 获取关键词
        /// </summary>
        /// <returns></returns>
        public virtual List<KeyWordReply> GetKeyWordReply(string keyword)
        {
            var keyWordReply = new List<KeyWordReply>();

            using (DbCommand cmd = GetSqlStringCommand(" select top 5 * from WXKeyWordReply where id in ( select ReplyId from WXKeyWord  with(nolock) where KeyWord=@KeyWord group by ReplyId)"))
            {
                AddInParameter(cmd, "@KeyWord", DbType.String, keyword);

                DataSet keyWordReplyDs = ExecuteDataSet(cmd);

                if (keyWordReplyDs != null && keyWordReplyDs.Tables.Count != 0 && keyWordReplyDs.Tables[0].Rows.Count != 0)
                {
                    keyWordReply.AddRange(from DataRow dataRow in keyWordReplyDs.Tables[0].Rows
                                          select new KeyWordReply
                                          {

                                              Title = To<string>(dataRow, "Title"),
                                              Description = To<string>(dataRow, "Description"),
                                              Url = To<string>(dataRow, "Url"),
                                              PicUrl = To<string>(dataRow, "PicUrl"),
                                          });
                }
            }
            return keyWordReply;
        }
    }
}