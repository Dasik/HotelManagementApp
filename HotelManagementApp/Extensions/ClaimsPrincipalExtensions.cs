﻿using System;
using System.Security.Claims;

namespace HotelManagementApp.Extensions
{
	public static class ClaimsPrincipalExtensions
	{
		public static string GetUserId(this ClaimsPrincipal principal)
		{
			if (principal == null)
				throw new ArgumentNullException(nameof(principal));

			return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		}
	}
}