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
    public class MstChallanBookCeategoryController : Controller
    {
        //
        // GET: /MstChallanBookCeategory/
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            List<Category> listCategory = new List<Category>();
            try
            {
                DataTable dtChallanBookCeategory = objDbTrx.GetChallanBookCeategory();
                if (dtChallanBookCeategory.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dtChallanBookCeategory.Rows.Count; iCnt++)
                    {
                        Category objMstCategory = new Category();
                        objMstCategory.CategoryID = Convert.ToInt16(dtChallanBookCeategory.Rows[iCnt]["ID"].ToString());
                        objMstCategory.Category_name = dtChallanBookCeategory.Rows[iCnt]["CHALLAN_BOOK_CATEGORY"].ToString();

                        listCategory.Add(objMstCategory);

                    }
                }

            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return View(listCategory);
        }
        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(Category objCategory)
        {
            try
            {
                bool isUpdated = objDbTrx.InsertInMstCategory(objCategory);
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return RedirectToAction("Index", "MstChallanBookCeategory");
        }

        public ActionResult ExportCategoryData()
        {


            try
            {

                StringBuilder strTableReport = new StringBuilder();
                StringBuilder strReport = new StringBuilder();

                DataTable dtChallanBookCeategory = objDbTrx.GetChallanBookCeategory();
                if (dtChallanBookCeategory.Rows.Count > 0)
                {
                    strReport.AppendLine("<tr>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >Category Name</th>");

                    strReport.AppendLine("</tr>");
                    for (int iCnt = 0; iCnt < dtChallanBookCeategory.Rows.Count; iCnt++)
                    {
                        strReport.AppendLine("<tr>");
                        strReport.AppendLine("      <td> " + dtChallanBookCeategory.Rows[iCnt]["CHALLAN_BOOK_CATEGORY"].ToString() + "      </td>");

                        strReport.AppendLine("</tr>");

                    }
                    strTableReport.AppendLine("<table border='1'>");
                    strTableReport.AppendLine("          " + strReport.ToString());
                    strTableReport.AppendLine("</table>");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    String FileName = "CategoryData" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".xls";

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
