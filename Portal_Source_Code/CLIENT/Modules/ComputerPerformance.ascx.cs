using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Office.Interop;
using HFCPortal.PerformanceFunds;
using HFCPortal;


public partial class ComputerPerformance : System.Web.UI.UserControl
{

    PerformanceFunds pfs = new PerformanceFunds();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack ){
            lblError.Attributes.Add("style", "display: none;");
            DateTime fundDate = pfs.FundStartDate(038);

            dtReportFrom.SelectedDate = fundDate;
        }
    }

    protected void btnGenerateReport_Click(object sender, EventArgs e)
    {
        try
        {
        Nullable<DateTime> startDate = dtReportFrom.SelectedDate;
        Nullable<DateTime> endDate = dtReportTo.SelectedDate;

        lblError.Attributes.Add("style", "display: none;");
        lblError.Attributes.Add("style", "display: none;");
        if (!startDate.HasValue) {
           throw new ArgumentException("Please select the start date");
        }

        if (!endDate.HasValue) {
            throw new ArgumentException("Please select the end date");
        }

        DateTime fStartDate = pfs.FundStartDate(038);

        if (startDate < fStartDate)
        {
            throw new ArgumentException("The reporting start date should not be greater than than fund start date i.e. " + fStartDate.Date.ToShortDateString());
        }

        //Check max Fund date
        DateTime fEndDate = pfs.FundEndDate(038);

        if (endDate > fEndDate)
        {
            throw new ArgumentException("The reporting end date should not be greatr than the last transaction date i.e. " + fEndDate.Date.ToShortDateString());
        }

        bool result = pfs.ModifiedDeitzReport(startDate,endDate);

        }
        catch (Exception exc)
        {
            lblError.Text = exc.Message;
            lblError.Attributes.Add("style", "display: '';");
        }
    }
    
}   