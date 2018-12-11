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
    public class MstLanguageController : Controller
    {
        //
        // GET: /MstLanguage/
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
              List<Language> listLanguage = new List<Language>();
            try
            {
                DataTable GetLanguageMasterDetails = objDbTrx.GetLanguageMasterDetails();
                if (GetLanguageMasterDetails.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < GetLanguageMasterDetails.Rows.Count; iCnt++)
                    {
                        Language objMstLanguage = new Language();
                        objMstLanguage.LanguageID = Convert.ToInt16(GetLanguageMasterDetails.Rows[iCnt]["ID"].ToString());
                        objMstLanguage.language_name = GetLanguageMasterDetails.Rows[iCnt]["LANGUAGE"].ToString();
                        
                        listLanguage.Add(objMstLanguage);

                    }
                }

            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return View(listLanguage);
        }


        public ActionResult AddLanguage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddLanguage(Language objLanguage)
        {
            try
            {
                bool isUpdated = objDbTrx.InsertInMstLanguage(objLanguage);
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return RedirectToAction("Index", "MstLanguage");
        }

         public ActionResult ExportLanguageData()
        {


            try
            {

                StringBuilder strTableReport = new StringBuilder();
                StringBuilder strReport = new StringBuilder();

                DataTable GetLanguageMasterDetails = objDbTrx.GetLanguageMasterDetails();
                if (GetLanguageMasterDetails.Rows.Count > 0)
                {
                    strReport.AppendLine("<tr>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >Language Name</th>");
                    
                    strReport.AppendLine("</tr>");
                    for (int iCnt = 0; iCnt < GetLanguageMasterDetails.Rows.Count; iCnt++)
                    {
                        strReport.AppendLine("<tr>");
                        strReport.AppendLine("      <td> " + GetLanguageMasterDetails.Rows[iCnt]["LANGUAGE"].ToString() + "      </td>");
                        
                        strReport.AppendLine("</tr>");

                    }
                    strTableReport.AppendLine("<table border='1'>");
                    strTableReport.AppendLine("          " + strReport.ToString());
                    strTableReport.AppendLine("</table>");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    String FileName = "LanguageData" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".xls";

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

