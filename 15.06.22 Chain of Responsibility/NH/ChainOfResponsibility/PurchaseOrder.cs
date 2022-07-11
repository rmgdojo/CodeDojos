namespace ChainOfResponsibility
{
    public record PurchaseOrder(decimal TotalCost, decimal UnitCost, decimal Quantity, string SupplierName, string Description);
}
