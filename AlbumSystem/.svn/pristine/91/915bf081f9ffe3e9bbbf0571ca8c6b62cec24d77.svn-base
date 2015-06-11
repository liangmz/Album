<%@ WebHandler Language="C#" Class="Search" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;

public class Search : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        localhost.AblumService AS = new localhost.AblumService();
        string keyword = context.Request.Form["keyword"].ToString();
        int page = Convert.ToInt32(context.Request.Form["page"].ToString());
        string userList = AS.Search(keyword,page);
        context.Response.Clear();
        context.Response.Write(userList);
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}