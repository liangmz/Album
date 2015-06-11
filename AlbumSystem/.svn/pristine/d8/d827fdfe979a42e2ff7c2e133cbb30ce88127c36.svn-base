<%@ WebHandler Language="C#" Class="TagDel" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;
using System.Collections.Generic;

public class TagDel : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {

        List<int> TID = new List<int>();

        int PID = Convert.ToInt32( context.Request["pid"].ToString());
        String[] tid = context.Request["tid"].ToString().Split(',');

        for (int i = 0; i < tid.Length; i++)
        {
            try {
                TID.Add(Convert.ToInt32(tid[i])); 
            }
            catch (Exception) { } 
        }
        
        
        localhost.AblumService AS = new localhost.AblumService();
        AS.CookieContainer = (CookieContainer)context.Session["cookie"];

        int[] TIDS = TID.ToArray();
        string reStr = AS.DelTag(PID, TIDS);

        context.Response.Clear();
        context.Response.Write(reStr);
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}