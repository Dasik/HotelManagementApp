using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementApp.Models
{
	[Table("Booking")]
	public class BookingModel
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();

		public string UserId { get; set; }
		public UserModel User { get; set; }

		public string ApartmentId { get; set; }
		public ApartmentModel Apartment { get; set; }

		public DateTime StartDate { get; set; }
		public DateTime ExpectedEndTime { get; set; }
		public DateTime? EndTime { get; set; }
	}
}