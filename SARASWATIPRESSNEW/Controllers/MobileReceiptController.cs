using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SARASWATIPRESSNEW.BusinessLogicLayer;
using SARASWATIPRESSNEW.Models;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Configuration;
using Newtonsoft.Json;
namespace SARASWATIPRESSNEW.Controllers
{
    public class MobileReceiptController : ApiController
    {

        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        UserSec _objUser;

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage CheckIPDetails(string ip)
        {
            string ipstackKey = string.Empty;
            string ipstackAPI = string.Empty;
            string result = "";
            WebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                ipstackKey = ConfigurationManager.AppSettings["ipstackKey"] != null ? ConfigurationManager.AppSettings["ipstackKey"] : "09e51f44f1f8ed47c09a4e8f5c06de45";
                ipstackAPI = ConfigurationManager.AppSettings["ipstackAPI"] != null ? ConfigurationManager.AppSettings["ipstackAPI"] : "http://api.ipstack.com/";

                request = WebRequest.Create(string.Format("{0}{1}?access_key={2}", ipstackAPI, ip, ipstackKey));
                response = (HttpWebResponse)request.GetResponse();

                Stream stream = response.GetResponseStream();
                Encoding ec = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader reader = new System.IO.StreamReader(stream, ec);
                result = reader.ReadToEnd();
                reader.Close();
                stream.Close();

                RootObject obj = JsonConvert.DeserializeObject<RootObject>(result);

                return Request.CreateResponse(HttpStatusCode.OK, new { Success = 1, Message = obj });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { Success = default(int), Message = ex.Message });
            }
        }

        //http://192.168.0.118/SARASWATIPRESSNEW/api/MobileReceipt/Isvaliduser?userName=Admin&userPassword=123456
        [HttpGet]
        public HttpResponseMessage Isvaliduser(String userName, String userPassword)
        {

            DataTable dt = objDbTrx.GetBDMSLoginDtlMobile(userName, userPassword);
            if (dt.Rows.Count > 0)
            {
                DataTable dtRefInfo = new DataTable();
                _objUser = new UserSec();
                _objUser.UserType = dt.Rows[0]["USER_TYPE"].ToString();
                _objUser.UserId = dt.Rows[0]["USER_ID"].ToString();
                _objUser.DisplayName = dt.Rows[0]["DISPLAY_NAME"].ToString();
                _objUser.UserUniqueId = dt.Rows[0]["ID"].ToString();

                return Request.CreateResponse(HttpStatusCode.OK, new { Success = 1, Message = true });
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { Success = default(int), Message = false });
        }

        [HttpOptions]
        public HttpResponseMessage Options()
        {
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }

        // POST api/mobilereceipt
        [HttpPost]
        public HttpResponseMessage SaveData(MobileReceipt mobileReceipt)
        {
            string ipstackKey = string.Empty;
            string ipstackAPI = string.Empty;
            string result = "";
            WebRequest request = null;
            HttpWebResponse response = null;

            try
            {
                var userObj = objDbTrx.GetBDMSUserDtlByUserName(mobileReceipt.UserID);

                mobileReceipt.UserID = userObj != null && userObj.Rows.Count > default(int) ? userObj.Rows[0]["USER_ID"].ToString() : mobileReceipt.UserID;
                mobileReceipt.PhoneNo = userObj != null && userObj.Rows.Count > default(int) ? userObj.Rows[0]["MOBILE_NO"].ToString() : "";
                mobileReceipt.SendersIP = System.Web.HttpContext.Current.Request.UserHostAddress;
                try
                {
                    if (string.IsNullOrWhiteSpace(mobileReceipt.Place.Trim()))
                    {
                        ipstackKey = ConfigurationManager.AppSettings["ipstackKey"] != null ? ConfigurationManager.AppSettings["ipstackKey"] : "09e51f44f1f8ed47c09a4e8f5c06de45";
                        ipstackAPI = ConfigurationManager.AppSettings["ipstackAPI"] != null ? ConfigurationManager.AppSettings["ipstackAPI"] : "http://api.ipstack.com/";

                        request = WebRequest.Create(string.Format("{0}{1}?access_key={2}", ipstackAPI, mobileReceipt.SendersIP, ipstackKey));
                        response = (HttpWebResponse)request.GetResponse();

                        Stream stream = response.GetResponseStream();
                        Encoding ec = System.Text.Encoding.GetEncoding("utf-8");
                        StreamReader reader = new System.IO.StreamReader(stream, ec);
                        result = reader.ReadToEnd();
                        reader.Close();
                        stream.Close();

                        RootObject obj = JsonConvert.DeserializeObject<RootObject>(result);
                        mobileReceipt.Place = string.Format("latitude - {0}, longitude - {1}, city - {2}, region_name - {3}, pin - {4}", obj.latitude, obj.longitude, obj.city, obj.region_name, obj.zip);
                    }
                }
                catch (Exception)
                {
                    mobileReceipt.Place = mobileReceipt.Place;
                }

                var smsCodeObj = objDbTrx.GetSMSCodeByChallan(mobileReceipt.ChallanBarcode);
                if (smsCodeObj != null && smsCodeObj.Rows.Count > default(int))
                {
                    var smscode = smsCodeObj.Rows[0]["SMSCode"] != null ? smsCodeObj.Rows[0]["SMSCode"].ToString() : string.Empty;
                    if (!string.IsNullOrWhiteSpace(smscode.Trim()))
                    {
                        if (smscode.Trim() == mobileReceipt.ReceiverCode.Trim())
                        {
                            objDbTrx.MobileCircleChallanReceived(mobileReceipt);
                            try
                            {
                                DataTable dtl = objDbTrx.GetIDByChallanBarcode(mobileReceipt.ChallanBarcode);
                                string challanid = dtl != null && dtl.Rows.Count > default(int) ? dtl.Rows[0]["ID"].ToString() : string.Empty;
                                if (!string.IsNullOrWhiteSpace(challanid))
                                {
                                    SendEmailAndSmsInOneShot(challanid);
                                }
                            }
                            catch (Exception excc) { }
                            return Request.CreateResponse(HttpStatusCode.OK, new { Success = 1, Message = "Data has been submitted successfully in our system" });
                        }
                        else
                        {
                            throw new Exception(string.Format("SMS Code does not matched for {0}", mobileReceipt.ChallanBarcode));
                        }
                    }
                    else
                    {
                        throw new Exception(string.Format("SMS Code does not exists for {0}", mobileReceipt.ChallanBarcode));
                    }
                }
                else
                {
                    throw new Exception(string.Format("Wrong challan number. {0} is not exists in our system", mobileReceipt.ChallanBarcode));
                }
            }
            catch (Exception ex)
            {
                string msg = string.Empty;
                int index = ex.Message.IndexOf("\r\n");
                if (index > 0)
                    msg = ex.Message.Substring(0, index);
                else
                    msg = ex.Message;

                objDbTrx.SaveSystemErrorLog(ex, System.Web.HttpContext.Current.Request.UserHostAddress);
                return Request.CreateResponse(HttpStatusCode.OK, new { Success = default(int), Message = msg });
            }
            //return Request.CreateResponse(HttpStatusCode.OK, new { Success = 1, Message = "Data has been submitted successfully in our system" });
        }

        [NonAction]
        private string SmsSendCode(string griddata)
        {
            string Msg = "";
            string ErrorMsg = "";
            string[] ChallanIds = griddata.Split(',');
            string challanId = "";
            WebClient client = new WebClient();
            try
            {
                string SmsUserID = System.Web.Configuration.WebConfigurationManager.AppSettings["SmsUserID"].ToString();
                string SmsPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["SmsPassword"].ToString();
                string SmsSenderName = System.Web.Configuration.WebConfigurationManager.AppSettings["SmsSenderName"].ToString();
                string TestSms = System.Web.Configuration.WebConfigurationManager.AppSettings["TestSms"].ToString();
                string TestMobileNo = System.Web.Configuration.WebConfigurationManager.AppSettings["TestMobileNo"].ToString();
                string MobileNo = TestMobileNo;
                string TextMsg = "";
                string baseurl = "";
                string MobileNoForRcvdChallanSMS = string.Empty;
                List<string> mobileNos = new List<string>();
                ErrorMsg = "";
                System.Threading.ThreadPool.QueueUserWorkItem(s =>
                {
                    for (int i = 0; i < ChallanIds.Count(); i++)
                    {
                        try
                        {
                            MobileNoForRcvdChallanSMS = ConfigurationManager.AppSettings["MobileNoForRcvdChallanSMS"] != null ? ConfigurationManager.AppSettings["MobileNoForRcvdChallanSMS"] : "8017331530,8420289888";
                            if (!string.IsNullOrEmpty(MobileNoForRcvdChallanSMS))
                            {
                                mobileNos = MobileNoForRcvdChallanSMS.Split(',').ToList();
                            }

                            challanId = Convert.ToInt32(ChallanIds[i]).ToString();
                            DataTable dtChallanDtl = objDbTrx.GetChallanDetailsById(Convert.ToInt64(challanId));
                            if (dtChallanDtl.Rows.Count > 0)
                            {
                                if (TestSms.ToUpper() == "FALSE")
                                {
                                    MobileNo = dtChallanDtl.Rows[0]["MOBILE_NO"].ToString();
                                }

                                #region [Changed Code]
                                if (dtChallanDtl.Rows[0]["ConfirmStatus"].ToString() == "1")
                                {
                                   // Dear Sir, challan no~has been successfully delivered. Thank you for your kind cooperation.
                                    //string smsBody = string.Format("Dear Sir challan no {0} to be delivered soon. Unique code is {1} Please share the code to transporter at the time of receiving of {2}",
                                    string smsBody = string.Format("Dear Sir, challan no {0} has been successfully delivered. Thank you for your kind cooperation.",
                                    Convert.ToString(dtChallanDtl.Rows[0]["CHALLAN_NUMBER"].ToString()),
                                    Convert.ToString(dtChallanDtl.Rows[0]["SMSCode"].ToString()),
                                    "Book");
                                    Utility.SendSMS(Convert.ToString(dtChallanDtl.Rows[0]["MOBILE_NO"].ToString()), smsBody);
                                    Utility.SendSMS(Convert.ToString(dtChallanDtl.Rows[0]["ALTERNATE_MOBILE_NO"].ToString()), smsBody);

                                    if (mobileNos != null && mobileNos.Count() > default(int))
                                    {
                                        foreach (var itm in mobileNos)
                                        {
                                            Utility.SendSMS(itm, smsBody);
                                        }
                                    }
                                }
                                #endregion [Changed Code]
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorMsg += ErrorMsg + " " + ex;
                        }
                    }
                });
                if (ErrorMsg == "")
                {
                    ErrorMsg = "SMS sent sucsscessfully.....";
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = "Some error occured while sending mail " + ex.Message;
            }

            return ErrorMsg;
        }

        [NonAction]
        private string EmailSendCode(string griddata)
        {
            string Msg = "";
            string ErrorMsg = "";
            string MailSubject = "";
            StringBuilder strMessage = new StringBuilder();
            string[] ChallanIds = griddata.Split(',');
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

                System.Threading.ThreadPool.QueueUserWorkItem(s =>
                {
                    for (int i = 0; i < ChallanIds.Count(); i++)
                    {
                        try
                        {
                            challanId = Convert.ToInt32(ChallanIds[i]).ToString();
                            DataTable dtChallanDtl = objDbTrx.GetChallanPrintDetailsById(Convert.ToInt64(challanId));
                            if (dtChallanDtl.Rows.Count > 0)
                            {
                                if (TestEmail.ToUpper() == "FALSE")
                                {
                                    EmailIdTo = dtChallanDtl.Rows[0]["EMAIL_ID"].ToString();
                                }
                                if (dtChallanDtl.Rows[0]["ConfirmStatus"].ToString() == "1")
                                {
                                    strMessage = new StringBuilder();
                                    MailSubject = "Chargeble Challan no.: " + dtChallanDtl.Rows[0]["CHALLAN_NUMBER"].ToString() + "  Date : " + Convert.ToDateTime(dtChallanDtl.Rows[0]["CHALLAN_DATE"]).ToString("dd-MMM-yyyy");
                                    strMessage.AppendLine(" <table id='InvoiceCumChallanReport' cellpadding='0' cellspacing='0' border='1' width='100%'>");
                                    strMessage.AppendLine("<tr><td colspan='4'><b>STATUS FOR PRINTING & DELIVERY OF 'NTB:</b></td>");
                                    strMessage.AppendLine("<tr>");
                                    strMessage.AppendLine("     <td colspan='3'>Delivery has been completed against Chargeble Challan no.: <b>" + dtChallanDtl.Rows[0]["CHALLAN_NUMBER"].ToString() + "</b></td>");
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
                        }
                        catch (Exception ex)
                        {
                            ErrorMsg += ErrorMsg + " " + ex;
                        }
                    }
                });
                if (ErrorMsg == "")
                {
                    ErrorMsg = "email sent sucsscessfully.....";
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = "Some error occoured while sending email. " + ex.Message + ". " + ex.InnerException;
            }
            return ErrorMsg;
        }

        [NonAction]
        private bool SendEmailAndSmsInOneShot(string griddata)
        {
            var smsResult = SmsSendCode(griddata);
            var emailResult = EmailSendCode(griddata);
            return true;
        }
    }
}