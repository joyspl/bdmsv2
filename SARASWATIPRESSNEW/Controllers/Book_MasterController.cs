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
    public class Book_MasterController : Controller
    {
        //
        // GET: /Book_Master/
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            List<Book_Master> listBookMaster = new List<Book_Master>();
            try
            {
                DataTable GetBookMasterdt = objDbTrx.GetBookMaster();
                if (GetBookMasterdt.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < GetBookMasterdt.Rows.Count; iCnt++)
                    {
                        Book_Master objMstBookMaster = new Book_Master();
                        objMstBookMaster.BookID = Convert.ToInt16(GetBookMasterdt.Rows[iCnt]["ID"].ToString());
                        objMstBookMaster.BookCode = GetBookMasterdt.Rows[iCnt]["BOOK_CODE"].ToString();
                        objMstBookMaster.CategoryID = Convert.ToInt16(GetBookMasterdt.Rows[iCnt]["CATEGORY_ID"].ToString());
                        objMstBookMaster.LanguageID = Convert.ToInt16(GetBookMasterdt.Rows[iCnt]["LANGUAGE_ID"].ToString());
                        objMstBookMaster.classname = GetBookMasterdt.Rows[iCnt]["CLASS"].ToString();
                        objMstBookMaster.BookName = GetBookMasterdt.Rows[iCnt]["BOOK_NAME"].ToString();
                        objMstBookMaster.rate = Convert.ToDouble(GetBookMasterdt.Rows[iCnt]["RATE"]);
                        objMstBookMaster.quantity = Convert.ToInt16(GetBookMasterdt.Rows[iCnt]["QUANTITY"]);
                        objMstBookMaster.unitprice = Convert.ToDouble(GetBookMasterdt.Rows[iCnt]["UNIT_PRICE"]);
                        objMstBookMaster.challanbookcategory = Convert.ToInt16(GetBookMasterdt.Rows[iCnt]["CHALLAN_BOOK_CATEGORY_ID"].ToString());
                        objMstBookMaster.classinteger = Convert.ToInt16(GetBookMasterdt.Rows[iCnt]["CLASS_INT"].ToString());
                        objMstBookMaster.surplusquanity = GetBookMasterdt.Rows[iCnt]["SURPLUS_QTY"].ToString();
                        objMstBookMaster.surplusmode = GetBookMasterdt.Rows[iCnt]["SURPLUS_MODE"].ToString();
                        objMstBookMaster.hsnsac = GetBookMasterdt.Rows[iCnt]["HSN_SAC"].ToString();
                        objMstBookMaster.uqc = GetBookMasterdt.Rows[iCnt]["UQC"].ToString();
                        objMstBookMaster.cgstrate = GetBookMasterdt.Rows[iCnt]["CGST_RATE"].ToString();
                        objMstBookMaster.sgstrate = GetBookMasterdt.Rows[iCnt]["SGST_RATE"].ToString();
                        objMstBookMaster.igstrate = GetBookMasterdt.Rows[iCnt]["IGST_RATE"].ToString();
                        objMstBookMaster.booknamedescription = GetBookMasterdt.Rows[iCnt]["BOOK_NAME_DESC"].ToString();
                        objMstBookMaster.Bookweight = Convert.ToDouble(GetBookMasterdt.Rows[iCnt]["Book_Weight"].ToString());
                        objMstBookMaster.BookcategoryName = GetBookMasterdt.Rows[iCnt]["BOOK_CATEGORY"].ToString();
                        objMstBookMaster.ChallanBookcategoryName = GetBookMasterdt.Rows[iCnt]["CHALLAN_BOOK_CATEGORY"].ToString();
                        objMstBookMaster.LanguageName = GetBookMasterdt.Rows[iCnt]["LANGUAGE"].ToString();
                        objMstBookMaster.Book_Lock = Convert.ToBoolean( GetBookMasterdt.Rows[iCnt]["Book_Lock"]);
                        listBookMaster.Add(objMstBookMaster);

                    }
                }

            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return View(listBookMaster);
        }

        [HttpPost]
        public bool LockThisBook(int bookId, bool val)
        {
            try
            {
                var flag = objDbTrx.GetLockThisBook(bookId,  val);
               

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
