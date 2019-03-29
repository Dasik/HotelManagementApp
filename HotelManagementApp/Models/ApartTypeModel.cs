using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementApp.Models
{
	[Table("ApartType")]
	public class ApartTypeModel
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }

		public List<ApartmentModel> Apartments { get; set; } = new List<ApartmentModel>();

		[Timestamp]
		public byte[] Timestamp { get; set; }
	}
}