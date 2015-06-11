<%@ WebHandler Language="C#" Class="LoginOut" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;

public class LoginOut : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        HttpContext.Current.Session.Clear();
        context.Response.Clear();
        context.Response.End();
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}