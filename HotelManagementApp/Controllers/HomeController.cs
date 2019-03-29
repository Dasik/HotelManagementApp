using HotelManagementApp.Models;
using HotelManagementApp.Services;
using HotelManagementApp.ViewModels;
using HotelManagementApp.ViewModels.Enums;
using HotelManagementApp.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HotelManagementApp.ViewModels.Db;

namespace HotelManagementApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger _logger;
		private readonly ApartmentService _apartmentService;
		private readonly ApartTypeService _apartTypeService;

		public HomeController(ILogger<HomeController> logger, ApartmentService apartmentService, ApartTypeService apartTypeService)
		{
			_logger = logger;
			_apartmentService = apartmentService;
			_apartTypeService = apartTypeService;
		}

		//public async Task<IActionResult> Index(IndexViewModel viewModel)
		public async Task<IActionResult> Index(int? capacity = null, CompareOrder capacityOrder = CompareOrder.Unspecified,
												string typeId = "",
												decimal? price = null, CompareOrder priceOrder = CompareOrder.Unspecified,
												string sortKey = null, SortOrder sortState = SortOrder.Unspecified)
		{
			var apartTypes = await _apartTypeService.GetAllTypes();
			apartTypes.Insert(0, new ApartTypeModel() { Name = "Все", Id = null });
			var viewModel = new IndexViewModel()
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
			};

			var apartments = _apartmentService.GetAvailableApartments();

			#region filter

			if (!CompareOrder.Unspecified.Equals(viewModel.PriceOrder) && viewModel.Price != 0)
			{
				var model = viewModel;
				apartments = CompareOrder.More.Equals(viewModel.PriceOrder) ?
					apartments.Where(apart => apart.Type.Price >= model.Price) :
					apartments.Where(apart => apart.Type.Price <= model.Price);
			}
			if (!CompareOrder.Unspecified.Equals(viewModel.CapacityOrder) && viewModel.Capacity != 0)
			{
				var model = viewModel;
				apartments = CompareOrder.More.Equals(viewModel.CapacityOrder) ?
					apartments.Where(apart => apart.Capacity >= model.Capacity) :
					apartments.Where(apart => apart.Capacity <= model.Capacity);
			}

			if (!String.IsNullOrWhiteSpace(typeId))
				apartments = apartments.Where(apart => apart.TypeId.Equals(typeId));

			#endregion

			#region sort

			if (!SortOrder.Unspecified.Equals(viewModel.SortState))
			{
				Expression<Func<ApartmentModel, object>> orderKey;
				switch (viewModel.SortKey)
				{
					case "Hotel":
						orderKey = (apart => apart.Hotel);
						break;
					case "Description":
						orderKey = (apart => apart.Description);
						break;
					case "Capacity":
						orderKey = (apart => apart.Capacity);
						break;
					case "Types":
						orderKey = (apart => apart.Type);
						break;
					case "Price":
						orderKey = (apart => apart.Type.Price);
						break;
					default:
						orderKey = null;
						break;
				}
				if (orderKey != null)
					apartments = SortOrder.Ascending.Equals(viewModel.SortState) ?
						apartments.OrderBy(orderKey) :
						apartments.OrderByDescending(orderKey);
			}

			#endregion

			var aparts = await apartments.AsNoTracking().ToListAsync();
			_logger.LogInformation($"Showing {aparts.Count} apartaments");
			viewModel.Aparts = aparts.Select(apart => new ApartViewModel(apart));
			return View(viewModel);
		}

		public IActionResult About()
		{
			ViewData["Message"] = "Описание приложения";

			return View();
		}

		public IActionResult Contact()
		{
			ViewData["Message"] = "Контакты автора";

			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
