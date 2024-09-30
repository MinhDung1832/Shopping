using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class CategoryModel
    {
        [Key]
        public int id { get; set; }
        [Required,MinLength(4,ErrorMessage = "Yêu cầu nhập tên danh mục")]
        public string name { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập mô tả danh mục")]
		public string description { get; set; }
		public string slug { get; set; }
        public int status { get; set; }
    }
}
