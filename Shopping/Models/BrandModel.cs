using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class BrandModel
    {
		[Key]
		public int id { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập tên thương hiệu")]
		public string name { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập mô tả thương hiệu")]
		public string description { get; set; }
		public string slug { get; set; }
		public int status { get; set; }
	}
}
