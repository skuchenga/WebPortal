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
using System.Web.UI.Adapters;
using HFCPortal;

public partial class CreateAdministrators : System.Web.UI.Page
{
    Double Pass;
    DateTime UpdatedOn;
    DateTime ctrldate = DateTime.Now;
    string strMsg = "";

    DataAccess DataClass;
    Functions fn;

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

        loadadministrators();


    }

    protected void btnadd_Click(object sender, EventArgs e)
    {
        Page.Validate();
        if (!(Page.IsValid))
            return;
        DataClass = new DataAccess();


        fn = new Functions();
        Pass = fn.EncryptUserPassword(txtconfipassword.Text, ctrldate);


        DbDataReader rs = DataClass.GetDBResults(ref strMsg, "sp_AddEditAdmin",
        "@OurBranchID", "",
        "@ModuleID", "015",
        "@UserID", txtUserID.Text.ToUpper().Replace("'", "''"),
        "@FullName", txtFullname.Text.ToUpper().Replace("'", "''"),
        "@Email", txtemail.Text,
        "@Password", Pass,
        "@IsAdmin", 0,
        "@IsEnabled", 0,
        "@FirstLogin", 1,
        "@IsSupervisionRequired", 1,
        "@OperatorID", "admin",
        "@CreatedOn", ctrldate,
        "@NewRecord", 1);

        if (strMsg != "")
        {
            lblmessageline2.Text = strMsg;
            return;
        }


        btnadd.Enabled = false;

        loadadministrators();
        Btnsavechanges.Enabled = false;
        //CheckBox4.Checked = false;
        SaveActivity SaveUserActivity = new SaveActivity();
        SaveUserActivity.Activity = "Added New Admin:" + txtUserID.Text + "Name" + txtFullname.Text;
        if (SaveUserActivity.SaveUserActivity(ref strMsg) != "")
        {
            return;
        }
        SaveUserActivity = null;
        clear();
    }

    protected void btnback_Click(object sender, EventArgs e)
    {
        Response.Redirect("mainmenu.aspx");
    }

    protected void Btnsavechanges_Click(object sender, EventArgs e)
    {

        Page.Validate();
        if (!(Page.IsValid))
            return;
        DataClass = new DataAccess();


        fn = new Functions();
        Pass = fn.EncryptUserPassword(txtconfipassword.Text, ctrldate);


        DbDataReader rs = DataClass.GetDBResults(ref strMsg, "sp_AddEditAdmin",
        "@OurBranchID", "",
        "@ModuleID", "015",
        "@UserID", txtUserID.Text.ToUpper().Replace("'", "''"),
        "@FullName", txtFullname.Text.ToUpper().Replace("'", "''"),
        "@Email", txtemail.Text,
        "@Password", Pass,
        "@IsAdmin", 0,
        "@IsEnabled", 0,
        "@FirstLogin", 1,
        "@IsSupervisionRequired", 1,
        "@OperatorID", "admin",
        "@CreatedOn", ctrldate,
        "@NewRecord", 0);

        if (strMsg != "")
        {
            lblmessageline2.Text = strMsg;
            return;
        }


        btnadd.Enabled = false;

        loadadministrators();
        Btnsavechanges.Enabled = false;
        //CheckBox4.Checked = false;
        SaveActivity SaveUserActivity = new SaveActivity();
        SaveUserActivity.Activity = "Edit Admin:" + txtUserID.Text + "Name" + txtFullname.Text;
        if (SaveUserActivity.SaveUserActivity(ref strMsg) != "")
        {
            return;
        }
        SaveUserActivity = null;
        clear();
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        SaveActivity SaveUserActivity = new SaveActivity();
        SaveUserActivity.Activity = "Failuire to delete Admin:" + txtUserID.Text + ":Function Not Available.";
        if (SaveUserActivity.SaveUserActivity(ref strMsg) != "")
        {
            return;
        }
        SaveUserActivity = null;
        if (txtUserID.Text == "")
        {
            lblMessageLine.ForeColor = System.Drawing.Color.Red;
            lblMessageLine.Text = "Select Admin to delete";
            return;
        }
        DataAccess DataClass = new DataAccess();
        DbDataReader rs = DataClass.GetDBResults(ref strMsg, "sp_Delete_Users", "@OperatorID", txtUserID.Text);
        lblMessageLine.ForeColor = System.Drawing.Color.Red;
        lblMessageLine.Text = "Admin deleted successfully.";

        clear();
    }
    protected void btnclear_Click(object sender, EventArgs e)
    {
        clear();
    }
    protected void clear()
    {
        Functions fn = new Functions();
        fn.ClearFields(this.Page);
        btnadd.Enabled = true;
        Btnsavechanges.Enabled = false;
        btnResetpwd.Enabled = false;
        btndelete.Enabled = false;
        txtUserID.Enabled = true;
    }

    protected void Showmail(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            //cboBranch.Enabled = false;
            btnadd.Enabled = false;
            btndelete.Enabled = true;
            btnResetpwd.Enabled = true;
            Btnsavechanges.Enabled = true;
            Session["username"] = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserID"].ToString();
            txtUserID.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserID"].ToString();
            txtemail.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Email"].ToString();
            txtFullname.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FullName"].ToString();
            //cboBranch.SelectedValue = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Branch_ID"].ToString();
        }
        catch (System.Exception ex)
        {
            lblMessageLine.Text = ex.Message;
        }


    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        strMsg = "";
        lblMessageLine.Text = "";
        fn = new Functions();
        DataClass = new DataAccess();
        DbDataReader rs = DataClass.GetDBResults(ref strMsg, "sp_ResetAdminPassword",
            "@UserID", txtUserID.Text.Replace("'", "''"),
            "@Password", fn.EncryptUserPassword(txtpassword.Text, ctrldate),
            "@Updatedon", ctrldate,
            "@OperatorID", Session["LoginID"].ToString());
        if (strMsg != "")
        {
            fn.logError(strMsg);
            lblMessageLine.Text = "sorry an error occurred";
            return;
        }

        lblMessageLine.Text = "User password re-set successfully.";

    }
    protected void loadadministrators()
    {
        lblmessageline2.Text = "";
        fn = new Functions();
        strMsg = "";
        lblmessageline2.Text = "";
        try
        {
            DataClass = new DataAccess();
            DataSet ds = DataClass.GetDBResultsDS("SP_AddEditShow_Admins");
            grdusers.DataSource = ds;
            grdusers.Rebind();
        }
        catch (System.Exception ex)
        {
            fn.logError(ex.Message);
            lblmessageline2.Text = "Sorry an error occurred. Cannot load existing administrators.";
        }
    }
    protected void txtUserID_TextChanged(object sender, EventArgs e)
    {
        lblMessageLine.Text = "";
        fn = new Functions();
        DataClass = new DataAccess();
        DbDataReader rs = DataClass.GetDBResults(ref strMsg, "sp_getAdminInfo", "@UserID", txtUserID.Text.Replace("'", "''"));
        if (strMsg != "")
        {
            fn.logError(strMsg);
            lblMessageLine.Text = "Sorry an error occurred";
            return;
        }
        if (rs.Read())
        {
            btnadd.Enabled = false;
            Btnsavechanges.Enabled = true;
            btndelete.Enabled = true;
            btnResetpwd.Enabled = true;
            txtUserID.Enabled = false;
            txtUserID.Text = rs["UserID"].ToString();
            txtFullname.Text = rs["FullName"].ToString();
            txtemail.Text = rs["Email"].ToString();
            CheckBox1.Checked = bool.Parse(rs["Disabled"].ToString());
        }
        rs.Close();
        rs.Dispose();
        DataClass = null;
    }
    protected void grdusers_ItemCommand(object source, GridCommandEventArgs e)
    {
        lblMessageLine.Text = "";
        fn = new Functions();

        try
        {


            if (e.CommandName == "Select")
            {
                btnadd.Enabled = false;
                Btnsavechanges.Enabled = true;
                btndelete.Enabled = true;
                btnResetpwd.Enabled = true;
                txtUserID.Enabled = false;

                txtUserID.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserID"].ToString();
                txtFullname.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FullName"].ToString();
                txtemail.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Email"].ToString();
                CheckBox1.Checked = bool.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Disabled"].ToString());


            }
        }
        catch (System.Exception ex)
        {
            lblMessageLine.Text = "Sorry an error occurred";
            fn.logError(ex.Message);
        }
    }
}
