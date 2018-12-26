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
    public class InterCircleTrnfController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();

        public ActionResult Index()
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
                return View();
            }
        }

        [HttpPost]
        public ActionResult SaveTrnf(string jData)
        {
            IEnumerable<CircleStockUpdate> pData = new List<CircleStockUpdate>();
            try
            {
                string unescapedStr = HttpUtility.UrlDecode(jData);
                pData = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<CircleStockUpdate>>(unescapedStr);
                if (pData != null && pData.Count() > default(int))
                {
                    pData.ToList().ForEach(item =>
                    {
                        item.TMP_ORGN_CIRCLE = Convert.ToInt32(GlobalSettings.oUserData.CircleID);
                    });

                    string xml = Utility.CreateXmlTraditional(Utility.ToDataTable<CircleStockUpdate>(pData.ToList())).InnerXml;
                    var result = objDbTrx.InsertUpdateCircleStockUpdateTrnf(xml);
                    if (result)
                        return Json(new { Success = 1, Message = "Data saved successfully" }, JsonRequestBehavior.AllowGet);
                    else
                        throw new Exception("Data cannot be saved. Please contact to system administrator");
                }
                else
                {
                    throw new Exception("No data found for update");
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return null;
        }

        [HttpGet]
        [ActionName("GetReqListTrnf")]
        public PartialViewResult GetReqListTrnf(int distid, int destcrclid, long catid, long langid)
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

                DataTable dtMastData = objDbTrx.GetReqStokDetailsWithTrnf(catid, langid, _circleId, destcrclid);
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
                        if (Convert.ToString(dtMastData.Rows[i]["TOTAL_TRNF_BOOKS"]).Trim() != "")
                        {
                            rq.TOTAL_TRNF_BOOKS = Convert.ToInt32(dtMastData.Rows[i]["TOTAL_TRNF_BOOKS"].ToString());
                        }
                        if (Convert.ToString(dtMastData.Rows[i]["TMP_ORGN_CIRCLE"]).Trim() != "")
                        {
                            rq.TMP_ORGN_CIRCLE = Convert.ToInt32(dtMastData.Rows[i]["TMP_ORGN_CIRCLE"].ToString());
                        }
                        if (Convert.ToString(dtMastData.Rows[i]["TMP_DESTN_CIRCLE"]).Trim() != "")
                        {
                            rq.TMP_DESTN_CIRCLE = Convert.ToInt32(dtMastData.Rows[i]["TMP_DESTN_CIRCLE"].ToString());
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
            return PartialView("_reqCollectionTrnf", lst_req);
        }

        [HttpGet]
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
            return Json(lstMstLanguage, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
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

        [HttpGet]
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
            return Json(lstMstCategory, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private List<MstCategory> GetCategoryList()
        {
            List<MstCategory> lstMstCategory = new List<MstCategory>();
            //DataTable dt = objDbTrx.GetBookCategoryMasterDetails();
            DataTable dt = objDbTrx.GetChallanBookCeategory();
            if (dt.Rows.Count > 0)
            {
                for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                {
                    MstCategory objCategory = new MstCategory();
                    objCategory.CategoryID = Convert.ToInt32(dt.Rows[iCnt]["ID"].ToString());
                    objCategory.Category = dt.Rows[iCnt]["CHALLAN_BOOK_CATEGORY"].ToString();
                    lstMstCategory.Add(objCategory);
                }
            }
            return lstMstCategory;
        }
    }
}
