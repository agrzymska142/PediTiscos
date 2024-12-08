using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateTime OrderDate { get; set; }

    public string ClientName { get; set; } = null!;

    public string ClientEmail { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
