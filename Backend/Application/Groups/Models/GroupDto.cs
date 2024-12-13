using Domain.Models;

namespace Application.Groups.Models
{
    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public GroupDto(Group group)
        {
            Id = group.Id;
            Name = group.Name;
        }
    }
}