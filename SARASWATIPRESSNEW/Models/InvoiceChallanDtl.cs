using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class InvoiceChallanDtl
    {
        public string InvoiceDtlId { get; set; }
        public string ChallanId { get; set; }       
        public string ChallanNo { get; set; }       
        public string ChallanDate { get; set; }       
        public string DistrictName { get; set; }       
        public string CircleName { get; set; }    
        public string CONSIGNEE_NO { get; set; }        
        public string VEHICLE_NO { get; set; }
        public string CategoryName { get; set; }
        public string Language { get; set; }
        public string Transporter { get; set; }       
    }
}