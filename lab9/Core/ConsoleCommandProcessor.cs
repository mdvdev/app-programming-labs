using DatabaseModels;

namespace Core;

public class ConsoleCommandProcessor : ICommandProcessor
{
    private readonly IDatabaseFacade _facade = new DatabaseFacade();

    public async Task RunAsync()
    {
        while (true)
        {
            Console.WriteLine("Choose an option:");
            foreach (var op in Enum.GetValues<OperationType>())
            {
                Console.WriteLine($"{(int)op} - {op}");
            }

            if (int.TryParse(Console.ReadLine(), out int choice) &&
                Enum.IsDefined(typeof(OperationType), choice))
            {
                var operation = (OperationType)choice;

                if (operation == OperationType.Exit)
                {
                    Console.WriteLine("Exiting...Bye!");
                    break;
                }

                await HandleOperationAsync(operation);
            }
            else
            {
                Console.WriteLine("Invalid option. Try again.");
            }
        }
    }

    private async Task HandleOperationAsync(OperationType operation)
    {
        switch (operation)
        {
            case OperationType.Insert:
                await HandleInsertAsync();
                break;
            case OperationType.Delete:
                await HandleDeleteAsync();
                break;
            case OperationType.Update:
                await HandleUpdateAsync();
                break;
            case OperationType.GetOrderCount:
                await HandleGetOrderCountAsync();
                break;
            case OperationType.GetDeliveryAddress:
                await HandleGetDeliveryAddressAsync();
                break;
            case OperationType.GetTotalSpent:
                await HandleGetTotalSpentAsync();
                break;
            case OperationType.PrintTable:
                HandlePrintTable();
                break;
            case OperationType.Exit:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
        }
    }

    private void HandlePrintTable()
    {
        Console.Write("Enter table name: ");
        var tableName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(tableName) || !_facade.IsTableExist(tableName))
        {
            Console.WriteLine("Table does not exist.");
            return;
        }

