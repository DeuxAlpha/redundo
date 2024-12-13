using System;
using Application.GroupItems.Models;
using Domain.Enums;
using MediatR;

namespace Application.GroupItems.Commands.CreateGroupItem
{
    public class CreateGroupItemCommand : IRequest<GroupItemDto>
    {
        public int GroupId { get; set; }
        public int ItemId { get; set; }
        public ItemStatusEnum ItemStatusId { get; set; }
        public string Notes { get; set; }
        public bool OneTimePurchase { get; set; }
    }
}