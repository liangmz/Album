using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ForgetPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    /// <summary>
    /// 邮件输入
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void call_ServerClick(object sender, EventArgs e)
    {
        string Email = Request["Email"].ToString();

        string URL = Request.Url.Scheme + "://" + Request.Url.Authority + "/ResetPassword.aspx";
        localhost.AblumService AS = new localhost.AblumService();
        int reCode = AS.PasswordResetUrl(Email, URL);
        switch (reCode)
        {
            case 1: { ResetMessage.InnerHtml = "邮箱发送成功，请进入邮箱以继续密码重设."; } break;
            case -1: { ResetMessage.InnerHtml = "邮箱未验证无法发送."; } break;
            case -2: { ResetMessage.InnerHtml = "该邮箱未注册."; } break;
        }
    }
}