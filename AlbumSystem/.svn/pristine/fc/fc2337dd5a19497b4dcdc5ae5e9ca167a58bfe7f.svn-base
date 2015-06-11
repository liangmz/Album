<%@ WebHandler Language="C#" Class="AddTagr" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;

public class AddTagr : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {

        int PID = Convert.ToInt32(context.Request["pid"].ToString());
        string TagName = context.Request["tagName"].ToString();
        
        localhost.AblumService AS = new localhost.AblumService();
        AS.CookieContainer = (CookieContainer)context.Session["cookie"];

        String reStr = AS.AddTag(PID,new String[]{TagName});

        context.Response.Clear();
        context.Response.Write(reStr);
        context.Response.End();
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}