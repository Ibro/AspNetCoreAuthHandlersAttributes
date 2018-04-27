using System.Threading.Tasks;

namespace Web.Authorization.Other
{
	public interface ISecurityService
	{
		Task<bool> HasAdmin(string userId);
	}
}