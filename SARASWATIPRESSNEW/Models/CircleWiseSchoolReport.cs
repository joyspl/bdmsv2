using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class CircleWiseSchoolReport
    {
        public string Book_Code { get; set; }
        public string Class { get; set; }
         public string BookName { get; set; }
         public string Language { get; set; }
         public string ReqQty { get; set; }
         public string StockQty { get; set; }
         public string NetReq { get; set; }
         public string ReceivedQty { get; set; }
         public string RemainAfterReceived { get; set; }
         public string DistributedInSchool { get; set; }
         public string RemainAfterDistribution { get; set; }
       
    }
}