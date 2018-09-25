using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Senparc.Weixin.MP.Entities;


namespace Senparc.Weixin.MP.Sample.CommonService.MessageHandlers.Rules
{



    /// <summary>
    ///IRule 的摘要说明
    /// </summary>
    public interface IRule
    {
        IResponseMessageBase GetMessage(RequestMessageText requestMessage);


    }   /// <summary>
        ///IVoiceRule 的摘要说明
        /// </summary>
    public interface IVoiceRule
    {
        IResponseMessageBase GetMessage(RequestMessageVoice requestMessage);


    }
    public interface IScancode_WaitmsgRule
    {
        IResponseMessageBase GetMessage(RequestMessageEvent_Scancode_Waitmsg requestMessage);


    }
}