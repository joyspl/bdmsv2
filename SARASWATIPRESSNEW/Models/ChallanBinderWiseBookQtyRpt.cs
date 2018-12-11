using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class ChallanBinderWiseBookQtyRpt
    {
        public int BinderID { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public List<Book>BookCollection { get; set; }
    }
}