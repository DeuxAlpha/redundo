using Application.Interfaces;
using MediatR;

namespace Application.GroupItems.Queries.GroupItemList
{
    public class GroupItemListQuery : IPageRequest, IRequest<GroupItemListViewModel>
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public int Items { get; set; } = 20;
        public int Page { get; set; } = 1;
    }
}