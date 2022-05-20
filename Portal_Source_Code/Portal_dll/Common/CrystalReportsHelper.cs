using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using HFCPortal;


namespace HFCPortal.Common
{
    public class CrystalReportsHelper
    {
        //ReportDocument rpt;
        string strMessage = string.Empty;

        public string GenerateAccountStatements(ReportDocument rpt, string OurBranchID, string FromAccountID, string ToAccountID, string FromDate, string ToDate, string Currency, string OperatorID)
        {
            try
            {

              
                //string reportPath = Server.MapPath("rptUTAccountStatement.rpt");


                CommonHelper.SetConnectionProperties();

               

                rpt.SetParameterValue("@OurBranchID", OurBranchID);
                rpt.SetParameterValue("@FromAccountID", FromAccountID);
                rpt.SetParameterValue("@ToAccountID", ToAccountID);
                rpt.SetParameterValue("@FromDate", FromDate);
                rpt.SetParameterValue("@ToDate", ToDate);
                rpt.SetParameterValue("@Currency", Currency);
                rpt.SetParameterValue("@DateOrValue", null);
                rpt.SetParameterValue("@ReturnFields", null);
                rpt.SetParameterValue("@ReturnFilters", null);
                rpt.SetParameterValue("Map", "Bankers Realm  1.045p");
                rpt.SetParameterValue("OperatorID", OperatorID);

                //rpt.SetDatabaseLogon(coreDB.dbUser, coreDB.dbPWD, coreDB.dbServer, coreDB.dbName);

                ConnectionInfo connectionInfo = new ConnectionInfo();

                connectionInfo.ServerName = CommonHelper.dbServer;

                connectionInfo.UserID = CommonHelper.dbUser;

                connectionInfo.Password = CommonHelper.dbPWD;

                connectionInfo.DatabaseName = CommonHelper.dbName;

                // crUTAccountStatement.ReportSource = rpt;


                SetDBLogonForReport(connectionInfo, rpt);

                return string.Empty;
                /*CrystalReportViewer1.EnableDatabaseLogonPrompt = false;

                CrystalReportViewer1.LogOnInfo;*/

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ExportReportToPDF(ReportDocument rptDoc, string PDFPathAndName)
        {
            
            strMessage = string.Empty;
            try
            {
                CrystalDecisions.CrystalReports.Engine.ReportDocument crReportDocument = rptDoc;//new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                DiskFileDestinationOptions crDiskFileDestinationOptions;
                ExportOptions crExportOptions;

             //   CrystalDecisions.Shared.ExportFormatType efileType = (CrystalDecisions.Shared.ExportFormatType)Enum.Parse(typeof(CrystalDecisions.Shared.ExportFormatType), "CrystalDecisions.Shared.ExportFormatType.PortableDocFormat");

                crDiskFileDestinationOptions = new DiskFileDestinationOptions();
                crDiskFileDestinationOptions.DiskFileName = PDFPathAndName;
                crExportOptions = crReportDocument.ExportOptions;
                {
                    crExportOptions.DestinationOptions = crDiskFileDestinationOptions;
                    crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                }

                crReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, PDFPathAndName);

                return strMessage;
            }
            catch (System.Exception ex)
            {
                strMessage = ex.Message;
                return strMessage;
            }
        }


        public void SetDBLogonForReport(ConnectionInfo myConnectionInfo, ReportDocument myReportDocument)
        {
            Tables myTables = myReportDocument.Database.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table myTable in myTables)
            {
                TableLogOnInfo myTableLogonInfo = myTable.LogOnInfo;
                myTableLogonInfo.ConnectionInfo = myConnectionInfo;
                myTable.ApplyLogOnInfo(myTableLogonInfo);
            }
        }

        public void SetDBLogonForSubreports(ConnectionInfo myConnectionInfo, ReportDocument myReportDocument)
        {
            Sections mySections = myReportDocument.ReportDefinition.Sections;
            foreach (Section mySection in mySections)
            {
                ReportObjects myReportObjects = mySection.ReportObjects;
                foreach (ReportObject myReportObject in myReportObjects)
                {
                    if (myReportObject.Kind == ReportObjectKind.SubreportObject)
                    {
                        SubreportObject mySubreportObject = (SubreportObject)myReportObject;
                        ReportDocument subReportDocument = mySubreportObject.OpenSubreport(mySubreportObject.SubreportName);
                        SetDBLogonForReport(myConnectionInfo, subReportDocument);
                    }
                }
            }
        }
    }
}
