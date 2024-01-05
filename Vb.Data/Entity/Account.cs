using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vb.Base.Entity;

namespace Vb.Data.Entity;
[Table("Account", Schema = "dbo")]
public class Account : BaseEntity
{
    public int CustomerId { get; set; }
    public virtual Customer Customer { get; set; }
    public int AccountNumber { get; set; }
    public string IBAN { get; set; }
    public decimal Balance { get; set; }
    public string CurrencyType { get; set; }
    public string Name { get; set; }
    public DateTime OpenDate { get; set; }
    
    public virtual List<AccountTransaction> AccountTransactions { get; set; }
    public virtual List<EftTransaction> EftTransaction { get; set; }
}

//The codes we wrote to specify the fields and their properties in the database
//Migrations are created with these codes and they are added to the database.
public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        // Not assigning values automatically
        builder.Property(z => z.AccountNumber).ValueGeneratedNever(); 
        
        builder.Property(z => z.InsertDate).IsRequired(true);
        builder.Property(z => z.InsertUserId).IsRequired(true);
        
        //Fields where data entry is not required
        builder.Property(z => z.UpdateDate).IsRequired(false);
        builder.Property(z => z.UpdateUserId).IsRequired(false);
        
        //It will start with a default value
        builder.Property(z => z.IsActive).IsRequired(true).HasDefaultValue(true); 
        
        builder.Property(z => z.CustomerId).IsRequired(true);
        builder.Property(z => z.AccountNumber).IsRequired(true);
        builder.Property(z => z.IBAN).IsRequired(true).HasMaxLength(34);
        builder.Property(z => z.Balance).IsRequired(true).HasPrecision(18,4);
        builder.Property(z => z.CurrencyType).IsRequired(true).HasMaxLength(100); 
        builder.Property(z => z.Name).IsRequired(false).HasMaxLength(100);
        builder.Property(z => z.OpenDate).IsRequired(true);

        builder.HasIndex(z => z.CustomerId);
        builder.HasIndex(z => z.AccountNumber).IsUnique(true);
        builder.HasKey(z => z.AccountNumber);

        // An account can have multiple AccountTransactions. Foreign key definition
        builder.HasMany(z => z.AccountTransactions)
            .WithOne(z => z.Account)
            .HasForeignKey(z => z.AccountId).IsRequired(true);
        
        //An account can have multiple EftTransaction. Foreign key definition
        builder.HasMany(z => z.EftTransaction)
            .WithOne(z => z.Account)
            .HasForeignKey(z => z.AccountId).IsRequired(true);
    }
}