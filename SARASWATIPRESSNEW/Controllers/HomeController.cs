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
    public class HomeController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Requisition(string CategoryId, string LanguageId, string SchoolId, string isConfirmed)
        {
            string CircleId = "";
            try { CircleId = ((UserSec)Session["UserSec"]).CircleID; }
            catch { CircleId = ""; }
            if (CircleId != "")
            {
                DataTable dt = objDbTrx.GetCircleUserMasterDetailsById(Convert.ToInt32(CircleId));
                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    if ((CategoryId == "" || CategoryId == null) && (LanguageId == "" || LanguageId == null) && (SchoolId == "" || SchoolId == null))
                    {
                        Session["ReqSessionCode"] = "";
                    }
                    return View(GetMasterDetails(CategoryId, LanguageId, SchoolId, isConfirmed));
                }
                else
                {
                    return RedirectToAction("Index", "CircleUserEdit");
                }
            }
            else { return RedirectToAction("Index", "CircleLogin"); }
        }

        [HttpPost]
        public ActionResult Requisition(Requisition objcust, string Command)
        {
            if (ModelState.IsValid)
            {
                string CircleId = "", UserId="";
                try { CircleId = ((UserSec)Session["UserSec"]).CircleID; UserId = ((UserSec)Session["UserSec"]).UserId; }
                catch { CircleId = "";UserId=""; }
            
                string ReqSessionCode = objcust.ReqSessionCode;
                if (objcust.stat == true)
                {
                    if (CircleId != "" && (Convert.ToString(ReqSessionCode) == null || Convert.ToString(ReqSessionCode) == ""))
                    {
                        try
                        {                          
                            objcust.CircleID = Convert.ToInt32(CircleId);
                            objcust.UserId = UserId;
                            string reqGenCode="";
                            objDbTrx.InsertInRequisition(objcust, out  reqGenCode);
                            return RedirectToAction("Index", "RequisionView");                                
                        }
                        catch (Exception ex) { objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress); }
                        finally { }
                    }
                    else if (Convert.ToString(ReqSessionCode) != null && Convert.ToString(ReqSessionCode) != "")
                    {
                        objcust.RequisitionID = Convert.ToInt64(ReqSessionCode);
                        objcust.CircleID = Convert.ToInt32(CircleId);
                        objcust.UserId = UserId;
                        objDbTrx.UpdateInRequisition(objcust);
                    }
                    else
                    {
                        return RedirectToAction("Index", "CircleLogin");
                    }
                }
            }
            return RedirectToAction("Index", "RequisionView");
        }   

        [HttpGet]
        public Models.Requisition GetMasterDetails(string categoryId, string LanguageId, string SchoolId, string isConfirmed)
        {
            Requisition lst_req = new Requisition();
            List<Language> lst_language = new List<Language>();
            DataTable dtMastData = new DataTable();

            lst_req.ReqSessionCode = Convert.ToString(Session["ReqSessionCode"]);
            lst_req.isConfirmed = isConfirmed;
            //Session["ReqSessionCode"] = "";
            if (Convert.ToString(categoryId) != null && Convert.ToString(LanguageId) != null && Convert.ToString(categoryId) != "" && Convert.ToString(LanguageId) != "")
            {
                List<RequisitionTrxDtl> lst_book = new List<RequisitionTrxDtl>();
                try
                {
                    DataTable dtReqDtl = new DataTable();
                    if (Convert.ToString(lst_req.ReqSessionCode) != null && Convert.ToString(lst_req.ReqSessionCode) != "")
                    {
                        dtReqDtl = objDbTrx.GetRequisitionViewDataByReqId(lst_req.ReqSessionCode);
                    }
                    DataTable dtBook = objDbTrx.GetBookMasterDetailsById(Convert.ToInt64(categoryId), Convert.ToInt64(LanguageId));
                    if (dtBook.Rows.Count > 0)
                    {
                        for (int iCnt = 0; iCnt < dtBook.Rows.Count; iCnt++)
                        {
                            RequisitionTrxDtl rq = new RequisitionTrxDtl();
                            rq.BookName = dtBook.Rows[iCnt]["BOOK_NAME"].ToString();
                            rq.BookID = Convert.ToInt64(dtBook.Rows[iCnt]["ID"].ToString());
                            rq.classname = dtBook.Rows[iCnt]["CLASS"].ToString();
                            if (Convert.ToString(lst_req.ReqSessionCode) != null && Convert.ToString(lst_req.ReqSessionCode) != "")
                            {
                                if (dtReqDtl.Rows.Count > 0)
                                {
                                    for (int jCnt = 0; jCnt < dtReqDtl.Rows.Count; jCnt++)
                                    {
                                        if (Convert.ToInt64(dtBook.Rows[iCnt]["ID"].ToString()) == Convert.ToInt64(dtReqDtl.Rows[jCnt]["BOOK_ID"].ToString()))
                                        {
                                            rq.StudentEnrolled = Convert.ToInt32(dtReqDtl.Rows[jCnt]["PREV_REQUISITION_QTY"].ToString());
                                            rq.QtyRequirement = Convert.ToInt32(dtReqDtl.Rows[jCnt]["CURR_REQUISITION_QTY"].ToString());
                                            break;
                                        }
                                    }
                                }
                            }
                            lst_book.Add(rq);
                        }
                    }
                    lst_req.reqTrxCollection = lst_book;
                }
                catch (Exception ex) { }
            }
            if (Convert.ToString(SchoolId) != null)
            {
                try
                {
                    dtMastData = objDbTrx.GetSchoolMasterDetailsById(Convert.ToInt64(SchoolId), Convert.ToInt64(((UserSec)Session["UserSec"]).CircleID));
                    if (dtMastData.Rows.Count > 0)
                    {
                        lst_req.school_contact_no = Convert.ToString(dtMastData.Rows[0]["SCHOOL_PHONE_NO"].ToString());
                        lst_req.school_email_id = Convert.ToString(dtMastData.Rows[0]["SCHOOL_EMAIL_ID"].ToString());
                        dtMastData.Dispose();
                    }
                }
                catch (Exception ex) { objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress); }
            }
            try
            {
                dtMastData = objDbTrx.GetLanguageMasterDetails();
                if (dtMastData.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dtMastData.Rows.Count; iCnt++)
                    {
                        Language rq = new Language();
                        rq.language_name = Convert.ToString(dtMastData.Rows[iCnt]["LANGUAGE"].ToString());
                        rq.LanguageID = Convert.ToInt32(dtMastData.Rows[iCnt]["ID"].ToString());
                        lst_language.Add(rq);
                    }
                    dtMastData.Dispose();
                }
                lst_req.languageCollection = lst_language;
                List<Category> lst_category = new List<Category>();
                try
                {
                    dtMastData = objDbTrx.GetBookCategoryMasterDetails();
                    if (dtMastData.Rows.Count > 0)
                    {
                        for (int iCnt = 0; iCnt < dtMastData.Rows.Count; iCnt++)
                        {
                            Category rq = new Category();
                            rq.Category_name = Convert.ToString(dtMastData.Rows[iCnt]["BOOK_CATEGORY"].ToString());
                            rq.CategoryID = Convert.ToInt32(dtMastData.Rows[iCnt]["ID"].ToString());
                            lst_category.Add(rq);
                        }
                        dtMastData.Dispose();
                    }
                    lst_req.categoryCollection = lst_category;
                }
                catch (Exception ex) { objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress); }

                List<School> lst_school = new List<School>();
                try
                {
                    dtMastData = objDbTrx.GetSchoolMasterDetailsByCircleId(Convert.ToInt16(((UserSec)Session["UserSec"]).CircleID));
                    if (dtMastData.Rows.Count > 0)
                    {
                        for (int iCnt = 0; iCnt < dtMastData.Rows.Count; iCnt++)
                        {
                            School rq = new School();
                            rq.School_name = Convert.ToString(dtMastData.Rows[iCnt]["SCHOOL_NAME"].ToString());
                            rq.SchoolID = Convert.ToInt32(dtMastData.Rows[iCnt]["ID"].ToString());
                            rq.schooldisplaytext = Convert.ToString(dtMastData.Rows[iCnt]["SCHOOL_CODE"].ToString()) + "_" + Convert.ToString(dtMastData.Rows[iCnt]["SCHOOL_NAME"].ToString());
                            lst_school.Add(rq);
                        }
                        dtMastData.Dispose();
                    }
                    lst_req.schoolCollection = lst_school;
                    lst_req.CircleID = Convert.ToInt32(((UserSec)Session["UserSec"]).CircleID);

                    lst_req.RequisitionDate = DateTime.Now.ToString("dd-MMM-yyyy HH:MM tt").ToUpper();
                    if (Convert.ToString(categoryId) != null && Convert.ToString(LanguageId) != null)
                    {
                        lst_req.cat_up_id = Convert.ToString(categoryId.ToString());
                        lst_req.lan_up_id = Convert.ToString(LanguageId.ToString());
                    }
                    else
                    {
                        lst_req.cat_up_id = "Select";
                        lst_req.lan_up_id = "Select";
                    }

                    if (Convert.ToString(Session["SchooldCode"]) == "")
                    {
                        if (Convert.ToString(SchoolId) == null)
                        {
                            lst_req.code_school = "Select";
                        }
                        else
                        {
                            lst_req.code_school = SchoolId.ToString();
                        }
                    }
                    else
                    {
                        lst_req.code_school = Convert.ToString(Session["SchooldCode"]);
                        Session["SchooldCode"] = "";
                        try
                        {
                            dtMastData = objDbTrx.GetSchoolMasterDetailsBySchoolId(Convert.ToInt64(lst_req.code_school));
                            if (dtMastData.Rows.Count > 0)
                            {
                                lst_req.school_contact_no = Convert.ToString(dtMastData.Rows[0]["SCHOOL_PHONE_NO"].ToString());
                                lst_req.school_email_id = Convert.ToString(dtMastData.Rows[0]["SCHOOL_EMAIL_ID"].ToString());
                                dtMastData.Dispose();
                            }
                        }
                        catch (Exception ex)
                        {
                           
                        }
                    }
                }
                catch (Exception ex) { objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress); }
            }
            catch (Exception ex) { objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress); }
            return lst_req;
        }

        public ActionResult Challan(string a, string b, string e)
        {
            if (Convert.ToString(Session["sp_name"]) != "")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "SPLogin");
            }
        }
    }
}
