using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SARASWATIPRESSNEW.Models;
using System.Collections;
using SARASWATIPRESSNEW.BusinessLogicLayer;
using System.IO;

namespace SARASWATIPRESSNEW.Controllers
{
    [SessionAuthorize]
    public class StockUpdateController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index(string a, string b)
        {
            string CircleId = "";
            try { CircleId = GlobalSettings.oUserData.CircleID; }
            catch { CircleId = ""; }

            if (CircleId == "")
            {
                return RedirectToAction("Index", "CircleLogin");
            }
            else
            {
                return View(get_language(a, b));
            }
        }

        [HttpGet]
        public Models.StockUpdate get_language(string CategoryID, string LanguageID)
        {
            //CategoryID, Int64 LanguageID, Int64 CircleID
            StockUpdate lst_req = new StockUpdate();
            List<Language> lst_language = new List<Language>();
            try
            {
                DataTable dtMastData = objDbTrx.GetLanguageMasterDetails();
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
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            List<Category> lst_category = new List<Category>();
            try
            {
                //DataTable dtMastData = objDbTrx.GetBookCategoryMasterDetails();
                DataTable dtMastData = objDbTrx.GetChallanBookCeategory();
                if (dtMastData.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dtMastData.Rows.Count; iCnt++)
                    {
                        Category rq = new Category();
                        rq.Category_name = Convert.ToString(dtMastData.Rows[iCnt]["CHALLAN_BOOK_CATEGORY"].ToString());
                        rq.CategoryID = Convert.ToInt32(dtMastData.Rows[iCnt]["ID"].ToString());
                        lst_category.Add(rq);
                    }
                    dtMastData.Dispose();
                }
                lst_req.categoryCollection = lst_category;
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }


            if (Convert.ToString(CategoryID) != null && Convert.ToString(LanguageID) != null)
            {
                lst_req.cat_id = Convert.ToString(CategoryID.ToString());
                lst_req.lan_id = Convert.ToString(LanguageID.ToString());
            }
            else
            {
                lst_req.cat_id = "Select";
                lst_req.lan_id = "Select";
            }

            return lst_req;
        }

        [HttpGet]
        [ActionName("GetReqList")]
        public PartialViewResult GetReqList(long catid, long langid)
        {
            StockUpdate lst_req = new StockUpdate();
            List<StockTrxDtl> lst_requisition = new List<StockTrxDtl>();
            try
            {
                #region [GetStockLockDetails]

                string objSsn = ((SARASWATIPRESSNEW.UserSec)System.Web.HttpContext.Current.Session["UserSec"]) != null ? ((SARASWATIPRESSNEW.UserSec)System.Web.HttpContext.Current.Session["UserSec"]).CircleID : default(int).ToString();
                int _circleId = Convert.ToInt32(objSsn);

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
                        //objCircleLock.ReqLockDate = DateTime.Parse(dtCircleLock.Rows[i]["Stock_lock"].ToString());
                        lstCircleLock.Add(objCircleLock);
                    }

                    //lstCircleLock.RemoveAll(c => !c.Req_year.Contains("/19"));

                    ViewBag.StockLock = lstCircleLock.FirstOrDefault() != null ? lstCircleLock.FirstOrDefault().Stock_lock : default(int).ToString();
                }

                #endregion [GetStockLockDetails]

