using Microsoft.Extensions.Configuration;

// Create an instance of IConfiguration Interface
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

string? connectionString = configuration.GetSection("ConnectionStrings")["DefaultConnection"];

Console.WriteLine(connectionString);
