using FAPP.Model;

using System.Collections.Generic;

namespace FAPP.Areas.AM.ViewModels
{
    public class WarehouseViewModel
    {
        public int? ProductId { get; set; }
        public int? WarehouseId { get; set; }
        public string TransferMethod { get; set; }
        public string TransferType { get; set; }
        public string Search { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<AMWarehouseProduct> WarehouseProductList { get; set; }
        public List<v_mnl_Openningstock_Result> v_mnl_OpenningstockList { get; set; }
    }
}