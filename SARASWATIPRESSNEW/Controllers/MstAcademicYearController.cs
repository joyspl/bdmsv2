using SARASWATIPRESSNEW.BusinessLogicLayer;
using SARASWATIPRESSNEW.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SARASWATIPRESSNEW.Controllers
{
    public class MstAcademicYearController : Controller
    {
        
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            List<MstAcademicYear> lstAcademicYear = new List<MstAcademicYear>();



            try
            {
                DataTable dtAcademicyear = objDbTrx.GetAllAcademicYear();
                if (dtAcademicyear.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dtAcademicyear.Rows.Count; iCnt++)
                    {
                        MstAcademicYear objAcademicYear = new MstAcademicYear();

                        objAcademicYear.AcademicYearID = Convert.ToInt16(dtAcademicyear.Rows[iCnt]["ID"].ToString());
                        objAcademicYear.AcademicYear = dtAcademicyear.Rows[iCnt]["ACAD_YEAR"].ToString();
                        objAcademicYear.ISACTIVE = Convert.ToInt16(dtAcademicyear.Rows[iCnt]["ISACTIVE"].ToString());
                        objAcademicYear.PFX_REQ = dtAcademicyear.Rows[iCnt]["PFX_REQ"].ToString();
                        objAcademicYear.PFX_CHALLAN = dtAcademicyear.Rows[iCnt]["PFX_CHALLAN"].ToString();
                        objAcademicYear.PFX_SCHCHALLAN = dtAcademicyear.Rows[iCnt]["PFX_SCHCHALLAN"].ToString();
                        objAcademicYear.PFX_INVOICE = dtAcademicyear.Rows[iCnt]["PFX_INVOICE"].ToString();
                        objAcademicYear.ACAD_YEAR_SHORT = dtAcademicyear.Rows[iCnt]["ACAD_YEAR_SHORT"].ToString();
                        objAcademicYear.PFX_BINDER = dtAcademicyear.Rows[iCnt]["PFX_BINDER"].ToString();
                        lstAcademicYear.Add(objAcademicYear);
                    }
                }

            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return View(lstAcademicYear);
            //return View();
        }

         [HttpPost]
        public bool UpdateStatus(int acyearid, int val)
        {
            try
            {
                var flag = objDbTrx.UpdateAcademicYear(acyearid, val);


            }
            catch (Exception ex)
            {
                return false;
                //objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return true;

        }

    }
}