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
    public class UnBilledChallanDtlByDistrictController : Controller
    {
        //
        // GET: /UnBilledChallanDtlByDistrict/
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

        #region [For MVC Report Export to Excel using Crystal Report]
        [HttpGet]
        [ActionName("GDBNBExportToExcel")]
        public FileResult GDBNBExportToExcel()
            
        {
            List<InvoiceCumChallan> objGDBNB = new List<InvoiceCumChallan>();
            string report_name = string.Empty;
            try
            {
                DataTable dt = objDbTrx.GetGDBNB();
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        try
                        {
                            InvoiceCumChallan gdbnb = new InvoiceCumChallan();
                            //gdbnb.DistrictId = Convert.ToInt32(dt.Rows[iCnt]["DISTRICT_ID"].ToString());
                            //gdbnb.CircleId = Convert.ToInt32(dt.Rows[iCnt]["CircleId"].ToString());
                            gdbnb.InvoiceCumChallanNo = Convert.ToString(dt.Rows[iCnt]["CHALLAN_NUMBER"].ToString());
                            gdbnb.InvoiceCumChallanDate  = Convert.ToString(dt.Rows[iCnt]["CHALLAN_DATE"].ToString());
                            gdbnb.TotalAmount = Convert.ToInt32(dt.Rows[iCnt]["QtyShippedQty"].ToString());
                            gdbnb.DistrictName  = Convert.ToString(dt.Rows[iCnt]["DISTRICT"].ToString());
                            gdbnb.CircleName = Convert.ToString(dt.Rows[iCnt]["CIRCLE_NAME"].ToString());
                            gdbnb.Transporter  = Convert.ToString(dt.Rows[iCnt]["Transport_Name"].ToString());
                            gdbnb.CONSIGNEE_NO  = Convert.ToString(dt.Rows[iCnt]["CONSIGNEE_NO"].ToString());
                            gdbnb.VEHICLE_NO = Convert.ToString(dt.Rows[iCnt]["VEHICLE_NO"].ToString());
                            

                            objGDBNB.Add(gdbnb);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }

                if (objGDBNB != null && objGDBNB.Count() > default(int))
                {
                    report_name = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".xls";

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "rptGDBNB.rpt"));
                    rd.SetDataSource(objGDBNB);

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

        #region [For MVC Report Export to Excel using Crystal Report]
        [HttpGet]
        [ActionName("GDBNBdtlExportToExcel")]
        public FileResult GDBNBdtlExportToExcel()
        {
            List<InvoiceCumChallan> objGDBNBdtl = new List<InvoiceCumChallan>();
            string report_name = string.Empty;
            try
            {
                DataTable dt = objDbTrx.GetGDBNBdtl();
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        try
                        {
                            InvoiceCumChallan gdbnbdtl = new InvoiceCumChallan();
                            //gdbnb.DistrictId = Convert.ToInt32(dt.Rows[iCnt]["DISTRICT_ID"].ToString());
                            //gdbnb.CircleId = Convert.ToInt32(dt.Rows[iCnt]["CircleId"].ToString());
                            gdbnbdtl.InvoiceCumChallanNo = Convert.ToString(dt.Rows[iCnt]["CHALLAN_NUMBER"].ToString());
                            gdbnbdtl.InvoiceCumChallanDate = Convert.ToString(dt.Rows[iCnt]["CHALLAN_DATE"].ToString());
                            gdbnbdtl.book_code = Convert.ToString(dt.Rows[iCnt]["book_code"].ToString());
                            gdbnbdtl.BOOK_NAME = Convert.ToString(dt.Rows[iCnt]["BOOK_NAME"].ToString());
                            gdbnbdtl.TotalAmount = Convert.ToInt32(dt.Rows[iCnt]["QtyShippedQty"].ToString());
                            gdbnbdtl.DistrictName = Convert.ToString(dt.Rows[iCnt]["DISTRICT"].ToString());
                            gdbnbdtl.CircleName = Convert.ToString(dt.Rows[iCnt]["CIRCLE_NAME"].ToString());
                            gdbnbdtl.Transporter = Convert.ToString(dt.Rows[iCnt]["Transport_Name"].ToString());
                            gdbnbdtl.CONSIGNEE_NO = Convert.ToString(dt.Rows[iCnt]["CONSIGNEE_NO"].ToString());
                            gdbnbdtl.VEHICLE_NO = Convert.ToString(dt.Rows[iCnt]["VEHICLE_NO"].ToString());


                            objGDBNBdtl.Add(gdbnbdtl);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }

                if (objGDBNBdtl != null && objGDBNBdtl.Count() > default(int))
                {
                    report_name = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".xls";

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "rptGDBNBdtl.rpt"));
                    rd.SetDataSource(objGDBNBdtl);

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
        
        //public ActionResult ExportUnBilledData(string DistrictID, string startDate, string endDate)
        //{

        //    List<InvoiceCumChallan> objChallanList = new List<InvoiceCumChallan>();
        //    try
        //    {
        //        StringBuilder strTableReport = new StringBuilder();
        //        StringBuilder strReport = new StringBuilder();             
               
        //        DataTable dt = objDbTrx.GetUnBilledChallanDtlByDistrict(Convert.ToInt64(DistrictID), startDate, endDate);
        //        if (dt.Rows.Count > 0)
        //        {
        //            strReport.AppendLine("<tr>");
        //            strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Challan No</th>");
        //            strReport.AppendLine("  <th style='text-align:Center;' bgcolor='#b3cbff'>Challan Date</th>");
        //            strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Consignee No</th>");
        //            strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Veichle No</th>");
        //            strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Transporter Name</th>");
        //            strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>Circle Name</th>");
        //            strReport.AppendLine("  <th style='text-align:left;' bgcolor='#b3cbff'>District Name</th>");
        //            strReport.AppendLine("</tr>");
        //            for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
        //            {
        //                strReport.AppendLine("<tr>");
        //                strReport.AppendLine("      <td> " + dt.Rows[iCnt]["CHALLAN_NUMBER"].ToString() + "      </td>");
        //                strReport.AppendLine("      <td style='text-align:Center;'> " + Convert.ToDateTime(dt.Rows[iCnt]["CHALLAN_DATE"].ToString()).ToString("dd-MMM-yyyy") + "      </td>");
        //                strReport.AppendLine("      <td> " + dt.Rows[iCnt]["CONSIGNEE_NO"].ToString() + "      </td>");
        //                strReport.AppendLine("      <td> " + dt.Rows[iCnt]["VEHICLE_NO"].ToString() + "      </td>");
        //                strReport.AppendLine("      <td> " + dt.Rows[iCnt]["Transport_Name"].ToString() + "      </td>");
        //                strReport.AppendLine("      <td> " + dt.Rows[iCnt]["CIRCLE_NAME"].ToString() + "      </td>");
        //                strReport.AppendLine("      <td> " + dt.Rows[iCnt]["DISTRICT"].ToString() + "      </td>");
        //                strReport.AppendLine("</tr>");

        //            }
        //            strTableReport.AppendLine("<table border='1'>");
        //            strTableReport.AppendLine("          " + strReport.ToString());
        //            strTableReport.AppendLine("</table>");


        //            Response.Clear();
        //            Response.Buffer = true;
        //            Response.ContentType = "application/vnd.ms-excel";
        //            String FileName = "UnBilledChallanDtl" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".xls";

        //            Response.AddHeader("Content-Disposition", "inline;filename=" + FileName);
        //            String HTMLDataToExport = strTableReport.ToString();


        //            Response.Write("<html><head><head>" +
        //            HTMLDataToExport.Replace("<BR>", "<br style='mso-data-placement:same-cell;'>")
        //                                            .Replace("<br>", "<br style='mso-data-placement:same-cell;'>")
        //                                            .Replace("<BR >", "<br style='mso-data-placement:same-cell;'>")
        //                                            .Replace("<BR />", "<br style='mso-data-placement:same-cell;'>")
        //                                            .Replace("<br />", "<br style='mso-data-placement:same-cell;'>")
        //                                            .Replace("<Br />", "<br style='mso-data-placement:same-cell;'>")
        //                                            .Replace("<Br>", "<br style='mso-data-placement:same-cell;'>")
        //                                            .Replace("<br >", "<br style='mso-data-placement:same-cell;'>") + "</html>");
        //            Response.End();



        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
        //    }
        //    return View("Index");
        //}
    }
}
