namespace App.Infrastructure;

public class DatabaseContext(string connectionString, string? entityDllName = null) : ExtendedDbContext(connectionString, entityDllName, codeFirstModel_deleteBeforeCreate: true)
{
}
