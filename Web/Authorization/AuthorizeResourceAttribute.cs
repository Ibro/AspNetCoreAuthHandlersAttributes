using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Web.Data;

namespace Web.Authorization
{
	public class AuthorizeResourceAttribute : TypeFilterAttribute
	{
		public AuthorizeResourceAttribute(Type requirementType)
			: base(typeof(AuthorizeResourceFilter))
		{
			Arguments = new object[] { requirementType };
		}

		private class AuthorizeResourceFilter : IAsyncActionFilter
		{
			private readonly IAuthorizationService _authorizationService;
			private readonly Type _requirementType;

			public AuthorizeResourceFilter(ApplicationDbContext dbContext, IAuthorizationService authorizationService, Type requirementType)
			{
				_authorizationService = authorizationService;
				_requirementType = requirementType;
			}

			public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
			{
				var resource = context.ActionArguments.First().Value;

				var requirement = Activator.CreateInstance(_requirementType) as IAuthorizationRequirement;

				var authorizationResult = await _authorizationService.AuthorizeAsync(context.HttpContext.User, resource, requirement);

				if (!authorizationResult.Succeeded)
				{
					context.Result = new ForbidResult();
					return;
				}

				await next();
			}
		}
	}
}