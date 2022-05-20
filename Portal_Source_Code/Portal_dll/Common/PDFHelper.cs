using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using HFCPortal;
using System.Net.Mail;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System.Web;
using System.Data;
using System.IO;
using System.Web.Configuration;
using System.Net.Mime;
using HFCPortal.SMTP;

namespace HFCPortal.Common
{
    public class PDFHelper
    {
        
        DataAccess DataClass;
        Functions fn;
        public static string fileName;
        string strMsg = "";

        public string sendPDFEmail(string filePath, string emailRecepient, string emailSuject, string emailBody)
        {
            fn = new Functions();
            DataClass = new DataAccess();
            string strError = string.Empty;

            try
            {
                DbDataReader rsSystemSettings = DataClass.GetDBResults(ref strMsg, "sp_getSystem");

                if (rsSystemSettings.Read())
                {
                    string EmailBody = string.Empty;

                    EmailBody = emailBody;

                    //EmailBody = EmailBody + "<html>";
                    //EmailBody = EmailBody + "<head><title></title></head>";
                    //EmailBody = EmailBody + "<body>";
                    //EmailBody = EmailBody + "<p>" + emailBody + "</p>";
                    //EmailBody = EmailBody + "<img src='" + HttpContext.Current.Server.MapPath("Images/hfc_logo.jpg") + "'>";
                    //EmailBody = EmailBody + "</body>";
                    //EmailBody = EmailBody + "</html>";



                    MailMessage message = new MailMessage(rsSystemSettings["sendusername"].ToString(), emailRecepient, emailSuject, EmailBody);
                    message.IsBodyHtml = true;
                    message.Subject = emailSuject;
                    message.Body = "<div style=\"font-family:Arial\">" + EmailBody + "<br /><br /><img src=\"@@IMAGE@@\" alt=\"\"><br /><br /></div>";
                    

                    //// generate the contentID string using the datetime
                    string contentID = Path.GetFileName(HttpContext.Current.Server.MapPath("EmailSignatures/EmailSignature.png")).Replace(".", "") + "@zofm";
                    //string strSignature = Path.GetFileName(HttpContext.Current.Server.MapPath("EmailSignatures/EmailSignature.png"));
                    //// create the INLINE attachment
                    //string attachmentPath = HttpContext.Current.Server.MapPath(@"EmailSignatures/EmailSignature.png");//Environment.CurrentDirectory + @"Images/hfc_logo.jpg";
                    //Attachment inline = new Attachment(attachmentPath);
                    //inline.ContentDisposition.Inline = true;
                    //inline.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                    //inline.ContentId = contentID;
                    //if (strSignature.EndsWith(".png"))
                    //{
                    //    inline.ContentType.MediaType = "image/png";
                    //}
                    //else
                    //{
                    //    if (strSignature.EndsWith(".jpg") || strSignature.EndsWith(".jpeg"))
                    //    {
                    //        inline.ContentType.MediaType = "image/jpg";
                    //    }
                    //}


                    //inline.ContentType.Name = Path.GetFileName(attachmentPath);
                    //message.Attachments.Add(inline);

                    // replace the tag with the correct content ID
                    message.Body = message.Body.Replace("@@IMAGE@@", "cid:" + contentID);

                    SmtpClient smtp = new SmtpClient(rsSystemSettings["SmtpServer"].ToString(), int.Parse(rsSystemSettings["smtpserverport"].ToString()));                   

                    System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(rsSystemSettings["sendusername"].ToString(), rsSystemSettings["sendpassword"].ToString());
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = false;
                    if (rsSystemSettings["smtpAuthenticate"].ToString() == "1")
                    {
                        smtp.EnableSsl = true;
                    }

                    smtp.Credentials = SMTPUserInfo;
                    //Attachment attachFile = new Attachment(filePath);
                    //message.Attachments.Add(inline);

                    Attachment attachFile = new Attachment(filePath);
                    message.Attachments.Add(attachFile);

                    smtp.Send(message);

                    message.Dispose();

                    if (File.Exists(filePath))
                        File.Delete(filePath);
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




        #region Methods
        /// <summary>
        /// Print Transactions to PDF
        /// </summary>
        /// <param name="Transactions"></param>
        /// <param name="filePath"></param>
        public static void PrintTransactionsToPdf(DataTable transactions, string filePath)
        {
            if (String.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("filePath");
            }

            Document doc = new Document();

            //Section sec = doc.AddSection();
            Section section = doc.AddSection();

            Paragraph p1 = section.AddParagraph("This is Clients name");
            p1.Format.Font.Bold = true;
            p1.Format.Font.Color = Colors.Black;

            Paragraph p2 = section.AddParagraph("This is Account Number");
            p2.Format.Font.Bold = true;
            p2.Format.Font.Color = Colors.Blue;

            int transCount = transactions.Rows.Count;
            Table tbl = section.AddTable();

            tbl.Borders.Visible = true;
            tbl.Borders.Width = 1;

            tbl.AddColumn(Unit.FromCentimeter(4));
            tbl.AddColumn(Unit.FromCentimeter(4));
            tbl.AddColumn(Unit.FromCentimeter(4));
            tbl.AddColumn(Unit.FromCentimeter(4));

            Row header = tbl.AddRow();

            header.Cells[0].AddParagraph("Date");
            header.Cells[0].Format.Alignment = ParagraphAlignment.Center;

            header.Cells[1].AddParagraph("Value Date");
            header.Cells[1].Format.Alignment = ParagraphAlignment.Center;

            header.Cells[2].AddParagraph("Transaction Type");
            header.Cells[2].Format.Alignment = ParagraphAlignment.Center;

            header.Cells[3].AddParagraph("Amount");
            header.Cells[3].Format.Alignment = ParagraphAlignment.Center;

            for (int i = 0; i < transactions.Rows.Count; i++)
            {
                DataRow currentTrans = transactions.Rows[i];
                Row transRow = tbl.AddRow();

                transRow.Cells[0].AddParagraph(string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Convert.ToString(currentTrans["Date"]))));

                transRow.Cells[1].AddParagraph(string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Convert.ToString(currentTrans["ValueDate"]))));

                transRow.Cells[2].AddParagraph(Convert.ToString(currentTrans["TrxType"]));

                transRow.Cells[3].AddParagraph(Convert.ToString(currentTrans["Amount"]));

            }


            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always);
            renderer.Document = doc;
            renderer.RenderDocument();
            renderer.PdfDocument.Save(filePath);
        }

        /// <summary>
        /// Write PDF file to response
        /// </summary>
        /// <param name="filePath">File napathme</param>
        /// <param name="targetFileName">Target file name</param>
        /// <remarks>For BeatyStore project</remarks>
        public static void WriteResponsePdf(string filePath, string targetFileName)
        {
            if (!String.IsNullOrEmpty(filePath))
            {
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.Charset = "utf-8";
                response.ContentType = "text/pdf";
                response.AddHeader("content-disposition", string.Format("attachment; filename={0}", targetFileName));
                response.BinaryWrite(File.ReadAllBytes(filePath));
                response.End();
            }
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets a file path to PDF logo
        /// </summary>
        public static string LogoFilePath
        {
            get
            {
                return HttpContext.Current.Request.PhysicalApplicationPath + "images/pdflogo.img";
            }
        }
        #endregion

    }


}
