using Application.Interfaces;
using MediatR;

namespace Application.Groups.Queries.GroupList
{
    public class GroupListQuery : IPageRequest, IRequest<GroupListViewModel>
    {
        public string Name { get; set; }
        public int? UserId { get; set; }
        public int Items { get; set; } = 20;
        public int Page { get; set; } = 1;
    }
}