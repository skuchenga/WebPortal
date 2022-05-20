using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HFCPortal.Common;

public partial class frmAMEmail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //From Client ID
            txtFromClientID.spName = "help_GetClientID";
            txtFromClientID.SelectColumn = "ClientID";
            txtFromClientID.Param1 = "ClientID";
            txtFromClientID.Param2 = "ClientType";
            txtFromClientID.Param3 = "ClientName";
            txtFromClientID.Text = "0";

            //To Client ID
            txtToClientID.spName = "help_GetClientID";
            txtToClientID.SelectColumn = "ClientID";
            txtToClientID.Param1 = "ClientID";
            txtToClientID.Param2 = "ClientType";
            txtToClientID.Param3 = "ClientName";
            txtToClientID.Text = "Z";
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        //java-script
        string adminJs = CommonHelper.GetSystemLocation() + "Scripts/admin.js";
        Page.ClientScript.RegisterClientScriptInclude(adminJs, adminJs);

        base.OnPreRender(e);
    }

    protected void gvClientTypes_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
}