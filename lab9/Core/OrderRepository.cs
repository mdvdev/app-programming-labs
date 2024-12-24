using DatabaseContext;
using DatabaseModels;

namespace Core;

public class OrderRepository : IOrderRepository
{
    private readonly ShopContext _db;

    public OrderRepository(ShopContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Order order)
    {
        await _db.Orders.AddAsync(order);
        await _db.SaveChangesAsync();
    }

    public async Task RemoveByIdAsync(int orderId)
    {
        var order = await _db.Orders.FindAsync(orderId);

        if (order == null)
        {
            throw new InvalidOperationException($"Order with ID {orderId} not found.");
        }

        _db.Orders.Remove(order);
        await _db.SaveChangesAsync();
    }

    public async Task UpdatePriceByIdAsync(int orderId, double price)
    {
        var order = await _db.Orders.FindAsync(orderId);

        if (order != null)
        {
            order.Price = price;
            await _db.SaveChangesAsync();
        }
    }

    public async Task UpdateDateByIdAsync(int orderId, DateTime date)
    {
        var order = await _db.Orders.FindAsync(orderId);

        if (order != null)
        {
            order.Date = date;
            await _db.SaveChangesAsync();
        }
    }

    public async Task<Order?> GetByIdAsync(int orderId)
    {
        return await _db.Orders.FindAsync(orderId);
    }

    public IQueryable<Order> GetAll()
    {
        return _db.Orders.AsQueryable();
    }
}