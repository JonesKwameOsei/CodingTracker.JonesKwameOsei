using Microsoft.Extensions.Configuration;

// Build configuration reading appsettings.json located next to the project (copied to output folder)
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

string? connectionString = configuration.GetSection("ConnectionStrings")["DefaultConnection"];
Console.WriteLine(connectionString);

Console.WriteLine("Press any key to exit...");
Console.ReadKey();