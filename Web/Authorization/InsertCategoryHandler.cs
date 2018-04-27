using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Web.Authorization.Other;
using Web.Data;

namespace Web.Authorization
{
	public class InsertCategoryHandler : AuthorizationHandler<InsertCategoryRequirement, Category>, IAuthorizationRequirement
	{
		private readonly ISecurityService _securityService;

		public InsertCategoryHandler(ISecurityService securityService) => _securityService = securityService;

		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
			InsertCategoryRequirement requirement, Category category)
		{
			if (!(context.Resource is Category))
			{
				context.Fail();
				return;
			}

			var userId = context.User.GetUserId();

			var hasAdminAccess = await _securityService.HasAdmin(userId);
			if (hasAdminAccess)
			{
				context.Succeed(requirement);
				return;
			}

			if (string.IsNullOrEmpty(category.Id) && category.UserId == userId)
			{
				context.Succeed(requirement);
				return;
			}

			context.Fail();
		}
	}

	public class InsertCategoryRequirement : IAuthorizationRequirement
	{
	}
}