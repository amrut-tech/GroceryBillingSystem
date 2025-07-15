using GroceryBillingSystem.Models;

public class BillItem
{
    public int BillItemId { get; set; }
    public int BillId { get; set; }
    public Bill Bill { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int Quantity { get; set; }
    public decimal Rate { get; set; }
    public decimal Total { get; set; }
}
