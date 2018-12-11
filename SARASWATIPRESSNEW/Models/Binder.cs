using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class Binder
    {
        [Key]
        public int BinderId { get; set; }
        public string BinderName { get; set; }
    }
}