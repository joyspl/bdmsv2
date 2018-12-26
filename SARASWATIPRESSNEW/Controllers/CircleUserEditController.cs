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
    [SessionAuthorize]
    public class CircleUserEditController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            string CircleId = "";
            try { CircleId = GlobalSettings.oUserData.CircleID; }
            catch { CircleId = ""; }
            if (CircleId != "")
            {
                return View(get_info());
            }

            else
            {
                return RedirectToAction("Index", "CircleLogin");
            }
        }

        [HttpPost]
        public ActionResult Index(CircleUser objcust)
        {
            if (ModelState.IsValid)
            {
                string CircleId = "";
                try { CircleId = GlobalSettings.oUserData.CircleID; }
                catch { CircleId = ""; }

                if (CircleId != "")
                {
                    try          
                    {
                        objcust.Userid = Convert.ToString(GlobalSettings.oUserData.UserId);
                        objDbTrx.UpdateInCericleUser(objcust);
                        TempData["Message"] = "Profile Successfully Updated";                        
                    }
                    catch (Exception ex)
                    {
                        TempData["Message"] = "Some Error occured while updating Profile."; 
                        objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
                    }
                    finally
                    {
                        
                    }
                }
                else
                {
                    return RedirectToAction("Index", "CircleLogin");
                }

            }
            return View();
        }

        [HttpGet]
        public Models.CircleMaster get_info()
        {
            CircleMaster lst_req = new CircleMaster();
            try
            {
                
                //DataTable dtCircleUserDtl = objDbTrx.GetCircleUserMasterDetailsById(Convert.ToInt32(GlobalSettings.oUserData.CircleID));
                DataTable dtCircleUserDtl = objDbTrx.GetCircleMasterDetailsByCircleIdNew(Convert.ToInt32(GlobalSettings.oUserData.CircleID));
                if (dtCircleUserDtl.Rows.Count > 0)
                {
                    CircleMaster rq = new CircleMaster();
                    rq.CircleID = Convert.ToInt32(dtCircleUserDtl.Rows[0]["ID"]);
                    rq.CircleOfficerName = Convert.ToString(dtCircleUserDtl.Rows[0]["CIRCLE_OFFICER_NAME"]);
                    rq.Address = Convert.ToString(dtCircleUserDtl.Rows[0]["CIRCLE_ADDRESS"]);
                    rq.CirclePinCode = Convert.ToString(dtCircleUserDtl.Rows[0]["Circle_Pincode"]);
                    rq.PoliceStation = Convert.ToString(dtCircleUserDtl.Rows[0]["POLICE_STATION"]);
                    rq.EmailId = Convert.ToString(dtCircleUserDtl.Rows[0]["EMAIL_ID"]);
                    rq.MobileNo = Convert.ToString(dtCircleUserDtl.Rows[0]["MOBILE_NO"]);
                    rq.AlternateMobileNo = Convert.ToString(dtCircleUserDtl.Rows[0]["ALTERNATE_MOBILE_NO"]);
                    rq.Active = Convert.ToInt32(dtCircleUserDtl.Rows[0]["ACTIVE"] != null && !string.IsNullOrWhiteSpace(dtCircleUserDtl.Rows[0]["ACTIVE"].ToString()) ? dtCircleUserDtl.Rows[0]["ACTIVE"].ToString() : "1");
                    //rq.ActiveStatus = rq.Active > default(int) ? true : false;
                    lst_req = rq;
                }
                
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return lst_req;
        }

        [HttpPost]
        [ActionName("SaveProfileData")]
        public JsonResult SaveProfileData(CircleMaster jData)
        {
            bool result = default(bool);
            try
            {
                //Code to insert/update data for Circle User
                if (jData.CircleID > default(int))
                {
                    //For update to DB
                    //result = objDbTrx.UpdateInCericleUser(jData);
                    result = objDbTrx.UpdateInCericleUserNew(jData);

                    string smsBody = "";
                    DataTable dt = objDbTrx.GetCircleDtilById(jData.CircleID);
                    System.Threading.ThreadPool.QueueUserWorkItem(s =>
                    {
                        Utility.SendSMS(dt.Rows[0]["MOBILE_NO"].ToString(), smsBody);
                        if (dt.Rows[0]["ALTERNATE_MOBILE_NO"] != null && !string.IsNullOrWhiteSpace(dt.Rows[0]["ALTERNATE_MOBILE_NO"].ToString()))
                        {
                            Utility.SendSMS(dt.Rows[0]["ALTERNATE_MOBILE_NO"].ToString(), smsBody);
                        }
                    });
                }
                else
                {
                    //code for insert to DB
                }
                return Json(new {
                    Success = result ? 1 : 0,
                    Message = result ? string.Format("Profile data have been {0} successfully in our system.", (jData.CircleID > default(int) ? "updated" : "inserted")) : "DB operation error detected."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
