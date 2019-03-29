using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.ViewModels.Enums
{
	public enum CompareOrder
	{
		[Display(Name = " ")]
		Unspecified,
		[Display(Name = "Больше")]
		More,
		[Display(Name = "Меньше")]
		Less
	}
}