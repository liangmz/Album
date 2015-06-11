using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PasswordReset : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["un"] != null)//重设用户名
        {
            UserName.Value = Request["un"].ToString();
        }
        else
        {
            Response.Redirect("Default.aspx");
        }
    }

    /// <summary>
    /// 重设
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ResetPsw_ServerClick(object sender, EventArgs e)
    {
        string userName = UserName.Value;
        string password = Request["Password"].ToString();

        string key = Request["key"].ToString();
        
        localhost.AblumService AS = new localhost.AblumService();
        if (AS.PasswordReset(userName, password, key))
        {
            ResetReportMessage.InnerHtml = "密码重置成功，请重新登陆!";
        }
        else
        {
            ResetReportMessage.InnerHtml = "验证码链接错误!";
        }
    }
}