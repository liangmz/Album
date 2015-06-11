<%@ WebHandler Language="C#" Class="UpdatePic" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;

public class UpdatePic : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        
        int PID = Convert.ToInt32(context.Request["pid"].ToString());
        //int AID = Convert.ToInt32(context.Request["aid"].ToString());
        string PIC_NAME = context.Request["picname"].ToString();
        string CONTENT = context.Request["content"].ToString();
        
        localhost.AblumService AS = new localhost.AblumService();
        AS.CookieContainer = (CookieContainer)context.Session["cookie"];
        int BOOL = AS.UpdatePicInfo(PID,PIC_NAME,-1,CONTENT);
        context.Response.Clear();
        context.Response.Write(BOOL);
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}