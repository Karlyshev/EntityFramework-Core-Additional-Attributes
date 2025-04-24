namespace App.Infrastructure.MetadataModels;

internal class MetadataUnit
{
    public required string Schema { get; set; }
    public required string EntityName { get; set; }
    public string? TableName { get; set; }
    public string? Comment { get; set; }

    public MetadataEnum[]? Enums { get; set; }

    public HashSet<MetadataProperty> Properties { get; set; } = [];

    public HashSet<MetadataNavigation> Navigations { get; set; } = [];

    public string FullTableName => !string.IsNullOrEmpty(TableName) ? $"{Schema}.{TableName}" : EntityName;
}
