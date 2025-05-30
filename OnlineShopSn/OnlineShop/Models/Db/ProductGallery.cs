namespace OnlineShop.Models.Db;

public class ProductGallery
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public virtual Product Product { get; set; } = default!;
    public string? ImageName { get; set; }
}
