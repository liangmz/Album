<%@ WebHandler Language="C#" Class="Ablum" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;

public class Ablum : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {

        localhost.AblumService AS = new localhost.AblumService();
        AS.CookieContainer = (CookieContainer)context.Session["cookie"];
        int page = Convert.ToInt32(context.Request.Form["page"].ToString());
        string Ablum = AS.MyAblumList(page);
        context.Response.Clear();
        context.Response.Write(Ablum);
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}