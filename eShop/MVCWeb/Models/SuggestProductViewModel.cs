namespace MVCWeb.Models
{
    public class SuggestProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductOptions { get; set; }
        public string ShortDescription { get; set; }
        public int UnitPrice { get; set; }
    }
}