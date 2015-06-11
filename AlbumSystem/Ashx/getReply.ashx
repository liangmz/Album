<%@ WebHandler Language="C#" Class="getReply" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;

public class getReply : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        localhost.AblumService AS = new localhost.AblumService();
        CookieContainer cookie = null;
        if (context.Session["cookie"] == null)
        {
            cookie = new CookieContainer();
            context.Session["cookie"] = cookie;
        }
        else {
            cookie = (CookieContainer)context.Session["cookie"];
        }
        AS.CookieContainer = cookie;

        int page = Convert.ToInt32(context.Request.Form["page"].ToString());
        int pid = Convert.ToInt32(context.Request.Form["pid"].ToString());
        
        context.Response.Clear();
        context.Response.Write(AS.GetEvaPic(pid,page));
        context.Response.End();
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}