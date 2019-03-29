using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementApp.Models
{
	[Table("Apartment")]
	public class ApartmentModel
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public string RoomNumber { get; set; }
		public string Description { get; set; }
		public byte[] Image { get; set; }
		public int Capacity { get; set; }

		public string HotelId { get; set; }
		public HotelModel Hotel { get; set; }

		public string TypeId { get; set; }
		public ApartTypeModel Type { get; set; }

		public List<BookingModel> Booking { get; set; } = new List<BookingModel>();

		[Timestamp]
		public byte[] Timestamp { get; set; }
	}
}