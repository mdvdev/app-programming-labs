using DatabaseModels;

namespace Core;

public interface IOrderRepository
{
    Task AddAsync(Order order);

    Task RemoveByIdAsync(int orderId);
    
    Task UpdatePriceByIdAsync(int orderId, double price); 
    
    Task UpdateDateByIdAsync(int orderId, DateTime date);

    Task<Order?> GetByIdAsync(int orderId);

    IQueryable<Order> GetAll();
}