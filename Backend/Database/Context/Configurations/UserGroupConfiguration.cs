using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Context.Configurations
{
	public class UserGroupConfiguration : IEntityTypeConfiguration<UserGroup>
	{
		public void Configure(EntityTypeBuilder<UserGroup> builder)
		{
			builder.ToTable("UserGroups");

			builder.HasKey(ug => new {ug.UserId, ug.GroupId});

			builder.HasOne(ug => ug.User)
				.WithMany(u => u.UserGroups)
				.HasForeignKey(ug => ug.UserId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(ug => ug.Group)
				.WithMany(g => g.GroupUsers)
				.HasForeignKey(ug => ug.GroupId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Cascade);

			builder.Property(ug => ug.UserId)
				.HasColumnName("UserID")
				.IsRequired();

			builder.Property(ug => ug.GroupId)
				.HasColumnName("GroupID")
				.IsRequired();

			builder.Property(ug => ug.IsManager)
				.HasDefaultValueSql("FALSE")
				.IsRequired();

			builder.Property(ug => ug.IsAcceptedByManager)
				.HasColumnName("ManagerAccepted")
				.HasDefaultValueSql("FALSE")
				.IsRequired();

			builder.Property(ug => ug.IsAcceptedByUser)
				.HasColumnName("UserAccepted")
				.HasDefaultValueSql("FALSE")
				.IsRequired();

			builder.Property(ug => ug.RequestMessage)
				.HasMaxLength(255);
		}
	}
}