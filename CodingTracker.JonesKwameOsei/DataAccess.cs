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
        using var connection = new SqliteConnection(ConnectionString);

        connection.Open();

        const string createTableQuery = @"
             CREATE TABLE IF NOT EXISTS codingSessions (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Language   TEXT NOT NULL,
                    DateStart  TEXT NOT NULL,
                    DateEnd    TEXT NOT NULL
             )";

        connection.Execute(createTableQuery);
    }

    internal void InsertRecord(CodingRecord codingRecord)
    {
        if (codingRecord is null) throw new ArgumentNullException(nameof(codingRecord));
        if (string.IsNullOrWhiteSpace(codingRecord.Language))
            throw new ArgumentException("Language cannot be null or empty.", nameof(codingRecord.Language));

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

    internal void InsertMultipleRecords(IEnumerable<CodingRecord> codingRecords)
    {
        if (codingRecords is null) throw new ArgumentNullException(nameof(codingRecords));

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        string insertQueries = @"
                INSERT INTO codingSessions (Language, DateStart, DateEnd)
                VALUES (@Language, @DateStart, @DateEnd)";

        var sanitised = codingRecords.Select(r =>
        {
            if (string.IsNullOrWhiteSpace(r.Language))
                r.Language = "C#";
            return new
            {
                r.Language,
                r.DateStart,
                r.DateEnd
            };
        });

        connection.Execute(insertQueries, sanitised);
    }

    internal IEnumerable<CodingRecord> GetAllSessions()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        const string selectQuery = "SELECT Id, Language, DateStart, DateEnd FROM codingSessions ORDER BY DateStart";

        var codingRecords = connection.Query<CodingRecord>(selectQuery).ToList();

        foreach (var cr in codingRecords)
        {
            cr.Duration = cr.DateEnd - cr.DateStart;
        }

        return codingRecords;
    }

    internal void ResetSessions()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        connection.Execute("DELETE FROM codingSessions");
        connection.Execute("DELETE FROM sqlite_sequence WHERE name = 'codingSessions';");
    }

    internal void UpdateRecord(CodingRecord updateRecord)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        const string updateQuery = @"
            UPDATE codingSessions
            SET Language = @Language, DateStart = @DateStart, DateEnd = @DateEnd
            WHERE Id = @Id";

        connection.Execute(updateQuery, new
        {
            updateRecord.Language,
            updateRecord.DateStart,
            updateRecord.DateEnd,
            updateRecord.Id
        });
    }

    internal int DeleteRecord(int id)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        const string deleteQuery = "DELETE FROM codingSessions WHERE Id = @Id";
        int rowsAffected = connection.Execute(deleteQuery, new { Id = id });

        return rowsAffected;
    }
}
