using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.Areas.AM.ViewModels

{
    public class v_mnl_Openningstock_Result
    {
        public DateTime? ExpiryDate { get; set; }
        public string ManufacturerProductNo { get; set; }
        public int ProductId { get; set; }
        public string Barcode { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? SalePrice { get; set; }
        public string CategoryName { get; set; }
        public decimal? OS { get; set; }
        public decimal? Pur { get; set; }
        public decimal? PR { get; set; }
        public decimal? Sale { get; set; }
        public decimal? SR { get; set; }
        public decimal? Damage { get; set; }
        public decimal? IssueItem { get; set; }
        public int? BrandId { get; set; }
        public decimal? StockTransfer { get; set; }
        public int ItemId { get; set; }
        public string ShortName { get; set; }
        public string ItemName { get; set; }
       
    }
}