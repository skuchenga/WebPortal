using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Common;
using System.Net.Mail;
using Telerik.Web.UI;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using HFCPortal;
using System.Web.SessionState;

/// <summary>
/// Summary description for DataAccess
/// </summary>

namespace HFCPortal
{

    public class Functions
    {

        string messageLine = "";

        public Functions()
        {

        }

        public void MessageLine(Page pg, String message, System.Drawing.Color colorName, string strLabelName)
        {
            foreach (Control c in pg.Controls)
            {
                MessageLineChild(c, pg, message, colorName, strLabelName);

            }
        }
        public void MessageLineChild(Control controlName, Page pageName, String messageChild, System.Drawing.Color colorNameChild, string strLabelName)
        {
            foreach (Control childc in controlName.Controls)
            {
                if (childc is System.Web.UI.WebControls.Label)
                {
                    if (childc.ID == strLabelName)
                    {
                        ((System.Web.UI.WebControls.Label)childc).ForeColor = colorNameChild;
                        ((System.Web.UI.WebControls.Label)childc).Text = messageChild;
                    }

                }

                MessageLineChild(childc, pageName, messageChild, colorNameChild, strLabelName);

            }
        }
        public Boolean CheckMeetingDayRule(int TrxOnMeetingDayOnly, int MeetingDay, DateTime WORKING_DATE)
        {
            if (TrxOnMeetingDayOnly == 1)
            {
                if (WeekDay(WORKING_DATE) == MeetingDay)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }

        }
        public string SendE_mail(string To, string Subject, string Body)
        {
            string Error = "";
            DataAccess DataClass;
            DataClass = new DataAccess();
            DbDataReader rsBankEmail = DataClass.GetDBResults(ref Error, "sp_getSystem");
            if (Error != "")
            {
                logError(Error);
                return Error;
            }
            if (rsBankEmail.Read())
            {
                try
                {
                    MailMessage message = new MailMessage(rsBankEmail["sendusername"].ToString(), To, Subject, Body);
                    message.IsBodyHtml = true;
                    SmtpClient client = new SmtpClient(rsBankEmail["SmtpServer"].ToString(), int.Parse(rsBankEmail["smtpserverport"].ToString()));
                    System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(rsBankEmail["sendusername"].ToString(), rsBankEmail["sendpassword"].ToString());
                    client.EnableSsl = false;
                    if (rsBankEmail["smtpAuthenticate"].ToString() == "1")
                    {
                        client.EnableSsl = true;
                    }
                    client.UseDefaultCredentials = false;
                    client.Credentials = SMTPUserInfo;
                    client.Send(message);
                    Error = "";
                }
                catch (Exception ex)
                {
                    logError(ex.Message);
                    return Error;
                }



            }
            else
            {
                Error = "Emails not configured.";
                return Error;
            }
            rsBankEmail.Close();
            rsBankEmail.Dispose();
            return Error;
        }
        public void logError(String strErr)
        {
            string strMsg = "";
            DataAccess dataAccess = new DataAccess();
            dataAccess.GetDBResults(ref strMsg, "sp_AddErrorLog", "@LogMessage", strErr);



            //StreamWriter sw_Errors = File.AppendText(WebConfigurationManager.AppSettings["errorPath"].ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + ".txt");
            //sw_Errors.WriteLine(DateTime.Now.ToString() + "  :  " + strErr);
            //sw_Errors.Close();
        }
        public static int WeekDay(DateTime dt)
        {

            // Set Year
            int yyyy = dt.Year;

            // Set Month
            int mm = dt.Month;

            // Set Day
            int dd = dt.Day;

            // Declare other required variables
            int DayOfYearNumber;
            int Jan1WeekDay;
            int WeekDay;


            int i, j, k, l;
            int[] Mnth = new int[12] { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };


            // Set DayofYear Number for yyyy mm dd
            DayOfYearNumber = dd + Mnth[mm - 1];

            // Increase of Dayof Year Number by 1, if year is leapyear and month is february
            if ((IsLeapYear(yyyy) == true) && (mm == 2))
                DayOfYearNumber += 1;

            // Find the Jan1WeekDay for year 
            i = (yyyy - 1) % 100;
            j = (yyyy - 1) - i;
            k = i + i / 4;
            Jan1WeekDay = 1 + (((((j / 100) % 4) * 5) + k) % 7);

            // Calcuate the WeekDay for the given date
            l = DayOfYearNumber + (Jan1WeekDay - 1);
            WeekDay = 1 + ((l - 1) % 7);

            return WeekDay;
        }
        public static bool IsLeapYear(int yyyy)
        {

            if ((yyyy % 4 == 0 && yyyy % 100 != 0) || (yyyy % 400 == 0))
                return true;
            else
                return false;
        }

