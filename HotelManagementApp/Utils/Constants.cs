namespace HotelManagementApp.Utils
{
	public static class Constants
	{
		public static readonly string ImagePlaceholder;

		static Constants()
		{
			ImagePlaceholder = "\\images\\placeholder.png";
		}

		public static class Roles
		{
			public const string Admin = "admin";
			public const string User = "user";
			public const string Manager = "manager";
		}
	}
}