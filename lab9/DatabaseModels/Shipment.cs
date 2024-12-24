using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseModels;

[Table("shipments")]
public class Shipment
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [ForeignKey("OrderId")]
    [Column("order_id")]
    public int OrderId { get; set; }
    
    [Column("courier_info")]
    public string CourierInfo { get; set; }
    
    public override string ToString() => $"ID: {Id}, OrderId: {OrderId}, CourierInfo: {CourierInfo}";
}