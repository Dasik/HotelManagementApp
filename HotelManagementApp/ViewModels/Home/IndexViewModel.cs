using HotelManagementApp.ViewModels.Db;
using HotelManagementApp.ViewModels.Enums;
using HotelManagementApp.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.ViewModels.Home
{
	public class IndexViewModel : ISortViewModel
	{
		public IEnumerable<ApartViewModel> Aparts { get; set; }

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

		public string SortKey { get; set; }
		public SortOrder SortState { get; set; }

		public ISortViewModel CloneViewModel(string sortKey, SortOrder sortState)
		{
			return new IndexViewModel()
			{
				SortKey = sortKey,
				SortState = sortState
			};
		}
	}
}