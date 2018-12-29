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
    [SessionAuthorize]
    public class TrxBinderAllotmentQuantityViewController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            ViewBag.Active = "TrxBinderAllotmentQuantityView";
            DateTime dtNow = DateTime.Now;
            BinderAllotQuantity objBinderAllotQuantity = new BinderAllotQuantity();
            objBinderAllotQuantity.StartDate = dtNow.ToString("dd-MMM-yyyy");
            objBinderAllotQuantity.EndDate = dtNow.ToString("dd-MMM-yyyy");

            #region [Pagination Initialization]
            try
            {
                Int16 AccadYear = Convert.ToInt16(GlobalSettings.oUserData.AcademicYearId);
                string allotmentcode = "";
                DataTable GetBinderAllotQtyDtl = objDbTrx.GetBinderAllotmentQtyViewNew(dtNow.AddDays(-(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["BinderStartDayInitial"]))), dtNow, AccadYear, default(int), default(int), allotmentcode);
                if (GetBinderAllotQtyDtl != null && GetBinderAllotQtyDtl.Rows.Count > default(int))
                {
                    ViewBag.TotalRecords = Convert.ToInt32(GetBinderAllotQtyDtl.Rows[0]["TotalRecords"].ToString());
                    ViewBag.pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["RecordsPerPage"]);
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            #endregion [Pagination Initialization]

            return View(objBinderAllotQuantity);
        }

        [HttpGet]
        public JsonResult GetTotalRecord(string startDate, string endDate, string allotmentcode)
        {
            int totalRecords = default(int);
            try
            {
                

                Int16 AccadYear = Convert.ToInt16(GlobalSettings.oUserData.AcademicYearId);
                DataTable dtCount = objDbTrx.GetBinderAllotmentQtyViewNew(Convert.ToDateTime(startDate + " 00:00:00.000"), Convert.ToDateTime(endDate + " 23:59:59.999"), AccadYear, default(int), default(int), allotmentcode);
                if (dtCount != null && dtCount.Rows.Count > default(int))
                    totalRecords = Convert.ToInt32(dtCount.Rows[0]["TotalRecords"].ToString());
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(new { TotalRecords = totalRecords }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetTrxBinderAllotmentQuantityData(string startDate, string endDate, int isonlyjson = 0, int pageNo = 1, string allotmentcode = "")
        {
            List<BinderAllotQuantity> listBinderAllotQuantity = new List<BinderAllotQuantity>();
            int totalRecords = default(int);
            try
            {
                Int16 AccadYear = Convert.ToInt16(GlobalSettings.oUserData.AcademicYearId);
                DataTable dtCount = objDbTrx.GetBinderAllotmentQtyViewNew(Convert.ToDateTime(startDate + " 00:00:00.000"), Convert.ToDateTime(endDate + " 23:59:59.999"), AccadYear, default(int), default(int), allotmentcode);
                if (dtCount != null && dtCount.Rows.Count > default(int))
                    totalRecords = Convert.ToInt32(dtCount.Rows[0]["TotalRecords"].ToString());

                var page = Utility.PageResults(totalRecords, -1, Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["RecordsPerPage"]), pageNo);
                //DataTable GetBinderAllotQtyDtl = objDbTrx.GetBinderAllotmentQtyView(Convert.ToDateTime(startDate + " 00:00:00.000"), Convert.ToDateTime(endDate + " 23:59:59.999"), AccadYear);
                DataTable GetBinderAllotQtyDtl = objDbTrx.GetBinderAllotmentQtyViewNew(Convert.ToDateTime(startDate + " 00:00:00.000"), Convert.ToDateTime(endDate + " 23:59:59.999"), AccadYear, page.Start, page.End, allotmentcode);
                if (GetBinderAllotQtyDtl.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < GetBinderAllotQtyDtl.Rows.Count; iCnt++)
                    {
                        BinderAllotQuantity objBinderAllotQuantity = new BinderAllotQuantity();
                        objBinderAllotQuantity.AllotmentCode = GetBinderAllotQtyDtl.Rows[iCnt]["BINDER_ALLOT_CODE"].ToString();
                        objBinderAllotQuantity.ID = Convert.ToInt32(GetBinderAllotQtyDtl.Rows[iCnt]["ID"].ToString());
                        objBinderAllotQuantity.BookCode = GetBinderAllotQtyDtl.Rows[iCnt]["BOOK_CODE"].ToString();
                        objBinderAllotQuantity.BinderName = GetBinderAllotQtyDtl.Rows[iCnt]["BinderName"].ToString();
                        objBinderAllotQuantity.LanguageName = GetBinderAllotQtyDtl.Rows[iCnt]["LANGUAGE"].ToString();
                        objBinderAllotQuantity.BookName = GetBinderAllotQtyDtl.Rows[iCnt]["BOOK_NAME"].ToString();
                        objBinderAllotQuantity.AllotmentDate = Convert.ToDateTime(GetBinderAllotQtyDtl.Rows[iCnt]["ALLOTMENT_DATE"].ToString()).ToString("dd-MMM-yyyy").ToUpper();
                        objBinderAllotQuantity.TotQty = Convert.ToInt32(GetBinderAllotQtyDtl.Rows[iCnt]["TOT_QTY"].ToString());
                        objBinderAllotQuantity.Lot = Convert.ToInt32(GetBinderAllotQtyDtl.Rows[iCnt]["LOT"].ToString());
                        objBinderAllotQuantity.ReqQty = Convert.ToInt32(GetBinderAllotQtyDtl.Rows[iCnt]["REQ_QTY"].ToString());
                        objBinderAllotQuantity.QtyIssued = Convert.ToInt32(GetBinderAllotQtyDtl.Rows[iCnt]["QTY_ISSUED"].ToString());
                        objBinderAllotQuantity.SaveStatus = Convert.ToInt16(GetBinderAllotQtyDtl.Rows[iCnt]["STATUS"].ToString());
                        listBinderAllotQuantity.Add(objBinderAllotQuantity);
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }

            if (isonlyjson > default(int))
            {
                var jsonResult = Json(listBinderAllotQuantity, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            else
            {
                return PartialView("_BinderAllotList", listBinderAllotQuantity);
            }
        }

        [NonAction]
        private void PrepareAndInsertDataForBinderAllotQtyDtl(string[] ChallanIds, string ChallanNo, string userId, int padcount)
        {
            foreach (var chr in ChallanIds)
            {
                string xData = string.Empty;
                bool result = default(bool);
                List<BinderAllotQuantityDtlMinimal> lstDtl = new List<BinderAllotQuantityDtlMinimal>();
                BinderAllotQuantity objBinderAllotQuantity = new BinderAllotQuantity();
                try
                {
                    DataTable GetBinderAllotQtyDtl = objDbTrx.GetBinderAllotmentQtyByID(Convert.ToInt32(chr));
                    if (GetBinderAllotQtyDtl.Rows.Count > 0)
                    {
                        objBinderAllotQuantity.ID = Convert.ToInt32(GetBinderAllotQtyDtl.Rows[0]["ID"].ToString());
                        objBinderAllotQuantity.AllotmentCode = GetBinderAllotQtyDtl.Rows[0]["BINDER_ALLOT_CODE"].ToString();
                        objBinderAllotQuantity.BookCode = GetBinderAllotQtyDtl.Rows[0]["BOOK_CODE"].ToString();
                        objBinderAllotQuantity.BinderName = GetBinderAllotQtyDtl.Rows[0]["BinderName"].ToString();
                        objBinderAllotQuantity.BinderShortCode = GetBinderAllotQtyDtl.Rows[0]["BinderShortCode"].ToString();
                        objBinderAllotQuantity.LanguageName = GetBinderAllotQtyDtl.Rows[0]["LANGUAGE"].ToString();
                        objBinderAllotQuantity.BookName = GetBinderAllotQtyDtl.Rows[0]["BOOK_NAME"].ToString();
                        objBinderAllotQuantity.AllotmentDate = Convert.ToDateTime(GetBinderAllotQtyDtl.Rows[0]["ALLOTMENT_DATE"].ToString()).ToString("dd-MMM-yyyy").ToUpper();
                        objBinderAllotQuantity.TotQty = Convert.ToInt32(GetBinderAllotQtyDtl.Rows[0]["TOT_QTY"].ToString());
                        objBinderAllotQuantity.Lot = Convert.ToInt32(GetBinderAllotQtyDtl.Rows[0]["LOT"].ToString());
                        objBinderAllotQuantity.ReqQty = Convert.ToInt32(GetBinderAllotQtyDtl.Rows[0]["REQ_QTY"].ToString());
                        objBinderAllotQuantity.QtyIssued = Convert.ToInt32(GetBinderAllotQtyDtl.Rows[0]["QTY_ISSUED"].ToString());
                        objBinderAllotQuantity.SaveStatus = Convert.ToInt16(GetBinderAllotQtyDtl.Rows[0]["STATUS"].ToString());
                    }

                    if (objBinderAllotQuantity != null && objBinderAllotQuantity.ID > default(int))
                    {
                        int StartNo = 1;
                        int EndNo = objBinderAllotQuantity.ReqQty;
                        for (int iCnt = StartNo; iCnt <= EndNo; iCnt++)
                        {
                            var code = string.Format("{0}{1}", objBinderAllotQuantity.AllotmentCode, Convert.ToInt64(iCnt.ToString().PadLeft((padcount + 1), '0')).ToString().PadLeft(3, '0'));
                            lstDtl.Add(new BinderAllotQuantityDtlMinimal()
                            {
                                //BINDER_ALLOT_ID = objBinderAllotQuantity.ID,
                                BINDER_ALLOT_CODE = code,
                                //BINDER_SHORT_CODE = objBinderAllotQuantity.BinderShortCode,
                                STICKER_CODE = iCnt.ToString().PadLeft((padcount + 1), '0'),
                                //CHALLAN_ID = default(int),
                                //BOOK_CODE = objBinderAllotQuantity.BookCode,
                                //SCANNED_STATUS = default(int),
                                //CREATED_BY = userId,
                            });
                        }

                        if (lstDtl.Count() > default(int))
                        {
                            xData = Utility.CreateXmlTraditional(Utility.ToDataTable<BinderAllotQuantityDtlMinimal>(lstDtl)).InnerXml;
                            /*try
                            {
                                if (!System.IO.Directory.Exists(Server.MapPath("~/Logs/XML/")))
                                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Logs/XML/"));

                                System.IO.File.WriteAllText(string.Format("{0}{1}.txt", Server.MapPath("~/Logs/XML/"), DateTime.Now.ToString("ddMMyyyyHHmmss")), xData);
                            }
                            catch { }*/
                            result = objDbTrx.BinderAllotmentDtlInsert(objBinderAllotQuantity.ID, objBinderAllotQuantity.BinderShortCode, default(int), objBinderAllotQuantity.BookCode, default(int), userId, xData);
                        }
                    }
                }
                catch (Exception ex)
                {
                    objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
                }
            }
        }

        [HttpGet]
        [ActionName("GetGetBinderLotDetails")]
        public ActionResult GetGetBinderLotDetails(int challanId)
        {
            List<BinderAllotQuantityDtl> lstDtl = new List<BinderAllotQuantityDtl>();
            try
            {
                var userSessionObject = Session["UserSec"] != null ? GlobalSettings.oUserData : new UserSec();
                if (string.IsNullOrEmpty(userSessionObject.UserId))
                {
                    throw new Exception("Session timed out. Please login again.");
                }
                DataTable dtblDetail = objDbTrx.GetBinderAllotDetailByChallanId(challanId, userSessionObject.UserId);
                if (dtblDetail.Rows.Count > 0)
                {
                    for (int i = 0; i < dtblDetail.Rows.Count; i++)
                    {
                        lstDtl.Add(new BinderAllotQuantityDtl()
                        {
                            DTLID = Convert.ToInt64(dtblDetail.Rows[i]["DTLID"].ToString()),
                            //BINDER_ID = Convert.ToInt32(dtblDetail.Rows[i]["BINDER_ID"].ToString()),
                            STICKER_CODE = dtblDetail.Rows[i]["STICKER_CODE"].ToString(),
                            BINDER_ALLOT_CODE = dtblDetail.Rows[i]["BINDER_ALLOT_CODE"].ToString(),
                            BINDER_SHORT_CODE = dtblDetail.Rows[i]["BINDER_SHORT_CODE"].ToString(),
                            BOOK_CODE = dtblDetail.Rows[i]["BOOK_CODE"].ToString(),
                            SCANNED_STATUS = Convert.ToInt32(dtblDetail.Rows[i]["SCANNED_STATUS"].ToString())
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }

            return PartialView("~/Views/InvoiceCumChallanReqList/_ReqBinderDetail.cshtml", lstDtl);
        }

        public ActionResult PrintBinderLot(Int32 BinderAllotmentId)
        {
            BinderAllotQuantity objBinderAllotQuantity = new BinderAllotQuantity();
            List<BinderAllotQuantityDtl> lstDtl = new List<BinderAllotQuantityDtl>();
            try
            {
                StringBuilder strReport = new StringBuilder();
                DataTable GetBinderAllotQtyDtl = objDbTrx.GetBinderAllotmentQtyByID(BinderAllotmentId);
                if (GetBinderAllotQtyDtl.Rows.Count > 0)
                {
                    objBinderAllotQuantity.ID = Convert.ToInt32(GetBinderAllotQtyDtl.Rows[0]["ID"].ToString());
                    objBinderAllotQuantity.AllotmentCode = GetBinderAllotQtyDtl.Rows[0]["BINDER_ALLOT_CODE"].ToString();
                    //objBinderAllotQuantity.BookCode = GetBinderAllotQtyDtl.Rows[0]["BOOK_CODE"].ToString();
                    objBinderAllotQuantity.BookCode = GetBinderAllotQtyDtl.Rows[0]["COMMON_BOOK_CODE"].ToString();
                    objBinderAllotQuantity.BinderName = GetBinderAllotQtyDtl.Rows[0]["BinderName"].ToString();
                    objBinderAllotQuantity.LanguageName = GetBinderAllotQtyDtl.Rows[0]["LANGUAGE"].ToString();
                    objBinderAllotQuantity.BookName = GetBinderAllotQtyDtl.Rows[0]["BOOK_NAME"].ToString();
                    objBinderAllotQuantity.AllotmentDate = Convert.ToDateTime(GetBinderAllotQtyDtl.Rows[0]["ALLOTMENT_DATE"].ToString()).ToString("dd-MMM-yyyy").ToUpper();
                    objBinderAllotQuantity.TotQty = Convert.ToInt32(GetBinderAllotQtyDtl.Rows[0]["TOT_QTY"].ToString());
                    objBinderAllotQuantity.Lot = Convert.ToInt32(GetBinderAllotQtyDtl.Rows[0]["LOT"].ToString());
                    objBinderAllotQuantity.ReqQty = Convert.ToInt32(GetBinderAllotQtyDtl.Rows[0]["REQ_QTY"].ToString());
                    objBinderAllotQuantity.QtyIssued = Convert.ToInt32(GetBinderAllotQtyDtl.Rows[0]["QTY_ISSUED"].ToString());
                    objBinderAllotQuantity.SaveStatus = Convert.ToInt16(GetBinderAllotQtyDtl.Rows[0]["STATUS"].ToString());

                    DataTable dtblDetail = objDbTrx.GetBinderAllotmentDtlByBinderId(objBinderAllotQuantity.AllotmentCode, default(int), objBinderAllotQuantity.ID);
                    if (dtblDetail.Rows.Count > default(int))
                    {
                        for (int i = 0; i < dtblDetail.Rows.Count; i++)
                        {
                            lstDtl.Add(new BinderAllotQuantityDtl()
                            {
                                DTLID = Convert.ToInt64(dtblDetail.Rows[i]["DTLID"].ToString()),
                                //BINDER_ID = Convert.ToInt32(dtblDetail.Rows[i]["BINDER_ID"].ToString()),
                                STICKER_CODE = dtblDetail.Rows[i]["STICKER_CODE"].ToString(),
                                BINDER_ALLOT_CODE = dtblDetail.Rows[i]["BINDER_ALLOT_CODE"].ToString(),
                                BINDER_SHORT_CODE = dtblDetail.Rows[i]["BINDER_SHORT_CODE"].ToString(),
                                BOOK_CODE = dtblDetail.Rows[i]["BOOK_CODE"].ToString(),
                                SCANNED_STATUS = Convert.ToInt32(dtblDetail.Rows[i]["SCANNED_STATUS"].ToString())
                            });
                        }
                    }

                    if (lstDtl.Count() > default(int))
                    {
                        int StartNo = 0, EndNo = 0;
                        int NoOfColumns = 3, colCnt = 0;
                        StartNo = 1;
                        EndNo = objBinderAllotQuantity.ReqQty;
                        var cnt = 0;
                        var barcode3 = 1;
                        var header = new StringBuilder();
                        strReport.AppendLine(" <style type='text/css'>");
                        strReport.AppendLine(".hidden{display:none}.visible{display:block}.button{background-color:#008CBA;border:none;color:#fff;padding:6px 14px;text-align:center;text-decoration:none;display:inline-block;font-size:16px;margin:4px 2px;cursor:pointer}table.LotNoGen td,th{padding:18px;vertical-align:middle}.binderdetails{display:inline-block;margin-right:10px}.col-3{width:33%;height:3.39cm;display:inline-block;text-align:center;margin:13px 0 14px;position: relative;}.bar-code-scan {position: absolute; top: 0; right: 0; bottom: 0; left: 0; margin: auto 0 auto 15px; height: 50px;}.barcode-3 {margin: 0 0 0 50px;}.barcode-1 {margin:0 45px 0 0;}span.barcode{font-size:10px;}html,body{width:23cm;max-height:29.4cm;margin:0!important!important}span.barcode{font-size:12;}span.barcodetitle{font-size:10px}.inline{font-size:10px}");

                        strReport.AppendLine(" </style>");
                        header.Append(" <div class='tblBinderAllotMent inline' cellpadding='0' cellspacing='0' border='0' width='100%'>");

                        header.Append("<div class='binderdetails' >Binder Name.: <b>");
                        header.Append(objBinderAllotQuantity.BinderName);
                        header.Append(",</b></div>");
                        header.Append("<div class='binderdetails' >Language: <b>");
                        header.Append(objBinderAllotQuantity.LanguageName);
                        header.Append(" ,</b></div>");


                        header.Append("<div class='binderdetails'>Allotment Date: <b>");
                        header.Append(objBinderAllotQuantity.AllotmentDate);
                        header.Append(" ,</b></div>");
                        header.Append("<div class='binderdetails'>Allotment Qty: <b>");
                        header.Append(objBinderAllotQuantity.TotQty);
                        header.Append(" ,</b></div>");

                        header.Append("<div class='binderdetails'>Book Name.: <b>");
                        header.Append(objBinderAllotQuantity.BookName);
                        header.Append(" ,</b></div>");
                        header.Append("<div class='binderdetails'>Book Code: <b>");
                        header.Append(objBinderAllotQuantity.BookCode);
                        header.Append(",</b></div>");

                        header.Append("<div class='binderdetails' >Lot: <b>");
                        header.Append(objBinderAllotQuantity.Lot);
                        header.Append(" ,</b></div>");
                        header.Append("<div class='binderdetails'>No of Sticker : <b>");
                        header.Append(objBinderAllotQuantity.ReqQty);
                        header.Append(" </b></div>");

                        header.Append("</div>");

                        //strReport.AppendLine( "<div border='0' width='100%' class='LotNoGen'>");
                        //strReport.AppendLine(header.ToString());
                        foreach (var obj in lstDtl)
                        {
                            if (cnt % 24 == 0)
                                strReport.AppendLine(header.ToString());
                            //if (colCnt == 0)
                            //    strReport.AppendLine("<tr>");

                            //strReport.AppendLine("<td>");
                            strReport.AppendLine("<div class='col-3'><div class='bar-code-scan'>");
                            if (barcode3 % 3 == 0)
                                strReport.AppendLine("<div class='barcode-3'>");

                            if (cnt % 3 == 0)
                                strReport.AppendLine("<div class='barcode-1'>");

                            strReport.AppendLine("  <span class='barcode' style='font-family: IDAutomationHC39M Free Version; text-align: center;'>*" + obj.BINDER_ALLOT_CODE + "*</span>");
                            //strReport.AppendLine("  <span  style='font-family: IDAutomationHC39M Free Version; text-align: center;'>*" + string.Format("{0}{1}", obj.BINDER_ALLOT_CODE, Convert.ToInt64(obj.STICKER_CODE).ToString().PadLeft(3, '0')) + "*</span>");

                            strReport.AppendLine("  <br><span class='barcodetitle'>" + obj.STICKER_CODE + "&nbsp;&nbsp;" + obj.BOOK_CODE.ToUpper() + "&nbsp;&nbsp;&nbsp;" + objBinderAllotQuantity.LanguageName.ToUpper() + "&nbsp;&nbsp;&nbsp;" + objBinderAllotQuantity.BookName.ToUpper() + "&nbsp;&nbsp;&nbsp;" + obj.BINDER_SHORT_CODE.ToUpper());
                            //strReport.AppendLine("</td>");
                            strReport.AppendLine("</span>");
                            if (barcode3 % 3 == 0)
                                strReport.AppendLine("</div>");
                            if (cnt % 3 == 0)
                                strReport.AppendLine("</div>");
                            strReport.AppendLine("</div></div>");
                            //colCnt++;
                            //if (colCnt == NoOfColumns)
                            //{
                            //    strReport.AppendLine("</tr>");
                            //    colCnt = 0;
                            //}
                            cnt++;
                            barcode3++;
                        }
                        //strReport.AppendLine(" </div> "):
                        //if (colCnt > 0)
                        //{
                        //    for (int iCnt = colCnt; iCnt < NoOfColumns; iCnt++)
                        //    {
                        //        strReport.AppendLine("<td>&nbsp;</td>");
                        //    }
                        //    strReport.AppendLine("</tr>");
                        //}

                        objBinderAllotQuantity.PrintDtl = strReport.ToString();
                        var pdfBytes = (new NReco.PdfGenerator.HtmlToPdfConverter()).GeneratePdf(objBinderAllotQuantity.PrintDtl);
                        return new FileContentResult(pdfBytes, "application/pdf");

                        //return View(objBinderAllotQuantity);
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }



            // return View(objBinderAllotQuantity);
            //return  Content(objBinderAllotQuantity.PrintDtl);
            //var htmlContent = String.Format(objBinderAllotQuantity.PrintDtl, DateTime.Now);

            return Content("Print Error");

        }

        public ActionResult BinderAllotmentOperation(Int32 AllotmentId, string Command)
        {
            try
            {
                if (Command == "Edit" || Command == "Confirmed")
                {
                    return RedirectToAction("Index", "TrxBinderAllotmentQuantity", new { BinderAllotmentId = AllotmentId });
                }
                else if (Command == "Print")
                {
                    return RedirectToAction("PrintBinderLot", "TrxBinderAllotmentQuantityView", new { BinderAllotmentId = AllotmentId });
                }
            }
            catch (Exception ex) { objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress); }
            return RedirectToAction("Index", "SchRequisionView");
        }


        public ActionResult BinderAllotmentOperationBarcodePrinting(Int32 AllotmentId, Int32 From, Int32 To, string Command)
        {
            try
            {
                if (Command == "Edit" || Command == "Confirmed")
                {
                    return RedirectToAction("Index", "TrxBinderAllotmentQuantity", new { BinderAllotmentId = AllotmentId });
                }
                else if (Command == "Print")
                {
                    return RedirectToAction("PrintBinderLotPrintBarcode", "TrxBinderAllotmentQuantityView", new { BinderAllotmentId = AllotmentId, From = From, To = To });
                }
            }
            catch (Exception ex) { objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress); }
            return RedirectToAction("Index", "SchRequisionView");
        }

        public string PrintBinderLotPrintBarcode(Int32 BinderAllotmentId, Int32 From, Int32 To, string IP)
        {
            BinderAllotQuantity objBinderAllotQuantity = new BinderAllotQuantity();
            List<BinderAllotQuantityDtl> lstDtl = new List<BinderAllotQuantityDtl>();
            try
            {
                objBinderAllotQuantity.barcodeDPL = new StringBuilder();
                StringBuilder strReport = new StringBuilder();
                DataTable GetBinderAllotQtyDtl = objDbTrx.GetBinderAllotmentQtyByID(BinderAllotmentId);
                if (GetBinderAllotQtyDtl.Rows.Count > 0)
                {
                    objBinderAllotQuantity.ID = Convert.ToInt32(GetBinderAllotQtyDtl.Rows[0]["ID"].ToString());
                    objBinderAllotQuantity.AllotmentCode = GetBinderAllotQtyDtl.Rows[0]["BINDER_ALLOT_CODE"].ToString();
                    //objBinderAllotQuantity.BookCode = GetBinderAllotQtyDtl.Rows[0]["BOOK_CODE"].ToString();
                    objBinderAllotQuantity.BookCode = GetBinderAllotQtyDtl.Rows[0]["COMMON_BOOK_CODE"].ToString();
                    objBinderAllotQuantity.BinderName = GetBinderAllotQtyDtl.Rows[0]["BinderName"].ToString();
                    objBinderAllotQuantity.LanguageName = GetBinderAllotQtyDtl.Rows[0]["LANGUAGE"].ToString();
                    objBinderAllotQuantity.BookName = GetBinderAllotQtyDtl.Rows[0]["BOOK_NAME"].ToString();
                    objBinderAllotQuantity.AllotmentDate = Convert.ToDateTime(GetBinderAllotQtyDtl.Rows[0]["ALLOTMENT_DATE"].ToString()).ToString("dd-MMM-yyyy").ToUpper();
                    objBinderAllotQuantity.TotQty = Convert.ToInt32(GetBinderAllotQtyDtl.Rows[0]["TOT_QTY"].ToString());
                    objBinderAllotQuantity.Lot = Convert.ToInt32(GetBinderAllotQtyDtl.Rows[0]["LOT"].ToString());
                    objBinderAllotQuantity.ReqQty = Convert.ToInt32(GetBinderAllotQtyDtl.Rows[0]["REQ_QTY"].ToString());
                    objBinderAllotQuantity.QtyIssued = Convert.ToInt32(GetBinderAllotQtyDtl.Rows[0]["QTY_ISSUED"].ToString());
                    objBinderAllotQuantity.SaveStatus = Convert.ToInt16(GetBinderAllotQtyDtl.Rows[0]["STATUS"].ToString());

                    DataTable dtblDetail = objDbTrx.GetBinderAllotmentDtlByBinderIdFrom_To(objBinderAllotQuantity.AllotmentCode, default(int), objBinderAllotQuantity.ID, From, To);
                    if (dtblDetail.Rows.Count > default(int))
                    {
                        for (int i = 0; i < dtblDetail.Rows.Count; i++)
                        {
                            lstDtl.Add(new BinderAllotQuantityDtl()
                            {
                                DTLID = Convert.ToInt64(dtblDetail.Rows[i]["DTLID"].ToString()),
                                //BINDER_ID = Convert.ToInt32(dtblDetail.Rows[i]["BINDER_ID"].ToString()),
                                STICKER_CODE = dtblDetail.Rows[i]["STICKER_CODE"].ToString(),
                                BINDER_ALLOT_CODE = dtblDetail.Rows[i]["BINDER_ALLOT_CODE"].ToString(),
                                BINDER_SHORT_CODE = dtblDetail.Rows[i]["BINDER_SHORT_CODE"].ToString(),
                                //BOOK_CODE = dtblDetail.Rows[i]["BOOK_CODE"].ToString(),
                                BOOK_CODE = dtblDetail.Rows[i]["COMMON_BOOK_CODE"].ToString(),
                                SCANNED_STATUS = Convert.ToInt32(dtblDetail.Rows[i]["SCANNED_STATUS"].ToString())
                            });
                        }
                    }

                    if (lstDtl.Count() > default(int))
                    {
                        var cnt = 0;
                        var barcode3 = 1;
                        foreach (var obj in lstDtl)
                        {
                            string book_name = objBinderAllotQuantity.BookName;
                            string booknameline1 = book_name.Substring(0, book_name.Length >= 16 ? 16 : book_name.Length);
                            string booknameline2 = "";
                            string booknameline3 = "";
                            if (book_name.Length > 16)
                            {
                                int pending_len = book_name.Length - 16;
                                if (pending_len > 16)
                                {
                                    booknameline2 = book_name.Substring(16, 16);
                                    pending_len = book_name.Length - 32;
                                    if (pending_len > 16)
                                    {
                                        booknameline3 = book_name.Substring(32, 16);
                                    }
                                    else
                                    {
                                        booknameline3 = book_name.Substring(32, pending_len);
                                    }
                                }
                                else
                                {
                                    booknameline2 = book_name.Substring(16, pending_len);
                                }
                            }
                            cnt++;

                            if (cnt % 2 != 0)
                            {
                                objBinderAllotQuantity.barcodeDPL.AppendLine("SYSVAR(48) = 0");
                                objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "SYSVAR(35)=0");
                                objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "qXPos% = 5");
                                objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "FT " + @"""Univers Bold""" + ",8,0,99");
                                objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "PP40+qXPos%,68:BARSET " + @"""QRCODE""" + ",1,1,6,2,1:PB " + '"' + obj.BINDER_ALLOT_CODE + '"');
                                objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "PP178++qXPos%,165:PT " + '"' + obj.BOOK_CODE + '"');
                                objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "PP178+qXPos%,138:PT " + '"' + booknameline1.ToUpper() + '"');
                                objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "PP178+qXPos%,109:PT " + '"' + booknameline2.ToUpper() + '"');
                                objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "PP178+qXPos%,85:PT " + '"' + booknameline3.ToUpper() + '"');
                                objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "PP44+qXPos%,44:PT " + '"' + obj.BINDER_ALLOT_CODE + '"');
                                objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "PP350+qXPos%,44:PT " + '"' + obj.BINDER_SHORT_CODE + '"');
                                objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "FT " + @"""Univers Bold""" + ",8,0,99");
                                objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "PP29+qXPos%,15:PT " + @""" ALOT QTY:" + objBinderAllotQuantity.TotQty + '"');
                                objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "PP231+qXPos%,15:PT " + @""" STICKERS:" + objBinderAllotQuantity.ReqQty + '"');
                                objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "qXPos% = 420");
                                objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "FT " + @"""Univers Bold""" + ",8,0,99");
                                if (cnt == lstDtl.Count())
                                {
                                    objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "PF" + System.Environment.NewLine);
                                }
                            }
                            else if (cnt % 2 == 0)
                            {
                                objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "PP40+qXPos%,68:BARSET " + @"""QRCODE""" + ",1,1,6,2,1:PB " + '"' + obj.BINDER_ALLOT_CODE + '"');
                                objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "PP178++qXPos%,165:PT " + '"' + obj.BOOK_CODE + '"');
                                objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "PP178+qXPos%,138:PT " + '"' + booknameline1.ToUpper() + '"');
                                objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "PP178+qXPos%,109:PT " + '"' + booknameline2.ToUpper() + '"');
                                objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "PP178+qXPos%,85:PT " + '"' + booknameline3.ToUpper() + '"');
                                objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "PP44+qXPos%,44:PT " + '"' + obj.BINDER_ALLOT_CODE + '"');
                                objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "PP350+qXPos%,44:PT " + '"' + obj.BINDER_SHORT_CODE + '"');
                                objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "FT " + @"""Univers Bold""" + ",8,0,99");
                                objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "PP29+qXPos%,15:PT " + @""" ALOT QTY:" + objBinderAllotQuantity.TotQty + '"');
                                objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "PP231+qXPos%,15:PT " + @""" STICKERS:" + objBinderAllotQuantity.ReqQty + '"');
                                objBinderAllotQuantity.barcodeDPL.AppendLine(System.Environment.NewLine + "PF" + System.Environment.NewLine);
                            }

                        }
                        System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
                        try
                        {
                            client.Connect(IP, 9100);
                        }
                        catch
                        {
                            return "Please check printer connected to network";
                        }

                        try
                        {
                            System.IO.StreamWriter writer = new StreamWriter(client.GetStream());
                            writer.Write(objBinderAllotQuantity.barcodeDPL.ToString());
                            writer.Flush();
                            writer.Close();
                            client.Close();
                            return "success";
                        }
                        catch
                        {
                            return "Printing error";
                        }
                        //ViewBag.BarcodeDPL = barcodeDPL;
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }

            return "Print Error";

        }


        public JsonResult DeleteBinderAllotment(string ReqisitionId)
        {
            string ErrorMessage = "";
            try
            {
                DataTable dtBinderAllotment = objDbTrx.GetBinderAllotmentQtyByID(Convert.ToInt32(ReqisitionId));
                if (dtBinderAllotment.Rows.Count > 0)
                {
                    ErrorMessage = "The Binder Allotment Code " + dtBinderAllotment.Rows[0]["BINDER_ALLOT_CODE"].ToString() + " and Allotment date " + Convert.ToDateTime(dtBinderAllotment.Rows[0]["ALLOTMENT_DATE"].ToString()).ToString("dd-MMM-yyyy").ToUpper() + "  deleted successfully";
                }
                dtBinderAllotment.Dispose();
                dtBinderAllotment.Clear();
                objDbTrx.DeleteBinderAllotment(ReqisitionId);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Some error occured while deleting requisition. please contact system administrator for further assitence.";
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(ErrorMessage);
        }
        [HttpPost]
        public JsonResult ConfirmBinderAllotment(string griddata)
        {
            string[] ChallanIds = griddata.TrimEnd(',').Split(',');
            string[] distinctChallanIds;
            string ErrorMessage = "";
            try
            {
                BinderAllotQuantity objBinderAllotQuantity = new BinderAllotQuantity();
                objBinderAllotQuantity.UserId = GlobalSettings.oUserData.UserId;
                objBinderAllotQuantity.SaveStatus = 1;
                objDbTrx.BinderAllotmentConfirm(objBinderAllotQuantity, griddata.TrimEnd(','));

                if (ChallanIds != null && ChallanIds.Count() > default(int))
                {
                    distinctChallanIds = ChallanIds.Distinct().ToArray();
                    var userId = GlobalSettings.oUserData.UserId;
                    var padcount = GlobalSettings.oAcademicYear.FormatNumberPaddingCount;
                    System.Threading.ThreadPool.QueueUserWorkItem(s =>
                    {
                        PrepareAndInsertDataForBinderAllotQtyDtl(distinctChallanIds, "", userId, padcount);
                    });
                }

                ErrorMessage = ChallanIds.Count() + " Binder Allotment confirmed successfully.";
            }
            catch (Exception ex)
            {
                ErrorMessage = "Some Error occured while confirming Requisition. Please confirm system administrator";
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(ErrorMessage);
        }

        public ActionResult ExportBinderAllotment(string startDate, string endDate)
        {
            List<BinderAllotQuantity> listBinderAllotQuantity = new List<BinderAllotQuantity>();
            try
            {

                StringBuilder strTableReport = new StringBuilder();
                StringBuilder strReport = new StringBuilder();
                strReport = new StringBuilder();
                Int16 AccadYear = Convert.ToInt16(GlobalSettings.oUserData.AcademicYearId);
                DataTable dtbinderallot = objDbTrx.GetBinderAllotmentQtyView(Convert.ToDateTime(startDate + " 00:00:00.000"), Convert.ToDateTime(endDate + " 23:59:59.999"), AccadYear);
                if (dtbinderallot.Rows.Count > 0)
                {
                    strReport.AppendLine("<tr>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:#b3cbff;' >BINDER_ALLOT_CODE</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:#b3cbff;' >BOOK_CODE</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:#b3cbff;' >BinderName</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:#b3cbff;' >LANGUAGE</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:#b3cbff;' >BOOK_NAME</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:#b3cbff;' >ALLOTMENT_DATE</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:#b3cbff;' >TOT_QTY</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:#b3cbff;' >LOT</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:#b3cbff;' >REQ_QTY</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:#b3cbff;' >QTY_ISSUED</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >STATUS</th>");

                    strReport.AppendLine("</tr>");
                    for (int iCnt = 0; iCnt < dtbinderallot.Rows.Count; iCnt++)
                    {
                        strReport.AppendLine("<tr>");
                        strReport.AppendLine("      <td> " + dtbinderallot.Rows[iCnt]["BINDER_ALLOT_CODE"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dtbinderallot.Rows[iCnt]["BOOK_CODE"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dtbinderallot.Rows[iCnt]["BinderName"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dtbinderallot.Rows[iCnt]["LANGUAGE"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dtbinderallot.Rows[iCnt]["BOOK_NAME"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dtbinderallot.Rows[iCnt]["ALLOTMENT_DATE"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dtbinderallot.Rows[iCnt]["TOT_QTY"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dtbinderallot.Rows[iCnt]["LOT"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dtbinderallot.Rows[iCnt]["REQ_QTY"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dtbinderallot.Rows[iCnt]["QTY_ISSUED"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + (dtbinderallot.Rows[iCnt]["STATUS"].ToString() == "0" ? "Draft" : "Confirmed") + "      </td>");
                        strReport.AppendLine("</tr>");
                    }
                    strTableReport.AppendLine("<table border='1'>");
                    strTableReport.AppendLine("          " + strReport.ToString());
                    strTableReport.AppendLine("</table>");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    String FileName = "BinderAllotBooks" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".xls";

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




        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            if (session.IsNewSession || Session["UserSec"] == null)
            {
                filterContext.Result = new RedirectResult("/SessionExpire/Index");
                return;
            }
            base.OnActionExecuting(filterContext);
        }

    }
}
