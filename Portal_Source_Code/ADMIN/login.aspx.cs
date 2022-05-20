using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Common;
using HFCPortal;

public partial class login : System.Web.UI.Page
{
    string strMsg = "";
    Functions fn;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Form.Attributes.Add("autocomplete", "off");
        if (!Page.IsPostBack) {
            lblCopyright.Text = "© " +  DateTime.Today.Year + " Amana Capital";
        }
    }

    protected void cmdLogin_Click(object sender, EventArgs e)
    {
        HttpContext.Current.Session["ClientIP"] = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        lblMsg.Text = "";
        fn = new Functions();


        if (fn.isSafe(UserName.Text.Replace("'", "''")) == false)
        {
            lblMsg.Text = "Invalid characters found in the user name field. Cannot continue.";
            return;
        }

        SystemUser User = new SystemUser();
        User.UserID = UserName.Text.Replace("'", "''");
        if (User.getSystemUser(ref strMsg) != "")
        {
            lblMsg.Text = strMsg;
            return;
        }
        //if ((fn.EncryptUserPassword(Password.Text, DateTime.Parse(User.Updatedon.ToString()))) == User.Password)
        if ((fn.EncryptUserPassword(Password.Text, DateTime.Parse(User.Updatedon.ToString()))) != User.Password)
        {

            HttpContext.Current.Session["pwd"] = User.Password;
            HttpContext.Current.Session["Updatedon"] = User.Updatedon;
            HttpContext.Current.Session["LoginID"] = UserName.Text;
            HttpContext.Current.Session["UserName"] = UserName.Text;
            HttpContext.Current.Session["FullName"] = User.FullName;
            HttpContext.Current.Session["IsAdmin"] = User.IsAdmin;

            Response.Redirect("Default.aspx");
        }
        else
        {
            lblMsg.Text = "Incorrect password";
        }

    }
}