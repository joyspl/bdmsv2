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
    public class AdminCircleLockController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCircleLock()
        {
            //      SA        if (!String.IsNullOrEmpty(Request["chkblock"]) || !String.IsNullOrEmpty(Request["chkbStocklock"]))
            if (Request["ddlReqYear"].ToString() != "-1" && !String.IsNullOrEmpty(Request["txtLockDate"]))
            {
                string district = Request["ddlDistrict"].ToString();
                string circle = Request["ddlCircle"].ToString();
                string reqYear = Request["ddlReqYear"].ToString();
                string lockDate = Request["txtLockDate"].ToString();
                int reqLock = String.IsNullOrEmpty(Request["chkblock"]) ? 0 : 1; 
                
                int stockLock = String.IsNullOrEmpty(Request["chkbStocklock"]) ? 0: 1;

                //if district is all get all circles from circle master
                //if date and req lock is selected
                //then upsert circles with req_lock and date
                //if stock lock is selected
                //then stock lock upsert 


                //if district and all cicrles selected get all circles of a district
                //if date and req lock is selected
                //then upsert circles with req_lock and date
                //if stock lock is selected
                //then stock lock upsert 

                try
                {
                    bool isUpdated = objDbTrx.InsertReqLockStock(district,circle,reqYear,lockDate,reqLock,stockLock);
                }                                               
                catch (Exception ex)                             
                {                                                 
                    objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
                }                                                
            }

            return View("Index");
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



        [HttpGet]
        public JsonResult GetRequestYear()
        {
            List<ReqYears> ObjYearLst = new List<ReqYears>();
            try
            {

                DataTable dt = objDbTrx.GetRequestYear();
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        ReqYears objYear = new ReqYears();
                        objYear.ReqYear = dt.Rows[iCnt]["SelectYear"].ToString();

                        ObjYearLst.Add(objYear);
                    }
                    //ViewBag.ObjYearList = new SelectList(ObjYearLst, "SelectYear", "SelectYear");
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(ObjYearLst, JsonRequestBehavior.AllowGet);
        }
    }
}
