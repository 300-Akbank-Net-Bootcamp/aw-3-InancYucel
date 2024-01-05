using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vb.Base.Entity;

namespace Vb.Data.Entity;
[Table("EftTransaction", Schema = "dbo")]

public class EftTransaction : BaseEntityWithId
{
    public int AccountId { get; set; }
    public virtual Account Account { get; set; }
    public string ReferenceNumber { get; set; }
    public DateTime TransactionDate { get; set; }
    
    [Precision(18, 4)]
    public decimal Amount { get; set; }
    public string Description { get; set; }
    
    public string SenderAccount { get; set; }
    public string SenderBank { get; set; }
    public string SenderIban { get; set; }
    public string SenderName { get; set; }
     
}

public class EftTransactionConfiguration : IEntityTypeConfiguration<EftTransaction>
{
    public void Configure(EntityTypeBuilder<EftTransaction> builder)
    {
        builder.Property(z => z.InsertDate).IsRequired(true);
        builder.Property(z => z.InsertUserId).IsRequired(true);
        
        //Fields where data entry is not required
        builder.Property(z => z.UpdateDate).IsRequired(false);
        builder.Property(z => z.UpdateUserId).IsRequired(false);
        
        //It will start with a default value
        builder.Property(z => z.IsActive).IsRequired(true).HasDefaultValue(true);
        
        builder.Property(z => z.AccountId).IsRequired(true);
        builder.Property(z => z.TransactionDate).IsRequired(true);
        builder.Property(z => z.Amount).IsRequired(true).HasColumnType("decimal(18,4)");
        builder.Property(z => z.Description).IsRequired(false).HasMaxLength(300);
        builder.Property(z => z.ReferenceNumber).IsRequired(true).HasMaxLength(50);
        builder.Property(z => z.SenderAccount).IsRequired(true).HasMaxLength(50);
        builder.Property(z => z.SenderIban).IsRequired(true).HasMaxLength(50);
        builder.Property(z => z.SenderName).IsRequired(true).HasMaxLength(50);
        
        //Indexing so that results come quickly
        builder.HasIndex(z => z.ReferenceNumber);
    }
}
