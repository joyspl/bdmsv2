using SARASWATIPRESSNEW.BusinessLogicLayer;
using SARASWATIPRESSNEW.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SARASWATIPRESSNEW.Controllers
{
    [SessionAuthorize]
    public class TrxSchRequisitionController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();

        public ActionResult Index(Int64? ReqisitionId, int circleId = 0)
        {
            ViewBag.Active = "TrxSchRequisition";
            ViewBag.RequisitionLock = false;
            int _circleId = default(int);
            if (ReqisitionId.HasValue)
            {
                string objSsn = ((SARASWATIPRESSNEW.UserSec)System.Web.HttpContext.Current.Session["UserSec"]) != null ? ((SARASWATIPRESSNEW.UserSec)System.Web.HttpContext.Current.Session["UserSec"]).CircleID : default(int).ToString();
                if (circleId <= default(int))
                {
                    int.TryParse(objSsn, out _circleId);
                }
                else
                {
                    _circleId = circleId;
                }
                List<CircleLock> lstCircleLock = new List<CircleLock>();
                DataTable dtCircleLock = objDbTrx.GetCircleLockByCircleId(_circleId);
                if (dtCircleLock != null && dtCircleLock.Rows.Count > default(int))
                {
                    for (int i = 0; i < dtCircleLock.Rows.Count; i++)
                    {
                        CircleLock objCircleLock = new CircleLock();
                        objCircleLock.id = Convert.ToInt32(dtCircleLock.Rows[i]["id"]);
                        objCircleLock.circle_id = Convert.ToInt32(dtCircleLock.Rows[i]["circle_id"]);
                        objCircleLock.Req_lock = dtCircleLock.Rows[i]["Req_lock"].ToString();
                        objCircleLock.Req_year = dtCircleLock.Rows[i]["Req_year"].ToString();
                        objCircleLock.Stock_lock = dtCircleLock.Rows[i]["Stock_lock"].ToString();
                        objCircleLock.ReqLockDate = DateTime.Parse(dtCircleLock.Rows[i]["ReqLockDate"].ToString());
                        lstCircleLock.Add(objCircleLock);
                    }

                    //lstCircleLock.RemoveAll(c => !c.Req_year.Contains("/19"));
                    var circlelock = lstCircleLock.FirstOrDefault();
                    if (circlelock != null)
                    {
                        var reqLockStatus = (DateTime.Today > circlelock.ReqLockDate && circlelock.Req_lock == "1") ? true : false;
                        ViewBag.RequisitionLock = reqLockStatus;
                    }
                }
                // var reqLockStatus =  ? lstCircleLock.FirstOrDefault().Req_lock : default(int).ToString();
                //ViewBag.RequisitionLock = if(DateTime.Today==
            }


            var schRequisitionDetails = SchRequisitionDtl(ReqisitionId);


            return View(schRequisitionDetails);
        }

        [HttpPost]
        public ActionResult Index(Int64? ReqisitionId, string command)
        {
            ViewBag.Active = "TrxSchRequisition";
            StringBuilder strTableReport = new StringBuilder();
            StringBuilder strRowDetails = new StringBuilder();

            var requsitionDtl = SchRequisitionDtl(ReqisitionId);

            var categoryList = GetCategoryList();
            var languageList = GetLanguageList();
            requsitionDtl.MstCategory = categoryList.FirstOrDefault(c => c.CategoryID == requsitionDtl.CategoryID) ?? new MstCategory();
            requsitionDtl.MstLanguage = languageList.FirstOrDefault(c => c.LanguageID == requsitionDtl.LanguageID) ?? new MstLanguage();

            var schoolDetails = SchoolDetail(Convert.ToInt32(requsitionDtl.SchoolID))[0];
            var bookRequisitionDtl = HelperBookRequisitionDtl(
                Convert.ToInt32(requsitionDtl.RequisitionID),
                Convert.ToInt32(requsitionDtl.CategoryID),
                Convert.ToInt32(requsitionDtl.LanguageID),
                Convert.ToInt32(requsitionDtl.SchoolID));


            strRowDetails.AppendLine("<tr>");
            strRowDetails.AppendLine("<td><B>Class</B></td>");
            strRowDetails.AppendLine("<td><B>Book Name</B></td>");
            strRowDetails.AppendLine("<td><B>Previous year Requirement</B></td>");
            strRowDetails.AppendLine("<td><B>Total Requirement</B></td>");
            strRowDetails.AppendLine("</tr>");

            foreach (var item in bookRequisitionDtl)
            {

                strRowDetails.AppendLine("<tr>");
                strRowDetails.AppendLine("<td>" + item.ClassName + "</td>");
                strRowDetails.AppendLine("<td>" + item.BookName + "</td>");
                strRowDetails.AppendLine("<td>" + item.PreviousYearRequirement + "</td>");
                strRowDetails.AppendLine("<td>" + item.StudentEnrolled + "</td>");
                strRowDetails.AppendLine("</tr>");
            }

            strTableReport.AppendLine("<table border='1' >");
            //strTableReport.AppendLine("     <tr><td>Circle Name</td><td><B>" + requsitionDtl.RequisitionDate + "</B></td><td>Circle Name</td><td>"+ "</td></tr>");
            strTableReport.AppendLine("     <tr><td>Requisition Date</td><td><B>" + requsitionDtl.RequisitionDate + "</B></td><td>Requisition Code</td><td><B>" + requsitionDtl.RequisitionCode + "</B></td></tr>");
            strTableReport.AppendLine("     <tr><td>School Name</td><td> <B>" + schoolDetails.SchoolName + " </B></td><td>School Code</td><td><B>" + schoolDetails.SchoolCode + "</B></td></tr>");
            strTableReport.AppendLine("     <tr><td>School Contact</td><td><B>" + schoolDetails.SchoolMobile + "</B></td><td>School Email</td><td><B>" + schoolDetails.SchoolEmailid + "</B></td></tr>");
            strTableReport.AppendLine("     <tr><td>School Category</td><td><B>" + requsitionDtl.MstCategory.Category + "</B></td><td>School Language</td><td><B>" + requsitionDtl.MstLanguage.LanguageName + "</B></td></tr>");
            strTableReport.AppendLine("<tr></tr>");
            strTableReport.AppendLine("<tr></tr>");
            strTableReport.AppendLine("<tr></tr>");
            strTableReport.AppendLine(strRowDetails.ToString());
            strTableReport.AppendLine("</table>");


            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            String FileName = "Book Requisition_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".xls";
            Response.AddHeader("Content-Disposition", "inline;filename=" + FileName);
            String HTMLDataToExport = strTableReport.ToString();


            Response.Write("<html><head><head>" +
            HTMLDataToExport.Replace("<BR>", "<br style='mso-data-placement:same-cell;'>")
                                           .Replace("<br>", "<br style='mso-data-placement:same-cell;'>")
                                           .Replace("<BR >", "<br style='mso-data-placement:same-cell;'>")
                                           .Replace("<BR />", "<br style='mso-data-placement:same-cell;'>")
                                           .Replace("<br />", "<br style='mso-data-placement:same-cell;'>")
                                           .Replace("<Br />", "<br style='mso-data-placement:same-cell;'>")
                                           .Replace("<Br>", "<br style='mso-data-placement:same-cell;'>")
                                           .Replace("<br >", "<br style='mso-data-placement:same-cell;'>") + "</html>");
            Response.End();


            //SchRequisitionDtl(ReqisitionId)
            return View("Index", new { ReqisitionId = ReqisitionId });
        }

        [HttpPost]
        public ActionResult TrxSchRequisitionUpdate(SchRequisition objSchRequisition)
        {
            Requisition objRequisition = new Requisition();
            try
            {
                if (objSchRequisition != null)
                {
                    objRequisition.SchoolID = Convert.ToInt32(objSchRequisition.SchoolID <= default(int) ? objSchRequisition.MstSchool.SchoolID : objSchRequisition.SchoolID);
                    objRequisition.LanguageID = objSchRequisition.MstLanguage.LanguageID;
                    objRequisition.CategoryID = objSchRequisition.MstCategory.CategoryID;
                    objRequisition.CircleID = objSchRequisition.CircleID > default(int) ? objSchRequisition.CircleID : Convert.ToInt32(GlobalSettings.oUserData.CircleID);
                    objRequisition.BookID = objSchRequisition.BookID;
                    objRequisition.RequisitionID = objSchRequisition.RequisitionID;
                    objRequisition.RequisitionDate = string.IsNullOrWhiteSpace(objSchRequisition.RequisitionDate) ? DateTime.Now.ToString() : objSchRequisition.RequisitionDate;
                    objRequisition.ReqSessionCode = objSchRequisition.RequisitionCode;
                    objRequisition.UserId = GlobalSettings.oUserData.UserId;
                    objRequisition.SaveStatus = objSchRequisition.SaveStatus.ToString();
                    objRequisition.reqTrxCollection = new List<RequisitionTrxDtl>();

                    foreach (var sobj in objSchRequisition.reqTrxCollection)
                    {
                        var robj = new RequisitionTrxDtl();
                        robj.BookCode = sobj.BookCode;
                        robj.BookID = sobj.BookID;
                        robj.BookName = sobj.BookName;
                        robj.classname = sobj.ClassName;
                        robj.QtyRequirement = sobj.RequisitionQuantity;
                        robj.RequisitionQuantity = sobj.RequisitionQuantity;
                        robj.StockQuantity = sobj.StockQuantity;
                        robj.StudentEnrolled = sobj.StudentEnrolled;
                        robj.PreviousYearReqQty = sobj.PreviousYearRequirement;

                        objRequisition.reqTrxCollection.Add(robj);
                    }
                }

                for (int iCnt = 0; iCnt < objRequisition.reqTrxCollection.Count; iCnt++)
                {
                    objRequisition.reqTrxCollection[iCnt].RequisitionQuantity = objRequisition.reqTrxCollection[iCnt].StudentEnrolled - objRequisition.reqTrxCollection[iCnt].StockQuantity;
                    objRequisition.reqTrxCollection[iCnt].QtyRequirement = objRequisition.reqTrxCollection[iCnt].RequisitionQuantity;
                }

                string reqGenCode = "";
                objRequisition.AY_ID = objSchRequisition.AcademicYearID = GlobalSettings.oUserData.AcademicYearId;
                //objSchRequisition.UserId = GlobalSettings.oUserData.UserId;
                if (objRequisition.RequisitionID <= 0)
                {
                    objRequisition.SaveStatus = "1";
                    //objDbTrx.InsertInSchRequisition(objSchRequisition, out  reqGenCode);
                    objDbTrx.InsertInRequisition(objRequisition, out reqGenCode);
                    TempData["AppMessage"] = "Requisition created successfully and the requisition code is " + reqGenCode;
                }
                else if (objRequisition.RequisitionID > 0)
                {
                    reqGenCode = objRequisition.ReqSessionCode;
                    objRequisition.SaveStatus = objSchRequisition.CircleID > default(int) ? objRequisition.SaveStatus : "0";
                    //objDbTrx.UpdateInSchRequisition(objSchRequisition);
                    objDbTrx.UpdateInRequisition(objRequisition);
                    TempData["AppMessage"] = "Requisition updated successfully for the requisition code is " + reqGenCode;
                }

            }
            catch (Exception ex)
            {
                TempData["AppMessage"] = "Some Error has occurred while performing your activity. Please contact the System Administrator for further assistance.";
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);

            }
            return RedirectToAction("Index", "SchRequisionView");
        }
        public ActionResult ReqOperation(string ReqisitionId, string Command, string ReqisitionCode, int CircleId = 0)
        {
            try
            {
                if (Command == "Edit" || Command == "Confirmed")
                {
                    return RedirectToAction("Index", "TrxSchRequisition", new { ReqisitionId = ReqisitionId, circleId = CircleId });
                }
            }
            catch (Exception ex) { objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress); }
            return RedirectToAction("Index", "SchRequisionView");
        }
        [HttpPost]
        public JsonResult GetSchoolMasterDtl(string circleid = "")
        {
            List<MstSchool> lstMstSchool = new List<MstSchool>();
            try
            {
                DataTable dt = objDbTrx.GetSchoolMasterDetailsByCircleId(!string.IsNullOrWhiteSpace(circleid) && circleid != "-1" ? Convert.ToInt16(circleid) : Convert.ToInt16(GlobalSettings.oUserData.CircleID));
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        MstSchool objSchool = new MstSchool();
                        objSchool.SchoolID = Convert.ToInt32(dt.Rows[iCnt]["ID"].ToString());
                        objSchool.SchoolCode = dt.Rows[iCnt]["SCHOOL_CODE"].ToString();
                        //objSchool.SchoolName = dt.Rows[iCnt]["SCHOOL_CODE"].ToString() + "_" + dt.Rows[iCnt]["SCHOOL_NAME"].ToString();
                        objSchool.SchoolName = dt.Rows[iCnt]["SCHOOL_NAME"].ToString();
                        lstMstSchool.Add(objSchool);
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(lstMstSchool);
        }
        [HttpPost]
        public JsonResult GetLanguageMasterDtl()
        {
            List<MstLanguage> lstMstLanguage = new List<MstLanguage>();
            try
            {
                lstMstLanguage = GetLanguageList();
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(lstMstLanguage);
        }

        private List<MstLanguage> GetLanguageList()
        {
            List<MstLanguage> lstMstLanguage = new List<MstLanguage>();
            DataTable dt = objDbTrx.GetLanguageMasterDetails();
            if (dt.Rows.Count > 0)
            {
                for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                {
                    MstLanguage objLanguage = new MstLanguage();
                    objLanguage.LanguageID = Convert.ToInt32(dt.Rows[iCnt]["ID"].ToString());
                    objLanguage.LanguageName = dt.Rows[iCnt]["LANGUAGE"].ToString();
                    lstMstLanguage.Add(objLanguage);
                }
            }
            return lstMstLanguage;
        }
        [HttpPost]
        public JsonResult GetCategoryMasterDtl()
        {
            List<MstCategory> lstMstCategory = null;
            try
            {
                lstMstCategory = GetCategoryList();
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(lstMstCategory);
        }

        private List<MstCategory> GetCategoryList()
        {
            List<MstCategory> lstMstCategory = new List<MstCategory>();
            DataTable dt = objDbTrx.GetBookCategoryMasterDetails();
            if (dt.Rows.Count > 0)
            {
                for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                {
                    MstCategory objCategory = new MstCategory();
                    objCategory.CategoryID = Convert.ToInt32(dt.Rows[iCnt]["ID"].ToString());
                    objCategory.Category = dt.Rows[iCnt]["BOOK_CATEGORY"].ToString();
                    lstMstCategory.Add(objCategory);
                }
            }
            return lstMstCategory;
        }
        [HttpPost]
        public JsonResult GetSchoolDtlBySchoolId(Int32 SchoolId)
        {

            return Json(SchoolDetail(SchoolId));
        }

        [HttpPost]
        public JsonResult GetBookRequisitionDtl(Int32 ReqDataId, Int32 categoryId, Int32 LanguageId, Int32 schoolId = 0)
        {
            if(schoolId==-11)
                return Json(HelperBookRequisitionDtl1(ReqDataId, categoryId, LanguageId, schoolId));
            else
            return Json(HelperBookRequisitionDtl(ReqDataId, categoryId, LanguageId, schoolId));
        }

        //partha
        private List<SchRequisitionDtl> HelperBookRequisitionDtl(Int32 ReqDataId, Int32 categoryId, Int32 LanguageId, Int32 schoolId = 0)
        {

            List<SchRequisitionDtl> lstBookDtl = new List<SchRequisitionDtl>();
            try
            {
                DataTable dtReqDtl = new DataTable();
                DataTable dtBook = new DataTable();
                if (ReqDataId > 0)
                {
                    //dtReqDtl = objDbTrx.GetRequisitionDtlByReqIdNew(ReqDataId);
                    dtReqDtl = objDbTrx.GetRequisitionDtlByReqIdSimplifiedNew(ReqDataId);
                }
                if (schoolId > default(int))
                {
                    dtBook = objDbTrx.GetBookMasterDetailsByIdNew(Convert.ToInt64(schoolId), Convert.ToInt64(categoryId), Convert.ToInt64(LanguageId));
                }
                else
                {
                    dtBook = objDbTrx.GetBookMasterDetailsById(Convert.ToInt64(categoryId), Convert.ToInt64(LanguageId));
                }

                if (dtBook.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dtBook.Rows.Count; iCnt++)
                    {
                        SchRequisitionDtl rq = new SchRequisitionDtl();

                        rq.BookName = dtBook.Rows[iCnt]["BOOK_NAME"].ToString() + (Convert.ToInt32(dtBook.Rows[iCnt]["IsOptional"]) > default(int) ? " (Optional)" : "");
                        rq.BookCode = dtBook.Rows[iCnt]["BOOK_CODE"].ToString();
                        rq.IsOptional = Convert.ToInt32(dtBook.Rows[iCnt]["IsOptional"]);
                        rq.BookID = Convert.ToInt64(dtBook.Rows[iCnt]["ID"].ToString());
                        rq.ClassName = dtBook.Rows[iCnt]["CLASS"].ToString();
                        rq.CLASS_INT = Convert.ToInt32(dtBook.Rows[iCnt]["CLASS_INT"]);
                        rq.BookLock = Convert.ToBoolean(dtBook.Rows[iCnt]["Book_Lock"]);
                        if (ReqDataId > 0)
                        {
                            if (dtReqDtl.Rows.Count > 0)
                            {
                                for (int jCnt = 0; jCnt < dtReqDtl.Rows.Count; jCnt++)
                                {
                                    if (Convert.ToInt64(dtBook.Rows[iCnt]["ID"].ToString()) == Convert.ToInt64(dtReqDtl.Rows[jCnt]["BOOK_ID"].ToString()))
                                    {
                                        ////rq.StudentEnrolled = Convert.ToInt32(dtReqDtl.Rows[jCnt]["NO_OF_STUDENT_ENROLLED"].ToString());
                                        rq.StudentEnrolled = ReqDataId > default(int) ? Convert.ToInt32(dtReqDtl.Rows[jCnt]["REQUISITION_QTY"].ToString()) : Convert.ToInt32(dtReqDtl.Rows[jCnt]["PreviousYearRequirement"].ToString());
                                        rq.RequisitionQuantity = Convert.ToInt32(dtReqDtl.Rows[jCnt]["REQUISITION_QTY"].ToString());
                                        rq.StockQuantity = Convert.ToInt32(dtReqDtl.Rows[jCnt]["STOCK_QTY"].ToString());
                                        rq.PreviousYearRequirement = Convert.ToInt32(dtReqDtl.Rows[jCnt]["PreviousYearRequirement"].ToString());
                                        //break;
                                    }
                                }
                            }
                            else
                            {
                                rq.StudentEnrolled = 0;
                                rq.RequisitionQuantity = 0;
                                rq.StockQuantity = 0;
                                rq.PreviousYearRequirement = 0;
                            }
                        }
                        else
                        {
                            rq.StudentEnrolled = (schoolId > default(int)) ? Convert.ToInt32(dtBook.Rows[iCnt]["PreviousYearRequirement"].ToString()) : default(int);
                            rq.RequisitionQuantity = (schoolId > default(int)) ? Convert.ToInt32(dtBook.Rows[iCnt]["RequisitionQuantity"].ToString()) : default(int);
                            rq.StockQuantity = 0;
                            rq.PreviousYearRequirement = (schoolId > default(int)) ? Convert.ToInt32(dtBook.Rows[iCnt]["PreviousYearRequirement"].ToString()) : default(int);
                        }
                        lstBookDtl.Add(rq);
                    }
                }

            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return lstBookDtl;



        }
        private List<SchRequisitionDtlforreport> HelperBookRequisitionDtl1(Int32 ReqDataId, Int32 categoryId, Int32 LanguageId, Int32 schoolId = 0)
        {

            List<SchRequisitionDtlforreport> lstBookDtl = new List<SchRequisitionDtlforreport>();
            try
            {
                DataTable dtReqDtl = new DataTable();
                DataTable dtBook = new DataTable();
                if (ReqDataId > 0)
                {
                    //dtReqDtl = objDbTrx.GetRequisitionDtlByReqIdNew(ReqDataId);
                    dtReqDtl = objDbTrx.GetRequisitionDtlByReqIdSimplifiedNew(ReqDataId);
                }
                if (schoolId > default(int))
                {
                    dtBook = objDbTrx.GetBookMasterDetailsByIdNew(Convert.ToInt64(schoolId), Convert.ToInt64(categoryId), Convert.ToInt64(LanguageId));
                }
                else
                {
                    dtBook = objDbTrx.GetBookMasterDetailsById(Convert.ToInt64(categoryId), Convert.ToInt64(LanguageId));
                }

                if (dtBook.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dtBook.Rows.Count; iCnt++)
                    {
                        SchRequisitionDtlforreport rq = new SchRequisitionDtlforreport();
                        rq.BookType = dtBook.Rows[iCnt]["type"].ToString();
                        rq.BookName = dtBook.Rows[iCnt]["BOOK_NAME"].ToString();
                        rq.BookID = Convert.ToInt64(dtBook.Rows[iCnt]["ID"].ToString());
                        rq.ClassName = dtBook.Rows[iCnt]["CLASS"].ToString();
                        rq.BookLock = Convert.ToBoolean(dtBook.Rows[iCnt]["Book_Lock"]);
                        if (ReqDataId > 0)
                        {
                            if (dtReqDtl.Rows.Count > 0)
                            {
                                for (int jCnt = 0; jCnt < dtReqDtl.Rows.Count; jCnt++)
                                {
                                    if (Convert.ToInt64(dtBook.Rows[iCnt]["ID"].ToString()) == Convert.ToInt64(dtReqDtl.Rows[jCnt]["BOOK_ID"].ToString()))
                                    {
                                        ////rq.StudentEnrolled = Convert.ToInt32(dtReqDtl.Rows[jCnt]["NO_OF_STUDENT_ENROLLED"].ToString());
                                        rq.StudentEnrolled = ReqDataId > default(int) ? Convert.ToInt32(dtReqDtl.Rows[jCnt]["REQUISITION_QTY"].ToString()) : Convert.ToInt32(dtReqDtl.Rows[jCnt]["PreviousYearRequirement"].ToString());
                                        rq.RequisitionQuantity = Convert.ToInt32(dtReqDtl.Rows[jCnt]["REQUISITION_QTY"].ToString());
                                        rq.StockQuantity = Convert.ToInt32(dtReqDtl.Rows[jCnt]["STOCK_QTY"].ToString());
                                        rq.PreviousYearRequirement = Convert.ToInt32(dtReqDtl.Rows[jCnt]["PreviousYearRequirement"].ToString());
                                        //break;
                                    }
                                }
                            }
                            else
                            {
                                rq.StudentEnrolled = 0;
                                rq.RequisitionQuantity = 0;
                                rq.StockQuantity = 0;
                                rq.PreviousYearRequirement = 0;
                            }
                        }
                        else
                        {
                            rq.StudentEnrolled = (schoolId > default(int)) ? Convert.ToInt32(dtBook.Rows[iCnt]["PreviousYearRequirement"].ToString()) : default(int);
                            rq.RequisitionQuantity = (schoolId > default(int)) ? Convert.ToInt32(dtBook.Rows[iCnt]["RequisitionQuantity"].ToString()) : default(int);
                            rq.StockQuantity = 0;
                            rq.PreviousYearRequirement = (schoolId > default(int)) ? Convert.ToInt32(dtBook.Rows[iCnt]["PreviousYearRequirement"].ToString()) : default(int);
                        }
                        lstBookDtl.Add(rq);
                    }
                }

            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return lstBookDtl;



        }

        private List<MstSchool> SchoolDetail(Int32 SchoolId)
        {
            List<MstSchool> lstMstSchool = new List<MstSchool>();
            try
            {
                DataTable dt = objDbTrx.GetSchoolMasterDetailsBySchoolId(SchoolId);
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        MstSchool objSchool = new MstSchool();
                        objSchool.SchoolID = Convert.ToInt32(dt.Rows[iCnt]["ID"].ToString());
                        objSchool.SchoolName = dt.Rows[iCnt]["SCHOOL_NAME"].ToString();
                        objSchool.SchoolEmailid = dt.Rows[iCnt]["SCHOOL_EMAIL_ID"].ToString();
                        objSchool.SchoolMobile = dt.Rows[iCnt]["SCHOOL_PHONE_NO"].ToString();
                        objSchool.SchoolCode = dt.Rows[iCnt]["SCHOOL_CODE"].ToString();

                        lstMstSchool.Add(objSchool);
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return lstMstSchool;
        }

        private SchRequisition SchRequisitionDtl(Int64? ReqisitionId)
        {

            SchRequisition objSchRequisition = new SchRequisition();
            if (ReqisitionId == null || ReqisitionId <= 0)
            {
                objSchRequisition.RequisitionDate = DateTime.Now.ToString("dd-MMM-yyyy").ToUpper();
                objSchRequisition.RequisitionCode = "REQ/XX/XXXXXXX";
                objSchRequisition.RequisitionID = -1;
            }
            else if (ReqisitionId > 0)
            {
                try
                {
                    //DataTable dtRequisition = objDbTrx.GetSchRequisitionByReqId(Convert.ToInt64(ReqisitionId));
                    DataTable dtRequisition = objDbTrx.GetRequisitionViewDataByReqId(ReqisitionId.ToString());
                    if (dtRequisition.Rows.Count > 0)
                    {
                        objSchRequisition.RequisitionDate = Convert.ToDateTime(dtRequisition.Rows[0]["REQUISITION_DATE"]).ToString("dd-MMM-yyyy");
                        objSchRequisition.RequisitionCode = dtRequisition.Rows[0]["REQ_CODE"].ToString();
                        objSchRequisition.RequisitionID = Convert.ToInt64(dtRequisition.Rows[0]["REQUISITION_ID"].ToString());
                        objSchRequisition.SchoolID = Convert.ToInt64(dtRequisition.Rows[0]["SCHOOL_ID"].ToString());
                        objSchRequisition.LanguageID = Convert.ToInt16(dtRequisition.Rows[0]["LANGUAGE_ID"].ToString());
                        objSchRequisition.CategoryID = Convert.ToInt16(dtRequisition.Rows[0]["CATEGORY_ID"].ToString());
                        objSchRequisition.SaveStatus = Convert.ToInt16(dtRequisition.Rows[0]["SAVE_STATUS"].ToString());

                        objSchRequisition.MstLanguage = new MstLanguage() { LanguageID = objSchRequisition.LanguageID };
                        objSchRequisition.MstCategory = new MstCategory() { CategoryID = objSchRequisition.CategoryID };
                        objSchRequisition.MstSchool = new MstSchool() { SchoolID = objSchRequisition.SchoolID };
                    }
                }
                catch (Exception ex)
                {
                    TempData["AppMessage"] = "Some Error has occurred while performing your activity. Please contact the System Administrator for further assistance.";
                    objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
                }
            }

            return objSchRequisition;

        }






        //partha


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
