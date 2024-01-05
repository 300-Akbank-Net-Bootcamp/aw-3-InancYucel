using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vb.Base.Entity;

namespace Vb.Data.Entity;
[Table("Contact", Schema = "dbo")]

public class Contact : BaseEntityWithId
{
    public int CustomerId { get; set; }
    public string ContactType { get; set; }
    public virtual Customer Customer { get; set; }
    public string Information { get; set; }
    public bool IsDefault { get; set; }
}

//The codes we wrote to specify the fields and their properties in the database
//Migrations are created with these codes and they are added to the database.
public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.Property(z => z.InsertDate).IsRequired(true);
        builder.Property(z => z.InsertUserId).IsRequired(true);
        
        //Fields where data entry is not required
        builder.Property(z => z.UpdateDate).IsRequired(false);
        builder.Property(z => z.UpdateUserId).IsRequired(false);
        
        //It will start with a default value
        builder.Property(z => z.IsActive).IsRequired(true).HasDefaultValue(true);
        
        builder.Property(z => z.CustomerId).IsRequired(true);
        builder.Property(z => z.ContactType).IsRequired(true).HasMaxLength(10);
        builder.Property(z => z.Information).IsRequired(true).HasMaxLength(10);
        
        //It will not start with a default value
        builder.Property(z => z.IsDefault).IsRequired(true).HasDefaultValue(false);
		builder.HasIndex(z => z.CustomerId);
        
        //This table can have one of Information and ContactType.
		builder.HasIndex(z => new {z.Information, z.ContactType}).IsUnique(true);
    }
}