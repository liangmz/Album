using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;

public partial class HD : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userName"] != null)
        {
            if (Session["cookie"] == null) { userName.InnerText = ""; return; }
            localhost.AblumService AS = new localhost.AblumService();
            AS.CookieContainer = (CookieContainer)Session["cookie"];
            if (AS.IsOnline(Session["userName"].ToString()))
            {
                userName.InnerHtml = Session["userName"].ToString() + " <b class=\"caret\"></b>";
            }
            else { userName.InnerText = ""; return; }
        }
    }
}
