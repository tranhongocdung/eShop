namespace MVCWeb.Cores.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductVariantId { get; set; }
        public int SellingPrice { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
        public virtual ProductVariant ProductVariant { get; set; }
        public virtual Order Order { get; set; }
    }
}