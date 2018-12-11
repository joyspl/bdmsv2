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
    public class BinderAllotQtyController : Controller
    {
        //
        // GET: /BinderAllotQty/
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            ViewBag.Active = "BinderAllotQty";
            List<BinderAllotQuantity> listBinderAllotQuantity = new List<BinderAllotQuantity>();
            try
            {
                DataTable GetBinderAllotQtyDtl = objDbTrx.GetBinderAllotQty();
                if (GetBinderAllotQtyDtl.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < GetBinderAllotQtyDtl.Rows.Count; iCnt++)
                    {
                        BinderAllotQuantity objBinderAllotQuantity = new BinderAllotQuantity();
                        objBinderAllotQuantity.ID = Convert.ToInt16(GetBinderAllotQtyDtl.Rows[iCnt]["ID"].ToString());
                        objBinderAllotQuantity.BinderId = Convert.ToInt16(GetBinderAllotQtyDtl.Rows[iCnt]["BINDER_ID"].ToString());
                        objBinderAllotQuantity.ChallanCategoryId = Convert.ToInt16(GetBinderAllotQtyDtl.Rows[iCnt]["CHALLAN_CATEGORY_ID"].ToString());
                        objBinderAllotQuantity.BookId = GetBinderAllotQtyDtl.Rows[iCnt]["BOOK_ID"].ToString();
                        objBinderAllotQuantity.TotQty = Convert.ToInt16(GetBinderAllotQtyDtl.Rows[iCnt]["TOT_QTY"].ToString());
                        objBinderAllotQuantity.Lot = Convert.ToInt16(GetBinderAllotQtyDtl.Rows[iCnt]["LOT"].ToString());
                        //objBinderAllotQuantity.LotFrom = Convert.ToInt16(GetBinderAllotQtyDtl.Rows[iCnt]["LOT_FROM"].ToString());
                        //objBinderAllotQuantity.LotTo = Convert.ToInt16(GetBinderAllotQtyDtl.Rows[iCnt]["LOT_TO"].ToString());
                        objBinderAllotQuantity.BookCategoryName = GetBinderAllotQtyDtl.Rows[iCnt]["CHALLAN_BOOK_CATEGORY"].ToString();
                        objBinderAllotQuantity.BookName = GetBinderAllotQtyDtl.Rows[iCnt]["BOOK_NAME"].ToString();
                        objBinderAllotQuantity.LanguageName = GetBinderAllotQtyDtl.Rows[iCnt]["LANGUAGE"].ToString();
                        objBinderAllotQuantity.BinderName = GetBinderAllotQtyDtl.Rows[iCnt]["BinderName"].ToString();
                        objBinderAllotQuantity.AllotmentDate = Convert.ToDateTime(GetBinderAllotQtyDtl.Rows[iCnt]["ALLOTMENT_DATE"].ToString()).ToString("dd-MMM-yyyy").ToUpper();
                        listBinderAllotQuantity.Add(objBinderAllotQuantity);

                    }
                }

            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return View(listBinderAllotQuantity);
        }
        public ActionResult AddBinderAllotQuantity()
        {
            return View();
        }

      


        public ActionResult ExportBinderAllotQuantityData()
        {


            try
            {

                StringBuilder strTableReport = new StringBuilder();
                StringBuilder strReport = new StringBuilder();

                DataTable dt = objDbTrx.GetBinderAllotQty();
                if (dt.Rows.Count > 0)
                {
                    strReport.AppendLine("<tr>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >Allotment Date Name</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >Binder Name</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >Book Category Name</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >Language Name</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >Book Name</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >Total Quantity</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >Lot</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >Lot From</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >Lot To</th>");

                    strReport.AppendLine("</tr>");
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        strReport.AppendLine("<tr>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["ALLOTMENT_DATE"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["BinderName"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["CHALLAN_BOOK_CATEGORY"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["LANGUAGE"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["BOOK_NAME"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["TOT_QTY"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["LOT"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["LOT_FROM"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["LOT_TO"].ToString() + "      </td>");
                        strReport.AppendLine("</tr>");

                    }
                    strTableReport.AppendLine("<table border='1'>");
                    strTableReport.AppendLine("          " + strReport.ToString());
                    strTableReport.AppendLine("</table>");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    String FileName = "BinderAllotQuantityData" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".xls";

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
        [HttpPost]
        public JsonResult GetBinderMaster()
        {
            try
            {
                //List<challan_remarks> ObjLstchallan = new List<challan_remarks>();
                //DataTable dt = objDbTrx.GetBinderMaster();
                //if (dt.Rows.Count > 0)
                //{
                //    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                //    {
                //        challan_remarks objchallan = new challan_remarks();
                //        objchallan.RemId = Convert.ToInt16(dt.Rows[iCnt]["Id"].ToString());
                //        objchallan.Remarks = Convert.ToString(dt.Rows[iCnt]["Remarks"].ToString());
                //        ObjLstchallan.Add(objchallan);
                //    }
                //    ViewBag.ObjchallanList = new SelectList(ObjLstchallan, "RemId", "Remarks");
                //}
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(ViewBag.ObjchallanList);
        }



        [HttpPost]
        public JsonResult GetBookMasterByCategoryAndLanguage(string InCategoryId, string InLanguageId)
        {
            try
            {
                //List<Bookmaster> ObjLstBook = new List<Bookmaster>();
                //DataTable dt = objDbTrx.GetBookMasterDtlByChallanCatAndLangId(Convert.ToInt16(InCategoryId),Convert.ToInt16(InLanguageId));
                //if (dt.Rows.Count > 0)
                //{
                //    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                //    {
                //        Bookmaster objbm = new Bookmaster();
                //        objbm.bookcode = Convert.ToString(dt.Rows[iCnt]["BOOK_CODE"].ToString());
                //        objbm.bookname = Convert.ToString(dt.Rows[iCnt]["BOOK_NAME"].ToString());
                //        ObjLstBook.Add(objbm);
                //    }
                //    ViewBag.ObjBookList = new SelectList(ObjLstBook, "bookcode", "bookname");
                //}
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(ViewBag.ObjBookList);
        }



    }
}
