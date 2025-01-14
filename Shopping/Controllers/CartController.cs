using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using Shopping.Models.ViewModels;
using Shopping.Repository;

namespace Shopping.Controllers
{
    public class CartController : Controller
    {
        private readonly DataContext _dataContext;

        public CartController(DataContext context)
        {
            _dataContext = context;
        }
        public IActionResult Index()
        {
            List<CartItemModel> cartItem = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemViewModel cartVM = new CartItemViewModel()
            {
                CartItems = cartItem,
                GrandTotal = cartItem.Sum(s => s.Quantity * s.Price)
            };

            return View(cartVM);
        }

        public async Task<IActionResult> Add(long id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(id);
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemModel cartItem = cart.Where(s => s.ProductId == id).FirstOrDefault();

            if (cartItem == null)
            {
                cart.Add(new CartItemModel(product));
            }
            else
            {
                cartItem.Quantity += 1;
            }

            HttpContext.Session.setJson("Cart", cart);
            TempData["success"] = "Them san pham thanh cong!";
            return Redirect(Request.Headers["Referer"].ToString());
        }

		public async Task<IActionResult> Increase(int Id)
		{
			ProductModel product = await _dataContext.Products.Where(p => p.Id == Id).FirstOrDefaultAsync();

			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();
			if (cartItem.Quantity >= 1 && product.Quantity > cartItem.Quantity)
			{
				++cartItem.Quantity;
				TempData["success"] = "Increase Product to cart Sucessfully! ";
			}
			else
			{
				cartItem.Quantity = product.Quantity;
				TempData["success"] = "Maximum Product Quantity to cart Sucessfully! ";

				//cart.RemoveAll(p => p.ProductId == Id);
			}
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.setJson("Cart", cart);
			}


			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Decrease(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();
			if (cartItem.Quantity > 1)
			{
				--cartItem.Quantity;
			}
			else
			{
				cart.RemoveAll(p => p.ProductId == Id);
			}
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.setJson("Cart", cart);
			}

			TempData["success"] = "Decrease Product to cart Sucessfully! ";
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Remove(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			cart.RemoveAll(p => p.ProductId == Id);
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.setJson("Cart", cart);
			}

			TempData["success"] = "Remove Product to cart Sucessfully! ";
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Clear()
        {
            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            return View();
        }
    }
}
