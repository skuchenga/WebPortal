using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using BrDataEncryption;

namespace HFCPortal.Common
{
    /// <summary>
    /// Summary description for CommonHelper
    /// </summary>
    public partial class CommonHelper
    {
        public static string dbServer { get; set; }
        public static string dbName { get; set; }
        public static string dbUser { get; set; }
        public static string dbPWD { get; set; }

        public string[] ConnectionProperties { get; set; }

        private static BrWebDataEcryption brWDE = new BrWebDataEcryption();

        public static string GetConnectionString()
        {
            SetConnectionProperties();

            string connString = "user id=" + dbUser + ";password=" + brWDE.DecyptKey(dbPWD) + ";server=" + DecryptItem(dbServer) + ";Trusted_Connection=no;database=" + dbName + ";connection timeout=300";
            
            return connString;
        }

        public static void SetConnectionProperties()
        {
         
            dbServer = WebConfigurationManager.AppSettings["DBServer"];
            dbName = WebConfigurationManager.AppSettings["DBName"];
            dbUser = WebConfigurationManager.AppSettings["DBUserID"];
            dbPWD = WebConfigurationManager.AppSettings["DBUserPWD"];
        }


        /// <summary>
        /// Reloads current page
        /// </summary>
        public static void ReloadCurrentPage()
        {
            bool useSSL = IsCurrentConnectionSecured();
            ReloadCurrentPage(useSSL);
        }

        /// <summary>
        /// Reloads current page
        /// </summary>
        /// <param name="useSsl">Use SSL</param>
        public static void ReloadCurrentPage(bool useSsl)
        {
            string appHost = GetSystemLocation(useSsl);
            if (appHost.EndsWith("/"))
                appHost = appHost.Substring(0, appHost.Length - 1);
            string url = appHost + HttpContext.Current.Request.RawUrl;
            url = url.ToLowerInvariant();
            HttpContext.Current.Response.Redirect(url);
        }

        /// <summary>
        /// Gets System location
        /// </summary>
        /// <returns>System location</returns>
        public static string GetSystemLocation()
        {
            bool useSSL = IsCurrentConnectionSecured();
            return GetSystemLocation(useSSL);
        }

        /// <summary>
        /// Gets System location
        /// </summary>
        /// <param name="useSsl">Use SSL</param>
        /// <returns>System location</returns>
        public static string GetSystemLocation(bool useSsl)
        {
            string result = GetSystemHost(useSsl);
            if (result.EndsWith("/"))
                result = result.Substring(0, result.Length - 1);
            result = result + HttpContext.Current.Request.ApplicationPath;
            if (!result.EndsWith("/"))
                result += "/";

            return result.ToLowerInvariant();
        }

        /// <summary>
        /// Bind jQuery
        /// </summary>
        /// <param name="page">Page</param>
        public static void BindJQuery(Page page)
        {
            BindJQuery(page, false);
        }

        /// <summary>
        /// Bind jQuery
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="useHosted">Use hosted jQuery</param>
        public static void BindJQuery(Page page, bool useHosted)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            //update version if required (e.g. 1.4)
            string jQueryUrl = string.Empty;
            if (useHosted)
            {
                jQueryUrl = "http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js";
                if (CommonHelper.IsCurrentConnectionSecured())
                {
                    jQueryUrl = jQueryUrl.Replace("http://", "https://");
                }
            }
            else
            {
                jQueryUrl = CommonHelper.GetSystemLocation() + "Scripts/jquery-1.4.min.js";
            }

            jQueryUrl = string.Format("<script type=\"text/javascript\" src=\"{0}\" ></script>", jQueryUrl);

            if (page.Header != null)
            {
                //we have a header
                if (HttpContext.Current.Items["JQueryRegistered"] == null ||
                    !Convert.ToBoolean(HttpContext.Current.Items["JQueryRegistered"]))
                {
                    Literal script = new Literal() { Text = jQueryUrl };
                    Control control = page.Header.FindControl("SCRIPTS");
                    if (control != null)
                        control.Controls.AddAt(0, script);
                    else
                        page.Header.Controls.AddAt(0, script);
                }
                HttpContext.Current.Items["JQueryRegistered"] = true;
            }
            else
            {
                //no header found
                page.ClientScript.RegisterClientScriptInclude(jQueryUrl, jQueryUrl);
            }
        }
        

