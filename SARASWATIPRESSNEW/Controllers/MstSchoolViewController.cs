using SARASWATIPRESSNEW.BusinessLogicLayer;
using SARASWATIPRESSNEW.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SARASWATIPRESSNEW.Controllers
{
    public class MstSchoolViewController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx(); 
        public ActionResult Index()
        {
            ViewBag.Active = "MstSchoolView";                  
            return View();
        }
        public ActionResult GetSchoolMasterListData(string startDate, string endDate)
        {
            List<MstSchool> objMstSchoolList = new List<MstSchool>();
            try
            {
                Int16 CircleId =Convert.ToInt16(((UserSec)Session["UserSec"]).CircleID);
                DataTable dtSchool = objDbTrx.GetSchoolMasterDetailsByCircleId(CircleId);
                if (dtSchool.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dtSchool.Rows.Count; iCnt++)
                    {
                        MstSchool objMstSchool = new MstSchool();
                        objMstSchool.SchoolID = Convert.ToInt32(dtSchool.Rows[iCnt]["ID"].ToString());
                        objMstSchool.CircleId = Convert.ToInt32(dtSchool.Rows[iCnt]["CIRCLE_ID"].ToString());

                        objMstSchool.SchoolCode = dtSchool.Rows[iCnt]["SCHOOL_CODE"].ToString();
                        objMstSchool.SchoolName = dtSchool.Rows[iCnt]["SCHOOL_NAME"].ToString();
                        objMstSchool.SchoolAdrees = dtSchool.Rows[iCnt]["SCHOOL_ADDRESS"].ToString();
                        objMstSchool.SchoolEmailid = dtSchool.Rows[iCnt]["SCHOOL_EMAIL_ID"].ToString();
                        objMstSchool.SchoolMobile = dtSchool.Rows[iCnt]["SCHOOL_PHONE_NO"].ToString();
                        objMstSchool.SchoolAlternateMobile = dtSchool.Rows[iCnt]["SCHOOL_ALT_PHONE_NO"].ToString();
                        objMstSchool.DeleivaryAddress = dtSchool.Rows[iCnt]["SCHOOL_DELIVARY_ADDRESS"].ToString();
                        objMstSchool.PoliceStation = dtSchool.Rows[iCnt]["POLICE_STATION"].ToString();
                        objMstSchool.PostalCode = dtSchool.Rows[iCnt]["POSTAL_CODE"].ToString();
                        objMstSchoolList.Add(objMstSchool);
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(objMstSchoolList, JsonRequestBehavior.AllowGet);
            //  return View();
        }
        public JsonResult DeleteSchool(string DataUniqueId)
        {           
            string ErrorMessage = "";
            try
            {
                DataTable dtSchool = objDbTrx.GetSchoolMasterDetailsBySchoolId(Convert.ToInt64(DataUniqueId));
                if (dtSchool.Rows.Count > 0)
                {
                    ErrorMessage = "The School Name " + dtSchool.Rows[0]["SCHOOL_NAME"].ToString() + " and School Code " + dtSchool.Rows[0]["SCHOOL_NAME"].ToString() + "  deleted successfully";
                }
                dtSchool.Dispose();
                dtSchool.Clear();
                objDbTrx.DeleteInSchool(Convert.ToInt32(DataUniqueId));                
            }
            catch (Exception ex)
            {
                ErrorMessage = "Some error occured while deleting requisition. please contact system administrator for further assitence.";
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(ErrorMessage);
        }



        public JsonResult isSchoolReferenceRecordExist(Int32 DataUniqueId)
        {
            DataCheck objDataCheck = new DataCheck();
            objDataCheck.DataCount = 0;
            try
            {
                DataTable dt = objDbTrx.IsSchoolRecordExistInRefTable(DataUniqueId);
                if (dt.Rows.Count > 0)
                {
                    objDataCheck.DataCount = dt.Rows.Count;
                }
                dt.Dispose();
                dt.Clear();
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(objDataCheck, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExportSchoolData()
        {


            try
            {

                StringBuilder strTableReport = new StringBuilder();
                StringBuilder strReport = new StringBuilder();
                Int16 CircleId =Convert.ToInt16(((UserSec)Session["UserSec"]).CircleID);
                DataTable SchoolMasterDetails = objDbTrx.GetSchoolMasterDetailsByCircleId(CircleId);
                if (SchoolMasterDetails.Rows.Count > 0)
                {
                    strReport.AppendLine("<tr>");                   
                    strReport.AppendLine("  <th style='text-align:left;background-color:#b3cbff;' >School Code</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:#b3cbff;' >School Name</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:#b3cbff;' >School Address</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:#b3cbff;' >Email</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:#b3cbff;' >Mobile</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:#b3cbff;' >Alternate Mobile</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:#b3cbff;' >Delivary Address</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:#b3cbff;' >Policae Station</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:#b3cbff;' >Postal Code</th>");
                    strReport.AppendLine("</tr>");
                    for (int iCnt = 0; iCnt < SchoolMasterDetails.Rows.Count; iCnt++)
                    {                      

                        strReport.AppendLine("<tr>");
                        strReport.AppendLine("      <td style='text-align:left;'> " + SchoolMasterDetails.Rows[iCnt]["SCHOOL_CODE"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + SchoolMasterDetails.Rows[iCnt]["SCHOOL_NAME"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + SchoolMasterDetails.Rows[iCnt]["SCHOOL_ADDRESS"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + SchoolMasterDetails.Rows[iCnt]["SCHOOL_EMAIL_ID"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + SchoolMasterDetails.Rows[iCnt]["SCHOOL_PHONE_NO"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + SchoolMasterDetails.Rows[iCnt]["SCHOOL_ALT_PHONE_NO"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + SchoolMasterDetails.Rows[iCnt]["SCHOOL_DELIVARY_ADDRESS"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + SchoolMasterDetails.Rows[iCnt]["POLICE_STATION"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + SchoolMasterDetails.Rows[iCnt]["POSTAL_CODE"].ToString() + "      </td>");
                        strReport.AppendLine("</tr>");

                    }
                    strTableReport.AppendLine("<table border='1'>");
                    strTableReport.AppendLine("          " + strReport.ToString());
                    strTableReport.AppendLine("</table>");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    String FileName = "SchoolData" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".xls";

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



                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return View();

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
