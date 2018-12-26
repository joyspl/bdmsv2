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
    public class TrxSchChallanViewConfirmedController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            ViewBag.Active = "TrxSchChallanViewConfirmed";
            SchProvisionalChallan objSchProvisionalChallanView = new SchProvisionalChallan();
            objSchProvisionalChallanView.StartDate = Convert.ToDateTime(DateTime.Now).ToString("dd-MMM-yyyy");
            objSchProvisionalChallanView.EndDate = Convert.ToDateTime(DateTime.Now).ToString("dd-MMM-yyyy");
            return View(objSchProvisionalChallanView);
        }
        public ActionResult GetTrxSchProvisionalChallanConfirmedData(string startDate, string endDate)
        {
            List<SchProvisionalChallan> objSchProvisionalChallanList = new List<SchProvisionalChallan>();
            try
            {
              //  Int16 CircleId = Convert.ToInt16(GlobalSettings.oUserData.CircleID);
                Int16 AccadYear = Convert.ToInt16(GlobalSettings.oUserData.AcademicYearId);
                DataTable dt = objDbTrx.GetSchProvisionalChallanConfirmedView(Convert.ToDateTime(startDate + " 00:00:00.000"), Convert.ToDateTime(endDate + " 23:59:59.999"),  AccadYear);
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
