<%@ WebHandler Language="C#" Class="AblumPics" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;

public class AblumPics : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        int aid = Convert.ToInt32( context.Request.Form["aid"].ToString());
        int page = Convert.ToInt32(context.Request.Form["page"].ToString());
        localhost.AblumService AS = new localhost.AblumService();
        AS.CookieContainer = (CookieContainer)context.Session["cookie"];

        context.Response.Clear();
        context.Response.Write(AS.GetUserPicList(page,aid));
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}