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
    public class ChallanReceivedAtCircleController : Controller
    {
        //
        // GET: /ChallanReceivedAtCircle/
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            string CircleId = "-1";
            try { CircleId = GlobalSettings.oUserData.CircleID; }
            catch { CircleId = "-1"; }
            InvoiceCumChallan objChallan = new InvoiceCumChallan();
            objChallan.IsPendingRequire = true;
            objChallan.CircleId = Convert.ToInt16(CircleId);
            return View(objChallan);
        }
        [HttpPost]
        public JsonResult GetChallanViewData(string startDate, string endDate, string PendingOnly)
        {
            List<InvoiceCumChallan> objChallanList = new List<InvoiceCumChallan>();
            try
            {
                string CircleId = "-1";
                try { CircleId = GlobalSettings.oUserData.CircleID; }
                catch { CircleId = "-1"; }

                DataTable dt = objDbTrx.GetChallanDtlByCircleId(startDate, endDate, CircleId, (PendingOnly.ToUpper() == "TRUE" ? "1" : "0"));
                if (dt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        InvoiceCumChallan icc = new InvoiceCumChallan();
                        icc.ChallanId = Convert.ToInt64(dt.Rows[iCnt]["ID"].ToString());
                        icc.InvoiceCumChallanNo = Convert.ToString(dt.Rows[iCnt]["Challan_Number"].ToString());
                        icc.CircleId = Convert.ToInt32(dt.Rows[iCnt]["CircleId"].ToString());
                        icc.InvoiceCumChallanDate = Convert.ToDateTime(dt.Rows[iCnt]["Challan_Date"].ToString()).ToString("dd-MMM-yyyy");
                        icc.CircleName = Convert.ToString(dt.Rows[iCnt]["Circle_Name"].ToString());
                        icc.CategoryName = Convert.ToString(dt.Rows[iCnt]["CHALLAN_BOOK_CATEGORY"].ToString());
                        icc.DistrictName = dt.Rows[iCnt]["DISTRICT"].ToString();
                        icc.Language = Convert.ToString(dt.Rows[iCnt]["LANGUAGE"].ToString());
                        icc.Transporter = Convert.ToString(dt.Rows[iCnt]["Transport_Name"].ToString());
                        icc.CONSIGNEE_NO = Convert.ToString(dt.Rows[iCnt]["CONSIGNEE_NO"].ToString());
                        icc.VEHICLE_NO = Convert.ToString(dt.Rows[iCnt]["VEHICLE_NO"].ToString());
                        icc.ReceivedAtCircle = Convert.ToString(dt.Rows[iCnt]["RECEIVED_AT_CIRCLE"].ToString());
                        icc.ReceivedBy = "";

                        // -- 4.12.18 Anik da part

                        //try
                        //{
                        //    var dtx = objDbTrx.GetCircleChallanCommentById_SeparateTable(Convert.ToInt32(dt.Rows[iCnt]["ID"].ToString()));
                        //    if (dtx .Rows.Count > 0)
                        //    {
                        //        icc.ChallanComment = Convert.ToString(dt.Rows[0]["ChallanComment"].ToString());
                        //    }
                        //}
                        //catch
                        //{
                        //    icc.ChallanComment = string.Empty;
                        //}

                        icc.ChallanComment = Convert.ToString(dt.Rows[iCnt]["ChallanComment"].ToString());
                        icc.ReceivedTimeStamp = "";
                        if (icc.ReceivedAtCircle == "1")
                        {
                            icc.ReceivedBy = Convert.ToString(dt.Rows[iCnt]["RECEIVED_BY"].ToString());
                            icc.ReceivedTimeStamp = Convert.ToDateTime(dt.Rows[iCnt]["RECEIVED_TS"].ToString()).ToString("dd-MMM-yyyy");
                        }
                        objChallanList.Add(icc);
                    }

                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(objChallanList);
        }
        [HttpPost]
        public JsonResult ReceiveChallan(string griddata, string ReceiveDate)
        {
            string[] ChallanIds = griddata.Split(',');
            string challanId = "";
            string ErrorMsg = "";
            try
            {
                for (int i = 0; i < ChallanIds.Count(); i++)
                {
                    try
                    {
                        challanId = Convert.ToInt32(ChallanIds[i]).ToString();
                        objDbTrx.UpdateCircleChallanReceived(challanId, GlobalSettings.oUserData.UserId, ReceiveDate);
                    }
                    catch (Exception ex) { ErrorMsg += ErrorMsg + " " + ex; }
                }
                if (ErrorMsg == "")
                {
                    ErrorMsg = "Challan has been received successfully.....";
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(ErrorMsg);
        }

        [HttpPost]
        public JsonResult SaveComment(CommentSaveRequest postdata)
        {
            int cId = default(int);
            string ErrorMsg = "";
            try
            {
                int.TryParse(postdata.ChallanID, out cId);
                //objDbTrx.UpdateCircleChallanCommentById_SeparateTable(cId, postdata.Comment, GlobalSettings.oUserData.UserId);

                objDbTrx.UpdateCircleChallanCommentById(cId, postdata.Comment, GlobalSettings.oUserData.UserId);
                if (ErrorMsg == "")
                {
                    ErrorMsg = "Comment has been saved successfully";
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(ErrorMsg);
        }
    }
}
