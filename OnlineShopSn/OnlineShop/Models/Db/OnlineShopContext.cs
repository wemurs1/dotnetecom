namespace OnlineShop.Models.Db;

public partial class OnlineShopContext(DbContextOptions<OnlineShopContext> options) : DbContext(options)
{
    public virtual DbSet<Menu> Menus { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Menu>(entity =>
        {
            entity.Property(e => e.Link).HasMaxLength(300).IsRequired();
            entity.Property(e => e.MenuTitle).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Type).HasMaxLength(20).IsRequired();
        });
    }
}
