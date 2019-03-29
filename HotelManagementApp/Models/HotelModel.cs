using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementApp.Models
{
	[Table("Hotel")]
	public class HotelModel
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public string Name { get; set; }
		public string Description { get; set; }
		public byte[] Image { get; set; }
		public string ContactPhone { get; set; }
		public string ContactEmail { get; set; }

		public List<ApartmentModel> Apartments { get; set; } = new List<ApartmentModel>();

		[Timestamp]
		public byte[] Timestamp { get; set; }
	}
}