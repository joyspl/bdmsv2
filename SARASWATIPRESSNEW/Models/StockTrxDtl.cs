using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SARASWATIPRESSNEW.Models
{
    public class StockTrxDtl
    {
        [XmlAttribute]
        public Int64 BookID { get; set; }
        [XmlAttribute]
        public string BookName { get; set; }
        [XmlAttribute]
        public Int16 tot { get; set; }
        [XmlAttribute]
        public Int16 StockUpdateQuantity { get; set; }
        [XmlAttribute]
        public string TimeStamp { get; set; }
        [XmlAttribute]
        public Int64 Balance { get; set; }
        [XmlAttribute]
        public Int16 StockDamageQuantity { get; set; }
        [XmlAttribute]
        public Int16 STOCK_DAMAGE_QTY_AFTERCONF { get; set; }
        [XmlAttribute]
        public int TOTAL_TRNF_BOOKS { get; set; }
        [XmlAttribute]
        public int TMP_ORGN_CIRCLE { get; set; }
        [XmlAttribute]
        public int TMP_DESTN_CIRCLE { get; set; }

        public long AutoID { get; set; }

        public int ISCONFIRMED { get; set; }
        public string BOOK_CODE { get; set; }
    }
}