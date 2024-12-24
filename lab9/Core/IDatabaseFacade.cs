namespace Core;

public interface IDatabaseFacade
{
    public ICustomerRepository Customers { get; }
    
    public IOrderRepository Orders { get; }
    
    public IShipmentRepository Shipments { get; }
    
    public bool IsTableExist(string tableName);
}