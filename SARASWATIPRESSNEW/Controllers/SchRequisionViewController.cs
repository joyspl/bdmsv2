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
    public class SchRequisionViewController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            ViewBag.Active = "SchRequisionView";
            SchRequisitionView objSchRequisitionView = new SchRequisitionView();
            objSchRequisitionView.StartDate = Convert.ToDateTime(DateTime.Now).ToString("dd-MMM-yyyy");
            objSchRequisitionView.EndDate = Convert.ToDateTime(DateTime.Now).ToString("dd-MMM-yyyy");
            return View(objSchRequisitionView);
        }

        public JsonResult GetSchRequisionViewListDataToExcel(string startDate, string endDate, string circleId = "")
        {
            SARASWATIPRESSNEW.UserSec objUser;
            try
            {
                objUser = Session["UserSec"] != null ? ((SARASWATIPRESSNEW.UserSec)Session["UserSec"]) : new SARASWATIPRESSNEW.UserSec();
            }
            catch (Exception)
            {
                objUser = new SARASWATIPRESSNEW.UserSec();
            }
            string usertype = objUser.UserType;
           

            string filename = string.Empty;
            try
            {
                clsDirectoryDeleteStatus clsDirectoryDeleteStatus = new clsDirectoryDeleteStatus
                {
                    status = true,
                    StatusMessage = "Init"
                };
                string str = Server.MapPath("~/Report/Requisition/");
                string text2 = str + ((UserSec)Session["UserSec"]).UserUniqueId + "\\";
                //Int16 CircleId = Convert.ToInt16(!string.IsNullOrWhiteSpace(((UserSec)Session["UserSec"]).CircleID) ? ((UserSec)Session["UserSec"]).CircleID : "0");
                Int16 CircleId = default(short);
                if (!string.IsNullOrWhiteSpace(circleId) && circleId != "-1")
                {
                    CircleId = Convert.ToInt16(circleId);
                }
                else
                {
                    CircleId = Convert.ToInt16(!string.IsNullOrWhiteSpace(((UserSec)Session["UserSec"]).CircleID) ? ((UserSec)Session["UserSec"]).CircleID : "0");
                }
                Int16 AccadYear = Convert.ToInt16(((UserSec)Session["UserSec"]).AcademicYearId);
                DataTable dt = objDbTrx.GetRequisitionViewNew(Convert.ToDateTime(startDate + " 00:00:00.000"), Convert.ToDateTime(endDate + " 23:59:59.999"), CircleId, AccadYear);
                if (dt != null && dt.Rows.Count > default(int))
                {
                    //var a = string.Join(",", dt.Columns.AsEnumerable().Select(column => column.ColumnName).ToArray());
                    dt.Columns.Add("STATUS", typeof(String));
                    
                    dt.Columns.Add("DISTRICT APPROVAL", typeof(String));
                    dt.Columns.Add("DIRECTOR APPROVAL", typeof(String));
                    dt.Columns.Add("ADMIN APPROVAL", typeof(String));
                    
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["STATUS"] = Convert.ToInt32(dt.Rows[i]["SAVE_STATUS"].ToString()) == 1 ? "Confirmed" : "Yet to confirm";
                        
                        dt.Rows[i]["DISTRICT APPROVAL"] = Convert.ToInt32(dt.Rows[i]["ISAPPROVED_DIST"].ToString()) == 1 ? "Approved" : "Pending";
                        dt.Rows[i]["DIRECTOR APPROVAL"] = Convert.ToInt32(dt.Rows[i]["ISAPPROVED"].ToString()) == 1 ? "Approved" : "Pending";
                        dt.Rows[i]["ADMIN APPROVAL"] = Convert.ToInt32(dt.Rows[i]["ISAPPROVED_ADMIN"].ToString()) == 1 ? "Approved" : "Pending";

                    }

                    if (Directory.Exists(text2))
                    {
                        clsDirectoryDeleteStatus = Utility.DeleteDirectory(text2);
                    }

                    if(usertype=="7")
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
                            "CIRCLE_ID",
                            "SCHOOL_ID",
                            "CATEGORY_ID",
                            "LANGUAGE_ID",
                            "CREATED_TS",
                            "CREATED_BY",
                            "UPDATED_TS",
                            "UPDATED_BY",
                            "REQ_SESSION_CODE",
                            "ISAPPROVED_DIST",
                            "APPROVED_BY_DIST",
                            "APPROVED_TS_DIST",
                            "ISAPPROVED_ADMIN"

                            
					    };

                            dt.Columns["REQ_CODE"].ColumnName = "REQUISITION_CODE";
                            //dt.Columns["ISAPPROVED"].ColumnName = "DIRECTOR APPROVAL";

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
                            "CIRCLE_ID",
                            "SCHOOL_ID",
                            "CATEGORY_ID",
                            "LANGUAGE_ID",
                            "CREATED_TS",
                            "CREATED_BY",
                            "UPDATED_TS",
                            "UPDATED_BY",
                            "ISAPPROVED_ADMIN",
                            "REQ_SESSION_CODE",
                            "ISAPPROVED_DIST",
                            "APPROVED_BY_DIST",
                            "APPROVED_TS_DIST",
                            "ADMIN APPROVAL"

                            
					    };

                            dt.Columns["REQ_CODE"].ColumnName = "REQUISITION_CODE";
                            //dt.Columns["ISAPPROVED"].ColumnName = "DIRECTOR APPROVAL";

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
                string fullPath = Path.Combine(Server.MapPath(string.Format("~/Report/Requisition/{0}", ((UserSec)Session["UserSec"]).UserUniqueId)), file);
                return File(Utility.FileAsByte(fullPath), System.Net.Mime.MediaTypeNames.Application.Octet, file);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult GetSchRequisionViewListData(string startDate, string endDate, string circleId = "")
        {
            List<SchRequisitionView> objSchRequisitionViewList = new List<SchRequisitionView>();
            try
            {
                Int16 CircleId = default(short);
                if (!string.IsNullOrWhiteSpace(circleId) && circleId != "-1")
                {
                    CircleId = Convert.ToInt16(circleId);
                }
                else
                {
                    CircleId = Convert.ToInt16(((UserSec)Session["UserSec"]).CircleID);
                }
                Int16 AccadYear = Convert.ToInt16(((UserSec)Session["UserSec"]).AcademicYearId);
                //DataTable dt = objDbTrx.GetSchRequisitionViewDataByCercleId(Convert.ToDateTime(startDate + " 00:00:00.000"), Convert.ToDateTime(endDate + " 23:59:59.999"), CircleId, AccadYear);
                DataTable dt = objDbTrx.GetRequisitionViewNew(Convert.ToDateTime(startDate + " 00:00:00.000"), Convert.ToDateTime(endDate + " 23:59:59.999"), CircleId, AccadYear);
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        SchRequisitionView objSchRequisitionView = new SchRequisitionView();
                        objSchRequisitionView.RequisitionID = Convert.ToInt64(dt.Rows[iCnt]["REQUISITION_ID"].ToString());
                        objSchRequisitionView.RequisitionCode = Convert.ToString(dt.Rows[iCnt]["REQ_CODE"].ToString());
                        objSchRequisitionView.RequisitionDate = Convert.ToDateTime(dt.Rows[iCnt]["REQUISITION_DATE"].ToString()).ToString("dd-MMM-yyyy");
                        objSchRequisitionView.SaveStatus = Convert.ToInt16(dt.Rows[iCnt]["SAVE_STATUS"].ToString());
                        objSchRequisitionView.SchoolCode = Convert.ToString(dt.Rows[iCnt]["SCHOOL_CODE"].ToString());
                        objSchRequisitionView.SchoolName = Convert.ToString(dt.Rows[iCnt]["SCHOOL_NAME"].ToString());
                        objSchRequisitionView.LanguageName = Convert.ToString(dt.Rows[iCnt]["LANGUAGE"].ToString());
                        objSchRequisitionView.Category = Convert.ToString(dt.Rows[iCnt]["BOOK_CATEGORY"].ToString());
                        objSchRequisitionView.LastUpdatedBy = Convert.ToString(dt.Rows[iCnt]["UPDATED_BY"].ToString());
                        objSchRequisitionView.LastUpdatedOn = Convert.ToDateTime(dt.Rows[iCnt]["UPDATED_TS"].ToString()).ToString("dd-MMM-yyyy");
                        objSchRequisitionView.ISAPPROVED = Convert.ToInt32(dt.Rows[iCnt]["ISAPPROVED"].ToString());
                        objSchRequisitionView.ISAPPROVED_DIST = Convert.ToInt32(dt.Rows[iCnt]["ISAPPROVED_DIST"].ToString());
                        objSchRequisitionView.ISAPPROVED_ADMIN = Convert.ToInt32(dt.Rows[iCnt]["ISAPPROVED_ADMIN"].ToString());
                        objSchRequisitionViewList.Add(objSchRequisitionView);
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(objSchRequisitionViewList, JsonRequestBehavior.AllowGet);
            //  return View();
        }
        [HttpPost]
        public JsonResult DeleteRequisition(string ReqisitionId)
        {
            string ErrorMessage = "";
            try
            {

                ////DataTable dtRequisition = objDbTrx.GetSchRequisitionByReqId(Convert.ToInt64(ReqisitionId));
                //DataTable dtRequisition = objDbTrx.GetRequisitionDtlByReqIdNew(Convert.ToInt64(ReqisitionId));
                DataTable dtRequisition = objDbTrx.GetRequisitionByReqIDNew(Convert.ToInt64(ReqisitionId));
                if (dtRequisition.Rows.Count > 0)
                {
                    ErrorMessage = "The Requisition " + dtRequisition.Rows[0]["REQ_CODE"].ToString() + " and Requisition date " + Convert.ToDateTime(dtRequisition.Rows[0]["REQUISITION_DATE"]).ToString("dd-MMM-yyyy") + "  deleted successfully";
                }
                dtRequisition.Dispose();
                dtRequisition.Clear();
                //objDbTrx.DeleteSchRequisition(ReqisitionId);
                objDbTrx.DeleteRequisitionNew(ReqisitionId);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Some error occured while deleting requisition. please contact system administrator for further assitence.";
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(ErrorMessage);
        }
        [HttpPost]
        public JsonResult ConfirmRequisition(string griddata)
        {
            string[] ChallanIds = griddata.TrimEnd(',').Split(',');
            string ErrorMessage = "";
            try
            {
                Requisition objRequisition = new Requisition();
                objRequisition.UserId = ((UserSec)Session["UserSec"]).UserId;
                objRequisition.SaveStatus = "1";
                objDbTrx.RequisitionConfirm(objRequisition, griddata.TrimEnd(','));
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