using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using Shopping.Repository;

namespace Shopping.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Route("Admin/Brand")]
	[Authorize(Roles = "Admin")]
	public class BrandController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BrandController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = context;
            _webHostEnvironment = webHostEnvironment;
        }

		//[Route("Index")]
		//public async Task<IActionResult> Index()
  //      {
  //          return View(await _dataContext.Brands.OrderByDescending(p => p.id).ToListAsync());
  //      }

		[Route("Index")]
		public async Task<IActionResult> Index(int pg = 1)
		{
			List<BrandModel> brand = _dataContext.Brands.ToList();


			const int pageSize = 10;

			if (pg < 1)
			{
				pg = 1;
			}
			int recsCount = brand.Count();

			var pager = new Paginate(recsCount, pg, pageSize);

			int recSkip = (pg - 1) * pageSize;

			var data = brand.Skip(recSkip).Take(pager.PageSize).ToList();

			ViewBag.Pager = pager;

			return View(data);
		}

        [Route("Create")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Category = new SelectList(_dataContext.Categories, "id", "name");
            ViewBag.Brand = new SelectList(_dataContext.Brands, "id", "name");


            return View();
        }

        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandModel brand)
        {
            if (ModelState.IsValid)
            {
                brand.Slug = brand.Name.Replace(" ", "-");
                var slug = await _dataContext.Brands.FirstOrDefaultAsync(s => s.Slug == brand.Slug);
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

        [Route("Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            BrandModel brand = await _dataContext.Brands.FirstOrDefaultAsync(p => p.Id == id);
            return View(brand);
        }

        [Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BrandModel brand)
        {
            var existed_brand = _dataContext.Brands.Find(brand.Id);
            if (ModelState.IsValid)
            {
                brand.Slug = brand.Name.Replace(" ", "-");
                //var slug = await _dataContext.Categories.FirstOrDefaultAsync(s => s.slug == category.slug);
                //if (slug != null)
                //{
                //    ModelState.AddModelError("", "Danh mục đã tồn tại");
                //    return View(category);
                //}
                existed_brand.Name = brand.Name;
                existed_brand.Slug = brand.Slug;
                existed_brand.Description = brand.Description;
                existed_brand.Status = brand.Status;

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
            BrandModel brand = await _dataContext.Brands.FirstOrDefaultAsync(p => p.Id == id);

            _dataContext.Brands.Remove(brand);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Xóa thương hiệu thành công";
            return RedirectToAction("Index");
        }
    }
}
