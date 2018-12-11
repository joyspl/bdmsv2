using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class MisReport
    {
        public string ReportData { get; set; }
        public string BookCode { get; set; }
        public string Language { get; set; }
        public string Category { get; set; }
        public string SchooldName { get; set; }
        public string UdiseCode{ get; set; }
        public Int64 TotalRequisitionQuantity { get; set; }
        public Int64 StockQuantity { get; set; }
        public Int64 NetRequisitionQuantity { get; set; }
        public List<MisReport> MisReportCollection { get; set; }
    }
}