<%@ WebHandler Language="C#" Class="AblumUpdate" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;


public class AblumUpdate : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        context.Response.Write("Hello World");

        int aid = Convert.ToInt32(context.Request.Form["aid"].ToString());
        string ablumname = context.Request.Form["ablumName"].ToString();
        localhost.AblumService AS = new localhost.AblumService();
        AS.CookieContainer = (CookieContainer)context.Session["cookie"];

        context.Response.Clear();
        context.Response.Write(AS.UpdateAblum(aid, ablumname));
        context.Response.End();

    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}