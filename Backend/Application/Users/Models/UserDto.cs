using Domain.Models;

namespace Application.Users.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public UserDto(User user)
        {
            Id = user.Id;
            Username = user.Username;
        }
    }
}