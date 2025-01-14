﻿using Shopping.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.Models
{

	public class ContactModel
	{
		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập tiêu đề website")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Yêu cầu nhập Bản đồ")]
		public string Map { get; set; }

		[Required(ErrorMessage = "Yêu cầu nhập Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Yêu cầu nhập Phone")]
		public string Phone { get; set; }

		[Required(ErrorMessage = "Yêu cầu nhập thông tin liên hệ")]
		public string Description { get; set; }

		public string LogoImg { get; set; }

		[NotMapped]
		[FileExtension]
		public IFormFile? ImageUpload { get; set; }
	}
}
