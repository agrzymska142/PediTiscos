namespace Backend.Dtos
{
    public class OrderDto
    {
        public string UserId { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; }
        public DateTime OrderDate { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
    }
}
