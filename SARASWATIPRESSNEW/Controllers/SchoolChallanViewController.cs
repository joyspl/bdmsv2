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
    public class SchoolChallanViewController : Controller
    {
        //
        // GET: /SchoolChallanView/
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetReqViewData(string SchoolId, string startDate, string endDate)
        {
            List<SchoolChallan> objReq = new List<SchoolChallan>();
            try
            {
                string CircleId = "-1";
                try { CircleId = ((UserSec)Session["UserSec"]).CircleID; }
                catch { CircleId = "-1"; }
                DataTable dtReqView = objDbTrx.GetSchoolChallanViewBySchooldId(Convert.ToInt64(SchoolId), Convert.ToInt64(CircleId), startDate, endDate);
                if (dtReqView.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dtReqView.Rows.Count; iCnt++)
                    {
                        SchoolChallan rq = new SchoolChallan();
                        rq.SchoolChallanUniqueId = Convert.ToInt64(dtReqView.Rows[iCnt]["ID"].ToString());
                        rq.RequisitionId = Convert.ToInt64(dtReqView.Rows[iCnt]["REQUISITION_ID"].ToString());
                        rq.SchoolChallanCode = Convert.ToString(dtReqView.Rows[iCnt]["SCH_CHALLAN_CODE"].ToString());
                        rq.SchoolChallanDate = Convert.ToDateTime(dtReqView.Rows[iCnt]["SCH_CHALLAN_DATE"].ToString()).ToString("dd-MMM-yyyy").ToUpper();                       
                        rq.ReqCode = Convert.ToString(dtReqView.Rows[iCnt]["REQ_CODE"].ToString());
                        rq.RequisitionDate = Convert.ToDateTime(dtReqView.Rows[iCnt]["REQUISITION_DATE"].ToString()).ToString("dd-MMM-yyyy").ToUpper();
                        rq.ChallanUpdatedBy = Convert.ToString(dtReqView.Rows[iCnt]["UPDATED_BY"].ToString());
                        rq.ChallanUpdatedTs = Convert.ToDateTime(dtReqView.Rows[iCnt]["UPDATED_TS"].ToString()).ToString("dd-MMM-yyyy hh:mm tt").ToUpper();
                        rq.SchoolCode = Convert.ToString(dtReqView.Rows[iCnt]["SCHOOL_CODE"].ToString());
                        rq.SchoolName = Convert.ToString(dtReqView.Rows[iCnt]["SCHOOL_NAME"].ToString());
                        objReq.Add(rq);
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(objReq);
        }

        public ActionResult ExportSchChallanData(string SchoolId, string startDate, string endDate)
        {
           
            List<InvoiceCumChallan> objChallanList = new List<InvoiceCumChallan>();
            try
            {
                StringBuilder strTableReport = new StringBuilder();
                StringBuilder strReport = new StringBuilder();
                strReport = new StringBuilder();
                string CircleId = "-1";
                try { CircleId = ((UserSec)Session["UserSec"]).CircleID; }
                catch { CircleId = "-1"; }
                DataTable dt = objDbTrx.GetSchoolChallanViewBySchooldId(Convert.ToInt64(SchoolId), Convert.ToInt64(CircleId), startDate, endDate);
                if (dt.Rows.Count > 0)
                {
                    strReport.AppendLine("<tr>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Challan Code</th>");
                    strReport.AppendLine("  <th style='text-align:Center;' bgcolor='#b3cbff'>Challan Date</th>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Requisiting Code</th>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Requisition Date</th>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>School Name</th>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>School Code</th>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Challan Updated By</th>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Challan Updated On</th>");
                    strReport.AppendLine("</tr>");
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        strReport.AppendLine("<tr>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["SCH_CHALLAN_CODE"].ToString() + "      </td>");
                        strReport.AppendLine("      <td style='text-align:Center;'> " + Convert.ToDateTime(dt.Rows[iCnt]["SCH_CHALLAN_DATE"].ToString()).ToString("dd-MMM-yyyy") + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["REQ_CODE"].ToString() + "      </td>");
                        strReport.AppendLine("      <td style='text-align:Center;'> " + Convert.ToDateTime(dt.Rows[iCnt]["REQUISITION_DATE"].ToString()).ToString("dd-MMM-yyyy") + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["SCHOOL_NAME"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["SCHOOL_CODE"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["UPDATED_BY"].ToString() + "      </td>");
                        strReport.AppendLine("      <td style='text-align:Center;'> " +  Convert.ToDateTime(dt.Rows[iCnt]["UPDATED_TS"].ToString()).ToString("dd-MMM-yyyy hh:mm tt").ToUpper() + "      </td>");
                        strReport.AppendLine("</tr>");

                    }
                    strTableReport.AppendLine("<table border='1'>");
                    strTableReport.AppendLine("          " + strReport.ToString());
                    strTableReport.AppendLine("</table>");


                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    String FileName = "SchChlnData" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".xls";

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
