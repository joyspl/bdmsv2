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
    public class SchoolChallanController : Controller
    {       
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateChallan()
        {
            SchoolChallan ObjReq = new SchoolChallan();

            try
            {
                Int64 ReqId = Convert.ToInt64(TempData["ID"].ToString());
                Int64 challanId = Convert.ToInt64(TempData["SchChallanId"].ToString());
                ObjReq.SchoolChallanUniqueId = challanId;
                DataTable dtReqView = objDbTrx.GetRequisitionDtlByReqId(ReqId, challanId);
                if (dtReqView.Rows.Count > 0)
                {
                    ObjReq.SchoolChallanDate = DateTime.Now.ToString("dd-MMM-yyyy");
                    //ObjReq.SchoolChallanCode = "SCH" + (DateTime.Now.Month >= 4 ? DateTime.Now.Year.ToString().Substring(2) + "-" + (Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2)) + 1) + "-XXXXXXX" : (Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2)) - 1) + "-" + DateTime.Now.Year.ToString().Substring(2) + "-XXXXXXX").ToString();
                    ObjReq.SchoolChallanCode = string.Format("{0}{1}", GlobalSettings.oAcademicYear.PFX_SCHCHALLAN, new String('X', GlobalSettings.oAcademicYear.FormatNumberPaddingCount));

                    ObjReq.RequisitionId = Convert.ToInt64(dtReqView.Rows[0]["REQUISITION_ID"].ToString());
                    ObjReq.RequisitionDate = Convert.ToDateTime(dtReqView.Rows[0]["REQUISITION_DATE"].ToString()).ToString("dd-MMM-yyyy hh:mm tt").ToUpper();
                    ObjReq.ReqCode = Convert.ToString(dtReqView.Rows[0]["REQ_CODE"].ToString());
                    ObjReq.SchoolName = Convert.ToString(dtReqView.Rows[0]["SCHOOL_NAME"].ToString());
                    ObjReq.SchoolCode = Convert.ToString(dtReqView.Rows[0]["SCHOOL_CODE"].ToString());
                    ObjReq.SchoolAddress = Convert.ToString(dtReqView.Rows[0]["SCHOOL_ADDRESS"].ToString());
                    ObjReq.SchoolContactNo = Convert.ToString(dtReqView.Rows[0]["SCHOOL_PHONE_NO"].ToString());
                    ObjReq.Language = Convert.ToString(dtReqView.Rows[0]["LANGUAGE"].ToString());
                    ObjReq.Category = Convert.ToString(dtReqView.Rows[0]["BOOK_CATEGORY"].ToString());

                     if (dtReqView.Rows[0]["SCH_CHALLAN_CODE"].ToString() != "")
                     {
                         ObjReq.SchoolChallanCode = Convert.ToString(dtReqView.Rows[0]["SCH_CHALLAN_CODE"].ToString());
                         ObjReq.SchoolChallanDate = Convert.ToDateTime(dtReqView.Rows[0]["SCH_CHALLAN_DATE"].ToString()).ToString("dd-MMM-yyyy");
                     }
                    
                     
                    List<SchoolChallanBookReqDtl> ObjlstChallan = new List<SchoolChallanBookReqDtl>();
                    for (int iCnt = 0; iCnt < dtReqView.Rows.Count; iCnt++)
                    {
                        //    Remaining = 0;
                        SchoolChallanBookReqDtl objChDtl = new SchoolChallanBookReqDtl();
                        objChDtl.ReqDtlId = Convert.ToInt64(dtReqView.Rows[iCnt]["REQUISITION_DTL_ID"].ToString());
                        objChDtl.BookID = Convert.ToInt64(dtReqView.Rows[iCnt]["BOOK_ID"].ToString());
                        objChDtl.BookCode = dtReqView.Rows[iCnt]["BOOK_CODE"].ToString();
                        objChDtl.BookName = dtReqView.Rows[iCnt]["BOOK_NAME"].ToString();
                        objChDtl.Class = dtReqView.Rows[iCnt]["CLASS"].ToString();
                        objChDtl.RequisitionQuantity = Convert.ToInt64(dtReqView.Rows[iCnt]["REQ_QTY"].ToString());
                        //06-12-2018 -- Changeed by Anik Sen
                        objChDtl.AvailableStockQuantity = (Convert.ToInt64(dtReqView.Rows[iCnt]["QTY_RECEIVED"].ToString()) + Convert.ToInt64(dtReqView.Rows[iCnt]["STOCK_QTY"].ToString())) - Convert.ToInt64(dtReqView.Rows[iCnt]["ALREADY_SHIPPED"].ToString());
                        objChDtl.AlreadyShippedQuantity = Convert.ToInt64(dtReqView.Rows[iCnt]["ALREADY_SHIPPED"].ToString());
                        objChDtl.QuantityForShipping = Convert.ToInt64(dtReqView.Rows[iCnt]["QTY_FOR_SHIPPING"].ToString());
                        objChDtl.QtyReceived = Convert.ToInt64(dtReqView.Rows[iCnt]["QTY_RECEIVED"].ToString());
                        objChDtl.StockQty = Convert.ToInt64(dtReqView.Rows[iCnt]["STOCK_QTY"].ToString());
                        ObjlstChallan.Add(objChDtl);
                    }
                    ObjReq.trxSchoolChallanBookReqDtl = ObjlstChallan;
                }
            }
            catch (Exception ex) { objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress); }
            return View(ObjReq);
        }
         [HttpPost]
        public ActionResult CreateChallan(SchoolChallan objSchoolChallan)
        {            
            try
            {
                string ChallanNo = "";
                //objSchoolChallan.UserId = GlobalSettings.oUserData.UserId;
                objSchoolChallan.UserId = GlobalSettings.oUserData.UserId;
                objSchoolChallan.AY_ID = GlobalSettings.oUserData.AcademicYearId;
                if (objSchoolChallan.SchoolChallanUniqueId == 0)
                {
                    objDbTrx.InsertInSchoolChallan(objSchoolChallan, GlobalSettings.oAcademicYear.PFX_SCHCHALLAN, GlobalSettings.oAcademicYear.FormatNumberPaddingCount, out ChallanNo);
                }
                else if (objSchoolChallan.SchoolChallanUniqueId > 0)
                {
                    objDbTrx.UpdateInSchoolChallan(objSchoolChallan);                
                }               

            }
            catch (Exception ex) { objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress); }
            return RedirectToAction("Index", "SchoolChallanView");   
        }
        public ActionResult CreateSchoolChallan(string ReqId,string SchChallanId)
        {
            TempData["ID"] = ReqId;
            TempData["SchChallanId"] = SchChallanId;     
            return RedirectToAction("CreateChallan", "SchoolChallan");            
        }
        [HttpPost]
        public JsonResult GetReqViewData(string SchoolId, string startDate, string endDate)
        {
            List<SchoolChallan> objReq = new List<SchoolChallan>(); 
            try
            {
                string CircleId = "-1";
                try { CircleId = GlobalSettings.oUserData.CircleID; }
                catch { CircleId = "-1"; }
                DataTable dtReqView = objDbTrx.GetRequisitionBySchoolId(Convert.ToInt64(SchoolId), (startDate + " 00:00:00.000"), (endDate + " 23:59:59.999"));
                if (dtReqView.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dtReqView.Rows.Count; iCnt++)
                    {
                        SchoolChallan rq = new SchoolChallan();
                        rq.RequisitionId = Convert.ToInt64(dtReqView.Rows[iCnt]["REQUISITION_ID"].ToString());
                        rq.RequisitionDate = Convert.ToDateTime(dtReqView.Rows[iCnt]["REQUISITION_DATE"].ToString()).ToString("dd-MMM-yyyy hh:mm tt").ToUpper();
                        rq.ReqCode = Convert.ToString(dtReqView.Rows[iCnt]["REQ_CODE"].ToString());
                        rq.SchoolName = Convert.ToString(dtReqView.Rows[iCnt]["SCHOOL_NAME"].ToString());
                        rq.SchoolCode = Convert.ToString(dtReqView.Rows[iCnt]["SCHOOL_CODE"].ToString());
                        rq.Language = Convert.ToString(dtReqView.Rows[iCnt]["LANGUAGE"].ToString());
                        rq.Category = Convert.ToString(dtReqView.Rows[iCnt]["BOOK_CATEGORY"].ToString());                       
                        objReq.Add(rq);
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(objReq);
        }


        [HttpPost]
        public JsonResult GetSchoolDetails()
        {
            string CircleId = "";
            try { CircleId = GlobalSettings.oUserData.CircleID; }
            catch { CircleId = ""; }
            List<School> ObjLstSchool = new List<School>();
            try
            {
                DataTable dt = objDbTrx.GetSchoolMasterDetailsByCircleId(Convert.ToInt64(CircleId));
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        School objSchool = new School();
                        objSchool.SchoolID = Convert.ToInt64(dt.Rows[iCnt]["ID"].ToString());
                        objSchool.School_name = dt.Rows[iCnt]["SCHOOL_NAME"].ToString() +" - "+dt.Rows[iCnt]["SCHOOL_CODE"].ToString();
                        //objSchool.School_Code = Convert.ToString(dt.Rows[iCnt]["SCHOOL_CODE"].ToString());
                        ObjLstSchool.Add(objSchool);
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(ObjLstSchool);
        }

    }
}
