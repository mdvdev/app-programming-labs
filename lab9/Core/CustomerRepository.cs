using DatabaseContext;
using DatabaseModels;

namespace Core;

public class CustomerRepository : ICustomerRepository
{
    private readonly ShopContext _db;
    
    public CustomerRepository(ShopContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Customer customer)
    {
        await _db.Customers.AddAsync(customer);
        await _db.SaveChangesAsync();
    }

    public async Task RemoveByIdAsync(int customerId)
    {
        var customer = await _db.Customers.FindAsync(customerId);

        if (customer == null)
        {
            throw new InvalidOperationException($"Customer with ID {customerId} not found.");
        }

        _db.Customers.Remove(customer);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateNameByIdAsync(int customerId, string name)
    {
        var customer = await _db.Customers.FindAsync(customerId);

        if (customer != null)
        {
            customer.Name = name;
            await _db.SaveChangesAsync();
        }
    }

    public async Task UpdateAddressByIdAsync(int customerId, string address)
    {
        var customer = await _db.Customers.FindAsync(customerId);

        if (customer != null)
        {
            customer.Address = address;
            await _db.SaveChangesAsync();
        }
    }

    public async Task UpdateAgeByIdAsync(int customerId, int age)
    {
        var customer = await _db.Customers.FindAsync(customerId);

        if (customer != null)
        {
            customer.Age = age;
            await _db.SaveChangesAsync();
        }
    }

    public async Task<Customer?> GetByIdAsync(int customerId)
    {
        return await _db.Customers.FindAsync(customerId);
    }

    public IQueryable<Customer> GetAll()
    {
        return _db.Customers.AsQueryable();
    }
}