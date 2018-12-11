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
    public class CircleViewController : Controller
    {

        [HttpGet]
        public ActionResult Index()
        {
            List<Circle> lst_rq = new List<Circle>();
            SqlConnection con = null;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                SqlCommand cmd = new SqlCommand("select * from circle_master", con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Circle rq = new Circle();
                    rq.Circle_code = Convert.ToString(rdr["CIRCLE_CODE"].ToString());
                    rq.Circle_name = Convert.ToString(rdr["CIRCLE_NAME"].ToString());
                    rq.district_id = Convert.ToInt32(rdr["DISTRICT_ID"].ToString());
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
