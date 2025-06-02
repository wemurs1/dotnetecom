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
                    string ImagePath = GetFullFileName(product.ImageName);
                    using var stream = new FileStream(ImagePath, FileMode.Create);
                    await MainImage.CopyToAsync(stream);
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
                        string ImagePath = GetFullFileName(newGallery.ImageName);
                        using var stream = new FileStream(ImagePath, FileMode.Create);
                        await item.CopyToAsync(stream);
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

            var product = await context.Products.Include(x => x.Gallery).FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Title,Description,FullDesc,Price,Discount,ImageName,Qty,Tags,VideoUrl")] Product product,
            IFormFile? MainImage,
            IFormFile[]? GalleryImages
        )
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (MainImage != null)
                    {
                        string fn = GetFullFileName(product.ImageName!);
                        if (System.IO.File.Exists(fn)) System.IO.File.Delete(fn);
                        using var stream = new FileStream(fn, FileMode.Create);
                        await MainImage.CopyToAsync(stream);
                    }
                    if (GalleryImages != null)
                    {
                        foreach (var item in GalleryImages)
                        {
                            var imageName = Guid.NewGuid() + Path.GetExtension(item.FileName);
                            var fn = GetFullFileName(imageName);
                            using var stream = new FileStream(fn, FileMode.Create);
                            await item.CopyToAsync(stream);
                            var galleryItem = new ProductGallery { ImageName = imageName, ProductId = product.Id };
                            context.ProductGalleries.Add(galleryItem);
                        }
                    }
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

        // GET: Admin/Products/DeleteGalley/5
        public async Task<IActionResult> DeleteGallery(int id)
        {
            var gallery = await context.ProductGalleries.FirstOrDefaultAsync(x => x.Id == id);
            if (gallery == null) return NotFound();

            string fn = GetFullFileName(gallery.ImageName!);
            if (System.IO.File.Exists(fn)) System.IO.File.Delete(fn);
            context.ProductGalleries.Remove(gallery);
            await context.SaveChangesAsync();
            return Redirect($"edit/{gallery.ProductId}");
        }

        private string GetFullFileName(string imageName)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products", imageName);
        }
    }
}
