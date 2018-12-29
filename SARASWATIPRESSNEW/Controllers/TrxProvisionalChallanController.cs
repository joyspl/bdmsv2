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
    [SessionAuthorize]
    public class TrxProvisionalChallanController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            ViewBag.ProvChallanView = 1;
            ViewBag.Active = "TrxProvisionalChallanView";
            return View(get_Data());
        }

        [HttpGet]
        public Models.InvoiceCumChallan get_Data()
        {
            InvoiceCumChallan lst_invCumChal = new InvoiceCumChallan();
            try
            {                
                lst_invCumChal.InvoiceCumChallanDate = DateTime.Now.ToString("dd-MMM-yyyy");
                //lst_invCumChal.InvoiceCumChallanNo = "TBC" + (DateTime.Now.Month >= 4 ? DateTime.Now.Year.ToString().Substring(2) + "-" + (Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2)) + 1) + "-XXXXXXX" : (Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2)) - 1) + "-" + DateTime.Now.Year.ToString().Substring(2) + "-XXXXXXX").ToString();
                lst_invCumChal.InvoiceCumChallanNo = string.Format("{0}{1}", GlobalSettings.oAcademicYear.PFX_CHALLAN, new String('X', GlobalSettings.oAcademicYear.FormatNumberPaddingCount));
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
                        lst_invCumChal.Status = Convert.ToInt16(dtChallanDtl.Rows[0]["STATUS"].ToString());
                        lst_invCumChal.DistrictId = Convert.ToInt32(dtChallanDtl.Rows[0]["DISTRICT_ID"].ToString());
                        lst_invCumChal.CircleId = Convert.ToInt32(dtChallanDtl.Rows[0]["CircleId"].ToString());
                        lst_invCumChal.CategoryId = Convert.ToInt32(dtChallanDtl.Rows[0]["CHALLAN_BOOK_CATEGORY_ID"].ToString());
                        lst_invCumChal.LanguageId = Convert.ToInt32(dtChallanDtl.Rows[0]["LANGUAGE_ID"].ToString());
                        lst_invCumChal.ManualChallanNo = dtChallanDtl.Rows[0]["ManualChallanNo"].ToString();
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
        public ActionResult Index(InvoiceCumChallan objInvCumChal)
        {
            
            if (ModelState.IsValid)
            {
                //if (String.IsNullOrEmpty(objInvCumChal.CONSIGNEE_NO))
                //    objInvCumChal.CONSIGNEE_NO = "";

                //if (String.IsNullOrEmpty(objInvCumChal.VEHICLE_NO))
                //    objInvCumChal.VEHICLE_NO = "";

                if (string.IsNullOrWhiteSpace(objInvCumChal.CONSIGNEE_NO) || string.IsNullOrWhiteSpace(objInvCumChal.VEHICLE_NO))
                {
                    TempData["AppMessage"] = "Vehicle or  Consignee Number cannot be blank";
                    return RedirectToAction("Index", "TrxProvisionalChallan");
                }

                try
                {
                    string ChallanNo = "";
                    var userId = Session["UserSec"] != null ? GlobalSettings.oUserData.UserId : string.Empty;
                    if (userId != "")
                    {
                        if (Convert.ToInt64(objInvCumChal.ChallanId) != 0)
                        {
                            objInvCumChal.UserId = GlobalSettings.oUserData.UserId;                           
                            objDbTrx.UpdateInProvisionalChallan(objInvCumChal);
                            TempData["AppMessage"] = "Challan updated successfully and the Challan code is " + objInvCumChal.InvoiceCumChallanNo;
                        }
                        else
                        {
                            objInvCumChal.AcadYearId = GlobalSettings.oUserData.AcademicYearId;
                            objInvCumChal.UserId = GlobalSettings.oUserData.UserId;
                            objDbTrx.InsertInProvisionalChallan(objInvCumChal, GlobalSettings.oAcademicYear.PFX_CHALLAN, GlobalSettings.oAcademicYear.FormatNumberPaddingCount, out ChallanNo);
                            TempData["AppMessage"] = "Challan created successfully and the Challan code is " + ChallanNo;
                        }
                        return RedirectToAction("Index", "TrxProvisionalChallanView");
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
            return RedirectToAction("Index", "TrxProvisionalChallanView");
        }

        [HttpPost]
        public ActionResult SaveAddress(string CircleId,string SchooldAddress, string Pincode,string InspectorName,string InspectorPhoneNo,string InspectorEmailId)
        {
            try
            {
                objDbTrx.UpdateCircleUserMasterAddress(CircleId, SchooldAddress, Pincode, InspectorName, InspectorPhoneNo, InspectorEmailId);
            }
            catch (Exception ex){
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
                DataTable dt = objDbTrx.GetCircleMasterDetailsByCircleId(Convert.ToInt32(CircleID));
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

       
    }
}
