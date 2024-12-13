using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Context.Configurations
{
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.ToTable("Users");

			builder.HasIndex(u => u.Username)
				.IsUnique();

			builder.HasKey(u => u.Id);

			builder.Property(u => u.Id)
				.HasColumnName("ID")
				.IsRequired();

			builder.Property(u => u.Username)
				.HasMaxLength(100)
				.IsRequired();

			builder.Property(u => u.Password)
				.HasMaxLength(255)
				.IsRequired();
		}
	}
}