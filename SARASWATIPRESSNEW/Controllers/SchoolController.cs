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
    public class SchoolController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(School objcust)
        {
            if (ModelState.IsValid)
            {
                if (objcust.stat == true)
                {                  
                    try
                    {
                        DataTable dt_chk = objDbTrx.GetSchoolMasterDetailsBySchoolCode(objcust.School_Code.ToUpper());                        
                        if (dt_chk.Rows.Count > 0)
                        {
                            dt_chk.Dispose();
                            return Content("<script language='javascript' type='text/javascript'>alert('School Code Already Exist!!!!');</script>");
                        }
                        else
                        {                           
                            objcust.DistrictId  = Convert.ToInt32(((UserSec)Session["UserSec"]).DistrictID);
                            objcust.CircleId = Convert.ToInt32(((UserSec)Session["UserSec"]).CircleID);
                            objcust.UserId = (((UserSec)Session["UserSec"]).UserId).ToString();
                            string SchoolId = "";
                           // objDbTrx.InsertInSchool(objcust, out  SchoolId);
                            Session["SchooldCode"] = SchoolId;                           
                            if (Convert.ToString(Session["ReqSessionCode"]) == "")
                            {
                                return RedirectToAction("Requisition", "Home");
                            }
                            else
                            {
                                return RedirectToAction("Index", "RequisitionEdit");
                            }

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
            return View();
        }

        //[HttpGet]
        //public Models.School get_dropdown()
        //{
        //    School lst_school = new School();
        //    List<District> lst_district = new List<District>();
        //    SqlConnection con = null;
        //    try
        //    {
        //        con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
        //        SqlCommand cmd = new SqlCommand("select * from district_master", con);
        //        cmd.CommandType = CommandType.Text;
        //        con.Open();
        //        SqlDataReader rdr = cmd.ExecuteReader();
        //        int C = 0;
        //        while (rdr.Read())
        //        {
        //            District rq = new District();
        //            rq.District_name = Convert.ToString(rdr["DISTRICT"].ToString());
        //            rq.DistrictID = Convert.ToInt32(rdr["ID"].ToString());
        //            lst_district.Add(rq);
        //        }
        //        lst_school.DistrictCollection = lst_district;
        //        rdr.Close();
        //        con.Close();

        //        List<Circle> lst_circle = new List<Circle>();
        //        try
        //        {
        //            con.Open();
        //            SqlCommand cmd1 = new SqlCommand("select * from circle_master", con);
        //            cmd1.CommandType = CommandType.Text;
        //            SqlDataReader rdr1 = cmd1.ExecuteReader();
        //            while (rdr1.Read())
        //            {
        //                Circle rq = new Circle();
        //                rq.Circle_name = Convert.ToString(rdr1["CIRCLE_NAME"].ToString());
        //                rq.CircleID = Convert.ToInt32(rdr1["ID"].ToString());
        //                lst_circle.Add(rq);
        //            }
        //            lst_school.CircleCollection = lst_circle;
        //            rdr1.Close();
        //            con.Close();

        //        }
        //        catch (Exception ex)
        //        {
        //            throw new Exception(ex.Message);
        //        }                
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    return lst_school;
        //}

    }
}
