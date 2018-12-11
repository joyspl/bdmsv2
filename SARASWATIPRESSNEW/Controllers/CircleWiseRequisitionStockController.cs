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
    public class CircleWiseRequisitionStockController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            return View(get_dropdown());
        }

        [HttpPost]
        public ActionResult Index(CircleWiseRequisitionStock objcust)
        {
            CircleWiseRequisitionStock lst_req = new CircleWiseRequisitionStock();
            List<CircleWiseRequisitionStock> lst_category = new List<CircleWiseRequisitionStock>();
            SqlConnection con = null;
            try
            {
                DataTable dtMastData;
                if (objcust != null && !string.IsNullOrWhiteSpace(objcust.district_id))
                {
                    dtMastData = objDbTrx.CircleWiseRequisitionStockByDistrictID(objcust.district_id);
                }
                else
                {
                    dtMastData = objDbTrx.CircleWiseRequisitionStock();
                }
                
                if (dtMastData.Rows.Count > 0)
                {
                    for (int i = 0; i < dtMastData.Rows.Count; i++)
                    {
                        CircleWiseRequisitionStock rq = new CircleWiseRequisitionStock();
                        rq.circle_name = Convert.ToString(dtMastData.Rows[i]["CIRCLE_NAME"].ToString());
                        rq.district_name = Convert.ToString(dtMastData.Rows[i]["DISTRICT"].ToString());
                        rq.no_of_school_cnf = Convert.ToString(dtMastData.Rows[i]["CntOfSchool"].ToString());
                        lst_category.Add(rq);
                    }
                }
                lst_req.circlewisecollection = lst_category;
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }

            List<District> lst_req2 = new List<District>();
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                SqlCommand cmd = new SqlCommand("select * from district_master", con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    District rq = new District();
                    rq.DistrictID = Convert.ToInt32(rdr["ID"].ToString());
                    rq.District_name = Convert.ToString(rdr["DISTRICT"].ToString());
                    lst_req2.Add(rq);
                }
                lst_req.districtcollectionlist = lst_req2;
                lst_req.district_id = objcust.district_id;
                rdr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }

            return View(lst_req);
        }


        public Models.CircleWiseRequisitionStock get_dropdown()
        {
            CircleWiseRequisitionStock lst_req = new CircleWiseRequisitionStock();
            SqlConnection con = null;
            List<District> lst_req2 = new List<District>();

            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                SqlCommand cmd = new SqlCommand("select * from district_master", con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    District rq = new District();
                    rq.DistrictID = Convert.ToInt32(rdr["ID"].ToString());
                    rq.District_name = Convert.ToString(rdr["DISTRICT"].ToString());
                    lst_req2.Add(rq);
                }
                lst_req.districtcollectionlist = lst_req2;
                rdr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }



            return lst_req;
        }


    }
}
