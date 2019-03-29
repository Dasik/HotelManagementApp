using HotelManagementApp.Controllers;
using HotelManagementApp.Models;
using HotelManagementApp.Models.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace HotelManagementApp.Services
{
	public class ApartmentService
	{
		private readonly ApplicationDbContext _db;
		private readonly ILogger _logger;

		public ApartmentService(ApplicationDbContext db, ILogger<HomeController> logger)
		{
			this._db = db;
			this._logger = logger;
		}

		public IQueryable<ApartmentModel> GetAvailableApartments()
		{
			return _db.Apartments
				.Include(model => model.Hotel)
				.Include(model => model.Type)
				.SelectMany(apart => apart.Booking.DefaultIfEmpty(),
					(apart, booking) =>
						new
						{
							Apart = apart,
							EndDate = booking == null ? DateTime.MinValue :
								booking.EndTime == null ? booking.ExpectedEndTime :
								booking.EndTime
						})
				.Where(apart => apart.EndDate < DateTime.Now)
				.Select(apart => apart.Apart);
		}

		public IQueryable<ApartmentModel> GetApartmentIfAvailable(string apartId)
		{
			return _db.Apartments
				.Include(model => model.Hotel)
				.Include(model => model.Type)
				.Where(apart => apart.Id == apartId)
				.SelectMany(apart => apart.Booking.DefaultIfEmpty(),
					(apart, booking) =>
						new
						{
							Apart = apart,
							EndDate = booking == null ? DateTime.MinValue :
								booking.EndTime == null ? booking.ExpectedEndTime :
								booking.EndTime
						})
				.Where(apart => apart.EndDate < DateTime.Now)
				.Select(apart => apart.Apart);
		}
	}
}