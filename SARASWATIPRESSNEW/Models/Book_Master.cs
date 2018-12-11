using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Collections;



namespace SARASWATIPRESSNEW.Models
{
    public class Book_Master
    {

       [Key]
       public int BookID { get; set; }
       public string BookCode { get; set; }
       public int CategoryID { get; set; }
       public int LanguageID { get; set; }
       public string classname { get; set; }
       public string BookName { get; set; }
       public double rate { get; set; }
       public int quantity { get; set; }
       public double unitprice { get; set; }
       public int challanbookcategory { get; set; }
       public int classinteger { get; set; }
       public string surplusquanity { get; set; }
       public string surplusmode { get; set; }
       public string hsnsac { get; set; }
       public string uqc { get; set; }
       public string cgstrate { get; set; }
       public string sgstrate { get; set; }
       public string igstrate { get; set; }
       public string booknamedescription { get; set; }
       public double Bookweight { get; set; }
       public string LotLimit { get; set; }
       public string BookcategoryName { get; set; }
       public string ChallanBookcategoryName { get; set; }
       public string LanguageName { get; set; }
       public List<Language> languageCollection { get; set; }
       public List<Category> categoryCollection { get; set; }
       public bool Book_Lock { get; set; }

       
    }
}