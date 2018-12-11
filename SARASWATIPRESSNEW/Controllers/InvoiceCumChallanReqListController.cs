using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SARASWATIPRESSNEW.Models;
using System.Collections;
using System.Text;
using SARASWATIPRESSNEW.BusinessLogicLayer;
using System.Net;
using System.IO;
using System.Net.Mail;
using CrystalDecisions.CrystalReports.Engine;

namespace SARASWATIPRESSNEW.Controllers
{
    public class InvoiceCumChallanReqListController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            UserSec objUser;
            ViewBag.Active = "InvoiceCumChallanReqList";
            DateTime now = DateTime.Now;
            try
            {
                objUser = Session["UserSec"] != null ? ((UserSec)Session["UserSec"]) : new UserSec();
            }
            catch (Exception)
            {
                objUser = new UserSec();
            }

            //return View(new InvoiceCumChallan()
            //{
            //    startDate = new DateTime(now.Year, now.Month, 1),
            //    endDate = now,
            //    CircleId = Convert.ToInt32(string.IsNullOrWhiteSpace(objUser.CircleID) ? "-1" : objUser.CircleID),
            //    DistrictId = Convert.ToInt32(string.IsNullOrWhiteSpace(objUser.DistrictID) ? "-1" : objUser.DistrictID)
            //});

