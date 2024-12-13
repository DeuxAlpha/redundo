using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Context.Configurations
{
	public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
	{
		public void Configure(EntityTypeBuilder<RefreshToken> builder)
		{
			builder.ToTable("RefreshTokens");

			builder.HasKey(rt => new {rt.UserId, rt.Token});

			builder.HasOne(rt => rt.User)
				.WithMany(u => u.RefreshTokens)
				.HasForeignKey(rt => rt.UserId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Cascade);

			builder.Property(rt => rt.UserId)
				.HasColumnName("UserID")
				.IsRequired();

			builder.Property(rt => rt.Token)
				.HasMaxLength(100)
				.IsRequired();

			builder.Property(rt => rt.ExpirationDate)
				.IsRequired();

			builder.Property(rt => rt.CreationDate)
				.IsRequired();

			builder.Property(rt => rt.IsInvalidated)
				.HasDefaultValueSql("FALSE")
				.IsRequired();
		}
	}
}