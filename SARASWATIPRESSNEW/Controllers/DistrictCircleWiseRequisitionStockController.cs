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
    public class DistrictCircleWiseRequisitionStockController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            string districtId = "";
            try { districtId = Session["district_id"].ToString(); }
            catch { districtId = ""; }
            if (districtId != "")
            {
                return View(get_report());
            }
            else
            {
                return RedirectToAction("Index", "DistrictLogin");
            }
        }

       [HttpGet]        
        public Models.DistrictCircleWiseRequisitionStock get_report()
        {
            DistrictCircleWiseRequisitionStock lst_req = new DistrictCircleWiseRequisitionStock();
            List<DistrictCircleWiseRequisitionStock> lst_category = new List<DistrictCircleWiseRequisitionStock>();
            string districtId = "";
            try { districtId = Session["district_id"].ToString(); }
            catch { districtId = ""; }
            try
            {
                DataTable dtReqView = objDbTrx.GetDistrictCircleWiseRequisitionStock(districtId);
                if (dtReqView.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dtReqView.Rows.Count; iCnt++)
                    {
                        DistrictCircleWiseRequisitionStock rq = new DistrictCircleWiseRequisitionStock();
                        rq.circle_name = Convert.ToString(dtReqView.Rows[iCnt]["CIRCLE_NAME"].ToString());
                        rq.no_of_school_cnf = Convert.ToString(dtReqView.Rows[iCnt]["CntOfSchool"].ToString());                        
                        lst_category.Add(rq);
                    }
                    lst_req.districtcirclewisecollection = lst_category;
                    lst_req.district_name = Convert.ToString(Session["district_name"]);
                }
            }
            catch (Exception ex)
            {
            throw new Exception(ex.Message);
            }
            
            return lst_req;
        }

    }
}
