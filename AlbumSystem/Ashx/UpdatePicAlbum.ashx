<%@ WebHandler Language="C#" Class="UploadPicAlbum" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;
using System.Collections.Generic;

public class UploadPicAlbum : IHttpHandler, IRequiresSessionState
{
    /// <summary>
    /// 更新图片所属专辑
    /// </summary>
    /// <param name="context"></param>
    public void ProcessRequest (HttpContext context) {

        int aid = Convert.ToInt32(context.Request["aid"].ToString());
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

        string ReStr = AS.UpdatePicAlbum(pids.ToArray(),aid);
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