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
    public class InvoiceController : Controller
    {
        //
        // GET: /Invoice/
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            Invoice objInvoice = new Invoice();
            objInvoice.InvoiceId = 0;
            objInvoice.InvoiceNo = "INV" + (DateTime.Now.Month >= 4 ? DateTime.Now.Year.ToString().Substring(2) + "-" + (Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2)) + 1) + "-XXXXXXX" : (Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2)) - 1) + "-" + DateTime.Now.Year.ToString().Substring(2) + "-XXXXXXX").ToString();
            objInvoice.InvoiceDate = DateTime.Now.ToString("dd-MMM-yyyy");
            try
            {
                if (Session["InvoiceId"].ToString() != "")
                {
                    objInvoice.InvoiceId = Convert.ToInt64(Session["InvoiceId"].ToString());
                    Session["InvoiceId"] = "";
                }
            }catch{
                Session["InvoiceId"] = "";
            }
            
            return View(objInvoice);
        }
        [HttpPost]
        public JsonResult GetChallanCategoryDetails()
        {
            try
            {
                List<Category> lst_Category = new List<Category>();
                DataTable dtMast = objDbTrx.GetChallanBookCeategory();
                if (dtMast.Rows.Count > 0)
                {

                    for (int iCnt = 0; iCnt < dtMast.Rows.Count; iCnt++)
                    {
                        Category ct = new Category();
                        ct.Category_name = Convert.ToString(dtMast.Rows[iCnt]["CHALLAN_BOOK_CATEGORY"].ToString());
                        ct.CategoryID = Convert.ToInt32(dtMast.Rows[iCnt]["ID"].ToString());
                        lst_Category.Add(ct);
                    }
                    dtMast.Dispose();
                }
                ViewBag.ObjCategoryList = new SelectList(lst_Category, "CategoryID", "Category_name");

            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(ViewBag.ObjCategoryList);
        }

        [HttpPost]
        public JsonResult GetInvoiceDetailForEdit(string InvoiceId)
        {
            Invoice objInvoice = new Invoice();
            try
            {
                objInvoice.InvoiceId = Convert.ToInt64(InvoiceId);
                DataTable dtInvoiceChallanDtl = objDbTrx.GetInvoiceDetailById(objInvoice.InvoiceId);
                if (dtInvoiceChallanDtl.Rows.Count > 0)
                {
                    objInvoice.InvoiceNo = dtInvoiceChallanDtl.Rows[0]["InvoiceNo"].ToString();
                    objInvoice.ManualInvoiceNo = dtInvoiceChallanDtl.Rows[0]["ManualInvoiceNo"].ToString();
                    objInvoice.InvoiceDate = Convert.ToDateTime(dtInvoiceChallanDtl.Rows[0]["InvoiceDate"]).ToString("dd-MMM-yyyy");
                    objInvoice.CategoryId = Convert.ToInt16(dtInvoiceChallanDtl.Rows[0]["CHALLAN_BOOK_CATEGORY_ID"].ToString());
                    objInvoice.SaveStatus = dtInvoiceChallanDtl.Rows[0]["Save_Status"].ToString();
                }

            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(objInvoice);
        }

        [HttpPost]
        public ActionResult InvoiceUpdate(string ChallanNo, string InvoiceNo,string ManualInvoiceNo, string InvoiceDate,string CategoryId, string InvoiceId)
        {
            string GenInvoiceNo="",retInvoiceId="0";
            Invoice objInvoice = new Invoice();
            try
            {
                objInvoice.UpdateCode = "0";
                objInvoice.InvoiceId = Convert.ToInt64(InvoiceId);
                if (ChallanNo != "")
                {
                    DataSet dtChallan = objDbTrx.GetChallanCodeOfaCategoryDtl(ChallanNo, Convert.ToInt64(CategoryId));
                    if (dtChallan.Tables[0].Rows.Count > 0 && dtChallan.Tables[1].Rows.Count == 0)
                    {
                        objInvoice.ChallanId = Convert.ToInt64(dtChallan.Tables[0].Rows[0]["ID"].ToString());
                        objInvoice.ChallanNo = ChallanNo;
                        objInvoice.InvoiceNo = InvoiceNo;
                        objInvoice.ManualInvoiceNo = ManualInvoiceNo;
                        objInvoice.InvoiceDate = Convert.ToDateTime(InvoiceDate).ToString("dd-MMM-yyyy");
                        objInvoice.CategoryId = Convert.ToInt16(CategoryId);
                        objInvoice.UserId = Convert.ToString(Session["sp_user_name"]);
                        if (InvoiceId == "0")
                        {
                            
                            objDbTrx.InsertInInvoice(objInvoice, out GenInvoiceNo, out retInvoiceId);
                            objInvoice.InvoiceNo = GenInvoiceNo;
                            objInvoice.InvoiceId = Convert.ToInt64(retInvoiceId);
                            objInvoice.UpdateCode = "NEW";
                            objInvoice.UpdateMsg = "Invoice No " + GenInvoiceNo + " Created.";
                        }
                        else
                        {
                            objDbTrx.AddChallanInInvoice(objInvoice);
                            objInvoice.UpdateCode = "EDIT";
                            objInvoice.UpdateMsg = "Challan No " + ChallanNo + " added under invoice " + InvoiceNo;
                        }
                    }
                    else
                    {
                        objInvoice.UpdateCode = "ERROR";
                        if (dtChallan.Tables[1].Rows.Count > 0)
                        {
                            objInvoice.UpdateMsg = "Challan No " + ChallanNo + " already exist Under Invoice no. " + dtChallan.Tables[1].Rows[0]["InvoiceNo"].ToString();
                        }
                        else
                        {
                            objInvoice.UpdateMsg = "Challan No " + ChallanNo + " is not valid.";
                        }
                    }
                }
                if (objInvoice.InvoiceId > 0)
                {
                    List<InvoiceChallanDtl> lst_InvoiceChallanDtl = new List<InvoiceChallanDtl>();
                    DataTable dtInvoiceChallanDtl = objDbTrx.GetInvoiceChallanDetails(objInvoice.InvoiceId);
                    if (dtInvoiceChallanDtl.Rows.Count > 0)
                    {
                        objInvoice.SaveStatus = dtInvoiceChallanDtl.Rows[0]["Save_Status"].ToString();
                        for (int iCnt = 0; iCnt < dtInvoiceChallanDtl.Rows.Count; iCnt++)
                        {
                            InvoiceChallanDtl objInvoiceChallanDtl = new InvoiceChallanDtl();                            
                            objInvoiceChallanDtl.InvoiceDtlId = dtInvoiceChallanDtl.Rows[iCnt]["InvoiceDtlId"].ToString();
                            objInvoiceChallanDtl.ChallanId = dtInvoiceChallanDtl.Rows[iCnt]["ChallanId"].ToString();
                            objInvoiceChallanDtl.ChallanNo = dtInvoiceChallanDtl.Rows[iCnt]["CHALLAN_NUMBER"].ToString();
                            objInvoiceChallanDtl.ChallanDate = Convert.ToDateTime(dtInvoiceChallanDtl.Rows[iCnt]["CHALLAN_DATE"]).ToString("dd-MMM-yyyy");
                            objInvoiceChallanDtl.DistrictName = dtInvoiceChallanDtl.Rows[iCnt]["DISTRICT"].ToString();
                            objInvoiceChallanDtl.CircleName = dtInvoiceChallanDtl.Rows[iCnt]["CIRCLE_NAME"].ToString();
                            objInvoiceChallanDtl.CategoryName = dtInvoiceChallanDtl.Rows[iCnt]["CHALLAN_BOOK_CATEGORY"].ToString();
                            objInvoiceChallanDtl.Language = dtInvoiceChallanDtl.Rows[iCnt]["LANGUAGE"].ToString();
                            objInvoiceChallanDtl.Transporter = dtInvoiceChallanDtl.Rows[iCnt]["Transport_Name"].ToString();
                            objInvoiceChallanDtl.CONSIGNEE_NO = dtInvoiceChallanDtl.Rows[iCnt]["CONSIGNEE_NO"].ToString();
                            objInvoiceChallanDtl.VEHICLE_NO = dtInvoiceChallanDtl.Rows[iCnt]["VEHICLE_NO"].ToString();
                            lst_InvoiceChallanDtl.Add(objInvoiceChallanDtl);
                        }
                    }
                    objInvoice.InvoiceChallanDtlCollection = lst_InvoiceChallanDtl;
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(objInvoice);
        }

        public ActionResult EditInvoice(string Id, string Command)
        {
            Session["InvoiceId"] = Id;            
            return RedirectToAction("Index", "Invoice");

        }

         [HttpPost]
        public ActionResult DeleteChallanIdInInvoice(string InvoiceId, string Command)
        {
            try
            {
                objDbTrx.DeleteChallanFromInvoice(InvoiceId);
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json("saved");
        }

         [HttpPost]
         public ActionResult UpdatedInInvoice(string ManualInvoiceNo, string InvoiceDate, string SaveStatus, string InvoiceId)
         {
             Invoice objInvoice = new Invoice();
             try
             {
                 objInvoice.InvoiceId =Convert.ToInt64(InvoiceId);
                 objInvoice.SaveStatus = SaveStatus;
                 objInvoice.ManualInvoiceNo = ManualInvoiceNo;
                 objInvoice.InvoiceDate = Convert.ToDateTime(InvoiceDate).ToString("dd-MMM-yyyy");
                 objInvoice.UserId = Convert.ToString(Session["sp_user_name"]);
                 objDbTrx.UpdateInvoiceDtl(objInvoice);
             }
             catch (Exception ex)
             {
                 objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
             }
             return Json("saved");
         }

         public ActionResult PrintInvoiceAnnexureI(string Id)
         {
             double Rate = 0, CGSTRate = 0, SGSTRate = 0, IGSTRate = 0, Qty = 0, TaxableAmnt = 0, CGSTAmt = 0, SGSTAmt = 0, IGSTAmt = 0, TotalAmt = 0, TaxableTotAmnt = 0, CGSTTotAmt=0,SGSTTotAmt = 0, IGSTTotAmt = 0, SumTotAmt = 0,TotalTaxAmt=0,RoundingOff=0,TotInvoiceAmt=0;
             InvoiceAnnexureI objInvoiceAnnexureI = new InvoiceAnnexureI();
             DataSet dtMast = objDbTrx.GetInvoiceAnnexureI(Convert.ToInt64(Id));
             if (dtMast.Tables[0].Rows.Count > 0)
             {
                 objInvoiceAnnexureI.InvoiceId = Convert.ToInt64(Id);
                 objInvoiceAnnexureI.DisplayInvoiceNo = (dtMast.Tables[0].Rows[0]["ManualInvoiceNo"].ToString() != "" ? dtMast.Tables[0].Rows[0]["ManualInvoiceNo"].ToString() : dtMast.Tables[0].Rows[0]["InvoiceNo"].ToString());
                 objInvoiceAnnexureI.InvoiceNo = dtMast.Tables[0].Rows[0]["InvoiceNo"].ToString();
                 objInvoiceAnnexureI.ManualInvoiceNo = dtMast.Tables[0].Rows[0]["ManualInvoiceNo"].ToString();
                 objInvoiceAnnexureI.InvoiceCategory = dtMast.Tables[0].Rows[0]["CHALLAN_BOOK_CATEGORY"].ToString();
                 

                 objInvoiceAnnexureI.InvoiceDate =Convert.ToDateTime(dtMast.Tables[0].Rows[0]["InvoiceDate"]).ToString("dd-MMM-yyyy");
                 List<InvoiceAnnexureIDtl> lstInvoiceAnnexureIDtl = new List<InvoiceAnnexureIDtl>();
                 TaxableTotAmnt = 0;
                 CGSTTotAmt=0;
                 SGSTTotAmt = 0;
                 IGSTTotAmt = 0;
                 SumTotAmt = 0;
                 TotalTaxAmt=0;
                 RoundingOff=0;
                 TotInvoiceAmt = 0;
                 objInvoiceAnnexureI.PgCount = dtMast.Tables[1].Rows.Count;
                 if (objInvoiceAnnexureI.PgCount > 5)
                 {

                     if ((objInvoiceAnnexureI.PgCount) % 5 == 0)
                     {
                         objInvoiceAnnexureI.PgCount = ((objInvoiceAnnexureI.PgCount) / 5);
                     }
                     else {
                         objInvoiceAnnexureI.PgCount = ((objInvoiceAnnexureI.PgCount) / 5)+1;                         
                     }        
                 }
                 else
                 {
                     objInvoiceAnnexureI.PgCount = 1;
                 }               

                 for (int iCnt = 0; iCnt < dtMast.Tables[1].Rows.Count; iCnt++)
                 {
                     Rate=0; 
                     CGSTRate=0;
                     SGSTRate=0;
                     IGSTRate = 0;
                     Qty = 0;
                     CGSTAmt = 0;
                     SGSTAmt = 0;
                     IGSTAmt = 0;
                     TotalAmt = 0;
                     InvoiceAnnexureIDtl objInvoiceAnnexureIDtl = new InvoiceAnnexureIDtl();
                     objInvoiceAnnexureIDtl.SlNo = (iCnt + 1).ToString();
                     objInvoiceAnnexureIDtl.BookCode = dtMast.Tables[1].Rows[iCnt]["BOOK_CODE"].ToString();
                     objInvoiceAnnexureIDtl.BookName = dtMast.Tables[1].Rows[iCnt]["BOOK_NAME"].ToString();
                     objInvoiceAnnexureIDtl.Language = dtMast.Tables[1].Rows[iCnt]["LANGUAGE"].ToString() + " Medium";
                     objInvoiceAnnexureIDtl.BookNameDesc = dtMast.Tables[1].Rows[iCnt]["BOOK_NAME_DESC"].ToString();
                     objInvoiceAnnexureIDtl.HsnSac = dtMast.Tables[1].Rows[iCnt]["HSN_SAC"].ToString();
                     objInvoiceAnnexureIDtl.UQC = dtMast.Tables[1].Rows[iCnt]["UQC"].ToString();
                                        
                     Qty = Convert.ToDouble(dtMast.Tables[1].Rows[iCnt]["QtyShippedQty"].ToString());
                     Rate = Convert.ToDouble(dtMast.Tables[1].Rows[iCnt]["RATE"].ToString());
                     CGSTRate = Convert.ToDouble(dtMast.Tables[1].Rows[iCnt]["CGST_RATE"].ToString());
                     SGSTRate = Convert.ToDouble(dtMast.Tables[1].Rows[iCnt]["SGST_RATE"].ToString());
                     IGSTRate = Convert.ToDouble(dtMast.Tables[1].Rows[iCnt]["IGST_RATE"].ToString());
                     TaxableAmnt = Qty * Rate;
                     CGSTAmt = (TaxableAmnt * CGSTRate)/100;
                     SGSTAmt = (TaxableAmnt * SGSTRate) / 100;
                     IGSTAmt = (TaxableAmnt * IGSTRate) / 100;
                     TotalAmt = TaxableAmnt + CGSTAmt + SGSTAmt + IGSTAmt;

                     TaxableTotAmnt = TaxableTotAmnt + TaxableAmnt;
                     CGSTTotAmt = CGSTTotAmt + SGSTAmt;
                     SGSTTotAmt = SGSTTotAmt + SGSTAmt;
                     IGSTTotAmt =IGSTTotAmt+ IGSTAmt;
                     SumTotAmt =SumTotAmt+ TotalAmt;

                     objInvoiceAnnexureIDtl.Qty = Qty;
                     objInvoiceAnnexureIDtl.Rate = string.Format("{0:#,##0.##}", Rate);
                     objInvoiceAnnexureIDtl.CGSTRate = string.Format("{0:#,##0.##}", CGSTRate);
                     objInvoiceAnnexureIDtl.CGSTRate = string.Format("{0:#,##0.##}", CGSTRate);
                     objInvoiceAnnexureIDtl.SGSTRate = string.Format("{0:#,##0.##}", SGSTRate);
                     objInvoiceAnnexureIDtl.IGSTRate = string.Format("{0:#,##0.##}", IGSTRate);
                     objInvoiceAnnexureIDtl.TaxableAmnt = string.Format("{0:n}", TaxableAmnt);
                     objInvoiceAnnexureIDtl.CGSTAmt = string.Format("{0:n}", CGSTAmt);
                     objInvoiceAnnexureIDtl.SGSTAmt = string.Format("{0:n}", SGSTAmt);
                     objInvoiceAnnexureIDtl.IGSTAmt = string.Format("{0:n}", IGSTAmt);
                     objInvoiceAnnexureIDtl.TotalAmt = string.Format("{0:n}", TotalAmt);

                     lstInvoiceAnnexureIDtl.Add(objInvoiceAnnexureIDtl);
                 }
                 

                 objInvoiceAnnexureI.TaxableTotAmnt = string.Format("{0:n}", TaxableTotAmnt);
                 objInvoiceAnnexureI.CGSTTotAmt = string.Format("{0:n}", CGSTTotAmt);
                 objInvoiceAnnexureI.SGSTTotAmt = string.Format("{0:n}", SGSTTotAmt);
                 objInvoiceAnnexureI.IGSTTotAmt = string.Format("{0:n}", IGSTTotAmt);
                 objInvoiceAnnexureI.SumTotAmt = string.Format("{0:n}", Math.Round(SumTotAmt));

                 TotalTaxAmt = CGSTTotAmt + SGSTTotAmt + IGSTTotAmt;
                 RoundingOff = SumTotAmt-Math.Round(SumTotAmt);
                 objInvoiceAnnexureI.TotalTaxAmt = string.Format("{0:n}", TotalTaxAmt);
                 objInvoiceAnnexureI.RoundingOff = string.Format("{0:n}", RoundingOff);
                 objInvoiceAnnexureI.AmountinWord = NumberToText(Convert.ToInt64(SumTotAmt)) +" Only.";
                 objInvoiceAnnexureI.InvoiceAnnexureIDtlCollection = lstInvoiceAnnexureIDtl;
                 objInvoiceAnnexureI.CustomerOrderNo = dtMast.Tables[2].Rows[0]["CUST_ORDER"].ToString();
                 dtMast.Dispose();
             }
             return View(objInvoiceAnnexureI);
         }

         public static string NumberToText(Int64 number)
         {
             if (number == 0) return "Zero";

             //if (number == -2147483648) return "Minus Two Hundred and Fourteen Crore Seventy Four Lakh Eighty Three Thousand Six Hundred and Forty Eight";

             Int64[] num = new Int64[4];
             Int64 first = 0;
             Int64 u, h, t;
             System.Text.StringBuilder sb = new System.Text.StringBuilder();

             if (number < 0)
             {
                 sb.Append("Minus ");
                 number = -number;
             }

             string[] words0 = { "", "One ", "Two ", "Three ", "Four ", "Five ", "Six ", "Seven ", "Eight ", "Nine " };

             string[] words1 = { "Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", "Fifteen ", "Sixteen ", "Seventeen ", "Eighteen ", "Nineteen " };

             string[] words2 = { "Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };

             string[] words3 = { "Thousand ", "Lakh ", "Crore " };

             num[0] = number % 1000; // units
             num[1] = number / 1000;
             num[2] = number / 100000;
             num[1] = num[1] - 100 * num[2]; // thousands
             num[3] = number / 10000000; // crores
             num[2] = num[2] - 100 * num[3]; // lakhs

             for (int i = 3; i > 0; i--)
             {
                 if (num[i] != 0)
                 {
                     first = i;
                     break;
                 }
             }

             for (Int64 i = first; i >= 0; i--)
             {
                 if (num[i] == 0) continue;
                 u = num[i] % 10; // ones
                 t = num[i] / 10;
                 h = num[i] / 100; // hundreds
                 t = t - 10 * h; // tens

                 if (h > 0) sb.Append(words0[h] + "Hundred ");
                 if (u > 0 || t > 0)
                 {
                     if (h > 0 || i == 0) sb.Append("and ");

                     if (t == 0)
                         sb.Append(words0[u]);
                     else if (t == 1)
                         sb.Append(words1[u]);
                     else
                         sb.Append(words2[t - 2] + words0[u]);
                 }
                 if (i != 0) sb.Append(words3[i - 1]);
             }
             return sb.ToString().TrimEnd();
         }
         public ActionResult ExportChallanData(int InvoiceId)
         {
             int CircleCnt = 0, BokCnt = 0;
             Int64 RemainingCnt = 0;
            
             StringBuilder strTableReport = new StringBuilder();
             StringBuilder strReport = new StringBuilder();
             StringBuilder strReport1 = new StringBuilder();
             StringBuilder strReport2 = new StringBuilder();
             StringBuilder strReportSchoolNameHead = new StringBuilder();
             StringBuilder strTotReport = new StringBuilder();
           
             try
             {
                     DataSet ds = objDbTrx.GetInvoiceAnnexureII(Convert.ToInt64(InvoiceId));
                     if (ds.Tables[0].Rows.Count > 0)
                     {
                         strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;' >Challn No  </td>");
                         strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;' >Name of District </td>");
                         strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;' >Circle Name  </td>");
                         strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;' >Book Name  </td>");
                         strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;' >Language</td>");
                         strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;' >Quantity  </td>");
                         strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;' >Rate Per Book  </td>");
                         strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;' >Net total Amount  </td>");
                         strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;' >SGST % on Net Value</td>");
                         strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;' >CGST % on Net Value</td>");
                         strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;' >Total Net Value including GST</td>");
                         Int64 QtyTotal = 0;
                         double NetTotalAmt=0,SGST=0,CGST=0,TotVal=0;
                         double SubQtyTotal=0, SubNetTotalAmt = 0, SumSGST = 0, SumCGST = 0, SumTotVal = 0;
                         for (int bkCnt = 0; bkCnt < ds.Tables[0].Rows.Count; bkCnt++)
                         {
                             strReport2.AppendLine("      <tr><td colspan='11' style='text-align:Left;vertical-align: middle;'> " + ds.Tables[0].Rows[bkCnt]["BOOK_NAME"].ToString() + " - " + ds.Tables[0].Rows[bkCnt]["BOOK_CODE"].ToString() + "     </td></tr>");
                             QtyTotal = 0;
                             for (int ChallanCnt = 0; ChallanCnt < ds.Tables[1].Rows.Count; ChallanCnt++)
                             {
                                 if (ds.Tables[0].Rows[bkCnt]["BOOK_CODE"].ToString() == ds.Tables[1].Rows[ChallanCnt]["BOOK_CODE"].ToString())
                                 {
                                     NetTotalAmt = 0; 
                                     SGST = 0; 
                                     CGST = 0;
                                     TotVal = 0;

                                     strReport2.AppendLine("    <tr>");
                                     strReport2.AppendLine("      <td  style='text-align:Left;vertical-align: middle;'> " + ds.Tables[1].Rows[ChallanCnt]["CHALLAN_NUMBER"].ToString() + " </td>");
                                     strReport2.AppendLine("      <td  style='text-align:Left;vertical-align: middle;'> " + ds.Tables[1].Rows[ChallanCnt]["DISTRICT"].ToString() + " </td>");
                                     strReport2.AppendLine("      <td  style='text-align:Left;vertical-align: middle;'> " + ds.Tables[1].Rows[ChallanCnt]["CIRCLE_NAME"].ToString() + " </td>");
                                     strReport2.AppendLine("      <td  style='text-align:Left;vertical-align: middle;'> " + ds.Tables[1].Rows[ChallanCnt]["BOOK_NAME"].ToString() + " </td>");
                                     strReport2.AppendLine("      <td  style='text-align:Left;vertical-align: middle;'> " + ds.Tables[1].Rows[ChallanCnt]["LANGUAGE"].ToString() + " </td>");
                                     strReport2.AppendLine("      <td  style='text-align:Left;vertical-align: middle;'> " + ds.Tables[1].Rows[ChallanCnt]["QtyShippedQty"].ToString() + " </td>");
                                     strReport2.AppendLine("      <td  style='text-align:Left;vertical-align: middle;'> " + ds.Tables[1].Rows[ChallanCnt]["RATE"].ToString() + " </td>");
                                    

                                     NetTotalAmt = (Convert.ToDouble(ds.Tables[1].Rows[ChallanCnt]["QtyShippedQty"]) * Convert.ToDouble(ds.Tables[1].Rows[ChallanCnt]["RATE"]));
                                     SubQtyTotal = SubQtyTotal + Convert.ToDouble(ds.Tables[1].Rows[ChallanCnt]["QtyShippedQty"]);
                                     SubNetTotalAmt = SubNetTotalAmt + NetTotalAmt;

                                     SGST=(NetTotalAmt* Convert.ToDouble(ds.Tables[1].Rows[ChallanCnt]["SGST_RATE"]) )/100;
                                     SumSGST = SumSGST + SGST;

                                     CGST = (NetTotalAmt * Convert.ToDouble(ds.Tables[1].Rows[ChallanCnt]["CGST_RATE"])) / 100;
                                     SumCGST = SumCGST + CGST;

                                     TotVal = NetTotalAmt + SGST + CGST;
                                     SumTotVal = SumTotVal + TotVal;
                                     //,CGST=0,TotVal=0

                                     strReport2.AppendLine("      <td  style='text-align:Left;vertical-align: middle;'> " + string.Format("{0:n}", NetTotalAmt) + " </td>");
                                     strReport2.AppendLine("      <td  style='text-align:Left;vertical-align: middle;'> " + string.Format("{0:n}", SGST) + " </td>");
                                     strReport2.AppendLine("      <td  style='text-align:Left;vertical-align: middle;'> " + string.Format("{0:n}", CGST) + " </td>");
                                     strReport2.AppendLine("      <td  style='text-align:Left;vertical-align: middle;'> " + string.Format("{0:n}", TotVal) + " </td>");
                                     strReport2.AppendLine("    </tr>");
                                     QtyTotal = QtyTotal + (Convert.ToInt64(ds.Tables[1].Rows[ChallanCnt]["QtyShippedQty"]));
                                 }
                             }
                             strReport2.AppendLine("      <tr><td colspan='5'>&nbsp;</td> <td style='text-align:Left;vertical-align: middle;'> <b>" + QtyTotal.ToString() + "</b>  </td><td colspan='5'>&nbsp;</td></tr>");
                         }

                         strReport2.AppendLine("      <tr><td colspan='5'></td>");
                         strReport2.AppendLine("            <td style='text-align:Left;vertical-align: middle;'><b>" + string.Format("{0:n}", SubQtyTotal) + "</b></td>");
                         strReport2.AppendLine("            <td >&nbsp;</td>");
                         strReport2.AppendLine("            <td style='text-align:Left;vertical-align: middle;'><b>" + string.Format("{0:n}", SubNetTotalAmt) + "</b></td>");
                         strReport2.AppendLine("            <td style='text-align:Left;vertical-align: middle;'><b>" + string.Format("{0:n}", SumSGST) + "</b></td>");
                         strReport2.AppendLine("            <td style='text-align:Left;vertical-align: middle;'><b>" + string.Format("{0:n}", SumCGST) + "</b></td>");
                         strReport2.AppendLine("            <td style='text-align:Left;vertical-align: middle;'><b>" + string.Format("{0:n}", SumTotVal) + "</b></td>");
                         strReport2.AppendLine("      </tr>");
                         strTotReport.AppendLine("<tr>" + strReport.ToString() + "</tr>");                        
                         strTotReport.AppendLine( strReport2.ToString());

                         ds.Dispose();

                         strTableReport.AppendLine("<table border='1' >");
                         strTableReport.AppendLine("     <tr><td colspan='11'> " + "Challan Statement against Invoice No <B>" + ds.Tables[1].Rows[0]["InvoiceNo"].ToString() + "</b></td></tr>");
                         strTableReport.AppendLine("          " + strTotReport.ToString());
                         strTableReport.AppendLine("</table>");

                         Response.Clear();
                         Response.Buffer = true;
                         Response.ContentType = "application/vnd.ms-excel";
                         String FileName = "AnnexureII" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".xls";
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
