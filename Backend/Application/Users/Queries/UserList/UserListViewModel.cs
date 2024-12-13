using System.Collections.Generic;
using System.Linq;
using Domain.Models;

namespace Application.Users.Queries.UserList
{
    public class UserListViewModel
    {
        public IEnumerable<UserListItemModel> Users { get; set; }

        public UserListViewModel(IEnumerable<User> users)
        {
            Users = users.Select(u => new UserListItemModel(u)).OrderBy(u => u.Username);
        }
    }
}