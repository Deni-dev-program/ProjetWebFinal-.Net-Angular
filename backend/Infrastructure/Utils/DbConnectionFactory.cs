using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;

namespace Infrastructure.Utils;

public class DbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(IConfiguration configuration) =>
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    public IDbConnection CreateConnection() => new MySqlConnection(_connectionString);
}
