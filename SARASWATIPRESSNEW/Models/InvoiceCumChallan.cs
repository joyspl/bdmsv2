using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Web.Mvc;


namespace SARASWATIPRESSNEW.Models
{
    public class InvoiceCumChallan
    {
        [Required(ErrorMessage = "Enter Invoice Cum Challan No")]
        public string InvoiceCumChallanNo { get; set; }

        public Int64 ChallanId { get; set; }
        //[Display(Name = "InvoiceCumChallanDate")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        [Required(ErrorMessage = "Enter Invoice Cum Challan Date")]
        public string InvoiceCumChallanDate { get; set; }

        [Required(ErrorMessage = "Select District")]
        public int DistrictId { get; set; }      
        public List<District> DistrictCollection { get; set; }
        public string DistrictName { get; set; }
        [Required(ErrorMessage = "Select Circle")]
        public int CircleId { get; set; }
        //public string CircleCode { get; set; }

        public List<SelectListItem> DistrictList;
        public List<SelectListItem> CircleList;
        public List<Circle> CircleCollection { get; set; }
        //[Required(ErrorMessage = "Select Circle")]
        public string CircleName { get; set; }
        public string CircleAddress { get; set; }
        public string CirclePinCode { get; set; }

        [Required(ErrorMessage = "Select Transporter")]
        public int TransporterID { get; set; }

        public List<Transporter> TransporterCollection { get; set; }

        //[Required(ErrorMessage = "Enter Consignee No.")]
        public string CONSIGNEE_NO { get; set; }
        public string BinderAllotMentCode { get; set; }
        
        //[Required(ErrorMessage = "Enter Vehivle No.")]
        public string VEHICLE_NO { get; set; }

        [Required(ErrorMessage = "Select Language")]
        public int LanguageId { get; set; }
        public List<Language> LanguageCollection { get; set; }

        public string CategoryName { get; set; }
        [Required(ErrorMessage = "Select Category")]
        public int CategoryId { get; set; }
        public List<Category> CategoryCollection { get; set; }

        //[Required(ErrorMessage = "Enter Inspector Name")]
        public string InspectorName { get; set; }

        //[Required(ErrorMessage = "Enter Inspector Phone no")]
        public string InspectorPhoneNo { get; set; }

        //[Required(ErrorMessage = "Enter Inspector Email ID")]
        public string InspectorEmailId { get; set; }
        public int SchoolID { get; set; }
        public string ReqStatus { get; set; }
        public string Language { get; set; }
        public string Transporter { get; set; }
        public string UserId { get; set; }
        public int AY_ID { get; set; }
        public List<InvoiceCumChallan> InvoiceCumChallanReqCollection { get; set; }
        public List<InvoiceCumChallanList> InvoiceCumChallanCollection { get; set; }
        public List<ChallanRemarks> ChallanRemarksCollection { get; set; }
        public List<BinderDetailsByScan> lstBinderDetailsByScan { get; set; }
        
        public int TotalAmount { get; set; }
        public string InvoiceCumChallanReport { get; set; }
        public int Cartoon { get; set; }
        public string Remarks { get; set; }
        public int Status { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int BookRwCnt { get; set; }
        public string InvoiceCumChallanYear { get; set; }
        public string CustomerOrderNo { get; set; }
        public string ReceivedAtCircle { get; set; }
        public string ReceivedBy { get; set; }
        public string ChallanComment { get; set; }
        public string ReceivedTimeStamp { get; set; }
        public bool IsPendingRequire { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedTimeStamp { get; set; }
        public int AcadYearId { get; set; }
        public string IsInvoiceCreated { get; set; }
        public int ConfirmStatus { get; set; }
        public string ManualChallanNo { get; set; }
        public int DayDifference { get; set; }
    }

    public class BinderDetailsByScan
    {
        public int BINDER_ALLOT_ID { get; set; }
        public int BINDER_ID { get; set; }
        public string BinderName { get; set; }
        public int Lot { get; set; }
        public string Book_Code { get; set; }
        public string Common_Book_Code { get; set; }
        public string Book_Name { get; set; }
        public double CategoryID { get; set; }
        public long TotalQty { get; set; }
        public long ScannedQty { get; set; }
        public long RemainingQty { get; set; }
    }

    public class ChallanConfirmCommunication
    {
        public int ChallanId { get; set; }
        public string ContactPerson { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
    }

    public class ChallanHeaderSimplified
    {
        public int ChallanID { get; set; }
        public string CHALLAN_NUMBER { get; set; }
        public string CONSIGNEE_NO { get; set; }
        public int TRANSPORTER_ID { get; set; }
        public string VEHICLE_NO { get; set; }
        public string Transport_Name { get; set; }
        public string ManualChallanNo { get; set; }
    }

    public class RevisedQtyMap
    {
        public int RevisedQty { get; set; }
        public int CancelledQty { get; set; }
        public string Book_Code { get; set; }
    }

    public class PartialChallanRevertObject
    {
        public string lst { get; set; }
        public int ChallanId { get; set; }
    }

    public class CommentSaveRequest
    {
        public string ChallanID { get; set; }
        public string Comment { get; set; }
    }
}