using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HFCPortal;
using HFCPortal.Common;

public partial class frmSystemSettings : System.Web.UI.Page
{
    Functions fn;
    DataAccess DataClass;
    string strMsg = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Keys.Count == 0 || Session["LoginID"] == null || Session["LoginID"].ToString() == "")
        {
                HttpContext.Current.Session.Clear();
            Response.Redirect("Login.aspx");

        }
        if (IsPostBack)
            return;
        lblmsg.ForeColor = System.Drawing.Color.PaleVioletRed;
        this.Form.Attributes.Add("autocomplete", "off");
        getrights();
        syssettings();
    }
    protected void getrights()
    {
        lblmsg.Text = "";
        DataClass = new DataAccess();
        fn = new Functions();
        DbDataReader rs = DataClass.GetDBResults(ref strMsg, "sp_MTgetModules", HttpContext.Current.Session["LoginID"].ToString());
        if (strMsg != "")
        {
            fn.logError(strMsg);
            fn.MessageLine(this.Page, "Sorry an error occurred", System.Drawing.Color.Red, "lblMessageLine");
            return;
        }
        while (rs.Read())
        {
            if (rs["ModuleID"].ToString() == "1005")
            {
                if (Boolean.Parse(rs["Checked"].ToString()) == false)
                {
                    Response.Redirect("Norights.aspx");
                }
            }
        }

    }

    protected SqlConnection connection() {
  

        SqlConnection conn = new SqlConnection(CommonHelper.GetConnectionString());

        return conn;
    }

    private void syssettings()
    {
        Functions fn;
        SqlConnection conn = connection();

        SqlDataAdapter dAd = new SqlDataAdapter("sp_getSystem", conn);
        dAd.SelectCommand.CommandType = CommandType.StoredProcedure;

        DataSet dSet = new DataSet();
        try
        {
            dAd.Fill(dSet, "mailparams");
            txtusername.Text = dSet.Tables[0].Rows[0]["sendusername"].ToString();
            txtpassword.Text = dSet.Tables[0].Rows[0]["sendpassword"].ToString();
            txthost.Text = dSet.Tables[0].Rows[0]["SmtpServer"].ToString();
            txtport.Text = dSet.Tables[0].Rows[0]["Smtpserverport"].ToString();
            chkEnableSSL.Checked = false;
            if (dSet.Tables[0].Rows[0]["smtpAuthenticate"].ToString() == "1")
            {
                chkEnableSSL.Checked = true;
            }

            txtPWFilePath.Text = dSet.Tables[0].Rows[0]["PWFilePath"].ToString();
           
           // txtMarquee.Text = dSet.Tables[0].Rows[0][6].ToString();
            //txtnewclientmail.Text = dSet.Tables[0].Rows[0][4].ToString();
            //txtnewclientmail.Text = dSet.Tables[0].Rows[0][4].ToString();

            //password policy
            DataRow dr = PasswordSettings().Rows[0];
            txtAttempts.Text = dr["MaxAttempts"].ToString();
            txtPWExpiry.Text = dr["PwExpiry"].ToString();
            txtHistory.Text = dr["KeepPwHistory"].ToString();
        }
        catch (System.Exception ex)
        {
            fn = new Functions();
            fn.logError(ex.Message);

        }
        finally
        {
            fn = null;
            dSet.Dispose();
            dAd.Dispose();
            conn.Dispose();
        }

    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        if (Session["IsAdmin"] == null) {
            lblmsg.ForeColor = System.Drawing.Color.Red;
            lblmsg.Text = "Only a superadmin can UPDATE system parameters.";
            return;
        }

        if (bool.Parse(Session["IsAdmin"].ToString()) == false)
        {
            lblmsg.ForeColor = System.Drawing.Color.Red;
            lblmsg.Text = "Only a superadmin can UPDATE system parameters.";
            return;
        }
        fn = new Functions();
        if (fn.isSafe(txthost.Text) == false || fn.isSafe(txtport.Text) == false || fn.isSafe(txtusername.Text) == false || fn.isSafe(txtpassword.Text) == false || fn.isSafe(txtFrom.Text) == false || fn.isSafe(txtTo.Text) == false || fn.isSafe(txtSubject.Text) == false || fn.isSafe(txtBody.Text) == false)
        {
            lblmsg.ForeColor = System.Drawing.Color.Red;
            lblmsg.Text = "Some fields may be empty.";
            return;
        }
        try
        {

            int enableSSL = 0;

            if (chkEnableSSL.Checked)
            {
                enableSSL = 1;
            }
            

            int result = UpdateSysSettings(txtusername.Text, txtpassword.Text, txthost.Text, int.Parse(txtport.Text), enableSSL, int.Parse(txtAttempts.Text), int.Parse(txtPWExpiry.Text), int.Parse(txtHistory.Text),txtPWFilePath.Text.Trim());
            if (result == -1)
            {
                syssettings();
                lblmsg.ForeColor = System.Drawing.Color.Blue;
                lblmsg.Text = "Update successful.";
                SaveActivity SaveUserActivity = new SaveActivity();
                SaveUserActivity.Activity = "Updated system parameters.";
                if (SaveUserActivity.SaveUserActivity(ref strMsg) != "")
                {
                    fn = new Functions();
                    fn.logError(strMsg);

                }
                SaveUserActivity = null;

            }
            else
            {
                lblmsg.ForeColor = System.Drawing.Color.Red;
                lblmsg.Text = "Update was not successful.";
            }
        }
        catch (System.Exception ex)
        {
            fn = new Functions();
            fn.logError(ex.Message);
            lblmsg.ForeColor = System.Drawing.Color.Red;
            lblmsg.Text = "Sorry an error occurred. Update was not successful.";
        }
        finally
        {
            fn = null;

        }
    }
    private int UpdateSysSettings(string sendusername, string sendpassword, string SmtpServer, int Smtpserverport, int EnableSSL, int MaxAttempts, int PwExpiry, int KeepPwHistory,string PWFilePath)
    {
        int result = 0;
        //declare SqlConnection and initialize it to the settings in the <connectionStrings> section of the web.config
        SqlConnection Conn = connection();
        //===============================
        //prepare the sql string
        string strSql = "sp_updatesyssettings";

        //declare sql command and initalize it
        SqlCommand Command = new SqlCommand(strSql, Conn);

        //set the command type
        Command.CommandType = CommandType.StoredProcedure;

        try
        {
            Command.Parameters.Clear();

            Command.Parameters.AddWithValue("@sendusername",sendusername);
            Command.Parameters.AddWithValue("@sendpassword",sendpassword);
            Command.Parameters.AddWithValue("@SmtpServer",SmtpServer);
            Command.Parameters.AddWithValue("@Smtpserverport",Smtpserverport);
            Command.Parameters.AddWithValue("@EnableSSL", EnableSSL);
            Command.Parameters.AddWithValue("@MaxAttempts",MaxAttempts);
            Command.Parameters.AddWithValue("@PwExpiry",PwExpiry);
            Command.Parameters.AddWithValue("@KeepPwHistory",KeepPwHistory);
            Command.Parameters.AddWithValue("@PWFilePath", PWFilePath);

            //open the database connection
            Conn.Open();
            //execute the command
            result = Command.ExecuteNonQuery();
        }
        catch (SqlException ex)
        {
            fn = new Functions();
            fn.logError(ex.Message);

        }
        finally
        {
            Command.Dispose();
            Conn.Dispose();
        }
        return result;
    }
    protected void btnSendtestmail_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        fn = new Functions();
        if (fn.isSafe(txthost.Text) == false || fn.isSafe(txtport.Text) == false || fn.isSafe(txtusername.Text) == false || fn.isSafe(txtpassword.Text) == false || fn.isSafe(txtFrom.Text) == false || fn.isSafe(txtTo.Text) == false || fn.isSafe(txtSubject.Text) == false || fn.isSafe(txtBody.Text) == false) return;

        try
        {

            MailMessage message = new MailMessage(txtFrom.Text, txtTo.Text, txtSubject.Text, txtBody.Text);
                //message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient(txthost.Text, int.Parse(txtport.Text));
            System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(txtusername.Text, txtpassword.Text);
            client.EnableSsl = chkEnableSSL.Checked;
            client.UseDefaultCredentials = false;
            client.Credentials = SMTPUserInfo;
            client.Send(message);
            lblmsg.ForeColor = System.Drawing.Color.Blue;
            lblmsg.Text = DateTime.Now + ": Mail Sent Successfully";
        }
        catch (System.Exception ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.Red;
            lblmsg.Text = ex.Message;
        }
        finally
        {
            fn = null;
        }
    }
    public DataTable PasswordSettings()
    {
        SqlConnection conn = connection();
        SqlDataAdapter dAd = new SqlDataAdapter("sp_getPasswordSettings", conn);
        dAd.SelectCommand.CommandType = CommandType.StoredProcedure;


        DataSet dSet = new DataSet();
        try
        {
            dAd.Fill(dSet, "PasswordSettings");
            return dSet.Tables["PasswordSettings"];
        }
        catch
        {
            throw;
        }
        finally
        {
            dSet.Dispose();
            dAd.Dispose();
            conn.Close();
            conn.Dispose();
        }
    }

}