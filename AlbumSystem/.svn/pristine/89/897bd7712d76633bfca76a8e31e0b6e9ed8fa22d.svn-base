<%@ WebHandler Language="C#" Class="Login" %>

using System;
using System.Web;
using System.Xml;
using System.Web.SessionState;
using System.Net;

public class Login : IHttpHandler, IRequiresSessionState{

    public void ProcessRequest(HttpContext context)
    {
        try
        {
            String userName = context.Request.Form["userName"].ToString();
            String password = context.Request.Form["password"].ToString();

            CookieContainer cookie = new CookieContainer();
            localhost.AblumService AS = new localhost.AblumService();
            AS.CookieContainer = cookie;

            String Ans_XML = AS.Login(userName, password);

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(Ans_XML);
            XmlElement root = xml.DocumentElement;
            XmlNode LoginE = root.SelectSingleNode("Login");
            XmlNode EuserName = LoginE.SelectSingleNode("USERNAME");
            XmlNode Eemail = LoginE.SelectSingleNode("EMAIL");
            XmlNode EUid = LoginE.SelectSingleNode("UID");
            XmlNode EPic = LoginE.SelectSingleNode("PIC");

            HttpContext.Current.Session["userName"] = EuserName.InnerText;
            HttpContext.Current.Session["email"] = Eemail.InnerText;
            HttpContext.Current.Session["uid"] = EUid.InnerText;
            HttpContext.Current.Session["pic"] = EPic.InnerText;
            HttpContext.Current.Session["cookie"] = cookie;

            context.Response.Clear();
            context.Response.Write(EuserName.InnerText);
        }
        catch (Exception)
        {
            context.Response.Clear();
            context.Response.Write(-1);
        }
        context.Response.End(); 
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}