using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using Shopping.Repository;

namespace Shopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BrandController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Brands.OrderByDescending(p => p.id).ToListAsync());
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
        public async Task<IActionResult> Create(BrandModel brand)
        {
            if (ModelState.IsValid)
            {
                brand.slug = brand.name.Replace(" ", "-");
                var slug = await _dataContext.Brands.FirstOrDefaultAsync(s => s.slug == brand.slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Thương hiệu đã tồn tại");
                    return View(brand);
                }

                _dataContext.Add(brand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm thương hiệu thành công!";
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
            return View(brand);
        }

        public async Task<IActionResult> Edit(int id)
        {
            BrandModel brand = await _dataContext.Brands.FirstOrDefaultAsync(p => p.id == id);
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BrandModel brand)
        {
            var existed_brand = _dataContext.Brands.Find(brand.id);
            if (ModelState.IsValid)
            {
                brand.slug = brand.name.Replace(" ", "-");
                //var slug = await _dataContext.Categories.FirstOrDefaultAsync(s => s.slug == category.slug);
                //if (slug != null)
                //{
                //    ModelState.AddModelError("", "Danh mục đã tồn tại");
                //    return View(category);
                //}
                existed_brand.name = brand.name;
                existed_brand.slug = brand.slug;
                existed_brand.description = brand.description;
                existed_brand.status = brand.status;

                _dataContext.Update(existed_brand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Cập nhật thương hiệu thành công!";
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
            return View(brand);
        }

        public async Task<IActionResult> Delete(int id)
        {
            BrandModel brand = await _dataContext.Brands.FirstOrDefaultAsync(p => p.id == id);

            _dataContext.Brands.Remove(brand);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Xóa thương hiệu thành công";
            return RedirectToAction("Index");
        }
    }
}
