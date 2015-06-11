<%@ WebHandler Language="C#" Class="Register" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;

public class Register : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        context.Response.Write("Hello World");

        string userName = context.Request.Form["userName"].ToString();
        string password = context.Request.Form["password"].ToString();
        string email = context.Request.Form["email"].ToString();
        string code = context.Request.Form["code"].ToString();
        
        localhost.AblumService AS = new localhost.AblumService();
        AS.CookieContainer = (CookieContainer)context.Session["cookie"];
        localhost.RegisterSoapHeader header = new localhost.RegisterSoapHeader();
        header.R_Code = code;
        AS.RegisterSoapHeaderValue = header;

        context.Response.Clear();
        context.Response.Write(AS.Register(userName,password,email));
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}