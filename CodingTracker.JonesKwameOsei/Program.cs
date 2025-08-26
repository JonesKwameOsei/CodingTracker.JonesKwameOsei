using CodingTracker.JonesKwameOsei;
using Microsoft.Extensions.Configuration;

// Create configuration to read from appsettings.json
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

string? connectionString = configuration.GetSection("ConnectionStrings")["DefaultConnection"];

var dataAcess = new DataAccess(connectionString ?? string.Empty);

dataAcess.CreateDatabase();

Console.WriteLine("Press any key to exit...");
Console.ReadKey();