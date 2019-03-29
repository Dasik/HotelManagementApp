using HotelManagementApp.ViewModels.Enums;
using HotelManagementApp.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.ViewModels.Booking
{
	public class BookingListViewModelModel : ISortViewModel
	{
		public IEnumerable<BookingViewModel> Booking { get; set; }

		[Display(Name = "Мест")]
		public int? Capacity { get; set; }
		public CompareOrder CapacityOrder { get; set; }
		[Display(Name = "Тип")]
		public SelectList Types { get; set; }
		public string TypeId { get; set; }
		[Display(Name = "Стоимость")]
		[DataType(DataType.Currency)]
		public decimal? Price { get; set; }
		public CompareOrder PriceOrder { get; set; }
		[Display(Name = "Начало")]
		[DataType(DataType.Date)]
		public DateTime? StartDate { get; set; }
		[Display(Name = "Конец")]
		[DataType(DataType.Date)]
		public DateTime? EndDate { get; set; }


		public string SortKey { get; set; }
		public SortOrder SortState { get; set; }
		public ISortViewModel CloneViewModel(string sortKey, SortOrder sortState)
		{
			return new BookingListViewModelModel()
			{
				SortKey = sortKey,
				SortState = sortState
			};
		}
	}
}