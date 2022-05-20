using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HFCPortal.Common;
using BRIMSPortalWebService;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel;

public partial class Modules_F2Help : System.Web.UI.UserControl
{

    BRInvestmentPortalService hfcs;
    static DataTable dtF2Results;
    String strMsg = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                BindGrid();

            }
            catch (Exception exc)
            {
                pnlError.Visible = true;
                lErrorTitle.Text = exc.Message;
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindGrid();
        }
        catch (Exception exc)
        {
            pnlError.Visible = true;
            lErrorTitle.Text = exc.Message;
        }
    }



    protected override void CreateChildControls()
    {

        createDynamicControls();

        base.CreateChildControls();
        ChildControlsCreated = true;
    }

    protected void createDynamicControls()
    {
        try
        {

            var tbl = new Table();
            tbl.ID = "tblControls";
            tbl.CssClass = "adminContent";

            List<string> lstCols = this.cols();

            foreach (string par in lstCols)
            {
                var tr1 = new TableRow();
                var tcAdminTitle = new TableCell();
                var tcAdminData = new TableCell();

                tcAdminTitle.CssClass = "adminTitle";
                tcAdminData.ID = string.Format("tc{0}", par);
                tcAdminData.CssClass = "adminData";

                var lbl = new Label();
                var txt = new TextBox();

                lbl.ID = string.Format("lbl{0}", par);
                lbl.Text = string.Format("{0}: ", par);
                txt.ID = string.Format("txt{0}", par);



                tcAdminTitle.Controls.Add(lbl);
                tcAdminData.Controls.Add(txt);
                tr1.Controls.AddAt(0, tcAdminTitle);
                tr1.Controls.AddAt(1, tcAdminData);

                tbl.Controls.Add(tr1);
            }


            phControls.Controls.Add(tbl);

        }
        catch (Exception exc)
        {
            pnlError.Visible = true;
            lErrorTitle.Text = exc.Message;
        }
    }

    private List<string> cols()
    {
        List<string> cols = new List<string>();

        if (!string.IsNullOrEmpty(Columns))
        {
            cols = this.Columns.Split(';').ToList();
        }
        return cols;
    }

    protected override void OnPreRender(EventArgs e)
    {
        CommonHelper.BindJQuery(this.Page);

        base.OnPreRender(e);
    }


    public string GetTextboxValue(string textboxID)
    {
        try
        {

        
        if (string.IsNullOrEmpty(textboxID))
        {
            return string.Empty;
        }

        TextBox txtBox = this.findControl(pnlData, textboxID) as TextBox;

        if (txtBox != null)
        {
            return txtBox.Text.Trim();
        }
        else
        {
            return string.Empty;
        }

        }
        catch (Exception exc)
        {
            pnlError.Visible = true;
            lErrorTitle.Text = exc.Message;
            throw exc;
        }         
    }

    private Control findControl(Control source, string ctrlId)
    {
        try
        {

            Control found = null;

            foreach (Control c in source.Controls)
            {
                found = c.FindControl(ctrlId);
                if (found == null)
                {
                    findControl(c, ctrlId);
                }
                else
                {
                    return found;
                }
            }

            return found;
        }
        catch (Exception exc)
        {
            pnlError.Visible = true;
            lErrorTitle.Text = exc.Message;
            throw exc;
        }
    }
    

    public string Columns
    {
        get
        {
            return CommonHelper.QueryString("cols");
        }
    }

    public string txtClientID
    {
        get
        {
            return CommonHelper.QueryString("txt");
        }
    }

    public string sp
    {
        get
        {
            return CommonHelper.QueryString("sp");
        }
    }

    public string SelectColumn
    {
        get
        {
            return CommonHelper.QueryString("sc");
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

    #region Gridview

    protected void BindGrid()
    {

        try
        {

            if (string.IsNullOrEmpty(this.sp))
            {
                lErrorTitle.Text = "F2 Control generic error. Please contact your system administrator.";
                pnlError.Visible = true;
                btnSearch.Visible = false;
                return;
            }

            //Goes through web service

            List<string> spParams = new List<string>();

            List<string> lstCols = this.cols();

            foreach (string par in lstCols)
            {

                spParams.Add(string.Format("@{0}", par));
                if (par.ToUpper() == "OURBRANCHID")
                {
                    spParams.Add(this.OurBranchID);
                }
                else
                {
                spParams.Add(GetTextboxValue(string.Format("txt{0}", par)));                       
                }
            }
            if (hfcs == null)
            {
                hfcs = new BRInvestmentPortalService();
            }

            try
            {
                dtF2Results = hfcs.BRIMSData(ref strMsg, this.sp, spParams.ToArray());

                //Bind grid
                if (dtF2Results.Rows.Count > 0)
                {
                    //Swap select column
                    if (dtF2Results.Columns[this.SelectColumn].Ordinal != 0)
                    {
                        dtF2Results.Columns[this.SelectColumn].SetOrdinal(0);
                    }
                    this.gvRecords.Visible = true;
                    this.pnlError.Visible = false;
                    this.gvRecords.DataSource = dtF2Results;
                    this.gvRecords.DataBind();

                }
                else
                {
                    this.gvRecords.Visible = false;
                    this.pnlError.Visible = true;
                    this.lErrorTitle.Text = "No records found.";
                }
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                this.pnlError.Visible = true;
                this.lErrorTitle.Text = "Please ensure that the web service is running.";
            }
        }
        catch (Exception exc)
        {
            pnlError.Visible = true;
            lErrorTitle.Text = exc.Message;
        }

    }

    protected void gvRecords_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        try
        {

        if (e.CommandName == "cmdSelect")
        {
            string strEv = e.CommandArgument.ToString();
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "closerefresh", "<script language=javascript>try {window.opener.document.forms[0]." + this.txtClientID + ".value='" + strEv + "';}catch (e){} window.close();</script>");
        }
        }
        catch (Exception exc)
        {
            pnlError.Visible = true;
            lErrorTitle.Text = exc.Message;
        }
        
    }

    protected void gvRecords_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            int intF2Cols = columnCount();

            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < intF2Cols; i++)
                {
                    e.Row.Cells[i].Text = dtF2Results.Columns[i].ColumnName;
                    e.Row.Cells[i].Visible = true;
                    TemplateControl tc = e.Row.Cells[i].TemplateControl;
                    gvRecords.Columns[i].Visible = true;
                    Control dcfc = e.Row.Cells[i].Parent;
                }
            }
            else
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        DataRowView dr = (DataRowView)e.Row.DataItem;


                        for (int i = 0; i < intF2Cols; i++)
                        {
                            if (i == 0)
                            {
                                LinkButton btnSelect = e.Row.FindControl("btnSelect") as LinkButton;
                                if (btnSelect != null)
                                {
                                    btnSelect.Text = dr[this.SelectColumn].ToString();
                                    btnSelect.CommandArgument = dr[this.SelectColumn].ToString();
                                }
                            }
                            else
                            {
                                string strColName = this.colName(i);
                                if (!string.IsNullOrEmpty(strColName))
                                {
                                    Label lblColumn = e.Row.FindControl(string.Format("lblCol{0}", i)) as Label;
                                    if (lblColumn != null)
                                    {
                                        lblColumn.Text = dr[strColName].ToString();
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }
        catch (Exception exc)
        {
            pnlError.Visible = true;
            lErrorTitle.Text = exc.Message;
        }
    }

    protected void gvRecords_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {

        this.gvRecords.PageIndex = e.NewPageIndex;
        BindGrid();
        }
        catch (Exception exc)
        {
            pnlError.Visible = true;
            lErrorTitle.Text = exc.Message;
        }
    }

    private int columnCount()
    {
        if (dtF2Results != null)
        {
           return dtF2Results.Columns.Count;
        }
        return -1;
    }

    private string colName(int index)
    {
        if (dtF2Results == null)
        {
            return string.Empty;
        }
        if (index > (dtF2Results.Columns.Count - 1))
        {
            return string.Empty;
        }
        else
        {
            return dtF2Results.Columns[index].ColumnName;
        }
    }
    


    #endregion
}