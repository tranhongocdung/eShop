using System.Collections.Generic;

namespace MVCWeb.Models
{
    public class OrderStatisticViewModel
    {
        public string TotalCash { get; set; }
        public string IncompletedTotalCash { get; set; }
        public string OrderCount { get; set; }
        public ICollection<LabelValueViewModel> SoldProductStat { get; set; }
        public ICollection<LabelValueViewModel> Top10BestCustomerStat { get; set; }
    }
}