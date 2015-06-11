<%@ WebHandler Language="C#" Class="Share" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;

public class Share : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        localhost.AblumService AS = new localhost.AblumService();
        int page = Convert.ToInt32(context.Request.Form["page"].ToString());
        string xml = AS.GetSharePic(page);
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