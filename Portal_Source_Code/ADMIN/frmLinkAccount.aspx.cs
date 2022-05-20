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
using Telerik.Web.UI;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.IO;
using BRIMSPortalWebService;
using HFCPortal;
using System.Collections.Generic;

public partial class frmLinkAccount : System.Web.UI.Page
{
    String strMsg = "";
    Functions fn;
    DataAccess DataClass;
    BRInvestmentPortalService hfcs;

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
        fn.FillCombo(dr, "cmbBranch", this.Page);
        dr.Close();
        dr.Dispose();
        fn = null;
        DataClass = null;
    }


    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Page.Validate();
        if (!(Page.IsValid))
            return;

        fn = new Functions();
        DataClass = new DataAccess();
        string accountID = string.Empty;
        string message = string.Empty;

        DbDataReader ResultSet = DataClass.GetDBResults(ref strMsg, "sp_AddCustomerAccount",
                   "@BranchID", cmbBranch.SelectedValue,
                   "@AccountID", accountID,
                   "@UserID", txtuserID.Text.Replace("'", "''"),
                   "@CurrnceyID", txtCurrencyID.Text,
                   "@NickName", txtName.Text);

        foreach (Telerik.Web.UI.GridDataItem dataItem in RadGrid1.MasterTableView.Items)
        {
            if (dataItem.Selected == true)
            {
                // Int32 ID = (Int32)dataItem.GetDataKeyValue("ID");
                accountID = dataItem.GetDataKeyValue("AccountID").ToString();

                //Check if account is already linked.
                DbDataReader AccountExists = DataClass.GetDBResults(ref strMsg, "sp_GetAccountByID", "@AccountID", accountID);
                if (strMsg != "")
                {

                    //lblMessageLine.Text = strMsg.ToString();
                    //return;

                    message = message + strMsg.ToString();
                }

                if (AccountExists.Read())
                {
                   // lblMessageLine.Text = "This account has already been linked. Please delink it first to continue.";
                    break;
                }


                //DbDataReader ResultSet = DataClass.GetDBResults(ref strMsg, "sp_AddCustomerAccount",
                //    "@BranchID", cmbBranch.SelectedValue,
                //    "@AccountID", txtAccountID.Text,
                //    "@UserID", txtuserID.Text.Replace("'", "''"),
                //    "@CurrnceyID", txtCurrencyID.Text,
                //    "@NickName", txtName.Text);

                //DbDataReader ResultSet = DataClass.GetDBResults(ref strMsg, "sp_AddCustomerAccount",
                //    "@BranchID", cmbBranch.SelectedValue,
                //    "@AccountID", accountID,
                //    "@UserID", txtuserID.Text.Replace("'", "''"),
                //    "@CurrnceyID", txtCurrencyID.Text,
                //    "@NickName", txtName.Text);


                if (strMsg != "")
                {

                    //lblMessageLine.Text = strMsg.ToString();
                    //return;

                    message = message + strMsg.ToString();
                }

            
            }
        }
        /////End 

         if (message != string.Empty)
         {
             lblMessageLine.Text = message;
             return;
         }

        BindDate(txtuserID.Text.Replace("'", "''"));
        strMsg = "";
        HFCPortal.SaveActivity SaveUserA = new SaveActivity();
        SaveUserA.Activity = "Link Account:" + txtAccountID.Text + ":To UserID:" + txtuserID.Text + ",Successfully.";
        if (SaveUserA.SaveUserActivity(ref strMsg) != "")
        {
            lblMessageLine.Text = strMsg;
            return;
        }
        
            
            SaveUserA = null;

        fn = null;
        DataClass = null;
        btnAdd.Enabled = false;
        ClearFields();
        lblMessageLine.Text = "Account successfully linked.";

    }


    protected void btnAdds_Click(object sender, EventArgs e)
    {
        Page.Validate();
        if (!(Page.IsValid))
            return;

        fn = new Functions();
        DataClass = new DataAccess();
        //Check if account is already linked.
        DbDataReader AccountExists = DataClass.GetDBResults(ref strMsg, "sp_GetAccountByID","@AccountID", txtAccountID.Text);
        if (strMsg != "")
        {

            lblMessageLine.Text = strMsg.ToString();
            return;
        }

        if (AccountExists.Read()) {
            lblMessageLine.Text ="This account has already been linked. Please delink it first to continue.";
            return;
        }
       

        DbDataReader ResultSet = DataClass.GetDBResults(ref strMsg, "sp_AddCustomerAccount",
            "@BranchID", cmbBranch.SelectedValue,
            "@AccountID", txtAccountID.Text,
            "@UserID", txtuserID.Text.Replace("'", "''"),
            "@CurrnceyID", txtCurrencyID.Text,
            "@NickName", txtName.Text);
        if (strMsg != "")
        {

            lblMessageLine.Text = strMsg.ToString();
            return;
        }
        BindDate(txtuserID.Text.Replace("'", "''"));
        strMsg = "";
        HFCPortal.SaveActivity SaveUserA = new SaveActivity();
        SaveUserA.Activity = "Link Account:" + txtAccountID.Text + ":To UserID:" + txtuserID.Text + ",Successfully.";
        if (SaveUserA.SaveUserActivity(ref strMsg) != "")
        {
            lblMessageLine.Text = strMsg;
            return;
        }
        SaveUserA = null;

        fn = null;
        DataClass = null;
        btnAdd.Enabled = false;
        ClearFields();
        lblMessageLine.Text = "Account successfully linked.";

    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (txtAccountID.Text == "")
        {
            lblMessageLine.ForeColor = System.Drawing.Color.Red;
            lblMessageLine.Text = "Please select record to delete";
        }
        else
        {
            SqlConnection Connection;
            SqlCommand Command;
            string strUpdate = "";
            int intUpdateCount;

            Connection = new SqlConnection(Session["BankingAtHomeConnection"].ToString());

            Command = new SqlCommand(strUpdate, Connection);
            strUpdate = "DELETE PW2 WHERE UserID = @UserID AND OurBranchID = @OurBranchID AND AccountID = @Account";
            Command.CommandText = strUpdate;

            Command.Parameters.AddWithValue("@UserID", txtuserID.Text);
            Command.Parameters.AddWithValue("@OurBranchID", cmbBranch.Text);
            Command.Parameters.AddWithValue("@Account", txtAccountID.Text);

            Connection.Open();

            intUpdateCount = Command.ExecuteNonQuery();
            Connection.Close();

            //grdAccounts.DataBind();
            ClearFields();
            lblMessageLine.ForeColor = System.Drawing.Color.Blue;
            lblMessageLine.Text = "Account deleted successfully";
            
            btnDelete.Enabled = false;
            strMsg = "";
            SaveActivity SaveUserActivity = new SaveActivity();
            SaveUserActivity.Activity = "Deleted UserID" + txtAccountID.Text;
            if (SaveUserActivity.SaveUserActivity(ref strMsg) != "")
            {
                return;
            }
            SaveUserActivity = null;
        }
    }
    protected void btnCancel_Click1(object sender, EventArgs e)
    {
        strMsg = "";
        SaveActivity SaveUserActivity = new SaveActivity();
        SaveUserActivity.Activity = "Exited Add Accounts Page";
        if (SaveUserActivity.SaveUserActivity(ref strMsg) != "")
        {
            return;
        }
        SaveUserActivity = null;
        InjectScriptLabel.Text = "<script>CloseOnReload()</" + "script>";
        //Response.Redirect("main.aspx");
    }
    protected void ShowUsers(Object sender, DataGridCommandEventArgs e)
    {

    }

    protected void ClearFields()
    {
        Functions fn = new Functions();
        fn.ClearFields(this.Page);
    }


    protected void txtAccountID_TextChanged1(object sender, EventArgs e)
    {


    }


    protected void BindDate(string strUserID)
    {
        lblMessageLine.Text = "";
        Functions fn = new Functions();
        DataAccess DataClass = new DataAccess();
        DataTable dtResult = DataClass.GetDataTable(ref strMsg, "sp_GetLinkedAccounts", "@UserID", strUserID);
        if (strMsg != "")
        {
            lblMessageLine.Text = strMsg;
            return;
        }
        //Insert currency name
        dtResult.Columns.Add("CurrencyName");

        if (hfcs == null)
        {
            hfcs = new BRInvestmentPortalService();
        }

        try
        {
            foreach (DataRow dr in dtResult.Rows)
            {
                dr["CurrencyName"] = hfcs.GetCurrencyName(dr["Currency"].ToString());
            }
        }
        catch (System.Net.Sockets.SocketException ex)
        {
            lblMessageLine.Text = "Please ensure that the web service is running.";
        }
        catch (Exception ex)
        {
            if (ex.Message.ToLowerInvariant() == ("Unable to connect to the remote server").ToLowerInvariant())
            {
                fn.logError(ex.Message);
                lblMessageLine.ForeColor = System.Drawing.Color.Red;
                lblMessageLine.Text = "Please ensure that the web service is running.";
                return;
            }
            else
            {
                throw ex;
            }

        }

        RadGrid1.DataSource = dtResult;
        RadGrid1.DataBind();

        DataClass = null;
        fn = null;
    }
    protected void GetUserDetails()
    {
        string strUserID = txtuserID.Text;

        lblMessageLine.Text = "";

        DataAccess DataClass = new DataAccess();
        DbDataReader rsResult = DataClass.GetDBResults(ref strMsg, "sp_getUserDetails", "@UserID", txtuserID.Text.Replace("'", "''"));
        if (rsResult.Read())
        {
            txtUsername.Text = rsResult["FullName"].ToString();
            txtAccountID.Enabled = true;
        }
        else
        {
            lblMessageLine.Text = "Invalid User ID.";
            txtUsername.Text = "";
        }


    }

    protected void grdAccounts_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void Btn_Click(object sender, EventArgs e)
    {
        DataAccess DataClass = new DataAccess();
        DbDataReader rsResult = DataClass.GetDBResults(ref strMsg, "sp_AddEditDefaultChargeAcc",
            "@UserId", txtuserID.Text,
            "@DefaultChargeAccount", txtAccountID.Text,
            "@DefaultChargeAccountBranchID", cmbBranch.Text);
        if (strMsg != "")
        {
            lblMessageLine.Text = strMsg; lblMessageLine.ForeColor = System.Drawing.Color.Red; return;
        }
        lblMessageLine.ForeColor = System.Drawing.Color.Blue;
        lblMessageLine.Text = "Default Charge Account Set Successfully for UserID-" + txtuserID.Text;
        GetUserDetails();

    }

    protected void txtuserID_TextChanged1(object sender, EventArgs e)
    {

        GetUserDetails();
        BindDate(txtuserID.Text.Replace("'", "''"));

    }
    protected void txtAccountID_TextChanged(object sender, EventArgs e)
    {
        strMsg = "";
        lblMessageLine.Text = "";
        txtName.Text = "";
        txtCurrencyID.Text = "";
        btnAdd.Enabled = false;
        
        if (hfcs == null)
        {
            hfcs = new BRInvestmentPortalService();
        }
        DataTable dt = null;
        string[] spParams = {"@OurBranchID", cmbBranch.SelectedItem.Value ,"@AccountID", txtAccountID.Text};

        try
        {
           dt = hfcs.GetClientAccount(ref strMsg, spParams);
        }

        catch (System.Net.Sockets.SocketException ex)
        {
            lblMessageLine.Text = "Please ensure that the web service is running.";
        }
        catch (Exception ex)
        {
            if (ex.Message.ToLowerInvariant() == ("Unable to connect to the remote server").ToLowerInvariant())
            {
                fn.logError(ex.Message);
                lblMessageLine.ForeColor = System.Drawing.Color.Red;
                lblMessageLine.Text = "Please ensure that the web service is running.";
                return;
            }
            else
            {
                throw ex;
            }

        }
        

        if (strMsg != "")
        {
            lblMessageLine.Text = strMsg;
            lblMessageLine.ForeColor = System.Drawing.Color.Red;
            return;
        }
        if (dt.Rows.Count > 0)
        {
            txtName.Text = dt.Rows[0]["AccountName"].ToString();
            txtCurrencyID.Text = dt.Rows[0]["CurrencyID"].ToString();
            txtCurrencyName.Text = dt.Rows[0]["CurrencyName"].ToString();
            btnAdd.Enabled = true;
        }
    }
    protected void Accounts_ItemCommand(object source, GridCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            btnAdd.Enabled = false;
            btnDelete.Enabled = true;
            //txtuserID.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FullName"].ToString();
            txtUsername.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FullName"].ToString();
            cmbBranch.SelectedValue = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OurBranchID"].ToString();
            txtAccountID.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AccountID"].ToString();
            txtName.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["NickName"].ToString();
            txtCurrencyID.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Currency"].ToString();
        }
    }
}
