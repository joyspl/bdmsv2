using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace SARASWATIPRESSNEW.Models
{
    public class Book
    {
        [Key]
        public int BookID { get; set; }

        //[Required(ErrorMessage = "Enter Book Code")]
        public string BookCode { get; set; }

        //[Required(ErrorMessage = "Enter Category")]
        public int CategoryID { get; set; }

        //[Required(ErrorMessage = "Enter Language")]
        public int LanguageID { get; set; }

        public string classname { get; set; }

        public string BookName { get; set; }

        //[Required(ErrorMessage = "Enter rate of book")]
        public int rate { get; set; }

        //[Required(ErrorMessage = "Enter School")]
        public int quantity { get; set; }

        //[Required(ErrorMessage = "Enter Requisition Quantity")]
        public int unitprice { get; set; }

        public int challanbookcategory { get; set; }

        public int classinteger { get; set; }

        public int surplusquanity { get; set; }

        public string surplusmode { get; set; }

        public int hsnsac { get; set; }

        public string uqc { get; set; }
        public int cgstrate { get; set; }
        public int sgstrate { get; set; }
        public int igstrate { get; set; }
        public string booknamedescription { get; set; }
        public int Bookweight { get; set; }
        public string BookcategoryName { get; set; }
        public string ChallanBookcategoryName { get; set; }
        public string LanguageName { get; set; }

        //public string requisitionquantity { get; set; }


        //public List<Book> BookCollection { get; set; }

        public List<Language> languageCollection { get; set; }

        public List<Category> categoryCollection { get; set; }

    }
}