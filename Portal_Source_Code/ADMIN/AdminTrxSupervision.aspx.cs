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
using System.Text;
using Telerik.Web.UI;
using System.Net.Mail;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Web.UI.Adapters;
using HFCPortal;
using HFCPortal.Common;

public partial class AdminTrxSupervision : System.Web.UI.Page
{

    String strMsg = "";
    DataAccess DataClass;
    Functions fn;
    CommonHelper commonHelper;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Form.Attributes.Add("autocomplete", "off");
        if (Session.Keys.Count == 0 || Session["LoginID"] == null || Session["LoginID"].ToString() == "")
        {
            HttpContext.Current.Session.Clear();
            Server.Transfer("Login.aspx");

        }
        if (this.IsPostBack)
            return;

        if (commonHelper == null) {
            commonHelper = new CommonHelper();
        }

        Functions fn = new Functions();
        dsmodule.ConnectionString = CommonHelper.GetConnectionString();
        dsmodule.SelectCommand = "SELECT moduleid,moduledescription from t_modules where IsForSupervision=1";
        dsmodule.SelectCommandType = SqlDataSourceCommandType.Text;
        cmbModules.DataSourceID = dsmodule.ID;
        cmbModules.DataTextField = "moduledescription";
        cmbModules.DataValueField = "moduleid";
        try
        {
            if (Session["SelectedModuleID"] != null)
            {
                cmbModules.SelectedValue = Session["SelectedModuleID"].ToString();
            }
            
        }
        catch
        {

        }

    }

    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        checkuserid();
    }



    protected void getData()
    {

        DataClass = new DataAccess();
        DbDataReader rs = DataClass.GetDBResults(ref strMsg, "sp_getAuditRecords",
            "@ModuleID", cmbModules.SelectedValue,
            "@OperatorID", Session["LoginID"].ToString());

        RadGrid1.DataSource = rs;
        RadGrid1.DataBind();


        // rs.Dispose();
    }
    protected void cmbModules_SelectedIndexChanged(object sender, EventArgs e)
    {
        getData();
        lblMessageLine.Text = "";
        Label3.Text = "";
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        getData();
        lblMessageLine.Text = "";
        Label3.Text = "";
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnPass_Click(object sender, EventArgs e)
    {
        Page.Validate();
        if (!(Page.IsValid))
            return;

        lblMessageLine.Text = "";
        Label3.Text = "";
        int rowindex = 0;
        foreach (GridDataItem row in RadGrid1.Items)
        {
            Control control1 = row.FindControl("chkAccount1");
            CheckBox checkBox = control1 as CheckBox;
            Type type1 = control1.GetType();
            string ns1 = type1.Namespace;

            if (checkBox.Checked == true)
            {
                rowindex += 1;
                string ColumnID = row.Cells[3].Text;
                string Status = row.Cells[5].Text;//Should be NEW CLIENT
                string FieldName = row.Cells[6].Text;//UserID
                string NewValue = row.Cells[8].Text;//UserID name
                string Email = row.Cells[9].Text;
                Session["SelectedUserID"] = NewValue;
                Session["Email"] = Email;
                if (cmbModules.SelectedValue == "009" && Status == "NEW CLIENT" && NewValue != "")
                {
                    strMsg = updatesysaudit(ColumnID);
                    if (strMsg != "")
                    {
                        lblMessageLine.Text = "";
                        return;
                    }
                    sendDetails();
                    SaveActivity SaveUserActivity = new SaveActivity();
                    SaveUserActivity.Activity = "Supervised New Customer:" + Email + ":UserID" + NewValue.Replace("'", "''");
                    if (SaveUserActivity.SaveUserActivity(ref strMsg) != "")
                    {
                        return;
                    }
                    SaveUserActivity = null;

                }
                if (cmbModules.SelectedValue == "009" && Status == "EDITED" && NewValue != "")
                {
                    strMsg = updatesysaudit(ColumnID);
                    if (strMsg != "")
                    {
                        lblMessageLine.Text = "";
                        return;
                    }
                    SaveActivity SaveUserActivity = new SaveActivity();
                    SaveUserActivity.Activity = "Authorized update on customer details:" + Email + ":UserID" + NewValue.Replace("'", "''");
                    if (SaveUserActivity.SaveUserActivity(ref strMsg) != "")
                    {
                        return;
                    }
                    SaveUserActivity = null;

                }

                if (cmbModules.SelectedValue == "015" && Status == "NEW ADMIN" && NewValue != "")
                {

                    SaveActivity SaveUserActivity = new SaveActivity();
                    SaveUserActivity.Activity = "Supervised New ADMIN:" + Email + ":UserID" + NewValue.Replace("'", "''");
                    if (SaveUserActivity.SaveUserActivity(ref strMsg) != "")
                    {
                        return;
                    }
                    SaveUserActivity = null;

                }




                lblMessageLine.ForeColor = System.Drawing.Color.Blue;
                lblMessageLine.Text = "Selected Records Have Been Supervised Successfully.";

                SaveActivity SaveUser = new SaveActivity();
                SaveUser.Activity = "Supervise/Approve Record:Date:" + DateTime.Now + ":Comment Issued:" + txtcomment.Text;
                if (SaveUser.SaveUserActivity(ref strMsg) != "")
                {
                    return;
                }
                SaveUser = null;
            }
        }
    }
    protected string updatesysaudit(string ColumnID)
    {
        fn = new Functions();
        strMsg = "";
        DataClass = new DataAccess();

        DbDataReader rsPass = DataClass.GetDBResults(ref strMsg, "sp_UpdateSysAudit",
         "@OurBranchID", "",
         "@ColumnID", ColumnID,
         "@OperatorID", Session["LoginID"].ToString(),
         "@SupervisedOn", DateTime.Now,
         "@Status", "S",
         "@NewRecord", 0, "@Comment", txtcomment.Text);
        if (strMsg != "")
        {
            fn.logError(strMsg);
            strMsg = "sorry an error occurred.";
            return strMsg;
        }
        getData();
        return strMsg;
    }
    protected void sendDetails()
    {
        fn = new Functions();
        DataClass = new DataAccess();
        string filename = Session["SelectedUserID"].ToString();
        //string userFile = "C:\\user.pdf";
        string BODY = "Dear Customer,\n\n" +
                "Your Amana Capital investment portal account is now ready. Please find your\n" +
                "login credentials in the attached document. The document has been password.\n" +
                "protected with the start key that you indicated on your application form\n" +
                "for investment portal .Please open the attachment and input this key to \n" +
                "enable you to access your login credentials. Please also note that the \n" +
                "attachment is in Adobe PDF file format. If you do not have the Adobe acrobat \n" +
                "software installed on your machine you may download it at the address provided below.\n\n" +

                "http://get.adobe.com/reader " + "\n\n" +

               "Once you are able to access your login credentials, you shall be able to login into your \n" +
               "investment portal account.\n" +
               "In case of any \n" +
               "queries, clarification or any difficulties accessing your \n" +
               "account, please feel free to get in touch with our call centre team via \n" +
               "the contact details provided below. \n \n" +

               "Email: brimsportal@hfcbank.com.gh " + "\n" +
               "Tel:   + " + "\n" +
               "Fax:   +  \n\n";



        try
        {
            DbDataReader rsBankDetails = DataClass.GetDBResults(ref strMsg , "sp_getSystem");
            if (strMsg != "")
            {
                fn.logError(strMsg);
                lblMessageLine.Text = "Sorry an error occurred";
                return;
            }
           
            if (rsBankDetails.Read())
            {

                ////send mail
                MailMessage message = new MailMessage(rsBankDetails["sendusername"].ToString(), Session["Email"].ToString(), "Amana Capital investment portal Login Details", BODY);
                SmtpClient smtp = new SmtpClient(rsBankDetails["SmtpServer"].ToString(), int.Parse(rsBankDetails["smtpserverport"].ToString()));
                System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(rsBankDetails["sendusername"].ToString(), rsBankDetails["sendpassword"].ToString());
                smtp.EnableSsl = false;
                if (rsBankDetails["sendusername"].ToString() == "1")
                {
                    smtp.EnableSsl = true;
                }
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = SMTPUserInfo;
                Attachment attachFile = new Attachment(CommonHelper.CheckFilePath(rsBankDetails["PWFilePath"].ToString()) + filename + ".pdf");
                message.Attachments.Add(attachFile);
                smtp.Send(message);

            }
            else
            {
                lblMessageLine.Text = "Emails not configured. Client login credentials not sent.";
                return;
            }

        }
        catch (System.Exception ex)
        {

            lblMessageLine.Text = ex.Message;
        }

    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        if (txtcomment.Text == "")
        {
            lblMessageLine.ForeColor = System.Drawing.Color.Red;
            lblMessageLine.Text = "Please Enter Comment/Remarks For Rejecting.";
            return;
        }
        lblMessageLine.Text = "";
        Label3.Text = "";
        int rowindex = 0;
        foreach (GridDataItem row in RadGrid1.Items)
        {
            Control control1 = row.FindControl("chkAccount1");
            CheckBox checkBox = control1 as CheckBox;
            Type type1 = control1.GetType();
            string ns1 = type1.Namespace;

            if (checkBox.Checked == true)
            {
                rowindex += 1;
                string ColumnID = row.Cells[3].Text;

                DataClass = new DataAccess();
                DbDataReader rsPass = DataClass.GetDBResults(ref strMsg, "sp_DeleteSysAudit",
                 "@OurBranchID", Session["OurBranchID"].ToString(),
                 "@ColumnID", ColumnID,
                 "@OperatorID", Session["LoginID"].ToString(),
                 "@SupervisedOn", DateTime.Now,
                 "@Status", "R",
                 "@NewRecord", 0);
                getData();
                lblMessageLine.ForeColor = System.Drawing.Color.Blue;
                lblMessageLine.Text = "Selected Records Have Been Cancelled Successfully.";

                SaveActivity SaveUser = new SaveActivity();
                SaveUser.Activity = "Supervise records By Rejecting:Date:" + DateTime.Now + ":Comment Issued:" + txtcomment.Text;
                if (SaveUser.SaveUserActivity(ref strMsg) != "")
                {
                    return;
                }
                SaveUser = null;

            }

        }
    }
    protected void btnApp_Click(object sender, EventArgs e)
    {
        if (txtPassword.Text == "")
        {
            Label3.ForeColor = System.Drawing.Color.Red;
            Label3.Text = "Please Enter Your Password.";
            cmbModules.Enabled = false;
            btnPass.Enabled = false;
            btnRefresh.Enabled = false;
            btnReject.Enabled = false;
        }
        else
        {
            checkuserid();
        }
    }
    public void checkuserid()
    {
        cmbModules.Enabled = false;
        Functions fn = new Functions();
        DateTime ctrldate;
        
        ctrldate = System.Convert.ToDateTime(Session["ctrldate"]);

        Double paswd = fn.EncryptUserPassword(txtPassword.Text, ctrldate);
        try
        {
            if (paswd == double.Parse(Session["Password"].ToString()))
            {
                strMsg = "";
                SaveActivity SaveUserActivity = new SaveActivity();
                SaveUserActivity.Activity = "Success To Supervise Transactions.";
                if (SaveUserActivity.SaveUserActivity(ref strMsg) != "")
                {
                    return;
                }
                SaveUserActivity = null;
                cmbModules.Enabled = true;
                Label3.ForeColor = System.Drawing.Color.Blue;
                Label3.Text = "Correct Password Entered.You Cannot Suppervise Your Own Transactions.";
                btnPass.Enabled = true;
                btnRefresh.Enabled = true;
                btnReject.Enabled = true;
            }
            else
            {
                Label3.ForeColor = System.Drawing.Color.Red;
                Label3.Text = "Invalid password.";
                cmbModules.Enabled = false;
                btnPass.Enabled = false;
                btnRefresh.Enabled = false;
                btnReject.Enabled = false;
                return;
            }
        }
        catch
        {
            Label3.ForeColor = System.Drawing.Color.Red;
            Label3.Text = "Session Expired log off.";

        }

    }
    protected void dsmodule_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {

    }
    protected void txtcomment_TextChanged(object sender, EventArgs e)
    {

    }

}

