using Application.GroupItems.Models;
using Domain.Enums;
using MediatR;

namespace Application.GroupItems.Commands.UpdateGroupItem
{
    public class UpdateGroupItemCommand : IRequest<GroupItemDto>
    {
        public int GroupId { get; set; }
        public int ItemId { get; set; }
        public ItemStatusEnum ItemStatusId { get; set; }
        public string Notes { get; set; }
        public bool OneTimePurchase { get; set; }
        public bool DoNotBuy { get; set; }
    }
}