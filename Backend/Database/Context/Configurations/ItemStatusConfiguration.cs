using System.Collections.Generic;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Context.Configurations
{
	public class ItemStatusConfiguration : IEntityTypeConfiguration<ItemStatus>
	{
		public void Configure(EntityTypeBuilder<ItemStatus> builder)
		{
			builder.HasData(new List<ItemStatus>
			{
				new ItemStatus
				{
					Id = ItemStatusEnum.Out,
					Name = "Out"
				},
				new ItemStatus
				{
					Id = ItemStatusEnum.Stocked,
					Name = "Stocked"
				},
				new ItemStatus
				{
					Id = ItemStatusEnum.AlmostOut,
					Name = "Almost out"
				},
			});
			
			builder.ToTable("ItemStatuses");

			builder.HasKey(i => i.Id);

			builder.Property(i => i.Id)
				.HasColumnName("ID")
				.IsRequired();

			builder.Property(i => i.Name)
				.HasMaxLength(100)
				.IsRequired();
		}
	}
}