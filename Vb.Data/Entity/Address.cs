using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vb.Base.Entity;

namespace Vb.Data.Entity;
[Table("Address", Schema = "dbo")]

public class Address : BaseEntityWithId
{
    public int CustomerId { get; set; }
    public virtual Customer Customer { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string County { get; set; }
    public string PostalCode { get; set; }
    public bool IsDefault { get; set; }
    
}

//The codes we wrote to specify the fields and their properties in the database
//Migrations are created with these codes and they are added to the database.
public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.Property(z => z.InsertDate).IsRequired(true);
        builder.Property(z => z.InsertUserId).IsRequired(true);
        
        //Fields where data entry is not required
        builder.Property(z => z.UpdateDate).IsRequired(false);
        builder.Property(z => z.UpdateUserId).IsRequired(false);
        
        //It will start with a default value
        builder.Property(z => z.IsActive).IsRequired(true).HasDefaultValue(true);
        
        builder.Property(z => z.CustomerId).IsRequired(true);
        builder.Property(z => z.Address1).IsRequired(true).HasMaxLength(150);
        builder.Property(z => z.Address2).IsRequired(false).HasMaxLength(150);
        builder.Property(z => z.Country).IsRequired(true).HasMaxLength(100);
        builder.Property(z => z.City).IsRequired(true).HasMaxLength(100);
        builder.Property(z => z.County).IsRequired(false).HasMaxLength(100);
        builder.Property(z => z.PostalCode).IsRequired(false).HasMaxLength(10);
        
        //It will not start with a default value
        builder.Property(z => z.IsDefault).IsRequired(true).HasDefaultValue(false);
        
        builder.HasIndex(z => z.CustomerId);
    }
}