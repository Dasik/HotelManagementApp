using System;
using System.Collections.Generic;
using HotelManagementApp.Models;
using HotelManagementApp.Utils;

namespace HotelManagementApp.ViewModels.Db
{
	public class HotelViewModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public string ContactPhone { get; set; }
		public string ContactEmail { get; set; }

		public List<ApartViewModel> Apartments { get; set; } = new List<ApartViewModel>();

		public HotelViewModel() { }

		public HotelViewModel(HotelModel hotel)
		{
			Id = hotel.Id;
			Name = hotel.Name;
			Description = hotel.Description;
			if (hotel.Image == null)
				Image = Constants.ImagePlaceholder;
			else
				Image = "data:image;base64," + Convert.ToBase64String(hotel.Image);
			ContactPhone = hotel.ContactPhone;
			ContactEmail = hotel.ContactEmail;
		}
	}
}