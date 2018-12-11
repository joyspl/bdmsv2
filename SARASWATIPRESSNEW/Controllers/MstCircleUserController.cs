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
    public class MstCircleUserController : Controller
    {
        //
        // GET: /MstCircleUser/
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            List<CircleUser> lstMstCircleuser = new List<CircleUser>();
            try
            {
                DataTable dtMstCircleusr = objDbTrx.GetCircleUser();
                if (dtMstCircleusr.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dtMstCircleusr.Rows.Count; iCnt++)
                    {
                        CircleUser objMstCircleUsr = new CircleUser();
                      //  objMstCircleUsr.circlename = Convert.ToInt16(dtMstCircleusr.Rows[iCnt]["ID"].ToString());
                        objMstCircleUsr.CircleUserID = Convert.ToInt16(dtMstCircleusr.Rows[iCnt]["CIRCLE_ID"].ToString());
                        objMstCircleUsr.circleName = dtMstCircleusr.Rows[iCnt]["CIRCLE_NAME"].ToString();
                        objMstCircleUsr.MobileNo = dtMstCircleusr.Rows[iCnt]["MOBILE_NO"].ToString();
                        objMstCircleUsr.CircleOfficerName = dtMstCircleusr.Rows[iCnt]["CIRCLE_OFFICER_NAME"].ToString();
                        objMstCircleUsr.EmailId = dtMstCircleusr.Rows[iCnt]["EMAIL_ID"].ToString();
                        objMstCircleUsr.Address = dtMstCircleusr.Rows[iCnt]["CIRCLE_ADDRESS"].ToString();
                        objMstCircleUsr.Userid = dtMstCircleusr.Rows[iCnt]["USER_ID"].ToString();
                        objMstCircleUsr.Password = dtMstCircleusr.Rows[iCnt]["PASSWORD"].ToString();
                        objMstCircleUsr.active = Convert.ToBoolean(dtMstCircleusr.Rows[iCnt]["ACTIVE"].ToString());
                        objMstCircleUsr.flag = Convert.ToBoolean(dtMstCircleusr.Rows[iCnt]["flag"].ToString());
                        objMstCircleUsr.CirclePinCode = dtMstCircleusr.Rows[iCnt]["Circle_PinCode"].ToString();
                        lstMstCircleuser.Add(objMstCircleUsr);

                    }
                }

            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return View(lstMstCircleuser);  
        }

        public ActionResult AddCircleUsers()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCircleUsers(CircleUser objCircleusers)
        {
            try
            {
                bool isUpdated = objDbTrx.InsertInMstCircleUser1(objCircleusers);
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return RedirectToAction("Index", "MstCircleUser");
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

        public ActionResult ExportCircleUserData()
        {


            try
            {

                StringBuilder strTableReport = new StringBuilder();
                StringBuilder strReport = new StringBuilder();

                DataTable dt = objDbTrx.GetCircleUser();
                if (dt.Rows.Count > 0)
                {
                    strReport.AppendLine("<tr>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >Circle Id</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >Circle Name</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >CIRCLE OFFICER NAME</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >MOBILE NO</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >EMAIL ID</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >CIRCLE ADDRESS</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >USER ID</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >PASSWORD</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >Active</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >flag</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >Circle PinCode</th>");
                    strReport.AppendLine("</tr>");
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        strReport.AppendLine("<tr>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["CIRCLE_ID"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["CIRCLE_NAME"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["CIRCLE_OFFICER_NAME"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["MOBILE_NO"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["EMAIL_ID"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["CIRCLE_ADDRESS"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["USER_ID"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["PASSWORD"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["ACTIVE"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["flag"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["Circle_PinCode"].ToString() + "      </td>");
                        strReport.AppendLine("</tr>");

                    }
                    strTableReport.AppendLine("<table border='2'>");
                    strTableReport.AppendLine("          " + strReport.ToString());
                    strTableReport.AppendLine("</table>");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    String FileName = "CircleUserData" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".xls";

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



    }
}
