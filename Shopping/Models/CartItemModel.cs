using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class CartItemModel
	{
		[Key]
		public long productId { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập tên thương hiệu")]
		public string productName { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập mô tả thương hiệu")]
		public int quantity { get; set; }
		public decimal price { get; set; }
		public decimal amount {
			get { return quantity * price; }
		}
		public string image { get; set; }

		public CartItemModel() { }
		public CartItemModel(ProductModel product) {
			productId = product.id;
			productName = product.name;
			price = product.price;
			quantity = 1;
			image = product.image;
		}
	}
}
