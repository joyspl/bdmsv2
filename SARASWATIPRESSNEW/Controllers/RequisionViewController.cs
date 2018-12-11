using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SARASWATIPRESSNEW.Models;
using System.Collections;
using SARASWATIPRESSNEW.BusinessLogicLayer;

namespace SARASWATIPRESSNEW.Controllers
{
   
    public class RequisionViewController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();     
        public ActionResult Index(String topLimit)
        {
            List<RequisitionView> lst_rq = new List<RequisitionView>();            
            try
            {
                Int64 TopLimitVal = 10;
                ViewBag.topLimit = TopLimitVal.ToString();
                try
                {
                    if (topLimit != "" && topLimit != null)
                    {
                        TopLimitVal = Convert.ToInt64(topLimit);
                        ViewBag.topLimit = TopLimitVal.ToString();
                        Session["TopLimit"] = TopLimitVal;
                    }                    
                    else if (topLimit == "" || topLimit == null && Session["TopLimit"].ToString() != "")
                    {
                        TopLimitVal = Convert.ToInt64(Session["TopLimit"].ToString());
                        ViewBag.topLimit = TopLimitVal.ToString();
                        Session["TopLimit"] = "";
                    }
                    else {                       
                        ViewBag.topLimit = TopLimitVal.ToString();
                        Session["TopLimit"] = TopLimitVal;
                    }                    
                }
                catch {
                    TopLimitVal = 10;
                    ViewBag.topLimit = TopLimitVal.ToString();
                   
                }
                string CircleId = "", CircleLock = "",ReqLockStartDate="", ReqLockDate="",defaultLock="0";

                try{
                    CircleId = ((UserSec)Session["UserSec"]).CircleID;
                    //ReqLockStartDate = "07-Feb-"+ DateTime.Now.Year +" 00:00:00.000";
                    string[] formats = { "dd/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "d/MM/yyyy", "dd/MM/yy", "dd/M/yy", "d/M/yy", "d/MM/yy" };
                    DateTime dtF = DateTime.Now;
                    DateTime.TryParseExact(ConfigurationManager.AppSettings["ReqLockStartDate"].ToString(), formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtF);
                    ReqLockStartDate = dtF.ToString("dd-MMM-yyyy") + " 00:00:00.000";
                }
                catch { 
                    CircleId = ""; 
                    CircleLock = "";
                    ReqLockDate = "";
                }

                if (Convert.ToString(CircleId) != "")
                {
                    DataTable dtReqView = objDbTrx.GetRequisitionViewDataByCercleId(Convert.ToInt32(CircleId), TopLimitVal);
                    if (dtReqView.Rows.Count > 0)
                    {
                        for (int iCnt = 0; iCnt < dtReqView.Rows.Count; iCnt++)
                        {
                            RequisitionView rq = new RequisitionView();
                            rq.requisitionid = Convert.ToInt64(dtReqView.Rows[iCnt]["REQUISITION_ID"].ToString());
                            rq.req_date = Convert.ToDateTime(dtReqView.Rows[iCnt]["REQUISITION_DATE"].ToString()).ToString("dd-MMM-yyyy hh:mm tt").ToUpper();


                            defaultLock = "1";
                            if (CircleLock == "1")
                            {
                                try
                                {
                                    if (Convert.ToDateTime(rq.req_date) <= Convert.ToDateTime(ReqLockStartDate) )
                                    {
                                        defaultLock = "0";
                                        rq.DeleteStatus = "";
                                        rq.DeleteUrl = "#";
                                        rq.requisition_stat = "Confirmed";
                                        rq.url = "/RequisionView/Requisition?ReqSessionCode=" + Convert.ToString(dtReqView.Rows[iCnt]["REQUISITION_ID"].ToString()) + "&isConfirmed=1";
                                    }                                   
                                }
                                catch {
                                    defaultLock = "1";
                                }                               
                            }
                           
                            if (defaultLock == "1")
                            {
                                rq.DeleteStatus = " Delete";
                                rq.DeleteUrl = "/RequisionView/DeleteReq?ReqSessionCode=" + Convert.ToString(dtReqView.Rows[iCnt]["REQUISITION_ID"].ToString()) + "&isConfirmed=0";

                                rq.requisition_stat = "Edit |";
                                rq.url = "/RequisionView/Requisition?ReqSessionCode=" + Convert.ToString(dtReqView.Rows[iCnt]["REQUISITION_ID"].ToString()) + "";
                            
                            }
                            rq.req_code = Convert.ToString(dtReqView.Rows[iCnt]["REQ_CODE"].ToString());
                            rq.school_name = Convert.ToString(dtReqView.Rows[iCnt]["SCHOOL_NAME"].ToString());
                            rq.school_code = Convert.ToString(dtReqView.Rows[iCnt]["SCHOOL_CODE"].ToString());
                            rq.language_name = Convert.ToString(dtReqView.Rows[iCnt]["LANGUAGE"].ToString());
                            rq.category_name = Convert.ToString(dtReqView.Rows[iCnt]["BOOK_CATEGORY"].ToString());
                            rq.topLimit = TopLimitVal;
                            lst_rq.Add(rq);
                        }
                    }          

                }
                else
                {
                    return RedirectToAction("Index", "CircleLogin");
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }

            finally
            {
                
            }

            return View(lst_rq);
        }

       
       public ActionResult Requisition(string ReqSessionCode,string isConfirmed)
       {
           string CategoryId = "";
           string LanguageId = "";
           string SchoolId = "";
           DataTable dtReq = objDbTrx.GetRequisitionViewDataByReqId(ReqSessionCode);
           if (dtReq.Rows.Count > 0)
           {
               CategoryId = dtReq.Rows[0]["CATEGORY_ID"].ToString();
               LanguageId = dtReq.Rows[0]["LANGUAGE_ID"].ToString();
               SchoolId = dtReq.Rows[0]["SCHOOL_ID"].ToString();
               dtReq.Dispose();
           }
           Session["ReqSessionCode"] = ReqSessionCode;
           return RedirectToAction("Requisition", "Home", new { CategoryId = CategoryId, LanguageId = LanguageId, SchoolId = SchoolId, isConfirmed = isConfirmed  });
       }

       public ActionResult DeleteReq(string ReqSessionCode)
       {
           try
           {               
               objDbTrx.DeleteRequisition(ReqSessionCode);
               TempData["Message"] = "The Requisition deleted successfully";
           }
           catch (Exception ex) { objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress); }
           return RedirectToAction("Index", "RequisionView");
       }

    }
}
