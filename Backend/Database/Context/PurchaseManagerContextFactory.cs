using System;
using Common;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Database.Context
{
	public class PurchaseManagerContextFactory : IDesignTimeDbContextFactory<PurchaseManagerContext>
	{
		public PurchaseManagerContext CreateDbContext(string[] args)
		{
			var configuration = Configuration.GetInstance();
			var optionsBuilder = new DbContextOptionsBuilder<PurchaseManagerContext>();
			var connectionString = configuration.GetValue<string>("ConnectionString");
			if (connectionString == null)
				throw new InvalidOperationException("Connection string is null.");

			optionsBuilder.UseNpgsql(connectionString);

			return new PurchaseManagerContext(optionsBuilder.Options);
		}
	}
}