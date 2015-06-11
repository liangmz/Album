<%@ WebHandler Language="C#" Class="ValiPic" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;

public class ValiPic : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        localhost.AblumService AS = new localhost.AblumService();
        CookieContainer cookie = new CookieContainer();
        AS.CookieContainer = cookie;
        context.Session["cookie"] = cookie;
        string picCode = AS.GetRegisterCode(230,60);

        context.Response.Clear();
        context.Response.Write(picCode);
        context.Response.End();
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}