﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using System.Drawing;
using System.Xml;

public partial class Infomation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uid"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        if (Request["info_old_password"] != null && Session["cookie"] != null)
        {
            string oldPassword = Request["info_old_password"].ToString();
            string newPassword = Request["info_new_password"] == null ? null : Request["info_new_password"].ToString();
            string newEmail = Request["info_newEmail"] == null ? null : Request["info_newEmail"].ToString();

            HttpPostedFile pic = Request.Files["info_new_pic"];
            
            //得到上传文件的长度 
            int upFileLength = pic.ContentLength;
            //得到上传文件的客户端MIME类型 
            string contentType = pic.ContentType;
            byte[] FileArray = new Byte[upFileLength];

            Stream fileStream = pic.InputStream;
            fileStream.Read(FileArray, 0, upFileLength);
            /*
            System.Drawing.Image image = System.Drawing.Image.FromStream(pic.InputStream);
            int height = image.Height;
            int width = image.Width;
            
            image.Dispose();*/

            string exname = Path.GetExtension(pic.FileName);

            localhost.AblumService AS = new localhost.AblumService();
            localhost.UpdateSoapHeader ush = new localhost.UpdateSoapHeader();
            ush.password = oldPassword;
            AS.UpdateSoapHeaderValue = ush;
            AS.CookieContainer = (CookieContainer)Session["cookie"];
            String Ans_XML = AS.UpdataUserInfo(newPassword.Length <= 0 ? null : newPassword
                , newEmail.Length <= 0 ? null : newEmail
                , fileStream.Length <= 0 ? null : FileArray
                , exname);

            fileStream.Close();
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(Ans_XML);
                XmlElement root = xml.DocumentElement;
                XmlNode UpdateUser = root.SelectSingleNode("UpdateUser");
                XmlNode EuserName = UpdateUser.SelectSingleNode("USERNAME");
                XmlNode Eemail = UpdateUser.SelectSingleNode("EMAIL");
                XmlNode EUid = UpdateUser.SelectSingleNode("UID");
                XmlNode EPic = UpdateUser.SelectSingleNode("PIC");

                HttpContext.Current.Session["userName"] = EuserName.InnerText;
                HttpContext.Current.Session["email"] = Eemail.InnerText;
                HttpContext.Current.Session["uid"] = EUid.InnerText;
                HttpContext.Current.Session["pic"] = EPic.InnerText;
            }
            catch (Exception) { }
            if (Ans_XML == null || Ans_XML.Length <= 0)
            {
                errorInfo.InnerHtml = "旧密码错误!";
            }
            else { errorInfo.InnerHtml = "成功!"; }

        }
        if (Session["userName"] != null)
        {
            string img;// = "data:image/jpeg;base64,";
            string mypics = Session["pic"].ToString();
            img = mypics;
            mypic.Src = img;
        }
    }
}