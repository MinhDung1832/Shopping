using Shopping.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.Models
{
	public class ProductModel
	{
		[Key]
		public int id { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập tên sản phẩm")]
		public string name { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập mô tả sản phẩm")]
		public string description { get; set; }
		public string slug { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập giá sản phẩm")]
		[Range(0.01, double.MaxValue)]
		[Column(TypeName = "decimal(8,2)")]
		public decimal price { get; set; }
		public string image { get; set; }
		[Required, Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn 1 thương hiệu")]
		public int brandId { get; set; }
		[Required, Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn 1 thương danh mục")]
		public int categoryId { get; set; }
		public CategoryModel category { get; set; }
		public BrandModel brand { get; set; }
		[NotMapped]
		[FileExtension]
		public IFormFile? ImageUpload { get; set; }
	}
}
