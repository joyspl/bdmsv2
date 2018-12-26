using SARASWATIPRESSNEW.BusinessLogicLayer;
using SARASWATIPRESSNEW.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SARASWATIPRESSNEW.Controllers
{
    [SessionAuthorize]
    public class TrxSchProvisionalChallanController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index(Int64? ChallanId)
        {
            ViewBag.Active = "TrxSchProvisionalChallan";
            SchProvisionalChallan objSchProvisionalChallan = new SchProvisionalChallan();
            if (ChallanId == null || ChallanId <= 0)
            {
                objSchProvisionalChallan.ChallanDate = DateTime.Now.ToString("dd-MMM-yyyy").ToUpper();
                objSchProvisionalChallan.ChallanNo = "TBC" + (DateTime.Now.Month >= 4 ? DateTime.Now.Year.ToString().Substring(2) + "-" + (Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2)) + 1) + "-XXXXXXX" : (Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2)) - 1) + "-" + DateTime.Now.Year.ToString().Substring(2) + "-XXXXXXX").ToString();
                objSchProvisionalChallan.ChallanId = -1;
            }
            else if (ChallanId > 0)
            {
                try
                {
                    DataTable dtRequisition = objDbTrx.GetSchProbChallanByChallanId(Convert.ToInt64(ChallanId));
                    if (dtRequisition.Rows.Count > 0)
                    {                        
                        objSchProvisionalChallan.ChallanId =Convert.ToInt32(dtRequisition.Rows[0]["ID"].ToString());
                        objSchProvisionalChallan.ChallanDate = Convert.ToDateTime(dtRequisition.Rows[0]["CHALLAN_DATE"]).ToString("dd-MMM-yyyy");
                        objSchProvisionalChallan.ChallanNo = dtRequisition.Rows[0]["CHALLAN_NUMBER"].ToString();
                        objSchProvisionalChallan.LanguageID = Convert.ToInt16(dtRequisition.Rows[0]["LANGUAGE_ID"].ToString());
                        objSchProvisionalChallan.CircleId = Convert.ToInt32(dtRequisition.Rows[0]["CIRCLE_ID"].ToString());
                        objSchProvisionalChallan.DistrictId = Convert.ToInt16(dtRequisition.Rows[0]["DISTRICT_ID"].ToString());
                        objSchProvisionalChallan.SaveStatus = Convert.ToInt32(dtRequisition.Rows[0]["STATUS"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    TempData["AppMessage"] = "Some Error has occurred while performing your activity. Please contact the System Administrator for further assistance.";
                    objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);

                }
            }
            return View(objSchProvisionalChallan);
        }
        [HttpPost]
        public ActionResult TrxSchProvisionalChallanUpdate(SchProvisionalChallan objSchProvisionalChallan)
        {
            try
            {               
                
                string reqGenCode = "";
                objSchProvisionalChallan.AcademicYearID = GlobalSettings.oUserData.AcademicYearId;
                objSchProvisionalChallan.UserId = GlobalSettings.oUserData.UserId;
                if (objSchProvisionalChallan.ChallanId <= 0)
                {
                    objSchProvisionalChallan.SaveStatus = 0;
                    objDbTrx.InsertInSchProvisionalChallan(objSchProvisionalChallan, out  reqGenCode);
                    TempData["AppMessage"] = "Challan created successfully and the Challan code is " + reqGenCode;
                }
                else if (objSchProvisionalChallan.ChallanId > 0)
                {
                    reqGenCode = objSchProvisionalChallan.ChallanNo;
                    objSchProvisionalChallan.SaveStatus = 0;
                   // objDbTrx.UpdateInSchRequisition(objSchProvisionalChallan);
                    TempData["AppMessage"] = "Challan updated successfully for the Challan code is " + reqGenCode;
                }
                
            }
            catch (Exception ex)
            {
                TempData["AppMessage"] = "Some Error has occurred while performing your activity. Please contact the System Administrator for further assistance.";
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);

            }
            return RedirectToAction("Index","TrxSchProvisionalChallanView");
        }
        public ActionResult ProvisionalChallanOperation(string ChallanId, string Command)
        {
            try
            {
                if (Command == "Edit" || Command == "Confirmed")
                {
                    return RedirectToAction("Index", "TrxSchProvisionalChallan", new { ChallanId = ChallanId });
                }
            }
            catch (Exception ex) { objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress); }
            return RedirectToAction("Index", "SchRequisionView");
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
        [HttpPost]
        public JsonResult GetLanguageMasterDtl()
        {
            List<MstLanguage> lstMstLanguage = new List<MstLanguage>();
            try
            {
                DataTable dt = objDbTrx.GetLanguageMasterDetails();
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        MstLanguage objLanguage = new MstLanguage();
                        objLanguage.LanguageID = Convert.ToInt32(dt.Rows[iCnt]["ID"].ToString());
                        objLanguage.LanguageName = dt.Rows[iCnt]["LANGUAGE"].ToString();
                        lstMstLanguage.Add(objLanguage);
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(lstMstLanguage);
        }
        [HttpPost]
        public JsonResult GetCircleAddressDetails(string CircleID)
        {
            MstCircle LstMstCircle = new MstCircle();
            
            try
            {
                DataTable dt = objDbTrx.GetCircleMasterDetailsByCircleId(Convert.ToInt32(CircleID));
                if (dt.Rows.Count > 0)
                {
                    LstMstCircle.CirclAddress = Convert.ToString(dt.Rows[0]["CIRCLE_ADDRESS"]) + "\n" + dt.Rows[0]["CIRCLE_PINCODE"];                   
                }
                else
                {
                    LstMstCircle.CirclAddress = "";                                  
                }

            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(LstMstCircle);
        }
                
        [HttpGet]
        public JsonResult GetSchRequisitionForProbChallan(Int16 InCircleID, Int16 InLanguageId, string BookCode)
        {
            List<SchRequisitionView> objSchRequisitionViewList = new List<SchRequisitionView>();
            try
            {
                Int16 CircleId = InCircleID;
                Int16 LanguageId = InLanguageId;
                Int16 AccadYear = Convert.ToInt16(GlobalSettings.oUserData.AcademicYearId);
                DataTable dt = objDbTrx.Sp_SchRequisitionForProbChallan(CircleId, AccadYear, InLanguageId, BookCode);
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        SchRequisitionView objSchRequisitionView = new SchRequisitionView();
                        objSchRequisitionView.RequisitionID = Convert.ToInt64(dt.Rows[iCnt]["REQUISITION_ID"].ToString());
                        objSchRequisitionView.RequisitionCode = Convert.ToString(dt.Rows[iCnt]["REQ_CODE"].ToString());
                        objSchRequisitionView.RequisitionDate = Convert.ToDateTime(dt.Rows[iCnt]["REQUISITION_DATE"].ToString()).ToString("dd-MMM-yyyy");                       
                        objSchRequisitionView.SchoolCode = Convert.ToString(dt.Rows[iCnt]["SCHOOL_CODE"].ToString());
                        objSchRequisitionView.SchoolName = Convert.ToString(dt.Rows[iCnt]["SCHOOL_NAME"].ToString());
                        objSchRequisitionView.LanguageName = Convert.ToString(dt.Rows[iCnt]["LANGUAGE"].ToString());
                        objSchRequisitionView.RemainQuantity = Convert.ToInt32(dt.Rows[iCnt]["REMAIN_QTY"].ToString());
                        objSchRequisitionViewList.Add(objSchRequisitionView);
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(objSchRequisitionViewList, JsonRequestBehavior.AllowGet);        
            
        }
        [HttpGet]
        public JsonResult GetSchRequisitionForProbChallanByChallanId(Int32 InChallanId)
        {
            List<SchRequisitionView> objSchRequisitionViewList = new List<SchRequisitionView>();
            try
            {
               
                DataTable dt = objDbTrx.GetSchRequisitionForProbChallanById(InChallanId);                
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        SchRequisitionView objSchRequisitionView = new SchRequisitionView();
                        objSchRequisitionView.RequisitionID = Convert.ToInt64(dt.Rows[iCnt]["REQUISITION_ID"].ToString());
                        objSchRequisitionView.RequisitionCode = Convert.ToString(dt.Rows[iCnt]["REQ_CODE"].ToString());
                        objSchRequisitionView.RequisitionDate = Convert.ToDateTime(dt.Rows[iCnt]["REQUISITION_DATE"].ToString()).ToString("dd-MMM-yyyy");
                        objSchRequisitionView.SchoolCode = Convert.ToString(dt.Rows[iCnt]["SCHOOL_CODE"].ToString());
                        objSchRequisitionView.SchoolName = Convert.ToString(dt.Rows[iCnt]["SCHOOL_NAME"].ToString());
                        objSchRequisitionView.LanguageName = Convert.ToString(dt.Rows[iCnt]["LANGUAGE"].ToString());
                        objSchRequisitionView.RemainQuantity = Convert.ToInt32(dt.Rows[iCnt]["REMAIN_QTY"].ToString());
                        objSchRequisitionViewList.Add(objSchRequisitionView);
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(objSchRequisitionViewList, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult GetSchBookRequisitionCalculatedDtl(Int16 InLanguageId, string InRequisitionIds, string InBookCodeIds)
        {
            List<BookRequisitionCalculatedDtl> objLstBookRequisitionCalculatedDtl = new List<BookRequisitionCalculatedDtl>();
            try
            {
                Int16 LanguageId = InLanguageId;
                DataTable dt = objDbTrx.GetSchBookRequisitionCalculatedDtl(InLanguageId, InRequisitionIds, InBookCodeIds);                               
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        BookRequisitionCalculatedDtl objBookRequisitionCalculatedDtl = new BookRequisitionCalculatedDtl();
                        objBookRequisitionCalculatedDtl.BookCode =dt.Rows[iCnt]["BOOK_CODE"].ToString();
                        objBookRequisitionCalculatedDtl.Qty = Convert.ToInt32(dt.Rows[iCnt]["Qty"].ToString());
                        objBookRequisitionCalculatedDtl.TotWeight = Convert.ToDouble(dt.Rows[iCnt]["TotWeight"].ToString());
                        objLstBookRequisitionCalculatedDtl.Add(objBookRequisitionCalculatedDtl);
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(objLstBookRequisitionCalculatedDtl, JsonRequestBehavior.AllowGet);        
            
        }

        [HttpGet]
        public JsonResult GetSchBookRequisitionCalculatedDtlByChallanId(Int32 InChallanId)
        {
            List<BookRequisitionCalculatedDtl> objLstBookRequisitionCalculatedDtl = new List<BookRequisitionCalculatedDtl>();
            try
            {

                DataTable dt= objDbTrx.GetSchBookDtlByChallanId(InChallanId);
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        BookRequisitionCalculatedDtl objBookRequisitionCalculatedDtl = new BookRequisitionCalculatedDtl();
                        objBookRequisitionCalculatedDtl.BookCode = dt.Rows[iCnt]["BOOK_CODE"].ToString();
                        objBookRequisitionCalculatedDtl.Qty = Convert.ToInt32(dt.Rows[iCnt]["Qty"].ToString());
                        objBookRequisitionCalculatedDtl.TotWeight = Convert.ToDouble(dt.Rows[iCnt]["TotWeight"].ToString());
                        objLstBookRequisitionCalculatedDtl.Add(objBookRequisitionCalculatedDtl);
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(objLstBookRequisitionCalculatedDtl, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetBookByLanguageIdDtl(Int32 LanguageId)
        {
            List<SchChallanBookDtl> lstBookDtl = new List<SchChallanBookDtl>();
            try
            {

                DataTable dtBook = objDbTrx.GetBookMasterDetailsByLanguageId(Convert.ToInt64(LanguageId));
                if (dtBook.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dtBook.Rows.Count; iCnt++)
                    {
                        SchChallanBookDtl rq = new SchChallanBookDtl();
                        rq.BookCode = dtBook.Rows[iCnt]["BOOK_CODE"].ToString();
                        rq.BookName = dtBook.Rows[iCnt]["BOOK_NAME"].ToString();
                        rq.ClassName = dtBook.Rows[iCnt]["CLASS"].ToString();                        
                        lstBookDtl.Add(rq);
                    }
                }

            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(lstBookDtl);
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
