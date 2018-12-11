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
    public class CircleWiseSchoolReportController : Controller
    {
        
        public ActionResult Index(string a)
        {
            return View(get_dropdown(a));
        }

        [HttpPost]
        public ActionResult Index(CircleWiseSchool objcust)
        {
            CircleWiseSchool result = new CircleWiseSchool();
            result = get_report(objcust.DistrictID,objcust.CircleID);
            return View(result);
        }



        public Models.CircleWiseSchool get_report(int district,int circleid)
        {
            CircleWiseSchool circle_school = new CircleWiseSchool();
            List<CircleWiseSchool> lst_req = new List<CircleWiseSchool>();            
            SqlConnection con = null;


            //try
            //{
            //    con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
            //    SqlCommand cmd = new SqlCommand("select tbl1.total,school_name,school_code from (select sum(CURR_REQUISITION_QTY) as total,school_id from requisition_entry where circle_id=" + circleid + " group by school_id) as tbl1 inner join school_master on school_master.ID=tbl1.school_id", con);
            //    cmd.CommandType = CommandType.Text;
            //    con.Open();
            //    SqlDataReader rdr = cmd.ExecuteReader();
            //    while (rdr.Read())
            //    {
            //        CircleWiseSchool rq = new CircleWiseSchool();
            //        rq.school_code = Convert.ToString(rdr["school_code"].ToString());
            //        rq.school_name = Convert.ToString(rdr["school_name"].ToString());
            //        rq.req_amt = Convert.ToString(rdr["total"].ToString());
            //        lst_req.Add(rq);
            //    }
            //    circle_school.circle_wise_school = lst_req;
            //    rdr.Close();
            //    con.Close();
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}

            List<Requisition> lst_requisition = new List<Requisition>();
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                SqlCommand cmd = new SqlCommand("select tbl.*,book_master.BOOK_NAME from(SELECT SUM(CURR_REQUISITION_QTY) AS TOT ,book_id FROM requisition_entry where CIRCLE_ID=" + circleid + " and SAVE_STATUS='0' group by book_id) as tbl inner join book_master on book_master.ID=tbl.book_id", con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                DataTable dt1 = new DataTable();
                SqlDataAdapter rdr = new SqlDataAdapter(cmd);
                rdr.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        Requisition rq = new Requisition();
                        rq.book_name = Convert.ToString(dt1.Rows[i]["BOOK_NAME"].ToString());
                        rq.BookID = Convert.ToInt32(dt1.Rows[i]["book_id"].ToString());
                        rq.tot = dt1.Rows[i]["TOT"].ToString();

                        DataTable dt = new DataTable();
                        SqlCommand cmd_stock = new SqlCommand("select STOCK_UPDATE_QTY AS STOCK_UPDATE_QTY,STOCK_UPDATE_TIMESTAMP from circle_stock_update where STOCK_UPDATE_BOOK_ID=" + dt1.Rows[i]["book_id"].ToString() + " AND CIRCLE_ID=" + circleid + " order by CIRCLE_STOCK_UPDATE_AUTO_ID desc", con);
                        cmd_stock.CommandType = CommandType.Text;
                        SqlDataAdapter da = new SqlDataAdapter(cmd_stock);
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            if (Convert.ToString(dt.Rows[0][0]) != "")
                            {
                                rq.stock_update_quantity = Convert.ToString(dt.Rows[0]["STOCK_UPDATE_QTY"].ToString());
                            }
                            else
                            {
                                rq.stock_update_quantity = "0";
                            }
                            if (Convert.ToString(dt.Rows[0]["STOCK_UPDATE_TIMESTAMP"]) != "")
                            {
                                rq.time_stamp = Convert.ToString(dt.Rows[0]["STOCK_UPDATE_TIMESTAMP"]);
                            }
                            else
                            {
                                rq.time_stamp = DateTime.Now.ToString();
                            }
                        }
                        else
                        {
                            rq.stock_update_quantity = "0";
                            rq.time_stamp = DateTime.Now.ToString();
                        }
                        rq.balance = Convert.ToString(Convert.ToInt32(rq.tot) - Convert.ToInt32(rq.stock_update_quantity));
                        lst_requisition.Add(rq);
                    }
                }
                circle_school.req_wise_collection = lst_requisition;
                con.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            List<District> lst_req1 = new List<District>();
            List<Circle> lst_req2 = new List<Circle>();

            try
            {               
                SqlCommand cmd = new SqlCommand("select * from district_master", con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    District rq = new District();
                    rq.DistrictID = Convert.ToInt32(rdr["ID"].ToString());
                    rq.District_name = Convert.ToString(rdr["DISTRICT"].ToString());
                    lst_req1.Add(rq);
                }
                circle_school.lst_district = lst_req1;
                circle_school.DistrictID = district;
                rdr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            try
            {
                SqlCommand cmd = new SqlCommand("select * from circle_master where DISTRICT_ID=" + Convert.ToDouble(district) + " and ID in (select CIRCLE_ID from school_master)", con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Circle rq = new Circle();
                    rq.CircleID = Convert.ToInt32(rdr["ID"].ToString());
                    rq.Circle_name = Convert.ToString(rdr["CIRCLE_NAME"].ToString());
                    lst_req2.Add(rq);
                }
                circle_school.lst_circle = lst_req2;
                circle_school.CircleID = circleid;
                rdr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }     

            return circle_school;
        }

        [HttpGet]
        public Models.CircleWiseSchool get_dropdown(string a)
        {
            CircleWiseSchool lst_req = new CircleWiseSchool();
            SqlConnection con = null;
            List<District> lst_req1 = new List<District>();
            List<Circle> lst_req2 = new List<Circle>();

            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                SqlCommand cmd = new SqlCommand("select * from district_master", con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                int C = 0;
                while (rdr.Read())
                {
                    District rq = new District();
                    rq.DistrictID = Convert.ToInt32(rdr["ID"].ToString());
                    rq.District_name = Convert.ToString(rdr["DISTRICT"].ToString());
                    lst_req1.Add(rq);
                }
                lst_req.lst_district = lst_req1;
                rdr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            if (Convert.ToString(a) != null && Convert.ToString(a) != "")
            {
                try
                {
                    con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                    SqlCommand cmd = new SqlCommand("select * from circle_master where DISTRICT_ID=" + Convert.ToDouble(a.ToString()) + " and ID in (select CIRCLE_ID from school_master)", con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Circle rq = new Circle();
                        rq.CircleID = Convert.ToInt32(rdr["ID"].ToString());
                        rq.Circle_name = Convert.ToString(rdr["CIRCLE_NAME"].ToString());
                        lst_req2.Add(rq);
                    }
                    lst_req.lst_circle = lst_req2;
                    lst_req.DistrictID = Convert.ToInt32(a.ToString());
                    rdr.Close();
                    con.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }                
            }
            else
            {
                Circle rq = new Circle();
                rq.CircleID = 0;
                rq.Circle_name = "";
                lst_req.lst_circle = lst_req2;
            }

            return lst_req;
        }

    }
}
