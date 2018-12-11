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
    public class SchoolChallanReportPrintController : Controller
    {
        //
        // GET: /SchoolChallanReportPrint/
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index(string SchChallanId)
        {
            return View(getData(SchChallanId));
        }
        [HttpGet]
        public Models.SchoolChallan getData(string SchChallanId)
        {
            SchoolChallan ObjReq = new SchoolChallan();
            
            try
            {
                Int64 challanId = Convert.ToInt64(SchChallanId);
                ObjReq.SchoolChallanUniqueId = challanId;
                DataTable dtReqView = objDbTrx.GetSchoolChallanPrintDtl(challanId);
                if (dtReqView.Rows.Count > 0)
                {
                    ObjReq.SchoolChallanCode = Convert.ToString(dtReqView.Rows[0]["SCH_CHALLAN_CODE"].ToString());
                    ObjReq.SchoolChallanDate = Convert.ToDateTime(dtReqView.Rows[0]["SCH_CHALLAN_DATE"].ToString()).ToString("dd-MMM-yyyy");
                    ObjReq.ChallanYear = (Convert.ToDateTime(ObjReq.SchoolChallanDate).Month >= 4 ? (Convert.ToInt32(Convert.ToDateTime(ObjReq.SchoolChallanDate).Year.ToString()) + 1).ToString() : (Convert.ToDateTime(ObjReq.SchoolChallanDate).Year.ToString()).ToString());

                    ObjReq.SchoolName = Convert.ToString(dtReqView.Rows[0]["SCHOOL_NAME"].ToString());
                    ObjReq.SchoolCode = Convert.ToString(dtReqView.Rows[0]["SCHOOL_CODE"].ToString());
                    ObjReq.SchoolAddress = Convert.ToString(dtReqView.Rows[0]["SCHOOL_ADDRESS"].ToString());
                    ObjReq.SchoolContactNo = Convert.ToString(dtReqView.Rows[0]["SCHOOL_PHONE_NO"].ToString());
                    ObjReq.SchoolEmailId = Convert.ToString(dtReqView.Rows[0]["SCHOOL_EMAIL_ID"].ToString());
                    ObjReq.Language = Convert.ToString(dtReqView.Rows[0]["LANGUAGE"].ToString());
                    ObjReq.Category = Convert.ToString(dtReqView.Rows[0]["BOOK_CATEGORY"].ToString());
                    ObjReq.CIRCLE_OFFICER_NAME = Convert.ToString(dtReqView.Rows[0]["CIRCLE_OFFICER_NAME"].ToString());
                    ObjReq.CIRCLE_NAME = Convert.ToString(dtReqView.Rows[0]["CIRCLE_NAME"].ToString());
                    ObjReq.CIRCLE_ADDRESS = Convert.ToString(dtReqView.Rows[0]["CIRCLE_ADDRESS"].ToString());
                    ObjReq.CIRCLE_PINCODE = Convert.ToString(dtReqView.Rows[0]["CIRCLE_PINCODE"].ToString());
                    ObjReq.DISTRICT = Convert.ToString(dtReqView.Rows[0]["DISTRICT"].ToString());
                    ObjReq.MOBILE_NO = Convert.ToString(dtReqView.Rows[0]["MOBILE_NO"].ToString());
                    ObjReq.ALTERNATE_MOBILE_NO = Convert.ToString(dtReqView.Rows[0]["ALTERNATE_MOBILE_NO"].ToString());
                    ObjReq.EMAIL_ID = Convert.ToString(dtReqView.Rows[0]["EMAIL_ID"].ToString());


                    ObjReq.BookRwCnt = dtReqView.Rows.Count;
                    if (ObjReq.BookRwCnt > 12)
                    {
                        ObjReq.BookRwCnt = ((ObjReq.BookRwCnt) / 12);
                        if (ObjReq.BookRwCnt > 0)
                        {
                            ObjReq.BookRwCnt++;
                        }
                    }
                    else
                    {
                        ObjReq.BookRwCnt = 1;
                    }
                    List<SchoolChallanBookReqDtl> ObjlstChallan = new List<SchoolChallanBookReqDtl>();
                    for (int iCnt = 0; iCnt < dtReqView.Rows.Count; iCnt++)
                    {                      
                        SchoolChallanBookReqDtl objChDtl = new SchoolChallanBookReqDtl();                       
                        objChDtl.BookCode = dtReqView.Rows[iCnt]["BOOK_CODE"].ToString();
                        objChDtl.BookName = dtReqView.Rows[iCnt]["BOOK_NAME"].ToString();
                        objChDtl.Class = dtReqView.Rows[iCnt]["CLASS"].ToString();
                        objChDtl.QuantityForShipping = Convert.ToInt16(dtReqView.Rows[iCnt]["SHIPPING_QTY"].ToString());                           
                        ObjlstChallan.Add(objChDtl);
                        
                    }
                    ObjReq.trxSchoolChallanBookReqDtl = ObjlstChallan;
                }
            }
            catch (Exception ex) { objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress); }
            return ObjReq;
        }
    }
}
