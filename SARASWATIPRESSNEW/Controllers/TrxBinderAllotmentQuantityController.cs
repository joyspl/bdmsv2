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
    public class TrxBinderAllotmentQuantityController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index(Int32? BinderAllotmentId)
        {
            ViewBag.Active = "TrxBinderAllotmentQuantityView";
            BinderAllotQuantity objBinderAllotQuantity = new BinderAllotQuantity();
            if (BinderAllotmentId == null || BinderAllotmentId <= 0)
            {

                objBinderAllotQuantity.AllotmentDate = DateTime.Now.ToString("dd-MMM-yyyy").ToUpper();
                objBinderAllotQuantity.AllotmentCode = "BLR" + (DateTime.Now.Month >= 4 ? DateTime.Now.Year.ToString().Substring(2) + (Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2)) + 1) + "XXXXXXXX" : (Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2)) - 1) +  DateTime.Now.Year.ToString().Substring(2) + "XXXXXXXX").ToString();
                objBinderAllotQuantity.ID = -1;
            }
            else if (BinderAllotmentId > 0)
            {
                try
                {
                    DataTable GetBinderAllotQtyDtl = objDbTrx.GetBinderAllotmentQtyByID(Convert.ToInt32(BinderAllotmentId));
                     if (GetBinderAllotQtyDtl.Rows.Count > 0)
                     {
                         objBinderAllotQuantity.ID = Convert.ToInt32(GetBinderAllotQtyDtl.Rows[0]["ID"].ToString());
                         objBinderAllotQuantity.AllotmentCode = GetBinderAllotQtyDtl.Rows[0]["BINDER_ALLOT_CODE"].ToString();
                         objBinderAllotQuantity.BinderId =Convert.ToInt16(GetBinderAllotQtyDtl.Rows[0]["BINDER_ID"].ToString());

                         objBinderAllotQuantity.LanguageId = GetBinderAllotQtyDtl.Rows[0]["LANGUAGE_ID"].ToString();
                         objBinderAllotQuantity.ChallanCategoryId = Convert.ToInt16(GetBinderAllotQtyDtl.Rows[0]["CHALLAN_CATEGORY_ID"].ToString());

                         objBinderAllotQuantity.BookCode = GetBinderAllotQtyDtl.Rows[0]["BOOK_CODE"].ToString();
                         objBinderAllotQuantity.BinderName = GetBinderAllotQtyDtl.Rows[0]["BinderName"].ToString();
                         objBinderAllotQuantity.LanguageName = GetBinderAllotQtyDtl.Rows[0]["LANGUAGE"].ToString();
                         objBinderAllotQuantity.BookName = GetBinderAllotQtyDtl.Rows[0]["BOOK_NAME"].ToString();
                         objBinderAllotQuantity.AllotmentDate = Convert.ToDateTime(GetBinderAllotQtyDtl.Rows[0]["ALLOTMENT_DATE"].ToString()).ToString("dd-MMM-yyyy").ToUpper();
                         objBinderAllotQuantity.TotQty = Convert.ToInt32(GetBinderAllotQtyDtl.Rows[0]["TOT_QTY"].ToString());
                         objBinderAllotQuantity.Lot = Convert.ToInt32(GetBinderAllotQtyDtl.Rows[0]["LOT"].ToString());
                         objBinderAllotQuantity.ReqQty = Convert.ToInt32(GetBinderAllotQtyDtl.Rows[0]["REQ_QTY"].ToString());
                         objBinderAllotQuantity.QtyIssued = Convert.ToInt32(GetBinderAllotQtyDtl.Rows[0]["QTY_ISSUED"].ToString());
                         objBinderAllotQuantity.SaveStatus = Convert.ToInt16(GetBinderAllotQtyDtl.Rows[0]["STATUS"].ToString());
                     }
                   
                }
                catch (Exception ex)
                {
                    TempData["AppMessage"] = "Some Error has occurred while performing your activity. Please contact the System Administrator for further assistance.";
                    objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);

                }
            }
            return View(objBinderAllotQuantity);
        }
        [HttpPost]
        public ActionResult TrxBinderAllotmentQuantityUpdate(BinderAllotQuantity objBinderAllotQuantity)
        {
            try
            {
                string reqGenCode = "";
                objBinderAllotQuantity.AcademicYearID = ((UserSec)Session["UserSec"]).AcademicYearId;
                objBinderAllotQuantity.UserId = ((UserSec)Session["UserSec"]).UserId;
                if (objBinderAllotQuantity.ID <= 0)
                {
                    objBinderAllotQuantity.ReqQty = objBinderAllotQuantity.TotQty / objBinderAllotQuantity.Lot;
                    objBinderAllotQuantity.SaveStatus = 0;
                    objDbTrx.InsertInBookAllotQty(objBinderAllotQuantity, out  reqGenCode);
                    TempData["AppMessage"] = "Binder Allotment created successfully and the Allotment code is " + reqGenCode;
                }
                else if (objBinderAllotQuantity.ID > 0)
                {
                    reqGenCode = objBinderAllotQuantity.AllotmentCode;
                    objBinderAllotQuantity.SaveStatus = 0;
                    objBinderAllotQuantity.ReqQty = objBinderAllotQuantity.ReqQty <= default(int) ? (objBinderAllotQuantity.TotQty > 0 && objBinderAllotQuantity.Lot > 0 && objBinderAllotQuantity.TotQty > objBinderAllotQuantity.Lot ? (objBinderAllotQuantity.TotQty / objBinderAllotQuantity.Lot) : default(int)) : objBinderAllotQuantity.ReqQty;
                    objDbTrx.UpdateInBookAllotQty(objBinderAllotQuantity);
                    TempData["AppMessage"] = "Binder Allotment updated successfully for the Allotment code is " + reqGenCode;
                }                
            }
            catch (Exception ex)
            {
                TempData["AppMessage"] = "Some Error has occurred while performing your activity. Please contact the System Administrator for further assistance.";
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return RedirectToAction("Index", "TrxBinderAllotmentQuantityView");
        }
        public ActionResult ReqOperation(string ReqisitionId, string Command, string ReqisitionCode)
        {
            try
            {
                if (Command == "Edit" || Command == "Confirmed")
                {
                    return RedirectToAction("Index", "TrxSchRequisition", new { ReqisitionId = ReqisitionId });
                }
            }
            catch (Exception ex) { objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress); }
            return RedirectToAction("Index", "SchRequisionView");
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
        public JsonResult GetBinderDetails()
        {
            try
            {
                List<Binder> lstBinder = new List<Binder>();
                DataTable dtMast = objDbTrx.GetBinderMaster();
                if (dtMast.Rows.Count > 0)
                {

                    for (int iCnt = 0; iCnt < dtMast.Rows.Count; iCnt++)
                    {
                        Binder onjBinder = new Binder();
                        onjBinder.BinderName = Convert.ToString(dtMast.Rows[iCnt]["BinderName"].ToString());
                        onjBinder.BinderId = Convert.ToInt32(dtMast.Rows[iCnt]["ID"].ToString());
                        lstBinder.Add(onjBinder);
                    }
                    dtMast.Dispose();
                }
                ViewBag.ObjBinderList = new SelectList(lstBinder, "BinderId", "BinderName");

            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(ViewBag.ObjBinderList);
        }

        [HttpPost]
        public JsonResult GetBookDtlByChallanCatIdAndLanguageId(Int32 InLanguageId, Int32 InChallanCatId)
        {
            try
            {
                List<Book_Master> lstBook = new List<Book_Master>();
                DataTable dtMast = objDbTrx.GetBookDtlByChallanCatIdAndLanguageId( InLanguageId,  InChallanCatId);
                if (dtMast.Rows.Count > 0)
                {

                    for (int iCnt = 0; iCnt < dtMast.Rows.Count; iCnt++)
                    {
                        Book_Master onjBook_Master = new Book_Master();
                        onjBook_Master.BookCode = Convert.ToString(dtMast.Rows[iCnt]["BOOK_CODE"].ToString());
                        onjBook_Master.BookName = dtMast.Rows[iCnt]["COMMON_BOOK_CODE"].ToString() + " - " + Convert.ToString(dtMast.Rows[iCnt]["BOOK_NAME"].ToString());
                        lstBook.Add(onjBook_Master);
                    }
                    dtMast.Dispose();
                }
                ViewBag.ObjCategoryList = new SelectList(lstBook, "BookCode", "BookName");

            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(ViewBag.ObjCategoryList);
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
