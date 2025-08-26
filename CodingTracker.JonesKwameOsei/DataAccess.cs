using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker.JonesKwameOsei;

internal class DataAccess
{
    private string ConnectionString;
    public DataAccess(string connectionString)
    {
        ConnectionString = connectionString;
    }
    internal void CreateDatabase()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            string createTableQuery = @"
             CREATE TABLE IF NOT EXISTS CodingSessions (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Language TEXT NOT NULL,
                    DateStart TEXT NOT NULL,
                    DateEnd TEXT NOT NULL
             )";

            connection.Execute(createTableQuery);
        }
    }
}
