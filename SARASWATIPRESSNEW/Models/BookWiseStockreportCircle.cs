using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class BookWiseStockreportCircle
    {
        public string language { get; set; }

        public string Category { get; set; }

        public string BookName { get; set; }

        public string BookCode { get; set; }

        public string req_quantity { get; set; }

        public string stock_quantity { get; set; }

        public string remaining_quantity { get; set; }

        public List<Circle> circle_collection { get; set; }

        public List<BookWiseStockreportCircle> bookwise_collection { get; set; }
        public string stock_damage_quantity { get; set; }
        public string QtyRcvdAtCircle { get; set; }
        public string QtyDlvToSchool { get; set; }

        public int CircleID { get; set; }

    }
}