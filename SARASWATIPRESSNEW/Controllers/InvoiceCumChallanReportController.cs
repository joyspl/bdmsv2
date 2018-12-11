using SARASWATIPRESSNEW.BusinessLogicLayer;
using SARASWATIPRESSNEW.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SARASWATIPRESSNEW.Controllers
{
    public class InvoiceCumChallanReportController : Controller
    {
        //
        // GET: /InvoiceCumChallanReport/
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index(int isdemoprint = 0)
        {
            ViewBag.DemoPrint = isdemoprint;
            string ChallanId = string.Empty;
            if (Convert.ToString(Session["ChallanId"]) != "")
            {
                ChallanId = Convert.ToString(Session["ChallanId"]);
                Session["ChallanId"] = "";

            }
            return View(getData(ChallanId));
        }

        [HttpGet]
        public Models.InvoiceCumChallan getData(string ChallanId)
        {
            int manualQty = 1;
            int.TryParse((ConfigurationManager.AppSettings["ManualQty"] != null ? ConfigurationManager.AppSettings["ManualQty"] : "25"), out manualQty);

            InvoiceCumChallan objInvCumChallan = new InvoiceCumChallan();
            DataTable dtChallanDtl = objDbTrx.GetChallanPrintDetailsById(Convert.ToInt64(ChallanId));
            if (dtChallanDtl.Rows.Count > 0)
            {
                objInvCumChallan.InvoiceCumChallanNo = dtChallanDtl.Rows[0]["CHALLAN_NUMBER"].ToString();
                objInvCumChallan.InvoiceCumChallanDate = Convert.ToDateTime(dtChallanDtl.Rows[0]["CHALLAN_DATE"]).ToString("dd-MMM-yyyy");
                // objInvCumChallan.InvoiceCumChallanYear = Convert.ToDateTime(dtChallanDtl.Rows[0]["CHALLAN_DATE"]).ToString("yyyy");

                objInvCumChallan.InvoiceCumChallanYear = (Convert.ToDateTime(dtChallanDtl.Rows[0]["CHALLAN_DATE"]).Month >= 4 ? (Convert.ToInt32(Convert.ToDateTime(dtChallanDtl.Rows[0]["CHALLAN_DATE"]).Year.ToString()) + 1).ToString() : (Convert.ToDateTime(dtChallanDtl.Rows[0]["CHALLAN_DATE"]).Year.ToString()).ToString());

                objInvCumChallan.Transporter = dtChallanDtl.Rows[0]["Transport_Name"].ToString();
                objInvCumChallan.Language = dtChallanDtl.Rows[0]["LANGUAGE"].ToString().ToUpper();
                objInvCumChallan.CONSIGNEE_NO = dtChallanDtl.Rows[0]["CONSIGNEE_NO"].ToString();
                objInvCumChallan.VEHICLE_NO = dtChallanDtl.Rows[0]["VEHICLE_NO"].ToString();
                objInvCumChallan.DistrictName = dtChallanDtl.Rows[0]["DISTRICT"].ToString();
                objInvCumChallan.CircleName = dtChallanDtl.Rows[0]["Circle_Name"].ToString();
                objInvCumChallan.UserId = dtChallanDtl.Rows[0]["UPDATED_BY"].ToString();
                objInvCumChallan.ManualChallanNo = dtChallanDtl.Rows[0]["ManualChallanNo"].ToString();
                objInvCumChallan.BookRwCnt = dtChallanDtl.Rows.Count;
                if (objInvCumChallan.BookRwCnt > 12)
                {
                    objInvCumChallan.BookRwCnt = ((objInvCumChallan.BookRwCnt) / 12);
                    if (objInvCumChallan.BookRwCnt > 0)
                    {
                        objInvCumChallan.BookRwCnt++;
                    }
                }
                else
                {
                    objInvCumChallan.BookRwCnt = 1;
                }
                DataTable lot = objDbTrx.GetlotfromchallanID(ChallanId);
                var strlisst = new List<string>();

                List<InvoiceCumChallanList> lst_InvCumChallan = new List<InvoiceCumChallanList>();
                for (int iCnt = 0; iCnt < dtChallanDtl.Rows.Count; iCnt++)
                {
                    InvoiceCumChallanList icc = new InvoiceCumChallanList();
                    icc.ClassName = Convert.ToString(dtChallanDtl.Rows[iCnt]["Class"].ToString());
                    icc.Book_Code = Convert.ToString(dtChallanDtl.Rows[iCnt]["Common_Book_Code"].ToString());
                    icc.Book_Name = Convert.ToString(dtChallanDtl.Rows[iCnt]["book_name"].ToString());
                    icc.QtyShipped = Convert.ToInt32(dtChallanDtl.Rows[iCnt]["QtyShippedQty"].ToString());

                    //foreach (DataRow row in lot.Rows)
                    //{
                    //    if (row[0].ToString() == icc.Book_Code)
                    //    {
                    //        strlisst.Add("" + row[1] + " pack," + row[2] + " book(s)");
                    //        ////7 cartoon 25 in each pack, 1 cartoon of (reminder) in 1 pack 
                    //    }
                    //}

                    //icc.Cartoon = string.Format("{0} in each pack", string.Join("; ", strlisst));
                    //strlisst.Clear();

                    icc.Cartoon = Convert.ToString((icc.QtyShipped / manualQty)) + " bundle, " + manualQty.ToString() + " book(s) in each bundle" + ((icc.QtyShipped % manualQty) > 0 ? "; " + Convert.ToString(icc.QtyShipped % manualQty) + " book(s) in 1 pack" : "");
                    //icc.Cartoon = dtChallanDtl.Rows[iCnt]["Cartoon"].ToString();

                    icc.Remarks = dtChallanDtl.Rows[iCnt]["Remarks"].ToString();
                    icc.Weight = icc.QtyShipped * Convert.ToInt32(dtChallanDtl.Rows[iCnt]["Book_Weight"]);
                    lst_InvCumChallan.Add(icc);
                }
                objInvCumChallan.TotalAmount = 0;
                objInvCumChallan.InvoiceCumChallanCollection = lst_InvCumChallan;
                DataTable dt = objDbTrx.GetCircleMasterDetailsByCircleId(Convert.ToInt32(dtChallanDtl.Rows[0]["CircleId"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    objInvCumChallan.CircleAddress = Convert.ToString(dt.Rows[0]["CIRCLE_ADDRESS"]);
                    objInvCumChallan.CirclePinCode = Convert.ToString(dt.Rows[0]["CIRCLE_PINCODE"]);
                    objInvCumChallan.InspectorName = Convert.ToString(dt.Rows[0]["CIRCLE_OFFICER_NAME"]);
                    objInvCumChallan.InspectorPhoneNo = Convert.ToString(dt.Rows[0]["MOBILE_NO"]);
                    objInvCumChallan.InspectorEmailId = Convert.ToString(dt.Rows[0]["EMAIL_ID"]);
                    dt.Dispose();
                }
                objInvCumChallan.CustomerOrderNo = "";
                dt = objDbTrx.GetCustomerOrderNoByLanguageAndChallanCatId(Convert.ToInt16(dtChallanDtl.Rows[0]["LANGUAGE_ID"].ToString()), Convert.ToInt16(dtChallanDtl.Rows[0]["CHALLAN_BOOK_CATEGORY_ID"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    objInvCumChallan.CustomerOrderNo = Convert.ToString(dt.Rows[0]["CUST_ORDER"]);
                    dt.Dispose();
                }

                dtChallanDtl.Dispose();
            }
            return objInvCumChallan;
        }

        [HttpGet]
        public Models.InvoiceCumChallan getDataLecacy(string ChallanId)
        {
            InvoiceCumChallan objInvCumChallan = new InvoiceCumChallan();
            DataTable dtChallanDtl = objDbTrx.GetChallanPrintDetailsById(Convert.ToInt64(ChallanId));
            if (dtChallanDtl.Rows.Count > 0)
            {
                objInvCumChallan.InvoiceCumChallanNo = dtChallanDtl.Rows[0]["CHALLAN_NUMBER"].ToString();
                objInvCumChallan.InvoiceCumChallanDate = Convert.ToDateTime(dtChallanDtl.Rows[0]["CHALLAN_DATE"]).ToString("dd-MMM-yyyy");
                // objInvCumChallan.InvoiceCumChallanYear = Convert.ToDateTime(dtChallanDtl.Rows[0]["CHALLAN_DATE"]).ToString("yyyy");

                objInvCumChallan.InvoiceCumChallanYear = (Convert.ToDateTime(dtChallanDtl.Rows[0]["CHALLAN_DATE"]).Month >= 4 ? (Convert.ToInt32(Convert.ToDateTime(dtChallanDtl.Rows[0]["CHALLAN_DATE"]).Year.ToString()) + 1).ToString() : (Convert.ToDateTime(dtChallanDtl.Rows[0]["CHALLAN_DATE"]).Year.ToString()).ToString());

                objInvCumChallan.Transporter = dtChallanDtl.Rows[0]["Transport_Name"].ToString();
                objInvCumChallan.Language = dtChallanDtl.Rows[0]["LANGUAGE"].ToString().ToUpper();
                objInvCumChallan.CONSIGNEE_NO = dtChallanDtl.Rows[0]["CONSIGNEE_NO"].ToString();
                objInvCumChallan.VEHICLE_NO = dtChallanDtl.Rows[0]["VEHICLE_NO"].ToString();
                objInvCumChallan.DistrictName = dtChallanDtl.Rows[0]["DISTRICT"].ToString();
                objInvCumChallan.CircleName = dtChallanDtl.Rows[0]["Circle_Name"].ToString();
                objInvCumChallan.UserId = dtChallanDtl.Rows[0]["UPDATED_BY"].ToString();
                objInvCumChallan.BookRwCnt = dtChallanDtl.Rows.Count;
                if (objInvCumChallan.BookRwCnt > 12)
                {
                    objInvCumChallan.BookRwCnt = ((objInvCumChallan.BookRwCnt) / 12);
                    if (objInvCumChallan.BookRwCnt > 0)
                    {
                        objInvCumChallan.BookRwCnt++;
                    }
                }
                else
                {
                    objInvCumChallan.BookRwCnt = 1;
                }
                List<InvoiceCumChallanList> lst_InvCumChallan = new List<InvoiceCumChallanList>();
                for (int iCnt = 0; iCnt < dtChallanDtl.Rows.Count; iCnt++)
                {
                    InvoiceCumChallanList icc = new InvoiceCumChallanList();
                    icc.ClassName = Convert.ToString(dtChallanDtl.Rows[iCnt]["Class"].ToString());
                    icc.Book_Code = Convert.ToString(dtChallanDtl.Rows[iCnt]["Book_Code"].ToString());
                    icc.Book_Name = Convert.ToString(dtChallanDtl.Rows[iCnt]["book_name"].ToString());
                    icc.QtyShipped = Convert.ToInt32(dtChallanDtl.Rows[iCnt]["QtyShippedQty"].ToString());
                    icc.Cartoon = dtChallanDtl.Rows[iCnt]["Cartoon"].ToString();
                    icc.Remarks = dtChallanDtl.Rows[iCnt]["Remarks"].ToString();
                    lst_InvCumChallan.Add(icc);
                }
                objInvCumChallan.TotalAmount = 0;
                objInvCumChallan.InvoiceCumChallanCollection = lst_InvCumChallan;
                DataTable dt = objDbTrx.GetCircleMasterDetailsByCircleId(Convert.ToInt32(dtChallanDtl.Rows[0]["CircleId"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    objInvCumChallan.CircleAddress = Convert.ToString(dt.Rows[0]["CIRCLE_ADDRESS"]);
                    objInvCumChallan.CirclePinCode = Convert.ToString(dt.Rows[0]["CIRCLE_PINCODE"]);
                    objInvCumChallan.InspectorName = Convert.ToString(dt.Rows[0]["CIRCLE_OFFICER_NAME"]);
                    objInvCumChallan.InspectorPhoneNo = Convert.ToString(dt.Rows[0]["MOBILE_NO"]);
                    objInvCumChallan.InspectorEmailId = Convert.ToString(dt.Rows[0]["EMAIL_ID"]);
                    dt.Dispose();
                }
                objInvCumChallan.CustomerOrderNo = "";
                dt = objDbTrx.GetCustomerOrderNoByLanguageAndChallanCatId(Convert.ToInt16(dtChallanDtl.Rows[0]["LANGUAGE_ID"].ToString()), Convert.ToInt16(dtChallanDtl.Rows[0]["CHALLAN_BOOK_CATEGORY_ID"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    objInvCumChallan.CustomerOrderNo = Convert.ToString(dt.Rows[0]["CUST_ORDER"]);
                    dt.Dispose();
                }

                dtChallanDtl.Dispose();
            }
            return objInvCumChallan;
        }

        [HttpGet]
        public ActionResult ExportChallanData(string startDate, string endDate, string CircleID, string DistrictID)
        {
            //string ChallanId = string.Empty;
            //if (Convert.ToString(Session["ChallanId"]) != "")
            //{
            //    ChallanId = Convert.ToString(Session["ChallanId"]);
            //    Session["ChallanId"] = "";
            //}
            List<InvoiceCumChallan> objChallanList = new List<InvoiceCumChallan>();
            try
            {
                StringBuilder strTableReport = new StringBuilder();
                StringBuilder strReport = new StringBuilder();
                strReport = new StringBuilder();
                DataTable dt = objDbTrx.GetChallanDtl(Convert.ToDateTime(startDate + " 00:00:00.000"), Convert.ToDateTime(endDate + " 23:59:59.999"), CircleID, DistrictID);
                if (dt.Rows.Count > 0)
                {
                    strReport.AppendLine("<tr>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Chalaan No.</th>");
                    strReport.AppendLine("  <th style='text-align:Center;' bgcolor='#b3cbff'>Date</th>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>District Name</th>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Circle Name</th>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Category</th>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Language</th>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Transporter</th>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Consignee No</th>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Vehicle No</th>");
                    strReport.AppendLine("</tr>");

                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        strReport.AppendLine("<tr>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["Challan_Number"].ToString() + "      </td>");
                        strReport.AppendLine("      <td style='text-align:Center;'> " + Convert.ToDateTime(dt.Rows[iCnt]["Challan_Date"].ToString()).ToString("dd-MMM-yyyy") + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["DISTRICT"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["Circle_Name"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["CHALLAN_BOOK_CATEGORY"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["LANGUAGE"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["Transport_Name"].ToString() + "      </td>");
                        strReport.AppendLine("      <td style='text-align:left;'> " + dt.Rows[iCnt]["CONSIGNEE_NO"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["VEHICLE_NO"].ToString() + "      </td>");
                        strReport.AppendLine("</tr>");
                    }
                    strTableReport.AppendLine("<table border='1'>");
                    strTableReport.AppendLine("          " + strReport.ToString());
                    strTableReport.AppendLine("</table>");


                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    String FileName = "ChallanData" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".xls";

                    Response.AddHeader("Content-Disposition", "inline;filename=" + FileName);
                    String HTMLDataToExport = strTableReport.ToString();


                    Response.Write("<html><head><head>" +
                    HTMLDataToExport.Replace("<BR>", "<br style='mso-data-placement:same-cell;'>")
                                                    .Replace("<br>", "<br style='mso-data-placement:same-cell;'>")
                                                    .Replace("<BR >", "<br style='mso-data-placement:same-cell;'>")
                                                    .Replace("<BR />", "<br style='mso-data-placement:same-cell;'>")
                                                    .Replace("<br />", "<br style='mso-data-placement:same-cell;'>")
                                                    .Replace("<Br />", "<br style='mso-data-placement:same-cell;'>")
                                                    .Replace("<Br>", "<br style='mso-data-placement:same-cell;'>")
                                                    .Replace("<br >", "<br style='mso-data-placement:same-cell;'>") + "</html>");
                    Response.End();



                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return View("~/Views/Shared/_ExportError.cshtml");
        }
        public ActionResult ExportChallanReceivedAtCircle(string startDate, string endDate, string CircleID, string PendingOnly)
        {

            List<InvoiceCumChallan> objChallanList = new List<InvoiceCumChallan>();
            try
            {
                StringBuilder strTableReport = new StringBuilder();
                StringBuilder strReport = new StringBuilder();
                string status = "";
                strReport = new StringBuilder();
                DataTable dt = objDbTrx.GetChallanDtlByCircleId(startDate, endDate, CircleID, (PendingOnly.ToUpper() == "TRUE" ? "1" : "0"));
                if (dt.Rows.Count > 0)
                {
                    strReport.AppendLine("<tr>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Status</th>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Chalaan No.</th>");
                    strReport.AppendLine("  <th style='text-align:Center;' bgcolor='#b3cbff'>Date</th>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>District Name</th>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Circle Name</th>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Category</th>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Language</th>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Transporter</th>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Consignee No</th>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Vehicle No</th>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Last Updated By</th>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Last Updated On</th>");
                    strReport.AppendLine("</tr>");

                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        if (dt.Rows[iCnt]["RECEIVED_AT_CIRCLE"].ToString() == "1") { status = "Received"; }
                        else { status = "Pending"; }

                        strReport.AppendLine("<tr>");
                        strReport.AppendLine("      <td> " + status + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["Challan_Number"].ToString() + "      </td>");
                        strReport.AppendLine("      <td style='text-align:Center;'> " + Convert.ToDateTime(dt.Rows[iCnt]["Challan_Date"].ToString()).ToString("dd-MMM-yyyy") + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["DISTRICT"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["Circle_Name"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["CHALLAN_BOOK_CATEGORY"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["LANGUAGE"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["Transport_Name"].ToString() + "      </td>");
                        strReport.AppendLine("      <td style='text-align:left;'> " + dt.Rows[iCnt]["CONSIGNEE_NO"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["VEHICLE_NO"].ToString() + "      </td>");
                        if (dt.Rows[iCnt]["RECEIVED_AT_CIRCLE"].ToString() == "1")
                        {
                            strReport.AppendLine("      <td> " + dt.Rows[iCnt]["RECEIVED_BY"].ToString() + "      </td>");
                            strReport.AppendLine("      <td> " + Convert.ToDateTime(dt.Rows[iCnt]["RECEIVED_TS"].ToString()).ToString("dd-MMM-yyyy") + "      </td>");
                        }
                        else
                        {
                            strReport.AppendLine("      <td>&nbsp;</td>");
                            strReport.AppendLine("      <td>&nbsp;</td>");

                        }
                        strReport.AppendLine("</tr>");
                    }
                    strTableReport.AppendLine("<table border='1'>");
                    strTableReport.AppendLine("          " + strReport.ToString());
                    strTableReport.AppendLine("</table>");
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    String FileName = "CircleChallanData" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".xls";

                    Response.AddHeader("Content-Disposition", "inline;filename=" + FileName);
                    String HTMLDataToExport = strTableReport.ToString();

                    Response.Write("<html><head><head>" +
                    HTMLDataToExport.Replace("<BR>", "<br style='mso-data-placement:same-cell;'>")
                                                    .Replace("<br>", "<br style='mso-data-placement:same-cell;'>")
                                                    .Replace("<BR >", "<br style='mso-data-placement:same-cell;'>")
                                                    .Replace("<BR />", "<br style='mso-data-placement:same-cell;'>")
                                                    .Replace("<br />", "<br style='mso-data-placement:same-cell;'>")
                                                    .Replace("<Br />", "<br style='mso-data-placement:same-cell;'>")
                                                    .Replace("<Br>", "<br style='mso-data-placement:same-cell;'>")
                                                    .Replace("<br >", "<br style='mso-data-placement:same-cell;'>") + "</html>");
                    Response.End();



                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return View("~/Views/Shared/_ExportError.cshtml");
        }
    }
}
