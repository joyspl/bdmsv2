using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace SARASWATIPRESSNEW.Models
{

    public class RequisitionModel
    {
        public List<Requisition> RequisitionList { get; set; }
    }

    public partial class Requisition
    {
        [Key]


        public string UserId { get; set; }

        public string ReqSessionCode { get; set; }
        public Int64 RequisitionID { get; set; }
       // public string requisitionquantity { get; set; }

        //[Required(ErrorMessage = "Enter Circle")]
        public int CircleID { get; set; }

        //[Required(ErrorMessage = "Enter School")]
        public int SchoolID { get; set; }

        //[Required(ErrorMessage = "Enter Category")]
        public int CategoryID { get; set; }

        //[Required(ErrorMessage = "Enter Requisition Date")]
        public string RequisitionDate { get; set; }

        //[Required(ErrorMessage = "Enter Language")]
        public int LanguageID { get; set; }

        //[Required(ErrorMessage = "Enter Book")]
        public int BookID { get; set; }

        //[Required(ErrorMessage = "Enter Requisition Quantity")]
        public int RequisitionQuantity { get; set; }

        public string SaveStatus { get; set; }

        public int ISAPPROVED { get; set; }
        public int ISAPPROVED_ADMIN { get; set; }
        public string APPROVED_BY { get; set; }
        public DateTime? APPROVED_TS { get; set; }

        public int ISAPPROVED_DIST { get; set; }
        public string APPROVED_BY_DIST { get; set; }
        public DateTime? APPROVED_TS_DIST { get; set; }

        public int id_book { get; set; }
        public string book_name { get; set; }
        public int book_count { get; set; }
       
        public List<Language> languageCollection { get; set; }

        public List<Category> categoryCollection { get; set; }

        public List<School> schoolCollection { get; set; }
        public List<RequisitionTrxDtl> reqTrxCollection { get; set; }
        //public IEnumerable<Requisition> Requisitionlist { get; set; }

        public List<Book> bookCollection { get; set; }
        public List<RequisitionFinal> reqCollection { get; set; }

        public string cat_up_id { get; set; }
        public string lan_up_id { get; set; }

        public string code_school { get; set; }

        public string tot { get; set; }

        public string stock_update_quantity { get; set; }

        public string time_stamp { get; set; }

        public string balance { get; set; }

        public List<RequisitionView> reqview_collection { get; set; }

        public string school_contact_no { get; set; }

        public string school_email_id { get; set; }

        public bool stat { get; set; }
        public bool IsPendingRequire { get; set; }

        public string isConfirmed { get; set; }
    }
     

}