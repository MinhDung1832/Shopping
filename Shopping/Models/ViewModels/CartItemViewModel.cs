﻿namespace Shopping.Models.ViewModels
{
	public class CartItemViewModel
	{
		public List<CartItemModel> CartItems { get; set; }
		public decimal totalAmount { get; set; }
	}
}
