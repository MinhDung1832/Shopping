using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using Shopping.Repository;

namespace Shopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Products.OrderByDescending(p => p.id).Include(p => p.category).Include(p => p.brand).ToListAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Category = new SelectList(_dataContext.Categories, "id", "name");
            ViewBag.Brand = new SelectList(_dataContext.Brands, "id", "name");


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel product)
        {
            ViewBag.Category = new SelectList(_dataContext.Categories, "id", "name", product.categoryId);
            ViewBag.Brand = new SelectList(_dataContext.Brands, "id", "name", product.brandId);
            if (ModelState.IsValid)
            {
                product.slug = product.name.Replace(" ", "-");
                var slug = await _dataContext.Products.FirstOrDefaultAsync(s => s.slug == product.slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Sản phẩm đã tồn tại");
                    return View(product);
                }

                if (product.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();

                    product.image = imageName;
                }

                _dataContext.Add(product);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm sản phẩm thành công!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Lỗi";
                List<string> errors = new List<string>();
                foreach (var item in ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int id)
        {
            ProductModel product = await _dataContext.Products.FirstOrDefaultAsync(p => p.id == id);
            ViewBag.Category = new SelectList(_dataContext.Categories, "id", "name", product.categoryId);
            ViewBag.Brand = new SelectList(_dataContext.Brands, "id", "name", product.brandId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductModel product)
        {
            ViewBag.Category = new SelectList(_dataContext.Categories, "id", "name", product.categoryId);
            ViewBag.Brand = new SelectList(_dataContext.Brands, "id", "name", product.brandId);

            var existed_product = _dataContext.Products.Find(product.id);
            if (ModelState.IsValid)
            {
                product.slug = product.name.Replace(" ", "-");
                var slug = await _dataContext.Products.FirstOrDefaultAsync(s => s.slug == product.slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Sản phẩm đã tồn tại");
                    return View(product);
                }

                if (product.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);
                    try
                    {
                        string oldfilePath = Path.Combine(uploadDir, existed_product.image);
                        if (System.IO.File.Exists(oldfilePath))
                        {
                            System.IO.File.Delete(oldfilePath);
                        }

                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Lỗi xảy ra khi xóa ảnh");
                    }
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();

                    existed_product.image = imageName;

                }
                existed_product.name = product.name;
                //existed_product.slug = product.slug;
                existed_product.description = product.description;
                existed_product.price = product.price;
                existed_product.categoryId = product.categoryId;
                existed_product.brandId = product.brandId;

                _dataContext.Update(existed_product);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Lỗi";
                List<string> errors = new List<string>();
                foreach (var item in ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
            return View(product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            ProductModel product = await _dataContext.Products.FirstOrDefaultAsync(p => p.id == id);

            if (!string.Equals(product.image, "noname.jpg"))
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                string filePath = Path.Combine(uploadDir, product.image);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            _dataContext.Products.Remove(product);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Xóa sản phẩm thành công";
            return RedirectToAction("Index");
        }
    }
}
