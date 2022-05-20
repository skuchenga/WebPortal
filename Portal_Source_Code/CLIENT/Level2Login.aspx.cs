using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Common;
using System.Text;
using System.Data.SqlClient;
using System.Net.Mail;
using HFCPortal;
using HFCPortal.Common;
using System.IO;

public partial class Level2Login : System.Web.UI.Page
{
    string strMsg = "";
    int Loginattempts = 0;
    int SetUpLoginAttempts = 0;
    Functions fn;
    DataAccess DataClass;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Form.Attributes.Add("autocomplete", "off");
        if (Session["Username"] == null || Session["username"].ToString() == "" || Session.Keys.Count == 0)
        {
            Response.Redirect("Ibstart.aspx");
        }
        if (IsPostBack)
            return;
        RadWindowManager1.Windows.Clear();
        txtUserID.Text = Session["username"].ToString();
        Image1.ImageUrl = "Handler2.ashx?UserID=" + Session["username"].ToString() + "";
        txtMessage.Text = Session["Message"].ToString();


    }
    protected void cmdLogin_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        Page.Validate();
        if (!(Page.IsValid))
            return;
        fn = new Functions();

        Session["PlainTextPwd"] = txtPassword.Text;
        Session["ClientIP"] = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

        if (Image1.Visible == true)
        {
            fn = new Functions();
            if (chkConfirmed.Checked == false)
            {
                lblMessage.Text = "";
                fn.MessageLine(this.Page, "Please confirm your seal before login.", System.Drawing.Color.Red, "lblMessageLine");
                return;
            }
            fn = null;
        }

        DataClass = new DataAccess();
        DbDataReader settings = DataClass.GetDBResults(ref strMsg, "sp_getpwsettings");
        if (strMsg != "")
        {
            fn.logError(strMsg);
            lblMessage.Text = "Sorry an error occurred";
            return;
        }
        if (settings.Read())
        {
            SetUpLoginAttempts = System.Convert.ToInt32(settings["MaxAttempts"].ToString());

        }
        settings.Close();
        settings.Dispose();

        //Check if user is deleted
        DbDataReader rs = DataClass.GetDBResults(ref strMsg, "sp_GetUserInfo",
            "@UserID", txtUserID.Text.Replace("'", "''"));
        if (strMsg != "")
        {
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Text = "Sorry an error occurred.";
            fn.logError(strMsg);
            return;
        }

        if (!rs.Read())
        {
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Text = "Invalid account";
            fn.logError(strMsg);
            return;
        }



        fn = new Functions();
        Double paswd = fn.EncryptUserPassword(txtPassword.Text, DateTime.Parse(Session["UpDatedOn"].ToString()));
        if (paswd == Double.Parse(Session["pwd"].ToString()))
        //if (paswd.GetType() == typeof(double))//== Double.Parse(Session["pwd"].ToString()))
        {
            strMsg = "";
            Session["UserID"] = txtUserID.Text;
            DataClass = new DataAccess();
            DbDataReader Reset = DataClass.GetDBResults(ref strMsg, "sp_ResetLoginAttempts",
                "@UserID", txtUserID.Text);
            if (strMsg != "")
            {
                fn = new Functions();
                fn.logError(strMsg);
                lblMessage.Text = "Sorry and error occurred.";
                return;
            }

            DbDataReader ResultSeLog = DataClass.GetDBResults(ref strMsg, "sp_UsersOnline",
                "@UserID", Session["UserID"].ToString(),
                "@DateAndTimeIN", System.DateTime.Now,
                "@dATEANDTIMEOUT", "",
                "@Activity", "Logged in",
                "@Online", "1",
                "@Clientip", Session["ClientIP"].ToString());
            if (strMsg != "")
            {
                fn = new Functions();
                fn.logError(strMsg);
                fn.MessageLine(this.Page, "Sorry and error occurred", System.Drawing.Color.Red, "lblMessage");
                return;
            }
            DataClass = null;
            log();

            if (bool.Parse(Session["Firstloggin"].ToString()) == true)
            {
                Response.Redirect("frmChangePassword.aspx?i=0");
                return;
            }
            if (bool.Parse(Session["allowmails"].ToString()) == true)
            {
                //send mail
                sendemail();

            }

            Response.Redirect("mainmenu.aspx");

        }
        else
        {
            fn.MessageLine(this.Page, "Invalid password.", System.Drawing.Color.BlueViolet, "lblMessage");
            DataClass = new DataAccess();
            DbDataReader rsUserAttempts = DataClass.GetDBResults(ref strMsg, "Sp_UpdateUserAttempts", txtUserID.Text.Replace("'", "''"));
            if (strMsg != "")
            {
                fn.logError(strMsg);
                fn.MessageLine(this.Page, "sorry an error occurred", System.Drawing.Color.Red, "lblMessageLine");
                return;
            }

            rsUserAttempts.Close();
            rsUserAttempts.Dispose();


        }
        if (Loginattempts > SetUpLoginAttempts)
        {
            fn = new Functions();
            Session["Loginattempts"] = Loginattempts.ToString();
            lblMessage.Text = "UserID Is Disabled,Please Contact The Bank.";

            cmdLogin.Enabled = false;
            DbDataReader rsw = DataClass.GetDBResults(ref strMsg, "sp_DisableUser",
              "@UserID", txtUserID.Text.Replace("'", "''"));
            if (strMsg != "")
            {
                fn.logError(strMsg);
                fn.MessageLine(this.Page, "sorry an error occurred", System.Drawing.Color.Red, "lblMessageLine");
                return;
            }

            SendDisable_email();

            return;
        }


    }
    protected void SendDisable_email()
    {
        string eMailMessage;
        string Subject;

        eMailMessage = "Dear Sir/Madam.My investment portal account has been disabled due to Wrong Password Entry," + Session["Loginattempts"].ToString() + " -times.Please Enable My account.My UserID is:" + txtUserID.Text + "..Regards.";
        Subject = "Investment Portal Customer Account Disabled";

        //+++++++Get email address for selected department+++++++++++++++++++++++++++++++++++

        Functions fn = new Functions();
        strMsg = "";
        string Error = fn.SendE_mail(Session["EmailAddr"].ToString(), Subject, eMailMessage);
        if (Error != "")
        {
            lblMessage.Text = "";
        }

        fn = null;
    }

    protected void log()
    {
        strMsg = "";
        fn = new Functions();
        DataClass = new DataAccess();
        DbDataReader rsUpdateLoggedIn = DataClass.GetDBResults(ref strMsg, "sp_updateLoggedIn",
            "@UserID", Session["UserID"].ToString());
        if (strMsg != "")
        {
            fn.MessageLine(this.Page, "sorry an error occurred", System.Drawing.Color.Red, "lblMessageLine");
            return;
        }
        rsUpdateLoggedIn.Close();
        rsUpdateLoggedIn.Dispose();
        DataClass = null;
        fn = null;
    }

    protected void cmdReadMore_Click(object sender, EventArgs e)
    {
        RadWindow rdContent = new RadWindow();
        rdContent.VisibleOnPageLoad = true;

        rdContent.Width = 600;
        rdContent.Height = 310;
        rdContent.Left = 150;
        rdContent.Top = 100;

        rdContent.VisibleStatusbar = true;

        rdContent.NavigateUrl = "SecurityNotes.aspx";
        rdContent.BorderWidth = 10;
        rdContent.Title = "Security Notes";
        RadWindowManager1.Windows.Add(rdContent);
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect("IBstart.aspx");
    }
    protected void sendemail()
    {
        string EmailAddress = Session["Email"].ToString();
        string Subject;

        string EmailBody;

        EmailBody = "<HTML>";
        EmailBody = EmailBody + "<BODY>";
        EmailBody = EmailBody + "<p> Dear Sir/Madam, </p> <P>This is an automated email sent to you, " +
                              "to notify you, that you logged into the Amana Capital investment portal on " + DateTime.Now.DayOfWeek.ToString() + ", ";
        EmailBody = EmailBody + "<br/>";
        EmailBody = EmailBody + DateTime.Now.ToString("MMMM") + ", " + DateTime.Now.Day + ", " + DateTime.Now.Year + " at " + DateTime.Now.ToLongTimeString();

        EmailBody = EmailBody + ": If you did not login at this time, please notify Amana Capital investment portal immediately. " +
                              "For enquires:";
        EmailBody = EmailBody + "<br/>";
        EmailBody = EmailBody + "Call: 020235 1735/020235 1741/42,";
        EmailBody = EmailBody + "<br/>";
        EmailBody = EmailBody + "Email: info@amanacapital.co.ke </p> ";
        EmailBody = EmailBody + "<p>Thank you for using Amana Capital investment portal.";
        EmailBody = EmailBody + "</p>";
        EmailBody = EmailBody + "</BODY>";
        EmailBody = EmailBody + "</HTML>";


        Subject = "Amana Capital Investment Portal.";


        fn = new Functions();
        string status = fn.SendE_mail(Session["Email"].ToString(), Subject, EmailBody);





    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        RadWindow Window = new RadWindow();

        Window.VisibleOnPageLoad = true;
        Window.NavigateUrl = "frmResetPWD.aspx";
        Window.Width = 610;
        Window.Height = 300;
        Window.Left = 300;
        Window.Top = 100;
        Window.VisibleStatusbar = true;
        Window.Title = "Forgot Password";
        RadWindowManager1.Windows.Add(Window);
    }

    protected void Btndownloadtoken_Click(object sender, EventArgs e)
    {
        String docName = "TokenSetup.zip";
        Response.Redirect("Downloads/" + docName + "");
    }
}