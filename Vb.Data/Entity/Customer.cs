using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vb.Base.Entity;

namespace Vb.Data.Entity;
[Table("Customer", Schema = "dbo")]

public class Customer : BaseEntity
{
    public string IdentityNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int CustomerNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime LastActivityDate { get; set; }
    public virtual List<Address> Addresses { get; set; }
    public virtual List<Contact> Contacts { get; set; }
    public virtual List<Account> Accounts { get; set; }
}

//The codes we wrote to specify the fields and their properties in the database
//Migrations are created with these codes and they are added to the database.
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        // Not assigning values automatically
        builder.Property(z => z.CustomerNumber).ValueGeneratedNever(); 
        
        builder.Property(z => z.InsertDate).IsRequired(true);
        builder.Property(z => z.InsertUserId).IsRequired(true);
        
        //Fields where data entry is not required
        builder.Property(z => z.UpdateDate).IsRequired(false);
        builder.Property(z => z.UpdateUserId).IsRequired(false);
        
        //It will start with a default value
        builder.Property(z => z.IsActive).IsRequired(true).HasDefaultValue(true);
        
        builder.Property(z => z.IdentityNumber).IsRequired(true).HasMaxLength(11);
        builder.Property(z => z.FirstName).IsRequired(true).HasMaxLength(50);
        builder.Property(z => z.LastName).IsRequired(true).HasMaxLength(50);
        builder.Property(z => z.CustomerNumber).IsRequired(true);
        builder.Property(z => z.DateOfBirth).IsRequired(true);
        builder.Property(z => z.LastActivityDate).IsRequired(true);

        //Indexing so that results come quickly
        //There cannot be another identical value in IdentiyNumbers and CustomerNumbers
        builder.HasIndex(z => z.IdentityNumber).IsUnique(true);
        builder.HasIndex(z => z.CustomerNumber).IsUnique(true);
        builder.HasKey(z => z.CustomerNumber);

        //A Customer can have multiple Accounts
        builder.HasMany(z => z.Accounts)
            .WithOne(z => z.Customer)
            .HasForeignKey(z => z.CustomerId).IsRequired(true);
        
        //A Customer can have multiple Contacts
        builder.HasMany(z => z.Contacts)
            .WithOne(z => z.Customer)
            .HasForeignKey(z => z.CustomerId).IsRequired(true);
        
        //A Customer can have multiple Addresses
        builder.HasMany(z => z.Addresses)
            .WithOne(z => z.Customer)
            .HasForeignKey(z => z.CustomerId).IsRequired(true);
    }
}
