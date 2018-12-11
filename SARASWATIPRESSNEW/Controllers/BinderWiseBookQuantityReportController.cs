using CrystalDecisions.CrystalReports.Engine;
using SARASWATIPRESSNEW.BusinessLogicLayer;
using SARASWATIPRESSNEW.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SARASWATIPRESSNEW.Controllers
{
    public class BinderWiseBookQuantityReportController : Controller
    {
        //
        // GET: /BinderWiseBookQuantityReport/
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            BinderWiseBookQtyRpt objBinderWiseBookQtyRpt = new BinderWiseBookQtyRpt();

            DataTable dtChallanRemarks = new DataTable();
            dtChallanRemarks = objDbTrx.GetBinderMaster(); 
            List<ChallanRemarks> ObjChallanRemarks = new List<ChallanRemarks>();
            if (dtChallanRemarks.Rows.Count > 0)
            {
                for (int iCnt = 0; iCnt < dtChallanRemarks.Rows.Count; iCnt++)
                {
                    ChallanRemarks chlnRemrks = new ChallanRemarks();
                    chlnRemrks.RemId = Convert.ToInt32(dtChallanRemarks.Rows[iCnt]["ID"].ToString());
                    chlnRemrks.Remarks = dtChallanRemarks.Rows[iCnt]["BinderName"].ToString();
                    ObjChallanRemarks.Add(chlnRemrks);
                }
            }
           // objBinderWiseBookQtyRpt.startDate = Convert.ToDateTime(Convert.ToDateTime(DateTime.Now).ToString("dd-MMM-yyyy") + " 00:00:00.000");
            //objBinderWiseBookQtyRpt.endDate = Convert.ToDateTime(Convert.ToDateTime(DateTime.Now).ToString("dd-MMM-yyyy") + " 23:59:59.999");           
            objBinderWiseBookQtyRpt.ChallanRemarksCollection = ObjChallanRemarks;
            return View(objBinderWiseBookQtyRpt);
        }

        #region [For MVC Report Export to Excel using Crystal Report]
        [HttpGet]
        [ActionName("BinderBookDtlExportToExcel")]
        public ActionResult BinderBookDtlExportToExcel(string startDate, string endDate, string BinderId)
        {
            List<BinderBookQtyRpt> objBinderDtlList = new List<BinderBookQtyRpt>();
            string report_name = string.Empty;
            int binderidint = default(int);
            try
            {
                int.TryParse(BinderId, out binderidint);
                DataTable dt = objDbTrx.GetbinderBookQtyReport(Convert.ToDateTime(startDate + " 00:00:00.000"), Convert.ToDateTime(endDate + " 23:59:59.999"), binderidint);
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        BinderBookQtyRpt icc = new BinderBookQtyRpt();
                        icc.binder_id = Convert.ToInt32(dt.Rows[iCnt]["BINDER_ID"].ToString());
                        icc.bindername = Convert.ToString(dt.Rows[iCnt]["BinderName"].ToString());
                        icc.book_code = Convert.ToString(dt.Rows[iCnt]["BOOK_CODE"].ToString());
                        icc.bookname = Convert.ToString(dt.Rows[iCnt]["BOOK_NAME"].ToString());
                        icc.startDate = Convert.ToDateTime(dt.Rows[iCnt]["UPDATED_TS"].ToString()).ToString("dd-MMM-yyyy");
                        icc.endDate = Convert.ToDateTime(dt.Rows[iCnt]["UPDATED_TS"].ToString()).ToString("dd-MMM-yyyy");
                        icc.language = Convert.ToString(dt.Rows[iCnt]["LANGUAGE"].ToString());
                        icc.language_id = Convert.ToInt32(dt.Rows[iCnt]["LANGUAGE_ID"].ToString());
                        icc.qtyissued = Convert.ToInt32(dt.Rows[iCnt]["QTY_ISSUED"].ToString());
                        icc.totalqty = Convert.ToInt32(dt.Rows[iCnt]["TOT_QTY"].ToString());

                        objBinderDtlList.Add(icc);
                    }
                }

                if (objBinderDtlList != null && objBinderDtlList.Count() > default(int))
                {
                    report_name = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".xls";

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "rptBinderBookQty.rpt"));
                    rd.SetDataSource(objBinderDtlList);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/octet", report_name);
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return null;
        }
        #endregion [For MVC Report Export to Excel using Crystal Report]

        public ActionResult ExportChallanData(string startDate, string endDate, string RemId)
        {

            int CircleCnt = 0;
            StringBuilder strTableReport = new StringBuilder();
            StringBuilder strReport = new StringBuilder();
            StringBuilder strHeading = new StringBuilder();
            bool found = false;
            if (RemId.Trim() == "") { RemId = "-1"; }

            try
            {

                    DataSet ds = objDbTrx.GetBinderWiseBookQtyRpt(startDate, endDate, RemId);
                    if (ds != null)
                    {
                        strHeading.AppendLine("<table>");
                        strHeading.AppendLine("    <tr><td style='text-align:center;vertical-align: middle;' colspan='5'> JOB ALLOTMENT SHEET FOR POST PRESS </td></tr>");
                        strHeading.AppendLine("    <tr>");
                        strHeading.AppendLine("         <td style='text-align:Left;vertical-align: middle;' colspan='2'> To, </td>");
                        strHeading.AppendLine("         <td style='text-align:Left;vertical-align: middle;font-weight: bold;font-size: 16px;' colspan='3'> WEST BENGAL TEXT BOOK CORP. LTD </td>");
                        strHeading.AppendLine("    </tr>");
                        strHeading.AppendLine("    <tr><td style='text-align:Left;vertical-align: middle;' colspan='5'> Asst. Mgr (M & C) </td></tr>");
                        strHeading.AppendLine("    <tr><td style='text-align:Left;vertical-align: middle;' colspan='5'> The order may be issued against vendors with given quantity. </td></tr>");
                        strHeading.AppendLine("    <tr><td style='text-align:Left;vertical-align: middle;' colspan='5'> Requisition Date : " + Convert.ToDateTime(startDate).ToString("dd-MMM-yyyy") + " To " + Convert.ToDateTime(endDate).ToString("dd-MMM-yyyy") + " </td></tr>");
                        strHeading.AppendLine("    <tr><td colspan='5'>&nbsp;</td></tr>"); 
                        strHeading.AppendLine("</table>");
                        strReport = new StringBuilder();                       
                        strReport.AppendLine("     <tr style='background-color:#a6b1a7'>");
                        strReport.AppendLine("           <td style='text-align:Left;vertical-align: middle;'> Book Code    </td>");
                        strReport.AppendLine("           <td style='text-align:Left;vertical-align: middle;' colspan='2'> Book Name    </td>");                        
                        strReport.AppendLine("           <td style='text-align:Left;vertical-align: middle;'> Language    </td>");
                        strReport.AppendLine("           <td style='text-align:Left;vertical-align: middle;'>Order Quantity    </td>");
                        strReport.AppendLine("     </tr>");
                        CircleCnt = ds.Tables[1].Rows.Count;
                        for (int bkCnt = 0; bkCnt < ds.Tables[0].Rows.Count; bkCnt++)
                        {
                            strReport.AppendLine("     <tr> <td style='text-align:Left;vertical-align: middle;font-weight: bold;' colspan='5'> " + ds.Tables[0].Rows[bkCnt]["BinderName"].ToString() + "     </td></tr>");
                           found = false;
                           for (int schCnt = 0; schCnt < ds.Tables[1].Rows.Count; schCnt++)
                           {
                               if (ds.Tables[0].Rows[bkCnt]["ID"].ToString() == ds.Tables[1].Rows[schCnt]["ID"].ToString())                               
                               {
                                   found = true;
                                   strReport.AppendLine("     <tr>");
                                   strReport.AppendLine("           <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[1].Rows[schCnt]["BOOK_CODE"].ToString() + "     </td>");
                                   strReport.AppendLine("           <td style='text-align:Left;vertical-align: middle;' colspan='2'> " + ds.Tables[1].Rows[schCnt]["BOOK_NAME"].ToString() + "     </td>");                                   
                                   strReport.AppendLine("           <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[1].Rows[schCnt]["LANGUAGE"].ToString() + "     </td>");
                                   strReport.AppendLine("           <td style='text-align:Center;vertical-align: middle;'> " + ds.Tables[1].Rows[schCnt]["QtyShippedQty"].ToString() + "     </td>");
                                   strReport.AppendLine("     </tr>");
                               }                      
                            }
                           if (found == false) {
                               strReport.AppendLine("     <tr> <td style='text-align:Left;vertical-align: middle;font-size: 12px;font-weight: bold;' colspan='5'> No Records Found    </td></tr>");
                           }
                        }
                        ds.Dispose();
                        strTableReport.AppendLine("          " + strHeading.ToString());
                        strTableReport.AppendLine("<table border='1'>");
                        strTableReport.AppendLine("          " + strReport.ToString());
                        strTableReport.AppendLine("</table>");

                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "application/vnd.ms-excel";
                        String FileName = "BinderWiseBookQtyRpt" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".xls";
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
            return View();
        }
    }
}
