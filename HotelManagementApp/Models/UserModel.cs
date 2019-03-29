using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace HotelManagementApp.Models
{
	//[Table("User")]
	public class UserModel : IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PatronymicName { get; set; }

		public List<BookingModel> Booking { get; set; } = new List<BookingModel>();
	}
}
