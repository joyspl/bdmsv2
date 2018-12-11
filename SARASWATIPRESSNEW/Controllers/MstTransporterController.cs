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
    public class MstTransporterController : Controller
    {
        //
        // GET: /MstTransporter/
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            List<MstTransporter> listTrasporter = new List<MstTransporter>();
            try
            {
                DataTable GetTransportDtl = objDbTrx.GetTransportDtl();
                if (GetTransportDtl.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < GetTransportDtl.Rows.Count; iCnt++)
                    {
                        MstTransporter objMsttransporter = new MstTransporter();
                        objMsttransporter.TransporterID = Convert.ToInt16(GetTransportDtl.Rows[iCnt]["ID"].ToString());
                        objMsttransporter.Transporter_name = GetTransportDtl.Rows[iCnt]["Transport_Name"].ToString();
                        objMsttransporter.Transporter_address = GetTransportDtl.Rows[iCnt]["Transport_address"].ToString();
                        objMsttransporter.Transporter_phone_no = GetTransportDtl.Rows[iCnt]["Transport_Phone_no"].ToString();
                        listTrasporter.Add(objMsttransporter);

                    }
                }

            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return View(listTrasporter);
        }


        public ActionResult AddTransport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddTransport(MstTransporter objTransport)
        {
            try
            {
                bool isUpdated = objDbTrx.InsertInMstTransport(objTransport);               
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return RedirectToAction("Index", "MstTransporter");
        }


        public ActionResult ExportTransporterData()
        {


            try
            {

                StringBuilder strTableReport = new StringBuilder();
                StringBuilder strReport = new StringBuilder();

                DataTable dt = objDbTrx.GetTransportDtl();
                if (dt.Rows.Count > 0)
                {
                    strReport.AppendLine("<tr>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >Transport Name</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >Transport Address</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >Transport Phone No</th>");
                    strReport.AppendLine("</tr>");
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        strReport.AppendLine("<tr>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["Transport_Name"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["Transport_address"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["Transport_Phone_no"].ToString() + "      </td>");
                        strReport.AppendLine("</tr>");

                    }
                    strTableReport.AppendLine("<table border='1'>");
                    strTableReport.AppendLine("          " + strReport.ToString());
                    strTableReport.AppendLine("</table>");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    String FileName = "TransporterData" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".xls";

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
