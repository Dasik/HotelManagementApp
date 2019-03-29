using HotelManagementApp.ViewModels.Enums;

namespace HotelManagementApp.ViewModels.Shared
{
	public interface ISortViewModel
	{
		string SortKey { get; set; }
		SortOrder SortState { get; set; }

		ISortViewModel CloneViewModel(string sortKey, SortOrder sortState);
	}
}