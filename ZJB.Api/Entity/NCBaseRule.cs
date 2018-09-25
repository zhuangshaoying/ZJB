using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJB.Api.Entity;
using ZJB.Core.Data;
using ZJB.Core.Utilities;

namespace ZJB.Api.BLL
{
    public class NCBaseRule
    {
        #region 模型实体
        /// <summary>
        /// 模型变量
        /// </summary>
        public  NCDBEntities objCurrentEntities = null;
        /// <summary>
        /// 模块
        /// </summary>
        public NCDBEntities CurrentEntities
        {
            get
            {
                if (this.objCurrentEntities == null)
                {
                    this.objCurrentEntities = new NCDBEntities(EntitiesHelper.GetEntityConnection<NCDBEntities>(ConfigUtility.GetValue("DBConnectionString")));
                }
                return this.objCurrentEntities;
            }
        }
        #endregion


      
    }
}
