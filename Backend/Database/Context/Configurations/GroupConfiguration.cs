using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Context.Configurations
{
	public class GroupConfiguration : IEntityTypeConfiguration<Group>
	{
		public void Configure(EntityTypeBuilder<Group> builder)
		{
			builder.ToTable("Groups");

			builder.HasIndex(g => g.Name)
				.IsUnique();

			builder.HasKey(g => g.Id);

			builder.HasOne(g => g.Owner)
				.WithMany(o => o.OwnedGroups)
				.HasForeignKey(g => g.OwnerId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			builder.Property(g => g.Id)
				.HasColumnName("ID")
				.IsRequired();

			builder.Property(g => g.OwnerId)
				.HasColumnName("OwnerID")
				.IsRequired();

			builder.Property(g => g.Name)
				.HasMaxLength(100)
				.IsRequired();
		}
	}
}