using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Email : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string userName = Request.QueryString["userName"];
            string key = Request["key"].ToString();

            /*
             * 加号Request[""]读取为空格
             * string key = Request.Url.Query;
            key = key.Substring(key.IndexOf('&'));
            key = key.Substring(key.IndexOf('=') + 1);
             */

            localhost.AblumService AS = new localhost.AblumService();
            if (AS.ValidateEmail_PutCode(userName, key))
            {
                what.InnerHtml = "验证成功.";
            }
            else {
                what.InnerHtml = "验证错误";
            }
        }
        catch (Exception)
        {
            what.InnerHtml = "验证错误";
        }
    }
}