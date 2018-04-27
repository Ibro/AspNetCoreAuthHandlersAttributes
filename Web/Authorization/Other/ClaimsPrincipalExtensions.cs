using System;
using System.Linq;
using System.Security.Claims;

namespace Web.Authorization.Other
{
	public static class ClaimsPrincipalExtensions
	{
		public static string GetUserId(this ClaimsPrincipal user)
		{
			string value = user.GetClaimValue(IdentityModel.JwtClaimTypes.Subject);
			if (string.IsNullOrEmpty(value))
			{
				throw new Exception($"User has no {IdentityModel.JwtClaimTypes.Subject} claim!");
			}

			return value;
		}

		public static string GetClaimValue(this ClaimsPrincipal user, string claimType)
			=> user.Claims?.FirstOrDefault(c => c.Type == claimType)?.Value;
	}
}