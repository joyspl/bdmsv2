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
    public class MstCircleController : Controller
    {
        //
        // GET: /MstCircle/
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            List<MstCircle> lstMstCircle = new List<MstCircle>();
            try
            {
                DataTable dtMstCircle = objDbTrx.GetCircleMasterDetails();
                if (dtMstCircle.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dtMstCircle.Rows.Count; iCnt++)
                    {
                        MstCircle objMstCircle = new MstCircle();
                        objMstCircle.DistrictId = Convert.ToInt16(dtMstCircle.Rows[iCnt]["DISTRICT_ID"].ToString());
                        objMstCircle.DistrictName = dtMstCircle.Rows[iCnt]["DISTRICT"].ToString();
                        objMstCircle.CircleCode = dtMstCircle.Rows[iCnt]["CIRCLE_CODE"].ToString();
                        objMstCircle.CircleName = dtMstCircle.Rows[iCnt]["CIRCLE_NAME"].ToString();
                        objMstCircle.CircleId = Convert.ToInt16(dtMstCircle.Rows[iCnt]["ID"].ToString());
                        lstMstCircle.Add(objMstCircle);

                    }
                }

            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return View(lstMstCircle);
        }

        public ActionResult CircleUpdate(Int16? DataUniqueId)
        {
            if (DataUniqueId != null)
            {
                DataTable dtCircleInfo = new DataTable();
                dtCircleInfo = objDbTrx.GetCircleDtilById(Convert.ToInt16(DataUniqueId));
                MstCircle objCircle = new MstCircle();
                objCircle.CircleId = Convert.ToInt16(dtCircleInfo.Rows[0]["ID"]);
                objCircle.DistrictId = Convert.ToInt16(dtCircleInfo.Rows[0]["DISTRICT_ID"]);
                objCircle.CircleCode = dtCircleInfo.Rows[0]["CIRCLE_CODE"].ToString();
                objCircle.CircleName = dtCircleInfo.Rows[0]["CIRCLE_NAME"].ToString();
                return View(objCircle);
            }
            return View();
        }

       
        [HttpPost]
        public ActionResult CircleUpdate(MstCircle objCircle)
        {
            try
            {
               
                if (objCircle.CircleId == null || objCircle.CircleId <= 0)
                {
                    bool isUpdated = objDbTrx.InsertInMstCircle(objCircle);
                    TempData["Message"] = "Information Saved Successfully";
                }
                else {
                    bool isUpdated = objDbTrx.UpdateInMstCircle(objCircle);
                    TempData["Message"] = "Information Updated Successfully";
                }
                
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            
            return RedirectToAction("Index", "MstCircle");
        }

        public ActionResult DeleteCircle(int DataUniqueId)
        {
            try
            {
                bool isDelete = objDbTrx.DeleteInCircle(DataUniqueId);                
                TempData["Message"] = "Information Deleted Successfully";
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return RedirectToAction("Index", "MstCircle");
        }
        public ActionResult EditCircle(int DataUniqueId)
        {
            return RedirectToAction("CircleUpdate", new { DataUniqueId = Convert.ToInt16(DataUniqueId) });
        }

        public ActionResult ExportCircleData()
        {


            try
            {

                StringBuilder strTableReport = new StringBuilder();
                StringBuilder strReport = new StringBuilder();

                DataTable dt = objDbTrx.GetCircleMasterDetails();
                if (dt.Rows.Count > 0)
                {
                    strReport.AppendLine("<tr>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >Circle Code</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >Circle Name</th>");
                    strReport.AppendLine("  <th style='text-align:left;background-color:b3cbff;' >District Name</th>");
                    strReport.AppendLine("</tr>");
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        strReport.AppendLine("<tr>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["CIRCLE_CODE"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["CIRCLE_NAME"].ToString() + "      </td>");
                        strReport.AppendLine("      <td> " + dt.Rows[iCnt]["DISTRICT"].ToString() + "      </td>");
                        strReport.AppendLine("</tr>");

                    }
                    strTableReport.AppendLine("<table border='1'>");
                    strTableReport.AppendLine("          " + strReport.ToString());
                    strTableReport.AppendLine("</table>");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    String FileName = "CircleData" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".xls";

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

        [HttpPost]
        public JsonResult GetDistrictDetails()
        {

            List<MstCircle> ObjMstCircle = new List<MstCircle>();
            try
            {
                DataTable dt = objDbTrx.GetDistrictDetails();
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        MstCircle objCircle = new MstCircle();
                        objCircle.DistrictId = Convert.ToInt16(dt.Rows[iCnt]["ID"].ToString());
                        objCircle.DistrictName = dt.Rows[iCnt]["DISTRICT"].ToString();
                        ObjMstCircle.Add(objCircle);
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(ObjMstCircle);
        }


        public JsonResult isCircleReferenceRecordExist(int DataUniqueId)
        {
            DataCheck objDataCheck = new DataCheck();
            objDataCheck.DataCount = 0;
            try
            {
                DataTable dt = objDbTrx.IsCircleRecordExistInRefTable(DataUniqueId);
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

        public JsonResult isDuplicateRecordExistInCircle(int CircleID, string CircleCode)
        {
             List<MstCircle> lstMstCircle = new List<MstCircle>();
            try
            {
                DataTable dtMstCircle = objDbTrx.isDuplicateRecordExistInCircle(CircleID, CircleCode);
                if (dtMstCircle.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dtMstCircle.Rows.Count; iCnt++)
                    {
                        MstCircle objMstCircle = new MstCircle();
                        objMstCircle.DistrictId = Convert.ToInt16(dtMstCircle.Rows[iCnt]["DISTRICT_ID"].ToString());
                        objMstCircle.DistrictName = dtMstCircle.Rows[iCnt]["DISTRICT"].ToString();
                        objMstCircle.CircleCode = dtMstCircle.Rows[iCnt]["CIRCLE_CODE"].ToString();
                        objMstCircle.CircleName = dtMstCircle.Rows[iCnt]["CIRCLE_NAME"].ToString();
                        objMstCircle.CircleId = Convert.ToInt16(dtMstCircle.Rows[iCnt]["ID"].ToString());
                        lstMstCircle.Add(objMstCircle);

                    }
                }          

            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(lstMstCircle, JsonRequestBehavior.AllowGet);
        }
        

    }
}
