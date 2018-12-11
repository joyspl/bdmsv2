using OfficeOpenXml;
using OfficeOpenXml.Style;
using SARASWATIPRESSNEW.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;

public static class Utility
{
    public static XmlDocument CreateXml(DataTable dt, string tableName, string rowName)
    {
        XmlDocument xmlDocument = new XmlDocument();
        XmlNode xmlNode = xmlDocument.CreateNode(XmlNodeType.Element, tableName, null);
        xmlDocument.AppendChild(xmlNode);
        if (dt != null)
        {
            foreach (DataRow dataRow in dt.Rows)
            {
                XmlNode xmlNode2 = xmlDocument.CreateNode(XmlNodeType.Element, rowName, null);
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    XmlAttribute typeAttr = xmlDocument.CreateAttribute(dataColumn.ColumnName);
                    typeAttr.Value = Convert.ToString(dataRow[dataColumn.ColumnName]);
                    xmlNode2.Attributes.Append(typeAttr);
                }
                xmlNode.AppendChild(xmlNode2);
            }
        }
        return xmlDocument;
    }

    public static XmlDocument CreateXmlTraditional(DataTable dt)
    {
        XmlDocument xmlDocument = new XmlDocument();
        XmlNode xmlNode = xmlDocument.CreateNode(XmlNodeType.Element, "Root", null);
        xmlDocument.AppendChild(xmlNode);
        if (dt != null)
        {
            XmlNode xmlNode2 = xmlDocument.CreateNode(XmlNodeType.Element, "Rows", null);
            xmlNode.AppendChild(xmlNode2);
            foreach (DataRow dataRow in dt.Rows)
            {
                XmlNode xmlNode3 = xmlDocument.CreateNode(XmlNodeType.Element, "Row", null);
                xmlNode2.AppendChild(xmlNode3);
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    XmlNode xmlNode4 = xmlDocument.CreateNode(XmlNodeType.Element, dataColumn.ColumnName, null);
                    xmlNode4.InnerText = Convert.ToString(dataRow[dataColumn.ColumnName]);
                    xmlNode3.AppendChild(xmlNode4);
                }
            }
        }
        return xmlDocument;
    }

    public static DataTable ToDataTable<T>(List<T> items)
    {
        DataTable dataTable = new DataTable(typeof(T).Name);
        PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
        PropertyInfo[] array = properties;
        for (int i = 0; i < array.Length; i++)
        {
            PropertyInfo propertyInfo = array[i];
            dataTable.Columns.Add(propertyInfo.Name);
        }
        foreach (T current in items)
        {
            object[] array2 = new object[properties.Length];
            for (int j = 0; j < properties.Length; j++)
            {
                array2[j] = properties[j].GetValue(current, null);
            }
            dataTable.Rows.Add(array2);
        }
        return dataTable;
    }

    public static Page PageResults(int total, int ordinal = -1, int pageSize = 10, int PageNo = 1)
    {
        int start = 1, end = total;
        var maxPage = total % pageSize == 0 ? total / pageSize : total / pageSize + 1;
        var page = -1;

        if (ordinal > 0)
        {
            var previousPage = (ordinal % pageSize == 0) ? (ordinal / pageSize) - 1 : ordinal / pageSize;
            page = previousPage + 1;

            start = previousPage * pageSize + 1;
            end = (start + pageSize - 1) <= total ? (start + pageSize - 1) : total;
        }
        else
        {
            page = PageNo;

            if (page <= maxPage)
            {
                start = ((page - 1) * pageSize) + 1;
                end = page * pageSize <= total ? page * pageSize : total;
            }
            else
            {
                start = 0;
                end = 0;
            }
        }

        return new Page() { Start = start, End = end };
    }

    public static bool SendSMS(string MobileNo, string smsBody)
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        string Msg = smsBody;
        bool success = default(bool);
        string baseurl = string.Empty;
        WebClient client = new WebClient();
        try
        {
            string SmsUserID = System.Web.Configuration.WebConfigurationManager.AppSettings["SmsUserID"].ToString();
            string SmsPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["SmsPassword"].ToString();
            string SmsSenderName = System.Web.Configuration.WebConfigurationManager.AppSettings["SmsSenderName"].ToString();
            string TestSms = System.Web.Configuration.WebConfigurationManager.AppSettings["TestSms"].ToString();
            if(TestSms=="TRUE")
                MobileNo = System.Web.Configuration.WebConfigurationManager.AppSettings["TestMobileNo"].ToString();

            try
            {
                baseurl = "http://bulksms.mysmsmantra.com:8080/WebSMS/SMSAPI.jsp?username=" + SmsUserID + "&password=" + SmsPassword + "&sendername=" + SmsSenderName + "&mobileno=" + MobileNo + "&message=" + smsBody;
                Stream data = client.OpenRead(baseurl);
                StreamReader reader = new StreamReader(data);
                Msg = reader.ReadToEnd();
                data.Close();
                reader.Close();
                success = true;
            }
            catch (Exception exc)
            {
                objDbTrx.SaveSystemErrorLog(exc);
            }
        }
        catch (Exception ex)
        {
            objDbTrx.SaveSystemErrorLog(ex);
        }
        return success;
    }

    public static string SendChallanEmail(DataTable dtChallanDtl, System.Web.HttpContextWrapper ctx = null)
    {
        string Msg = "";
        string ErrorMsg = "";
        string MailSubject = "";
        StringBuilder strMessage = new StringBuilder();
        string challanId = "";
        try
        {
            string GmailUserNameKey = System.Web.Configuration.WebConfigurationManager.AppSettings["GmailUserNameKey"].ToString();
            string GmailPasswordKey = System.Web.Configuration.WebConfigurationManager.AppSettings["GmailPasswordKey"].ToString();
            string GmailHostKey = System.Web.Configuration.WebConfigurationManager.AppSettings["GmailHostKey"].ToString();
            string GmailPortKey = System.Web.Configuration.WebConfigurationManager.AppSettings["GmailPortKey"].ToString();
            string GmailSslKey = System.Web.Configuration.WebConfigurationManager.AppSettings["GmailSslKey"].ToString();

            string TestEmail = System.Web.Configuration.WebConfigurationManager.AppSettings["TestEmail"].ToString();
            string EmailIdTo = System.Web.Configuration.WebConfigurationManager.AppSettings["TestEmailId"].ToString();

            SmtpClient smtp = new SmtpClient();
            smtp.Host = GmailHostKey;
            smtp.Port = Convert.ToInt16(GmailPortKey);
            smtp.EnableSsl = Convert.ToBoolean(GmailSslKey);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(GmailUserNameKey, GmailPasswordKey);

            try
            {
                //challanId = Convert.ToInt32(ChallanIds[i]).ToString();
                //DataTable dtChallanDtl = objDbTrx.GetChallanPrintDetailsById(Convert.ToInt64(challanId));
                if (dtChallanDtl.Rows.Count > 0)
                {
                    if (TestEmail.ToUpper() == "FALSE")
                    {
                        EmailIdTo = dtChallanDtl.Rows[0]["EMAIL_ID"].ToString();
                    }
                    strMessage = new StringBuilder();
                    MailSubject = "Chargeble Challan no.: " + dtChallanDtl.Rows[0]["CHALLAN_NUMBER"].ToString() + "  Date : " + Convert.ToDateTime(dtChallanDtl.Rows[0]["CHALLAN_DATE"]).ToString("dd-MMM-yyyy");
                    strMessage.AppendLine(" <table id='InvoiceCumChallanReport' cellpadding='0' cellspacing='0' border='1' width='100%'>");
                    strMessage.AppendLine("<tr><td colspan='7'><b>PRINTING & DELIVERY OF 'NTB:</b></td></tr>");
                    strMessage.AppendLine("<tr>");
                    strMessage.AppendLine("     <td colspan='3'>Chargeble Challan no.: <b>" + dtChallanDtl.Rows[0]["CHALLAN_NUMBER"].ToString() + "</b></td>");
                    strMessage.AppendLine("     <td colspan='4' style='text-align:left;'><b>Date : " + Convert.ToDateTime(dtChallanDtl.Rows[0]["CHALLAN_DATE"]).ToString("dd-MMM-yyyy") + "</b></td>");
                    strMessage.AppendLine("</tr>");
                    strMessage.AppendLine("<tr>");
                    strMessage.AppendLine("   <td style='text-align:left;font-size:small;'>Transporter Name</td>");
                    strMessage.AppendLine("   <td style='text-align:left;font-size:small;'>" + dtChallanDtl.Rows[0]["Transport_Name"].ToString() + "</td>");
                    strMessage.AppendLine("   <td style='text-align:left;font-size:small;'>Consignment No.</td>");
                    strMessage.AppendLine("   <td style='text-align:left;font-size:small;'>" + dtChallanDtl.Rows[0]["CONSIGNEE_NO"].ToString() + "</td>");
                    strMessage.AppendLine("   <td style='text-align:left;font-size:small;'>Truck No.</td>");
                    strMessage.AppendLine("   <td style='text-align:left;font-size:small;' colspan='2'>" + dtChallanDtl.Rows[0]["VEHICLE_NO"].ToString() + "</td>");
                    strMessage.AppendLine("</tr>");
                    strMessage.AppendLine("<tr>");
                    strMessage.AppendLine("   <td style='vertical-align:bottom;text-align:left;font-size:small;width:18%;'><b>Class</b></td>");
                    strMessage.AppendLine("   <td style='vertical-align:bottom;text-align:left;font-size:small;width:18%;'><b>Book Code</b></td>");
                    strMessage.AppendLine("   <td style='vertical-align:bottom;text-align:left;font-size:small;width:18%;'><b>Name of the Books/Forms</b></td>");
                    strMessage.AppendLine("   <td style='vertical-align:bottom;text-align:left;font-size:small;width:18%;'><b>Quantity Delivered</b></td>");
                    strMessage.AppendLine("   <td style='vertical-align:bottom;text-align:left;font-size:small;width:18%;'><b>Unit</b></td>");
                    strMessage.AppendLine("   <td style='vertical-align:bottom;text-align:left;font-size:small;width:18%;'><b>No. of Box <br />bundle/ cartoon/ pack</b></td>");
                    strMessage.AppendLine("   <td style='vertical-align:bottom;text-align:left;font-size:small;width:18%;'><b>Remarks (if any)</b></td>");
                    strMessage.AppendLine("</tr>");
                    for (int iCnt = 0; iCnt < dtChallanDtl.Rows.Count; iCnt++)
                    {
                        strMessage.AppendLine("<tr>");
                        strMessage.AppendLine("   <td style='text-align:left;'>" + dtChallanDtl.Rows[iCnt]["CLASS"].ToString() + "</td>");
                        strMessage.AppendLine("   <td style='text-align:left;'>" + dtChallanDtl.Rows[iCnt]["BOOK_CODE"].ToString() + "</td>");
                        strMessage.AppendLine("   <td style='text-align:left;'>" + dtChallanDtl.Rows[iCnt]["BOOK_NAME"].ToString() + "</td>");
                        strMessage.AppendLine("   <td style='text-align:left;'>" + dtChallanDtl.Rows[iCnt]["QtyShippedQty"].ToString() + "</td>");
                        strMessage.AppendLine("   <td style='text-align:left;'>Copies</td>");
                        strMessage.AppendLine("   <td style='text-align:left;'>" + dtChallanDtl.Rows[iCnt]["Cartoon"].ToString() + "</td>");
                        strMessage.AppendLine("   <td style='text-align:left;'>" + dtChallanDtl.Rows[iCnt]["Remarks"].ToString() + "</td>");
                        strMessage.AppendLine("</tr>");
                    }
                    strMessage.AppendLine("</table>");

                    using (var message = new MailMessage(GmailUserNameKey, EmailIdTo))
                    {
                        message.Subject = MailSubject;
                        message.Body = strMessage.ToString();
                        message.IsBodyHtml = true;
                        smtp.Send(message);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMsg += ErrorMsg + " " + ex;
            }

            if (ErrorMsg == "")
            {
                ErrorMsg = "Email sent";
            }


        }
        catch (Exception ex)
        {
            ErrorMsg = "Some error occoured while sending email. " + ex.Message + ". " + ex.InnerException;
        }
        return ErrorMsg;
    }

    public static string SendForgetpassEmail(string EMAILID, string MailSubject, string MailContent)
    {
        string Msg = "";
        string ErrorMsg = "";


        try
        {
            string GmailUserNameKey = System.Web.Configuration.WebConfigurationManager.AppSettings["GmailUserNameKey"].ToString();
            string GmailPasswordKey = System.Web.Configuration.WebConfigurationManager.AppSettings["GmailPasswordKey"].ToString();
            string GmailHostKey = System.Web.Configuration.WebConfigurationManager.AppSettings["GmailHostKey"].ToString();
            string GmailPortKey = System.Web.Configuration.WebConfigurationManager.AppSettings["GmailPortKey"].ToString();
            string GmailSslKey = System.Web.Configuration.WebConfigurationManager.AppSettings["GmailSslKey"].ToString();

            string TestEmail = System.Web.Configuration.WebConfigurationManager.AppSettings["TestEmail"].ToString();
            string EmailIdTo = System.Web.Configuration.WebConfigurationManager.AppSettings["TestEmailId"].ToString();

            SmtpClient smtp = new SmtpClient();
            smtp.Host = GmailHostKey;
            smtp.Port = Convert.ToInt16(GmailPortKey);
            smtp.EnableSsl = Convert.ToBoolean(GmailSslKey);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(GmailUserNameKey, GmailPasswordKey);

            try
            {


                using (var message = new MailMessage(GmailUserNameKey, EMAILID))
                {
                    message.Subject = MailSubject;
                    message.Body = MailContent;
                    message.IsBodyHtml = true;
                    smtp.Send(message);
                }

            }
            catch (Exception ex)
            {
                ErrorMsg += ErrorMsg + " " + ex;
            }

            if (ErrorMsg == "")
            {
                ErrorMsg = "Email sent";
            }


        }
        catch (Exception ex)
        {
            ErrorMsg = "Some error occoured while sending email. " + ex.Message + ". " + ex.InnerException;
        }
        return ErrorMsg;
    }

    public static clsDirectoryDeleteStatus DeleteDirectory(string path)
    {
        try
        {
            if (Directory.Exists(path))
            {
                //Delete all files from the Directory
                foreach (string file in Directory.GetFiles(path))
                {
                    System.IO.File.Delete(file);
                }
                //Delete all child Directories
                foreach (string directory in Directory.GetDirectories(path))
                {
                    DeleteDirectory(directory);
                }
                //Delete a Directory
                Directory.Delete(path);
            }
            return new clsDirectoryDeleteStatus() { status = true, StatusMessage = "Success" };
        }
        catch (Exception excp)
        {
            return new clsDirectoryDeleteStatus() { status = false, StatusMessage = excp.Message };
        }
    }

    public static void GenerateExcel2007(string p_strPath, DataTable p_dsSrc, List<string> deleteColumnsList, bool includeChart = false, int columnNumber = 0)
    {
        if (deleteColumnsList != null && deleteColumnsList.Count() > 0)
        {
            foreach (string col in deleteColumnsList)
            {
                p_dsSrc.Columns.Remove(col);
            }
            p_dsSrc.AcceptChanges();
        }

        using (ExcelPackage objExcelPackage = new ExcelPackage())
        {
            //Create the woorkbook
            ExcelWorkbook objWorkbook = objExcelPackage.Workbook;
            //Create the worksheet    
            ExcelWorksheet objWorksheet = objWorkbook.Worksheets.Add(p_dsSrc.TableName);
            //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1    
            objWorksheet.Cells["A1"].LoadFromDataTable(p_dsSrc, true);

            int colNumber = 1;
            foreach (DataColumn col in p_dsSrc.Columns)
            {
                if (col.DataType == typeof(DateTime))
                {
                    objWorksheet.Column(colNumber).Style.Numberformat.Format = "MM/dd/yyyy hh:mm AM/PM";
                }
                colNumber++;
            }

            objWorksheet.Cells.Style.Font.SetFromFont(new Font("Calibri", 11));

            //Add autoFilter to all columns
            objWorksheet.Cells[objWorksheet.Dimension.Address].AutoFilter = true;

            ////AutoFit All Columns
            //objWorksheet.Cells.AutoFitColumns();

            //Format the header
            //var headerCells = objWorksheet.Cells[1, 1, 1, objWorksheet.Dimension.End.Column];
            using (ExcelRange objRange = objWorksheet.Cells[1, 1, 1, objWorksheet.Dimension.End.Column])
            {
                objRange.Style.Font.Bold = true;
                //objRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;    
                //objRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;    
                objRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                objRange.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFCC66"));
            }

            if (includeChart)
            {
                ExcelWorksheet objWorksheetGraph = objWorkbook.Worksheets.Add(p_dsSrc.TableName + "_Graph");
                var chart = objWorksheetGraph.Drawings.AddChart("Chart", OfficeOpenXml.Drawing.Chart.eChartType.ColumnStacked);
                //objWorksheet.Cells[1, 8, 1, 8]
                var series = chart.Series.Add(objWorksheet.Cells[1, columnNumber], objWorksheet.Cells[1, columnNumber]);

            }

            //Write it back to the client    
            if (File.Exists(p_strPath))
                File.Delete(p_strPath);

            //Create excel file on physical disk    
            FileStream objFileStrm = File.Create(p_strPath);
            objFileStrm.Close();

            //Write content to excel file
            File.WriteAllBytes(p_strPath, objExcelPackage.GetAsByteArray());
        }
    }

    public static byte[] FileAsByte(string filePath)
    {
        if (File.Exists(filePath))
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return fileBytes;
        }
        else
        {
            return null;
        }
    }
}

public class clsDirectoryDeleteStatus
{
    public bool status { get; set; }
    public string StatusMessage { get; set; }
}

public static class DataColumnCollectionExtensions
{
    public static IEnumerable<DataColumn> AsEnumerable(this DataColumnCollection source)
    {
        return source.Cast<DataColumn>();
    }
}

public class Page
{
    public int Start;
    public int End;
}