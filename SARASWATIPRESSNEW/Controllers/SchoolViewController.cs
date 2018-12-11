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

namespace SARASWATIPRESSNEW.Controllers
{
    public class SchoolViewController : Controller
    {

        [HttpGet]
        public ActionResult Index()
        {
            List<School> lst_rq = new List<School>();
            SqlConnection con = null;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                SqlCommand cmd = new SqlCommand("select school_master.*,district_master.DISTRICT,circle_master.CIRCLE_NAME from school_master inner join district_master on school_master.DISTRICT_ID=district_master.ID inner join circle_master on  school_master.CIRCLE_ID=circle_master.ID", con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    School rq = new School();
                    rq.SchoolID = Convert.ToInt32(Convert.ToString(rdr["ID"]));
                    rq.School_name = Convert.ToString(rdr["SCHOOL_NAME"]);
                    rq.School_Emailid = Convert.ToString(rdr["SCHOOL_EMAIL_ID"]);
                    rq.School_Code = Convert.ToString(rdr["SCHOOL_CODE"]);
                    rq.School_Adrees = Convert.ToString(rdr["SCHOOL_ADDRESS"]);
                    rq.School_Mobile = Convert.ToString(rdr["SCHOOL_PHONE_NO"]);
                    rq.DistrictName = Convert.ToString(rdr["DISTRICT"]);
                    rq.CircleName = Convert.ToString(rdr["CIRCLE_NAME"]);
                    rq.School_alt_Mobile = Convert.ToString(rdr["SCHOOL_ALT_PHONE_NO"]);
                    lst_rq.Add(rq);
                }
                rdr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View(lst_rq);
        }

    }
}
