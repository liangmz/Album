<%@ WebHandler Language="C#" Class="Reply" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;

public class Reply : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        localhost.AblumService AS = new localhost.AblumService();
        AS.CookieContainer = (CookieContainer)context.Session["cookie"];

        string content = context.Request.Form["content"].ToString();
        int pid = Convert.ToInt32(context.Request.Form["pid"].ToString());

        context.Response.Clear();
        context.Response.Write(AS.EvaPic(pid, content));
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}