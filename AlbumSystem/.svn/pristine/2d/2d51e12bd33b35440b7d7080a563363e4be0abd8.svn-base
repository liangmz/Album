<%@ WebHandler Language="C#" Class="PicPowerUpdate" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;
using System.Collections.Generic;

public class PicPowerUpdate : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        int aid = Convert.ToInt32(context.Request["aid"].ToString());
        int power = Convert.ToInt32(context.Request["power"].ToString());
        string pidList = context.Request["pidList"].ToString();
        
        List<int> pids = new List<int>();
        string[] Item = pidList.Split(',');
        for (int i = 0; i < Item.Length; i++)
        {
            try
            {
                pids.Add(Convert.ToInt32(Item[i]));
            }
            catch (Exception) { }
        }

        localhost.AblumService AS = new localhost.AblumService();
        AS.CookieContainer = (CookieContainer)context.Session["cookie"];

        string ReStr = AS.UpdatePicPower(pids.ToArray(), aid, power);
        context.Response.Clear();
        context.Response.Write(ReStr);
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}