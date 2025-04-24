using Microsoft.EntityFrameworkCore.Metadata;

namespace App.Infrastructure.MetadataModels;

internal class MetadataProperty
{
    public required string Name { get; set; }
    public string? ColumnName { get; set; }
    public required string Type { get; set; }
    public string? ColumnType { get; set; }
    public int? LengthOrPrecision { get; set; }
    public int? Scale { get; set; }
    public string? Comment { get; set; }
    public int? ColumnOrder { get; set; }
    public long? IdentityMinValue { get; set; }
    public long? IdentityStartValue { get; set; }
    public long? IdentityMaxValue { get; set; }
    public bool IsPrimaryKey { get; set; }
    public bool IsForeignKey { get; set; }
    public bool IsNullable { get; set; }
    public bool IsIndex { get; set; }
    public bool IsUniqueIndex { get; set; }
    public bool? IsStored { get; set; }
    public ValueGenerated? ValueGenerated { get; set; }
    public object? DefaultValue { get; set; }
    public string? DefaultValueSql { get; set; }
    public string? Collation { get; set; }
    public string? ComputedSQL { get; set; }
}
