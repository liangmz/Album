<%@ WebHandler Language="C#" Class="Upload" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;
using System.IO;
using System.Drawing;
using System.Security.Cryptography;
using System.Collections.Generic;

public class Upload : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {

        localhost.AblumService AS = new localhost.AblumService();
        AS.CookieContainer = (CookieContainer)context.Session["cookie"];

        int aid = Convert.ToInt32(context.Request.Form["aid"].ToString());
        int power = Convert.ToInt32(context.Request.Form["power"].ToString());//权限
        int ans = 0;

        List<byte[]> File = new List<byte[]>();
        String[] FileName = new String[context.Request.Files.Count];
        String[] Exname = new String[context.Request.Files.Count];
        
        for (int i = 0; i < context.Request.Files.Count; i++)
        {
            try
            {
                HttpPostedFile pic = context.Request.Files[i];
                
                //得到上传文件的长度 
                int upFileLength = pic.ContentLength;
                //得到上传文件的客户端MIME类型 
                string contentType = pic.ContentType;
                byte[] FileArray = new Byte[upFileLength];

                Stream fileStream = pic.InputStream;
                fileStream.Read(FileArray, 0, upFileLength);
                System.Drawing.Image image = System.Drawing.Image.FromStream(fileStream);

                int height = image.Height;
                int width = image.Width;

                image.Dispose();
                fileStream.Close();
                
                File.Add(FileArray);
                Exname[i] = Path.GetExtension(pic.FileName);

                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    FileName[i] = Convert.ToBase64String(md5.ComputeHash(FileArray));
                }
                
                //ans += AS.UploadPic(FileArray, Convert.ToBase64String(md5.ComputeHash(FileArray)), aid, exname);
            }
            catch (Exception)
            { }
        }
        ans = AS.UploadPic_2(File.ToArray(), FileName, aid, Exname, power);
        context.Response.Clear();
        context.Response.Write(ans);
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}