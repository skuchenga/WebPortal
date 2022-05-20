using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    string strMsg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Keys.Count == 0 || Session["LoginID"] == null || Session["LoginID"].ToString() == "")
        {
            HttpContext.Current.Session.Clear();
            Server.Transfer("Login.aspx");

        }
        if (this.IsPostBack)
            return;

        lblusername.Text = Session["LoginID"].ToString();
        lblfullname.Text = Session["Fullname"].ToString();
        //lblprofile.Text = Session["Profile"].ToString();
        lbldate.Text = DateTime.Now.ToString();

    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect("login.aspx");


    }


}