                DataTable dtMastData = objDbTrx.GetReqStokDetails(catid, langid, _circleId);
                //dtMastData = dtMastData.AsEnumerable().OrderBy(r => r.Field<int>("ID")).CopyToDataTable();
                if (dtMastData.Rows.Count > 0)
                {
                    ViewBag.IsAlreadyConfirmed = dtMastData.Rows[0]["ISCONFIRMED"] != null && !string.IsNullOrWhiteSpace(dtMastData.Rows[0]["ISCONFIRMED"].ToString()) ? Convert.ToInt32(dtMastData.Rows[0]["ISCONFIRMED"].ToString()) : default(int);
                    //ViewBag.IsAlreadyConfirmed = 0;
                    for (int i = 0; i < dtMastData.Rows.Count; i++)
                    {
                        StockTrxDtl rq = new StockTrxDtl();
                        rq.AutoID = Convert.ToInt64(!string.IsNullOrWhiteSpace(dtMastData.Rows[i]["CIRCLE_STOCK_UPDATE_AUTO_ID"].ToString()) ? dtMastData.Rows[i]["CIRCLE_STOCK_UPDATE_AUTO_ID"].ToString() : "0");

                        rq.BookName = Convert.ToString(dtMastData.Rows[i]["BOOK_NAME"].ToString());
                        rq.BOOK_CODE = dtMastData.Rows[i]["BOOK_CODE"].ToString();
                        rq.BookID = Convert.ToInt32(dtMastData.Rows[i]["ID"].ToString());
                        rq.tot = 0;
                        rq.StockUpdateQuantity = 0;
                        if (Convert.ToString(dtMastData.Rows[i]["TOT"]).Trim() != "")
                        {
                            rq.tot = Convert.ToInt16(dtMastData.Rows[i]["TOT"].ToString());
                        }
                        if (Convert.ToString(dtMastData.Rows[i]["STOCK_UPDATE_QTY"]).Trim() != "")
                        {
                            rq.StockUpdateQuantity = Convert.ToInt16(dtMastData.Rows[i]["STOCK_UPDATE_QTY"].ToString());
                        }
                        if (Convert.ToString(dtMastData.Rows[i]["STOCK_DAMAGE_QTY"]).Trim() != "")
                        {
                            rq.StockDamageQuantity = Convert.ToInt16(dtMastData.Rows[i]["STOCK_DAMAGE_QTY"].ToString());
                        }
                        if (Convert.ToString(dtMastData.Rows[i]["STOCK_DAMAGE_QTY_AFTERCONF"]).Trim() != "")
                        {
                            rq.STOCK_DAMAGE_QTY_AFTERCONF = Convert.ToInt16(dtMastData.Rows[i]["STOCK_DAMAGE_QTY_AFTERCONF"].ToString());
                        }
                        if (Convert.ToString(dtMastData.Rows[i]["STOCK_UPDATE_TIMESTAMP"]).Trim() != "")
                        {
                            rq.TimeStamp = dtMastData.Rows[i]["STOCK_UPDATE_TIMESTAMP"].ToString();
                        }
                        else
                        {
                            rq.TimeStamp = DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");
                        }
                        rq.ISCONFIRMED = Convert.ToInt32(Convert.ToString(dtMastData.Rows[i]["ISCONFIRMED"] != null && !string.IsNullOrWhiteSpace(dtMastData.Rows[i]["ISCONFIRMED"].ToString()) ? dtMastData.Rows[i]["ISCONFIRMED"] : default(int)));
                        try
                        {
                            var noOfBooks = Convert.ToInt16(rq.tot);
                            var stockUpdateQuantity = Convert.ToInt16(rq.StockUpdateQuantity);
                            var stockDamageQty = Convert.ToInt16(rq.StockDamageQuantity);
                            var balance = 0;
                            if (stockUpdateQuantity < noOfBooks)
                            {
                                balance = (Convert.ToInt16(rq.tot) - Convert.ToInt16(Math.Abs(Convert.ToInt32(rq.StockUpdateQuantity) - Convert.ToInt32(rq.StockDamageQuantity))));
                            }

                            rq.Balance = balance;
                        }
                        catch { rq.Balance = 0; }
                        lst_requisition.Add(rq);
                    }
                }
                lst_req.reqStockCollection = lst_requisition;
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }

            //return Json(new { lst_req.reqStockCollection }, JsonRequestBehavior.AllowGet);
            return PartialView("_reqCollection", lst_req);
        }

        [HttpPost]
        [ActionName("SubmitStockUpdate")]
        public JsonResult SubmitStockUpdate(List<CircleStockUpdate> jData)
        {
            int _isConfirmed = default(int);
            bool returnVal = default(bool);
            try
            {
                string objSsn = ((SARASWATIPRESSNEW.UserSec)System.Web.HttpContext.Current.Session["UserSec"]) != null ? ((SARASWATIPRESSNEW.UserSec)System.Web.HttpContext.Current.Session["UserSec"]).CircleID : default(int).ToString();
                string objUser = ((SARASWATIPRESSNEW.UserSec)System.Web.HttpContext.Current.Session["UserSec"]) != null ? ((SARASWATIPRESSNEW.UserSec)System.Web.HttpContext.Current.Session["UserSec"]).UserId : string.Empty;
                int _circleId = Convert.ToInt32(objSsn);

                if (jData != null && jData.Count() > default(int))
                {
                    _isConfirmed = jData[0].ISCONFIRMED;
                    jData.ForEach(jd =>
                    {
                        jd.CIRCLE_ID = _circleId;
                        jd.CREATED_BY = objUser;
                        jd.UPDATED_BY = objUser;
                        jd.STOCK_UPDATE_TIMESTAMP = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    });

                    string xml = Utility.CreateXmlTraditional(Utility.ToDataTable<CircleStockUpdate>(jData)).InnerXml;
                    returnVal = objDbTrx.InsertUpdateCircleStockUpdate(xml, _isConfirmed);

                    if (returnVal)
                        return Json(new { Success = 1, Message = "Data saved successfully" }, JsonRequestBehavior.AllowGet);
                    else
                        throw new Exception("Data cannot be saved. Please contact to system administrator");
                }
                else
                    throw new Exception("No data found for update");
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult Index(StockUpdate objcust)
        {
            if (ModelState.IsValid)
            {
                if (objcust.stat == true)
                {
                    try
                    {
                        string circleId = "";
                        try
                        {
                            circleId = GlobalSettings.oUserData.CircleID;
                        }
                        catch { }
                        if (circleId != "")
                        {
                            objDbTrx.InsertInReqStokDetails(objcust, Convert.ToInt64(circleId));
                            Response.Write("<script> alert ('Data has been submitted successfully...') </script> ");
                        }
                        else
                        {
                            RedirectToAction("Index", "CircleLogin");
                        }
                    }
                    catch (Exception ex)
                    {
                        objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
                    }
                    finally
                    {

                    }

                }
            }

            return RedirectToAction("Index", "StockUpdate");
        }

        [HttpPost]
        [ActionName("SubmitStockDamage")]
        public JsonResult SubmitStockDamage(List<CircleStockUpdate> jData)
        {
            bool returnVal = default(bool);
            try
            {
                string objSsn = ((SARASWATIPRESSNEW.UserSec)System.Web.HttpContext.Current.Session["UserSec"]) != null ? ((SARASWATIPRESSNEW.UserSec)System.Web.HttpContext.Current.Session["UserSec"]).CircleID : default(int).ToString();
                string objUser = ((SARASWATIPRESSNEW.UserSec)System.Web.HttpContext.Current.Session["UserSec"]) != null ? ((SARASWATIPRESSNEW.UserSec)System.Web.HttpContext.Current.Session["UserSec"]).UserId : string.Empty;
                int _circleId = Convert.ToInt32(objSsn);

                if (jData != null && jData.Count() > default(int))
                {
                    jData.ForEach(jd =>
                    {
                        jd.CIRCLE_ID = _circleId;
                        jd.CREATED_BY = objUser;
                        jd.UPDATED_BY = objUser;
                    });

                    string xml = Utility.CreateXmlTraditional(Utility.ToDataTable<CircleStockUpdate>(jData)).InnerXml;
                    returnVal = objDbTrx.InsertUpdateCircleStockDamage(xml);

                    if (returnVal)
                        return Json(new { Success = 1, Message = "Data saved successfully" }, JsonRequestBehavior.AllowGet);
                    else
                        throw new Exception("Data cannot be saved. Please contact to system administrator");
                }
                else
                    throw new Exception("No data found for update");
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
