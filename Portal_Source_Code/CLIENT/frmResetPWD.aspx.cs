using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Common;
//using iTextSharp.text.pdf;
//using Telerik.WebControls;
using System.Net.Mail;
using System.Web.Mail;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Text;
//using iTextSharp.text;
using System.Web.UI.Adapters;
using HFCPortal;
using HFCPortal.Common;



public partial class frmAgent : System.Web.UI.Page
{
    string strMsg = "", GenPassword;
    double Pass = 0.00;
    DataAccess DataClass;
    Functions fn;
    DateTime ctrldate = DateTime.Now;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            cmdOk.Attributes.Add("OnClick", "window.close()");
            cmdSubmit.Style.Add("display", "");
            cmdOk.Style.Add("display", "none");

        }
    }
    
    protected void cmdCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Login.aspx");
    }
    public string GetRandomString(Random rnd, int length)
    {
        string charPool = "1234567890";
        StringBuilder rs = new StringBuilder();

        while (length-- > 0)
            rs.Append(charPool[(int)(rnd.NextDouble() * charPool.Length)]);

        return rs.ToString();
    }
    protected void ChangePwd()
    {
        DateTime ctrlDate = DateTime.Now;
        Functions fnx = new Functions();
        double usrPassword = 0;
        GenPassword = "";
        int i = 50;
        Random rnd = new Random();
        while (i-- > 0)

            GenPassword = GetRandomString(rnd, 5);
        i = 50;
        Random rnd2 = new Random();
      

        Session["ResetPassword"] = GenPassword;


        fn = new Functions();
        DateTime CtrlDate;
        CtrlDate = DateTime.Now;
        usrPassword = fn.EncryptUserPassword(GenPassword, CtrlDate);

        Session["UserPassword"] = System.Convert.ToInt32(usrPassword).ToString();
        Session["pwd"] = System.Convert.ToInt32(usrPassword).ToString();


        
   

        try
        {
            DataAccess DataClass = new DataAccess();
            DbDataReader rsSave = DataClass.GetDBResults(ref strMsg, "sp_addEditUsersPass",
                        "@UserID", txtUsername.Text.Replace("'", "''"),
                        "@Password", usrPassword,
                        "@Updatedon", CtrlDate,
                        "@OperatorID", "");
            if (strMsg != "")
            {
                fn = new Functions();
                fn.logError(strMsg);
                lblMessageLine.Text = "Sorry an error occurred.";
                return;
            }
           
        }
        catch (System.Exception ex)
        {
            fn.logError(ex.Message);
            lblMessageLine.ForeColor = System.Drawing.Color.Red;

            lblMessageLine.Text = "Sorry an error occurred.";
        }
    }
    //protected void cmdOk_Click(object sender, EventArgs e) {
    //    Response.Redirect("Level2Login.aspx");
    //}

    protected void cmdSubmit_Click(object sender, EventArgs e)
    {
      
        DataClass = new DataAccess();
        DbDataReader rs = DataClass.GetDBResults(ref strMsg, "sp_MTgetUser",
            "@UserID",txtUsername.Text.Replace ("'","''"),
            "@Email",txtEmail.Text.Replace("'","''"));
        if (strMsg != "")
        {
            lblMessageLine.Text = "Undetected errror occurred: " + strMsg.ToString();
            txtEmail.Text = "" ;
            txtUsername.Text = "";
            return;
        }
        if (rs.Read())
        {
            ChangePwd();
            sendDetails();
            lblMessageLine.Text = "Password successfully reset and an email sent to your account";
            cmdOk.Style.Add("display","");
            cmdSubmit.Style.Add("display", "none");
            return;

        }
        else
        {
            lblMessageLine.Text = "Wrong information given cannot change password";
            return;
        }
    }
    protected void sendDetails()
    {
         
        try
        {
 
            ////send mail on user creation

            strMsg = "";
            DataClass = new DataAccess();
            DbDataReader rs = DataClass.GetDBResults(ref strMsg, "sp_getSystem");
            if (strMsg != "")
            {
                lblMessageLine.Text = strMsg;
                return;
            }
            if (rs.Read())
            {
                string Subject;

                string EmailBody;

                EmailBody = "<HTML>";
                EmailBody = EmailBody + "<BODY>";
                EmailBody = EmailBody + "<P> Dear Customer, </P>";
                EmailBody = EmailBody + "<P>Below, Please find your Username and a New Password.</P>";
                EmailBody = EmailBody + "<P>User name: " + txtUsername.Text + "</P>";
                EmailBody = EmailBody + "<P> Password : " + GenPassword + "</P>";
                EmailBody = EmailBody + "<P> To access the investment portal click on " + CommonHelper.GetSystemLocation() + "</P>";
                EmailBody = EmailBody + "</BODY>";
                EmailBody = EmailBody + "</HTML>";

            
                Subject = "Amana Capital Login Credentials";
                // Mail initialization
                System.Web.Mail.MailMessage mailMsg = new System.Web.Mail.MailMessage();
                mailMsg.From = rs["SendUsername"].ToString();
                mailMsg.To = txtEmail.Text ;

                mailMsg.Subject = Subject;
                mailMsg.BodyFormat = MailFormat.Text;
                mailMsg.Body = EmailBody;
                //mailMsg.Priority = MailPriority.High;
                // Smtp configuration
                SmtpMail.SmtpServer = rs["SmtpServer"].ToString();
                // - smtp.gmail.com use smtp authentication
                mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
                mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", rs["sendusername"].ToString());
                mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", rs["sendpassword"].ToString());
                // - smtp.gmail.com use port 465 or 587
                mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", rs["smtpserverport"].ToString());
                // - smtp.gmail.com use STARTTLS (some call this SSL)
                mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");
                 // try to send Mail
                try
                {
                    SmtpMail.Send(mailMsg);
                    //SmtpClient smtpcl = new SmtpClient();
                    //smtpcl.Send(new System.Net.Mail.MailMessage( mailMsg);
                    lblMessageLine.Text = "Password reset successful and e-mail sent with new password.";
                    //lblmsg.Text = "Email sent!";
                }
                catch (System.Exception ex)
                {
                    lblMessageLine.ForeColor = System.Drawing.Color.Black;
                    lblMessageLine.Text = "Password has been reset successfully but no mail has been sent to the user,check mail server";
                }


            }


        }
        catch (System.Exception ex)
        {
            lblMessageLine.ForeColor = System.Drawing.Color.Black;
            lblMessageLine.Text = ex.Message;
        }

    }
     
}
