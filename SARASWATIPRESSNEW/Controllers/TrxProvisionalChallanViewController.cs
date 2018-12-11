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

namespace SARASWATIPRESSNEW.Controllers
{
    public class TrxProvisionalChallanViewController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            ViewBag.Active = "TrxProvisionalChallanView";
            try
            {
                bool result = objDbTrx.AutoCorrectBinderDtlDuplicates();
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return View();
        }
        public ActionResult EditOperation(string Id, string Command)
        {
            Session["ChallanId"] = Id;
            return RedirectToAction("Index", "TrxProvisionalChallan");
        }
        public ActionResult PrintOperation(string Id, string Command)
        {
            Session["ChallanId"] = Id;
            return RedirectToAction("Index", "InvoiceCumChallanReport");
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
            catch (Exception ex){
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
        public JsonResult GetChallanViewData(string startDate, string endDate, string CircleID, string DistrictID)
        {
            List<InvoiceCumChallan> objChallanList = new List<InvoiceCumChallan>();
            try
            {
                Int16 AccadYear = Convert.ToInt16(((UserSec)Session["UserSec"]).AcademicYearId);
                DataTable dt = objDbTrx.GetProvisionalChallanViewModified(startDate, endDate, CircleID, DistrictID, AccadYear);
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
                        icc.CategoryName = Convert.ToString(dt.Rows[iCnt]["CHALLAN_BOOK_CATEGORY"].ToString());
                        icc.DistrictName = dt.Rows[iCnt]["DISTRICT"].ToString();
                        icc.Language = Convert.ToString(dt.Rows[iCnt]["LANGUAGE"].ToString());
                        icc.Transporter = Convert.ToString(dt.Rows[iCnt]["Transport_Name"].ToString());
                        icc.CONSIGNEE_NO = Convert.ToString(dt.Rows[iCnt]["CONSIGNEE_NO"].ToString());
                        icc.VEHICLE_NO = Convert.ToString(dt.Rows[iCnt]["VEHICLE_NO"].ToString());
                        icc.Status = Convert.ToInt16(dt.Rows[iCnt]["STATUS"].ToString());
                        icc.UpdatedBy = Convert.ToString(dt.Rows[iCnt]["UPDATED_BY"].ToString());
                        icc.UpdatedTimeStamp = Convert.ToDateTime(dt.Rows[iCnt]["UPDATED_TS"].ToString()).ToString("dd-MMM-yyyy");
                        icc.IsInvoiceCreated = Convert.ToString(dt.Rows[iCnt]["IsInvoiceCreated"].ToString());
                        objChallanList.Add(icc);
                    }             
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(objChallanList);
        }

        [HttpPost]
        public JsonResult ConfirmProvisionalChallan(string griddata)
        {
            string[] ChallanIds = griddata.TrimEnd(',').Split(',');
            string ErrorMessage = "";
            try
            { 
                InvoiceCumChallan objInvoiceCumChallan = new InvoiceCumChallan();
                objInvoiceCumChallan.UserId = ((UserSec)Session["UserSec"]).UserId;
                objInvoiceCumChallan.Status = 1;
                objDbTrx.ConfirmProvisionalChallan(objInvoiceCumChallan, griddata.TrimEnd(','));
                ErrorMessage = ChallanIds.Count() + " Challan confirmed successfully.";
            }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
            catch (Exception ex)
            {
                ErrorMessage = "Some Error occured while confirming Requisition. Please confirm system administrator";
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(ErrorMessage);
        }
      }
}
