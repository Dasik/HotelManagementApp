using System;
using HotelManagementApp.Models;
using HotelManagementApp.Utils;

namespace HotelManagementApp.ViewModels.Db
{
	public class ApartViewModel
	{
		public string Id { get; set; }
		public string RoomNumber { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public int Capacity { get; set; }
		public HotelViewModel Hotel { get; set; }
		public string Type { get; set; }
		public decimal Price { get; set; }

		public ApartViewModel() { }

		public ApartViewModel(ApartmentModel apartment)
		{
			Id = apartment.Id;
			RoomNumber = apartment.RoomNumber;
			Description = apartment.Description;
			if (apartment.Image == null)
				Image = Constants.ImagePlaceholder;
			else
				Image = "data:image;base64," + Convert.ToBase64String(apartment.Image);
			Capacity = apartment.Capacity;
			Hotel = new HotelViewModel(apartment.Hotel);
			Type = apartment.Type.Name;
			Price = apartment.Type.Price;
		}
	}
}