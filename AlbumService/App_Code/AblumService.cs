﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.Data;
using System.Text;
using System.Security.Cryptography;
using System.Threading;
using System.Net.Mail;
using System.Net;
using System.Drawing;
using System.IO;
using System.Configuration;

/// <summary>
/// AblumService 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class AblumService : System.Web.Services.WebService
{
    #region ×几乎不关事
    public AblumService()
    {
        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    [WebMethod(EnableSession = true)]
    public string HelloWorld()
    {
        return "Hello World";
    }
    #endregion

    #region 登陆注册下线 OK
    /// <summary>
    /// 获取验证图片
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public String GetRegisterCode(int width, int height)
    {
        byte[] CODE = new byte[1];
        String CodeString = VCode.GetCodeImage(ref CODE, width, height);
        Session["VCode"] = CodeString;
        return Convert.ToBase64String(CODE);

    }

    /// <summary>
    /// 登陆
    /// </summary>
    /// <param name="USERNAME"></param>
    /// <param name="PASSWORD"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true
        , Description = "登陆")]
    public String Login(String USERNAME, String PASSWORD)
    {
        XmlDocument XML = ToXML.GetXml();
        XmlElement ROOT = XML.DocumentElement;
        using (DBSelect DBS = new DBSelect())
        {
            DataSet DS;
            try
            {
                DS = DBS.SelectTheUser(USERNAME);
                if (DS == null) throw new Exception();
            }
            catch (Exception)
            {
                ROOT.InnerText = "";
                return XML.OuterXml;
            }
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            PASSWORD = Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(PASSWORD)));
            md5.Dispose();
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                if (dr["u_userName"].Equals(USERNAME) && dr["u_password"].Equals(PASSWORD))
                {
                    XmlElement EID = XML.CreateElement("UID");
                    EID.InnerText = dr["u_id"].ToString();
                    XmlElement ENAME = XML.CreateElement("USERNAME");
                    ENAME.InnerText = dr["u_userName"].ToString();
                    XmlElement EMAIL = XML.CreateElement("EMAIL");
                    EMAIL.InnerText = dr["u_Email"].ToString();
                    XmlElement ECODE = XML.CreateElement("CODE");
                    ECODE.InnerText = dr["u_Code"].ToString();
                    XmlElement EPIC = XML.CreateElement("PIC");
                    string path = dr["p_path"] == null ? "" : FileIO.GetPicUrl(dr["p_path"].ToString());//FileIO.FileBase64(FileIO.FileRead(dr["p_path"].ToString()));
                    EPIC.InnerText = path;

                    XmlElement LOGINELEMENT = XML.CreateElement("Login");
                    LOGINELEMENT.AppendChild(EID);
                    LOGINELEMENT.AppendChild(ENAME);
                    LOGINELEMENT.AppendChild(EMAIL);
                    LOGINELEMENT.AppendChild(ECODE);
                    LOGINELEMENT.AppendChild(EPIC);

                    ROOT.AppendChild(LOGINELEMENT);

                    Session["u_id"] = dr["u_id"].ToString();
                    Session["userName"] = dr["u_userName"].ToString();
                    Session["email"] = dr["u_Email"].ToString();
                    Session["u_Code"] = dr["u_Code"].ToString();
                    Session["p_path"] = path;
                    Session["password"] = dr["u_password"].ToString();
                    break;
                }
            }
        }
        return XML.OuterXml;
    }

    public RegisterSoapHeader rsh;
    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="USERNAME"></param>
    /// <param name="PASSWORD"></param>
    /// <param name="EMAIL"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true,
        Description = "注册,服务获得验证信息，并存放于请求头中")]
    [SoapHeader("rsh")]
    public int Register(String USERNAME, String PASSWORD, String EMAIL)
    {
        if (rsh == null || !rsh.IsValid(Session["VCode"].ToString()))
        {
            return -1;
        }
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        PASSWORD = Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(PASSWORD)));
        md5.Clear();
        md5.Dispose();
        int Ans = 0;
        using (DBCreateDelete DBC = new DBCreateDelete())
        {
            Ans = DBC.InsertUser(USERNAME, PASSWORD, EMAIL, "");
        }
        return Ans;
    }

    /// <summary>
    /// 下线
    /// </summary>
    /// <returns></returns>
    [WebMethod(EnableSession = true
        , Description = "下线")]
    public int LoginOut()
    {
        Session.Clear();
        return 1;
    }

    public UpdateSoapHeader ush;
    /// <summary>
    /// 用户信息修改
    /// </summary>
    /// <param name="PASSWORD"></param>
    /// <param name="EMAIL"></param>
    /// <param name="IMAGE"></param>
    /// <param name="EXNAME"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true, Description = "用户信息修改")]
    [SoapHeader("ush")]
    public string UpdataUserInfo(String PASSWORD, String EMAIL, byte[] IMAGE,String EXNAME)
    {
        using (MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider())
        {
            if (ush == null || !ush.IsValid(Session["password"].ToString())) 
            {
                return null;
            }
        }
        PASSWORD = PASSWORD == null || PASSWORD.Length <= 0 ? null : PASSWORD;
        EMAIL = EMAIL == null || EMAIL.Length <= 0 ? null : EMAIL;
        XmlDocument XML = ToXML.GetXml();
        XmlElement ROOT = XML.DocumentElement;
        if (Session["u_id"] != null)
        {
            using (MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider())
            {
                PASSWORD = PASSWORD == null ? null : Convert.ToBase64String(MD5.ComputeHash(Encoding.UTF8.GetBytes(PASSWORD)));
            }
            string path = "";
            String md5 = null;
            if (IMAGE != null && IMAGE.Length > 0)
            {
                using (MemoryStream MS = new MemoryStream(IMAGE))
                {
                    Image image = Image.FromStream(MS);
                    md5 = FileIO.FileMD5OfBase64(IMAGE);
                    FileIO.FileSave(IMAGE, md5 + EXNAME, ref path);
                }
            }
            DataSet DS = DBUpdate.UpdateUserInfo(Convert.ToInt32(Session["u_id"].ToString()), PASSWORD, path, md5, EMAIL);
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                XmlElement EID = XML.CreateElement("UID");
                EID.InnerText = dr["u_id"].ToString();
                XmlElement ENAME = XML.CreateElement("USERNAME");
                ENAME.InnerText = dr["u_userName"].ToString();
                XmlElement EEMAIL = XML.CreateElement("EMAIL");
                EEMAIL.InnerText = dr["u_Email"].ToString();
                XmlElement ECODE = XML.CreateElement("CODE");
                ECODE.InnerText = dr["u_Code"].ToString();
                XmlElement EPIC = XML.CreateElement("PIC");
                string PATH = dr["p_path"] == null ? "" : FileIO.GetPicUrl(dr["p_path"].ToString());//FileIO.FileBase64(FileIO.FileRead(dr["p_path"].ToString()));
                EPIC.InnerText = PATH;

                XmlElement LOGINELEMENT = XML.CreateElement("UpdateUser");
                LOGINELEMENT.AppendChild(EID);
                LOGINELEMENT.AppendChild(ENAME);
                LOGINELEMENT.AppendChild(EEMAIL);
                LOGINELEMENT.AppendChild(ECODE);
                LOGINELEMENT.AppendChild(EPIC);

                ROOT.AppendChild(LOGINELEMENT);

                Session["u_id"] = dr["u_id"].ToString();
                Session["userName"] = dr["u_userName"].ToString();
                Session["email"] = dr["u_Email"].ToString();
                Session["u_Code"] = dr["u_Code"].ToString();
                Session["p_path"] = PATH;
                break;

            }
        }
        return XML.OuterXml;
    }

    #endregion

    #region  邮箱验证  密码重设
    [WebMethod(EnableSession = true, Description = "获得邮箱验证代码，到邮箱取")]
    public int ValidateEmail_GetCode(string HOSTURL)
    {
        int UID;
        if (LoginYes(out UID))//用户必须登陆
        {
            if (Session["u_Code"].Equals("OK")) return 0;
            string UN = Session["userName"].ToString();
            string USEREMail = Session["email"].ToString();
            string KEY = VCode.getEmailVCode(USEREMail);

            if (DBUpdate.UpdateUserEmailCode(UID, KEY) == -1) return -1;

            HOSTURL = HOSTURL + "?userName=" + UN + "&key=" + KEY;
            //发一封邮件
            MailAddress From = new MailAddress(ConfigurationManager.AppSettings["systemEmail"]);
            MailAddress To = new MailAddress(USEREMail);

            MailMessage msg = new MailMessage(From, To);

            msg.Priority = MailPriority.Normal;
            msg.IsBodyHtml = true;
            msg.Subject = "Album 邮箱验证";
            msg.Body = "<a href='" + HOSTURL + "'>" + HOSTURL + "</a>";

            SmtpClient sc = new SmtpClient(ConfigurationManager.AppSettings["emailSmtp"]);
            sc.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["systemEmail"], ConfigurationManager.AppSettings["emailPassword"]);
            sc.Send(msg);

            return 1;
        }
        return -1;
    }

    [WebMethod(EnableSession = true, Description = "验证邮箱")]
    public bool ValidateEmail_PutCode(String USERNAME, String KEY)
    {
        using (DBSelect DBS = new DBSelect())
        {
            DataSet DS;
            try
            {
                DS = DBS.SelectTheUser(USERNAME);
                if (DS == null) throw new Exception();
            }
            catch (Exception)
            {
                return false;
            }
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                if (dr["u_userName"].Equals(USERNAME) && dr["u_Code"].Equals(KEY))
                {
                    DBUpdate.SuccessValiEmail(USERNAME);
                    return true;
                }
            }
            return false;
        }
    }

    public UpdateSoapHeader psh;
    /// <summary>
    /// 邮箱获取密码重设链接
    /// </summary>
    /// <param name="EMAIL">邮箱</param>
    /// <param name="HOSTURL">客户端系统地址</param>
    /// <returns>1:OK/-1:未验证/-2:无该用户</returns>
    [WebMethod(EnableSession = true)]
    public int PasswordResetUrl(String EMAIL,String HOSTURL)
    {
        int returnCode = 1;
        String UserName = "";
        using (DBSelect DBS = new DBSelect())
        {
            DataSet DS = DBS.SelectUserForEmail(EMAIL);
            try
            {
                UserName = DS.Tables[0].Rows[0]["u_userName"].ToString();
                string VCODE = DS.Tables[0].Rows[0]["u_Code"].ToString();
                if (!VCODE.Equals("OK"))
                {
                    UserName = "";
                    returnCode = -1;
                }
            }
            catch (Exception) { returnCode = -2; }
        }
        if (returnCode != 1) return returnCode;

        HOSTURL += "?un=" + UserName + "&key=" + VCode.PSWresetCode(UserName);

        MailAddress From = new MailAddress(ConfigurationManager.AppSettings["systemEmail"]);
        MailAddress To = new MailAddress(EMAIL);
        MailMessage msg = new MailMessage(From, To);
        msg.Priority = MailPriority.Normal;
        msg.IsBodyHtml = true;
        msg.Subject = "Album 密码重置";
        msg.Body = "<a href='" + HOSTURL + "'>" + HOSTURL + "</a><br/><p>该链接有效时间为20分钟.</p>";

        SmtpClient sc = new SmtpClient(ConfigurationManager.AppSettings["emailSmtp"]);
        sc.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["systemEmail"], ConfigurationManager.AppSettings["emailPassword"]);
        sc.Send(msg);

        return returnCode;
    }

    /// <summary>
    /// 密码重设
    /// </summary>
    /// <param name="NewPasword">新密码明文</param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public bool PasswordReset(String USERNAME,String NewPasword, String KEY)
    {
        //if (psh == null || psh.IsValid(Session["pswResetCode"].ToString())) { return false; }
        if (!VCode.PSW_Code_Verification(USERNAME,KEY))
            return false;

        using (MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider())
        {
            NewPasword = Convert.ToBase64String(MD5.ComputeHash(Encoding.UTF8.GetBytes(NewPasword)));
        }
        using (DBSelect DBS = new DBSelect())
        {
            DataSet DS = DBS.SelectTheUser(USERNAME);
            int UID = Convert.ToInt32(DS.Tables[0].Rows[0]["u_id"].ToString());
            DBUpdate.UpdateUserInfo(UID, NewPasword, null, null, null);
            return true;
        }
    }
    #endregion

    #region 搜索 OK
    /// <summary>
    /// 用户搜索
    /// </summary>
    /// <param name="USERNAME"></param>
    /// <param name="PAGE"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string Search(String USERNAME, int PAGE)
    {
        int PAGECOUNT = 0;
        XmlDocument XML = ToXML.GetXml();
        XmlElement ROOT = XML.DocumentElement;
        using (DBSelect DBS = new DBSelect())
        {
            DataSet ds = DBS.SelectUser(USERNAME, PAGE, ref PAGECOUNT);

            XmlElement SearchElement = XML.CreateElement("Search");
            XmlElement PageCount = XML.CreateElement("SearchPageCount");
            PageCount.InnerText = Convert.ToString(PAGECOUNT);
            ROOT.AppendChild(PageCount);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                XmlElement USER = XML.CreateElement("User");
                XmlElement EuserID = XML.CreateElement("UID");
                XmlElement EuserName = XML.CreateElement("USERNAME");
                XmlElement Eemail = XML.CreateElement("EMAIL");
                XmlElement EPIC = XML.CreateElement("PIC");

                EuserID.InnerText = dr["u_id"].ToString();
                EuserName.InnerText = dr["u_userName"].ToString();
                Eemail.InnerText = dr["u_Email"].ToString();
                string path = dr["p_path"] == null ? "" : FileIO.GetPicUrl(dr["p_path"].ToString());
                //FileIO.FileBase64(FileIO.FileRead(dr["p_path"].ToString()));
                EPIC.InnerText = path;

                USER.AppendChild(EuserID);
                USER.AppendChild(EuserName);
                USER.AppendChild(Eemail);
                USER.AppendChild(EPIC);
                SearchElement.AppendChild(USER);
            }
            ROOT.AppendChild(SearchElement);
        }
        return XML.OuterXml;
    }

    /// <summary>
    /// 图片搜索
    /// </summary>
    /// <param name="PICKEYWORD"></param>
    /// <param name="PAGE"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string SearchPic(String PICKEYWORD, int PAGE)
    {
        XmlDocument XML = ToXML.GetXml();
        XmlElement ROOT = XML.DocumentElement;

        using (DBSelect DBS = new DBSelect())
        {
            int PAGECOUNT;
            DataSet DS = DBS.SelectPicKeyWord(PICKEYWORD, 2, PAGE, out PAGECOUNT);

            try
            {
                XmlElement SearchPic = XML.CreateElement("SearchPic");
                XmlElement PageCount = XML.CreateElement("SearchPicPageCount");
                PageCount.InnerText = Convert.ToString(PAGECOUNT);
                ROOT.AppendChild(PageCount);

                foreach (DataRow dr in DS.Tables[0].Rows)
                {
                    XmlElement EPIC = XML.CreateElement("SPic");

                    XmlElement EPID = XML.CreateElement("PID");
                    XmlElement EPNAME = XML.CreateElement("PNAME");
                    XmlElement EPTIME = XML.CreateElement("PTIME");
                    XmlElement EPDES = XML.CreateElement("PDES");
                    XmlElement EPDATA = XML.CreateElement("PDATA");

                    EPID.InnerText = dr["up_id"].ToString();
                    EPNAME.InnerText = dr["up_name"].ToString();
                    EPTIME.InnerText = dr["up_uploadTime"].ToString();
                    EPDES.InnerText = dr["up_description"].ToString();
                    EPDATA.InnerText = FileIO.GetPicUrl(dr["p_path"].ToString());//FileIO.FileBase64(FileIO.FileRead(dr["p_path"].ToString()));

                    EPIC.AppendChild(EPID);
                    EPIC.AppendChild(EPNAME);
                    EPIC.AppendChild(EPTIME);
                    EPIC.AppendChild(EPDES);
                    EPIC.AppendChild(EPDATA);

                    SearchPic.AppendChild(EPIC);
                }
                ROOT.AppendChild(SearchPic);
            }
            catch (Exception) {  }

        }
        return XML.OuterXml;
    }
    #endregion

    #region 好友关注 OK
    [WebMethod(EnableSession = true , Description = "关注好友")]
    public int AddFriend(int FID)//XML
    {
        int UID;
        if (LoginYes(out UID))//用户必须登陆
        {
            return DBCreateDelete.InsertRelation(UID, FID);
        }
        return -1;
    }

    [WebMethod(EnableSession = true, Description = "取消关注")]
    public int DelFriend(int[] USERNAME_LIST)//XML
    {
        int UID;
        if (LoginYes(out UID))//用户必须登陆
        {
            int count = 0;
            for (int i = 0; i < USERNAME_LIST.Length; i++)
            {
                count += DBCreateDelete.DeleteRelatin(UID, USERNAME_LIST[i]);
            }
            return count < USERNAME_LIST.Length ? -1 : count;
        } 
        return -1;
    }

    [WebMethod(EnableSession = true, Description = "好友列表")]
    public String FriendsList(int PAGE)
    {
        XmlDocument XML = ToXML.GetXml();
        int UID;
        if (LoginYes(out UID))//用户必须登陆
        {
            XmlElement ROOT = XML.DocumentElement;
            using (DBSelect DBS = new DBSelect())
            {
                try
                {
                    int PAGECOUNT = -1;
                    DataSet ds = DBS.FriendList(UID, PAGE, ref PAGECOUNT);

                    XmlElement FriendList = XML.CreateElement("FriendList");
                    XmlElement PageCount = XML.CreateElement("FriendListPageCount");
                    PageCount.InnerText = Convert.ToString(PAGECOUNT);
                    ROOT.AppendChild(PageCount);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        XmlElement USER = XML.CreateElement("User");
                        XmlElement EuserID = XML.CreateElement("UID");
                        XmlElement EuserName = XML.CreateElement("USERNAME");
                        XmlElement Eemail = XML.CreateElement("EMAIL");
                        XmlElement EPIC = XML.CreateElement("PIC");

                        EuserID.InnerText = dr["r_friend"].ToString();
                        EuserName.InnerText = dr["u_userName"].ToString();
                        Eemail.InnerText = dr["u_Email"].ToString();
                        string path = dr["p_path"] == null ? "" : FileIO.GetPicUrl(dr["p_path"].ToString());// FileIO.FileBase64(FileIO.FileRead(dr["p_path"].ToString()));
                        EPIC.InnerText = path;

                        USER.AppendChild(EuserID);
                        USER.AppendChild(EuserName);
                        USER.AppendChild(Eemail);
                        USER.AppendChild(EPIC);

                        FriendList.AppendChild(USER);
                    }
                    ROOT.AppendChild(FriendList);
                }
                catch (Exception) { ROOT.InnerText = ""; }
            }
        }
        return XML.OuterXml;
    }

    #endregion

    #region 回复 OK*（回复列表）
    [WebMethod(EnableSession = true, Description = "回复")]
    public int EvaPic(int PICID, String CONTENT)
    {
        int UID;
        if (LoginYes(out UID))//用户必须登陆
        {
            return DBCreateDelete.InsertEvaluation(UID, PICID, CONTENT);
        }
        return -1;
    }

    [WebMethod(EnableSession = true, Description = "取得图片回复列表")]
    public String GetEvaPic(int PICID, int PAGE)
    {
        /*未确认图片是否可视*/
        XmlDocument XML = ToXML.GetXml();
        XmlElement ROOT = XML.DocumentElement;
        using (DBSelect DBS = new DBSelect())
        {
            int PAGECOUNT = 0;
            DataSet ds = DBS.SelectEvaluation(PICID, PAGE, ref PAGECOUNT);
            if (ds == null)
                return XML.OuterXml;

            XmlElement Evaluation = XML.CreateElement("Evaluations");
            XmlElement PageCount = XML.CreateElement("EvaluationCount");
            PageCount.InnerText = Convert.ToString(PAGECOUNT);
            ROOT.AppendChild(PageCount);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                XmlElement _Evaluation = XML.CreateElement("Evaluation");

                XmlElement EuserID = XML.CreateElement("UID");
                XmlElement EuserName = XML.CreateElement("USERNAME");
                XmlElement Etime = XML.CreateElement("ETIME");
                XmlElement __Eevaluation = XML.CreateElement("EVALUATION");
                XmlElement EPIC = XML.CreateElement("PIC");

                EuserID.InnerText = dr["e_uid"].ToString();
                EuserName.InnerText = dr["u_userName"].ToString();
                Etime.InnerText = dr["e_time"].ToString();
                __Eevaluation.InnerText = dr["e_evaluation"].ToString();
                string path = dr["p_path"] == null ? "" : FileIO.GetPicUrl(dr["p_path"].ToString());// FileIO.FileBase64(FileIO.FileRead(dr["p_path"].ToString()));
                EPIC.InnerText = path;

                _Evaluation.AppendChild(EuserID);
                _Evaluation.AppendChild(EuserName);
                _Evaluation.AppendChild(Etime);
                _Evaluation.AppendChild(__Eevaluation);
                _Evaluation.AppendChild(EPIC);

                Evaluation.AppendChild(_Evaluation);
            }
            ROOT.AppendChild(Evaluation);
        }
        return XML.OuterXml;
    }
    #endregion

    #region 修改专辑信息
    [WebMethod(EnableSession = true, Description = "创建专辑")]
    public int CreateAblum(String PIC_NAME)
    {
        int UID;
        if (LoginYes(out UID))//用户必须登陆
        {
            return DBCreateDelete.CreateAblum(PIC_NAME, UID);
        }
        return -2;
    }

    [WebMethod(EnableSession = true, Description = "删除专辑")]
    public int DeleteAblum(String PIC_NAME)
    {
        int UID;
        if (LoginYes(out UID))//用户必须登陆
        {
            return DBCreateDelete.DeleteAblum(PIC_NAME, UID);
        }
        return -2;
    }

    [WebMethod(EnableSession = true, Description = "修改专辑的信息")]
    public int UpdateAblum(int ABLUMID, String NEW_NAME)
    {
        int UID;
        if (LoginYes(out UID))//用户必须登陆
        {
            return DBUpdate.UpdateAblum(UID, ABLUMID, NEW_NAME);
        }
        return -1;
    }

    /// <summary>
    /// 图片集列表
    /// </summary>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public String MyAlbumList()
    {
        XmlDocument XML = ToXML.GetXml();
        XmlElement ROOT = XML.DocumentElement;
        using (DBSelect DBS = new DBSelect())
        {
            try
            {
                int UID = Convert.ToInt32(Session["u_id"].ToString());
                DataSet ds = DBS.SelectAlbumList(UID);
                XmlElement Albums = XML.CreateElement("MyAlbumList");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    XmlElement EABLUM = XML.CreateElement("Album");
                    XmlElement EAID = XML.CreateElement("AID");
                    XmlElement EANAME = XML.CreateElement("ANAME");
                    EAID.InnerText = dr["a_id"].ToString();
                    EANAME.InnerText = dr["a_name"].ToString();
                    EABLUM.AppendChild(EAID);
                    EABLUM.AppendChild(EANAME);
                    Albums.AppendChild(EABLUM);
                }
                ROOT.AppendChild(Albums);
            }
            catch (Exception) { ROOT.InnerText = ""; }
        }
        return XML.OuterXml;
    }


    [WebMethod(EnableSession = true
        , Description = "我的专辑列表")]
    public String MyAblumList(int PAGE)
    {
        XmlDocument XML = ToXML.GetXml();
        XmlElement ROOT = XML.DocumentElement;
        using (DBSelect DBS = new DBSelect())
        {
            try
            {
                int UID = Convert.ToInt32(Session["u_id"].ToString());
                int PAGECOUNT = -1;
                DataSet ds = DBS.SelectAblumList(UID, PAGE, ref PAGECOUNT);

                XmlElement MyAblums = XML.CreateElement("MyAblumList");
                XmlElement PageCount = XML.CreateElement("AblumCount");
                PageCount.InnerText = Convert.ToString(PAGECOUNT);
                ROOT.AppendChild(PageCount);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    XmlElement EABLUM = XML.CreateElement("Ablum");

                    XmlElement EAID = XML.CreateElement("AID");
                    XmlElement EANAME = XML.CreateElement("ANAME");
                    XmlElement EATIME = XML.CreateElement("ATIME");
                    XmlElement EEVACOUNT = XML.CreateElement("AECOUNT");

                    EAID.InnerText = dr["a_id"].ToString();
                    EANAME.InnerText = dr["a_name"].ToString();
                    EATIME.InnerText = dr["a_createTime"].ToString();
                    EEVACOUNT.InnerText = dr["a_count"].ToString();

                    EABLUM.AppendChild(EAID);
                    EABLUM.AppendChild(EANAME);
                    EABLUM.AppendChild(EATIME);
                    EABLUM.AppendChild(EEVACOUNT);

                    MyAblums.AppendChild(EABLUM);
                }
                ROOT.AppendChild(MyAblums);

            }
            catch (Exception) { ROOT.InnerText = ""; }
        }
        return XML.OuterXml;
    }

    #endregion

    #region 图片信息 OK*

    //[WebMethod(EnableSession = true
    //    , Description = "上传图片")]
    //public int UploadPic(byte[] IMAGE, String PIC_NAME, int FROM_ABLUM, string EXNAME)
    //{
    //    if (Session["u_id"] != null)
    //    {
    //        String FILEBASE64 = FileIO.FileMD5OfBase64(IMAGE);
    //        int UID = Convert.ToInt32(Session["u_id"].ToString());
    //        String PATH = "";
    //        FileIO.FileSave(IMAGE, FILEBASE64 + EXNAME, ref PATH);
    //        return DBCreateDelete.InsertPicture(PIC_NAME, FROM_ABLUM, FILEBASE64, PATH);
    //    }
    //    return -1;
    //}

    /// <summary>
    /// 批量图片上图
    /// </summary>
    /// <param name="IMAGE">图片字节</param>
    /// <param name="PIC_NAME">图片名</param>
    /// <param name="FROM_ALBUM">相册ID</param>
    /// <param name="EXNAME">图片扩展名</param>
    /// <returns>添加图片数量</returns>
    [WebMethod(EnableSession = true, Description = "批量图片上传")]
    public int UploadPic_2(List<byte[]> IMAGE, String[] PIC_NAME, int FROM_ALBUM, String[] EXNAME, int POWER)
    {
        if (IMAGE.Count != PIC_NAME.Length || PIC_NAME.Length != EXNAME.Length)
        {
            return -1;
        }
        int UID;
        if (LoginYes(out UID))//用户必须登陆
        {
            String[] PATH = new String[IMAGE.Count];
            String[] FILEBASE64 = new String[IMAGE.Count];
            for (int i = 0; i < IMAGE.Count; i++)
            {
                FILEBASE64[i] = FileIO.FileMD5OfBase64(IMAGE[i]);
                PATH[i] = "";
                FileIO.FileSave(IMAGE[i], FILEBASE64[i] + EXNAME[i], ref PATH[i]);
            }
            using (DBCreateDelete DBC = new DBCreateDelete ()){
                return DBC.InsertPicture2(PIC_NAME, FROM_ALBUM, FILEBASE64, PATH, POWER);
            }
        }
        return -1;
    }


    /// <summary>
    /// 删除图片 单张
    /// </summary>
    /// <param name="PICID"></param>
    /// <returns></returns>
    //[WebMethod(EnableSession = true)]
    //public int RemovePic(int PICID)
    //{
    //    try
    //    {
    //        int UID = Convert.ToInt32(Session["u_id"].ToString());
    //        if (!DBSelect.PicIsExist(PICID, UID)) { return -1; }
    //        return DBCreateDelete.DeleteMyPic(PICID);
    //    }
    //    catch (Exception)
    //    { return -1; }
    //}

    /// <summary>
    /// 删除图片 多张
    /// </summary>
    /// <param name="PID"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public int RemovePics(int[] PICID)
    {
        int UID;
        if (LoginYes(out UID))//用户必须登陆
        {
            try
            {
                return DBCreateDelete.DeleteMyPics(PICID);
            }
            catch (Exception) { }
        }
        return 0;
    }

    /// <summary>
    /// 获取共享图片信息
    /// </summary>
    /// <param name="PID"></param>
    /// <returns></returns>
    [WebMethod]
    public string GetPicInfo(int PID)
    {
        XmlDocument XML = ToXML.GetXml();
        XmlElement ROOT = XML.DocumentElement;
        using (DBSelect DS = new DBSelect())
        {
            DataSet ds = DS.SelectSharePic(PID);
            CreatePicNode(ref XML, ref ds, ref ROOT);
            ds.Dispose();
        }
        return XML.OuterXml;
    }

    /// <summary>
    /// 获取用户分享图片信息
    /// </summary>
    /// <param name="PID"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public String GetUserSharePicInfo(int PID)
    {
        XmlDocument XML = ToXML.GetXml();
        int UID;
        if (LoginYes(out UID))//用户必须登陆
        {
            XmlElement ROOT = XML.DocumentElement;
            using (DBSelect DS = new DBSelect())
            {
                DataSet ds = DS.SelectUserSharePic(UID, PID);
                CreatePicNode(ref XML, ref ds, ref ROOT);
                if (ds != null) ds.Dispose();
            }

        }
        return XML.OuterXml;
    }

    /// <summary>
    /// 获取我的图片信息
    /// </summary>
    /// <param name="PID"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public String GetMyPicInfo(int PID)
    {
        XmlDocument XML = ToXML.GetXml();
        int UID;
        if (LoginYes(out UID))//用户必须登陆
        {
            using (DBSelect DS = new DBSelect())
            {
                DataSet ds = DS.SelectMyPic(UID,PID);
                XmlElement ROOT = XML.DocumentElement;
                CreatePicNode(ref XML,ref ds,ref ROOT);
                if (ds != null) ds.Dispose();
            }
        }
        return XML.OuterXml;
    }

    /// <summary>
    ///  修改图片信息
    /// </summary>
    /// <param name="PICID"></param>
    /// <param name="PIC_NAME"></param>
    /// <param name="FROM_ABLUM"></param>
    /// <param name="CONTENT"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true
        , Description = "修改图片信息16,***隐患***,FROM_ABLUM不修改为-1，其他不修改为null")]
    public int UpdatePicInfo(int PICID, String PIC_NAME, int FROM_ABLUM, String CONTENT)//PICID->MD5
    {
        /*未确认修改图片为自己图片*/
        int UID;
        if (LoginYes(out UID))
        {
            PIC_NAME = PIC_NAME == "" ? null : PIC_NAME;
            CONTENT = CONTENT == "" ? null : CONTENT;
            int count = 0;
            //if (FROM_ABLUM != -1) { count += DBUpdate.UpdateUPictureAblum(UID, PICID, FROM_ABLUM); }
            if (PIC_NAME != null || CONTENT != null) { count += DBUpdate.UpdateUPicture(PICID, PIC_NAME, CONTENT); }
            return count;
        }
        return -1;
    }

    /// <summary>
    /// 批量修改图片所属图片集
    /// </summary>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string UpdatePicAlbum(int[] PID,int AID)
    {
        string ReStr = "";
        int UID;
        if (LoginYes(out UID))
        {
            int[] PIDS = DBUpdate.UpdatePicAID(PID, AID);
            for (int i = 0; i < PIDS.Length; i++)
            {
                ReStr += PIDS[i].ToString() + ",";
            }
            if (ReStr != "") ReStr.Substring(0, ReStr.Length - 1);
        }
        return ReStr;
    }

    /// <summary>
    /// 修改图片权限 多张同个权限
    /// </summary>
    /// <param name="PID"></param>
    /// <param name="AID"></param>
    /// <param name="POWER"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string UpdatePicPower(int[] PID,int AID, int POWER)
    {
        string ReStr = "";
        int UID;
        if (LoginYes(out UID))
        {
            DBUpdate DBU = new DBUpdate();
            int[] PIDS = DBU.UpdatePicPower(PID, AID, POWER);
            for (int i = 0; i < PIDS.Length; i++)
            {
                ReStr += PIDS[i].ToString() + ",";
            }
            if (ReStr != "") ReStr.Substring(0, ReStr.Length - 1);
        }
        return ReStr;
    }

    #endregion

    #region 图片列表 OK

    /// <summary>
    /// 获取共享图片列表
    /// </summary>
    /// <param name="PAGE"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string GetSharePic(int PAGE)
    {
        XmlDocument XML = ToXML.GetXml();
        XmlElement ROOT = XML.DocumentElement;
        using (DBSelect DS = new DBSelect())
        {
            try
            {
                int PAGECOUNT = -1;
                DataSet ds = DS.SelectShare(PAGE, ref PAGECOUNT);

                XmlElement Share = XML.CreateElement("Share");
                XmlElement PageCount = XML.CreateElement("SharePageCount");
                CreatePicList(ref XML, ds, ref  ROOT, Share, PageCount, PAGECOUNT);
                #region ×
                //PageCount.InnerText = Convert.ToString(PAGECOUNT);
                //ROOT.AppendChild(PageCount);

                //foreach (DataRow dr in ds.Tables[0].Rows)
                //{
                //    XmlElement EPIC = XML.CreateElement("UPic");

                //    XmlElement EPID = XML.CreateElement("PID");
                //    XmlElement EPNAME = XML.CreateElement("PNAME");
                //    XmlElement EPTIME = XML.CreateElement("PTIME");
                //    XmlElement EPDES = XML.CreateElement("PDES");
                //    XmlElement EPDATA = XML.CreateElement("PDATA");

                //    EPID.InnerText = dr["up_id"].ToString();
                //    EPNAME.InnerText = dr["up_name"].ToString();
                //    EPTIME.InnerText = dr["up_uploadTime"].ToString();
                //    EPDES.InnerText = dr["up_description"].ToString();
                //    EPDATA.InnerText = FileIO.GetPicUrl(dr["p_path"].ToString());//FileIO.FileBase64(FileIO.FileRead(dr["p_path"].ToString()));

                //    EPIC.AppendChild(EPID);
                //    EPIC.AppendChild(EPNAME);
                //    EPIC.AppendChild(EPTIME);
                //    EPIC.AppendChild(EPDES);
                //    EPIC.AppendChild(EPDATA);

                //    UserShare.AppendChild(EPIC);
                //}
                //ROOT.AppendChild(UserShare);
                #endregion
            }
            catch (Exception) { ROOT.InnerText = ""; }
        }
        return XML.OuterXml;
    }

    /// <summary>
    /// 获得好友分享图片全部
    /// </summary>
    /// <param name="PAGE"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string GetUserShare(int PAGE)
    {
        XmlDocument XML = ToXML.GetXml();
        int UID;
        if (LoginYes(out UID))
        {
            XmlElement ROOT = XML.DocumentElement;
            using (DBSelect DS = new DBSelect())
            {
                try
                {
                    int PAGECOUNT = -1;
                    DataSet ds = DS.SelectFriendShare(UID, PAGE, ref PAGECOUNT);

                    XmlElement UserShare = XML.CreateElement("UserShare");
                    XmlElement PageCount = XML.CreateElement("UserSharePageCount");
                    CreatePicList(ref XML, ds, ref  ROOT, UserShare, PageCount, PAGECOUNT);
                    #region ×
                    //PageCount.InnerText = Convert.ToString(PAGECOUNT);
                    //ROOT.AppendChild(PageCount);

                    //foreach (DataRow dr in ds.Tables[0].Rows)
                    //{
                    //    XmlElement EPIC = XML.CreateElement("UPic");

                    //    XmlElement EPID = XML.CreateElement("PID");
                    //    XmlElement EPNAME = XML.CreateElement("PNAME");
                    //    XmlElement EPTIME = XML.CreateElement("PTIME");
                    //    XmlElement EPDES = XML.CreateElement("PDES");
                    //    XmlElement EPDATA = XML.CreateElement("PDATA");
                    //    try
                    //    {
                    //        EPID.InnerText = dr["up_id"].ToString();
                    //        EPNAME.InnerText = dr["up_name"].ToString();
                    //        EPTIME.InnerText = dr["up_uploadTime"].ToString();
                    //        EPDES.InnerText = dr["up_description"].ToString();
                    //        EPDATA.InnerText = FileIO.GetPicUrl(dr["p_path"].ToString());//FileIO.FileBase64(FileIO.FileRead(dr["p_path"].ToString()));
                    //    }
                    //    catch (Exception) { continue; }
                    //    EPIC.AppendChild(EPID);
                    //    EPIC.AppendChild(EPNAME);
                    //    EPIC.AppendChild(EPTIME);
                    //    EPIC.AppendChild(EPDES);
                    //    EPIC.AppendChild(EPDATA);

                    //    UserShare.AppendChild(EPIC);
                    //}
                    //ROOT.AppendChild(UserShare);
                    #endregion
                }
                catch (Exception)
                {
                    ROOT.InnerText = "";
                }
            }
        }
        return XML.OuterXml;
    }

    /// <summary>
    ///  获取本人一个专辑的图片列表
    /// </summary>
    /// <param name="PAGE"></param>
    /// <param name="ABLUM"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string GetUserPicList(int PAGE, int ABLUM)
    {
        XmlDocument XML = ToXML.GetXml();
        int UID;
        if (LoginYes(out UID))
        {
            XmlElement ROOT = XML.DocumentElement;
            using (DBSelect DBS = new DBSelect())
            {
                try
                {
                    int PAGECOUNT = -1;
                    DataSet ds = DBS.SelectAblumPics(UID, ABLUM, PAGE, ref PAGECOUNT);

                    XmlElement AblumPics = XML.CreateElement("AblumPicList");
                    XmlElement PageCount = XML.CreateElement("AblumPicCount");
                    CreatePicList(ref XML, ds, ref  ROOT, AblumPics, PageCount, PAGECOUNT);
                }
                catch (Exception)
                {
                    ROOT.InnerText = "";
                }
            }
        }
        return XML.OuterXml;
    }

    #endregion

    #region 状态判断
    [WebMethod(EnableSession = true
        , Description = "我是否在线?")]
    public bool IsOnline(string USERNAME)
    {
        if (Session["userName"] == null) return false;
        if (!Session["userName"].ToString().Equals(USERNAME)) return false;
        return true;
    }
    #endregion

    #region 标签

    /// <summary>
    /// 给图片添加标签
    /// </summary>
    /// <param name="PID"></param>
    /// <param name="TAG"></param>
    /// <returns>TID列表</returns>
    [WebMethod(EnableSession = true)]
    public String AddTag(int PID, String[] TAG)
    {
        XmlDocument XML = ToXML.GetXml();
        XmlElement ROOT = XML.DocumentElement;

        XmlElement TagAddList = XML.CreateElement("TagAddList");
        using (DBCreateDelete DBC = new DBCreateDelete())
        {
            for (int i = 0; i < TAG.Length; i++)
            {
                int TID = DBC.InsertPicTag(PID, TAG[i]);
                if (TID != 0)
                {
                    XmlElement ETAG = XML.CreateElement("TAG");
                    XmlElement ETID = XML.CreateElement("TID");
                    XmlElement ETNAME = XML.CreateElement("TNAME");

                    ETID.InnerText = TID.ToString();
                    ETNAME.InnerText = TAG[i];

                    ETAG.AppendChild(ETID);
                    ETAG.AppendChild(ETNAME);

                    TagAddList.AppendChild(ETAG);
                }
            }
        }
        ROOT.AppendChild(TagAddList);
        return XML.OuterXml;
    }

    /// <summary>
    /// 删除图片标签
    /// </summary>
    /// <param name="PID"></param>
    /// <param name="TID"></param>
    /// <returns>TID列表</returns>
    [WebMethod(EnableSession = true)]
    public String DelTag(int PID, int[] TID)
    {
        string str = "";
        using (DBCreateDelete DBD = new DBCreateDelete())
        {
            for (int i = 0; i < TID.Length; i++)
            {
                int D_index = DBD.DeletePicTag(PID, TID[i]);
                if (D_index != 0)
                {
                    str += TID[i] + ",";
                }
            }
        }
        str = str.Substring(0, str.Length - 1);
        return str;
    }

    /// <summary>
    /// 获得指定图片的标签列表
    /// </summary>
    /// <param name="PID"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public String TagList(int PID)
    {
        XmlDocument XML = ToXML.GetXml();
        XmlElement ROOT = XML.DocumentElement;

        XmlElement TagList = XML.CreateElement("TagList");
        using (DBSelect DBS = new DBSelect())
        {
            DataSet DS = DBS.SelectPicTag(PID);
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                XmlElement ETAG = XML.CreateElement("TAG");
                XmlElement ETID = XML.CreateElement("TID");
                XmlElement ETNAME = XML.CreateElement("TNAME");

                ETID.InnerText = dr["t_id"].ToString();
                ETNAME.InnerText = dr["t_name"].ToString();

                ETAG.AppendChild(ETID);
                ETAG.AppendChild(ETNAME);

                TagList.AppendChild(ETAG);
            }
        }
        ROOT.AppendChild(TagList);
        return XML.OuterXml;
    }
    #endregion

    #region ×未使用

    #endregion

    #region Else
    /// <summary>
    /// 登陆判断
    /// </summary>
    /// <param name="UID"></param>
    /// <returns></returns>
    private bool LoginYes(out int UID)
    {
        if (Session["u_id"] != null)
        {
            UID = Convert.ToInt32(Session["u_id"].ToString());
            return true;
        }
        UID = -1;
        return false;
    }

    /// <summary>
    /// 给出DataSet数据生成XML数据,单张图片信息
    /// </summary>
    /// <param name="XML"></param>
    /// <param name="ds"></param>
    /// <param name="ROOT"></param>
    private void CreatePicNode(ref XmlDocument XML, ref DataSet ds, ref XmlElement ROOT)
    {
        XmlElement ThePic = XML.CreateElement("ThePicture");
        try
        {
            XmlElement EPID = XML.CreateElement("PID");
            XmlElement EUSERNAME = XML.CreateElement("USERNAME");
            XmlElement ENAME = XML.CreateElement("PNAME");
            XmlElement ETIME = XML.CreateElement("PTIME");
            XmlElement EDES = XML.CreateElement("PDES");
            XmlElement EPDATA = XML.CreateElement("PDATA");
            XmlElement EAUTHORPIC = XML.CreateElement("AUTHORPIC");

            XmlElement EPHEIGHT = XML.CreateElement("PHEIGHT");
            XmlElement EPWIDTH = XML.CreateElement("PWIDTH");
            XmlElement EPSPACE = XML.CreateElement("PSPACE");
            XmlElement EPEXNAME = XML.CreateElement("PEXNAME");

            EPID.InnerText = ds.Tables[0].Rows[0]["up_id"].ToString();
            EUSERNAME.InnerText = ds.Tables[0].Rows[0]["u_userName"].ToString();
            ENAME.InnerText = ds.Tables[0].Rows[0]["up_name"].ToString();
            ETIME.InnerText = ds.Tables[0].Rows[0]["up_uploadTime"].ToString();
            EDES.InnerText = ds.Tables[0].Rows[0]["up_description"].ToString();

            string path = ds.Tables[0].Rows[0]["p_path"].ToString();
            EPDATA.InnerText = FileIO.GetPicUrl(path);//FileIO.FileBase64(FileIO.FileRead(path));
            try
            {
                EAUTHORPIC.InnerText = FileIO.GetPicUrl(ds.Tables[0].Rows[0]["u_p_path"].ToString());//FileIO.FileBase64(FileIO.FileRead(ds.Tables[0].Rows[0]["u_p_path"].ToString()));
            }
            catch (Exception) { }

            int height = -1;
            int width = -1;
            double size = FileIO.GetPicSize(path, ref height, ref width);
            EPHEIGHT.InnerText = Convert.ToString(height);
            EPWIDTH.InnerText = Convert.ToString(width);
            size /= 1024;
            EPSPACE.InnerText = String.Format("{0:F}", size);
            EPEXNAME.InnerText = path.Substring(path.IndexOf('.') + 1);

            ThePic.AppendChild(EPID);
            ThePic.AppendChild(EUSERNAME);
            ThePic.AppendChild(ENAME);
            ThePic.AppendChild(ETIME);
            ThePic.AppendChild(EDES);
            ThePic.AppendChild(EPDATA);
            ThePic.AppendChild(EAUTHORPIC);

            ThePic.AppendChild(EPHEIGHT);
            ThePic.AppendChild(EPWIDTH);
            ThePic.AppendChild(EPSPACE);
            ThePic.AppendChild(EPEXNAME);

            ROOT.AppendChild(ThePic);
        }
        catch (Exception)
        {
            ROOT.InnerText = "";
        }
    }

    /// <summary>
    /// 根据DataSet数据构建图片列表信息
    /// </summary>
    /// <param name="XML"></param>
    /// <param name="ds"></param>
    /// <param name="ROOT"></param>
    /// <param name="ListNode"></param>
    /// <param name="PageCountNode"></param>
    /// <param name="PAGECOUNT"></param>
    private void CreatePicList(ref XmlDocument XML, DataSet ds, ref XmlElement ROOT, XmlElement ListNode, XmlElement PageCountNode, int PAGECOUNT)
    {
        try
        {
            PageCountNode.InnerText = Convert.ToString(PAGECOUNT);
            ROOT.AppendChild(PageCountNode);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                XmlElement EPIC = XML.CreateElement("Pic");

                XmlElement EPID = XML.CreateElement("PID");
                XmlElement EPNAME = XML.CreateElement("PNAME");
                XmlElement EPTIME = XML.CreateElement("PTIME");
                XmlElement EPDES = XML.CreateElement("PDES");
                XmlElement EPDATA = XML.CreateElement("PDATA");
                XmlElement EPOWER = XML.CreateElement("PPOWER");

                try
                {
                    EPID.InnerText = dr["up_id"].ToString();
                    EPNAME.InnerText = dr["up_name"].ToString();
                    EPTIME.InnerText = dr["up_uploadTime"].ToString();
                    EPDES.InnerText = dr["up_description"].ToString();
                    EPOWER.InnerText = dr["up_power"].ToString();
                    EPDATA.InnerText = FileIO.GetPicUrl(dr["p_path"].ToString());//FileIO.FileBase64(FileIO.FileRead(dr["p_path"].ToString()));
                }
                catch (Exception)
                {
                    continue;
                }
                EPIC.AppendChild(EPID);
                EPIC.AppendChild(EPNAME);
                EPIC.AppendChild(EPTIME);
                EPIC.AppendChild(EPDES);
                EPIC.AppendChild(EPOWER);
                EPIC.AppendChild(EPDATA);

                ListNode.AppendChild(EPIC);
            }
            ROOT.AppendChild(ListNode);
        }
        catch (Exception)
        {
            ROOT.InnerText = "";
        }
    }

    #endregion
}

//注册前必须获得代码
public class RegisterSoapHeader : SoapHeader
{
    public String R_Code;
    public bool IsValid(String SessionCode)
    {
        if (!SessionCode.Equals(R_Code))
            return false;
        return true;
    }
}

//更新用户信息包含的密码验证
public class UpdateSoapHeader : SoapHeader
{
    public String password;
    public bool IsValid(String SessionMd5Password)
    {
        using (MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider())
        {
            password = Convert.ToBase64String(MD5.ComputeHash(Encoding.UTF8.GetBytes(password)));
            if (SessionMd5Password.Equals(password)) { return true; }
            return false;
        }
    }
}

public class ResetSoapHeader : SoapHeader 
{
    public String Code;
    public bool IsValid(String C)
    {
        if (Code.Equals(C))
            return true;
        return false;
    }
}


