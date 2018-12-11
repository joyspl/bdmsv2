using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Collections;

namespace SARASWATIPRESSNEW.Models
{
    public class BinderBookQtyRpt
    {
        public int binder_id { get; set; }
        public string book_code { get; set; }
        public int totalqty { get; set; }
        public int qtyissued { get; set; }
        public int language_id { get; set; }
        public string language { get; set; }
        public string bindername { get; set; }
        public string bookname { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }
}