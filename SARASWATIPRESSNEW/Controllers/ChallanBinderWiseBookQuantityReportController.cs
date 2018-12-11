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
    public class ChallanBinderWiseBookQuantityReportController : Controller
    {
        //
        // GET: /BinderWiseBookQuantityReport/
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            ChallanBinderWiseBookQtyRpt objChallanBinderWiseBookQtyRpt = new ChallanBinderWiseBookQtyRpt();

            DataTable dtBook = new DataTable();
            dtBook = objDbTrx.GetBookMasterDetails(); 
            List<Book> ObjBookDtl = new List<Book>();
            if (dtBook.Rows.Count > 0)
            {
                for (int iCnt = 0; iCnt < dtBook.Rows.Count; iCnt++)
                {
                    Book objBook     = new Book();
                    objBook.BookCode = dtBook.Rows[iCnt]["Book_Code"].ToString();
                    objBook.BookName = dtBook.Rows[iCnt]["Book_Code"].ToString() + "-" + dtBook.Rows[iCnt]["Book_Name"].ToString();
                    ObjBookDtl.Add(objBook);
                }
            }
           // objBinderWiseBookQtyRpt.startDate = Convert.ToDateTime(Convert.ToDateTime(DateTime.Now).ToString("dd-MMM-yyyy") + " 00:00:00.000");
            //objBinderWiseBookQtyRpt.endDate = Convert.ToDateTime(Convert.ToDateTime(DateTime.Now).ToString("dd-MMM-yyyy") + " 23:59:59.999");           
            objChallanBinderWiseBookQtyRpt.BookCollection = ObjBookDtl;
            return View(objChallanBinderWiseBookQtyRpt);
        }

        #region [For MVC Report Export to Excel using Crystal Report]
        [HttpGet]
        [ActionName("ExportToExcel")]
        public ActionResult ExportToExcel(string startDate, string endDate, string bookCode)
        {
            List<BinderBookQtyRpt> objBinderDtlList = new List<BinderBookQtyRpt>();
            string report_name = string.Empty;
            try
            {
                //DataTable dt = objDbTrx.GetbinderBookQtyReport(Convert.ToDateTime(startDate + " 00:00:00.000"), Convert.ToDateTime(endDate + " 23:59:59.999"), bookCode);
                DataTable dt = new DataTable();
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

        [HttpGet]
        public ActionResult GenerateExcel(string bookcode, int districtid, int circleid, string fromDate, string toDate)
        {
            return null;
        }
        #endregion [For MVC Report Export to Excel using Crystal Report]

        public ActionResult ExportChallanDataForTransporterAndBinder(string startDate, string endDate, string bookCode,string BookName)
        {

            int CircleCnt = 0;
            StringBuilder strTableReport = new StringBuilder();
            StringBuilder strReport = new StringBuilder();
            StringBuilder strHeading = new StringBuilder();
            bool found = false;
           

            try
            {

                    DataSet ds = objDbTrx.GetChallanBinderDtl(startDate, endDate, bookCode);
                    if (ds != null)
                    {
                        strHeading.AppendLine("<table>");
                        strHeading.AppendLine("    <tr><td style='text-align:Left;vertical-align: middle;' colspan='6'> Date : " + Convert.ToDateTime(startDate).ToString("dd-MMM-yyyy") + " To " + Convert.ToDateTime(endDate).ToString("dd-MMM-yyyy") + " </td></tr>");
                        strHeading.AppendLine("    <tr><td style='text-align:Left;vertical-align: middle;' colspan='6'><b> Book Name : " + bookCode + " - " + BookName + " </b></td></tr>");
                        strHeading.AppendLine("</table>");
                        strReport = new StringBuilder();                       
                        strReport.AppendLine("     <tr style='background-color:#a6b1a7'>");
                        strReport.AppendLine("           <td style='text-align:Left;vertical-align: middle;'>Challan No    </td>");
                        strReport.AppendLine("           <td style='text-align:Left;vertical-align: middle;'>Challan Date    </td>");
                        strReport.AppendLine("           <td style='text-align:Left;vertical-align: middle;'>Language   </td>");
                        strReport.AppendLine("           <td style='text-align:Left;vertical-align: middle;'>Transporter Name   </td>");                        
                        strReport.AppendLine("           <td style='text-align:Left;vertical-align: middle;'>Binder    </td>");
                        strReport.AppendLine("           <td style='text-align:Left;vertical-align: middle;'>Quantity    </td>");
                        strReport.AppendLine("     </tr>");
                        for (int bkCnt = 0; bkCnt < ds.Tables[0].Rows.Count; bkCnt++)
                        {
                            strReport.AppendLine("     <tr>");
                            strReport.AppendLine("           <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[0].Rows[bkCnt]["CHALLAN_NUMBER"].ToString() + "     </td>");
                            strReport.AppendLine("           <td style='text-align:Left;vertical-align: middle;'> " + Convert.ToDateTime(ds.Tables[0].Rows[bkCnt]["CHALLAN_DATE"]).ToString("dd-MMM-yyyy") + "     </td>");
                            strReport.AppendLine("           <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[0].Rows[bkCnt]["LANGUAGE"].ToString() + "     </td>");
                            strReport.AppendLine("           <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[0].Rows[bkCnt]["Transport_Name"].ToString() + "     </td>");
                            strReport.AppendLine("           <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[0].Rows[bkCnt]["Binder"].ToString() + "     </td>");
                            strReport.AppendLine("           <td style='text-align:Center;vertical-align: middle;'> " + ds.Tables[0].Rows[bkCnt]["QtyShippedQty"].ToString() + "     </td>");
                            strReport.AppendLine("     </tr>");
                        }
                        ds.Dispose();
                        strTableReport.AppendLine("          " + strHeading.ToString());
                        strTableReport.AppendLine("<table border='1'>");
                        strTableReport.AppendLine("          " + strReport.ToString());
                        strTableReport.AppendLine("</table>");

                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "application/vnd.ms-excel";
                        String FileName = "ChallanBinderWiseBookQtyRpt" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".xls";
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
