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
    public class DirectorateCircleWiseSchoolReportController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index(string a)
        {
            return View(get_dropdown(a));
        }
        [HttpPost]
        public ActionResult Index(CircleWiseSchool objcust)
        {
            CircleWiseSchool result = new CircleWiseSchool();
            result = get_report(objcust.DistrictID, objcust.CircleID);
            return View(result);
        }
        public Models.CircleWiseSchool get_report(int district, int circleid)
        {
            CircleWiseSchool circle_school = new CircleWiseSchool();
            List<CircleWiseSchool> lst_req = new List<CircleWiseSchool>();
            SqlConnection con = null;
            Int64 ReqQty = 0, StockQty = 0, NetReq = 0, ReceivedQty = 0, RemainAfterReceived = 0, DistributedInSchool = 0, RemainAfterDistribution = 0;

            List<CircleWiseSchoolReport> objCircleWiseSchoolReport = new List<CircleWiseSchoolReport>();
            try
            {
                DataTable dtMastData = objDbTrx.GetDirectorateCircleWiseSchoolReport(Convert.ToInt64(circleid));
                if (dtMastData.Rows.Count > 0)
                {
                    for (int i = 0; i < dtMastData.Rows.Count; i++)
                    {
                        CircleWiseSchoolReport objRpt = new CircleWiseSchoolReport();
                        ReqQty = 0;
                        StockQty = 0;
                        NetReq = 0; ReceivedQty = 0;
                        RemainAfterReceived = 0; 
                        DistributedInSchool = 0;
                        RemainAfterDistribution = 0;
                        objRpt.Book_Code = Convert.ToString(dtMastData.Rows[i]["BOOK_NAME"].ToString());
                        objRpt.Class = Convert.ToString(dtMastData.Rows[i]["CLASS"].ToString());
                        objRpt.BookName = Convert.ToString(dtMastData.Rows[i]["BOOK_NAME"].ToString());
                        objRpt.Language = Convert.ToString(dtMastData.Rows[i]["LANGUAGE"].ToString());
                         
                        ReqQty = Convert.ToInt64(dtMastData.Rows[i]["REQUISITION_QTY"].ToString());
                        StockQty = Convert.ToInt64(dtMastData.Rows[i]["STOCK_UPDATE_QTY"].ToString());                       
                        NetReq = ((ReqQty - StockQty) >= 0 ? (ReqQty - StockQty) : 0);
                        ReceivedQty = Convert.ToInt64(dtMastData.Rows[i]["ALREADYSHIPPED"].ToString());
                        RemainAfterReceived = (ReceivedQty + StockQty);
                        DistributedInSchool = Convert.ToInt64(dtMastData.Rows[i]["DISTRIBUTEDINSCHOOL"].ToString());
                        RemainAfterDistribution = (RemainAfterReceived - DistributedInSchool);

                        objRpt.ReqQty = ReqQty.ToString();
                        objRpt.StockQty = StockQty.ToString();
                        objRpt.NetReq = NetReq.ToString();
                        objRpt.ReceivedQty =ReceivedQty.ToString();
                        objRpt.RemainAfterReceived =RemainAfterReceived.ToString();
                        objRpt.DistributedInSchool =DistributedInSchool.ToString();  
                        objRpt.RemainAfterDistribution = RemainAfterDistribution.ToString();
                        objCircleWiseSchoolReport.Add(objRpt);
                    }
                    //dtMastData.Rows[i]
                }


                circle_school.CollectionCircleWiseSchoolReport = objCircleWiseSchoolReport;
                con.Close();
            }
            catch (Exception ex) { objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress); }

            List<District> lst_req1 = new List<District>();
            List<Circle> lst_req2 = new List<Circle>();

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
                    lst_req1.Add(rq);
                }
                circle_school.lst_district = lst_req1;
                circle_school.DistrictID = district;
                rdr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }

            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
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
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
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
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
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
                    objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
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
