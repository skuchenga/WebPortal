using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BRIMSPortalWebService;
using System.Data;
using System.ComponentModel;

public partial class Modules_BRIMSClientTypes : System.Web.UI.UserControl
{
    BRInvestmentPortalService hfcs;
    static DataTable dtBRIMSData;
    String strMsg = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindGrid();
        }
    }

    protected void BindGrid()
    {
        try
        {
            if (hfcs == null)
            {
                hfcs = new BRInvestmentPortalService();
            }

            string[] spParams = { "@OurBranchID", this.OurBranchID };

            if (hfcs == null)
            {
                hfcs = new BRInvestmentPortalService();
            }

           dtBRIMSData = hfcs.BRIMSData(ref strMsg, "sp_GetAMClientTypes", spParams);

            if (!string.IsNullOrEmpty(strMsg))
            {
                pnlError.Visible = true;
                pnlAMClient.Visible = false;
                lErrorTitle.Text = "Client Types " + strMsg ;
                return;
            }

            if (dtBRIMSData.Rows.Count > 0)
            {
                this.gvAMClientTypes.Visible = true;
                this.pnlError.Visible = false;
                this.gvAMClientTypes.DataSource = dtBRIMSData;
                this.gvAMClientTypes.DataBind();

            }
            else
            {
                this.gvAMClientTypes.Visible = false;
                this.pnlError.Visible = true;
                this.lErrorTitle.Text = "No client types exist.";
            }
        }
        catch (Exception exc)
        {
            throw exc;
        }
    }

    protected void gvAMClientTypes_RowDataBound(object sender, GridViewRowEventArgs e)
    { 
    
    }

    protected void gvAMClientTypes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.gvAMClientTypes.PageIndex = e.NewPageIndex;
        BindGrid();
    }


    #region Properties

    public string SelectedClientTypes
    {
        get
        {
            string clientTypes = string.Empty;
            foreach (GridViewRow row in gvAMClientTypes.Rows)
            {
                var cbClientType = row.FindControl("cbClientType") as CheckBox;
                var hfClientTypeId = row.FindControl("hfClientTypeId") as HiddenField;

                bool isChecked = cbClientType.Checked;
                string clientTypeId = hfClientTypeId.Value;
                if (isChecked)
                {
                    if (string.IsNullOrEmpty(clientTypes))
                    {
                        clientTypes = clientTypeId;
                    }
                    else
                    {
                        clientTypes = clientTypes + "," +  clientTypeId;                        
                    }
                }
            }
            return clientTypes;
        }
    }

    public List<string> SelectedClientTypesList
    {
        get
        {
            List<string> clientTypes = new List<string>();
            foreach (GridViewRow row in gvAMClientTypes.Rows)
            {
                var cbClientType = row.FindControl("cbClientType") as CheckBox;
                var hfClientTypeId = row.FindControl("hfClientTypeId") as HiddenField;

                bool isChecked = cbClientType.Checked;
                string clientTypeId = hfClientTypeId.Value;
                if (isChecked)
                {
                    clientTypes.Add(clientTypeId);
                }
            }
            return clientTypes;
        }
    }

    [DefaultValue("001")]
    public string OurBranchID
    {
        get
        {
            object obj2 = this.ViewState["OurBranchID"];
            if (obj2 != null)
                return (string)obj2;
            else
                return string.Empty;
        }
        set
        {
            this.ViewState["OurBranchID"] = value;
        }
    }

    #endregion

}