using DatabaseModels;

namespace Core;

public interface IShipmentRepository
{
    Task AddAsync(Shipment shipment);

    Task RemoveByIdAsync(int shipmentId);
    
    Task UpdateCourierInfoByIdAsync(int shipmentId, string courierInfo); 
    
    Task<Shipment?> GetByIdAsync(int shipmentId);

    IQueryable<Shipment> GetAll();
}