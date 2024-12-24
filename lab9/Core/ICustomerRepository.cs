using DatabaseModels;

namespace Core;

public interface ICustomerRepository
{
    Task AddAsync(Customer customer);
    
    Task RemoveByIdAsync(int customerId);

    Task UpdateNameByIdAsync(int customerId, string name); 
    
    Task UpdateAddressByIdAsync(int customerId, string address); 
    
    Task UpdateAgeByIdAsync(int customerId, int age); 
    
    Task<Customer?> GetByIdAsync(int customerId);
    
    IQueryable<Customer> GetAll();
}