        switch (tableName.ToLower())
        {
            case "customers":
                HandlePrintCustomersTable();
                break;
            case "orders":
                HandlePrintOrdersTable();
                break;
            case "shipments":
                HandlePrintShipmentsTable();
                break;
            default:
                Console.WriteLine("Unsupported table.");
                break;
        }
    }

    private void HandlePrintShipmentsTable()
    {
        foreach (var shipment in _facade.Shipments.GetAll())
        {
            Console.WriteLine(shipment);
        }
    }

    private void HandlePrintOrdersTable()
    {
        foreach (var order in _facade.Orders.GetAll())
        {
            Console.WriteLine(order);
        }
    }

    private void HandlePrintCustomersTable()
    {
        foreach (var customer in _facade.Customers.GetAll())
        {
            Console.WriteLine(customer);
        }
    }

    private async Task HandleGetTotalSpentAsync()
    {
        Console.Write("Enter customer ID: ");
        if (!int.TryParse(Console.ReadLine(), out var customerId) || customerId < 1)
        {
            Console.WriteLine("Invalid customer ID.");
            return;
        }

        if (await _facade.Customers.GetByIdAsync(customerId) == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }

        var totalSpent = _facade.Orders.GetAll()
            .Where(o => o.CustomerId == customerId)
            .Sum(o => o.Price);
        
        Console.WriteLine($"Total spent for customer with ID {customerId}: {totalSpent}");
    }

    private async Task HandleGetDeliveryAddressAsync()
    {
        Console.Write("Enter order ID: ");
        if (!int.TryParse(Console.ReadLine(), out var orderId) || orderId < 1)
        {
            Console.WriteLine("Invalid order ID.");
            return;
        }

        var order = await _facade.Orders.GetByIdAsync(orderId);
        if (order == null)
        {
            Console.WriteLine("Order not found.");
            return;
        }

        var address = _facade.Customers.GetAll()
            .Where(customer => customer.Id == order.CustomerId)
            .Select(customer => customer.Address)
            .FirstOrDefault();

        if (address == null)
        {
            Console.WriteLine("Address not found.");
            return;
        }
        
        Console.WriteLine($"Address of customer with order ID {orderId}: {address}");
    }

    private async Task HandleGetOrderCountAsync()
    {
        Console.Write("Enter customer ID: ");
        if (!int.TryParse(Console.ReadLine(), out var customerId) || customerId < 1)
        {
            Console.WriteLine("Invalid customer ID.");
            return;
        }

        if (await _facade.Customers.GetByIdAsync(customerId) == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }

        var count = _facade.Orders.GetAll()
            .Count(order => order.CustomerId == customerId);
        
        Console.WriteLine($"Total orders for customer with ID {customerId}: {count}");
    }

    private async Task HandleUpdateAsync()
    {
        Console.Write("Enter table name to update: ");
        var tableName = Console.ReadLine()?.ToLower();

        if (string.IsNullOrWhiteSpace(tableName) || !_facade.IsTableExist(tableName))
        {
            Console.WriteLine("Table does not exist.");
            return;
        }

        switch (tableName)
        {
            case "customers":
                await HandleUpdateCustomersAsync();
                break;
            case "orders":
                await HandleUpdateOrdersAsync();
                break;
            case "shipments":
                await HandleUpdateShipmentsAsync();
                break;
            default:
                Console.WriteLine("Unsupported table.");
                break;
        }
    }

    private async Task HandleUpdateCustomersAsync()
    {
        Console.Write("Enter customer ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out var customerId) || customerId <= 0)
        {
            Console.WriteLine("Invalid customer ID.");
            return;
        }

        var customer = await _facade.Customers.GetByIdAsync(customerId);
        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }

        Console.Write($"Enter new name (current: {customer.Name}): ");
        var name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Invalid name.");
            return;
        }

        Console.Write($"Enter new address (current: {customer.Address}): ");
        var address = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(address))
        {
            Console.WriteLine("Invalid address.");
            return;
        }

        Console.Write($"Enter new age (current: {customer.Age}): ");
        if (!int.TryParse(Console.ReadLine(), out var age) || age <= 0)
        {
            Console.WriteLine("Invalid age.");
            return;
        }

        await _facade.Customers.UpdateAddressByIdAsync(customerId, address);
        await _facade.Customers.UpdateAgeByIdAsync(customerId, age);
        await _facade.Customers.UpdateNameByIdAsync(customerId, name);
        Console.WriteLine("Customer updated successfully.");
    }

    private async Task HandleUpdateOrdersAsync()
    {
        Console.Write("Enter order ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out var orderId) || orderId <= 0)
        {
            Console.WriteLine("Invalid order ID.");
            return;
        }

        var order = await _facade.Orders.GetByIdAsync(orderId);
        if (order == null)
        {
            Console.WriteLine("Order not found.");
            return;
        }

        Console.Write($"Enter new order date (current: {order.Date:yyyy-MM-dd}): ");
        if (!DateTime.TryParse(Console.ReadLine(), out var date))
        {
            Console.WriteLine("Invalid date.");
            return;
        }

        Console.Write($"Enter new price (current: {order.Price}): ");
        if (!double.TryParse(Console.ReadLine(), out var price) || price <= 0)
        {
            Console.WriteLine("Invalid price.");
            return;
        }

        await _facade.Orders.UpdateDateByIdAsync(orderId, date);
        await _facade.Orders.UpdatePriceByIdAsync(orderId, price);
        Console.WriteLine("Order updated successfully.");
    }

    private async Task HandleUpdateShipmentsAsync()
    {
        Console.Write("Enter shipment ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out var shipmentId) || shipmentId <= 0)
        {
            Console.WriteLine("Invalid shipment ID.");
            return;
        }

        var shipment = await _facade.Shipments.GetByIdAsync(shipmentId);
        if (shipment == null)
        {
            Console.WriteLine("Shipment not found.");
            return;
        }

        Console.Write($"Enter new courier info (current: {shipment.CourierInfo}): ");
        var courierInfo = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(courierInfo))
        {
            Console.WriteLine("Invalid courier info.");
            return;
        }

        await _facade.Shipments.UpdateCourierInfoByIdAsync(shipmentId, courierInfo);
        Console.WriteLine("Shipment updated successfully.");
    }

    private async Task HandleDeleteAsync()
    {
        Console.Write("Enter table name: ");
        var tableName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(tableName) || !_facade.IsTableExist(tableName))
        {
            Console.WriteLine("Table does not exist.");
            return;
        }

        Console.Write("Enter the ID of the record to delete: ");
        if (!int.TryParse(Console.ReadLine(), out var id) || id <= 0)
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        switch (tableName.ToLower())
        {
            case "customers":
                await HandleDeleteCustomerAsync(id);
                break;
            case "orders":
                await HandleDeleteOrderAsync(id);
                break;
            case "shipments":
                await HandleDeleteShipmentAsync(id);
                break;
            default:
                Console.WriteLine("Unsupported table.");
                break;
        }
    }

    private async Task HandleDeleteCustomerAsync(int id)
    {
        var customerToDelete = await _facade.Customers.GetByIdAsync(id);
        if (customerToDelete != null)
        {
            await _facade.Customers.RemoveByIdAsync(customerToDelete.Id);
            Console.WriteLine("Customer deleted successfully.");
        }
        else
        {
            Console.WriteLine("Customer not found.");
        }
    }

    private async Task HandleDeleteOrderAsync(int id)
    {
        var orderToDelete = await _facade.Orders.GetByIdAsync(id);
        if (orderToDelete != null)
        {
            await _facade.Orders.RemoveByIdAsync(orderToDelete.Id);
            Console.WriteLine("Order deleted successfully.");
        }
        else
        {
            Console.WriteLine("Order not found.");
        }
    }

    private async Task HandleDeleteShipmentAsync(int id)
    {
        var shipmentToDelete = await _facade.Shipments.GetByIdAsync(id);
        if (shipmentToDelete != null)
        {
            await _facade.Shipments.RemoveByIdAsync(shipmentToDelete.Id);
            Console.WriteLine("Shipment deleted successfully.");
        }
        else
        {
            Console.WriteLine("Shipment not found.");
        }
    }

    private async Task HandleInsertAsync()
    {
        Console.Write("Enter table name: ");
        var tableName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(tableName) || !_facade.IsTableExist(tableName))
        {
            Console.WriteLine("Table does not exist.");
            return;
        }

        switch (tableName.ToLower())
        {
            case "customers":
                await HandleInsertCustomersAsync();
                break;
            case "orders":
                await HandleInsertOrdersAsync();
                break;
            case "shipments":
                await HandleInsertShipmentsAsync();
                break;
            default:
                Console.WriteLine("Unsupported table.");
                break;
        }
    }

    private async Task HandleInsertCustomersAsync()
    {
        Console.Write("Enter customer name: ");
        var name = Console.ReadLine();

        Console.Write("Enter address: ");
        var address = Console.ReadLine();

        Console.Write("Enter age: ");
        if (!int.TryParse(Console.ReadLine(), out var age) || age <= 0)
        {
            Console.WriteLine("Invalid age.");
            return;
        }

        var customer = new Customer
        {
            Name = name,
            Address = address,
            Age = age
        };

        await _facade.Customers.AddAsync(customer);
        Console.WriteLine("Customer inserted successfully.");
    }

    private async Task HandleInsertOrdersAsync()
    {
        Console.Write("Enter customer ID: ");
        if (!int.TryParse(Console.ReadLine(), out var customerId) || customerId <= 0)
        {
            Console.WriteLine("Invalid customer ID.");
            return;
        }

        Console.Write("Enter order date (yyyy-MM-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out var date))
        {
            Console.WriteLine("Invalid date.");
            return;
        }

        Console.Write("Enter price: ");
        if (!double.TryParse(Console.ReadLine(), out var price) || price <= 0)
        {
            Console.WriteLine("Invalid price.");
            return;
        }

        var order = new Order
        {
            CustomerId = customerId,
            Date = date,
            Price = price
        };

        await _facade.Orders.AddAsync(order);
        Console.WriteLine("Order inserted successfully.");
    }

    private async Task HandleInsertShipmentsAsync()
    {
        Console.Write("Enter order ID: ");
        if (!int.TryParse(Console.ReadLine(), out var orderId) || orderId <= 0)
        {
            Console.WriteLine("Invalid order ID.");
            return;
        }

        Console.Write("Enter courier info: ");
        var courierInfo = Console.ReadLine();

        var shipment = new Shipment
        {
            OrderId = orderId,
            CourierInfo = courierInfo
        };

        await _facade.Shipments.AddAsync(shipment);
        Console.WriteLine("Shipment inserted successfully.");
    }
}