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
    public class MstDistrictController : Controller
    {
        //
        // GET: /MstDistrict/
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            List<MstDistrictDtl> lstMstDistrict = new List<MstDistrictDtl>();
            try
            {
                DataTable dtDistricrt = objDbTrx.GetDistrictDetails();
                if (dtDistricrt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dtDistricrt.Rows.Count; iCnt++)
                    {
                        MstDistrictDtl objMstDistrict = new MstDistrictDtl();                      

                        objMstDistrict.districtId = Convert.ToInt16(dtDistricrt.Rows[iCnt]["ID"].ToString());
                        objMstDistrict.districtName = dtDistricrt.Rows[iCnt]["DISTRICT"].ToString();
                        lstMstDistrict.Add(objMstDistrict);
                    }
                }

            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return View(lstMstDistrict);           
        }

        public ActionResult AddNewDistrict()
        {
            return View();           
        }
        [HttpPost]
        public ActionResult AddNewDistrict(MstDistrictDtl objDist)
        {
            return View();
        }
        public ActionResult ExportDistrictData()
        {

           
            try
            {
                
                StringBuilder strTableReport = new StringBuilder();
                StringBuilder strReport = new StringBuilder();        
               
                DataTable dt = objDbTrx.GetDistrictDetails();
                if (dt.Rows.Count > 0)
                {
                    strReport.AppendLine("<tr>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >District Name</th>");                  
                    strReport.AppendLine("</tr>");
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        strReport.AppendLine("<tr>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["DISTRICT"].ToString() + "      </td>");                       
                        strReport.AppendLine("</tr>");
                    }
                    strTableReport.AppendLine("<table border='1'>");
                    strTableReport.AppendLine("          " + strReport.ToString());
                    strTableReport.AppendLine("</table>");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    String FileName = "DistrictData" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".xls";

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
