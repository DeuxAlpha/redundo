using System.Collections.Generic;
using System.Linq;
using Domain.Models;

namespace Application.Users.Queries.UserList
{
    public class UserListItemModel
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public UserListItemModel(User user)
        {
            Id = user.Id;
            Username = user.Username;
        }
    }
}