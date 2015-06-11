<%@ WebHandler Language="C#" Class="AblumCreate" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;

public class AblumCreate : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        localhost.AblumService AS = new localhost.AblumService();
        AS.CookieContainer = (CookieContainer)context.Session["cookie"];
        string CreateAblumName = context.Request.Form["CreateAblumName"].ToString();
        int aid = AS.CreateAblum(CreateAblumName);
        context.Response.Clear();
        context.Response.Write(aid);
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}