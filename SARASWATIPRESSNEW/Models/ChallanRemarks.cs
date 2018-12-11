using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class ChallanRemarks
    {
        [Key]
        public int RemId { get; set; }
        public string Remarks { get; set; }
    }

    public class MobileReceipt
    {
        public string ChallanBarcode { get; set; }
        public string UserID { get; set; }
        public string PhoneNo { get; set; }
        public string ReceiverCode { get; set; }
        public string ReceiverPic { get; set; }
        public string SendersIP { get; set; }
        public string Place { get; set; }
        public string DeviceUUID { get; set; }
    }
}