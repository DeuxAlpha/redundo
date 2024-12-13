using System.IO;
using Microsoft.Extensions.Configuration;

namespace Common
{
	public class Configuration
	{
		private static IConfiguration _configuration;

		public static IConfiguration GetInstance()
		{
			var sharedFolder = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).ToString(), "Shared");
			return _configuration ?? (_configuration = new ConfigurationBuilder()
				       .AddUserSecrets<Configuration>()
				       .AddJsonFile(Path.Combine(sharedFolder, "sharedSettings.json"), true, true)
				       .AddJsonFile("appsettings.json", true, true)
				       .AddJsonFile("appsettings.<Environment>.json", true, true)
				       .AddEnvironmentVariables()
				       .Build());
		}
	}
}