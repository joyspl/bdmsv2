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
    public class DistSummaryReportController : Controller
    {
        //
        // GET: /DistSummaryReport/

        public ActionResult Index()
        {
            return View();
        }

        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();

        #region [For MVC Report Export to Excel using Crystal Report]
        [HttpGet]
        [ActionName("DistBookSummaryReportExportToExcel")]
        public FileResult DistBookSummaryReportExportToExcel(string startDate, string endDate, int DistrictId)
        {
            List<SummaryReport> objBookSummaryReport = new List<SummaryReport>();
            string report_name = string.Empty;
            try
            {
                DataTable dt = objDbTrx.GetDistBookSummaryRpt(Convert.ToDateTime(startDate + " 00:00:00.000"), Convert.ToDateTime(endDate + " 23:59:59.999"), DistrictId);
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        SummaryReport bksr = new SummaryReport();
                        bksr.DistrictId = Convert.ToInt32(dt.Rows[iCnt]["DISTRICT_ID"].ToString());
                        bksr.DistrictName = Convert.ToString(dt.Rows[iCnt]["DISTRICT"].ToString());
                        bksr.CircleId = Convert.ToInt32(dt.Rows[iCnt]["ID"].ToString());
                        bksr.CIRCLE_NAME = Convert.ToString(dt.Rows[iCnt]["CIRCLE_NAME"].ToString());
                        bksr.Total_Requisition_Quantity = Convert.ToInt64(dt.Rows[iCnt]["Total_Requisition_QUantity"].ToString());
                        bksr.recvd_challan_qty = Convert.ToInt64(dt.Rows[iCnt]["recvd_challan_qty"].ToString());
                        bksr.books_delivered = Convert.ToInt64(dt.Rows[iCnt]["books_delivered"].ToString());
                        bksr.school_challan_Quantity = Convert.ToInt64(!string.IsNullOrWhiteSpace(dt.Rows[iCnt]["school_challan_Quantity"].ToString()) ? dt.Rows[iCnt]["school_challan_Quantity"].ToString() : "0");

                        objBookSummaryReport.Add(bksr);
                    }
                }

                if (objBookSummaryReport != null && objBookSummaryReport.Count() > default(int))
                {
                    report_name = DateTime.Now.ToString("ClrcBookRptddMMyyyy") + ".pdf";

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "rptDistBookSummaryReport.rpt"));
                    rd.SetDataSource(objBookSummaryReport);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
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

        #region [For MVC Report Export to Excel using Crystal Report]
        [HttpGet]
        [ActionName("DistKhataSummaryReportExportToExcel")]
        public FileResult DistKhataSummaryReportExportToExcel(string startDate, string endDate, int DistrictId)
        {
            List<SummaryReport> objKhataSummaryReport = new List<SummaryReport>();
            string report_name = string.Empty;
            try
            {
                DataTable dt = objDbTrx.GetDistKhataSummaryRpt(Convert.ToDateTime(startDate + " 00:00:00.000"), Convert.ToDateTime(endDate + " 23:59:59.999"), DistrictId);
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        SummaryReport bksr = new SummaryReport();
                        bksr.DistrictId = Convert.ToInt32(dt.Rows[iCnt]["DISTRICT_ID"].ToString());
                        bksr.DistrictName = Convert.ToString(dt.Rows[iCnt]["DISTRICT"].ToString());
                        bksr.CircleId = Convert.ToInt32(dt.Rows[iCnt]["ID"].ToString());
                        bksr.CIRCLE_NAME = Convert.ToString(dt.Rows[iCnt]["CIRCLE_NAME"].ToString());
                        bksr.Total_Requisition_Quantity = Convert.ToInt64(dt.Rows[iCnt]["Total_Requisition_QUantity"].ToString());
                        bksr.recvd_challan_qty = Convert.ToInt64(dt.Rows[iCnt]["recvd_challan_qty"].ToString());
                        bksr.books_delivered = Convert.ToInt64(dt.Rows[iCnt]["books_delivered"].ToString());
                        bksr.school_challan_Quantity = Convert.ToInt64(!string.IsNullOrWhiteSpace(dt.Rows[iCnt]["school_challan_Quantity"].ToString()) ? dt.Rows[iCnt]["school_challan_Quantity"].ToString() : "0");

                        objKhataSummaryReport.Add(bksr);
                    }
                }

                if (objKhataSummaryReport != null && objKhataSummaryReport.Count() > default(int))
                {
                    report_name = DateTime.Now.ToString("ClrcKhataRptddMMyyyy") + ".pdf";

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "rptDistKhataSummaryReport.rpt"));
                    rd.SetDataSource(objKhataSummaryReport);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
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
    }
}