        /// <summary>
        /// Gets System host location
        /// </summary>
        /// <param name="useSsl">Use SSL</param>
        /// <returns>System host location</returns>
        public static string GetSystemHost(bool useSsl)
        {
            string result = "http://" + ServerVariables("HTTP_HOST");
            if (!result.EndsWith("/"))
                result += "/";
            if (useSsl)
            {
                //shared SSL certificate URL
                string sharedSslUrl = string.Empty;
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SharedSSLUrl"]))
                    sharedSslUrl = ConfigurationManager.AppSettings["SharedSSLUrl"].Trim();

                if (!String.IsNullOrEmpty(sharedSslUrl))
                {
                    //shared SSL
                    result = sharedSslUrl;
                }
                else
                {
                    //standard SSL
                    result = result.Replace("http:/", "https:/");
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["UseSSL"])
                    && Convert.ToBoolean(ConfigurationManager.AppSettings["UseSSL"]))
                {
                    //SSL is enabled

                    //get shared SSL certificate URL
                    string sharedSslUrl = string.Empty;
                    if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SharedSSLUrl"]))
                        sharedSslUrl = ConfigurationManager.AppSettings["SharedSSLUrl"].Trim();
                    if (!String.IsNullOrEmpty(sharedSslUrl))
                    {
                        string nonSharedSslUrl = string.Empty;
                        if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["NonSharedSSLUrl"]))
                            nonSharedSslUrl = ConfigurationManager.AppSettings["NonSharedSSLUrl"].Trim();
                        if (string.IsNullOrEmpty(nonSharedSslUrl))
                            throw new Exception("NonSharedSSLUrl app config setting is not empty");
                        result = nonSharedSslUrl;
                    }
                }
            }

            if (!result.EndsWith("/"))
                result += "/";

            return result.ToLowerInvariant();
        }
        
        /// <summary>
        /// Gets server variable by name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Server variable</returns>
        public static string ServerVariables(string name)
        {
            string tmpS = string.Empty;
            try
            {
                if (HttpContext.Current.Request.ServerVariables[name] != null)
                {
                    tmpS = HttpContext.Current.Request.ServerVariables[name].ToString();
                }
            }
            catch
            {
                tmpS = string.Empty;
            }
            return tmpS;
        }

        /// <summary>
        /// Gets a value indicating whether current connection is secured
        /// </summary>
        /// <returns>true - secured, false - not secured</returns>
        public static bool IsCurrentConnectionSecured()
        {
            bool useSSL = false;
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                useSSL = HttpContext.Current.Request.IsSecureConnection;
                //when your hosting uses a load balancer on their server then the Request.IsSecureConnection is never got set to true, use the statement below
                //just uncomment it
                //useSSL = HttpContext.Current.Request.ServerVariables["HTTP_CLUSTER_HTTPS"] == "on" ? true : false;
            }

            return useSSL;
        }

        /// <summary>
        /// Verifies that a string is in valid e-mail format
        /// </summary>
        /// <param name="email">Email to verify</param>
        /// <returns>true if the string is a valid e-mail address and false if it's not</returns>
        public static bool IsValidEmail(string email)
        {
            bool result = false;
            if (String.IsNullOrEmpty(email))
                return result;
            email = email.Trim();
            result = Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            return result;
        }

        /// <summary>
        /// Gets query string value by name
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <returns>Query string value</returns>
        public static string QueryString(string name)
        {
            string result = string.Empty;
            if (HttpContext.Current != null && HttpContext.Current.Request.QueryString[name] != null)
                result = HttpContext.Current.Request.QueryString[name].ToString();
            return result;
        }

        /// <summary>
        /// Gets boolean value from query string 
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <returns>Query string value</returns>
        public static bool QueryStringBool(string name)
        {
            string resultStr = QueryString(name).ToUpperInvariant();
            return (resultStr == "YES" || resultStr == "TRUE" || resultStr == "1");
        }

        /// <summary>
        /// Gets integer value from query string 
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <returns>Query string value</returns>
        public static int QueryStringInt(string name)
        {
            string resultStr = QueryString(name).ToUpperInvariant();
            int result;
            Int32.TryParse(resultStr, out result);
            return result;
        }

        /// <summary>
        /// Gets integer value from query string 
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Query string value</returns>
        public static int QueryStringInt(string name, int defaultValue)
        {
            string resultStr = QueryString(name).ToUpperInvariant();
            if (resultStr.Length > 0)
            {
                return Int32.Parse(resultStr);
            }
            return defaultValue;
        }

        /// <summary>
        /// Gets GUID value from query string 
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <returns>Query string value</returns>
        public static Guid? QueryStringGuid(string name)
        {
            string resultStr = QueryString(name).ToUpperInvariant();
            Guid? result = null;
            try
            {
                result = new Guid(resultStr);
            }
            catch
            {
            }
            return result;
        }


        public static string DecryptItem(string StringToDecrypt)
        {
            string SeedValue = "0123456789ABCDEF";
            //StreamReader objStreamReader;
            //objStreamReader = File.OpenText(HttpContext.Current.Server.MapPath("security\\" + "user.ini"));
            //while (objStreamReader.Peek() >= 0)
            //{
            //    StringToDecrypt = objStreamReader.ReadLine();
            //}
            //objStreamReader.Close();
            String Temp = "";

            int i, dblCountLength;
            int intLengthChar, intCountChar, intRandomSeed, intBeforeMulti, intAfterMulti, intSubNinetyNine, intInverseAsc;
            double dblCurrentChar;
            String strCurrentChar, Decrypt = "";

            if (SeedValue != "0123456789ABCDEF")
                return "";

            try
            {
                for (dblCountLength = 0; dblCountLength < StringToDecrypt.Length; dblCountLength++)
                {
                    intLengthChar = System.Convert.ToInt16(StringToDecrypt.Substring(dblCountLength, 1));
                    strCurrentChar = StringToDecrypt.Substring(dblCountLength + 1, intLengthChar);
                    dblCurrentChar = 0;

                    for (intCountChar = 0; intCountChar < strCurrentChar.Length; intCountChar++)
                    {
                        double x = 93, y = System.Convert.ToDouble(strCurrentChar.Length - intCountChar - 1);
                        dblCurrentChar = dblCurrentChar + (strCurrentChar[intCountChar] - 33) * (Math.Pow(x, y));
                    }
                    intRandomSeed = System.Convert.ToInt16(dblCurrentChar.ToString().Substring(2, 2));

                    intBeforeMulti = System.Convert.ToInt32(dblCurrentChar.ToString().Substring(0, 2) + dblCurrentChar.ToString().Substring(4, 2));
                    intAfterMulti = intBeforeMulti / intRandomSeed;
                    intSubNinetyNine = intAfterMulti - 99;
                    intInverseAsc = 256 - intSubNinetyNine;
                    Decrypt = Decrypt + System.Convert.ToChar(intInverseAsc).ToString();
                    dblCountLength = dblCountLength + intLengthChar;
                }

                for (i = 0; i < Decrypt.Length; i++)
                {
                    char c = (char)((int)(Decrypt[i] - (Decrypt.Length + i + 1)));
                    Temp = Temp + c.ToString();
                }
            }
            catch (Exception se)
            {
                ;
            }
            Decrypt = Temp;
            return Decrypt;
        }

        /// <summary>
        /// Modifies query string
        /// </summary>
        /// <param name="url">Url to modify</param>
        /// <param name="queryStringModification">Query string modification</param>
        /// <param name="targetLocationModification">Target location modification</param>
        /// <returns>New url</returns>
        public static string ModifyQueryString(string url, string queryStringModification, string targetLocationModification)
        {
            if (url == null)
                url = string.Empty;
            url = url.ToLowerInvariant();

            if (queryStringModification == null)
                queryStringModification = string.Empty;
            queryStringModification = queryStringModification.ToLowerInvariant();

            if (targetLocationModification == null)
                targetLocationModification = string.Empty;
            targetLocationModification = targetLocationModification.ToLowerInvariant();


            string str = string.Empty;
            string str2 = string.Empty;
            if (url.Contains("#"))
            {
                str2 = url.Substring(url.IndexOf("#") + 1);
                url = url.Substring(0, url.IndexOf("#"));
            }
            if (url.Contains("?"))
            {
                str = url.Substring(url.IndexOf("?") + 1);
                url = url.Substring(0, url.IndexOf("?"));
            }
            if (!string.IsNullOrEmpty(queryStringModification))
            {
                if (!string.IsNullOrEmpty(str))
                {
                    var dictionary = new Dictionary<string, string>();
                    foreach (string str3 in str.Split(new char[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str3))
                        {
                            string[] strArray = str3.Split(new char[] { '=' });
                            if (strArray.Length == 2)
                            {
                                dictionary[strArray[0]] = strArray[1];
                            }
                            else
                            {
                                dictionary[str3] = null;
                            }
                        }
                    }
                    foreach (string str4 in queryStringModification.Split(new char[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str4))
                        {
                            string[] strArray2 = str4.Split(new char[] { '=' });
                            if (strArray2.Length == 2)
                            {
                                dictionary[strArray2[0]] = strArray2[1];
                            }
                            else
                            {
                                dictionary[str4] = null;
                            }
                        }
                    }
                    var builder = new StringBuilder();
                    foreach (string str5 in dictionary.Keys)
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append("&");
                        }
                        builder.Append(str5);
                        if (dictionary[str5] != null)
                        {
                            builder.Append("=");
                            builder.Append(dictionary[str5]);
                        }
                    }
                    str = builder.ToString();
                }
                else
                {
                    str = queryStringModification;
                }
            }
            if (!string.IsNullOrEmpty(targetLocationModification))
            {
                str2 = targetLocationModification;
            }
            return (url + (string.IsNullOrEmpty(str) ? "" : ("?" + str)) + (string.IsNullOrEmpty(str2) ? "" : ("#" + str2))).ToLowerInvariant();
        }

        public static string CheckFilePath(string fPath)
        {
            string myPath = fPath;

            if (string.IsNullOrEmpty(myPath))
            {
                return string.Empty;//string.IsNullOrEmpty();
            }

            if (!myPath.EndsWith("\\"))
            {
                myPath += "\\";
            }

            return myPath;
        }

        

    }

}


