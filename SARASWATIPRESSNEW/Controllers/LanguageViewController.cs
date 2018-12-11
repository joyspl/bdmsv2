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
    public class LanguageViewController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            List<Language> lst_rq = new List<Language>();
            SqlConnection con = null;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                SqlCommand cmd = new SqlCommand("select * from language_master", con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Language rq = new Language();
                    rq.language_name = Convert.ToString(rdr["LANGUAGE"].ToString());
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
