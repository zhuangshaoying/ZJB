using System.ComponentModel;
using System.Data;
using Senparc.Weixin.MP.Entities;
using ZJB.Api.BLL;


namespace Senparc.Weixin.MP.Sample.CommonService.MessageHandlers.Rules
{
    /// <summary>
    ///LouPanRule 的摘要说明
    /// </summary>


    public class LouPanRule : IRule
    {

        //根据楼盘名获取信息
        public IResponseMessageBase GetMessage(RequestMessageText requestMessage)
        {

            //logger.Debug("开始运行……");

            IResponseMessageBase responseMessage = null;
            var responseMessageNews = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageNews>(requestMessage);
            CommunityBll communityBll = new CommunityBll();
            var communitys = communityBll.GetCommunityList(requestMessage.Content, 8);
            if (communitys != null && communitys.Count > 0)
            {

                foreach (var community in communitys)
                {
                    responseMessageNews.Articles.Add(new Article()
                    {
                        Title =
                            "【" + community.Name + "】" +
                           community.DistrctName
                            + "，" +
                            (community.SellPrice > 0 ? community.SellPrice + "元/㎡" : "待定"),
                        Description = "",
                        PicUrl = string.IsNullOrEmpty(community.CoverImg) ? "http://www.zhujia001.com/images/nopic.jpg" : community.CoverImg,
                        Url =
                            "http://www.zhujia001.com/Community/Detail?communityId=" +
                            community.CommunityID
                    });
                }
                responseMessage = responseMessageNews;
            }

            return responseMessage;
        }
    }



}