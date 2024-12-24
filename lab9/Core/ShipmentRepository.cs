using DatabaseContext;
using DatabaseModels;

namespace Core;

public class ShipmentRepository : IShipmentRepository
{
    private readonly ShopContext _db;

    public ShipmentRepository(ShopContext db)
    {
        _db = db;
    }
    
    public async Task AddAsync(Shipment shipment)
    {
        await _db.Shipments.AddAsync(shipment);
        await _db.SaveChangesAsync();
    }

    public async Task RemoveByIdAsync(int shipmentId)
    {
        var shipment = await _db.Shipments.FindAsync(shipmentId);

        if (shipment == null)
        {
            throw new InvalidOperationException($"Shipment with ID {shipmentId} not found.");
        }

        _db.Shipments.Remove(shipment);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateCourierInfoByIdAsync(int shipmentId, string courierInfo)
    {
        var shipment = await _db.Shipments.FindAsync(shipmentId);

        if (shipment != null)
        {
            shipment.CourierInfo = courierInfo;
            await _db.SaveChangesAsync();
        }
    }

    public async Task<Shipment?> GetByIdAsync(int shipmentId)
    {
        return await _db.Shipments.FindAsync(shipmentId);
    }

    public IQueryable<Shipment> GetAll()
    {
        return _db.Shipments.AsQueryable();
    }
}