        public void ClearFields(Page pageName)
        {

            foreach (Control c in pageName.Controls)
            {
                ClearChildFields(c);

            }
        }
        public void ClearChildFields(Control controlName)
        {
            foreach (Control childc in controlName.Controls)
            {
                if (childc is RadTextBox)
                {

                    ((RadTextBox)childc).Text = "";

                }
                if (childc is TextBox)
                {

                    ((TextBox)childc).Text = "";

                }
                if (childc is RadDateInput)
                {

                    ((RadDateInput)childc).Text = "";

                }
                if (childc is RadNumericTextBox)
                {

                    ((RadNumericTextBox)childc).Text = "";

                }
                if (childc is Telerik.Web.UI.RadTextBox)
                {

                    ((Telerik.Web.UI.RadTextBox)childc).Text = "";

                }
                if (childc is Telerik.Web.UI.RadNumericTextBox)
                {

                    ((Telerik.Web.UI.RadNumericTextBox)childc).Text = "0";

                }
                ClearChildFields(childc);

            }
        }

        //check required fields+++++++++++++++++++++++++++++
        public string Isvalid(Page pageName, SqlDataReader recordset)
        {
            while (recordset.Read())
            {
                Boolean pass = EnumerateFields((System.Web.UI.Page)pageName, recordset["ObjectName"].ToString(), Boolean.Parse(recordset["Required"].ToString()));
            }
            recordset.Close();
            return messageLine;

        }

