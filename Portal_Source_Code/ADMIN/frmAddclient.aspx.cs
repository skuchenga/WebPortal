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
using System.Web.UI.Adapters;
using iTextSharp.text.pdf;
using iTextSharp.text;
using HFCPortal;
using HFCPortal.Common;


public partial class frmAddclient : System.Web.UI.Page
{
    public static string fileName;
    string strMsg = "";
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
        DateTime dtUpdatedOn = DateTime.Now;
        if (this.IsPostBack)
            return;
        loadgrid();
        LoadBranches();

    }
    protected void LoadBranches()
    {
        fn = new Functions();
        DataClass = new DataAccess();

        DbDataReader dr = DataClass.GetDBResults(ref strMsg, "sp_hfcBranches");
        if (strMsg != "")
        {
            return;
        }
        fn.FillCombo(dr, "cboBranch", this.Page);
        dr.Dispose();
        dr = null;
        fn = null;
        DataClass = null;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtUserID.Text == "")
        {
            lblMessageLine.ForeColor = System.Drawing.Color.Red;
            lblMessageLine.Text = "Please enter User ID";
        }
        else
        {
            lblMessageLine.Text = "";
            ClearFields();
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (txtUserID.Text == "")
        {
            lblMessageLine.ForeColor = System.Drawing.Color.Red;
            lblMessageLine.Text = "Please select a record from the table below";
        }
        else
        {


            DataAccess DataClass = new DataAccess();
            DbDataReader ResultSet = DataClass.GetDBResults(ref strMsg, "sp_Delete_user", "@UserID", txtUserID.Text);


            ClearFields();
            lblMessageLine.ForeColor = System.Drawing.Color.Blue;
            lblMessageLine.Text = "Record Deleted Successfully.";

        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("mainmenu.aspx");
    }
    protected void ShowUsers(Object sender, DataGridCommandEventArgs e)
    {
        txtUserID.Text = e.Item.Cells[1].Text;
        txteMail.Text = e.Item.Cells[3].Text;
        txtFullName.Text = e.Item.Cells[4].Text;
        txtPostalAddress.Text = e.Item.Cells[5].Text;
        // txtOfficeTel.Text = e.Item.Cells[6].Text;
        txtMobile.Text = e.Item.Cells[7].Text;
        lblMessageLine.Text = "";
    }
    protected void ClearFields()
    {
        txtUserID.Text = "";
        txteMail.Text = "";
        txtPassword.Text = "";
        txtConfirmPassword.Text = "";
    }
    protected void ClearFieldsx()
    {
        txtUserID.Text = "";
        txteMail.Text = "";
        txtPassword.Text = "";
        txtConfirmPassword.Text = "";

    }
    protected void SendMail()
    {


    }
    protected double EncryptPasswordBHome(string UserID, string Password, DateTime PWUpdatedOn)
    {
        double lngN;
        int intI;
        double ll;

        lngN = 0;

        Password = Password.PadRight(8).ToUpper();
        for (intI = 1; intI <= 8; intI++)
        {
            char ch = Password[intI - 1];
            ll = ((int)ch * intI);
            ll = PWUpdatedOn.Day;
            ll = PWUpdatedOn.Month;
            ll = Convert.ToByte(((PWUpdatedOn.Month + 3) / 3));
            ll = PWUpdatedOn.Year - 2000;
            lngN = lngN + ((int)ch * intI) + PWUpdatedOn.Day + PWUpdatedOn.Month + Convert.ToByte(((PWUpdatedOn.Month + 3) / 3)) + PWUpdatedOn.Year - 2000;
        }
        return lngN;
    }
    protected void btnCancel_Click1(object sender, EventArgs e)
    {
        //Response.Redirect("mainmenu.aspx");
        // InjectScriptLabel.Text = "<script>CloseOnReload()</" + "script>";
    }
    protected void sendmail(string emailadd)
    {


    }

    protected void btnPrinterFriendly_Click(object sender, EventArgs e)
    {
        Response.Redirect("EBankingClientsReport.aspx");

    }

    protected void txtUserID_TextChanged(object sender, EventArgs e)
    {

        Search();
    }
    protected void Search()
    {
        lblMessageLine.Text = "";
        fn = new Functions();


        DataClass = new DataAccess();
        DbDataReader rs = DataClass.GetDBResults(ref strMsg, "sp_GetUserInfo",
            "@UserID", txtUserID.Text.Replace("'", "''"));
        if (strMsg != "")
        {
            lblMessageLine.ForeColor = System.Drawing.Color.Red;
            lblMessageLine.Text = "Sorry an error occurred.";
            fn.logError(strMsg);
            return;
        }

        if (rs.Read())
        {
            try
            {
                txtUserID.Text = rs["UserID"].ToString();
                txtPostalAddress.Text = rs["PostalAddress"].ToString();
                txtFullName.Text = rs["FullName"].ToString();
                txteMail.Text = rs["EMail"].ToString();
                txtMobile.Text = rs["Mobile"].ToString();
                cboBranch.SelectedValue = rs["OurBranchID"].ToString();
                CheckBox1.Checked = bool.Parse(rs["Disable"].ToString());
                Button1.Enabled = true;
                BtnSaveChanges.Enabled = true;
                changePWD.Enabled = true;
                cmdAddRecord.Enabled = false;

            }
            catch (System.Exception ex)
            {
                lblMessageLine.ForeColor = System.Drawing.Color.Red;
                lblMessageLine.Text = "Error occurred.";
                fn.logError(ex.Message);
                return;
            }

        }
        else
        {
            //Clear();
            lblMessageLine.Text = "User Details Not Found Please Add User.";
            cmdAddRecord.Enabled = true;
            changePWD.Enabled = false;
            BtnSaveChanges.Enabled = false;
            txtPassword.ReadOnly = false;
            txtConfirmPassword.ReadOnly = false;
            Button1.Enabled = false;
        }

    }
    protected void Clear()
    {
        fn = new Functions();
        fn.ClearFields(this.Page);
        Button1.Enabled = false;
    }
    protected void grdUsers_SortCommand(object source, DataGridSortCommandEventArgs e)
    {

    }

    protected void BindData()
    {


    }

    protected void cmdHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("MainMenu.aspx");
    }
    protected void Show(Object sender, DataGridCommandEventArgs e)
    {

    }
    public string GetRandomString(Random rnd, int length)
    {
        string charPool = "1234567890";
        StringBuilder rs = new StringBuilder();

        while (length-- > 0)
            rs.Append(charPool[(int)(rnd.NextDouble() * charPool.Length)]);

        return rs.ToString();
    }


    //protected void sendDetails(string pwd)
    //{
    //    fn = new Functions();
    //    DataClass = new DataAccess();
    //    string filename = txtUserID.Text;

    //    Document myDocument = new Document(PageSize.A4.Rotate());
    //    try
    //    {

    //        DbDataReader rsSystemSettings = DataClass.GetDBResults(ref strMsg, "sp_getSystem");

    //        string PWFilePath = string.Empty;

    //        if (rsSystemSettings.Read())
    //        {
    //            PWFilePath = rsSystemSettings["PWFilePath"].ToString();
    //        }

    //        if (string.IsNullOrEmpty(PWFilePath))
    //        {
    //            return;
    //        }

    //        if (commonHelper == null)
    //        {
    //            commonHelper = new CommonHelper();
    //        }


    //        PWFilePath = commonHelper.CheckFilePath(PWFilePath);

    //        string userFile = PWFilePath + filename + ".pdf";

    //        PdfWriter.GetInstance(myDocument, new FileStream(userFile, FileMode.Create));
    //        myDocument.Open();
    //        myDocument.Add(new Paragraph("Dear Customer."));
    //        myDocument.Add(new Paragraph("Below,Please find your User Name and Password."));
    //        myDocument.Add(new Paragraph("_____________________________________________"));
    //        myDocument.Add(new Paragraph("User Name " + txtUserID.Text));
    //        myDocument.Add(new Paragraph("Password " + pwd ));
    //        //myDocument.Add(new Paragraph("Transaction Password: " +  Session["UserTrxPassword"].ToString()));
    //        myDocument.Close();

    //        //encrypt the password
    //        Stream output = new FileStream(userFile, FileMode.Create, FileAccess.Write, FileShare.None);
    //        PdfReader rdr = new PdfReader(userFile);
    //        PdfEncryptor.Encrypt(rdr, output, true, txtConfirmPassword.Text, txtConfirmPassword.Text, PdfWriter.ALLOW_PRINTING);

    //        //send mail on resetting
    //        MailMessage message = new MailMessage(System.Configuration.ConfigurationManager.AppSettings["BankemailAddress"].ToString(), txteMail.Text, "HFC Internet Banking Password Reset.", );
    //        SmtpClient smtp = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["smtpServer"].ToString(), 25);
    //        //System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential("customerservice@ecb.co.ke", "");
    //        //smtp.UseDefaultCredentials = false;
    //        //smtp.Credentials = SMTPUserInfo;
    //        Attachment attachFile = new Attachment(userFile);
    //        message.Attachments.Add(attachFile);
    //        smtp.Send(message);

    //        if (File.Exists(userFile))
    //            File.Delete(userFile);
    //    }
    //    catch (System.Exception ex)
    //    {

    //        lblMessageLine.Text = ex.Message;
    //    }
    //    finally {
    //        myDocument.Close();
    //    }
    //}

    protected string sendPDFEmail(string userFile) 
    {
        fn = new Functions();
        DataClass = new DataAccess();
        string strError=string.Empty;

        try
        {
            DbDataReader rsSystemSettings = DataClass.GetDBResults(ref strMsg, "sp_getSystem");

            if (rsSystemSettings.Read())
            {

                ////send mail on resetting

                string EmailBody = string.Empty;
                EmailBody = EmailBody + "<HTML>";
                EmailBody = EmailBody + "<BODY>";
                EmailBody = EmailBody + "<p> Dear Customer, </p>";
                EmailBody = EmailBody + "<p>Attached, Please find your Username and a New Password.</p>";
                
                EmailBody = EmailBody + "<p> To access the investment portal click on </p><a><href=" +    CommonHelper.GetSystemLocation() + ">BRIMS Web Portal</a>" ;
                EmailBody = EmailBody + "</BODY>";
                EmailBody = EmailBody + "</HTML>";
                MailMessage message = new MailMessage(rsSystemSettings["sendusername"].ToString(), txteMail.Text, "Amana Capital investment portal Login Details.", EmailBody);
                SmtpClient smtp = new SmtpClient(rsSystemSettings["SmtpServer"].ToString(), int.Parse(rsSystemSettings["smtpserverport"].ToString()));
                System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(rsSystemSettings["sendusername"].ToString(), rsSystemSettings["sendpassword"].ToString());
                smtp.EnableSsl = false;
                if (rsSystemSettings["smtpAuthenticate"].ToString() == "1")
                {
                    smtp.EnableSsl = true;    
                }
                
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = SMTPUserInfo;
                Attachment attachFile = new Attachment(userFile);
                message.Attachments.Add(attachFile);
                message.IsBodyHtml = true;
                smtp.Send(message);


                //if (File.Exists(PWFilePath + filename + ".pdf"))
                //    File.Delete(PWFilePath + filename + ".pdf");
            }
            else
            {
                strError = "Emails not configured. Client login credentials not sent.";
            }
            return strError;

        }
        catch (System.Exception exc) 
        {
            return exc.Message;
        }
    }

    protected void sendDetailsReset(string Password)
    {
        fn = new Functions();
        DataClass = new DataAccess();


        DbDataReader rsSystemSettings = DataClass.GetDBResults(ref strMsg, "sp_getSystem");

        string PWFilePath = string.Empty;

        if (rsSystemSettings.Read())
        {
            PWFilePath = rsSystemSettings["PWFilePath"].ToString();
        }
        else
        {
            lblMessageLine.Text = "Emails not configured. Client login credentials not sent.";
            return;
        }


        if (string.IsNullOrEmpty(PWFilePath)) {
            return;
        }

        if (!PWFilePath.EndsWith("\\"))
        {
            PWFilePath += "\\";
        }

        string filename = txtUserID.Text;

        string userFile = PWFilePath +  "user.pdf";
        string Ref = DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Year.ToString().PadLeft(4, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Millisecond.ToString().PadLeft(2, '0');
        filename = txtUserID.Text + Ref;
        Document myDocument = new Document(PageSize.A4.Rotate());
        try
        {

            PdfWriter.GetInstance(myDocument, new FileStream(PWFilePath + "user.pdf", FileMode.Create));
            myDocument.Open();
            myDocument.Add(new Paragraph("Dear Customer."));
            myDocument.Add(new Paragraph("Below,Please find your User Name and a New Password."));
            myDocument.Add(new Paragraph("_____________________________________________"));
            myDocument.Add(new Paragraph("User Name  " + txtUserID.Text));
            myDocument.Add(new Paragraph("Password  " + Password));
            //myDocument.Add(new Paragraph("Transaction Password: " +  Session["UserTrxPassword"].ToString()));
            myDocument.Close();

            //encrypt the password
            Stream output = new FileStream(PWFilePath + filename + ".pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            PdfReader rdr = new PdfReader(PWFilePath + "user.pdf");
            PdfEncryptor.Encrypt(rdr, output, true, txtConfirmPassword.Text, txtConfirmPassword.Text, PdfWriter.ALLOW_PRINTING);

            if (strMsg != "")
            {
                fn.logError(strMsg);
                lblMessageLine.Text = "Sorry an error occurred";
                return;
            }


            string EmailBody = string.Empty;
            

            EmailBody = EmailBody + "<html>";
            EmailBody = EmailBody + "<head><title></title></head>";
            EmailBody = EmailBody + "<body>";
            EmailBody = EmailBody + "<p> Dear Customer, </p>";
            EmailBody = EmailBody + "<p>Attached, Please find your Username and a New Password.</p>";

            EmailBody = EmailBody + "<p> To access the investment portal click on the link below.</p>";
            EmailBody = EmailBody + "http://portaladdress";  
            EmailBody = EmailBody + "</body>";
            EmailBody = EmailBody + "</html>";
            

                ////send mail on resetting
                MailMessage message = new MailMessage(rsSystemSettings["sendusername"].ToString(), txteMail.Text, "Amana Capital investment portal Password Reset.", EmailBody);
                SmtpClient smtp = new SmtpClient(rsSystemSettings["SmtpServer"].ToString(), int.Parse(rsSystemSettings["smtpserverport"].ToString()));
                System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(rsSystemSettings["sendusername"].ToString(), rsSystemSettings["sendpassword"].ToString());
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = SMTPUserInfo;
                smtp.EnableSsl = false;
                if (rsSystemSettings["smtpAuthenticate"].ToString() == "1")
                {
                    smtp.EnableSsl = true;
                }
                Attachment attachFile = new Attachment(PWFilePath + filename + ".pdf");
                message.Attachments.Add(attachFile);
                message.IsBodyHtml = true;
                smtp.Send(message);

                //if (File.Exists(PWFilePath + filename + ".pdf"))
                //    File.Delete(PWFilePath + filename + ".pdf");
           
           
        }
        catch (System.Exception ex)
        {

            lblMessageLine.Text = ex.Message;
        }
        //Page.ClientScript.RegisterStartupScript(GetType(), "key", "<script type='text/javascript'>alert('Request successfully sent');</script>", false);
    }

    protected void cmdAddRecord_Click1(object sender, EventArgs e)
    {
        Page.Validate();
        if (!(Page.IsValid))
            return;
        lblMessageLine.Text = "";
        DateTime ctrlDate = DateTime.Now;
        fn = new Functions();
        string GenPassword = "";
      
        int i = 50;
        Random rnd = new Random();
        while (i-- > 0)

        GenPassword = GetRandomString(rnd, 8);
        decimal Password = Decimal.Parse(fn.EncryptUserPassword(GenPassword, ctrlDate).ToString());


        BALclients BAL = new BALclients();
        try
        {
           string strPdf = createpdf(GenPassword);
           if (strPdf != "")
            {
                if (!strPdf.Contains(".pdf"))
                {
                    lblMessageLine.Text = strPdf;
                    return;
                }
            }
            int status = BAL.addEditClients(cboBranch.SelectedValue.ToString(), txtUserID.Text.Replace("'", "''").ToUpper(), Password, txtFullName.Text.Replace("'", "''").ToUpper(), CheckBox1.Checked, txteMail.Text, txtMobile.Text, ctrlDate, "admin", txtPostalAddress.Text, 1);
            if (status > 0)
            {
                //lblMessageLine.Text = "New client successfully added";
               string strEmailLog= sendPDFEmail(strPdf);
               if (string.IsNullOrEmpty(strEmailLog))
               {
                   lblMessageLine.Text = "Client added and login credentials emailed successfully";
               }
               else 
               { 
                   lblMessageLine.Text = "Client added successfully but login credentials email failed. Please check your email configurations." ;
               }

               Response.Redirect("~/frmAddClient.aspx");
            }
            else
            {
                lblMessageLine.ForeColor = System.Drawing.Color.Red;
                lblMessageLine.Text = "New client was not added. Please check data entered.";
            }
        }
        catch (System.Exception ex)
        {
            fn.logError(ex.Message);
            lblMessageLine.ForeColor = System.Drawing.Color.Red;
            lblMessageLine.Text = "Sorry an error occurred. New client was not added.";
        }
    }

    protected void changePWD_Click(object sender, EventArgs e)
    {
        lblMessageLine.Text = "";
        //Page.Validate();
        //if (!(Page.IsValid))
        //    return;
        if (txtPassword.Text != txtConfirmPassword.Text && txtPassword.Text == "")
            return;

        fn = new Functions();
        try
        {
            DateTime ctrlDate = DateTime.Now;

            SaveActivity SaveUser = new SaveActivity();
            SaveUser.Activity = "Reset Customer with UserID:" + txtUserID.Text;
            if (SaveUser.SaveUserActivity(ref strMsg) != "")
            {
                return;
            }
            SaveUser = null;
            ResetUser();

        }
        catch (System.Exception ex)
        {
            fn.logError(ex.Message);
            lblMessageLine.ForeColor = System.Drawing.Color.Red;
            lblMessageLine.Text = "Sorry an error occurred.";
        }
    }
    protected void ResetUser()
    {
        lblMessageLine.Text = "";
        DateTime ctrlDate = DateTime.Now;
        Functions fnx = new Functions();
        string GenPassword = "";
        string GenTRXPassword = "";
        int i = 50;
        Random rnd = new Random();
        while (i-- > 0)

            GenPassword = GetRandomString(rnd, 5);
        i = 50;
        Random rnd2 = new Random();
        while (i-- > 0)

            GenTRXPassword = GetRandomString(rnd, 8);

        Session["UserPassword"] = GenPassword;
        Session["UserTrxPassword"] = GenTRXPassword;
        DateTime CtrlDate;
        CtrlDate = DateTime.Now;

        fn = new Functions();
        try
        {
            DataAccess DataClass = new DataAccess();
            DbDataReader rsSave = DataClass.GetDBResults(ref strMsg, "sp_addEditUsersPass",
                        "@UserID", txtUserID.Text.Replace("'", "''"),
                        "@Password", fn.EncryptUserPassword(GenPassword, CtrlDate),
                        "@Updatedon", CtrlDate,
                        "@OperatorID", "");
            if (strMsg != "")
            {
                fn = new Functions();
                fn.logError(strMsg);
                lblMessageLine.Text = "Sorry an error occurred.";
                return;
            }
            lblMessageLine.Text = "Customer Password Reset Successfully.";
            cmdAddRecord.Enabled = false;
            sendDetailsReset(GenPassword);
        }
        catch (System.Exception ex)
        {
            fn.logError(ex.Message);
            lblMessageLine.ForeColor = System.Drawing.Color.Red;

            lblMessageLine.Text = "Sorry an error occurred.";
            cmdAddRecord.Enabled = true;
        }

    }

    protected void BtnClear_Click(object sender, EventArgs e)
    {
        Clear();
        BtnSaveChanges.Enabled = false;

    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {

        Search();
    }



    protected void Button1_Click1(object sender, EventArgs e)
    {
        DataAccess DataClass = new DataAccess();
        DbDataReader rsSave = DataClass.GetDBResults(ref strMsg, "sp_UnlockUsers",
                    "@UserID", txtUserID.Text);

        lblMessageLine.Text = "User Unlocked Successfully.";
        SaveActivity SaveUser = new SaveActivity();
        SaveUser.Activity = "Unlocked Customer With UserID:" + txtUserID.Text;
        if (SaveUser.SaveUserActivity(ref strMsg) != "")
        {
            return;
        }
        SaveUser = null;
    }

    protected void BTNDELETE_Click(object sender, EventArgs e)
    {
        DataClass = new DataAccess();
        DbDataReader rsSave = DataClass.GetDBResults(ref strMsg, "sp_DeleteUsers",
                    "@UserID", txtUserID.Text);
        if (strMsg != "")
        {
            lblMessageLine.ForeColor = System.Drawing.Color.Red;
            lblMessageLine.Text = "Error occurred. Cannot continue..";
            fn.logError(strMsg);
        }

        lblMessageLine.Text = "User Deleted Successfully.";
        SaveActivity SaveUser = new SaveActivity();
        SaveUser.Activity = "Deleted Customer With UserID:" + txtUserID.Text;
        if (SaveUser.SaveUserActivity(ref strMsg) != "")
        {
            return;
        }
        SaveUser = null;
    }

    protected void loadgrid()
    {
        fn = new Functions();
        DataTable dt = new DataTable();
        BALclients BAL = new BALclients();
        try
        {
            dt = BAL.getUsers();
            RadGrid1.DataSource = dt;
            RadGrid1.DataBind();
        }
        catch (System.Exception ex)
        {
            //logging actual error
            fn.logError(ex.Message);
            //displaying friendly error
            lblMessageLine.ForeColor = System.Drawing.Color.Red;
            lblMessageLine.Text = "Sorry an error occurred";


        }
        finally
        {
            HttpContext.Current.Session["Clients"] = dt;
            dt.Dispose();
            BAL = null;
        }
    }
    protected string createpdf(string GenPassword)
    {
        

        strMsg = "";
        fn = new Functions();
        DataClass = new DataAccess();

        string filename = txtUserID.Text;
        string userFile =  "user.pdf";

        Document myDocument = new Document(PageSize.A4.Rotate());
        try
        {

            DbDataReader rsSystemSettings = DataClass.GetDBResults(ref strMsg, "sp_getSystem");
            if (commonHelper == null){
                commonHelper = new CommonHelper();
            }

            string PWFilePath = string.Empty;
            string pdfFile = string.Empty;
            //PWFilePath = rsSystemSettings["PWFilePath"].ToString();

            if (rsSystemSettings.Read())
            {
                PWFilePath = rsSystemSettings["PWFilePath"].ToString();
            }

            PWFilePath = CommonHelper.CheckFilePath(PWFilePath);


            PdfWriter.GetInstance(myDocument, new FileStream(PWFilePath + userFile, FileMode.Create));
            myDocument.Open();
            myDocument.Add(new Paragraph("Dear Customer."));
            myDocument.Add(new Paragraph("Below,Please find your User Name and Password."));
            myDocument.Add(new Paragraph("_____________________________________________"));
            myDocument.Add(new Paragraph("User Name: " + txtUserID.Text));
            myDocument.Add(new Paragraph("Password : " + GenPassword));
            //myDocument.Add(new Paragraph("Transaction Password: " +  Session["UserTrxPassword"].ToString()));
            myDocument.Close();

            pdfFile = PWFilePath + txtUserID.Text + ".pdf";

            //encrypt the password
            Stream output = new FileStream(pdfFile, FileMode.Create, FileAccess.Write, FileShare.None);
            PdfReader rdr = new PdfReader(PWFilePath + userFile);
            PdfEncryptor.Encrypt(rdr, output, true, txtPassword.Text, txtPassword.Text, PdfWriter.ALLOW_PRINTING);

            return pdfFile;


        }
        catch (System.Exception ex)
        {
            fn.logError(ex.Message);
            strMsg = "sorry an error occurred";
        }
        finally {
            myDocument.Close();
            
        }
        return strMsg;

    }
    protected void RadGrid1_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        fn = new Functions();
        try
        {
            RadGrid1.DataSource = (DataTable)Session["Clients"];
        }
        catch (System.Exception ex)
        {
            //logging actual error
            fn.logError(ex.Message);
            //displaying friendly error
            lblMessageLine.ForeColor = System.Drawing.Color.Red;
            lblMessageLine.Text = "Sorry an error occurred";
        }

    }
    protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
    {
        lblMessageLine.Text = "";
        fn = new Functions();
        lblMessageLine.Text = "";
        try
        {


            if (e.CommandName == "Select")
            {
                cmdAddRecord.Enabled = false;
                BtnSaveChanges.Enabled = true;
                changePWD.Enabled = true;
                txtUserID.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserID"].ToString();
                cboBranch.SelectedValue = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OurBranchID"].ToString();
                txtFullName.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FullName"].ToString();
                txtPostalAddress.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["PostalAddress"].ToString();
                txtMobile.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Mobile"].ToString();
                CheckBox1.Checked = bool.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Disable"].ToString());
                txteMail.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Email"].ToString();
            }
        }
        catch (System.Exception ex)
        {
            lblMessageLine.Text = "Sorry an error occurred";
            fn.logError(ex.Message);
        }
    }
    protected void BtnSaveChanges_Click(object sender, EventArgs e)
    {
        fn = new Functions();
        BALclients BAL = new BALclients();
        try
        {
            int status = BAL.addEditClients(cboBranch.SelectedValue.ToString(), txtUserID.Text.Replace("'", "''").ToUpper(), 0, txtFullName.Text.Replace("'", "''").ToUpper(), CheckBox1.Checked, txteMail.Text, txtMobile.Text, DateTime.Now, "admin", txtPostalAddress.Text, 2);
            if (status > 0)
            {
                lblMessageLine.Text = "Details successfully saved";
                loadgrid();
            }
            else
            {
                lblMessageLine.ForeColor = System.Drawing.Color.Red;
                lblMessageLine.Text = "New client was not added. Please check data entered.";
            }
        }
        catch (System.Exception ex)
        {
            fn.logError(ex.Message);
            lblMessageLine.Text = "sorry an error occurred";
        }
    }

}
