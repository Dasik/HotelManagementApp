using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Models.Context
{
	public class ApplicationDbContext : IdentityDbContext<UserModel>
	{
		public ApplicationDbContext(DbContextOptions options)
			: base(options)
		{
			Database.EnsureCreated();
		}

		public DbSet<ApartmentModel> Apartments { get; set; }
		public DbSet<ApartTypeModel> ApartTypes { get; set; }
		public DbSet<BookingModel> Booking { get; set; }
		public DbSet<HotelModel> Hotels { get; set; }
		public DbSet<UserModel> Users { get; set; }
	}
}