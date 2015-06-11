<%@ WebHandler Language="C#" Class="UnLook" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;

public class UnLook : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        int uid = Convert.ToInt32(context.Request.Form["uid"].ToString());
        localhost.AblumService AS = new localhost.AblumService();
        AS.CookieContainer = (CookieContainer)context.Session["cookie"];
        int[] u = { uid};
        context.Response.Clear();
        context.Response.Write(AS.DelFriend(u));
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}