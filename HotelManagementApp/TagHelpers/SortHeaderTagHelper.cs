using HotelManagementApp.ViewModels.Enums;
using HotelManagementApp.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HotelManagementApp.TagHelpers
{
	public class SortHeaderTagHelper : TagHelper
	{
		/// <summary>
		/// значение текущего свойства, для которого создается тег
		/// </summary>
		public SortOrder SortOrder { get; set; }
		/// <summary>
		/// имя текущего свойства
		/// </summary>
		public string CurrentKey { get; set; }
		/// <summary>
		///  имя активного свойства, выбранного для сортировки
		/// </summary>
		public string ActiveKey { get; set; }
		/// <summary>
		/// действие контроллера, на которое создается ссылка
		/// </summary>
		public string Action { get; set; }
		public ISortViewModel Model { get; set; }//todo: need try to use <form></form>

		private IUrlHelperFactory urlHelperFactory;
		public SortHeaderTagHelper(IUrlHelperFactory helperFactory)
		{
			urlHelperFactory = helperFactory;
		}
		[ViewContext]
		[HtmlAttributeNotBound]
		public ViewContext ViewContext { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
			output.TagName = "a";
			string url = urlHelper.Action(Action,
				Model.CloneViewModel(CurrentKey,
									CurrentKey == ActiveKey ?
									ShiftSortOrder(SortOrder) :
									SortOrder.Ascending));
			//string url = urlHelper.Action(
			//	Action,
			//					//Model.AsAnon(
			//					//	new ISortViewModel(
			//					//	CurrentKey,
			//					//	CurrentKey == ActiveKey ?
			//					//		ShiftSortOrder(SortOrder) :
			//					//		SortOrder.Ascending
			//					//)));
			//	null
			//	);
			output.Attributes.SetAttribute("href", url);

			if (CurrentKey == ActiveKey)
			{
				TagBuilder tag = new TagBuilder("i");
				tag.AddCssClass("glyphicon");

				if (SortOrder.Ascending.Equals(SortOrder))
				{
					tag.AddCssClass("glyphicon-chevron-up");
				}
				else if (SortOrder.Descending.Equals(SortOrder))
				{
					tag.AddCssClass("glyphicon-chevron-down");
				}

				output.PreContent.AppendHtml(tag);
			}
		}

		protected SortOrder ShiftSortOrder(SortOrder sortState)
		{
			switch (sortState)
			{
				case SortOrder.Ascending:
					return SortOrder.Descending;
				case SortOrder.Descending:
					return SortOrder.Unspecified;
				case SortOrder.Unspecified:
					return SortOrder.Ascending;
				default:
					return SortOrder.Unspecified;
			}
		}
	}
}