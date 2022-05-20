using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage2 : System.Web.UI.MasterPage
{
    string strMsg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Keys.Count == 0 || Session["UserID"] == null || Session["UserID"].ToString() == "")
        {
            HttpContext.Current.Session.Clear();
            Server.Transfer("IBStart.aspx");

        }
        if (this.IsPostBack)
            return;

        lblusername.Text = Session["UserID"].ToString();
        lblfullname.Text = Session["Fullname"].ToString();
        //lblprofile.Text = Session["Profile"].ToString();
        lbldate.Text = Session["LastLoggin"].ToString();

    }
    protected void getRights()
    {

    }
    protected override void OnInit(EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetNoStore();
        Response.Cache.SetExpires(DateTime.MinValue);

        base.OnInit(e);
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        //Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //Response.Cache.SetNoStore();

        //EFTLogs.SaveLog savelog = new EFTLogs.SaveLog();
        //savelog.Activity = "Logged out.";
        //if (savelog.SaveUserLog(ref strMsg) == false)
        //{

        //}
        //savelog = null;
        Session.Clear();
        Response.Redirect("IBStart.aspx");
    }


}
