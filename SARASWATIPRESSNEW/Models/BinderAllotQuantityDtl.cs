using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class BinderAllotQuantityDtl
    {
        private DateTime? _CREATED_TS = null;
        private DateTime? _UPDATED_TS = null;

        public long DTLID { get; set; }
        public int BINDER_ALLOT_ID { get; set; }
        public string STICKER_CODE { get; set; }
        public string BINDER_ALLOT_CODE { get; set; }
        public string BINDER_SHORT_CODE { get; set; }
        public string BOOK_CODE { get; set; }
        public int CHALLAN_ID { get; set; }
        public int SCANNED_STATUS { get; set; }
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
        public string xData { get; set; }
    }

    public class BinderAllotQuantityDtlMinimal
    {
        public string STICKER_CODE { get; set; }
        public string BINDER_ALLOT_CODE { get; set; }
    }
}