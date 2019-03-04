namespace MVCWeb.Models
{
    public class BasePagingViewModel
    {
        public int ItemCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
    }
}