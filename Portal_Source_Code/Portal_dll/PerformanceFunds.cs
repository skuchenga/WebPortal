using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Office.Interop.Excel;
using System.Text.RegularExpressions;

namespace HFCPortal.PerformanceFunds
{
    public class PerformanceFunds
    {
        private Functions fn;
        //private DataAccess.DataAccessCore Core;
        string DataProviderName = "System.Data.SqlClient";
        String strMsg = "";
        string StrClientID = null;

        System.Data.DataTable rsTwo = new System.Data.DataTable();
        DbConnection conCurrency;
        DbDataReader rdCurrency = null;
        DbCommand cnCurrency;

        public bool ModifiedDeitzReport(Nullable<DateTime> startDate,Nullable<DateTime> endDate)
        {
            bool functionReturnValue = false;
            try
            {
            Application objXL = new Application();
            Workbook wbXL = null;
            Worksheet wsXL = null;
            int TheRows = 0;
            int tCol = 0;
            string HedaTitle = null;
            int counter = 0;

            //XL Format values
            const XlHAlign xlCenter = XlHAlign.xlHAlignCenter;
            const XlLineStyle vbBSSolid = XlLineStyle.xlContinuous;
            const XlLineStyle xlNone = XlLineStyle.xlLineStyleNone;
            const XlBordersIndex xlEdgeTop = XlBordersIndex.xlEdgeTop;
            const XlBordersIndex xlEdgeLeft = XlBordersIndex.xlEdgeLeft;
            const XlBorderWeight xlThick = XlBorderWeight.xlThick;

            //Chart
            const XlAxisType xlValue = XlAxisType.xlValue;
            const XlCategoryType xlCategory = XlCategoryType.xlCategoryScale;
            //Color consts
            Color vbGreen = Color.Lime;
            Color vbRed = Color.Red;
            Color vbYellow = Color.Yellow;
            Color vbWhite = Color.White;

            //General consts
            string cNo_Text = "";

            string FundLong = null;
            string FundShort = null;
            System.Data.DataTable rsTransactions = new System.Data.DataTable();
            string sqlTwo = string.Empty;

            if ((objXL == null))
            {
                throw new ArgumentException("You need Microsoft Excel to use this function");
            }

            cnCurrency.Connection = conCurrency;

            //rdCurrency = Core.GetDBResults(ref strMsg, "sp_GetClientDetails", "@ClientID", "C0038");
            
            //rdCurrency = Core.GetDBResults(ref strMsg, "Sp_GeneratePerformance", "@OurBranchID", "001", "@ClientID", "C0038", "@FromDate", startDate, "@ToDate", endDate);

            rsTwo.Rows.Clear();

            rsTwo.Load(rdCurrency);

            dynamic _with1 = rsTwo;
            if (rsTwo.Rows.Count < 1)
            {
                throw new ArgumentException("Unable to retrieve fund information.");
            }
            else
            {
                dynamic _with2 = rsTwo.Rows[0];
                StrClientID = (string)_with2["ClientID"];
                FundLong = (string)_with2["ClientName"];
                FundShort = Convert.ToString(_with2["clientnameshort"]).Replace("/", "").Replace("*", "").Replace("\\", "");
            }

           //rdCurrency = Core.GetDBResults(ref strMsg, "Sp_GeneratePerformance", "@OurBranchID", "001", "@ClientID", StrClientID, "@FromDate", startDate, "@ToDate", endDate);

           rdCurrency = cnCurrency.ExecuteReader(CommandBehavior.CloseConnection);

            System.Data.DataTable rsUnfilteredTrans = new System.Data.DataTable();
            rsUnfilteredTrans.Load(rdCurrency);

            rsUnfilteredTrans.DefaultView.RowFilter = "IsGraphData = 0";
            rsTransactions = rsUnfilteredTrans.DefaultView.ToTable();

            dynamic _with3 = rsTwo;
            if (rsTwo.Rows.Count < 1)
            {
                throw new ArgumentException("Unable to retrieve fund information.");
            }

            // open Excel
            objXL.Visible = false;
            objXL.DisplayAlerts = false;

            //add a workbook
            wbXL = objXL.Workbooks.Add();

            //add a worksheet
            objXL.Worksheets.Add();
            wsXL = objXL.ActiveSheet;
            wsXL.Name = FundShort + " MODIFIED DIETZ";
            HedaTitle = "PORTFOLIO TOTAL RETURN WITH ASSET CLASSES";

            int Cntr = 0;
            int RowConst = 0;
            RowConst = 7;
            TheRows = rsTransactions.Rows.Count;

             dynamic _wsXL = wsXL;
            _wsXL.Range("A1", "E1").Merge();
            _wsXL.Range("A1", "E1").Font.Size = 14;
            _wsXL.Range("A1", "E1").Font.Bold = true;

            _wsXL.Range("A2", "E2").Merge();
            _wsXL.Range("A2", "E2").Font.Size = 14;
            _wsXL.Range("A2", "E2").Font.Bold = true;

            _wsXL.Range("A3", "E3").Merge();
            _wsXL.Range("A3", "E3").Font.Bold = true;

            _wsXL.Cells[1, 1].Value = HedaTitle;
            _wsXL.Cells[2, 1].Value = FundLong;
            _wsXL.Cells[3, 1].Value = "Report Generated on " + DateTime.UtcNow.Date.ToShortDateString() + " at " + DateTime.UtcNow.ToShortTimeString();
            _wsXL.Cells[6, 1].Value = "Asset Class Movement and Return";
            _wsXL.Cells[6, 1].HorizontalAlignment = xlCenter;
            _wsXL.Cells[6, 2].Value = "Equity";
            _wsXL.Cells[6, 2].HorizontalAlignment = xlCenter;
            _wsXL.Cells[6, 3] = "Fixed Income";
            _wsXL.Cells[6, 3].HorizontalAlignment = xlCenter;
            _wsXL.Cells[6, 4] = "Offshore";
            _wsXL.Cells[6, 4].HorizontalAlignment = xlCenter;
            _wsXL.Cells[6, 5] = "Total Value";
            _wsXL.Cells[6, 5].HorizontalAlignment = xlCenter;

            dynamic _with5 = _wsXL.Range("A6:E6");
            _with5.Font.Bold = true;
            _with5.Borders.LineStyle = vbBSSolid;
            _with5.Borders.Weight = xlThick;

            foreach (DataRow row in rsTransactions.Rows)
            {
                if (row.IsNull("Equity"))
                {
                    _wsXL.Cells[counter + RowConst, 1].Value = cNo_Text;
                    _wsXL.Cells[counter + RowConst, 2] = cNo_Text;
                }
                else
                {
                    if ((String)row["AssetDetails"] == cNo_Text)
                    {
                        _wsXL.Cells[counter + RowConst, 1] = ((DateTime)row["TransDate"]).Date.ToShortDateString();
                    }
                    else
                    {
                        _wsXL.Cells[counter + RowConst, 1] = row["AssetDetails"];
                    }


                    if (Convert.ToDouble(row["Equity"]) < 0)
                    {
                        wsXL.Cells[counter + RowConst, 2].Font.Color = vbRed;
                    }
                    _wsXL.Cells[counter + RowConst, 2] = (Convert.ToDouble(row["Equity"]) != 0 ? row["Equity"] : "");
                    _wsXL.Cells[counter + RowConst, 2].NumberFormat = "0.00";

                    if (Convert.ToDouble(row["FI"]) < 0)
                    {
                        wsXL.Cells[counter + RowConst, 3].Font.Color = vbRed;
                    }
                    _wsXL.Cells[counter + RowConst, 3] = (Convert.ToDouble(row["FI"]) != 0 ? row["FI"] : "");
                    _wsXL.Cells[counter + RowConst, 3].NumberFormat = "0.00";

                    if (Convert.ToDouble(row["OffShore"]) < 0)
                    {
                        wsXL.Cells[counter + RowConst, 4].Font.Color = vbRed;
                    }
                    _wsXL.Cells[counter + RowConst, 4] = (Convert.ToDouble(row["OffShore"]) != 0 ? row["OffShore"] : "");
                    _wsXL.Cells[counter + RowConst, 4].NumberFormat = "0.00";

                    if (Convert.ToDouble(row["Total"]) < 0)
                    {
                        wsXL.Cells[counter + RowConst, 5].Font.Color = vbRed;
                    }
                    _wsXL.Cells[counter + RowConst, 5] = (Convert.ToDouble(row["Total"]) != 0 ? row["Total"] : "");
                    _wsXL.Cells[counter + RowConst, 5].NumberFormat = "0.00";
                }

                for (Cntr = 1; Cntr <= 6; Cntr++)
                {
                    _wsXL.Cells[counter + RowConst, Cntr].Borders(xlEdgeLeft).Weight = xlThick;
                    _wsXL.Cells[counter + RowConst, Cntr].Borders(xlEdgeLeft).LineStyle = vbBSSolid;
                }


                bool isValid = false;
                if (row.IsNull("isValid"))
                {
                    isValid = true;
                }
                else
                {
                    isValid = Convert.ToBoolean(row["isValid"]);
                }

                if (isValid == false & !string.IsNullOrEmpty((string)row["AssetDetails"]))
                {
                    _wsXL.Range("A" + (counter + RowConst), "E" + (counter + RowConst)).Font.Bold = true;
                    _wsXL.Range("A" + (counter + RowConst), "E" + (counter + RowConst)).Interior.Color = 0xc0c0c0;
                }

                if (row.IsNull("isValid") == true & string.IsNullOrEmpty((string)row["AssetDetails"]))
                {
                    _wsXL.Range("A" + (counter + RowConst), "E" + (counter + RowConst)).Merge();
                    _wsXL.Range("A" + (counter + RowConst), "E" + (counter + RowConst)).Interior.Color = vbWhite;
                    _wsXL.Range("A" + (counter + RowConst), "E" + (counter + RowConst)).Borders.Weight = xlThick;
                    _wsXL.Range("A" + (counter + RowConst), "E" + (counter + RowConst)).Borders.LineStyle = vbBSSolid;
                }

                var weighted = (string)(row["AssetDetails"]);

                if (weighted.Contains("Beginning Weighted Portfolio Return Calculation"))
                {
                   _wsXL.Range("A" + (counter + RowConst), "E" + (counter + RowConst)).Merge();
                   _wsXL.Range("A" + (counter + RowConst), "E" + (counter + RowConst)).Interior.Color = vbYellow;
                   _wsXL.Range("A" + (counter + RowConst), "E" + (counter + RowConst)).Borders.Weight = xlThick;
                   _wsXL.Range("A" + (counter + RowConst), "E" + (counter + RowConst)).Borders.LineStyle = vbBSSolid;
                }

                Regex rgx = new Regex("Cash Flows.*");
                string ac = (string)row["AssetDetails"];

                if (rgx.IsMatch(ac))
                {
                    _wsXL.Range("A" + (counter + RowConst), "E" + (counter + RowConst)).Interior.Color = vbWhite;
                }

                counter += 1;
            }

            //LAST ROW FORMATTING
            dynamic _with6 = wsXL.Range["A" + (RowConst + TheRows), "E" + (RowConst + TheRows)];
            _with6.Borders(xlEdgeTop).LineStyle = vbBSSolid;
            _with6.Borders(xlEdgeTop).Weight = xlThick;

            for (tCol = 1; tCol <= 5; tCol++)
            {
                //autofit
                wsXL.Columns[tCol].AutoFit();
            }

            objXL.ActiveWindow.DisplayGridlines = false;
            objXL.Visible = true;

            //Graph
            objXL.Worksheets.Add();
            wsXL = objXL.ActiveSheet;
            wsXL.Name = FundShort + " MODIFIED DIETZ GRAPH";
            HedaTitle = "PORTFOLIO TOTAL RETURN WITH ASSET CLASSES";

            rsUnfilteredTrans.DefaultView.RowFilter = "";
            rsUnfilteredTrans.DefaultView.RowFilter = "IsGraphData = 1 AND IsFinalGraph = 1";

            rsTransactions.Rows.Clear();
            rsTransactions = rsUnfilteredTrans.DefaultView.ToTable();

            RowConst = 7;
            TheRows = rsTransactions.Rows.Count;

            dynamic _with7 = wsXL;
            _with7.Range("A1", "E1").Merge();
            _with7.Range("A1", "E1").Font.Size = 14;
            _with7.Range("A1", "E1").Font.Bold = true;

            _with7.Range("A2", "E2").Merge();
            _with7.Range("A2", "E2").Font.Size = 14;
            _with7.Range("A2", "E2").Font.Bold = true;

            _with7.Range("A3", "E3").Merge();
            _with7.Range("A3", "E3").Font.Bold = true;

            _with7.Cells[1, 1].Value = HedaTitle;
            _with7.Cells[2, 1].Value = FundLong;
            _with7.Cells[3, 1].Value = "Report Generated on " + DateTime.UtcNow.ToShortDateString() + " at " + DateTime.UtcNow.ToShortTimeString();
            
            _with7.Cells[6, 1] = "";
            _with7.Cells[6, 1].HorizontalAlignment = xlCenter;

            _with7.Cells[6, 2] = "RETURN";
            _with7.Cells[6, 2].HorizontalAlignment = xlCenter;


            dynamic _with8 = _with7.Range("A6:B6");
            _with8.Font.Bold = true;
            _with8.Borders.LineStyle = vbBSSolid;
            _with8.Borders.Weight = xlThick;
            _with8.Interior.Color = vbGreen;

            counter = 0;

            foreach (DataRow trow in rsTransactions.Rows)
            {
                if ((trow.IsNull("Equity")) == true)
                {
                    _with7.Cells[counter + RowConst, 1] = cNo_Text;
                    _with7.Cells[counter + RowConst, 2] = cNo_Text;
                }
                else
                {
                    if (string.IsNullOrEmpty(Convert.ToString(trow["AssetDetails"])))
                    {
                        _with7.Cells[counter + RowConst, 1] = ((DateTime)trow["TransDate"]).Date.ToShortDateString();
                    }
                    else
                    {
                        _with7.Cells[counter + RowConst, 1] = trow["AssetDetails"];
                    }


                    if (Convert.ToDouble(trow["IsFinalGraph"]) == 0)
                    {
                        if (Convert.ToDouble(trow["Total"]) < 0)
                        {
                            wsXL.Cells[counter + RowConst, 2].Font.Color = vbRed;
                        }


                        _with7.Cells[counter + RowConst, 2] = (Convert.ToDouble(trow["Total"]) != 0 ? (trow["Total"]) : "");
                    }
                    else
                    {
                        if (Convert.ToDecimal(trow["MonthlyReturn"]) < 0)
                        {
                            wsXL.Cells[counter + RowConst, 2].Font.Color = vbRed;
                        }

                        _with7.Cells[counter + RowConst, 2] = (Convert.ToDouble(trow["MonthlyReturn"]) != 0 ? (trow["MonthlyReturn"]) : "");
                    }

                    _with7.Cells[counter + RowConst, 2].NumberFormat = "0.00";
                }

                for (Cntr = 1; Cntr <= 3; Cntr++)
                {
                    _with7.Cells[counter + RowConst, Cntr].Borders(xlEdgeLeft).Weight = xlThick;
                    _with7.Cells[counter + RowConst, Cntr].Borders(xlEdgeLeft).LineStyle = vbBSSolid;
                }

                if (Convert.ToBoolean(trow["isValid"]) == false & !string.IsNullOrEmpty((string)trow["AssetDetails"]))
                {
                    _with7.Range("A" + (counter + RowConst), "B" + (counter + RowConst)).Font.Bold = true;
                    _with7.Range("A" + (counter + RowConst), "B" + (counter + RowConst)).Interior.Color = 0xc0c0c0;
                }

                if (Convert.ToBoolean(trow.IsNull("isValid")) == true & string.IsNullOrEmpty((string)trow["AssetDetails"]))
                {
                    _with7.Range("A" + (counter + RowConst), "B" + (counter + RowConst)).Merge();
                    _with7.Range("A" + (counter + RowConst), "B" + (counter + RowConst)).Interior.Color = vbWhite;
                    _with7.Range("A" + (counter + RowConst), "E"  + (counter + RowConst)).Borders.Weight = xlThick;
                    _with7.Range("A" + (counter + RowConst), "B" + (counter + RowConst)).Borders.LineStyle = vbBSSolid;
                }

                if (((string)trow["AssetDetails"]).Contains("Beginning Weighted Portfolio Return Calculation"))
                {
                    _with7.Range("A" + (counter + RowConst), "B" + (counter + RowConst)).Merge();
                    _with7.Range("A" + (counter + RowConst), "B" + (counter + RowConst)).Interior.Color = vbYellow;
                    _with7.Range("A" + (counter + RowConst), "E" + (counter + RowConst)).Borders.Weight = xlThick;
                    _with7.Range("A" + (counter + RowConst), "B" + (counter + RowConst)).Borders.LineStyle = vbBSSolid;
                }

                Regex rgx = new Regex("Cash Flows.*");
                string ac = (string)trow["AssetDetails"];

                if (rgx.IsMatch(ac))
                {
                    _with7.Range("A" + (counter + RowConst), "B" + (counter + RowConst)).Interior.Color = vbWhite;
                }

                counter += 1;
            }

            //LAST ROW FORMATTING
            dynamic _with9 = wsXL.Range["A" + (RowConst + TheRows), "B" + (RowConst + TheRows)];
            _with9.Borders(xlEdgeTop).LineStyle = vbBSSolid;
            _with9.Borders(xlEdgeTop).Weight = xlThick;

            for (tCol = 1; tCol <= 2; tCol++)
            {
                //autofit
                wsXL.Columns[tCol].AutoFit();
            }

            objXL.ActiveWindow.DisplayGridlines = false;
            objXL.Visible = true;

            Range xlsRange; 
            Chart xlsChart;
            long ChartAStart = 0;
            long ChartAEnd = 0;

            ChartAStart = 6;
            ChartAEnd = 12;

            xlsRange = wsXL.Range["A" + (ChartAStart + 1) + ":B" + ChartAEnd];
            xlsChart = wsXL.Parent.Charts.Add;
            dynamic _with10 = xlsChart;
            _with10.ChartWizard(xlsRange, XlChartType.xl3DColumn, XlRowCol.xlColumns);
            Series xlSeries = _with10.SeriesCollection(1);
            xlsChart.PlotBy = XlRowCol.xlColumns;

            xlSeries.XValues = wsXL.Range["A" + (ChartAStart + 1), "B" + ChartAEnd];
            xlSeries.Name = FundShort; //wsXL.Cells[ChartAStart, 2].Value;
            //xlSeries.ApplyDataLabels();

            xlSeries.Border.LineStyle = xlNone;

            _with10.ChartArea.Border.LineStyle = xlNone;
            _with10.ChartArea.Interior.ColorIndex = xlNone;
            _with10.PlotArea.Border.LineStyle = xlNone;
            _with10.PlotArea.Interior.ColorIndex = xlNone;

            _with10.Axes(xlValue).MajorTickMark = xlNone;
            _with10.Axes(xlValue).MinorTickMark = xlNone;
            _with10.Axes(xlValue).TickLabelPosition = xlNone;
            _with10.Axes(xlValue).Border.LineStyle = xlNone;

            _with10.Axes(xlCategory).HasMajorGridlines = false;
            _with10.Axes(xlCategory).HasMinorGridlines = false;

            _with10.Axes(xlValue).HasMajorGridlines = false;
            _with10.Axes(xlValue).HasMinorGridlines = false;

            _with10.Legend.Border.LineStyle = xlNone;

            _with10.Legend.Position = XlLegendPosition.xlLegendPositionTop;

            _with10.Location(XlChartLocation.xlLocationAsObject, wsXL.Name);


            wsXL.ChartObjects("Chart 1").Activate();
            Series activeSeriesCollection = wbXL.ActiveChart.SeriesCollection(1);
            if (activeSeriesCollection != null)
            {
                activeSeriesCollection.Interior.Color = Color.FromArgb(0, 106, 81);
            }

            wbXL.ActiveChart.ChartArea.Border.Weight = 2;
            wbXL.ActiveChart.ChartArea.Border.LineStyle = 0;
            int rowNumber = Convert.ToInt32(ChartAEnd + 3);
            //Chart chartPage = default(Chart);
            ChartObjects xlCharts = wsXL.ChartObjects();
            ChartObject myChart = xlCharts.Item("Chart 1");

            myChart.Top = wsXL.Rows[rowNumber].Top;
            myChart.Left = wsXL.Columns[1].Left;
            myChart.Width = 500;
            return functionReturnValue;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public System.DateTime FundEndDate(int FID)
        {

            //string SQLThree = "SELECT MAX(TransDate) AS TransDate FROM PerformanceTransSub WHERE TransAggID = " + Convert.ToString(FID);
            //rdCurrency = Core.GetDBResults(ref strMsg, "sp_GetFundDate", "@ClientID", StrClientID,"@FundType","E");
            
            rsTwo.Rows.Clear();
            rsTwo.Load(rdCurrency);


            var _with1 = rsTwo;
            if ((rsTwo == null))
            {
                return DateTime.UtcNow;
            }
            else
            {

                if (rsTwo.Rows[0].IsNull("TransDate"))
                {
                    return DateTime.UtcNow.Date;
                }
                else
                {
                    return ((DateTime)(rsTwo.Rows[0]["TransDate"])).Date;
                }
            }
        }


        public System.DateTime FundStartDate(int FID)
        {

            //DbProviderFactory dpf = DbProviderFactories.GetFactory(DataProviderName);
            //conCurrency = dpf.CreateConnection();
            //conCurrency.ConnectionString = connectionString;

            //string SQLThree = "Select FundDate from Clients Where ClientNo = " + Convert.ToString(FID);
            //rdCurrency = Core.GetDBResults(ref strMsg, "sp_GetFundDate", "@ClientID", StrClientID, "@FundType", "S");

            rdCurrency = cnCurrency.ExecuteReader(CommandBehavior.CloseConnection);

            rsTwo.Load(rdCurrency);


            var _with1 = rsTwo;
            if ((rsTwo  == null))
            {
                return DateTime.UtcNow.Date;

            }
            else
            {

                if (rsTwo.Rows[0].IsNull("FundDate"))
                {
                    return DateTime.UtcNow;
                }
                else
                {
                    return ((DateTime) (rsTwo.Rows[0]["FundDate"])).Date;
                }
            }
        }

    }
}





