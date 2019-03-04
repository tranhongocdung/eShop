using System.Collections.Generic;

namespace MVCWeb.Cores
{
    public class FilterParams
    {
        public string Keyword { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public List<int> CustomerIds { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int StatusId { get; set; }

        public int IsVIP { get; set; }

        public int CategoryId { get; set; }

        //For Pagination
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string SortField { get; set; }
        public bool SortASC { get; set; }

        public FilterParams()
        {
            Keyword = "";
            Month = 0;
            Year = 0;
            PageSize = 10;
            PageNumber = 1;
            SortField = "Id";
            SortASC = false;
            CustomerIds = new List<int>();
            StatusId = 0;
            IsVIP = 0;
            CategoryId = 0;
        }
    }
}