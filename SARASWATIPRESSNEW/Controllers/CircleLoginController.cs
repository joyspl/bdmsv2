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
namespace SARASWATIPRESSNEW.Controllers
{
    public class CircleLoginController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            //DataTable dtAccademicYear = new DataTable();
            //dtAccademicYear = objDbTrx.GetAcademicYearDtl();
            //List<SelectListItem> liAccademicYear = new List<SelectListItem>();
            //for (int rows = 0; rows <= dtAccademicYear.Rows.Count - 1; rows++)
            //{
            //    liAccademicYear.Add(new SelectListItem { Text = dtAccademicYear.Rows[rows]["ACAD_YEAR"].ToString(), Value = dtAccademicYear.Rows[rows]["ID"].ToString() });
            //    if (rows == 1) { break; }
            //}
           // ViewData["AccadmicYearList"] = liAccademicYear;

            // modified 25.12.18
            DataTable dtAccademicYear = new DataTable();
            dtAccademicYear = objDbTrx.GetAllAcademicYear();
            List<SelectListItem> liAccademicYear = new List<SelectListItem>();
            for (int rows = 0; rows <= dtAccademicYear.Rows.Count - 1; rows++)
            {
                liAccademicYear.Add(new SelectListItem { Text = dtAccademicYear.Rows[rows]["ACAD_YEAR"].ToString(), Value = dtAccademicYear.Rows[rows]["ID"].ToString() });
                //if (rows == 1) { break; }
            }

            ViewData["AccadmicYearList"] = liAccademicYear;

            /*try
            {
                Session.Clear();
                Session.Abandon();
            }
            catch { }*/

