using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using Newtonsoft.Json;


/// <summary>
/// Handler 的摘要说明
/// </summary>
public abstract class Handler
{
	public Handler(HttpContext context)
	{
        Handler.Request = context.Request;
        this.Response = context.Response;
        this.Context = context;
        this.Server = context.Server;
	}

    public abstract void Process();

    protected void WriteJson(object response)
    {
        string jsonpCallback = Request["callback"],
            json = JsonConvert.SerializeObject(response);
        if (String.IsNullOrWhiteSpace(jsonpCallback))
        {
            Response.AddHeader("Content-Type", "text/plain");
            Response.Write(json);
        }
        else 
        {
            Response.AddHeader("Content-Type", "application/javascript");
            Response.Write(String.Format("{0}({1});", jsonpCallback, json));
        }
        Response.End();
    }

    public static HttpRequest Request { get; private set; }
    public HttpResponse Response { get; private set; }
    public HttpContext Context { get; private set; }
    public HttpServerUtility Server { get; private set; }

    public static string ToMd5String(byte[] data)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] hashResult = md5.ComputeHash(data);
        return BitConverter.ToString(hashResult).Replace("-", string.Empty);
    }
}