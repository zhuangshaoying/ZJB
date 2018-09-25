using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using ZJB.Api.BLL;
using ZJB.Api.Models;
using ZJB.Core.Injection;
using System.Web.Http;
using ZJB.WX.Models;


namespace ZJB.WX.Filters
{
    public class TokenAttribute : ActionFilterAttribute
    {

        private readonly UserBll userBll = Container.Instance.Resolve<UserBll>();

        private bool blockIfNoToken = true;

        public bool BlockIfNoToken
        {
            get { return blockIfNoToken; }
            set { blockIfNoToken = value; }
        }

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var httpContext = HttpContext.Current;
            string tokenString = httpContext.Request.Headers["Token"];

            if (string.IsNullOrEmpty(tokenString) && httpContext.Request.Cookies["EnableApiDebug"] != null)
                tokenString = httpContext.Request.QueryString["Token"];

            Guid token;
            if (Guid.TryParse(tokenString, out token))
            {
                Credential credential = userBll.GetCredentialByToken(tokenString);

                if (credential != null)
                {
                    actionContext.Request.SetCredential(credential);
                    base.OnActionExecuting(actionContext);
                    return;
                }
            }

            if (!blockIfNoToken)
            {
                base.OnActionExecuting(actionContext);
                return;
            }

            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden,
                new ApiResponse(Metas.CREDENTIAL_EXPIRED, null));
        }
    }
}
       
