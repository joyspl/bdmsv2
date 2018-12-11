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


namespace SARASWATIPRESSNEW.Controllers
{
    public class InvoiceCumChallanController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            ViewBag.Active = "ChallanLootWiseScan";
            return View(get_Data());
        }

        [HttpGet]
        public Models.InvoiceCumChallan get_Data()
        {
            InvoiceCumChallan lst_invCumChal = new InvoiceCumChallan();
            try
            {
                lst_invCumChal.InvoiceCumChallanDate = DateTime.Now.ToString("dd-MMM-yyyy");
                lst_invCumChal.InvoiceCumChallanNo = "TBC" + (DateTime.Now.Month >= 4 ? DateTime.Now.Year.ToString().Substring(2) + "-" + (Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2)) + 1) + "-XXXXXXX" : (Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2)) - 1) + "-" + DateTime.Now.Year.ToString().Substring(2) + "-XXXXXXX").ToString();
                lst_invCumChal.ChallanId = 0;
                if (Convert.ToString(Session["ChallanId"]) != "")
                {
                    lst_invCumChal.ChallanId = Convert.ToInt64(Session["ChallanId"]);
                    Session["ChallanId"] = "";
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
            finally { }
            return lst_invCumChal;
        }

        [HttpPost]
        [ActionName("SaveAndConfirmChallan")]
        public ActionResult SaveAndConfirmChallan(InvoiceCumChallan objInvoiceCumChallan,string  barcodeList,string  duplicatebarcodeList, bool isDraft = false)
        {
            string ChallanNo = string.Empty;
            try
            {
                var userSessionObject = Session["UserSec"] != null ? ((UserSec)Session["UserSec"]) : new UserSec();
                objInvoiceCumChallan.UserId = userSessionObject.UserId;
                objInvoiceCumChallan.InvoiceCumChallanDate = DateTime.Now.ToString();

                if (objInvoiceCumChallan.InvoiceCumChallanCollection != null && objInvoiceCumChallan.InvoiceCumChallanCollection.Count() > default(int))
                {
                    foreach (var ic in objInvoiceCumChallan.InvoiceCumChallanCollection)
                    {
                        if (ic.TotalLot > default(int) && !string.IsNullOrWhiteSpace(ic.TotalLotDelimited))
                        {
                            List<int> TagIds = new List<int>();
                            try
                            {
                                TagIds = ic.TotalLotDelimited.Split(',').Select(int.Parse).ToList();
                                if (TagIds != null && TagIds.Count() > default(int))
                                {
                                    StringBuilder sb = new StringBuilder();
                                    foreach (var tg in TagIds)
                                    {
                                        sb.Append(string.Format("{0} pack, {1} book(s); ", (ic.QtyShipped > 0 ? (ic.QtyShipped > tg ? (ic.QtyShipped / tg) : 1) : 0), tg.ToString()));
                                    }
                                    ic.Cartoon = string.Format("{0} in each pack", sb.ToString().Remove(sb.ToString().LastIndexOf(";")));
                                    //ic.Cartoon = string.Format("{0} pack, {1} Books in each pack", (ic.QtyShipped > 0 ? (ic.QtyShipped > ic.TotalLot ? (ic.QtyShipped / ic.TotalLot) : 1) : 0), ic.TotalLot.ToString());
                                }
                            }
                            catch (Exception)
                            {
                                ic.Cartoon = "";
                            }
                        }
                        else
                        {
                            ic.Cartoon = "";
                        }
                    }
                }


                if (Convert.ToInt64(objInvoiceCumChallan.ChallanId) != 0)
                {
                    objDbTrx.UpdateInChallan(objInvoiceCumChallan);
                    //objDbTrx.UpdateInChallanNew(objInvoiceCumChallan,barcodeList,duplicatebarcodeList, isDraft);
                }

                else
                {
                    objDbTrx.InsertInChallan(objInvoiceCumChallan, out ChallanNo);
                    //objDbTrx.InsertInChallanNew(objInvoiceCumChallan, barcodeList, duplicatebarcodeList, out ChallanNo);
                }

                //return RedirectToAction("Index", "InvoiceCumChallanReqList");
                return Json(new { Success = 1, Message = string.Format("Challan {0} successfully", string.IsNullOrEmpty(ChallanNo) ? "updated" : "inserted") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
                return Json(new { Success = 0, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Index(InvoiceCumChallan objInvCumChal)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string ChallanNo = "";
                    if (Convert.ToString(Session["sp_user_name"]) != "")
                    {
                        if (Convert.ToInt64(objInvCumChal.ChallanId) != 0)
                        {

                            objInvCumChal.UserId = Convert.ToString(Session["sp_user_name"]);
                            objDbTrx.UpdateInChallan(objInvCumChal);

                        }
                        else
                        {
                            objInvCumChal.UserId = Convert.ToString(Session["sp_user_name"]);
                            objDbTrx.InsertInChallan(objInvCumChal, out ChallanNo);

                        }

                        return RedirectToAction("Index", "InvoiceCumChallanReqList");
                    }
                    else
                    {
                        RedirectToAction("Index", "SPLogin");
                    }
                }
                catch (Exception ex)
                {
                    objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
                }
                finally
                {

                }
            }
            return RedirectToAction("Index", "InvoiceCumChallan");
        }

        [HttpPost]
        public ActionResult SaveAddress(string CircleId, string SchooldAddress, string Pincode, string InspectorName, string InspectorPhoneNo, string InspectorEmailId)
        {
            try
            {
                objDbTrx.UpdateCircleMasterAddressNew(CircleId, SchooldAddress, Pincode, InspectorName, InspectorPhoneNo, InspectorEmailId);
                //objDbTrx.UpdateCircleUserMasterAddress(CircleId, SchooldAddress, Pincode, InspectorName, InspectorPhoneNo, InspectorEmailId);
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json("saved");
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
        public JsonResult GetChallanLanguageDetails()
        {
            try
            {
                List<Language> lst_Language = new List<Language>();
                DataTable dtMast = objDbTrx.GetLanguageMasterDetails();
                if (dtMast.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dtMast.Rows.Count; iCnt++)
                    {
                        Language lg = new Language();
                        lg.language_name = Convert.ToString(dtMast.Rows[iCnt]["Language"].ToString());
                        lg.LanguageID = Convert.ToInt32(dtMast.Rows[iCnt]["ID"].ToString());
                        lst_Language.Add(lg);
                    }
                    dtMast.Dispose();
                }
                dtMast.Dispose();
                ViewBag.ObjLanguageList = new SelectList(lst_Language, "LanguageID", "language_name");

            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(ViewBag.ObjLanguageList);
        }

        [HttpPost]
        public JsonResult GetTransporterDetails()
        {
            try
            {
                List<Transporter> lst_Transporter = new List<Transporter>();
                DataTable dtMast = objDbTrx.GetTransportDtl();
                if (dtMast.Rows.Count > 0)
                {

                    for (int iCnt = 0; iCnt < dtMast.Rows.Count; iCnt++)
                    {
                        Transporter tr = new Transporter();
                        tr.Transporter_name = Convert.ToString(dtMast.Rows[iCnt]["Transport_name"].ToString());
                        tr.TransporterID = Convert.ToInt32(dtMast.Rows[iCnt]["ID"].ToString());
                        lst_Transporter.Add(tr);
                    }
                    dtMast.Dispose();
                }
                ViewBag.ObjTransporterList = new SelectList(lst_Transporter, "TransporterID", "Transporter_name");

            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(ViewBag.ObjTransporterList);
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
        [HttpPost]
        public JsonResult GetCircleAddressDetails(string CircleID)
        {
            InvoiceCumChallan lst_invCumChal = new InvoiceCumChallan();
            try
            {
                //DataTable dt = objDbTrx.GetCircleMasterDetailsByCircleId(Convert.ToInt32(CircleID));
                DataTable dt = objDbTrx.GetCircleMasterDetailsByCircleIdNew(Convert.ToInt32(CircleID));
                if (dt.Rows.Count > 0)
                {
                    lst_invCumChal.CircleAddress = Convert.ToString(dt.Rows[0]["CIRCLE_ADDRESS"]);
                    lst_invCumChal.CirclePinCode = Convert.ToString(dt.Rows[0]["CIRCLE_PINCODE"]);
                    lst_invCumChal.InspectorName = Convert.ToString(dt.Rows[0]["CIRCLE_OFFICER_NAME"]);
                    lst_invCumChal.InspectorPhoneNo = Convert.ToString(dt.Rows[0]["MOBILE_NO"]);
                    lst_invCumChal.InspectorEmailId = Convert.ToString(dt.Rows[0]["EMAIL_ID"]);
                }
                else
                {
                    lst_invCumChal.CircleAddress = "";
                    lst_invCumChal.CirclePinCode = "";
                    lst_invCumChal.InspectorName = "";
                    lst_invCumChal.InspectorPhoneNo = "";
                    lst_invCumChal.InspectorEmailId = "";
                }

            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(lst_invCumChal);
        }

        [HttpPost]
        [ActionName("RevertScannedItems")]
        public JsonResult RevertScannedItems(string challanId, string pData)
        {
            bool result = default(bool);
            int intChallanID = default(int);
            try
            {
                var userSessionObject = Session["UserSec"] != null ? ((UserSec)Session["UserSec"]) : new UserSec();
                if (string.IsNullOrEmpty(userSessionObject.UserId))
                {
                    throw new Exception("Session timed out. Please login again.");
                }
                int.TryParse(challanId, out intChallanID);
                //var objects = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(pData);
                //string[] pDataDefined = objects.Select(obj => Newtonsoft.Json.JsonConvert.SerializeObject(obj)).ToArray();
                //string fullstr = string.Join(",", pDataDefined).Replace('"', ' ').Trim().Replace(" ", "");
                if (string.IsNullOrWhiteSpace(pData))
                {
                    throw new Exception("No item selected");
                }
                ////result = objDbTrx.UpdateBinderDtlOnScanUndo(intChallanID, pData, userSessionObject.UserId);
                //result = objDbTrx.UpdateBinderDtlOnScanUndoSingle(intChallanID, pData, userSessionObject.UserId);
                result = objDbTrx.UpdateBinderDtlOnScanUndoSingleNew(intChallanID, pData, userSessionObject.UserId);
                if (result)
                    return Json(new { Success = 1, Message = "Selected item(s) reverted successfully" }, JsonRequestBehavior.AllowGet);
                else
                    throw new Exception("Revert operation failed");
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
                return Json(new { Success = 0, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpPost]
        //[ActionName("UpdateBinderDtlOnScan")]
        //public JsonResult UpdateBinderDtlOnScan(int challanId, string binderAllotCode)
        //{
        //    bool result = default(bool);
        //    try
        //    {
        //        var userSessionObject = Session["UserSec"] != null ? ((UserSec)Session["UserSec"]) : new UserSec();
        //        if (string.IsNullOrEmpty(userSessionObject.UserId))
        //        {
        //            throw new Exception("Session timed out. Please login again.");
        //        }
        //        result = objDbTrx.UpdateBinderDtlOnScan(challanId, binderAllotCode, userSessionObject.UserId);
        //        if (result)
        //            return Json(new { Success = 1, Message = "Scanned and updated successfully" }, JsonRequestBehavior.AllowGet);
        //        else
        //            throw new Exception("Failed to update data");
        //    }
        //    catch (Exception ex)
        //    {
        //        objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
        //        return Json(new { Success = 0, Message = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        
        

        [HttpPost]
        [ActionName("UpdateBinderDtlOnScan")]
        public JsonResult UpdateBinderDtlOnScan(int challanId, string binderAllotCode)
        {
            bool result = default(bool);
            try
            {
                var userSessionObject = Session["UserSec"] != null ? ((UserSec)Session["UserSec"]) : new UserSec();
                if (string.IsNullOrEmpty(userSessionObject.UserId))
                {
                    throw new Exception("Session timed out. Please login again.");
                }
                //result = objDbTrx.UpdateBinderDtlOnScanBarcode(challanId, binderAllotCode, userSessionObject.UserId);
                result = objDbTrx.UpdateBinderDtlOnScan(challanId, binderAllotCode, userSessionObject.UserId);
                if (result)
                    return Json(new { Success = 1, Message = "Scanned and updated successfully" }, JsonRequestBehavior.AllowGet);
                else
                    throw new Exception("Failed to update data");
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
                if (ex.Message != null && ex.Message.Contains("Barcode already in used in other challan"))
                {
                    return Json(new { Success = 0, Message = "Barcode already in used in other challan" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Success = 0, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [ActionName("GetScanBarcodes")]
        public JsonResult GetScanBarcodes(int challanId)
        {
            List<string> lstDtl = new List<string>();
            try
            {
                var userSessionObject = Session["UserSec"] != null ? ((UserSec)Session["UserSec"]) : new UserSec();
                if (string.IsNullOrEmpty(userSessionObject.UserId))
                {
                    throw new Exception("Session timed out. Please login again.");
                }
                DataTable dtblDetail = objDbTrx.GetBinderAllotDetailByChallanId(challanId, userSessionObject.UserId);
                if (dtblDetail.Rows.Count > 0)
                {
                    for (int i = 0; i < dtblDetail.Rows.Count; i++)
                    {
                        lstDtl.Add(dtblDetail.Rows[i]["BINDER_ALLOT_CODE"].ToString());
                    }
                }
                return Json(lstDtl, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
                return Json(new { Success = 0, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            
        }
        
        [HttpPost]
        [ActionName("GetBinderAlotDtlByAllotCode")]
        public JsonResult GetBinderAlotDtlByAllotCode(string binderAllotCode,string date)
        {
            int status = default(int);
            string msg = string.Empty;
            try
            {
                DataTable dt = objDbTrx.GetBinderAlotDtlByAllotCode(binderAllotCode);
                if (dt.Rows.Count > default(int))
                {
                    int challanId = default(int);
                    int.TryParse(dt.Rows[0]["CHALLAN_ID"].ToString(), out challanId);

                    if (challanId == default(int))
                    {
                        msg = string.Format("{0}^{1}^{2}", dt.Rows[0]["COMMON_BOOK_CODE"].ToString(), dt.Rows[0]["LOT"].ToString(), dt.Rows[0]["DTLID"].ToString());
                        status = 1;
                    }
                    else
                    {
                        status = 5;
                        msg = "Already Exist";
                    }
                }
                else
                {
                    //return Json(new { Success = 5, Message = "Already Exist" });
                    status = 2;
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
                status = default(int);
                msg = ex.Message;
            }
            return Json(new { Success = status, Message = msg });
        }

        [HttpPost]
        [ActionName("GetLiveBinderBookStatusOnScan")]
        public ActionResult GetLiveBinderBookStatusOnScan(string binderAllotCode, string categoryId, bool isPartialViewRequest = true)
        {
            int catId = default(int);
            InvoiceCumChallan objInv = new InvoiceCumChallan();
            try
            {
                Int16 AccadYear = Convert.ToInt16(((UserSec)Session["UserSec"]).AcademicYearId);
                int.TryParse(categoryId, out catId);
                DataTable dt = objDbTrx.GetLiveBinderBookStatusOnScan(binderAllotCode, catId, AccadYear);
                if (dt.Rows.Count > default(int))
                {
                    objInv.lstBinderDetailsByScan = new List<BinderDetailsByScan>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        BinderDetailsByScan obj = new BinderDetailsByScan();
                        obj.BINDER_ALLOT_ID = Convert.ToInt32(dt.Rows[i]["BINDER_ALLOT_ID"].ToString());
                        obj.BINDER_ID = Convert.ToInt32(dt.Rows[i]["BINDER_ID"].ToString());
                        obj.BinderName = dt.Rows[i]["BinderName"].ToString();
                        obj.Book_Code = dt.Rows[i]["Book_Code"].ToString();
                        obj.Common_Book_Code = dt.Rows[i]["Common_Book_Code"].ToString();
                        obj.Book_Name = dt.Rows[i]["Book_Name"].ToString();
                        obj.CategoryID = Convert.ToDouble(dt.Rows[i]["CATEGORY_ID"].ToString());
                        obj.Lot = Convert.ToInt32(dt.Rows[i]["Lot"].ToString());
                        obj.TotalQty = Convert.ToInt64(dt.Rows[i]["TotalQty"].ToString());
                        obj.ScannedQty = Convert.ToInt64(dt.Rows[i]["ScannedQty"].ToString());
                        obj.RemainingQty = Convert.ToInt64(dt.Rows[i]["RemainingQty"].ToString());

                        objInv.lstBinderDetailsByScan.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            if (isPartialViewRequest)
                return PartialView("~/Views/InvoiceCumChallanReqList/_ReqBookDtl.cshtml", objInv);
            else
                return Json(objInv, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("GetBooksReqDetails")]
        public ActionResult GetBooksReqDetails(string District, string CircleId, string categoryId, string languageId, string ChallanId, bool isPartialViewRequest = false)
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
                DataTable dtTransaction = objDbTrx.GetChallanListDtlForBarcode(CircleId, categoryId, languageId, ChallanId, AccadYear);

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
                        //icc.Cartoon = !string.IsNullOrWhiteSpace(dtTransaction.Rows[iCnt]["LOT"].ToString()) ? string.Format("Lot - {0}, Books per lot - {1}", dtTransaction.Rows[iCnt]["LOT"].ToString(), dtTransaction.Rows[iCnt]["REQ_QTY"].ToString()) : "";
                        //icc.Lot = dtTransaction.Rows[iCnt]["LOT"].ToString();
                        //icc.TotBooksPerLot = icc.Cartoon = !string.IsNullOrWhiteSpace(dtTransaction.Rows[iCnt]["LOT"].ToString()) ? string.Format("Lot - {0}, Books per lot - {1}", dtTransaction.Rows[iCnt]["LOT"].ToString(), dtTransaction.Rows[iCnt]["REQ_QTY"].ToString()) : "";
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
                                    // icc.RemarksId = dtReqDtl.Rows[jCnt]["RemarksID"].ToString();
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
                                        var lstOb = obNew.GroupBy(x => x.BOOK_CODE).Select(g => new {
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





                                    //var ob = lst.Where(s => s.BOOK_CODE == item.Book_Code).FirstOrDefault();
                                    //item.TotalLot = ob != null ? ob.LOT : item.TotalLot;
                                    ////item.TotalLotDelimited = "";
                                    //item.QtyShipped = ob != null ? ob.TotalScannedCount : item.QtyShipped;
                                    //item.RemainBal = (item.NetReqQty - item.AlreadyShippedQty) - item.QtyShipped;
                                    //if (item.RemainBal < 1)
                                    //    item.RemainBal = 0;
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
                return PartialView("~/Views/InvoiceCumChallanReqList/_ReqBookDtl.cshtml", lst_invCumChal);
            else
                return Json(lst_invCumChal, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateFinalChallanHeader(InvoiceCumChallan pData)
        {
            bool result = default(bool);
            string msg = string.Empty;
            try
            {
                pData.UserId = ((UserSec)Session["UserSec"]).UserId;
                result = objDbTrx.UpdateFinalChallanHeaderNew(pData);
                msg = result ? "Challan updated successfully" : "Update error detected";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(new { Success = (result ? 1 : 0), Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CancelChallan(int challanId)
        {
            bool result = default(bool);
            string msg = string.Empty;
            try
            {
                //pData.UserId = ((UserSec)Session["UserSec"]).UserId;
                result = objDbTrx.CancelChallan(challanId);
                msg = result ? "Challan Cancel successfully" : "Error detected";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(new { Success = (result ? 1 : 0), Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBinderAllotMentCodeDtl(string BinderAllotMentCode, Int32 ChallanId)
        {
            bool IsError = false;
            SchProvisionalChallan objSchProvisionalChallan = new SchProvisionalChallan();
            //List<BookRequisitionCalculatedDtl> objLstBookRequisitionCalculatedDtl = new List<BookRequisitionCalculatedDtl>();
            //BookRequisitionCalculatedDtl objBookRequisitionCalculatedDtl = new BookRequisitionCalculatedDtl();
            try
            {
                if (BinderAllotMentCode.Length > 0)
                {
                    DataSet dtBinderinfo = objDbTrx.GetBinderAllotmentDtlByCode(BinderAllotMentCode, ChallanId);
                    if (dtBinderinfo.Tables[0].Rows.Count > 0)
                    {
                        objSchProvisionalChallan.IssuedQty = Convert.ToInt32(dtBinderinfo.Tables[0].Rows[0]["QTY_ISSUED"].ToString());
                        objSchProvisionalChallan.Lot = Convert.ToInt32(dtBinderinfo.Tables[0].Rows[0]["Lot"].ToString());
                        // objBookRequisitionCalculatedDtl.BookCode = dtBinderinfo.Tables[0].Rows[0]["BOOK_CODE"].ToString();
                        // objBookRequisitionCalculatedDtl.BookCode = dtBinderinfo.Tables[0].Rows[0]["BOOK_CODE"].ToString();

                    }
                    if (dtBinderinfo.Tables[1].Rows.Count > 0)
                    {
                        objSchProvisionalChallan.AllotedQty = Convert.ToInt32(dtBinderinfo.Tables[1].Rows[0]["AllotedQty"].ToString());
                        objSchProvisionalChallan.IssuedQty = Convert.ToInt32(dtBinderinfo.Tables[1].Rows[0]["IssuedQty"].ToString());
                    }
                    if (dtBinderinfo.Tables[0].Rows.Count == 0 || dtBinderinfo.Tables[0] == null)
                    {
                        objSchProvisionalChallan.UpdateCode = "ERROR";
                        objSchProvisionalChallan.UpdateMessage = "Allotment code " + BinderAllotMentCode + " is Invalid. Please try with some other Allotment Code";
                        IsError = true;
                    }
                }
            }
            catch (Exception ex)
            {
                objSchProvisionalChallan.UpdateCode = "ERROR";
                objSchProvisionalChallan.UpdateMessage = "Some Error occured. please contact system administrator.";

                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(objSchProvisionalChallan, JsonRequestBehavior.AllowGet);
        }
    }
}
