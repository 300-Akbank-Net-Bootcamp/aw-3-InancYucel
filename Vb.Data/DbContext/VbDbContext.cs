using Microsoft.EntityFrameworkCore;
using Vb.Data.Entity;

namespace Vb.Data;

public class VbDbContext : DbContext //DbContext comes from EntityFrameWorkCore
{
    public VbDbContext(DbContextOptions<VbDbContext> options): base(options) //DbContextOptions<options>
    {
        
    }
    
    //DB sets
    public DbSet <Account> Accounts { get; set; }
    public DbSet <AccountTransaction> AccountTransactions { get; set; }
    public DbSet <Address> Address { get; set; }
    public DbSet <Contact> Contact { get; set; }
    public DbSet <Customer> Customers { get; set; }
    public DbSet <EftTransaction> EftTransactions { get; set; }

     protected override void OnModelCreating(ModelBuilder modelBuilder)
     {
         modelBuilder.ApplyConfiguration(new AccountConfiguration());
         modelBuilder.ApplyConfiguration(new AccountTransactionConfiguration());
         modelBuilder.ApplyConfiguration(new AddressConfiguration());
         modelBuilder.ApplyConfiguration(new ContactConfiguration());
         modelBuilder.ApplyConfiguration(new CustomerConfiguration());
         base.OnModelCreating(modelBuilder);
     }
}