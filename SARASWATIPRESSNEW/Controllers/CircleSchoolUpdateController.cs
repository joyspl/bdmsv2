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
    public class CircleSchoolUpdateController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index(string e)
        {
            if (Convert.ToString(((UserSec)Session["UserSec"]).CircleID) != "")
            {
                return View(get_dropdown(e));
            }
            else
            {
                return RedirectToAction("Index", "CircleLogin");
            }
        }

        [HttpPost]
        public ActionResult Index(School objcust)
        {   string CircleId = "";
            try { CircleId = ((UserSec)Session["UserSec"]).CircleID; }
            catch { CircleId = ""; }
            if (CircleId != "")
            {
                if (ModelState.IsValid)
                {
                    if (objcust.stat == true)
                    {                      
                        try
                        {

                           
                           DataTable dt_chk = objDbTrx.CheckSchoolMasterDetailsBySchoolCode(objcust.School_Code.ToUpper(), Convert.ToInt64(objcust.SchoolID));
                            if (dt_chk.Rows.Count > 0)
                            {
                                TempData["Message"] =objcust.School_Code.ToUpper()+ " Already Exist.";
                            }
                            else
                            {
                                objcust.CircleId = Convert.ToInt32(((UserSec)Session["UserSec"]).CircleID);
                                objcust.UserId = (((UserSec)Session["UserSec"]).UserId).ToString();
                               // objDbTrx.UpdateInSchool(objcust);
                                TempData["Message"] =  "School information Successfully Updated";
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
                return RedirectToAction("Index", "CircleSchoolUpdate");            
            }
            else
            {
                return RedirectToAction("Index", "CircleLogin");
            }
        }


        [HttpGet]
        public Models.School get_dropdown(string SchoolId)
        {
            School lst_req = new School();
            SqlConnection con = null;

                List<School> lst_school = new List<School>();
                try
                {
                   DataTable dtMastData = objDbTrx.GetSchoolMasterDetailsByCircleId(Convert.ToInt16(((UserSec)Session["UserSec"]).CircleID));
                    if (dtMastData.Rows.Count > 0)
                    {
                        for (int iCnt = 0; iCnt < dtMastData.Rows.Count; iCnt++)
                        {
                            School rq = new School();
                            rq.School_name = Convert.ToString(dtMastData.Rows[iCnt]["SCHOOL_NAME"].ToString());
                            rq.SchoolID = Convert.ToInt32(dtMastData.Rows[iCnt]["ID"].ToString());
                            lst_school.Add(rq);
                        }
                        dtMastData.Dispose();
                    }
                    lst_req.school_collection = lst_school;              
                }
                catch (Exception ex)
                {
                    objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
                }

                if (Convert.ToString(SchoolId) != null && Convert.ToString(SchoolId) != "" && Convert.ToString(SchoolId) != "0")
            {
                //List<School> lst_school = new List<School>();
                try
                {
                    DataTable dtMastData = objDbTrx.GetSchoolMasterDetailsBySchoolId(Convert.ToInt64(SchoolId));
                    if (dtMastData.Rows.Count > 0)
                    {
                        lst_req.School_name = Convert.ToString(dtMastData.Rows[0]["SCHOOL_NAME"].ToString());
                        lst_req.School_Code = Convert.ToString(dtMastData.Rows[0]["SCHOOL_CODE"].ToString());
                        lst_req.School_Adrees = Convert.ToString(dtMastData.Rows[0]["SCHOOL_ADDRESS"].ToString());
                        lst_req.School_Mobile = Convert.ToString(dtMastData.Rows[0]["SCHOOL_PHONE_NO"].ToString());
                        lst_req.School_alt_Mobile = Convert.ToString(dtMastData.Rows[0]["SCHOOL_ALT_PHONE_NO"].ToString());
                        lst_req.School_Emailid = Convert.ToString(dtMastData.Rows[0]["SCHOOL_EMAIL_ID"].ToString());

                        dtMastData.Dispose();
                    }
                    lst_req.SchoolID = Convert.ToInt32(SchoolId.ToString());
                  
                }
                catch (Exception ex)
                {
                    objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
                }
            }

            return lst_req;
        }

    }
}
