<%@ WebHandler Language="C#" Class="PicRemove" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;
using System.Collections;

public class PicRemove : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest (HttpContext context) {
        String pidList = context.Request["pidList"].ToString();

        String[] pidArray = pidList.Split(',');
        ArrayList pids = new ArrayList();
        
        for (int i = 0; i < pidArray.Length; i++)
        {
            try
            {
                pids.Add(Convert.ToInt32(pidArray[i]));
            }
            catch (Exception)
            { }
        }
        int[] pidL = (int[])pids.ToArray(typeof(int)); 
        
        localhost.AblumService AS = new localhost.AblumService();
        AS.CookieContainer = (CookieContainer)context.Session["cookie"];

        int count = AS.RemovePics(pidL);
        context.Response.Clear();
        context.Response.Write(count);
        context.Response.End();
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}