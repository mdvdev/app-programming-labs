using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseModels;

[Table("orders")]
public class Order
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [ForeignKey("CustomerId")]
    [Column("customer_id")]
    public int CustomerId { get; set; }

    private DateTime _date;
    
    [Column("date")]
    public DateTime Date 
    {
        get => _date;
        set => _date = value.ToUniversalTime();
    }

    [Column("price")]
    public double Price { get; set; }
    
    public override string ToString() => $"ID: {Id}, customer ID: {CustomerId}, Date: {Date}, Price: {Price}";
}