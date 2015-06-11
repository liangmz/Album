<%@ WebHandler Language="C#" Class="AblumDelete" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;

public class AblumDelete : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        
        localhost.AblumService AS = new localhost.AblumService();
        AS.CookieContainer = (CookieContainer)context.Session["cookie"];
        string ablumName = context.Request.Form["ablumName"].ToString();
        
        context.Response.Clear();
        context.Response.Write(AS.DeleteAblum(ablumName));
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}