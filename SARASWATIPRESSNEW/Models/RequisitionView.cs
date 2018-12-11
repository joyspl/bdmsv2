using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace SARASWATIPRESSNEW.Models
{
    public class RequisitionView
    {

        public string Circle { get; set; }
        public int School { get; set; }
        public string Category { get; set; }
        public DateTime RequisitionDate { get; set; }
        public string Language { get; set; }
        public string Book { get; set; }
        public int Prev_RequisitionQuantity { get; set; }
        public int Curr_RequisitionQuantity { get; set; }
        public string schoolname { get; set; }
        public Int64 requisitionid { get; set; }
        public string requisition_stat { get; set; }
        public string req_code { get; set; }
        public string url { get; set; }
        public string DeleteStatus { get; set; }
        public string DeleteUrl { get; set; }
        public string requisition_id { get; set; }
        public string req_date { get; set; }
        public string school_name { get; set; }
        public string school_code { get; set; }
        public string language_name { get; set; }
        public string category_name { get; set; }
        public Int64 topLimit { get; set; }

        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public bool IsPendingRequire { get; set; }
        public string SchoolChallanCode { get; set; }
        public string ChallanUpdatedTs { get; set; }
        public string ChallanUpdatedBy { get; set; }
        public int SchoolID { get; set; }
        public string SchoolName { get; set; }
        public int SchoolCode { get; set; }
        public string  SchoolContactNo { get; set; }
        public string SchoolEmailId { get; set; }
    }
    
}