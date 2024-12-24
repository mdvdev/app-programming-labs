using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseModels;

[Table("customers")]
public class Customer
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
    
    [Column("address")]
    public string Address { get; set; }
    
    [Column("age")]
    public int Age { get; set; }

    public override string ToString() => $"ID: {Id}, Name: {Name}, Address: {Address}, Age: {Age}";
}