            return View("ChallanRequest", new InvoiceCumChallan()
            {
                startDate = new DateTime(now.Year, now.Month, 1),
                endDate = now,
                CircleId = Convert.ToInt32(string.IsNullOrWhiteSpace(objUser.CircleID) ? "-1" : objUser.CircleID),
                DistrictId = Convert.ToInt32(string.IsNullOrWhiteSpace(objUser.DistrictID) ? "-1" : objUser.DistrictID)
            });
        }

        [HttpPost]
        [ActionName("UpdateRevertChallanDetails")]
        public ActionResult UpdateRevertChallanDetails(PartialChallanRevertObject mData)
        {
            bool result = default(bool);
            IEnumerable<RevisedQtyMap> psData = new List<RevisedQtyMap>();
            try
            {
                UserSec objUser;
                try
                {
                    objUser = Session["UserSec"] != null ? ((SARASWATIPRESSNEW.UserSec)Session["UserSec"]) : new SARASWATIPRESSNEW.UserSec();
                }
                catch (Exception)
                {
                    objUser = new SARASWATIPRESSNEW.UserSec();
                }

                string unescapedStr = HttpUtility.UrlDecode(mData.lst);
                psData = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<RevisedQtyMap>>(unescapedStr);
                if (psData != null && psData.Count() > default(int))
                {
                    result = objDbTrx.PartialChallanRevertUpdate(mData.ChallanId, objUser.UserId, psData);
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            if (result)
                return Json(new { Success = 1, Message = "Partial challan revert operation successful" }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Success = 0, Message = "Partial challan revert operation failed. Please try again later" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("GetChallanDetailsOnDemandForRevert")]
        public PartialViewResult GetChallanDetailsOnDemandForRevert(string challanId)
        {
            InvoiceCumChallan lst_invCumChal = new InvoiceCumChallan();
            try
            {
                lst_invCumChal.InvoiceCumChallanDate = DateTime.Now.ToString("dd-MMM-yyyy");
                lst_invCumChal.InvoiceCumChallanNo = "TBC" + (DateTime.Now.Month >= 4 ? DateTime.Now.Year.ToString().Substring(2) + "-" + (Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2)) + 1) + "-XXXXXXX" : (Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2)) - 1) + "-" + DateTime.Now.Year.ToString().Substring(2) + "-XXXXXXX").ToString();
                lst_invCumChal.ChallanId = 0;
                if (Convert.ToString(challanId) != "")
                {
                    lst_invCumChal.ChallanId = Convert.ToInt64(challanId);
                }
                if (Convert.ToInt64(lst_invCumChal.ChallanId) != 0)
                {
                    DataTable dtChallanDtl = objDbTrx.GetProvisionalChallanDetailsById(Convert.ToInt64(lst_invCumChal.ChallanId));
                    if (dtChallanDtl.Rows.Count > 0)
                    {
                        lst_invCumChal.InvoiceCumChallanNo = dtChallanDtl.Rows[0]["CHALLAN_NUMBER"].ToString();
                        lst_invCumChal.InvoiceCumChallanDate = Convert.ToDateTime(dtChallanDtl.Rows[0]["CHALLAN_DATE"].ToString()).ToString("dd-MMM-yyyy");
                        lst_invCumChal.CONSIGNEE_NO = dtChallanDtl.Rows[0]["CONSIGNEE_NO"].ToString();
                        lst_invCumChal.VEHICLE_NO = dtChallanDtl.Rows[0]["VEHICLE_NO"].ToString();
                        lst_invCumChal.TransporterID = Convert.ToInt16(dtChallanDtl.Rows[0]["TRANSPORTER_ID"].ToString());
                        lst_invCumChal.Transporter = dtChallanDtl.Rows[0]["TRANSPORT_NAME"].ToString();
                        lst_invCumChal.DistrictId = Convert.ToInt32(dtChallanDtl.Rows[0]["DISTRICT_ID"].ToString());
                        lst_invCumChal.CircleId = Convert.ToInt32(dtChallanDtl.Rows[0]["CircleId"].ToString());
                        lst_invCumChal.CategoryId = Convert.ToInt32(dtChallanDtl.Rows[0]["CHALLAN_BOOK_CATEGORY_ID"].ToString());
                        lst_invCumChal.LanguageId = Convert.ToInt32(dtChallanDtl.Rows[0]["LANGUAGE_ID"].ToString());
                        lst_invCumChal.CircleName = dtChallanDtl.Rows[0]["CIRCLE_NAME"].ToString();
                        lst_invCumChal.DistrictName = dtChallanDtl.Rows[0]["DISTRICT"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }

            return PartialView("_ChallanDetailsRevert", lst_invCumChal);
        }

        #region [For MVC Report Export to Excel using Crystal Report]
        [HttpGet]
        [ActionName("DataExportToExcel")]
        public ActionResult DataExportToExcel(string startDate, string endDate, string CircleID, string DistrictID, string challanNumber = "")
        {
            List<InvoiceCumChallanList> objChallanList = new List<InvoiceCumChallanList>();
            string report_name = string.Empty;
            try
            {
                DataTable dt = objDbTrx.GetChallanDtlModified(Convert.ToDateTime(startDate + " 00:00:00.000"), Convert.ToDateTime(endDate + " 23:59:59.999"), CircleID, DistrictID, challanNumber, 2);
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        InvoiceCumChallanList icc = new InvoiceCumChallanList();

                        icc.InvCumChallanNo = Convert.ToString(dt.Rows[iCnt]["Challan_Number"].ToString());
                        icc.DistrictName = dt.Rows[iCnt]["DISTRICT"].ToString();
                        icc.CircleName = Convert.ToString(dt.Rows[iCnt]["Circle_Name"].ToString());
                        icc.CategoryName = Convert.ToString(dt.Rows[iCnt]["CHALLAN_BOOK_CATEGORY"].ToString());
                        icc.LanguageName = Convert.ToString(dt.Rows[iCnt]["LANGUAGE"].ToString());
                        //icc.QtyShipped = Convert.ToInt32(dt.Rows[iCnt]["QtyShipped"].ToString());
                        //icc.CONSIGNEE_NO = Convert.ToString(dt.Rows[iCnt]["CONSIGNEE_NO"].ToString());
                        //icc.VEHICLE_NO = Convert.ToString(dt.Rows[iCnt]["VEHICLE_NO"].ToString());
                        //icc.InvoiceCumChallanDate = Convert.ToDateTime(dt.Rows[iCnt]["Challan_Date"].ToString()).ToString("dd-MMM-yyyy");



                        objChallanList.Add(icc);
                    }
                }

                if (objChallanList != null && objChallanList.Count() > default(int))
                {
                    report_name = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".xls";

                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "rptChallanDetails.rpt"));
                    rd.SetDataSource(objChallanList);

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


        [HttpPost]
        [ActionName("GetBooksReqDetailsForRevert")]
        public ActionResult GetBooksReqDetailsForRevert(string District, string CircleId, string categoryId, string languageId, string ChallanId, bool isPartialViewRequest = false)
        {
            InvoiceCumChallan lst_invCumChal = new InvoiceCumChallan();
            try
            {

                Int16 AccadYear = Convert.ToInt16(((UserSec)Session["UserSec"]).AcademicYearId);
                DataTable dtReqDtl = new DataTable();
                if (Convert.ToInt64(ChallanId) != 0)
                {
                    dtReqDtl = objDbTrx.GetChallanDetailsById(Convert.ToInt64(ChallanId));
                }
                DataTable dtTransaction = objDbTrx.GetChallanListDtlForRevert(CircleId, categoryId, languageId, ChallanId, AccadYear);

                DataTable dt = objDbTrx.GetBinderDtlListByChallanIdOnly(ChallanId);

                List<InvoiceCumChallanList> ObjlstInvCumCha = new List<InvoiceCumChallanList>();
                if (dtTransaction.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dtTransaction.Rows.Count; iCnt++)
                    {
                        InvoiceCumChallanList icc = new InvoiceCumChallanList();
                        icc.ClassName = Convert.ToString(dtTransaction.Rows[iCnt]["Class"].ToString());
                        icc.Book_Code = Convert.ToString(dtTransaction.Rows[iCnt]["Book_Code"].ToString());
                        icc.Common_Book_Code = Convert.ToString(dtTransaction.Rows[iCnt]["Common_Book_Code"].ToString());
                        icc.Book_Name = Convert.ToString(dtTransaction.Rows[iCnt]["book_name"].ToString());
                        icc.NetReqQty = Convert.ToInt32(dtTransaction.Rows[iCnt]["NetReqQtyAfterStockDeduction"].ToString()) > default(int) ? Convert.ToInt32(dtTransaction.Rows[iCnt]["NetReqQtyAfterStockDeduction"].ToString()) : default(int);
                        icc.AlreadyShippedQty = Convert.ToInt32(dtTransaction.Rows[iCnt]["AlreadyShipped"].ToString());
                        icc.QtyShipped = 0;
                        icc.Cartoon = "";
                        icc.TotalLot = Convert.ToInt32(dtTransaction.Rows[iCnt]["LOT"] != null && !string.IsNullOrWhiteSpace(dtTransaction.Rows[iCnt]["LOT"].ToString()) ? dtTransaction.Rows[iCnt]["LOT"].ToString() : default(int).ToString());
                        icc.Remarks = "";
                        if (dtReqDtl != null && dtReqDtl.Rows.Count > default(int))
                        {
                            for (int jCnt = 0; jCnt < dtReqDtl.Rows.Count; jCnt++)
                            {
                                if (dtTransaction.Rows[iCnt]["Book_Code"].ToString() == dtReqDtl.Rows[jCnt]["Book_Code"].ToString())
                                {
                                    icc.QtyShipped = Convert.ToInt32(dtReqDtl.Rows[jCnt]["QtyShippedQty"].ToString());
                                    icc.Cartoon = dtReqDtl.Rows[jCnt]["Cartoon"].ToString();
                                    break;
                                }
                            }
                        }
                        try
                        {
                            icc.RemainBal = (icc.NetReqQty - icc.AlreadyShippedQty) - icc.QtyShipped;
                            if (icc.RemainBal < 1)
                            {
                                icc.RemainBal = 0;
                            }
                        }
                        catch
                        {
                            icc.RemainBal = 0;
                        }
                        icc.BookSurplusQty = "Gross Req: " + dtTransaction.Rows[iCnt]["TOTAL"].ToString() + ",<br/> Stock total: " + dtTransaction.Rows[iCnt]["STOCK_TOTAL"].ToString() + ",<br/> Already Shiped: " + dtTransaction.Rows[iCnt]["ALREADYSHIPPED"].ToString() + ",<br/> Extra Book " + dtTransaction.Rows[iCnt]["SURPLUS_QTY"].ToString() + " " + (dtTransaction.Rows[iCnt]["SURPLUS_MODE"].ToString() == "PER" ? "%" : "Copies");
                        ObjlstInvCumCha.Add(icc);
                    }

                    if (dt != null && dt.Rows.Count > default(int))
                    {
                        if (ObjlstInvCumCha != null && ObjlstInvCumCha.Count() > default(int))
                        {
                            List<BinderDtlListByChallan> lst = new List<BinderDtlListByChallan>();
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                BinderDtlListByChallan obj = new BinderDtlListByChallan();
                                obj.BOOK_CODE = dt.Rows[i]["BOOK_CODE"].ToString();
                                obj.COMMON_BOOK_CODE = dt.Rows[i]["COMMON_BOOK_CODE"].ToString();
                                obj.TotalScannedCount = Convert.ToInt32(dt.Rows[i]["TotalScannedCount"].ToString());
                                obj.LOT = Convert.ToInt32(dt.Rows[i]["LOT"].ToString());
                                lst.Add(obj);
                            }

                            foreach (var item in ObjlstInvCumCha)
                            {
                                try
                                {
                                    IEnumerable<BinderDtlListByChallan> obNew = lst.Where(s => s.BOOK_CODE == item.Book_Code);
                                    if (obNew != null && obNew.Count() > default(int))
                                    {
                                        var lstOb = obNew.GroupBy(x => x.BOOK_CODE).Select(g => new
                                        {
                                            Key = g.Key,
                                            Value = g.Sum(s => s.TotalScannedCount),
                                            LOT = g.First().LOT,
                                            LOTDELIMITED = string.Join(",", g.Select(c => c.LOT.ToString()).ToArray())
                                        });

                                        var ob = lstOb.FirstOrDefault();
                                        item.TotalLot = ob != null ? ob.LOT : item.TotalLot;
                                        item.TotalLotDelimited = ob != null ? (string.IsNullOrWhiteSpace(ob.LOTDELIMITED) ? item.TotalLot.ToString() : ob.LOTDELIMITED) : item.TotalLot.ToString();
                                        item.QtyShipped = ob != null ? ob.Value : item.QtyShipped;
                                        item.RemainBal = (item.NetReqQty - item.AlreadyShippedQty) - item.QtyShipped;
                                        if (item.RemainBal < 1)
                                            item.RemainBal = 0;
                                    }
                                }
                                catch (Exception) { }
                            }
                        }
                    }

                    lst_invCumChal.TotalAmount = 0;
                    lst_invCumChal.InvoiceCumChallanCollection = ObjlstInvCumCha.Where(o => o.NetReqQty > 0).ToList();
                }
                dtTransaction.Dispose();
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }

            if (isPartialViewRequest)
                return PartialView("~/Views/InvoiceCumChallanReqList/_ReqBookDtlForRevert.cshtml", lst_invCumChal);
            else
                return Json(lst_invCumChal, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("SendSMSToAll")]
        public JsonResult SendSMSToAll(string pData)
        {
            var unescapedData = HttpUtility.UrlDecode(pData);

            var objects = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<ChallanConfirmCommunication>>(unescapedData);

            if (objects != null && objects.Count() > default(int))
            {
                string[] pDataDefined = objects.Select(obj => obj.ChallanId.ToString()).ToArray();
                string fullstr = string.Join(",", pDataDefined).Replace('"', ' ').Trim().Replace(" ", "");

                DataTable dtbl = objDbTrx.GetConfirmedChallanInfoById(fullstr);

                System.Threading.ThreadPool.QueueUserWorkItem(s =>
                {
                    if (dtbl != null && dtbl.Rows.Count > default(int))
                    {
                        for (int iCnt = 0; iCnt < dtbl.Rows.Count; iCnt++)
                        {
                            Convert.ToString(dtbl.Rows[iCnt]["MOBILE_NO"].ToString());
                            Convert.ToString(dtbl.Rows[iCnt]["ALTERNATE_MOBILE_NO"].ToString());
                        }
                    }
                });
            }

            return Json(new { Success = 1, Message = "SMS sent successfully." }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("GetChallanDetailsOnDemand")]
        public PartialViewResult GetChallanDetailsOnDemand(string challanId)
        {
            InvoiceCumChallan lst_invCumChal = new InvoiceCumChallan();
            try
            {
                lst_invCumChal.InvoiceCumChallanDate = DateTime.Now.ToString("dd-MMM-yyyy");
                lst_invCumChal.InvoiceCumChallanNo = "TBC" + (DateTime.Now.Month >= 4 ? DateTime.Now.Year.ToString().Substring(2) + "-" + (Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2)) + 1) + "-XXXXXXX" : (Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2)) - 1) + "-" + DateTime.Now.Year.ToString().Substring(2) + "-XXXXXXX").ToString();
                lst_invCumChal.ChallanId = 0;
                if (Convert.ToString(challanId) != "")
                {
                    lst_invCumChal.ChallanId = Convert.ToInt64(challanId);
                }
                if (Convert.ToInt64(lst_invCumChal.ChallanId) != 0)
                {
                    DataTable dtChallanDtl = objDbTrx.GetProvisionalChallanDetailsById(Convert.ToInt64(lst_invCumChal.ChallanId));
                    if (dtChallanDtl.Rows.Count > 0)
                    {
                        lst_invCumChal.InvoiceCumChallanNo = dtChallanDtl.Rows[0]["CHALLAN_NUMBER"].ToString();
                        lst_invCumChal.InvoiceCumChallanDate = Convert.ToDateTime(dtChallanDtl.Rows[0]["CHALLAN_DATE"].ToString()).ToString("dd-MMM-yyyy");
                        lst_invCumChal.CONSIGNEE_NO = dtChallanDtl.Rows[0]["CONSIGNEE_NO"].ToString();
                        lst_invCumChal.VEHICLE_NO = dtChallanDtl.Rows[0]["VEHICLE_NO"].ToString();
                        lst_invCumChal.TransporterID = Convert.ToInt16(dtChallanDtl.Rows[0]["TRANSPORTER_ID"].ToString());
                        lst_invCumChal.Transporter = dtChallanDtl.Rows[0]["TRANSPORT_NAME"].ToString();
                        lst_invCumChal.DistrictId = Convert.ToInt32(dtChallanDtl.Rows[0]["DISTRICT_ID"].ToString());
                        lst_invCumChal.CircleId = Convert.ToInt32(dtChallanDtl.Rows[0]["CircleId"].ToString());
                        lst_invCumChal.CategoryId = Convert.ToInt32(dtChallanDtl.Rows[0]["CHALLAN_BOOK_CATEGORY_ID"].ToString());
                        lst_invCumChal.LanguageId = Convert.ToInt32(dtChallanDtl.Rows[0]["LANGUAGE_ID"].ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }

            return PartialView("_ChallanDetails", lst_invCumChal);
        }

        public ActionResult EditOperation(string Id, string Command)
        {
            Session["ChallanId"] = Id;
            return RedirectToAction("Index", "InvoiceCumChallan");
        }

        public ActionResult PrintOperation(string Id, string Command, int isdemoprint = 0)
        {
            Session["ChallanId"] = Id;
            return RedirectToAction("Index", "InvoiceCumChallanReport", new { @isdemoprint = isdemoprint });
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

        [HttpGet]
        [ActionName("GetChallanDetailsByChallanIdSimplified")]
        public ActionResult GetChallanDetailsByChallanIdSimplified(int ChallanId)
        {
            ChallanHeaderSimplified obj = new ChallanHeaderSimplified();
            try
            {
                DataTable dt = objDbTrx.GetChallanDetailsByIdSimplified(ChallanId);
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        obj.ChallanID = Convert.ToInt32(dt.Rows[iCnt]["ID"].ToString());
                        obj.CHALLAN_NUMBER = dt.Rows[iCnt]["CHALLAN_NUMBER"].ToString();
                        obj.CONSIGNEE_NO = dt.Rows[iCnt]["CONSIGNEE_NO"].ToString();
                        obj.TRANSPORTER_ID = Convert.ToInt32(dt.Rows[iCnt]["TRANSPORTER_ID"].ToString());
                        obj.VEHICLE_NO = dt.Rows[iCnt]["VEHICLE_NO"].ToString();
                        obj.Transport_Name = dt.Rows[iCnt]["Transport_Name"].ToString();
                        obj.ManualChallanNo = dt.Rows[iCnt]["ManualChallanNo"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return PartialView("~/Views/InvoiceCumChallan/_EditChallanHdr.cshtml", obj);
        }

        [HttpPost]
        [ActionName("GetChallanViewDataMinimal")]
        public ActionResult GetChallanViewDataMinimal(string startDate, string endDate, string CircleID, string DistrictID, string challanNumber = "", bool isPartialViewRequested = false, bool isFinalRequest = false)
        {
            List<InvoiceCumChallan> objChallanList = new List<InvoiceCumChallan>();
            try
            {
                DataTable dt = isFinalRequest ? objDbTrx.GetChallanDtlModifiedMinimal(Convert.ToDateTime(startDate + " 00:00:00.000"), Convert.ToDateTime(endDate + " 23:59:59.999"), CircleID, DistrictID, 2) : objDbTrx.GetChallanDtlModified(Convert.ToDateTime(startDate + " 00:00:00.000"), Convert.ToDateTime(endDate + " 23:59:59.999"), CircleID, DistrictID, challanNumber);
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        InvoiceCumChallan icc = new InvoiceCumChallan();
                        icc.ChallanId = Convert.ToInt64(dt.Rows[iCnt]["ID"].ToString());
                        icc.InvoiceCumChallanNo = Convert.ToString(dt.Rows[iCnt]["Challan_Number"].ToString());

                        objChallanList.Add(icc);
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            var jsonResult = Json(objChallanList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetPendingChallanViewDataDDL(string CircleID, string DistrictID)
        {
            List<string> lst = new List<string>();
            int NoOfDaysToConsiderForPendingChallan = default(int);
            try
            {
                if (ConfigurationManager.AppSettings["NoOfDaysToConsiderForPendingChallan"] != null)
                    int.TryParse(ConfigurationManager.AppSettings["NoOfDaysToConsiderForPendingChallan"], out NoOfDaysToConsiderForPendingChallan);
                else
                    NoOfDaysToConsiderForPendingChallan = 10;

                DataTable dt = objDbTrx.GetPendingChallanDtlBasedOnDayDiffDDL(NoOfDaysToConsiderForPendingChallan, CircleID, DistrictID);
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        lst.Add(dt.Rows[iCnt]["DayDifference"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(new { Data = lst }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("GetPendingChallanViewData")]
        public ActionResult GetPendingChallanViewData(string CircleID, string DistrictID, int noOfDays = 0)
        {
            List<InvoiceCumChallan> objChallanList = new List<InvoiceCumChallan>();
            int NoOfDaysToConsiderForPendingChallan = default(int);
            try
            {
                if (noOfDays > default(int))
                {
                    NoOfDaysToConsiderForPendingChallan = noOfDays;
                }
                else
                {
                    if (ConfigurationManager.AppSettings["NoOfDaysToConsiderForPendingChallan"] != null)
                        int.TryParse(ConfigurationManager.AppSettings["NoOfDaysToConsiderForPendingChallan"], out NoOfDaysToConsiderForPendingChallan);
                    else
                        NoOfDaysToConsiderForPendingChallan = 10;
                }

                DataTable dt = objDbTrx.GetPendingChallanDtlBasedOnDayDiff(NoOfDaysToConsiderForPendingChallan, CircleID, DistrictID);
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        InvoiceCumChallan icc = new InvoiceCumChallan();
                        icc.ChallanId = Convert.ToInt64(dt.Rows[iCnt]["ID"].ToString());
                        icc.InvoiceCumChallanNo = Convert.ToString(dt.Rows[iCnt]["Challan_Number"].ToString());
                        icc.CircleId = Convert.ToInt32(dt.Rows[iCnt]["CircleId"].ToString());
                        icc.InvoiceCumChallanDate = Convert.ToDateTime(dt.Rows[iCnt]["Challan_Date"].ToString()).ToString("dd-MMM-yyyy");
                        icc.CircleName = Convert.ToString(dt.Rows[iCnt]["Circle_Name"].ToString());
                        icc.InspectorName = Convert.ToString(dt.Rows[iCnt]["Circle_Officer_Name"].ToString());
                        icc.InspectorPhoneNo = Convert.ToString(dt.Rows[iCnt]["Mobile_No"].ToString());
                        icc.InspectorEmailId = Convert.ToString(dt.Rows[iCnt]["Email_Id"].ToString());
                        icc.ConfirmStatus = Convert.ToInt32(string.IsNullOrWhiteSpace(dt.Rows[iCnt]["ConfirmStatus"].ToString()) ? "0" : dt.Rows[iCnt]["ConfirmStatus"].ToString());
                        icc.CategoryName = Convert.ToString(dt.Rows[iCnt]["CHALLAN_BOOK_CATEGORY"].ToString());
                        icc.DistrictName = dt.Rows[iCnt]["DISTRICT"].ToString();
                        icc.Language = Convert.ToString(dt.Rows[iCnt]["LANGUAGE"].ToString());
                        icc.Transporter = Convert.ToString(dt.Rows[iCnt]["Transport_Name"].ToString());
                        icc.CONSIGNEE_NO = Convert.ToString(dt.Rows[iCnt]["CONSIGNEE_NO"].ToString());
                        icc.VEHICLE_NO = Convert.ToString(dt.Rows[iCnt]["VEHICLE_NO"].ToString());
                        icc.UpdatedBy = Convert.ToString(dt.Rows[iCnt]["UPDATED_BY"].ToString());
                        icc.UpdatedTimeStamp = Convert.ToDateTime(dt.Rows[iCnt]["UPDATED_TS"].ToString()).ToString("dd-MMM-yyyy");
                        icc.ConfirmStatus = Convert.ToInt32(dt.Rows[iCnt]["ConfirmStatus"].ToString());
                        icc.DayDifference = Convert.ToInt32(dt.Rows[iCnt]["DayDifference"].ToString());

                        objChallanList.Add(icc);
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return PartialView("~/Views/InvoiceCumChallanPending/_PendingChallanDetailList.cshtml", objChallanList);
        }

        [HttpPost]
        [ActionName("GetChallanViewData")]
        public ActionResult GetChallanViewData(string startDate, string endDate, string CircleID, string DistrictID, string challanNumber = "", bool isPartialViewRequested = false, bool isFinalRequest = false)
        {
            List<InvoiceCumChallan> objChallanList = new List<InvoiceCumChallan>();
            try
            {
                DataTable dt = isFinalRequest ? objDbTrx.GetChallanDtlModified(Convert.ToDateTime(startDate + " 00:00:00.000"), Convert.ToDateTime(endDate + " 23:59:59.999"), CircleID, DistrictID, challanNumber, 2) : objDbTrx.GetChallanDtlModified(Convert.ToDateTime(startDate + " 00:00:00.000"), Convert.ToDateTime(endDate + " 23:59:59.999"), CircleID, DistrictID, challanNumber);
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        InvoiceCumChallan icc = new InvoiceCumChallan();
                        icc.ChallanId = Convert.ToInt64(dt.Rows[iCnt]["ID"].ToString());
                        icc.InvoiceCumChallanNo = Convert.ToString(dt.Rows[iCnt]["Challan_Number"].ToString());
                        icc.CircleId = Convert.ToInt32(dt.Rows[iCnt]["CircleId"].ToString());
                        icc.InvoiceCumChallanDate = Convert.ToDateTime(dt.Rows[iCnt]["Challan_Date"].ToString()).ToString("dd-MMM-yyyy");
                        icc.CircleName = Convert.ToString(dt.Rows[iCnt]["Circle_Name"].ToString());
                        icc.InspectorName = Convert.ToString(dt.Rows[iCnt]["Circle_Officer_Name"].ToString());
                        icc.InspectorPhoneNo = Convert.ToString(dt.Rows[iCnt]["Mobile_No"].ToString());
                        icc.InspectorEmailId = Convert.ToString(dt.Rows[iCnt]["Email_Id"].ToString());
                        icc.ConfirmStatus = Convert.ToInt32(string.IsNullOrWhiteSpace(dt.Rows[iCnt]["ConfirmStatus"].ToString()) ? "0" : dt.Rows[iCnt]["ConfirmStatus"].ToString());
                        icc.CategoryName = Convert.ToString(dt.Rows[iCnt]["CHALLAN_BOOK_CATEGORY"].ToString());
                        icc.DistrictName = dt.Rows[iCnt]["DISTRICT"].ToString();
                        icc.Language = Convert.ToString(dt.Rows[iCnt]["LANGUAGE"].ToString());
                        icc.Transporter = Convert.ToString(dt.Rows[iCnt]["Transport_Name"].ToString());
                        icc.CONSIGNEE_NO = Convert.ToString(dt.Rows[iCnt]["CONSIGNEE_NO"].ToString());
                        icc.VEHICLE_NO = Convert.ToString(dt.Rows[iCnt]["VEHICLE_NO"].ToString());

                        icc.UpdatedBy = Convert.ToString(dt.Rows[iCnt]["UPDATED_BY"].ToString());
                        icc.UpdatedTimeStamp = Convert.ToDateTime(dt.Rows[iCnt]["UPDATED_TS"].ToString()).ToString("dd-MMM-yyyy");
                        icc.ConfirmStatus = Convert.ToInt32(dt.Rows[iCnt]["ConfirmStatus"].ToString());

                        objChallanList.Add(icc);
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            if (isPartialViewRequested)
                return PartialView("~/Views/InvoiceCumChallan/_ChallanDetailList.cshtml", objChallanList);
            else
            {
                var jsonResult = Json(objChallanList, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
        }

        [HttpPost]
        [ActionName("UnconfirmCreateInvoice")]
        public JsonResult UnconfirmCreateInvoice(string pData)
        {
            bool result = default(bool);
            try
            {
                var userSessionObject = Session["UserSec"] != null ? ((UserSec)Session["UserSec"]) : new UserSec();
                if (string.IsNullOrEmpty(userSessionObject.UserId))
                {
                    throw new Exception("Session timed out. Please login again.");
                }
                var unescapedData = HttpUtility.UrlDecode(pData);

                var objects = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<ChallanConfirmCommunication>>(unescapedData);
                string[] pDataDefined = objects.Select(obj => obj.ChallanId.ToString()).ToArray();
                string fullstr = string.Join(",", pDataDefined).Replace('"', ' ').Trim().Replace(" ", "");
                if (string.IsNullOrWhiteSpace(fullstr))
                {
                    throw new Exception("No item selected");
                }
                result = objDbTrx.UpdateChallanRevertById(fullstr, userSessionObject.UserId);

                if (result)
                    return Json(new { Success = 1, Message = "Selected challan number(s) have been reverted successfully" }, JsonRequestBehavior.AllowGet);
                else
                    throw new Exception("Challan revert process failed");
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
                return Json(new { Success = 0, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ActionName("ConfirmCreateInvoice")]
        public JsonResult ConfirmCreateInvoice(string pData)
        {
            bool result = default(bool);
            try
            {
                var userSessionObject = Session["UserSec"] != null ? ((UserSec)Session["UserSec"]) : new UserSec();
                if (string.IsNullOrEmpty(userSessionObject.UserId))
                {
                    throw new Exception("Session timed out. Please login again.");
                }

                var unescapedData = HttpUtility.UrlDecode(pData);

                var objects = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<ChallanConfirmCommunication>>(unescapedData);
                string[] pDataDefined = objects.Select(obj => obj.ChallanId.ToString()).ToArray();
                string fullstr = string.Join(",", pDataDefined).Replace('"', ' ').Trim().Replace(" ", "");
                if (string.IsNullOrWhiteSpace(fullstr))
                {
                    throw new Exception("No item selected");
                }

                //for update confirmed challan list
                result = objDbTrx.UpdateChallanConfirmById(fullstr, userSessionObject.UserId);

                if (result)
                {
                    try
                    {
                        //to fetch list of same confirmed challan to send email and sms one by one
                        DataTable dtbl = objDbTrx.GetConfirmedChallanInfoById(fullstr);
                        var resFin = SendEmailAndSmsInOneShot(fullstr);
                        //System.Threading.ThreadPool.QueueUserWorkItem(s =>
                        //{
                        //    CommunicateViaEmailSms(dtbl);
                        //});
                    }
                    catch { }

                    return Json(new { Success = 1, Message = "Selected challan numbers have been confirmed successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                    throw new Exception("Challan confirmation failed");
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
                return Json(new { Success = 0, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [NonAction]
        private void CommunicateViaEmailSms(DataTable dtbl)
        {
            string smsBody = string.Empty;
            try
            {
                if (dtbl != null && dtbl.Rows.Count > default(int))
                {
                    for (int iCnt = 0; iCnt < dtbl.Rows.Count; iCnt++)
                    {
                        //smsBody = string.Format("Dear {0}, challan no: {1} to be delivered soon. Your secret code is {2}. Please share the code to the transporter by request.",
                        //    Convert.ToString(dtbl.Rows[iCnt]["CIRCLE_OFFICER_NAME"].ToString()),
                        //    Convert.ToString(dtbl.Rows[iCnt]["CHALLAN_NUMBER"].ToString()),
                        //    Convert.ToString(dtbl.Rows[iCnt]["SMSCode"].ToString())
                        //    );
                        //Dear~challan no~to be delivered soon. Unique code is~Please share the code to transporter at the time of receiving of~
                        smsBody = string.Format("Dear Sir challan no {0} to be delivered soon. Unique code is {1} Please share the code to transporter at the time of receiving of {2}",
                            Convert.ToString(dtbl.Rows[iCnt]["CHALLAN_NUMBER"].ToString()),
                            Convert.ToString(dtbl.Rows[iCnt]["SMSCode"].ToString()),
                            "Book"
                            );
                        Utility.SendSMS(Convert.ToString(dtbl.Rows[iCnt]["MOBILE_NO"].ToString()), smsBody);
                        Utility.SendSMS(Convert.ToString(dtbl.Rows[iCnt]["ALTERNATE_MOBILE_NO"].ToString()), smsBody);
                        //Utility.SendSMS("9748084241", smsBody);
                    }
                    Utility.SendChallanEmail(dtbl);
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
        }

        public ActionResult ChallanDetails()
        {
            ViewBag.Active = "InvoiceCumChallanReqDetails";
            return View("~/Views/InvoiceCumChallan/ChallanFinal.cshtml", new InvoiceCumChallan());
        }

        public ActionResult PartialChallanRevart()
        {
            ViewBag.Active = "InvoiceCumChallanReqRevertPartial";
            return View("~/Views/InvoiceCumChallan/PartialChallanRevert.cshtml", new InvoiceCumChallan());
        }

        public ActionResult ChallanNotRcvdAtCircle()
        {
            ViewBag.Active = "InvoiceCumChallanReqDetailsNotRcvd";
            return View("~/Views/InvoiceCumChallanPending/ChallanNotRcvdAtCrcl.cshtml", new InvoiceCumChallan());
        }

        [HttpPost]
        public JsonResult SMSSend(string griddata)
        {
            string ErrorMsg = SmsSendCode(griddata);
            return Json(ErrorMsg);
        }

        [HttpPost]
        public JsonResult SendEmail(string griddata)
        {
            string ErrorMsg = EmailSendCode(griddata);
            return Json(ErrorMsg);
        }

        [HttpPost]
        public JsonResult PendingListSMSSend(string griddata)
        {
            string ErrorMsg = PendingListSmsSendCode(griddata);
            return Json(ErrorMsg);
        }

        [HttpPost]
        public JsonResult PendingListSendEmail(string griddata)
        {
            string ErrorMsg = PendingListEmailSendCode(griddata);
            return Json(ErrorMsg);
        }

        [NonAction]
        private string PendingListSmsSendCode(string griddata)
        {
            string Msg = "";
            string ErrorMsg = "";
            string[] ChallanIds = griddata.Split(',');
            string challanId = "";
            WebClient client = new WebClient();
            try
            {
                string SmsUserID = System.Web.Configuration.WebConfigurationManager.AppSettings["SmsUserID"].ToString();
                string SmsPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["SmsPassword"].ToString();
                string SmsSenderName = System.Web.Configuration.WebConfigurationManager.AppSettings["SmsSenderName"].ToString();
                string TestSms = System.Web.Configuration.WebConfigurationManager.AppSettings["TestSms"].ToString();
                string TestMobileNo = System.Web.Configuration.WebConfigurationManager.AppSettings["TestMobileNo"].ToString();
                string MobileNo = TestMobileNo;
                string TextMsg = "";
                string baseurl = "";
                ErrorMsg = "";
                System.Threading.ThreadPool.QueueUserWorkItem(s =>
                {
                    for (int i = 0; i < ChallanIds.Count(); i++)
                    {
                        try
                        {
                            challanId = Convert.ToInt32(ChallanIds[i]).ToString();
                            DataTable dtChallanDtl = objDbTrx.GetChallanDetailsById(Convert.ToInt64(challanId));
                            if (dtChallanDtl.Rows.Count > 0)
                            {
                                if (TestSms.ToUpper() == "FALSE")
                                {
                                    MobileNo = dtChallanDtl.Rows[0]["MOBILE_NO"].ToString();
                                }

                                #region [Changed Code]
                                if (dtChallanDtl.Rows[0]["ConfirmStatus"].ToString() == "1")
                                {
                                    string smsBody = string.Format("Dear Sir challan no {0} to be delivered soon. Unique code is {1} Please share the code to transporter at the time of receiving of {2}",
                                    Convert.ToString(dtChallanDtl.Rows[0]["CHALLAN_NUMBER"].ToString()),
                                    Convert.ToString(dtChallanDtl.Rows[0]["SMSCode"].ToString()),
                                    "Book");
                                    Utility.SendSMS(Convert.ToString(dtChallanDtl.Rows[0]["MOBILE_NO"].ToString()), smsBody);
                                    Utility.SendSMS(Convert.ToString(dtChallanDtl.Rows[0]["ALTERNATE_MOBILE_NO"].ToString()), smsBody);
                                }
                                #endregion [Changed Code]
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorMsg += ErrorMsg + " " + ex;
                        }
                    }
                });
                if (ErrorMsg == "")
                {
                    ErrorMsg = "SMS sent sucsscessfully.....";
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = "Some error occured while sending mail " + ex.Message;
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }

            return ErrorMsg;
        }

        [NonAction]
        private string PendingListEmailSendCode(string griddata)
        {
            string Msg = "";
            string ErrorMsg = "";
            string MailSubject = "";
            StringBuilder strMessage = new StringBuilder();
            string[] ChallanIds = griddata.Split(',');
            string challanId = "";
            try
            {
                string GmailUserNameKey = System.Web.Configuration.WebConfigurationManager.AppSettings["GmailUserNameKey"].ToString();
                string GmailPasswordKey = System.Web.Configuration.WebConfigurationManager.AppSettings["GmailPasswordKey"].ToString();
                string GmailHostKey = System.Web.Configuration.WebConfigurationManager.AppSettings["GmailHostKey"].ToString();
                string GmailPortKey = System.Web.Configuration.WebConfigurationManager.AppSettings["GmailPortKey"].ToString();
                string GmailSslKey = System.Web.Configuration.WebConfigurationManager.AppSettings["GmailSslKey"].ToString();

                string TestEmail = System.Web.Configuration.WebConfigurationManager.AppSettings["TestEmail"].ToString();
                string EmailIdTo = System.Web.Configuration.WebConfigurationManager.AppSettings["TestEmailId"].ToString();

                SmtpClient smtp = new SmtpClient();
                smtp.Host = GmailHostKey;
                smtp.Port = Convert.ToInt16(GmailPortKey);
                smtp.EnableSsl = Convert.ToBoolean(GmailSslKey);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(GmailUserNameKey, GmailPasswordKey);

                System.Threading.ThreadPool.QueueUserWorkItem(s =>
                {
                    for (int i = 0; i < ChallanIds.Count(); i++)
                    {
                        try
                        {
                            challanId = Convert.ToInt32(ChallanIds[i]).ToString();
                            DataTable dtChallanDtl = objDbTrx.GetChallanPrintDetailsById(Convert.ToInt64(challanId));
                            if (dtChallanDtl.Rows.Count > 0)
                            {
                                if (TestEmail.ToUpper() == "FALSE")
                                {
                                    EmailIdTo = dtChallanDtl.Rows[0]["EMAIL_ID"].ToString();
                                }
                                if (dtChallanDtl.Rows[0]["ConfirmStatus"].ToString() == "1")
                                {
                                    strMessage = new StringBuilder();
                                    MailSubject = "Chargeble Challan no.: " + dtChallanDtl.Rows[0]["CHALLAN_NUMBER"].ToString() + "  Date : " + Convert.ToDateTime(dtChallanDtl.Rows[0]["CHALLAN_DATE"]).ToString("dd-MMM-yyyy");
                                    strMessage.AppendLine(" <table id='InvoiceCumChallanReport' cellpadding='0' cellspacing='0' border='1' width='100%'>");
                                    strMessage.AppendLine("<tr><td colspan='4'><b>PRINTING & DELIVERY OF 'NTB:</b></td>");
                                    strMessage.AppendLine("<td colspan='3'><b>Unique Code for Delivery: <span style='color: #ff0000;'>" + dtChallanDtl.Rows[0]["SMSCode"].ToString() + "</span></b></td></tr>");
                                    strMessage.AppendLine("<tr>");
                                    strMessage.AppendLine("     <td colspan='3'>Chargeble Challan no.: <b>" + dtChallanDtl.Rows[0]["CHALLAN_NUMBER"].ToString() + "</b></td>");
                                    strMessage.AppendLine("     <td colspan='4' style='text-align:left;'><b>Date : " + Convert.ToDateTime(dtChallanDtl.Rows[0]["CHALLAN_DATE"]).ToString("dd-MMM-yyyy") + "</b></td>");
                                    strMessage.AppendLine("</tr>");
                                    strMessage.AppendLine("<tr>");
                                    strMessage.AppendLine("   <td style='text-align:left;font-size:small;'>Transporter Name</td>");
                                    strMessage.AppendLine("   <td style='text-align:left;font-size:small;'>" + dtChallanDtl.Rows[0]["Transport_Name"].ToString() + "</td>");
                                    strMessage.AppendLine("   <td style='text-align:left;font-size:small;'>Consignment No.</td>");
                                    strMessage.AppendLine("   <td style='text-align:left;font-size:small;'>" + dtChallanDtl.Rows[0]["CONSIGNEE_NO"].ToString() + "</td>");
                                    strMessage.AppendLine("   <td style='text-align:left;font-size:small;'>Truck No.</td>");
                                    strMessage.AppendLine("   <td style='text-align:left;font-size:small;' colspan='2'>" + dtChallanDtl.Rows[0]["VEHICLE_NO"].ToString() + "</td>");
                                    strMessage.AppendLine("</tr>");
                                    strMessage.AppendLine("<tr>");
                                    strMessage.AppendLine("   <td style='vertical-align:bottom;text-align:left;font-size:small;width:18%;'><b>Class</b></td>");
                                    strMessage.AppendLine("   <td style='vertical-align:bottom;text-align:left;font-size:small;width:18%;'><b>Book Code</b></td>");
                                    strMessage.AppendLine("   <td style='vertical-align:bottom;text-align:left;font-size:small;width:18%;'><b>Name of the Books/Forms</b></td>");
                                    strMessage.AppendLine("   <td style='vertical-align:bottom;text-align:left;font-size:small;width:18%;'><b>Quantity Delivered</b></td>");
                                    strMessage.AppendLine("   <td style='vertical-align:bottom;text-align:left;font-size:small;width:18%;'><b>Unit</b></td>");
                                    strMessage.AppendLine("   <td style='vertical-align:bottom;text-align:left;font-size:small;width:18%;'><b>No. of Box <br />bundle/ cartoon/ pack</b></td>");
                                    strMessage.AppendLine("   <td style='vertical-align:bottom;text-align:left;font-size:small;width:18%;'><b>Remarks (if any)</b></td>");
                                    strMessage.AppendLine("</tr>");
                                    for (int iCnt = 0; iCnt < dtChallanDtl.Rows.Count; iCnt++)
                                    {
                                        strMessage.AppendLine("<tr>");
                                        strMessage.AppendLine("   <td style='text-align:left;'>" + dtChallanDtl.Rows[iCnt]["CLASS"].ToString() + "</td>");
                                        strMessage.AppendLine("   <td style='text-align:left;'>" + dtChallanDtl.Rows[iCnt]["BOOK_CODE"].ToString() + "</td>");
                                        strMessage.AppendLine("   <td style='text-align:left;'>" + dtChallanDtl.Rows[iCnt]["BOOK_NAME"].ToString() + "</td>");
                                        strMessage.AppendLine("   <td style='text-align:left;'>" + dtChallanDtl.Rows[iCnt]["QtyShippedQty"].ToString() + "</td>");
                                        strMessage.AppendLine("   <td style='text-align:left;'>Copies</td>");
                                        strMessage.AppendLine("   <td style='text-align:left;'>" + dtChallanDtl.Rows[iCnt]["Cartoon"].ToString() + "</td>");
                                        strMessage.AppendLine("   <td style='text-align:left;'>" + dtChallanDtl.Rows[iCnt]["Remarks"].ToString() + "</td>");
                                        strMessage.AppendLine("</tr>");

                                    }
                                    strMessage.AppendLine("</table>");

                                    using (var message = new MailMessage(GmailUserNameKey, EmailIdTo))
                                    {
                                        message.Subject = MailSubject;
                                        message.Body = strMessage.ToString();
                                        message.IsBodyHtml = true;
                                        smtp.Send(message);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorMsg += ErrorMsg + " " + ex;
                        }
                    }
                });
                if (ErrorMsg == "")
                {
                    ErrorMsg = "email sent sucsscessfully.....";
                }


            }
            catch (Exception ex)
            {
                ErrorMsg = "Some error occoured while sending email. " + ex.Message + ". " + ex.InnerException;
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return ErrorMsg;
        }

        [NonAction]
        private string SmsSendCode(string griddata)
        {
            string Msg = "";
            string ErrorMsg = "";
            string[] ChallanIds = griddata.Split(',');
            string challanId = "";
            WebClient client = new WebClient();
            try
            {
                string SmsUserID = System.Web.Configuration.WebConfigurationManager.AppSettings["SmsUserID"].ToString();
                string SmsPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["SmsPassword"].ToString();
                string SmsSenderName = System.Web.Configuration.WebConfigurationManager.AppSettings["SmsSenderName"].ToString();
                string TestSms = System.Web.Configuration.WebConfigurationManager.AppSettings["TestSms"].ToString();
                string TestMobileNo = System.Web.Configuration.WebConfigurationManager.AppSettings["TestMobileNo"].ToString();
                string MobileNo = TestMobileNo;
                string TextMsg = "";
                string baseurl = "";
                ErrorMsg = "";
                System.Threading.ThreadPool.QueueUserWorkItem(s =>
                {
                    for (int i = 0; i < ChallanIds.Count(); i++)
                    {
                        try
                        {
                            challanId = Convert.ToInt32(ChallanIds[i]).ToString();
                            DataTable dtChallanDtl = objDbTrx.GetChallanDetailsById(Convert.ToInt64(challanId));
                            if (dtChallanDtl.Rows.Count > 0)
                            {
                                if (TestSms.ToUpper() == "FALSE")
                                {
                                    MobileNo = dtChallanDtl.Rows[0]["MOBILE_NO"].ToString();
                                }

                                #region [Changed Code]
                                if (dtChallanDtl.Rows[0]["ConfirmStatus"].ToString() == "1")
                                {
                                    string smsBody = string.Format("Dear Sir challan no {0} to be delivered soon. Unique code is {1} Please share the code to transporter at the time of receiving of {2}",
                                    Convert.ToString(dtChallanDtl.Rows[0]["CHALLAN_NUMBER"].ToString()),
                                    Convert.ToString(dtChallanDtl.Rows[0]["SMSCode"].ToString()),
                                    "Book");
                                    Utility.SendSMS(Convert.ToString(dtChallanDtl.Rows[0]["MOBILE_NO"].ToString()), smsBody);
                                    Utility.SendSMS(Convert.ToString(dtChallanDtl.Rows[0]["ALTERNATE_MOBILE_NO"].ToString()), smsBody);
                                }
                                #endregion [Changed Code]
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorMsg += ErrorMsg + " " + ex;
                        }
                    }
                });
                if (ErrorMsg == "")
                {
                    ErrorMsg = "SMS sent sucsscessfully.....";
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = "Some error occured while sending mail " + ex.Message;
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }

            return ErrorMsg;
        }

        [NonAction]
        private string EmailSendCode(string griddata)
        {
            string Msg = "";
            string ErrorMsg = "";
            string MailSubject = "";
            StringBuilder strMessage = new StringBuilder();
            string[] ChallanIds = griddata.Split(',');
            string challanId = "";
            try
            {
                string GmailUserNameKey = System.Web.Configuration.WebConfigurationManager.AppSettings["GmailUserNameKey"].ToString();
                string GmailPasswordKey = System.Web.Configuration.WebConfigurationManager.AppSettings["GmailPasswordKey"].ToString();
                string GmailHostKey = System.Web.Configuration.WebConfigurationManager.AppSettings["GmailHostKey"].ToString();
                string GmailPortKey = System.Web.Configuration.WebConfigurationManager.AppSettings["GmailPortKey"].ToString();
                string GmailSslKey = System.Web.Configuration.WebConfigurationManager.AppSettings["GmailSslKey"].ToString();

                string TestEmail = System.Web.Configuration.WebConfigurationManager.AppSettings["TestEmail"].ToString();
                string EmailIdTo = System.Web.Configuration.WebConfigurationManager.AppSettings["TestEmailId"].ToString();

                SmtpClient smtp = new SmtpClient();
                smtp.Host = GmailHostKey;
                smtp.Port = Convert.ToInt16(GmailPortKey);
                smtp.EnableSsl = Convert.ToBoolean(GmailSslKey);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(GmailUserNameKey, GmailPasswordKey);

                System.Threading.ThreadPool.QueueUserWorkItem(s =>
                {
                    for (int i = 0; i < ChallanIds.Count(); i++)
                    {
                        try
                        {
                            challanId = Convert.ToInt32(ChallanIds[i]).ToString();
                            DataTable dtChallanDtl = objDbTrx.GetChallanPrintDetailsById(Convert.ToInt64(challanId));
                            if (dtChallanDtl.Rows.Count > 0)
                            {
                                if (TestEmail.ToUpper() == "FALSE")
                                {
                                    EmailIdTo = dtChallanDtl.Rows[0]["EMAIL_ID"].ToString();
                                }
                                if (dtChallanDtl.Rows[0]["ConfirmStatus"].ToString() == "1")
                                {
                                    strMessage = new StringBuilder();
                                    MailSubject = "Chargeble Challan no.: " + dtChallanDtl.Rows[0]["CHALLAN_NUMBER"].ToString() + "  Date : " + Convert.ToDateTime(dtChallanDtl.Rows[0]["CHALLAN_DATE"]).ToString("dd-MMM-yyyy");
                                    strMessage.AppendLine(" <table id='InvoiceCumChallanReport' cellpadding='0' cellspacing='0' border='1' width='100%'>");
                                    strMessage.AppendLine("<tr><td colspan='4'><b>PRINTING & DELIVERY OF 'NTB:</b></td>");
                                    strMessage.AppendLine("<td colspan='3'><b>Unique Code for Delivery: <span style='color: #ff0000;'>" + dtChallanDtl.Rows[0]["SMSCode"].ToString() + "</span></b></td></tr>");
                                    strMessage.AppendLine("<tr>");
                                    strMessage.AppendLine("     <td colspan='3'>Chargeble Challan no.: <b>" + dtChallanDtl.Rows[0]["CHALLAN_NUMBER"].ToString() + "</b></td>");
                                    strMessage.AppendLine("     <td colspan='4' style='text-align:left;'><b>Date : " + Convert.ToDateTime(dtChallanDtl.Rows[0]["CHALLAN_DATE"]).ToString("dd-MMM-yyyy") + "</b></td>");
                                    strMessage.AppendLine("</tr>");
                                    strMessage.AppendLine("<tr>");
                                    strMessage.AppendLine("   <td style='text-align:left;font-size:small;'>Transporter Name</td>");
                                    strMessage.AppendLine("   <td style='text-align:left;font-size:small;'>" + dtChallanDtl.Rows[0]["Transport_Name"].ToString() + "</td>");
                                    strMessage.AppendLine("   <td style='text-align:left;font-size:small;'>Consignment No.</td>");
                                    strMessage.AppendLine("   <td style='text-align:left;font-size:small;'>" + dtChallanDtl.Rows[0]["CONSIGNEE_NO"].ToString() + "</td>");
                                    strMessage.AppendLine("   <td style='text-align:left;font-size:small;'>Truck No.</td>");
                                    strMessage.AppendLine("   <td style='text-align:left;font-size:small;' colspan='2'>" + dtChallanDtl.Rows[0]["VEHICLE_NO"].ToString() + "</td>");
                                    strMessage.AppendLine("</tr>");
                                    strMessage.AppendLine("<tr>");
                                    strMessage.AppendLine("   <td style='vertical-align:bottom;text-align:left;font-size:small;width:18%;'><b>Class</b></td>");
                                    strMessage.AppendLine("   <td style='vertical-align:bottom;text-align:left;font-size:small;width:18%;'><b>Book Code</b></td>");
                                    strMessage.AppendLine("   <td style='vertical-align:bottom;text-align:left;font-size:small;width:18%;'><b>Name of the Books/Forms</b></td>");
                                    strMessage.AppendLine("   <td style='vertical-align:bottom;text-align:left;font-size:small;width:18%;'><b>Quantity Delivered</b></td>");
                                    strMessage.AppendLine("   <td style='vertical-align:bottom;text-align:left;font-size:small;width:18%;'><b>Unit</b></td>");
                                    strMessage.AppendLine("   <td style='vertical-align:bottom;text-align:left;font-size:small;width:18%;'><b>No. of Box <br />bundle/ cartoon/ pack</b></td>");
                                    strMessage.AppendLine("   <td style='vertical-align:bottom;text-align:left;font-size:small;width:18%;'><b>Remarks (if any)</b></td>");
                                    strMessage.AppendLine("</tr>");
                                    for (int iCnt = 0; iCnt < dtChallanDtl.Rows.Count; iCnt++)
                                    {
                                        strMessage.AppendLine("<tr>");
                                        strMessage.AppendLine("   <td style='text-align:left;'>" + dtChallanDtl.Rows[iCnt]["CLASS"].ToString() + "</td>");
                                        strMessage.AppendLine("   <td style='text-align:left;'>" + dtChallanDtl.Rows[iCnt]["BOOK_CODE"].ToString() + "</td>");
                                        strMessage.AppendLine("   <td style='text-align:left;'>" + dtChallanDtl.Rows[iCnt]["BOOK_NAME"].ToString() + "</td>");
                                        strMessage.AppendLine("   <td style='text-align:left;'>" + dtChallanDtl.Rows[iCnt]["QtyShippedQty"].ToString() + "</td>");
                                        strMessage.AppendLine("   <td style='text-align:left;'>Copies</td>");
                                        strMessage.AppendLine("   <td style='text-align:left;'>" + dtChallanDtl.Rows[iCnt]["Cartoon"].ToString() + "</td>");
                                        strMessage.AppendLine("   <td style='text-align:left;'>" + dtChallanDtl.Rows[iCnt]["Remarks"].ToString() + "</td>");
                                        strMessage.AppendLine("</tr>");

                                    }
                                    strMessage.AppendLine("</table>");

                                    using (var message = new MailMessage(GmailUserNameKey, EmailIdTo))
                                    {
                                        message.Subject = MailSubject;
                                        message.Body = strMessage.ToString();
                                        message.IsBodyHtml = true;
                                        smtp.Send(message);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorMsg += ErrorMsg + " " + ex;
                        }
                    }
                });
                if (ErrorMsg == "")
                {
                    ErrorMsg = "email sent sucsscessfully.....";
                }


            }
            catch (Exception ex)
            {
                ErrorMsg = "Some error occoured while sending email. " + ex.Message + ". " + ex.InnerException;
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return ErrorMsg;
        }

        [NonAction]
        private bool SendEmailAndSmsInOneShot(string griddata)
        {
            var smsResult = SmsSendCode(griddata);
            var emailResult = EmailSendCode(griddata);
            return true;
        }
    }
}
