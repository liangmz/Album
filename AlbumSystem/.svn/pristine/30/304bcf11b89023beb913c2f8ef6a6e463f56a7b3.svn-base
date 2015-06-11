<%@ WebHandler Language="C#" Class="ValiEmail" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;

public class ValiEmail : IHttpHandler, IRequiresSessionState
{ 
    
    public void ProcessRequest (HttpContext context) {
        localhost.AblumService AS = new localhost.AblumService();
        AS.CookieContainer = (CookieContainer)context.Session["cookie"];
        string URL = context.Request.Url.Scheme + "://" + context.Request.Url.Authority + "/Email.aspx";
        //string URL = "http://localhost:84/Email.aspx";
        int Ans = AS.ValidateEmail_GetCode(URL);
        if (Ans == 1 )
        {
            context.Response.Write(1);
            context.Response.End();
        }
        else if(Ans == -1){ context.Response.Write(-1); context.Response.End(); }
        else{context.Response.Write(0); context.Response.End();}
        
    }
    
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}