            return View();
        }

        [HttpPost]
        public ActionResult Index(UserLogin objcust)
        {
            /*var isNavigateToUpdateProfile = default(bool);*/
            if (ModelState.IsValid)
            {
                try
                {
                    DataTable dt = objDbTrx.GetBDMSLoginDtl(objcust.UserName, objcust.UserPassword, objcust.AccadmicYear);
                    if (dt.Rows.Count > 0)
                    {
                        DataTable dtRefInfo = new DataTable();
                        UserSec objUser = new UserSec();
                        objUser.UserType = dt.Rows[0]["USER_TYPE"].ToString();
                        objUser.UserId = dt.Rows[0]["USER_ID"].ToString();
                        objUser.DisplayName = dt.Rows[0]["DISPLAY_NAME"].ToString();
                        objUser.UserUniqueId = dt.Rows[0]["ID"].ToString();
                        objUser.AcademicYearId = objcust.AccadmicYear;
                        objUser.AcademicYear = dt.Rows[0]["ACAD_YEAR"].ToString();
                        objUser.HasReqEditPermission = Convert.ToInt32(dt.Rows[0]["HasReqEditPermission"].ToString());
                        objUser.HasChallanRevertPermission = Convert.ToInt32(dt.Rows[0]["HasChallanRevert"].ToString());
                        objUser.CircleID = "";
                        objUser.DistrictID = "";
                        objUser.CircleName = "";
                        objUser.DistrictNname = "";
                        if (objUser.UserType == "1")//Circle User
                        {
                            Session["REF_CRCL_ID_NEW"] = Convert.ToInt32(dt.Rows[0]["REF_ID"].ToString());
                            dtRefInfo = objDbTrx.GetCircleDtilById(Convert.ToInt32(dt.Rows[0]["REF_ID"].ToString()));
                            if (dtRefInfo.Rows.Count > 0)
                            {
                                objUser.CircleID = dtRefInfo.Rows[0]["ID"].ToString();
                                objUser.DistrictID = dtRefInfo.Rows[0]["DISTRICT_ID"].ToString();
                                objUser.CircleName = dtRefInfo.Rows[0]["CIRCLE_NAME"].ToString();
                                objUser.DistrictNname = dtRefInfo.Rows[0]["DISTRICT"].ToString();

                                /*if (string.IsNullOrWhiteSpace(dtRefInfo.Rows[0]["EMAIL_ID"].ToString())
                                    || string.IsNullOrWhiteSpace(dtRefInfo.Rows[0]["MOBILE_NO"].ToString())
                                    || string.IsNullOrWhiteSpace(dtRefInfo.Rows[0]["ALTERNATE_MOBILE_NO"].ToString())
                                    || string.IsNullOrWhiteSpace(dtRefInfo.Rows[0]["CIRCLE_OFFICER_NAME"].ToString()))
                                {
                                    isNavigateToUpdateProfile = true;
                                }
                                else
                                {
                                    isNavigateToUpdateProfile = default(bool);
                                }*/
                            }
                        }
                        if (objUser.UserType == "2")//District User User
                        {
                            dtRefInfo = objDbTrx.GetCircleDtilById(Convert.ToInt32(dt.Rows[0]["REF_ID"].ToString()));

                            if (dtRefInfo.Rows.Count > 0)
                            {
                                objUser.CircleID = dtRefInfo.Rows[0]["ID"].ToString();
                                objUser.CircleName = dtRefInfo.Rows[0]["CIRCLE_NAME"].ToString();

                                try
                                {
                                    DataTable dtbl = objDbTrx.GetCircleDtilByDistId(Convert.ToInt32(dt.Rows[0]["REF_ID"].ToString()));
                                    if (dtbl.Rows.Count > 0)
                                    {
                                        objUser.DistrictID = dtbl.Rows[0]["DISTRICT_ID"].ToString();
                                        objUser.DistrictNname = dtbl.Rows[0]["DISTRICT"].ToString();
                                    }
                                    else
                                    {
                                        objUser.DistrictID = dtRefInfo.Rows[0]["DISTRICT_ID"].ToString();
                                        objUser.DistrictNname = dtRefInfo.Rows[0]["DISTRICT"].ToString();
                                    }
                                }
                                catch (Exception)
                                {
                                    objUser.DistrictID = dtRefInfo.Rows[0]["DISTRICT_ID"].ToString();
                                    objUser.DistrictNname = dtRefInfo.Rows[0]["DISTRICT"].ToString();
                                }
                            }
                            else
                            {
                                dtRefInfo = objDbTrx.GetCircleDtilByDistId(Convert.ToInt32(dt.Rows[0]["REF_ID"].ToString()));
                                if (dtRefInfo.Rows.Count > 0)
                                {
                                    objUser.DistrictID = dtRefInfo.Rows[0]["DISTRICT_ID"].ToString();
                                    objUser.DistrictNname = dtRefInfo.Rows[0]["DISTRICT"].ToString();
                                }
                            }
                        }
                        Session["UserSec"] = objUser;
                        GlobalSettings.oUserData = objUser;
                        if (objUser.UserType == "1")//Circle User
                        {
                            Session["BDMSLoginType"] = "CIRCLE";
                        }
                        else if (objUser.UserType == "2")//District
                        {
                            Session["BDMSLoginType"] = "DISTRICT";
                            return RedirectToAction("Index", "SchRequisitionApproval");
                        }
                        else if (objUser.UserType == "3") //ADMIN
                        {
                            Session["BDMSLoginType"] = "ADMIN";
                        }
                        else if (objUser.UserType == "4") //TB Login
                        {
                            Session["BDMSLoginType"] = "TBLOGIN";
                        }
                        else if (objUser.UserType == "5") //DIRECORATE
                        {
                            Session["BDMSLoginType"] = "DIRECORATE";
                            return RedirectToAction("Index", "SchRequisitionApproval");
                        }
                        else if (objUser.UserType == "6") //TRANSPORTER
                        {
                            Session["BDMSLoginType"] = "TRANSPORTER";
                        }
                        else if (objUser.UserType == "7") //TRANSPORTER
                        {
                            Session["BDMSLoginType"] = "CHALLAN";
                            return RedirectToAction("Index", "LandingPage");
                        }
                        else if (objUser.UserType == "11") //Logistic User For Challan
                        {
                            Session["BDMSLoginType"] = "LOGISTIC";

                        }

                        /*if (isNavigateToUpdateProfile)
                        {
                            return RedirectToAction("Index", "CircleUserEdit");
                        }
                        else
                        {
                            return RedirectToAction("Index", "WelcomeLandingPage");
                        }*/
                        return RedirectToAction("Index", "WelcomeLandingPage");
                    }
                    else
                    {
                        TempData["AppMessage"] = "Invalid User name or password..";
                    }
                }
                catch (Exception ex)
                {
                    objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
                }
                finally { }
            }
            return RedirectToAction("Index", "CircleLogin");
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            ViewBag.IsOTPSent = false;
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string username, bool isOtpSent = false, string otp = null, string newpassword = null)
        {
            ViewData["Username"] = username;
            ViewBag.IsOTPSent = isOtpSent;
            if (!string.IsNullOrEmpty(otp))
            {
                var result = objDbTrx.ValidatePasswordResetRequest(username, otp);
                if (result && !string.IsNullOrEmpty(newpassword))
                {
                    var defaultPassword = newpassword;
                    result = objDbTrx.UpdateUserPassword(username, defaultPassword);
                    if (result)
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ViewData["Message"] = "Invalid OTP Or New Password is empty";

                }
                ViewBag.IsOTPSent = true;
            }
            else
            {
                DataTable dt = objDbTrx.GetBDMSUserDtlByUserName(username);

                if (dt.Rows.Count == 1)
                {
                    var mobile = dt.Rows[0]["MOBILE_NO"] != null && !string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["MOBILE_NO"])) ? dt.Rows[0]["MOBILE_NO"].ToString() : dt.Rows[0]["USERTBL_MOBILE_NO"].ToString();
                    var EMAILID = dt.Rows[0]["EMAIL_ID"] != null && !string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["EMAIL_ID"])) ? dt.Rows[0]["EMAIL_ID"].ToString() : dt.Rows[0]["USERTBL_EMAIL_ID"].ToString(); 
                    var OTP = objDbTrx.CreatePasswordResetRequest(username, mobile);
                    if (!string.IsNullOrEmpty(OTP))
                    {
                        var MailSubject = "Forget Password OTP for BDMS";
                        var MailContent = "Your one time password is " + OTP + ". Please use the OTP to login in BDMS";
                        Utility.SendForgetpassEmail(EMAILID, MailSubject, MailContent);

                        //if (!Utility.SendSMS(mobile, "Your one time password is " + OTP + ". Please use the OTP to login in " + username)) 
                        if (!Utility.SendSMS(mobile, "Your one time password is " + OTP + ". Please use the OTP to login in BDMS"))
                        {
                            ViewData["Message"] = "OTP Failed";
                        }
                        else
                        {
                            ViewData["Message"] = "OTP Sent to " + mobile;
                            ViewBag.IsOTPSent = true;
                        }
                    }
                }
                else
                {
                    ViewData["Message"] = "User doesn't exist";
                }
            }

            return View("ForgotPassword");
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult EncryptionChecker(string data, int needdecrypt = 0)
        {
            string result = string.Empty;
            try
            {
                if (needdecrypt <= default(int))
                {
                    result = SecurityController.Encrypt(data);
                }
                else
                {
                    result = SecurityController.Decrypt(data);
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(new { Result = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult EncryptDecryptAllUsersPassword(int isdecrypt = 0)
        {
            List<UserObject> lst = new List<UserObject>();
            try
            {
                DataTable dt = objDbTrx.GetAllUsersForPasswordEncryptDecrypt();
                if (dt != null && dt.Rows.Count > default(int))
                {
                    for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                    {
                        if (Convert.ToInt32(dt.Rows[iCnt]["IsPasswordEncrypted"].ToString()) == isdecrypt)
                        {
                            var obj = new UserObject();
                            obj.ID = Convert.ToInt32(dt.Rows[iCnt]["ID"].ToString());
                            obj.REF_ID = Convert.ToDouble(dt.Rows[iCnt]["REF_ID"].ToString());
                            obj.USER_ID = dt.Rows[iCnt]["USER_ID"].ToString();
                            obj.PASSWORD = isdecrypt > default(int) ? SecurityController.Decrypt(dt.Rows[iCnt]["PASSWORD"].ToString()) : SecurityController.Encrypt(dt.Rows[iCnt]["PASSWORD"].ToString());
                            obj.IsPasswordEncrypted = isdecrypt > default(int) ? default(int) : 1;
                            lst.Add(obj);
                        }
                    }
                    if (lst != null && lst.Count() > default(int))
                    {
                        System.Threading.ThreadPool.QueueUserWorkItem(s =>
                        {
                            objDbTrx.UpdateUserPasswordBulkEncryptDecrypt(lst);
                        });
                    }
                    else
                    {
                        return Json(new { Message = string.Format("Password for all users already been {0}. Hence no operation required", isdecrypt > default(int) ? "decrypted" : "encrypted") }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return Json(new { Message = string.Format("Password {0} process has been initiated for all users", isdecrypt > default(int) ? "decryption" : "encryption") }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult ManualResetPasswordByUser(string uid, string newpwd)
        {
            bool result = default(bool);
            try
            {
                if (!string.IsNullOrWhiteSpace(uid) && !string.IsNullOrWhiteSpace(newpwd))
                {
                    result = objDbTrx.UpdateUserPassword(uid, newpwd);
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            if (result)
                return Json(new { Success = 1, Message = string.Format("Password has been modified for {0}", uid) }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Success = default(int), Message = string.Format("Unable to change password for {0} right now. Please try again later", uid) }, JsonRequestBehavior.AllowGet);
        }
    }
}
