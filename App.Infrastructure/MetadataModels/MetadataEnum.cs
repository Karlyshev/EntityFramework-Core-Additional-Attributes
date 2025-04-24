namespace App.Infrastructure.MetadataModels;

internal class MetadataEnum
{
    public string? Schema { get; set; }
    public required string Name { get; set; }
    public string[] Labels { get; set; } = [];
}