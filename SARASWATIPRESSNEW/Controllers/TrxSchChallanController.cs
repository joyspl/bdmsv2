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
    public class TrxSchChallanController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index(Int64? ChallanId)
        {
            ViewBag.Active = "TrxSchChallanViewConfirmed";
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

                        objSchProvisionalChallan.ConsigneeNo = dtRequisition.Rows[0]["CONSIGNEE_NO"].ToString();
                        objSchProvisionalChallan.VehicleNo = dtRequisition.Rows[0]["VEHICLE_NO"].ToString();

                        objSchProvisionalChallan.SaveStatus = Convert.ToInt32(dtRequisition.Rows[0]["STATUS"].ToString());
                        if (dtRequisition.Rows[0]["TRANSPORTER_ID"].ToString() != ""  )
                        {
                            objSchProvisionalChallan.TransporterID = dtRequisition.Rows[0]["TRANSPORTER_ID"].ToString();
                        }
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
        public JsonResult UpdateSchConfirmChallan(string TransporterID, string ConsigneeNo, string VehicleNo, Int32 ChallanId)
        {
            SchProvisionalChallan objSchProvisionalChallan = new SchProvisionalChallan();
            try
            {
                objSchProvisionalChallan.UserId = GlobalSettings.oUserData.UserId;
                objSchProvisionalChallan.TransporterID = TransporterID;
                objSchProvisionalChallan.ConsigneeNo = ConsigneeNo;
                objSchProvisionalChallan.VehicleNo = VehicleNo;
                objSchProvisionalChallan.ChallanId = ChallanId;
                bool isUpdated = objDbTrx.UpdateSchConfirmChallan(objSchProvisionalChallan);
                if (isUpdated == true)
                {
                    objSchProvisionalChallan.UpdateCode = "SUCCESS";
                    objSchProvisionalChallan.UpdateMessage = "Challan information saved successfully";
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
       
        public ActionResult ChallanOperation(string ChallanId, string Command)
        {
            try
            {
                if (Command == "Edit" || Command == "Confirmed" || Command == "Create")
                {
                    return RedirectToAction("Index", "TrxSchChallan", new { ChallanId = ChallanId });
                }
            }
            catch (Exception ex) { objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress); }
            return RedirectToAction("Index", "SchRequisionView");
        }
        
        [HttpPost]
        public JsonResult UpdateSchChallan(string BinderAllotMentCode,Int32 ChallanId)
        {
            SchProvisionalChallan objSchProvisionalChallan = new SchProvisionalChallan();
            DataTable dt = new DataTable();
            List<BookRequisitionCalculatedDtl> ObjlstBookRequisitionCalculatedDtl = new List<BookRequisitionCalculatedDtl>();
            Int32 RequiredQty=0;
            Int32 IssuedQty=0;
            Double RequiredWeight =0;
            Double IssuedWeight =0;
            bool IsError = false;
            try
            {
                if (BinderAllotMentCode.Length > 0)
                {
                    DataSet dtBinderinfo = objDbTrx.GetBinderAllotmentDtlByCode(BinderAllotMentCode, ChallanId);
                    if (dtBinderinfo.Tables[1].Rows.Count > 0)
                    {
                        Int32 Qty= Convert.ToInt32(dtBinderinfo.Tables[1].Rows[0]["QTY"].ToString());
                        Int32 QtyToBeIssued = Convert.ToInt32(dtBinderinfo.Tables[1].Rows[0]["AllotedQty"].ToString()) + Convert.ToInt32(dtBinderinfo.Tables[1].Rows[0]["LOT"].ToString()); ;
                        if (QtyToBeIssued > Qty)
                        {
                            objSchProvisionalChallan.UpdateCode = "ERROR";
                            objSchProvisionalChallan.UpdateMessage = "Quantity to be issued will be " + QtyToBeIssued + " and it can not be more than the requisiont qty" + Qty + " for Book " + dtBinderinfo.Tables[1].Rows[0]["BOOK_CODE"].ToString() + " - " + dtBinderinfo.Tables[1].Rows[0]["BOOK_NAME"].ToString();
                            IsError = true;
                        }
                    }
                    if (dtBinderinfo.Tables[0].Rows.Count == 0 || dtBinderinfo.Tables[0] ==null)
                    {
                        objSchProvisionalChallan.UpdateCode = "ERROR";
                        objSchProvisionalChallan.UpdateMessage = "Allotment code " + BinderAllotMentCode + " is Invalid. Please try with some other Allotment Code";
                        IsError = true;
                    }
                    if (dtBinderinfo.Tables[0].Rows.Count > 0 && IsError==false)
                    {
                        objSchProvisionalChallan.ChallanId = ChallanId;
                        objSchProvisionalChallan.BinderAllotMentCode = BinderAllotMentCode;
                        objSchProvisionalChallan.AcademicYearID = GlobalSettings.oUserData.AcademicYearId;
                        objSchProvisionalChallan.UserId = GlobalSettings.oUserData.UserId;
                        objSchProvisionalChallan.BinderAllotMentId = Convert.ToInt32(dtBinderinfo.Tables[0].Rows[0]["ID"].ToString());
                        objSchProvisionalChallan.ChallanQty = 1;

                        dt = objDbTrx.IsValidBinderAllotmentBookDtl(objSchProvisionalChallan.ChallanId, objSchProvisionalChallan.BinderAllotMentId);
                        if (dt == null || dt.Rows.Count == 0)
                        {
                            objSchProvisionalChallan.UpdateCode = "ERROR";
                            objSchProvisionalChallan.UpdateMessage = "Invalid Binder book details. The Book details is not available for this challan.";
                          //  return Json(objSchProvisionalChallan, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            Int32 ReqQty = Convert.ToInt32(dtBinderinfo.Tables[0].Rows[0]["REQ_QTY"].ToString());
                            Int32 QtyIssued = Convert.ToInt32(dtBinderinfo.Tables[0].Rows[0]["QTY_ISSUED"].ToString());
                            if (QtyIssued < ReqQty)
                            {
                                bool isUpdated = objDbTrx.UpdateSchChallan(objSchProvisionalChallan);
                                if (isUpdated == true)
                                {
                                    objSchProvisionalChallan.UpdateCode = "SUCCESS";
                                    objSchProvisionalChallan.UpdateMessage = "Lot Quantity updated successfully";
                                }
                                else
                                {
                                    objSchProvisionalChallan.UpdateCode = "ERROR";
                                    objSchProvisionalChallan.UpdateMessage = "Lot Quantity updated unsccessfull";
                                }
                            }
                            else
                            {
                                objSchProvisionalChallan.UpdateCode = "ERROR";
                                objSchProvisionalChallan.UpdateMessage = "Allotment Quantity is not valid.All the lot  for allotment code " + BinderAllotMentCode + " is complited.";
                            }
                        }
                    }
                    
                }

                dt = objDbTrx.GetProbChallanBookDetailsById(ChallanId);
                DataTable dtBookAllotedQty = objDbTrx.GetBookAllotedQtyByChallaId(ChallanId);
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {                       
                        BookRequisitionCalculatedDtl objBookRequisitionCalculatedDtl = new BookRequisitionCalculatedDtl();
                        objBookRequisitionCalculatedDtl.Class = dt.Rows[iCnt]["CLASS"].ToString();
                        objBookRequisitionCalculatedDtl.BookCode = dt.Rows[iCnt]["BOOK_CODE"].ToString();
                        objBookRequisitionCalculatedDtl.BookName = dt.Rows[iCnt]["BOOK_NAME"].ToString();
                        objBookRequisitionCalculatedDtl.Qty = Convert.ToInt32(dt.Rows[iCnt]["Qty"].ToString());
                        objBookRequisitionCalculatedDtl.TotWeight = Convert.ToDouble(dt.Rows[iCnt]["TotWeight"].ToString());
                        objBookRequisitionCalculatedDtl.ChallanQty = 0;
                        RequiredQty = RequiredQty + objBookRequisitionCalculatedDtl.Qty;
                        RequiredWeight = RequiredWeight + objBookRequisitionCalculatedDtl.TotWeight;

                        for (int jCnt = 0; jCnt < dtBookAllotedQty.Rows.Count; jCnt++)
                        {
                            if (dt.Rows[iCnt]["BOOK_CODE"].ToString() == dtBookAllotedQty.Rows[jCnt]["BOOK_CODE"].ToString())
                            {
                                objBookRequisitionCalculatedDtl.ChallanQty = Convert.ToInt32(dtBookAllotedQty.Rows[jCnt]["AllotedQty"].ToString());
                                IssuedQty = IssuedQty + objBookRequisitionCalculatedDtl.ChallanQty;
                                IssuedWeight = IssuedWeight + Convert.ToDouble(dtBookAllotedQty.Rows[jCnt]["TotWeight"].ToString());
                                break;
                            }
                        }
                        ObjlstBookRequisitionCalculatedDtl.Add(objBookRequisitionCalculatedDtl);
                       
                    }
                }

                objSchProvisionalChallan.BookRequisitionCalculatedDtlCollection = ObjlstBookRequisitionCalculatedDtl;
                objSchProvisionalChallan.RequiredQty=RequiredQty;
                objSchProvisionalChallan.IssuedQty = IssuedQty;
                objSchProvisionalChallan.RemainigQty = RequiredQty - IssuedQty;

                objSchProvisionalChallan.RequiredWeight = RequiredWeight;
                objSchProvisionalChallan.IssuedWeight = IssuedWeight;
                objSchProvisionalChallan.RemainigWeight = RequiredWeight - IssuedWeight;

            }
            catch (Exception ex)
            {
                objSchProvisionalChallan.UpdateCode = "ERROR";
                objSchProvisionalChallan.UpdateMessage = "Some Error occured. please contact system administrator.";

                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(objSchProvisionalChallan, JsonRequestBehavior.AllowGet);
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

        public ActionResult SchChallanPrint(Int64 ChallanId)
        {
            SchProvisionalChallan objSchProvisionalChallan = new SchProvisionalChallan();
            if (ChallanId > 0)
            {
                try
                {
                    DataTable dtRequisition = objDbTrx.GetSchProbChallanByChallanId(Convert.ToInt64(ChallanId));
                    if (dtRequisition.Rows.Count > 0)
                    {
                        objSchProvisionalChallan.ChallanId = Convert.ToInt32(dtRequisition.Rows[0]["ID"].ToString());
                        objSchProvisionalChallan.ChallanDate = Convert.ToDateTime(dtRequisition.Rows[0]["CHALLAN_DATE"]).ToString("dd-MMM-yyyy");
                        objSchProvisionalChallan.ChallanNo = dtRequisition.Rows[0]["CHALLAN_NUMBER"].ToString();
                        objSchProvisionalChallan.LanguageID = Convert.ToInt16(dtRequisition.Rows[0]["LANGUAGE_ID"].ToString());
                        objSchProvisionalChallan.CircleId = Convert.ToInt32(dtRequisition.Rows[0]["CIRCLE_ID"].ToString());
                        objSchProvisionalChallan.DistrictId = Convert.ToInt16(dtRequisition.Rows[0]["DISTRICT_ID"].ToString());
                        objSchProvisionalChallan.ConsigneeNo = dtRequisition.Rows[0]["CONSIGNEE_NO"].ToString();
                        objSchProvisionalChallan.VehicleNo = dtRequisition.Rows[0]["VEHICLE_NO"].ToString();
                        objSchProvisionalChallan.SaveStatus = Convert.ToInt32(dtRequisition.Rows[0]["STATUS"].ToString());
                        if (dtRequisition.Rows[0]["TRANSPORTER_ID"].ToString() != "")
                        {
                            objSchProvisionalChallan.TransporterID = dtRequisition.Rows[0]["TRANSPORTER_ID"].ToString();
                        }
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

    }
}
