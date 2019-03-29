using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.ViewModels.Enums
{
	public enum SortOrder
	{
		[Display(Name = " ")]
		Unspecified = -1,
		[Display(Name = "По возрастанию")]
		Ascending = 0,
		[Display(Name = "По убыванию")]
		Descending = 1,
	}
}