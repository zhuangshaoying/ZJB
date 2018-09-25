using System;
using System.Web;

namespace ZJB.Web.PermanentlyMoved
{
    //todo: support regular expression
    public class PermanentlyMovedModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication application)
        {
            application.BeginRequest += BeginRequest;
        }

        void BeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;

            HttpRequest request = application.Context.Request;

            PermanentlyMovedConfig connfig = PermanentlyMovedConfig.GetPermanentlyMovedConfig();

            PermanentlyMoved permanentlyMoved =
                connfig.Find(pm => request.Path.Equals(pm.SourceUrl, StringComparison.OrdinalIgnoreCase));

            if (permanentlyMoved == null) return;

            string newUrl = permanentlyMoved.TargetUrl;

            if (permanentlyMoved.WithQuery)
            {
                string query = request.QueryString.ToString();

                if (!string.IsNullOrEmpty(query))
                {
                    if (!newUrl.Contains("?"))
                        newUrl += "?";
                    else
                        newUrl += "&";

                    newUrl += query;
                }
            }

            PermanentlyMovedUtility.Redirect(newUrl);
        }
    }
}
