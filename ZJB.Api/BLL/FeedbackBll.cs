using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJB.Api.DAL;
using ZJB.Api.Models;
using ZJB.Core.Injection;

namespace ZJB.Api.BLL
{
    public class FeedbackBll
    {
        private readonly FeedbackDal feedbackDal = Container.Instance.Resolve<FeedbackDal>();

        #region 列表
        public virtual List<FeedbackModel> GetFeedbackList(FeedbackListReq parame, ref int totalSize)
        {
            return feedbackDal.GetFeedbackList(parame, ref  totalSize);
        }
        #endregion
    }
}
