<%@ WebHandler Language="C#" Class="Look" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;

public class Look : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        context.Response.Write("Hello World");
        localhost.AblumService AS = new localhost.AblumService();
        AS.CookieContainer = (CookieContainer)context.Session["cookie"];
        int UID = Convert.ToInt32(context.Request.Form["UID"].ToString());
        int ans = AS.AddFriend(UID);
        
        context.Response.Clear();
        context.Response.Write(ans);
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}