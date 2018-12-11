using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class SchChallanBookDtl
    {
        public int BookID { get; set; }
        public string BookCode { get; set; }
        public string ClassName { get; set; }
        public string BookName { get; set; }        
        public double BookWeight { get; set; }
        public string LotLimit { get; set; }       
    }
}