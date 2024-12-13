using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Context.Configurations
{
	public class ItemConfiguration : IEntityTypeConfiguration<Item>
	{
		public void Configure(EntityTypeBuilder<Item> builder)
		{
			builder.ToTable("Items");

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