using HotelManagementApp.Extensions;
using HotelManagementApp.Models;
using HotelManagementApp.Services;
using HotelManagementApp.Utils;
using HotelManagementApp.ViewModels;
using HotelManagementApp.ViewModels.Booking;
using HotelManagementApp.ViewModels.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HotelManagementApp.Models.Context;
using HotelManagementApp.ViewModels.Db;

namespace HotelManagementApp.Controllers
{
	public class BookingController : Controller
	{
		private readonly ApplicationDbContext _db;
		private readonly ILogger _logger;
		private readonly BookingService _bookingService;
		private readonly ApartmentService _apartmentService;
		private readonly ApartTypeService _apartTypeService;

		public BookingController(ApplicationDbContext db, ILogger<BookingController> logger, BookingService bookingService, ApartmentService apartmentService, ApartTypeService apartTypeService)
		{
			_db = db;
			_logger = logger;
			_bookingService = bookingService;
			_apartmentService = apartmentService;
			_apartTypeService = apartTypeService;
		}

		public async Task<IActionResult> Index(string Id)
		{
			if (String.IsNullOrWhiteSpace(Id))
				return RedirectToAction("Index", "Home");

			var apart = await _apartmentService.GetApartmentIfAvailable(Id).AsNoTracking().FirstOrDefaultAsync();
			return View(new BookingViewModel() { Apart = new ApartViewModel(apart) });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = Constants.Roles.User + "," + Constants.Roles.Admin)]
		public async Task<IActionResult> Create(BookingViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var userId = User.IsInRole(Constants.Roles.Admin) ? viewModel.UserId : User.GetUserId();
				try
				{
					var booking = await _bookingService.BookApartment(viewModel.Apart.Id, userId,
						viewModel.StartDate, viewModel.ExpectedEndDate);
					return View("Success", new BookingViewModel(booking));
				}
				catch (DBConcurrencyException)
				{
					return View("Fail");
				}
			}
			return View("Index", viewModel);
		}

		public async Task<IActionResult> List(int? capacity = null, CompareOrder capacityOrder = CompareOrder.Unspecified,
												string typeId = "",
												decimal? price = null, CompareOrder priceOrder = CompareOrder.Unspecified,
												string sortKey = null, SortOrder sortState = SortOrder.Unspecified,
												DateTime? startDate = null, DateTime? endDate = null
			)
		{

			var apartTypes = await _apartTypeService.GetAllTypes();
			apartTypes.Insert(0, new ApartTypeModel() { Name = "Все", Id = null });

			var viewModel = new BookingListViewModelModel()
			{
				SortState = sortState,
				SortKey = sortKey,
				Capacity = capacity,
				Types = String.IsNullOrWhiteSpace(typeId) ?
					new SelectList(apartTypes, "Id", "Name") :
					new SelectList(apartTypes, "Id", "Name", typeId),
				CapacityOrder = capacityOrder,
				PriceOrder = priceOrder,
				Price = price,
				StartDate = startDate,
				EndDate = endDate,
			};

			var booking = _bookingService.FindBookingByUser(User.GetUserId(), startDate, endDate);

			#region filter

			if (!CompareOrder.Unspecified.Equals(viewModel.PriceOrder) && viewModel.Price != 0)
			{
				var model = viewModel;
				booking = CompareOrder.More.Equals(viewModel.PriceOrder) ?
					booking.Where(apart => apart.Apartment.Type.Price >= model.Price) :
					booking.Where(apart => apart.Apartment.Type.Price <= model.Price);
			}
			if (!CompareOrder.Unspecified.Equals(viewModel.CapacityOrder) && viewModel.Capacity != 0)
			{
				var model = viewModel;
				booking = CompareOrder.More.Equals(viewModel.CapacityOrder) ?
					booking.Where(apart => apart.Apartment.Capacity >= model.Capacity) :
					booking.Where(apart => apart.Apartment.Capacity <= model.Capacity);
			}

			if (!String.IsNullOrWhiteSpace(typeId))
				booking = booking.Where(apart => apart.Apartment.TypeId.Equals(typeId));

			#endregion

			#region sort

			if (!SortOrder.Unspecified.Equals(viewModel.SortState))
			{
				Expression<Func<BookingModel, object>> orderKey;
				switch (viewModel.SortKey)
				{
					case "Hotel":
						orderKey = (model => model.Apartment.Hotel);
						break;
					case "Description":
						orderKey = (model => model.Apartment.Description);
						break;
					case "Capacity":
						orderKey = (model => model.Apartment.Capacity);
						break;
					case "Types":
						orderKey = (model => model.Apartment.Type);
						break;
					case "Price":
						orderKey = (apart => apart.Apartment.Type.Price);
						break;
					case "StartDate":
						orderKey = (model => model.StartDate);
						break;
					case "EndDate":
						orderKey = (model => model.EndTime ?? model.ExpectedEndTime);
						break;
					default:
						orderKey = (apart => apart.Apartment.Hotel);
						break;
				}

				booking = SortOrder.Ascending.Equals(viewModel.SortState) ?
					booking.OrderBy(orderKey) :
					booking.OrderByDescending(orderKey);
			}

			#endregion

			var result = await booking.AsNoTracking().ToListAsync();
			_logger.LogInformation($"Showing {result.Count} booking");
			viewModel.Booking = result.Select(model => new BookingViewModel(model));
			return View(viewModel);
		}
	}
}