using MediatR;

namespace Application.GroupItems.Commands.DeleteGroupItem
{
    public class DeleteGroupItemCommand : IRequest
    {
        public int GroupId { get; set; }
        public int ItemId { get; set; }
    }
}