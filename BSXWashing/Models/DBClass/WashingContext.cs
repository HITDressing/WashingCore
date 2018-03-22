namespace BSXWashing.Models.DBClass
{
    using Microsoft.EntityFrameworkCore;
    public class WashingContext : DbContext
    {
        public WashingContext(DbContextOptions<WashingContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
        public virtual DbSet<AccountModel> AccountModels { get; set; }
        public virtual DbSet<TopupModel> TopupModels { get; set; }
        public virtual DbSet<BorrowModel> BorrowModels { get; set; }
        public virtual DbSet<PaybackModel> PaybackModels { get; set; }
        public virtual DbSet<ItemModel> ItemModels { get; set; }
        public virtual DbSet<WarehouseModel> WarehouseModels { get; set; }
    }
}