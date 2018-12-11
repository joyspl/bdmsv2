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
    public class ForgotPasswordController : Controller
    {
        
        public ActionResult Index()
        {
            if (Convert.ToString(((UserSec)Session["UserSec"]).UserUniqueId) == "")
            {
                return RedirectToAction("Index", "CircleLogin");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Index(ForgotPassword objcust)
        {
            if (ModelState.IsValid)
            {
                SqlConnection con = null;
                string result = "";
                try
                {
                    if (Convert.ToString(((UserSec)Session["UserSec"]).UserUniqueId) != "")
                    {
                        if (objcust.new_password != objcust.confirm_password)
                        {
                            Response.Write("<script> alert('Password not matched. Please check and try again') </script> ");
                        }
                        else
                        {
                            int userId = Convert.ToInt32(((UserSec)Session["UserSec"]).UserUniqueId);
                            objcust.CircleuserId = ((UserSec)Session["UserSec"]).CircleID;
                            con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                            //SqlCommand cmd = new SqlCommand("update circle_user_master set PASSWORD=@PASSWORD where ID=@ID", con);
                            SqlCommand cmd = new SqlCommand("update user_master set PASSWORD=@PASSWORD where REF_ID=@REF_ID AND ID=@ID", con);
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@REF_ID", objcust.CircleuserId);
                            cmd.Parameters.AddWithValue("@PASSWORD", SecurityController.Encrypt(objcust.new_password));
                            cmd.Parameters.AddWithValue("@ID", userId);
                            con.Open();
                            result = cmd.ExecuteReader().ToString();
                            con.Close();
                            Response.Write("<script> alert('Password has been changed successfully') </script> ");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "CircleLogin");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return View();
        }    

    }
}