        protected Boolean EnumerateFields(Page pageName, string currentControl, Boolean req)
        {

            foreach (Control c in pageName.Controls)
            {
                Boolean pass = EnumerateChildFields(c, currentControl, req);
            }
            return true;
        }
        public Boolean EnumerateChildFields(Control controlName, string controlTocheck, Boolean required)
        {

            foreach (Control childc in controlName.Controls)
            {
                if (childc.ID == controlTocheck && required == true)
                {
                    if (childc is RadTextBox)
                    {

                        if (((RadTextBox)childc).Text == "")
                        {
                            messageLine = "Please Enter " + controlTocheck;
                            return false;
                        }

                    }

                    if (childc is RadDateInput)
                    {

                        if (((RadDateInput)childc).Text == "")
                        {
                            messageLine = "Please Enter " + controlTocheck;
                            return false;
                        }
                    }

                    if (childc is RadNumericTextBox)
                    {
                        if (((RadNumericTextBox)childc).Text == "")
                        {
                            messageLine = "Please Enter " + controlTocheck;
                            return false;
                        }
                    }
                }
                EnumerateChildFields(childc, controlTocheck, required);

            }
            return true;
        }
        //++++++++++++++++++++++++++++++++++++++++++++++++++
        //Enable fields +++++++++++++++++++++++++++++++++++++
        public void EnableFields(Page pageName)
        {

            foreach (Control c in pageName.Controls)
            {
                EnableChildFields(c);

            }
        }
        public void EnableChildFields(Control controlName)
        {
            foreach (Control childc in controlName.Controls)
            {
                if (childc is RadTextBox)
                {

                    ((RadTextBox)childc).Enabled = true;

                }
                if (childc is RadDateInput)
                {

                    ((RadDateInput)childc).Enabled = true;

                }
                if (childc is RadNumericTextBox)
                {

                    ((RadNumericTextBox)childc).Enabled = true;

                }
                if (childc is RadComboBox)
                {

                    ((RadComboBox)childc).Enabled = true;

                }
                EnableChildFields(childc);

            }
        }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++
        //Disable fields ++++++++++++++++++++++++++++++++++++
        public void DisableFields(Page pageName)
        {

            foreach (Control c in pageName.Controls)
            {
                DisableChildFields(c);

            }
        }
        public void DisableChildFields(Control controlName)
        {
            foreach (Control childc in controlName.Controls)
            {
                if (childc is RadTextBox)
                {

                    ((RadTextBox)childc).Enabled = false;

                }
                if (childc is RadDateInput)
                {

                    ((RadDateInput)childc).Enabled = false;

                }
                if (childc is RadNumericTextBox)
                {

                    ((RadNumericTextBox)childc).Enabled = false;

                }
                if (childc is RadComboBox)
                {

                    ((RadComboBox)childc).Enabled = false;

                }
                DisableChildFields(childc);

            }
        }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++
        public Boolean isDate(string DateLiteral)
        {
            try
            {
                DateTime.Parse(DateLiteral);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public string IsvalidVb(Page pageName, string currentControl, string caption)
        {

            Boolean pass = EnumerateFieldsVb((System.Web.UI.Page)pageName, currentControl, caption);

            return messageLine;

        }

        protected Boolean EnumerateFieldsVb(Page pageName, string currentControl, string caption)
        {

            foreach (Control c in pageName.Controls)
            {
                Boolean pass = EnumerateChildFieldsVb(c, currentControl, caption);
            }
            return true;
        }
        public Boolean EnumerateChildFieldsVb(Control controlName, string controlTocheck, string caption)
        {

            foreach (Control childc in controlName.Controls)
            {
                if (childc.ID == controlTocheck)
                {
                    if (childc is RadTextBox)
                    {

                        if (((RadTextBox)childc).Text == "")
                        {
                            messageLine = "Please Enter " + caption;
                            return false;
                        }

                    }

                    if (childc is RadDateInput)
                    {

                        if (((RadDateInput)childc).Text == "")
                        {
                            messageLine = "Please Enter " + caption;
                            return false;
                        }
                    }

                    if (childc is RadNumericTextBox)
                    {
                        if (((RadNumericTextBox)childc).Text == "")
                        {
                            messageLine = "Please Enter " + caption;
                            return false;
                        }
                    }
                }
                EnumerateChildFieldsVb(childc, controlTocheck, caption);

            }
            return true;
        }
        public double EncryptPassword(string UserID, string Password, DateTime PWUpdatedOn)
        {

            double lngN;
            int intI;
            String OurBranchID;
            String BankID;
            double days;
            double Qtr;
            HttpContext.Current.Session["OurBranchID"] = "001";
            HttpContext.Current.Session["OurBankID"] = "14";

            DateTime StartDate = new DateTime(PWUpdatedOn.Year, 01, 01);
            TimeSpan ts = new TimeSpan(PWUpdatedOn.Ticks - StartDate.Ticks);
            //Response.Write(ts.Days+1); 
            days = ts.Days + 1;
            lngN = 0;


            switch (PWUpdatedOn.Month)
            {
                case 1:
                    Qtr = 1;
                    break;
                case 2:
                    Qtr = 1;
                    break;
                case 3:
                    Qtr = 1;
                    break;
                case 4:
                    Qtr = 2;
                    break;
                case 5:
                    Qtr = 2;
                    break;
                case 6:
                    Qtr = 2;
                    break;
                case 7:
                    Qtr = 3;
                    break;
                case 8:
                    Qtr = 3;
                    break;
                case 9:
                    Qtr = 3;
                    break;
                case 10:
                    Qtr = 4;
                    break;
                case 11:
                    Qtr = 4;
                    break;
                case 12:
                    Qtr = 4;
                    break;
                default:
                    Qtr = 1;
                    break;
            }

            Password = Password.PadRight(15);
            for (intI = 0; intI < 15; intI++)
            {
                char ch = Password[intI];
                lngN = lngN + ((int)ch * (intI + 1)) + PWUpdatedOn.Day + PWUpdatedOn.Month + Qtr + days;
            }


            UserID = UserID.PadRight(15);
            for (intI = 0; intI < 15; intI++)
            {
                char ch = UserID[intI];
                lngN = lngN + ((int)ch * (intI + 1)) + PWUpdatedOn.Day + PWUpdatedOn.Month + Qtr + days;
            }

            OurBranchID = HttpContext.Current.Session["OurBranchID"].ToString();
            OurBranchID = OurBranchID.PadRight(15);

            for (intI = 0; intI < 4; intI++)
            {
                char ch = OurBranchID[intI];
                lngN = lngN + ((int)ch * (intI + 1)) + PWUpdatedOn.Day + PWUpdatedOn.Month + Qtr + days;
            }

            BankID = HttpContext.Current.Session["OurBankID"].ToString();
            BankID = BankID.PadRight(2);

            for (intI = 0; intI < 2; intI++)
            {
                char ch = BankID[intI];
                lngN = lngN + ((int)ch * (intI + 1)) + PWUpdatedOn.Day + PWUpdatedOn.Month + Qtr + days;
            }

            return lngN;



        }
        public double EncryptUserPassword(string Password, DateTime PWUpdatedOn)
        {

            double lngN;
            int intI;
            double days;
            double Qtr;


            DateTime StartDate = new DateTime(PWUpdatedOn.Year, 01, 01);
            TimeSpan ts = new TimeSpan(PWUpdatedOn.Ticks - StartDate.Ticks);
            //Response.Write(ts.Days+1); 
            days = ts.Days + 1;
            lngN = 0;


            switch (PWUpdatedOn.Month)
            {
                case 1:
                case 2:
                case 3:
                    Qtr = 1;
                    break;
                case 4:
                case 5:
                case 6:
                    Qtr = 2;
                    break;
                case 7:
                    Qtr = 3;
                    break;
                case 8:
                    Qtr = 3;
                    break;
                case 9:
                    Qtr = 3;
                    break;
                case 10:
                    Qtr = 4;
                    break;
                case 11:
                    Qtr = 4;
                    break;
                case 12:
                    Qtr = 4;
                    break;
                default:
                    Qtr = 1;
                    break;
            }

            Password = Password.PadRight(8);
            for (intI = 0; intI < 8; intI++)
            {
                char ch = Password[intI];
                lngN = lngN + ((int)ch * (intI + 1)) + PWUpdatedOn.Day + PWUpdatedOn.Month + Qtr + PWUpdatedOn.Year - 2000;
            }
            return lngN;
        }
        public void FillCombo(DataTable Recordset, String ControlName, Page pg)
        {
            foreach (Control c in pg.Controls)
            {
                FillComboChild(c, ControlName, Recordset);

            }
        }
        public void FillComboChild(Control control, String comboControl, DataTable records)
        {
            foreach (Control childc in control.Controls)
            {
                if (childc is System.Web.UI.WebControls.DropDownList)//System.Web.UI.WebControls.DropDownList
                {
                    if (((System.Web.UI.WebControls.DropDownList)childc).ID == comboControl)//System.Web.UI.WebControls.DropDownList
                    {

                        for (int i = 0; i < records.Rows.Count - 1; i++)
                        {
                            ((System.Web.UI.WebControls.DropDownList)childc).Items.Add(new ListItem(records.Rows[i][0].ToString() + "-" + records.Rows[i][1].ToString(), records.Rows[i][1].ToString()));//System.Web.UI.WebControls.DropDownList   
                        }
                    }

                }
                FillComboChild(childc, comboControl, records);
            }
        }

        public void FillCombo(DbDataReader Recordset, String ControlName, Page pg)
        {
            foreach (Control c in pg.Controls)
            {
                FillComboChild(c, ControlName, Recordset);

            }
        }
        public void FillComboChild(Control control, String comboControl, DbDataReader records)
        {
            foreach (Control childc in control.Controls)
            {
                if (childc is System.Web.UI.WebControls.DropDownList)//System.Web.UI.WebControls.DropDownList
                {
                    if (((System.Web.UI.WebControls.DropDownList)childc).ID == comboControl)//System.Web.UI.WebControls.DropDownList
                    {

                        while (records.Read())
                        {
                            ((System.Web.UI.WebControls.DropDownList)childc).Items.Add(new ListItem(records[0].ToString() + "-" + records[1].ToString(), records[0].ToString()));//System.Web.UI.WebControls.DropDownList   
                        }
                    }

                }
                FillComboChild(childc, comboControl, records);
            }
        }

        public void FillCombo2(DataTable Recordset, String ControlName, Page pg)
        {
            foreach (Control c in pg.Controls)
            {
                FillComboChild2(c, ControlName, Recordset);

            }
        }
        public void FillComboChild2(Control control, String comboControl, DataTable records)
        {
            foreach (Control childc in control.Controls)
            {
                if (childc is System.Web.UI.WebControls.DropDownList)//System.Web.UI.WebControls.DropDownList
                {
                    if (((System.Web.UI.WebControls.DropDownList)childc).ID == comboControl)//System.Web.UI.WebControls.DropDownList
                    {

                        for (int i = 0; i < records.Rows.Count - 1; i++)
                        {
                            ((System.Web.UI.WebControls.DropDownList)childc).Items.Add(new ListItem(records.Rows[i][0].ToString(), records.Rows[i][1].ToString()));//System.Web.UI.WebControls.DropDownList
                        }

                    }

                }
                FillComboChild2(childc, comboControl, records);
            }
        }
        //+++++++++++++++++++++++++
        public void comb(DataTable Recordset, String ControlName, Page pg)
        {
            foreach (Control c in pg.Controls)
            {
                comb(c, ControlName, Recordset);

            }
        }
        public void comb(Control control, String comboControl, DataTable records)
        {
            foreach (Control childc in control.Controls)
            {
                if (childc is System.Web.UI.WebControls.DropDownList)
                {
                    if (((System.Web.UI.WebControls.DropDownList)childc).ID == comboControl)
                    {
                        for (int i = 0; i < records.Rows.Count - 1; i++)
                        {
                            ((System.Web.UI.WebControls.DropDownList)childc).Items.Add(new ListItem(records.Rows[i][0].ToString(), records.Rows[i][4].ToString()));
                        }

                    }

                }
                comb(childc, comboControl, records);
            }
        }
        public bool IsNumeric(string input)
        {
            bool result = true;
            for (int i = 0; i < input.Length; i++)
            {
                if (!Char.IsNumber(input[i]))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        //++++++++++send mail combo+++++++++++++++
        public void comm(DbDataReader Recordset, String ControlName, Page pg)
        {
            foreach (Control c in pg.Controls)
            {
                comm(c, ControlName, Recordset);

            }
        }
        public void comm(Control control, String comboControl, DbDataReader records)
        {
            foreach (Control childc in control.Controls)
            {
                if (childc is System.Web.UI.WebControls.DropDownList)
                {
                    if (((System.Web.UI.WebControls.DropDownList)childc).ID == comboControl)
                    {
                        while (records.Read())
                        {
                            //((System.Web.UI.WebControls.DropDownList)childc).Items.Add(new ListItem(records[0].ToString(), records[4].ToString()));
                            ((System.Web.UI.WebControls.DropDownList)childc).Items.Add(new ListItem(records[0].ToString(), records[3].ToString()));
                        }

                    }

                }
                comm(childc, comboControl, records);
            }
        }
        public Boolean isSafe(String rawInput)
        {
            Boolean temp = true;
            if (rawInput.Length != 0)
            {

                int start;

                String strTemp = rawInput;
                start = strTemp.IndexOf("'", 0, 1);
                if (start != -1)
                {
                    temp = false;
                }
                start = strTemp.IndexOf(";", 0, strTemp.Length);
                if (start != -1)
                {
                    temp = false;
                }
                if (strTemp.Length > 1)
                {
                    start = strTemp.IndexOf("--", strTemp.Length - 2, 2);
                    if (start != -1)
                    {
                        temp = false;
                    }
                }

            }

            //strTemp = strTemp.Replace("'", "");
            //strTemp = strTemp.Replace("--", "");
            //strTemp = strTemp.Replace(";", "");
            return temp;
        }
        public string getMonthName(int intMonth)
        {
            string strMonth = "";
            switch (intMonth)
            {
                case 1:
                    strMonth = "JAN";
                    break;
                case 2:
                    strMonth = "FEB";
                    break;
                case 3:
                    strMonth = "MAR";
                    break;
                case 4:
                    strMonth = "APR";
                    break;
                case 5:
                    strMonth = "MAY";
                    break;
                case 6:
                    strMonth = "JUN";
                    break;
                case 7:
                    strMonth = "JUL";
                    break;
                case 8:
                    strMonth = "AUG";
                    break;
                case 9:
                    strMonth = "SEP";
                    break;
                case 10:
                    strMonth = "OCT";
                    break;
                case 11:
                    strMonth = "NOV";
                    break;
                case 12:
                    strMonth = "DEC";
                    break;
                default:
                    strMonth = "JAN";
                    break;
            }
            return strMonth;
        }
        public string getCustomDate(DateTime dtDate)
        {
            string strDate = "";
            strDate = dtDate.Day.ToString() + "-" + getMonthName(dtDate.Month) + "-" + dtDate.Year.ToString().PadLeft(4, '0');
            return strDate;
        }

    }
}