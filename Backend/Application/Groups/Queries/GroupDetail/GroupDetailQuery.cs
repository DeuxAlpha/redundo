using MediatR;

namespace Application.Groups.Queries.GroupDetail
{
    public class GroupDetailQuery : IRequest<GroupViewModel>
    {
        public int Id { get; set; }
    }
}