using Application.Interfaces;
using MediatR;

namespace Application.UserGroups.Queries.UserGroupList
{
    public class UserGroupListQuery : IPageRequest, IRequest<UserGroupListViewModel>
    {
        public int GroupId { get; set; }
        public int Items { get; set; } = 20;
        public int Page { get; set; } = 1;
    }
}