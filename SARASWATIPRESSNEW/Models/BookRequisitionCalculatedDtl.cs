using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class BookRequisitionCalculatedDtl
    {
        public string Class { get; set; }
        public string BookCode { get; set; }
        public string BookName { get; set; }
        public Int32 Qty { get; set; }
        public Int32 ChallanQty { get; set; }
        public double TotWeight { get; set; }
    }
}