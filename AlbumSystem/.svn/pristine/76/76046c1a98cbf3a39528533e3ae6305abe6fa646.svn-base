<%@ WebHandler Language="C#" Class="ThePic" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;
using System.Xml;

public class ThePic : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        int pid = Convert.ToInt32(context.Request["pid"].ToString());
        localhost.AblumService AS = new localhost.AblumService();
        AS.CookieContainer = context.Session["cookie"] != null ? (CookieContainer)context.Session["cookie"] : new CookieContainer();

        String PICXML = AS.GetPicInfo(pid);
        XmlDocument XML = new XmlDocument();

        XML.LoadXml(PICXML);
        if (XML.InnerText == "")
        {
            AS.CookieContainer = context.Session["cookie"] != null ? (CookieContainer)context.Session["cookie"] : new CookieContainer();
            PICXML = AS.GetUserSharePicInfo(pid);
            XML.LoadXml(PICXML);
        }
        if (XML.InnerText == "")
        {
            AS.CookieContainer = context.Session["cookie"] != null ? (CookieContainer)context.Session["cookie"] : new CookieContainer();
            PICXML = AS.GetMyPicInfo(pid);
            XML.LoadXml(PICXML);
        }
        
        context.Response.Clear();
        context.Response.Write(PICXML);
        context.Response.End();

    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}