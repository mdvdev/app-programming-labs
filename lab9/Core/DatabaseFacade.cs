using DatabaseContext;

namespace Core;

public class DatabaseFacade : IDatabaseFacade
{
    public ICustomerRepository Customers { get; }
    public IOrderRepository Orders { get; }
    public IShipmentRepository Shipments { get; }

    private readonly HashSet<string> _tableNames;

    public DatabaseFacade()
    {
        var db = new ShopContext();
        Customers = new CustomerRepository(db);
        Orders = new OrderRepository(db);
        Shipments = new ShipmentRepository(db);
        
        _tableNames =
        [
            "customers",
            "orders",
            "shipments"
        ];
    }

    public bool IsTableExist(string tableName) => _tableNames.Contains(tableName);
}