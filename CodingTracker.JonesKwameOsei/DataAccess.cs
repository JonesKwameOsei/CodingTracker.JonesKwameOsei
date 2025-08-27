using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace CodingTracker.JonesKwameOsei;

internal class DataAccess
{
    IConfiguration Configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

    private string? ConnectionString;

    public DataAccess()
    {
        ConnectionString = Configuration.GetSection("ConnectionStrings")["DefaultConnection"];
    }

    internal void CreateDatabase()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            string createTableQuery = @"
             CREATE TABLE IF NOT EXISTS codingSessions (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Language TEXT NOT NULL,
                    DateStart TEXT NOT NULL,
                    DateEnd TEXT NOT NULL
             )";

            connection.Execute(createTableQuery);
        }
    }

    internal void InsertRecord(CodingRecord codingRecord)
    {
        if (codingRecord is null) throw new ArgumentNullException(nameof(codingRecord));

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        const string insertQuery = @"
            INSERT INTO codingSessions (Language, DateStart, DateEnd)
            VALUES (@Language, @DateStart, @DateEnd);";

        connection.Execute(insertQuery, new
        {
            codingRecord.Language,
            codingRecord.DateStart,
            codingRecord.DateEnd
        });
    }

    internal IEnumerable<CodingRecord> GetAllSessions()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            string selectQuery = "SELECT * FROM codingSessions";

            var codingRecords = connection.Query<CodingRecord>(selectQuery);

            foreach (var codingRecord in codingRecords)
            {
                codingRecord.Duration = codingRecord.DateEnd - codingRecord.DateStart;
            }

            return codingRecords;
        }
    }
}
