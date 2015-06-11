<%@ WebHandler Language="C#" Class="TagList" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;

public class TagList : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {

        int PID = Convert.ToInt32(context.Request["pid"].ToString());

        localhost.AblumService AS = new localhost.AblumService();
        AS.CookieContainer = (CookieContainer)context.Session["cookie"];

        String reXml = AS.TagList(PID);
        
        context.Response.Clear();
        context.Response.Write(reXml);
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}