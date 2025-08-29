using CodingTracker.JonesKwameOsei;

var dataAcess = new DataAccess();

dataAcess.CreateDatabase();

dataAcess.ResetSessions();
SeedData.SeedRecords(20, new[] { "C#", "Java", "Rust" });

UserInterface.MainMenu();

Console.ReadKey();