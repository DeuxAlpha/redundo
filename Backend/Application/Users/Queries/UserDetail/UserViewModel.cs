using Domain.Models;

namespace Application.Users.Queries.UserDetail
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public UserViewModel(User user)
        {
            Id = user.Id;
            Username = user.Username;
        }
    }
}