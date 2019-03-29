using HotelManagementApp.Models;
using HotelManagementApp.Models.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementApp.Services
{
	public class BookingService
	{
		private readonly ApplicationDbContext _db;
		private readonly ILogger _logger;

		public BookingService(ApplicationDbContext db, ILogger<BookingService> logger)
		{
			_db = db;
			_logger = logger;
		}

		public async Task<BookingModel> BookApartment(string apartId, string userId, DateTime startDate, DateTime expectedEndDate)
		{
			if (startDate < DateTime.Now)
				throw new ArgumentException($"Argument {nameof(startDate)} is less than current date");
			if ((expectedEndDate - startDate).Days < 1)
				throw new InvalidOperationException("Can't book an apartment for less than 1 day");
			var bookingExist = await FindBookingByApartment(apartId, startDate, expectedEndDate).AnyAsync();
			if (bookingExist)
				throw new DBConcurrencyException();
			var booking = new BookingModel()
			{
				ApartmentId = apartId,
				UserId = userId,
				StartDate = startDate,
				ExpectedEndTime = expectedEndDate
			};
			await _db.Booking.AddAsync(booking);
			await _db.SaveChangesAsync();
			return await _db.Booking
				.Include(model => model.User)
				.Include(model => model.Apartment)
				.ThenInclude(model => model.Hotel)
				.Include(model => model.Apartment)
				.ThenInclude(model => model.Type)
				.SingleOrDefaultAsync(model => model.Id == booking.Id);
		}

		public IQueryable<BookingModel> FindBookingByUser(string userId, DateTime? startDate = null, DateTime? endDate = null)
		{
			if (startDate == null)
				startDate = DateTime.MinValue;
			if (endDate == null)
				endDate = DateTime.MaxValue;
			if ((endDate.Value - startDate.Value).Days < 0)
				throw new InvalidOperationException($"Argument {nameof(endDate)} is less than {nameof(startDate)}");

			return _db.Booking
				.Include(model => model.User)
				.Include(model => model.Apartment)
					.ThenInclude(model => model.Hotel)
				.Include(model => model.Apartment)
					.ThenInclude(model => model.Type)
				.Where(booking => booking.User.Id == userId &&
								  (booking.StartDate < endDate.Value && startDate.Value < (booking.EndTime ?? booking.ExpectedEndTime)));

		}

		public IQueryable<BookingModel> FindBookingByApartment(string apartId, DateTime? startDate = null, DateTime? endDate = null)
		{
			if (startDate == null)
				startDate = DateTime.MinValue;
			if (endDate == null)
				endDate = DateTime.MaxValue;
			if ((endDate.Value - startDate.Value).Days < 0)
				throw new InvalidOperationException($"Argument {nameof(endDate)} is less than {nameof(startDate)}");

			return _db.Booking
				.Include(model => model.Apartment)
					.ThenInclude(model => model.Hotel)
				.Include(model => model.Apartment)
					.ThenInclude(model => model.Type)
				.Where(booking => booking.ApartmentId == apartId &&
								  (booking.StartDate < endDate.Value && startDate.Value < (booking.EndTime ?? booking.ExpectedEndTime)));

		}
	}
}