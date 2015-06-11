<%@ WebHandler Language="C#" Class="SearchPic" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;

public class SearchPic : IHttpHandler
{
    public void ProcessRequest (HttpContext context) {
        localhost.AblumService AS = new localhost.AblumService();
        string keyword = context.Request.Form["keyword"].ToString();
        int page = Convert.ToInt32(context.Request.Form["page"].ToString());
        string picList = AS.SearchPic(keyword,page);
        context.Response.Clear();
        context.Response.Write(picList);
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}