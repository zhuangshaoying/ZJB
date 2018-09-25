using System.Collections.Generic;
using Senparc.Weixin.MP.Entities;
using ZJB.Api.BLL;
using ZJB.Api.Model;


namespace Senparc.Weixin.MP.Sample.CommonService.MessageHandlers.Rules
{


    /// <summary>
    ///KeyWordRule 的摘要说明
    /// </summary>


    public class KeyWordRule : IRule
    {
        //根据关键词获取信息
        public IResponseMessageBase GetMessage(RequestMessageText requestMessage)
        {

            IResponseMessageBase responseMessage = null;
            var responseMessageNews = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageNews>(requestMessage);
            var responseMessageText = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            WxBll weChatApi = new WxBll();
            int count = 0;
            List<KeyWordReply> list = new List<KeyWordReply>();

            list = weChatApi.GetKeyWordReply(requestMessage.Content);

            if (list != null && list.Count > 0)
            {

                for (var i = 0; i < list.Count; i++)
                {
                    if (list[i].Url != null && !string.IsNullOrEmpty(list[i].Url))
                    {
                        responseMessageNews.Articles.Add(new Article()
                        {
                            Title = list[i].Title,
                            Description = list[i].Description,
                            PicUrl = list[i].PicUrl,
                            Url = list[i].Url,
                        });
                    }
                    else
                    {

                        responseMessageText.Content = list[i].Description;
                        responseMessage = responseMessageText;
                        return responseMessage;
                    }
                }
                responseMessage = responseMessageNews;
            }


            return responseMessage;
        }

    }


   
}