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
    public class SummaryReport
    {
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int CircleId { get; set; }
        public string CIRCLE_NAME { get; set; }
        public long total_no_of_challan_generated { get; set; }
        public long total_no_of_received_challan { get; set; }
        public long no_of_challan_recvd_for_books { get; set; }
        public long recvd_challan_qty { get; set; }
        public long Total_Requisition_Quantity { get; set; }
        public long books_delivered { get; set; }
        public long school_challan_Quantity { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}