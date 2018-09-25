using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJB.Api.DAL;
using ZJB.Api.Entity;
using ZJB.Core.Injection;
namespace ZJB.Api.BLL
{
    public class VoteSurveyBll
    {
        private readonly VoteSurveyDal voteDal = Container.Instance.Resolve<VoteSurveyDal>();
        #region 添加投票
        public virtual int VoteAdd(VoteSurveyName surveyItem, List<VoteOptionName> optionList)
        {
            return voteDal.VoteAdd(surveyItem, optionList);
        }
        #endregion
        #region 获取投票列表
        
        public virtual List<VoteSurveyName> GetSurveyNameListByIds(List<int> ids)
        {
            return voteDal.GetSurveyNameListByIds(ids);
        }
        public virtual List<VoteOptionName> GetOptionNameListByVoteIds(List<int> ids)
        {
            return voteDal.GetOptionNameListByVoteIds(ids);
        }
        public virtual List<VoteSurveySubmitRecord> GetSubmitRecordListByVoteIds(List<int> ids)
        {
            return voteDal.GetSubmitRecordListByVoteIds(ids);
        }
        #endregion
        #region 参与投票
        public virtual int SubmitVote(int VoteId, int UserId, List<VoteOptionName> optionList,int isChange, ref int dynamicId)
        {
            return voteDal.SubmitVote(VoteId, UserId, optionList,isChange, ref dynamicId);

        }
        #endregion
    }
}
