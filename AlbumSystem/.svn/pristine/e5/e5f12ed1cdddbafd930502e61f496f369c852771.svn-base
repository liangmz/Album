<%@ WebHandler Language="C#" Class="MyLook" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;

public class MyLook : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {

        localhost.AblumService AS = new localhost.AblumService();
        AS.CookieContainer = (CookieContainer)context.Session["cookie"];
        int page = Convert.ToInt32(context.Request.Form["page"].ToString());
        string xml = AS.FriendsList(page);

        context.Response.Clear();
        context.Response.Write(xml);
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}