namespace App.Infrastructure.MetadataModels;

internal class MetadataNavigation
{
    public required string FkSchema { get; set; }
    public required string FkEntityName { get; set; }
    public string? FkTableName { get; set; }

    public HashSet<ValueTuple<string, string>> ForeignKeys { get; set; } = [];

    public required string PkSchema { get; set; }
    public required string PkEntityName { get; set; }
    public string? PkTableName { get; set; }

    public HashSet<ValueTuple<string, string>> PrincipalKeys { get; set; } = [];

    public string FullFkTableName => !string.IsNullOrEmpty(FkTableName) ? $"{FkSchema}.{FkTableName}" : FkEntityName;
    public string FullPkTableName => !string.IsNullOrEmpty(PkTableName) ? $"{PkSchema}.{PkTableName}" : PkEntityName;

    public string FullName => $"{FullFkTableName}({string.Join(',', ForeignKeys.Select(fk => !string.IsNullOrEmpty(fk.Item2) ? fk.Item2 : fk.Item1))}) -> {FullPkTableName}({string.Join(',', PrincipalKeys.Select(pk => !string.IsNullOrEmpty(pk.Item2) ? pk.Item2 : pk.Item1))})";

    public required string Type { get; set; }
}
