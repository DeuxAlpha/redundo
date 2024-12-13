using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Context.Configurations
{
	public class GroupItemConfiguration : IEntityTypeConfiguration<GroupItem>
	{
		public void Configure(EntityTypeBuilder<GroupItem> builder)
		{
			builder.ToTable("GroupItems");

			builder.HasKey(gi => new {gi.GroupId, gi.ItemId});

			builder.HasOne(gi => gi.Group)
				.WithMany(g => g.GroupItems)
				.HasForeignKey(gi => gi.GroupId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(gi => gi.Item)
				.WithMany(i => i.GroupItems)
				.HasForeignKey(gi => gi.ItemId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(gi => gi.ItemStatus)
				.WithMany(i => i.GroupItems)
				.HasForeignKey(gi => gi.ItemStatusId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			builder.Property(gi => gi.GroupId)
				.HasColumnName("GroupID")
				.IsRequired();

			builder.Property(gi => gi.ItemId)
				.HasColumnName("ItemID")
				.IsRequired();

			builder.Property(gi => gi.ItemStatusId)
				.HasColumnName("ItemStatusID")
				.IsRequired();

			builder.Property(gi => gi.LastUpdate)
				.IsRequired();

			builder.Property(gi => gi.Notes)
				.HasMaxLength(255);

			builder.Property(gi => gi.DoNotBuy)
				.HasDefaultValueSql("FALSE")
				.IsRequired();

			builder.Property(gi => gi.OneTimePurchase)
				.HasDefaultValueSql("FALSE")
				.IsRequired();
		}
	}
}