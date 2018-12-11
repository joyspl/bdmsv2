using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SARASWATIPRESSNEW.Models
{
    public class CircleStockUpdate
    {
        protected DateTime? _CREATED_TS = null;
        protected DateTime? _UPDATED_TS = null;
        public long CIRCLE_STOCK_UPDATE_AUTO_ID { get; set; }
        public int STOCK_UPDATE_BOOK_ID { get; set; }
        public int STOCK_UPDATE_QTY { get; set; }
        public string STOCK_UPDATE_TIMESTAMP { get; set; }
        public int CIRCLE_ID { get; set; }
        public DateTime CREATED_TS
        {
            get
            {
                return this._CREATED_TS.HasValue ? this._CREATED_TS.Value : DateTime.Now;
            }
            set
            {
                this._CREATED_TS = value;
            }
        }
        public string CREATED_BY { get; set; }
        public DateTime UPDATED_TS
        {
            get
            {
                return this._UPDATED_TS.HasValue ? this._UPDATED_TS.Value : DateTime.Now;
            }
            set
            {
                this._UPDATED_TS = value;
            }
        }
        public string UPDATED_BY { get; set; }
        public string RM { get; set; }
        public int ISCONFIRMED { get; set; }
        public int STOCK_DAMAGE_QTY { get; set; }
        public int STOCK_DAMAGE_QTY_AFTERCONF { get; set; }
        public string BOOK_CODE { get; set; }
        
        public int TOTAL_TRNF_BOOKS { get; set; }
        public int TMP_ORGN_CIRCLE { get; set; }
        public int TMP_DESTN_CIRCLE { get; set; }
    }
}