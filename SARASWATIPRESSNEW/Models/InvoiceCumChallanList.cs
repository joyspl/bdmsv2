using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SARASWATIPRESSNEW.Models
{
    public class InvoiceCumChallanList
    {
        private string _RemarksId = string.Empty;
        [XmlAttribute]
        public string InvCumChallanNo { get; set; }
        [XmlAttribute]
        public string ClassName { get; set; }
        [XmlAttribute]
        public int Book_Id { get; set; }
        [XmlAttribute]
        public string Book_Code { get; set; }
        [XmlAttribute]
        public string Common_Book_Code { get; set; }
        [XmlAttribute]
        public string Book_Name { get; set; }
        [XmlAttribute]
        public int NetReqQty { get; set; }
        [XmlAttribute]
        public int AlreadyShippedQty { get; set; }
        [XmlAttribute]
        public int QtyShipped { get; set; }
        [XmlAttribute]
        public Decimal Rate { get; set; }
        [XmlAttribute]
        public Decimal Amount { get; set; }
        [XmlAttribute]
        public string DistrictName { get; set; }
        [XmlAttribute]
        public string CircleName { get; set; }
        [XmlAttribute]
        public string LanguageName { get; set; }
        [XmlAttribute]
        public string CategoryName { get; set; }
        [XmlAttribute]
        public int TotalAmount { get; set; }

        [XmlAttribute]
        public string Cartoon { get; set; }


        [XmlAttribute]
        public double Weight { get; set; }

        [XmlAttribute]
        public string Remarks { get; set; }

        [XmlAttribute]
        public string RemarksId
        {
            get
            {
                return !string.IsNullOrEmpty(this._RemarksId) ? this._RemarksId : default(int).ToString();
            }
            set
            {
                this._RemarksId = value;
            }
        }

        [XmlAttribute]
        public Decimal RemainBal { get; set; }
        [XmlAttribute]
        public string BookSurplusQty { get; set; }
        public List<InvoiceCumChallanList> InvoiceCumChallanCollection { get; set; }

        public string Lot { get; set; }
        public string TotBooksPerLot { get; set; }

        public int TotalLot { get; set; }

        public string TotalLotDelimited { get; set; }

        public int RevicedQty { get; set; }
    }

    public class BinderDtlListByChallan
    {
        public string BOOK_CODE { get; set; }
        public string COMMON_BOOK_CODE { get; set; }
        public int TotalScannedCount { get; set; }
        public int LOT { get; set; }
        public string LOTDELIMITED { get; set; }
    }
}