using Microsoft.EntityFrameworkCore;

namespace HITWashing.Models.DBClass
{
    public class WashingContext : DbContext
    {
        public WashingContext(DbContextOptions<WashingContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
        public virtual DbSet<AccountModel> AccountModels { get; set; }
        public virtual DbSet<BalanceModel> Balances { get; set; }
        public virtual DbSet<BorrowModel> Borrows { get; set; }
        public virtual DbSet<PaybackModel> Paybacks { get; set; }
        public virtual DbSet<ItemModel> Items { get; set; }
        public virtual DbSet<WarehouseModel> Warehouses { get; set; }
    }
}
