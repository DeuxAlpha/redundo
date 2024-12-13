using Database.Context.Configurations;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Database.Context
{
	public class PurchaseManagerContext : DbContext
	{
		public PurchaseManagerContext(DbContextOptions options) : base(options){}
		
		public DbSet<User> Users { get; private set; }
		public DbSet<Group> Groups { get; private set; }
		public DbSet<UserGroup> UserGroups { get; private set; }
		public DbSet<Item> Items { get; private set; }
		public DbSet<ItemStatus> ItemStatuses { get; private set; }
		public DbSet<GroupItem> GroupItems { get; private set; }
		public DbSet<RefreshToken> RefreshTokens { get; private set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ForNpgsqlUseIdentityByDefaultColumns();

			modelBuilder.ApplyConfiguration(new GroupConfiguration());
			modelBuilder.ApplyConfiguration(new GroupItemConfiguration());
			modelBuilder.ApplyConfiguration(new GroupItemConfiguration());
			modelBuilder.ApplyConfiguration(new ItemConfiguration());
			modelBuilder.ApplyConfiguration(new ItemStatusConfiguration());
			modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
			modelBuilder.ApplyConfiguration(new UserConfiguration());
			modelBuilder.ApplyConfiguration(new UserGroupConfiguration());
		}
	}
}