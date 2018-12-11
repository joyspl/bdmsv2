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
    public class LanguageController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

       [HttpPost]
        public ActionResult Index(Language objcust)
        {
            if (ModelState.IsValid)
            {
                SqlConnection con = null;
                string result = "";
                try
                {
                    con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                    SqlCommand cmd = new SqlCommand("insert into language_master (LANGUAGE) values (@language_name)", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@language_name", objcust.language_name);
                    con.Open();
                    result = cmd.ExecuteReader().ToString();
                    Response.Write("<script> alert ('Data has been submitted successfully...') </script> ");
                }
                catch (Exception ex)
                {
                    return View();
                }
                finally
                {
                    con.Close();
                }

            }

            return RedirectToAction("Index", "LanguageView");
        }

    }
}
