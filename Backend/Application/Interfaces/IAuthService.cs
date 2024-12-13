using Domain.Models;

namespace Application.Interfaces
{
	public interface IAuthService
	{
		int? GetUserId();

		bool IsUserIdSelf(int? userId);

		bool UserIsManagerOfGroup(Group group);

		bool UserIsManagerOfGroup(User user, Group group);

		bool UserIsOwnerOfGroup(Group group);

		bool UserIsOwnerOfGroup(User user, Group group);
		bool UserIsPartOfGroup(Group group);
	}
}