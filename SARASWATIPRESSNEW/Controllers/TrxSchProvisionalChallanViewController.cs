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
    public class TrxSchProvisionalChallanViewController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            ViewBag.Active = "TrxSchProvisionalChallanView";
            SchProvisionalChallan objSchProvisionalChallanView = new SchProvisionalChallan();
            objSchProvisionalChallanView.StartDate = Convert.ToDateTime(DateTime.Now).ToString("dd-MMM-yyyy");
            objSchProvisionalChallanView.EndDate = Convert.ToDateTime(DateTime.Now).ToString("dd-MMM-yyyy");
            return View(objSchProvisionalChallanView);
        }
        public ActionResult GetTrxSchProvisionalChallanListData(string startDate, string endDate)
        {
            List<SchProvisionalChallan> objSchProvisionalChallanList = new List<SchProvisionalChallan>();
            try
            {
               // Int16 CircleId = Convert.ToInt16(((UserSec)Session["UserSec"]).CircleID);
                Int16 AccadYear = Convert.ToInt16(((UserSec)Session["UserSec"]).AcademicYearId);
                DataTable dt = objDbTrx.GetSchProvisionalChallanView(Convert.ToDateTime(startDate + " 00:00:00.000"), Convert.ToDateTime(endDate + " 23:59:59.999"),  AccadYear);
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        SchProvisionalChallan objSchProvisionalChallan = new SchProvisionalChallan();
                        objSchProvisionalChallan.ChallanId = Convert.ToInt32(dt.Rows[iCnt]["ID"].ToString());
                        objSchProvisionalChallan.ChallanNo = Convert.ToString(dt.Rows[iCnt]["CHALLAN_NUMBER"].ToString());
                        objSchProvisionalChallan.ChallanDate = Convert.ToDateTime(dt.Rows[iCnt]["CHALLAN_DATE"].ToString()).ToString("dd-MMM-yyyy");
                        objSchProvisionalChallan.DistrictName = dt.Rows[iCnt]["DISTRICT"].ToString();
                        objSchProvisionalChallan.CircleName = dt.Rows[iCnt]["CIRCLE_NAME"].ToString();
                        objSchProvisionalChallan.SaveStatus = Convert.ToInt16(dt.Rows[iCnt]["STATUS"].ToString());
                        objSchProvisionalChallan.LanguageName = Convert.ToString(dt.Rows[iCnt]["LANGUAGE"].ToString());
                        objSchProvisionalChallan.LastUpdatedBy = Convert.ToString(dt.Rows[iCnt]["UPDATED_BY"].ToString());
                        objSchProvisionalChallan.LastUpdatedOn = Convert.ToDateTime(dt.Rows[iCnt]["UPDATED_TS"].ToString()).ToString("dd-MMM-yyyy");
                        objSchProvisionalChallanList.Add(objSchProvisionalChallan);
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(objSchProvisionalChallanList, JsonRequestBehavior.AllowGet);
            //  return View();
        }
        public JsonResult DeleteRequisition(string ReqisitionId)
        {
            string ErrorMessage = "";
            try
            {

                DataTable dtProbChallanRequisition = objDbTrx.GetSchProbChallanByChallanId(Convert.ToInt64(ReqisitionId));
                if (dtProbChallanRequisition.Rows.Count > 0)
                {
                    ErrorMessage = "The Challan " + dtProbChallanRequisition.Rows[0]["CHALLAN_NUMBER"].ToString() + " and Challan date " + Convert.ToDateTime(dtProbChallanRequisition.Rows[0]["CHALLAN_DATE"]).ToString("dd-MMM-yyyy") + "  deleted successfully";
                }
                dtProbChallanRequisition.Dispose();
                dtProbChallanRequisition.Clear();
                objDbTrx.SchProbChallanDelete(Convert.ToInt64(ReqisitionId));

            }
            catch (Exception ex)
            {
                ErrorMessage = "Some error occured while deleting requisition. please contact system administrator for further assitence.";
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(ErrorMessage);
        }
        public JsonResult ConfirmRequisition(string griddata)
        {

            string[] ChallanIds = griddata.TrimEnd(',').Split(',');
            string ErrorMessage = "";
            try
            {
                SchProvisionalChallan objSchProvisionalChallan = new SchProvisionalChallan();
                objSchProvisionalChallan.UserId = ((UserSec)Session["UserSec"]).UserId;
                objSchProvisionalChallan.SaveStatus = 1;
                objDbTrx.SchRequisitionProbChallanConfirm(objSchProvisionalChallan, griddata.TrimEnd(','));
                ErrorMessage = ChallanIds.Count() + " Challan confirmed successfully.";
            }
            catch (Exception ex)
            {
                ErrorMessage = "Some Error occured while confirming Requisition. Please confirm system administrator";
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(ErrorMessage);
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
