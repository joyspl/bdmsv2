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
    public class InvoiceViewController : Controller
    {
        //
        // GET: /InvoiceView/
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetDistrictDetails()
        {
            try
            {
                List<District> ObjLstDistrict = new List<District>();
                DataTable dt = objDbTrx.GetDistrictDetails();
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        District objDistrict = new District();
                        objDistrict.DistrictID = Convert.ToInt16(dt.Rows[iCnt]["ID"].ToString());
                        objDistrict.District_name = Convert.ToString(dt.Rows[iCnt]["DISTRICT"].ToString());

                        ObjLstDistrict.Add(objDistrict);
                    }
                    ViewBag.ObjDistrictList = new SelectList(ObjLstDistrict, "DistrictID", "District_name");
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(ViewBag.ObjDistrictList);
        }
        [HttpPost]
        public JsonResult GetCircleDetailsOfaDistrict(string DistrictID)
        {
            try
            {
                List<Circle> ObjLstCircle = new List<Circle>();
                DataTable dt = objDbTrx.GetCircleMasterDetailsForDistrict(Convert.ToInt32(DistrictID));
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        Circle objCircle = new Circle();
                        objCircle.CircleID = Convert.ToInt16(dt.Rows[iCnt]["ID"].ToString());
                        objCircle.Circle_name = Convert.ToString(dt.Rows[iCnt]["CIRCLE_NAME"].ToString());
                        ObjLstCircle.Add(objCircle);
                    }
                    ViewBag.ObjDistrictList = new SelectList(ObjLstCircle, "CircleID", "Circle_name");
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(ViewBag.ObjDistrictList);
        }

        public ActionResult GetInvoiceListData(string startDate, string endDate)
        {
            List<InvoiceView> objInvoiceList = new List<InvoiceView>();
            try
            {
                DataTable dt = objDbTrx.GetInvoiceViewDtl(startDate, endDate);
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        InvoiceView icc = new InvoiceView();
                        icc.InvoiceId = Convert.ToInt64(dt.Rows[iCnt]["InvoiceId"].ToString());
                        icc.InvoiceNo = Convert.ToString(dt.Rows[iCnt]["InvoiceNo"].ToString());
                        icc.InvoiceDate = Convert.ToDateTime(dt.Rows[iCnt]["InvoiceDate"].ToString()).ToString("dd-MMM-yyyy");
                        icc.Save_Status = (dt.Rows[iCnt]["Save_Status"].ToString() == "1" ? "Confirm" : "Draft");
                        icc.CategoryName = Convert.ToString(dt.Rows[iCnt]["CHALLAN_BOOK_CATEGORY"].ToString());
                        icc.UpdatedBy = Convert.ToString(dt.Rows[iCnt]["UPDATED_BY"].ToString());
                        icc.UpdatedTimeStamp = Convert.ToDateTime(dt.Rows[iCnt]["UPDATED_TS"].ToString()).ToString("dd-MMM-yyyy");
                        objInvoiceList.Add(icc);
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(objInvoiceList, JsonRequestBehavior.AllowGet);
          //  return View();
        }
        public ActionResult ExportInvoiceData(string startDate, string endDate)
        {
            
            List<InvoiceCumChallan> objChallanList = new List<InvoiceCumChallan>();
            try
            {
                StringBuilder strTableReport = new StringBuilder();
                StringBuilder strReport = new StringBuilder();
                strReport = new StringBuilder();
                DataTable dt = objDbTrx.GetInvoiceViewDtl(startDate, endDate);
                if (dt.Rows.Count > 0)
                {
                    strReport.AppendLine("<tr>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Invoice No.</th>");
                    strReport.AppendLine("  <th style='text-align:Center;' bgcolor='#b3cbff'>Invoice Date</th>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Save Status</th>");                    
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Category</th>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Updated By</th>");
                    strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Updated Time Stamp</th>");                   
                    strReport.AppendLine("</tr>");

                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                       
                        strReport.AppendLine("<tr>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["InvoiceNo"].ToString() + "      </td>");
                        strReport.AppendLine("      <td style='text-align:Center;'> " + Convert.ToDateTime(dt.Rows[iCnt]["InvoiceDate"].ToString()).ToString("dd-MMM-yyyy") + "      </td>");
                        strReport.AppendLine("      <td> " + (dt.Rows[iCnt]["Save_Status"].ToString() == "1" ? "Confirm" : "Draft") + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["CHALLAN_BOOK_CATEGORY"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["UPDATED_BY"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + Convert.ToDateTime(dt.Rows[iCnt]["UPDATED_TS"].ToString()).ToString("dd-MMM-yyyy") + "      </td>");  
                        strReport.AppendLine("</tr>");
                    }
                    strTableReport.AppendLine("<table border='1'>");
                    strTableReport.AppendLine("          " + strReport.ToString());
                    strTableReport.AppendLine("</table>");


                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    String FileName = "InvoiceData" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".xls";

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
            return null;
        }
        
    }
}
