namespace OnlineShop.Models.Db;

public partial class OnlineShopContext(DbContextOptions<OnlineShopContext> options) : DbContext(options)
{
    public virtual DbSet<Menu> Menus { get; set; }
    public virtual DbSet<Banner> Banners { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<ProductGallery> ProductGalleries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Menu>(entity =>
        {
            entity.Property(e => e.Link).HasMaxLength(300).IsRequired();
            entity.Property(e => e.MenuTitle).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Type).HasMaxLength(20).IsRequired();
        });
        modelBuilder.Entity<Banner>(entity =>
        {
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.SubTitle).HasMaxLength(1000);
            entity.Property(e => e.ImageName).HasMaxLength(50);
            entity.Property(e => e.Link).HasMaxLength(100);
            entity.Property(e => e.Position).HasMaxLength(50);
        });
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.FullDesc).HasMaxLength(4000);
            entity.Property(e => e.ImageName).HasMaxLength(50);
            entity.Property(e => e.Tags).HasMaxLength(1000);
            entity.Property(e => e.VideoUrl).HasMaxLength(300);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.Discount).HasColumnType("money");
        });
        modelBuilder.Entity<ProductGallery>(entity =>
        {
            entity.Property(e => e.ImageName).HasMaxLength(50);
        });
    }
}
