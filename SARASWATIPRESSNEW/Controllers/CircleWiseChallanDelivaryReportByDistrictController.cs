using SARASWATIPRESSNEW.BusinessLogicLayer;
using SARASWATIPRESSNEW.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SARASWATIPRESSNEW.Controllers
{
    public class CircleWiseChallanDelivaryReportByDistrictController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(CircleWiseChallanDeliveryReport objcust)
        {
            int CircleCnt = 0, BokCnt = 0;
            Int64 RemainingCnt = 0;
            string DistrictId = "";
            StringBuilder strTableReport = new StringBuilder();
            StringBuilder strReport = new StringBuilder();
            StringBuilder strReport1 = new StringBuilder();
            StringBuilder strReport2 = new StringBuilder();
            StringBuilder strReportSchoolNameHead = new StringBuilder();
            StringBuilder strTotReport = new StringBuilder();
            try { DistrictId = ((UserSec)Session["UserSec"]).DistrictID; }
            catch { DistrictId = ""; }
            string usertype = string.Empty;

            try
            {
                usertype = (((UserSec)Session["UserSec"])).UserType;
            }
            catch { }
            try
            {
                if (DistrictId != "")
                {
                    DataSet ds = objDbTrx.GetCircleWiseChallanDelivaryReport(Convert.ToInt64(DistrictId), usertype);
                    if (ds != null)
                    {
                        CircleCnt = ds.Tables[1].Rows.Count;
                        for (int bkCnt = 0; bkCnt < ds.Tables[0].Rows.Count; bkCnt++)
                        {
                            strReport = new StringBuilder();
                            strReport1 = new StringBuilder();
                            strReport2 = new StringBuilder();
                            //rowspan='3'
                            strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;border-top: 2pt solid black;' > " + ds.Tables[0].Rows[bkCnt]["BOOK_CODE"].ToString() + "     </td>");
                            strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;border-top: 2pt solid black;' > " + ds.Tables[0].Rows[bkCnt]["CLASS"].ToString() + "         </td>");
                            strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;border-top: 2pt solid black;' > " + ds.Tables[0].Rows[bkCnt]["BOOK_NAME"].ToString() + "     </td>");
                            strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;border-top: 2pt solid black;' > " + ds.Tables[0].Rows[bkCnt]["LANGUAGE"].ToString() + "      </td>");
                            strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;border-top: 2pt solid black;'>Net Requisition Quantity</td>");

                            strReport1.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[0].Rows[bkCnt]["BOOK_CODE"].ToString() + "     </td>");
                            strReport1.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[0].Rows[bkCnt]["CLASS"].ToString() + "         </td>");
                            strReport1.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[0].Rows[bkCnt]["BOOK_NAME"].ToString() + "     </td>");
                            strReport1.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[0].Rows[bkCnt]["LANGUAGE"].ToString() + "      </td>");
                            strReport1.AppendLine("      <td style='text-align:Left;vertical-align: middle;'>Already Shiped Quantity</td>");

                            strReport2.AppendLine("      <td style='text-align:Left;vertical-align: middle;border-bottom: 2pt solid black;'> " + ds.Tables[0].Rows[bkCnt]["BOOK_CODE"].ToString() + "     </td>");
                            strReport2.AppendLine("      <td style='text-align:Left;vertical-align: middle;border-bottom: 2pt solid black;'> " + ds.Tables[0].Rows[bkCnt]["CLASS"].ToString() + "         </td>");
                            strReport2.AppendLine("      <td style='text-align:Left;vertical-align: middle;border-bottom: 2pt solid black;'> " + ds.Tables[0].Rows[bkCnt]["BOOK_NAME"].ToString() + "     </td>");
                            strReport2.AppendLine("      <td style='text-align:Left;vertical-align: middle;border-bottom: 2pt solid black;'> " + ds.Tables[0].Rows[bkCnt]["LANGUAGE"].ToString() + "      </td>");
                            strReport2.AppendLine("      <td style='text-align:Left;vertical-align: middle;border-bottom: 2pt solid black;'>Quantity for Shiping</td>");


                            //PrintSchoolDtl = true;
                            for (int schCnt = 0; schCnt < ds.Tables[1].Rows.Count; schCnt++)
                            {
                                if (bkCnt == 0)
                                {
                                    strReportSchoolNameHead.AppendLine("      <td style='text-align:Left;vertical-align: middle;' > " + ds.Tables[1].Rows[schCnt]["CIRCLE_NAME"].ToString() + "    </td>");
                                }

                                bool found = false;
                                for (int iCnt = 0; iCnt < ds.Tables[2].Rows.Count; iCnt++)
                                {

                                    if (ds.Tables[0].Rows[bkCnt]["BOOK_CODE"].ToString() == ds.Tables[2].Rows[iCnt]["BOOK_CODE"].ToString() && ds.Tables[1].Rows[schCnt]["ID"].ToString() == ds.Tables[2].Rows[iCnt]["ID"].ToString() && ds.Tables[0].Rows[bkCnt]["LANGUAGE"].ToString() == ds.Tables[2].Rows[iCnt]["LANGUAGE"].ToString())
                                    {
                                        found = true;
                                        if (Convert.ToInt64(ds.Tables[2].Rows[iCnt]["NetReqQtyAfterStockDeduction"].ToString()) > 0)
                                        {
                                            strReport.AppendLine("      <td style='text-align:center;vertical-align: middle;' bgcolor='#b3cbff' > " + ds.Tables[2].Rows[iCnt]["NetReqQtyAfterStockDeduction"].ToString() + "      </td>");
                                        }
                                        else
                                        {
                                            strReport.AppendLine("      <td style='text-align:center;vertical-align: middle;' > " + ds.Tables[2].Rows[iCnt]["NetReqQtyAfterStockDeduction"].ToString() + "      </td>");
                                        }
                                        if (Convert.ToInt64(ds.Tables[2].Rows[iCnt]["AlreadyShipped"].ToString()) > 0)
                                        {
                                            strReport1.AppendLine("      <td style='text-align:center;vertical-align: middle;' bgcolor='#99ff99' > " + ds.Tables[2].Rows[iCnt]["AlreadyShipped"].ToString() + "      </td>");
                                        }
                                        else
                                        {
                                            strReport1.AppendLine("      <td style='text-align:center;vertical-align: middle;' > " + ds.Tables[2].Rows[iCnt]["AlreadyShipped"].ToString() + "      </td>");
                                        }
                                        try
                                        {
                                            RemainingCnt = (Convert.ToInt64(ds.Tables[2].Rows[iCnt]["NetReqQtyAfterStockDeduction"].ToString()) - Convert.ToInt64(ds.Tables[2].Rows[iCnt]["AlreadyShipped"].ToString()));
                                            if(RemainingCnt<0){
                                                RemainingCnt=0;
                                            }
                                        }
                                        catch
                                        {
                                            RemainingCnt=0;
                                        }
                                        if (objcust.IsDetailsRequire == true)
                                        {
                                            strReport2.AppendLine("      <td style='text-align:center;vertical-align: middle;' bgcolor='#ffff99' >  Remaining: <b>" + RemainingCnt + "</b> Req: <b>" + ds.Tables[2].Rows[iCnt]["TOTAL"].ToString() + " </b> Stock: <b>" + ds.Tables[2].Rows[iCnt]["STOCK_TOTAL"].ToString() + "</b> </td>");
                                        }
                                        else
                                        {
                                            if (RemainingCnt > 0)
                                            {
                                                strReport2.AppendLine("      <td style='text-align:center;vertical-align: middle;' bgcolor='#ffff99' > " + RemainingCnt + "      </td>");
                                            }
                                            else
                                            {
                                                strReport2.AppendLine("      <td style='text-align:center;vertical-align: middle;' > " + RemainingCnt + "      </td>");
                                            }                                           
                                        }
                                        if (ds.Tables[2].Rows.Count != 1)
                                        {
                                            ds.Tables[2].Rows.RemoveAt(iCnt);
                                        }
                                    }
                                    if (found == true)
                                    {
                                        break;
                                    }
                                }
                                if (found == false)
                                {
                                    strReport.AppendLine("      <td style='text-align:center;vertical-align: middle;' > 0</td>");
                                    strReport1.AppendLine("      <td style='text-align:center;vertical-align: middle;' > 0</td>");
                                    strReport2.AppendLine("      <td style='text-align:center;vertical-align: middle;' > 0</td>");
                                }
                            }                            
                            strTotReport.AppendLine("<tr>" + strReport.ToString() + "</tr>");
                            strTotReport.AppendLine("<tr>" + strReport1.ToString() + "</tr>");
                            strTotReport.AppendLine("<tr>" + strReport2.ToString() + "</tr>");
                        }
                        ds.Dispose();                        
                        //strReportSchoolCodeHead.AppendLine("      <td style='text-align:Left;'> &nbsp; </td>");
                        //strReportSchoolNameHead.AppendLine("      <td style='text-align:Left;'> Stock Qty.   </td>");
                        //strTableReport.AppendLine("<table border='1' width='" + (((CircleCnt * 2) + 5) * 50) + "Px' >");
                        strTableReport.AppendLine("<table border='1' >");
                        strTableReport.AppendLine("     <tr><td colspan='" + (CircleCnt + 5) + "'> " + "Challan Delivery  Report of District- <B>" + ds.Tables[1].Rows[0]["DISTRICT"].ToString() + "</b></td></td></tr>");
                        strTableReport.AppendLine("     <tr><td colspan='" + (CircleCnt + 5) + "'> " + "From  <B>" + Convert.ToDateTime(objcust.startDate).ToString("dd-MMM-yyyy") + "</b> To <b>" + Convert.ToDateTime(objcust.endDate).ToString("dd-MMM-yyyy") + "</b></td></td></tr>");
                        strTableReport.AppendLine("     <tr>");
                        strTableReport.AppendLine("         <td style='text-align:Left;'>Book Code</td>");
                        strTableReport.AppendLine("         <td style='text-align:Left;'>Class</td>");
                        strTableReport.AppendLine("         <td style='text-align:Left;'>TITLE OF BOOKS</td>");
                        strTableReport.AppendLine("         <td>Language</td>");
                        strTableReport.AppendLine("         <td>&nbsp;</td>");
                        strTableReport.AppendLine("          " + strReportSchoolNameHead.ToString());
                        strTableReport.AppendLine("     </tr>");
                        strTableReport.AppendLine("          " + strTotReport.ToString());
                        strTableReport.AppendLine("</table>");

                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "application/vnd.ms-excel";
                        String FileName = "CircleChallanDeliveryRpt" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".xls";
                        Response.AddHeader("Content-Disposition", "inline;filename=" + FileName);
                        String HTMLDataToExport = strTableReport.ToString();
                        //System.IO.FileInfo fiCSSInfo = new System.IO.FileInfo(Server.MapPath("/resources/css/ReportFormat.css"));
                        //System.Text.StringBuilder sbCSSInfo = new System.Text.StringBuilder();
                        //System.IO.StreamReader srCSSInfo = fiCSSInfo.OpenText();
                        //while (srCSSInfo.Peek() > 0)
                        //{
                        //    sbCSSInfo.Append(srCSSInfo.ReadLine());
                        //}
                        //srCSSInfo.Close();

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
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return View(get_dropdown());
        }

        public Models.CircleWiseChallanDeliveryReport get_dropdown()
        {
            CircleWiseChallanDeliveryReport lst_req = new CircleWiseChallanDeliveryReport();
            List<District> lst_req2 = new List<District>();
            try
            {
                DataTable dtMastData = objDbTrx.GetDistrictDetails();
                if (dtMastData.Rows.Count > 0)
                {
                    for (int i = 0; i < dtMastData.Rows.Count; i++)
                    {
                        District rq = new District();
                        rq.DistrictID = Convert.ToInt32(dtMastData.Rows[i]["ID"].ToString());
                        rq.District_name = Convert.ToString(dtMastData.Rows[i]["DISTRICT"].ToString());
                        lst_req2.Add(rq);
                    }
                }
                lst_req.districtcollectionlist = lst_req2;
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return lst_req;
        }

    }
}
