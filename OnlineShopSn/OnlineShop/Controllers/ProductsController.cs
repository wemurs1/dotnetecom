namespace OnlineShop.Controllers
{
    public class ProductsController(OnlineShopContext context) : Controller
    {
        // GET: ProductsController
        public async Task<ActionResult> Index()
        {
            List<Product> products = await context.Products.OrderByDescending(x => x.Id).ToListAsync();
            return View(products);
        }
    }
}
