namespace Ecomm.Application.Common.DTOs
{
    public class CartItemDto
    {
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public int Quantity { get; set; }
        public string Currency { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalPriceWithDiscount { get; set; }
    }
}
