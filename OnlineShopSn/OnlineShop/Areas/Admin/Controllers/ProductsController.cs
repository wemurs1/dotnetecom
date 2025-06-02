using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController(OnlineShopContext context) : Controller
    {
        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            return View(await context.Products.ToListAsync());
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Title,Description,FullDesc,Price,Discount,ImageName,Qty,Tags,VideoUrl")] Product product,
            IFormFile? MainImage,
            IFormFile[]? GalleryImages
        )
        {
            if (ModelState.IsValid)
            {
                if (MainImage != null)
                {
                    product.ImageName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(MainImage.FileName);
                    string ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products", product.ImageName);
                    using var stream = new FileStream(ImagePath, FileMode.Create);
                    MainImage.CopyTo(stream);
                }
                context.Add(product);
                await context.SaveChangesAsync();
                if (GalleryImages != null)
                {
                    foreach (var item in GalleryImages)
                    {
                        var newGallery = new ProductGallery();
                        newGallery.ProductId = product.Id;
                        newGallery.ImageName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(item.FileName);
                        string ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products", newGallery.ImageName);
                        using var stream = new FileStream(ImagePath, FileMode.Create);
                        item.CopyTo(stream);
                        context.ProductGalleries.Add(newGallery);
                    }
                    await context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["gallery"] = await context.ProductGalleries.Where(x => x.ProductId == product.Id).ToListAsync();
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,FullDesc,Price,Discount,ImageName,Qty,Tags,VideoUrl")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(product);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await context.Products.FindAsync(id);
            if (product != null)
            {
                context.Products.Remove(product);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return context.Products.Any(e => e.Id == id);
        }
    }
}
