using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using Shopping.Repository;

namespace Shopping.Controllers
{
    public class BrandController : Controller
    {
		private readonly DataContext _dataContext;

		public BrandController(DataContext context)
		{
			_dataContext = context;
		}
		public async Task<IActionResult> Index(string slug = "")
		{
			BrandModel brand = _dataContext.Brands.Where(s => s.Slug == slug).FirstOrDefault();
			if (brand == null) return RedirectToAction("Index");
			var productByBrand = _dataContext.Products.Where(s => s.BrandId == brand.Id);
			return View(await productByBrand.OrderByDescending(s => s.Id).ToListAsync());
		}
	}
}
