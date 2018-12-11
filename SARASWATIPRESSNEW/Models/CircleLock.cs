using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace SARASWATIPRESSNEW.Models
{
    public class CircleLock
    {
        protected DateTime? _ReqLockDate = null;
        public int id { get; set; }
        public int circle_id { get; set; }
        public string Req_year { get; set; }
        public string Req_lock { get; set; }
        public string Stock_lock { get; set; }
        public DateTime ReqLockDate
        {
            get
            {
                return this._ReqLockDate.HasValue ? this._ReqLockDate.Value : DateTime.Now;
            }
            set
            {
                this._ReqLockDate = value;
            }
        }
    }
}