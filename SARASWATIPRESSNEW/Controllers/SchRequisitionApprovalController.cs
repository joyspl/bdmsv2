using SARASWATIPRESSNEW.BusinessLogicLayer;
using SARASWATIPRESSNEW.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SARASWATIPRESSNEW.Controllers
{
    [SessionAuthorize]
    public class SchRequisitionApprovalController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            ViewBag.Active = "SchRequisitionApproval";
            SchRequisitionView objSchRequisitionView = new SchRequisitionView();
            objSchRequisitionView.StartDate = Convert.ToDateTime(DateTime.Now).ToString("dd-MMM-yyyy");
            objSchRequisitionView.EndDate = Convert.ToDateTime(DateTime.Now).ToString("dd-MMM-yyyy");
            return View(objSchRequisitionView);
        }

        public ActionResult GetSchRequisionViewListData(string startDate, string endDate, string districtId = "", string circleId = "", int isapproved = 0)
        {
            List<SchRequisitionView> objSchRequisitionViewList = new List<SchRequisitionView>();
            try
            {
                int CircleId = default(int);
                int DistrictId = default(int);
                int.TryParse(districtId, out DistrictId);
                string usertype = string.Empty;
                try
                {
                    usertype = (GlobalSettings.oUserData).UserType;
                }
                catch { }
                if (!string.IsNullOrWhiteSpace(circleId))
                {
                    CircleId = Convert.ToInt32(circleId);
                }
                else
                {
                    if (usertype != "2")
                        CircleId = Convert.ToInt32(!string.IsNullOrWhiteSpace(GlobalSettings.oUserData.CircleID) ? GlobalSettings.oUserData.CircleID : "-1");
                    else
                        CircleId = -1;
                }

                int IsForAdminApproval = default(int);
                int AdminApprovalstatus = default(int);

                

                int AccadYear = Convert.ToInt32(GlobalSettings.oUserData.AcademicYearId);
               // DataTable dt = objDbTrx.GetRequisitionViewNewForApproval(Convert.ToDateTime(startDate + " 00:00:00.000"), Convert.ToDateTime(endDate + " 23:59:59.999"), DistrictId, CircleId, (usertype == "5" ? isapproved : 0), AccadYear, IsForDistApproval: (usertype == "5" ? 0 : (usertype == "2" ? 1 : 0)), DistApprovalstatus: (usertype == "2" ? isapproved : 0));
                DataTable dt = objDbTrx.GetRequisitionViewNewForApproval(Convert.ToDateTime(startDate + " 00:00:00.000")
                    , Convert.ToDateTime(endDate + " 23:59:59.999"), DistrictId, CircleId
                    , (usertype == "5" ? isapproved : 0), AccadYear, IsForDistApproval: (usertype == "5" ? 0 : (usertype == "2" ? 1 : 0))
                    , DistApprovalstatus: (usertype == "2" ? isapproved : 0), IsForAdminApproval: (usertype == "5" ? 0 : (usertype == "3" ? 1 : 0)), AdminApprovalstatus: (usertype == "5" ? 0 : (usertype == "3" ? isapproved : 0)));
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        var pp = dt.Rows[iCnt]["ISAPPROVED_ADMIN"];
                        Console.WriteLine(pp);

                        SchRequisitionView objSchRequisitionView = new SchRequisitionView();
                        objSchRequisitionView.RequisitionID = Convert.ToInt64(dt.Rows[iCnt]["REQUISITION_ID"].ToString());
                        try
                        {
                            objSchRequisitionView.ISAPPROVED = Convert.ToInt32(dt.Rows[iCnt]["ISAPPROVED"].ToString());
                            objSchRequisitionView.APPROVED_BY = dt.Rows[iCnt]["APPROVED_BY"].ToString();
                            if (!string.IsNullOrWhiteSpace(dt.Rows[iCnt]["APPROVED_TS"].ToString()))
                            {
                                objSchRequisitionView.APPROVED_TS = Convert.ToDateTime(dt.Rows[iCnt]["APPROVED_TS"].ToString());
                            }
                        }
                        catch
                        {
                            continue;
                        }
                        try
                        {
                            objSchRequisitionView.ISAPPROVED_DIST = Convert.ToInt32(dt.Rows[iCnt]["ISAPPROVED_DIST"].ToString());
                            objSchRequisitionView.APPROVED_BY_DIST = dt.Rows[iCnt]["APPROVED_BY_DIST"].ToString();
                            if (!string.IsNullOrWhiteSpace(dt.Rows[iCnt]["APPROVED_TS_DIST"].ToString()))
                            {
                                objSchRequisitionView.APPROVED_TS_DIST = Convert.ToDateTime(dt.Rows[iCnt]["APPROVED_TS_DIST"].ToString());
                            }
                        }
                        catch
                        {
                            continue;
                        }
                        objSchRequisitionView.RequisitionCode = Convert.ToString(dt.Rows[iCnt]["REQ_CODE"].ToString());
                        objSchRequisitionView.RequisitionDate = Convert.ToDateTime(dt.Rows[iCnt]["REQUISITION_DATE"].ToString()).ToString("dd-MMM-yyyy");
                        objSchRequisitionView.SaveStatus = Convert.ToInt16(dt.Rows[iCnt]["SAVE_STATUS"].ToString());
                        objSchRequisitionView.SchoolCode = Convert.ToString(dt.Rows[iCnt]["SCHOOL_CODE"].ToString());
                        objSchRequisitionView.SchoolName = Convert.ToString(dt.Rows[iCnt]["SCHOOL_NAME"].ToString());
                        objSchRequisitionView.LanguageName = Convert.ToString(dt.Rows[iCnt]["LANGUAGE"].ToString());
                        objSchRequisitionView.Category = Convert.ToString(dt.Rows[iCnt]["BOOK_CATEGORY"].ToString());
                        objSchRequisitionView.LastUpdatedBy = Convert.ToString(dt.Rows[iCnt]["UPDATED_BY"].ToString());
                        objSchRequisitionView.LastUpdatedOn = Convert.ToDateTime(dt.Rows[iCnt]["UPDATED_TS"].ToString()).ToString("dd-MMM-yyyy");

                        // 4.12.18 Admin approval section
                        try
                        {
                            objSchRequisitionView.ISAPPROVED_ADMIN = Convert.ToInt32(dt.Rows[iCnt]["ISAPPROVED_ADMIN"].ToString());
                            
                        }
                        catch
                        {
                            continue;
                        }

                        
                        objSchRequisitionViewList.Add(objSchRequisitionView);
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            //return Json(objSchRequisitionViewList, JsonRequestBehavior.AllowGet);
            return PartialView("_ReqList", objSchRequisitionViewList);
        }

        public JsonResult GetSchRequisionViewListDataToExcel(string startDate, string endDate, string districtId = "", string circleId = "", int isapproved = 0)
        {
            string filename = string.Empty;
            try
            {
                clsDirectoryDeleteStatus clsDirectoryDeleteStatus = new clsDirectoryDeleteStatus
                {
                    status = true,
                    StatusMessage = "Init"
                };
                string str = Server.MapPath("~/Report/Requisition/");
                string text2 = str + GlobalSettings.oUserData.UserUniqueId + "\\";
                int CircleId = default(int);
                int DistrictId = default(int);
                int.TryParse(districtId, out DistrictId);
                string usertype = string.Empty;
                try
                {
                    usertype = (GlobalSettings.oUserData).UserType;
                }
                catch { }
                if (!string.IsNullOrWhiteSpace(circleId))
                {
                    CircleId = Convert.ToInt32(circleId);
                }
                else
                {
                    CircleId = Convert.ToInt32(!string.IsNullOrWhiteSpace(GlobalSettings.oUserData.CircleID) ? GlobalSettings.oUserData.CircleID : "-1");
                }
                Int16 AccadYear = Convert.ToInt16(GlobalSettings.oUserData.AcademicYearId);
                //DataTable dt = objDbTrx.GetRequisitionViewNewForApproval(Convert.ToDateTime(startDate + " 00:00:00.000"), Convert.ToDateTime(endDate + " 23:59:59.999"), DistrictId, CircleId, isapproved, AccadYear);
                DataTable dt = objDbTrx.GetRequisitionViewNewForApproval(Convert.ToDateTime(startDate + " 00:00:00.000")
                    , Convert.ToDateTime(endDate + " 23:59:59.999"), DistrictId, CircleId
                    , (usertype == "5" ? isapproved : 0), AccadYear, IsForDistApproval: (usertype == "5" ? 0 : (usertype == "2" ? 1 : 0))
                    , DistApprovalstatus: (usertype == "2" ? isapproved : 0)  , IsForAdminApproval: (usertype == "5" ? 0 : (usertype == "3" ? 1 : 0)), AdminApprovalstatus: (usertype == "5" ? 0 : (usertype == "3" ? isapproved : 0)));
                if (dt != null && dt.Rows.Count > default(int))
                {
                    
                    dt.Columns.Add("DISTRICT_APPROVAL_STATUS", typeof(String));
                    dt.Columns.Add("DIRECTOR_APPROVAL_STATUS", typeof(String));
                   // dt.Columns.Add("ADMIN_APPROVAL_STATUS", typeof(String));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["DISTRICT_APPROVAL_STATUS"] = Convert.ToInt32(dt.Rows[i]["ISAPPROVED_DIST"].ToString()) == 1 ? "Approved" : "Not Approved";
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["DIRECTOR_APPROVAL_STATUS"] = Convert.ToInt32(dt.Rows[i]["ISAPPROVED"].ToString()) == 1 ? "Approved" : "Not Approved";
                    }
                    

                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    dt.Rows[i]["ADMIN_APPROVAL_STATUS"] = Convert.ToInt32(dt.Rows[i]["ISAPPROVED_ADMIN"].ToString()) == 1 ? "Approved" : "Not Approved";
                    //}

                    if (Directory.Exists(text2))
                    {
                        clsDirectoryDeleteStatus = Utility.DeleteDirectory(text2);
                    }

                    if (usertype=="3") // admin
                    {
                        if (clsDirectoryDeleteStatus.status)
                        {
                            List<string> deleteColumnsList = new List<string>
					    {
						    "REQUISITION_ID",
                            "SAVE_STATUS",
                            "ISAPPROVED",
                            "APPROVED_BY",
                            "APPROVED_TS",
                            "ISAPPROVED_DIST",
                            "APPROVED_BY_DIST",
                            "APPROVED_TS_DIST",
                            "CIRCLE_ID",
                            "SCHOOL_ID",
                            "CATEGORY_ID",
                            "LANGUAGE_ID",
                            "CREATED_TS",
                            "CREATED_BY",
                            "UPDATED_TS",
                            "UPDATED_BY",
                            "REQ_SESSION_CODE"
                            //"ISAPPROVED_ADMIN"
					    };

                            dt.Columns["REQ_CODE"].ColumnName = "REQUISITION_CODE";

                            string text = DateTime.Now.ToString("ddMMyyyyHHmm");
                            Directory.CreateDirectory(text2);
                            Utility.GenerateExcel2007(text2 + text + ".xlsx", dt, deleteColumnsList);

                            filename = text;
                        }
                    }

                    else
                    {
                        if (clsDirectoryDeleteStatus.status)
                        {
                            List<string> deleteColumnsList = new List<string>
					    {
						    "REQUISITION_ID",
                            "SAVE_STATUS",
                            "ISAPPROVED",
                            "APPROVED_BY",
                            "APPROVED_TS",
                            "ISAPPROVED_DIST",
                            "APPROVED_BY_DIST",
                            "APPROVED_TS_DIST",
                            "CIRCLE_ID",
                            "SCHOOL_ID",
                            "CATEGORY_ID",
                            "LANGUAGE_ID",
                            "CREATED_TS",
                            "CREATED_BY",
                            "UPDATED_TS",
                            "UPDATED_BY",
                            "REQ_SESSION_CODE",
                            "ISAPPROVED_ADMIN"
                            //"ADMIN_APPROVAL_STATUS"
					    };

                            dt.Columns["REQ_CODE"].ColumnName = "REQUISITION_CODE";

                            string text = DateTime.Now.ToString("ddMMyyyyHHmm");
                            Directory.CreateDirectory(text2);
                            Utility.GenerateExcel2007(text2 + text + ".xlsx", dt, deleteColumnsList);

                            filename = text;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(new { Filename = filename }, JsonRequestBehavior.AllowGet);
        }

        public FileResult Download(string file)
        {
            try
            {
                file = file + ".xlsx";
                string fullPath = Path.Combine(Server.MapPath(string.Format("~/Report/Requisition/{0}", GlobalSettings.oUserData.UserUniqueId)), file);
                return File(Utility.FileAsByte(fullPath), System.Net.Mime.MediaTypeNames.Application.Octet, file);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult RequisitionApproval(string griddata)
        {
            string[] ChallanIds = griddata.TrimEnd(',').Split(',');
            string ErrorMessage = "";
            string usertype = string.Empty;
            int isfordistrictApproval = default(int);
            try
            {
                usertype = (GlobalSettings.oUserData).UserType;
                Requisition objRequisition = new Requisition();
                objRequisition.UserId = GlobalSettings.oUserData.UserId;
                objRequisition.ISAPPROVED_DIST = (usertype == "2" ? 1 : default(int));
                objRequisition.ISAPPROVED = (usertype == "5" ? 1 : default(int));
                isfordistrictApproval = (usertype == "2" ? 1 : default(int));

                // 4.12.18 Admin approval

                objRequisition.ISAPPROVED_ADMIN = (usertype == "3" ? 1 : default(int));

                objDbTrx.RequisitionApproval(objRequisition, griddata.TrimEnd(','), isfordistrictApproval);
                ErrorMessage = ChallanIds.Count() + " Requisition confirmed successfully.";
            }
            catch (Exception ex)
            {
                ErrorMessage = "Some Error occured while confirming Requisition. Please confirm system administrator";
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(ErrorMessage);
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
