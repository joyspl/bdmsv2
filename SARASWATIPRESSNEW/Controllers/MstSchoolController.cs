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
    [SessionAuthorize]
    public class MstSchoolController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index(Int64? DataUniqueId)
        {
            ViewBag.Active = "MstSchoolView";
            MstSchool objMstSchool = new MstSchool();
            if (DataUniqueId == null || DataUniqueId <= 0)
            {               
                objMstSchool.SchoolID = -1;
            }
            else if (DataUniqueId > 0)
            {
                try
                {
                    DataTable dtRequisition = objDbTrx.GetSchoolMasterDetailsBySchoolId(Convert.ToInt64(DataUniqueId));
                    if (dtRequisition.Rows.Count > 0)
                    {
                        objMstSchool.SchoolID =Convert.ToInt32(dtRequisition.Rows[0]["ID"].ToString());
                        objMstSchool.CircleId = Convert.ToInt32(dtRequisition.Rows[0]["CIRCLE_ID"].ToString());
                        objMstSchool.SchoolCode =dtRequisition.Rows[0]["SCHOOL_CODE"].ToString();
                        objMstSchool.SchoolName = dtRequisition.Rows[0]["SCHOOL_NAME"].ToString();
                        objMstSchool.SchoolAdrees = dtRequisition.Rows[0]["SCHOOL_ADDRESS"].ToString();
                        objMstSchool.SchoolEmailid = dtRequisition.Rows[0]["SCHOOL_EMAIL_ID"].ToString();
                        objMstSchool.SchoolMobile = dtRequisition.Rows[0]["SCHOOL_PHONE_NO"].ToString();
                        objMstSchool.SchoolAlternateMobile = dtRequisition.Rows[0]["SCHOOL_ALT_PHONE_NO"].ToString();
                        objMstSchool.DeleivaryAddress = dtRequisition.Rows[0]["SCHOOL_DELIVARY_ADDRESS"].ToString();
                        objMstSchool.PoliceStation = dtRequisition.Rows[0]["POLICE_STATION"].ToString();
                        objMstSchool.PostalCode = dtRequisition.Rows[0]["POSTAL_CODE"].ToString();
                        
                    }
                }
                catch (Exception ex)
                {
                    TempData["AppMessage"] = "Some Error has occurred while performing your activity. Please contact the System Administrator for further assistance.";
                    objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);

                }
            }
            return View(objMstSchool);
        }
        [HttpPost]
        public ActionResult MstSchoolUpdate(MstSchool objMstSchool)
        {
            try
            {
                string reqGenCode = "";
                objMstSchool.UserId = GlobalSettings.oUserData.UserId;
                objMstSchool.CircleId = Convert.ToInt32(GlobalSettings.oUserData.CircleID);
                objMstSchool.DistrictID = Convert.ToInt32(GlobalSettings.oUserData.DistrictID);
                if (objMstSchool.SchoolID <= 0)
                {
                    objDbTrx.InsertInSchool(objMstSchool, out  reqGenCode);
                    TempData["AppMessage"] = "School Name " + objMstSchool.SchoolName + " and school Code " + objMstSchool.SchoolCode + " Saved successfully";
                }
                else if (objMstSchool.SchoolID > 0)
                {
                    objDbTrx.UpdateInSchool(objMstSchool);
                    TempData["AppMessage"] = "School Name " + objMstSchool.SchoolName + " and school Code " + objMstSchool.SchoolCode + " Updated successfully";
                }

            }
            catch (Exception ex)
            {
                TempData["AppMessage"] = "Some Error has occurred while performing your activity. Please contact the System Administrator for further assistance.";
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);

            }
            return RedirectToAction("Index", "MstSchoolView");
        }
        public ActionResult ReqOperation(string DataUniqueId, string Command)
        {
            try
            {
                if (Command == "Edit")
                {
                    return RedirectToAction("Index", "MstSchool", new { DataUniqueId = DataUniqueId });
                }
            }
            catch (Exception ex) { objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress); }
            return RedirectToAction("Index", "SchRequisionView");
        }

        public JsonResult isDuplicateRecordExistInSchool(Int32 InDataUniqueId, string InSchoolCode)
        {
            List<MstSchool> lstMstSchool = new List<MstSchool>();
            try
            {
                DataTable dtMstCircle = objDbTrx.isDuplicateRecordExistInSchool(InDataUniqueId, InSchoolCode);
                if (dtMstCircle.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dtMstCircle.Rows.Count; iCnt++)
                    {
                        MstSchool objMstSchool = new MstSchool();
                        objMstSchool.SchoolCode = dtMstCircle.Rows[iCnt]["SCHOOL_CODE"].ToString();
                        objMstSchool.SchoolName = dtMstCircle.Rows[iCnt]["SCHOOL_NAME"].ToString();
                        objMstSchool.CircleName = dtMstCircle.Rows[iCnt]["CIRCLE_NAME"].ToString();
                        lstMstSchool.Add(objMstSchool);
                    }
                }

            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(lstMstSchool, JsonRequestBehavior.AllowGet);
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