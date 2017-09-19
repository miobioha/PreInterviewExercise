using Banking.Core.Aggregates;
using System.Data.Entity;

namespace Banking.Core.ReadModel.Implementation
{
    public class BankContext : DbContext
    {
        public BankContext() : base("name=BankDbContext")
        {
        }

        public DbSet<CashCard> CashCards { get; set; }

        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CashCard>()
                .HasKey(card => card.Id);

            modelBuilder.Entity<Account>()
                .HasKey(account => account.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
