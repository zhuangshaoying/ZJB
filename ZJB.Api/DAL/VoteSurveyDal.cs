using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJB.Api.Entity;
using ZJB.Api.Models;
using System.Data;
using System.Data.Common;
using log4net;
using ZJB.Api.BLL;
namespace ZJB.Api.DAL
{
     public  class VoteSurveyDal:BaseDal
    {
         public VoteSurveyDal()
            : base("WX")
        { }
         private ILog logger = LogManager.GetLogger("VoteSurveyDal");
         private NCBaseRule ncBase = new NCBaseRule();
       
         #region 添加投票
         public virtual int VoteAdd(VoteSurveyName surveyItem, List<VoteOptionName> optionList)
         {
             DbCommand cmd = GetStoredProcCommand("P_Vote_Add");
             AddInParameter(cmd, "@SurveyName", DbType.String, surveyItem.SurveyName);
             AddInParameter(cmd, "@CityId", DbType.Int32, surveyItem.CityId);
             AddInParameter(cmd, "@UserId", DbType.Int32, surveyItem.UserId);
             AddInParameter(cmd, "@SurveyType", DbType.Int32, surveyItem.SurveyType);
             AddInParameter(cmd, "@ViewData", DbType.Int32, surveyItem.ViewData);
             AddInParameter(cmd, "@EndTime", DbType.DateTime,surveyItem.EndTime);
             AddInParameter(cmd, "@IP", DbType.String, ZJB.Web.Utilities.IpUtility.GetIp());
             AddInParameter(cmd, "@OptionNameList", DbType.Xml, ZJB.Core.Utilities.XmlUtility.Serialize(optionList, Encoding.UTF8, "OptionNameList"));
             int result = ExecuteNonQuery(cmd);
             return result;
         }
        #endregion
         #region 获取投票列表
         public virtual List<VoteSurveyName> GetSurveyNameListByIds(List<int> ids)
         {
            List<VoteSurveyName> voteList=(from id in ids
                                               from vote in ncBase.CurrentEntities.VoteSurveyName.Where(v=>v.VoteId==id && v.Status==1)
                                              select vote
                                               ).ToList();
            return voteList;
               
         }
         public virtual List<VoteOptionName> GetOptionNameListByVoteIds(List<int> ids)
         {
             List<VoteOptionName> voteOptionList = (from id in ids
                                              from vote in ncBase.CurrentEntities.VoteOptionName.Where(v => v.VoteId == id)
                                              select vote
                                                ).ToList();
             return voteOptionList;
         }
         public virtual List<VoteSurveySubmitRecord> GetSubmitRecordListByVoteIds(List<int> ids)
         {
             List<VoteSurveySubmitRecord> voteRecordList = (from id in ids
                                                    from vote in ncBase.CurrentEntities.VoteSurveySubmitRecord.Where(v => v.VoteId== id)
                                                    select vote
                                                ).ToList();
             return voteRecordList;
         }
         #endregion
         #region 参与投票
         public virtual int SubmitVote(int VoteId, int UserId, List<VoteOptionName> optionList,int isChange, ref int dynamicId)
         {
             DbCommand cmd = GetStoredProcCommand("P_Vote_Submit");
             AddInParameter(cmd, "@UserId", DbType.Int32, UserId);
             AddInParameter(cmd, "@VoteId", DbType.Int32, VoteId);
             AddInParameter(cmd, "@isChange", DbType.Int32, isChange);
             AddOutParameter(cmd, "@DynamicId", DbType.Int32, 4);
             AddInParameter(cmd, "@Ip", DbType.String, ZJB.Web.Utilities.IpUtility.GetIp());
             AddInParameter(cmd, "@OptionList", DbType.Xml, ZJB.Core.Utilities.XmlUtility.Serialize(optionList,Encoding.UTF8,"OptionName"));
             int rows = ExecuteNonQuery(cmd);
             int.TryParse(cmd.Parameters["@DynamicId"].Value.ToString(), out dynamicId);
             return rows;
         }
         #endregion
    }
}
