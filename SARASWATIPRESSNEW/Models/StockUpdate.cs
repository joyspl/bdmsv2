using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace SARASWATIPRESSNEW.Models
{
    public class StockUpdate
    {

        public int CategoryID { get; set; }
        
        public int LanguageID { get; set; }

        public int BookID { get; set; }

        public List<Language> languageCollection { get; set; }

        public List<Category> categoryCollection { get; set; }

        public List<Requisition> reqCollection { get; set; }
        public List<StockTrxDtl> reqStockCollection { get; set; }
        

        public string cat_id { get; set; }
        public string lan_id { get; set; }

        public string STOCK_UPDATE_TIMESTAMP { get; set; }

        public string STOCK_UPDATE_QTY { get; set; }

        public string STOCK_UPDATE_BOOK_ID { get; set; }
        public string UserId { get; set; }
        public bool stat { get; set; }
        public string STOCK_DAMAGE_QTY { get; set; }
    }
}