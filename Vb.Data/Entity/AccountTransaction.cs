using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vb.Base.Entity;

namespace Vb.Data.Entity;
[Table("AccountTransaction", Schema = "dbo")]

public class AccountTransaction : BaseEntityWithId
{
    public int AccountId { get; set; }
    public virtual Account Account { get; set; }
    public string ReferenceNumber { get; set; }
    public DateTime TransactionDate { get; set; }
    
    [Precision(18, 2)]
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string TransferType { get; set; }

}

//The codes we wrote to specify the fields and their properties in the database
//Migrations are created with these codes and they are added to the database.
public class AccountTransactionConfiguration : IEntityTypeConfiguration<AccountTransaction>
{
    public void Configure(EntityTypeBuilder<AccountTransaction> builder)
    {
        builder.Property(z => z.InsertDate).IsRequired(true);
        builder.Property(z => z.InsertUserId).IsRequired(true);
        builder.Property(z => z.UpdateDate).IsRequired(false);
        builder.Property(z => z.UpdateUserId).IsRequired(false);
        
        //It will start with a default value
        builder.Property(z => z.IsActive).IsRequired(true).HasDefaultValue(true);
        
        builder.Property(z => z.AccountId).IsRequired(true);
        builder.Property(z => z.TransactionDate).IsRequired(true);
        builder.Property(z => z.Amount).IsRequired(true).HasPrecision(18, 4);
        builder.Property(z => z.Description).IsRequired(false).HasMaxLength(300);
        builder.Property(z => z.TransferType).IsRequired(true).HasMaxLength(10);
        builder.Property(z => z.ReferenceNumber).IsRequired(true).HasMaxLength(50);
        
        builder.HasIndex(z => z.ReferenceNumber);
    }
}