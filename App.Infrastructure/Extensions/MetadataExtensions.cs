using App.Infrastructure.MetadataModels;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace App.Infrastructure.Extensions;

/// <summary>
/// RU: Работа с метаданными контекста базы данных
/// <br/><br/>
/// EN: Work with metadata of database context
/// </summary>
public static class MetadataExtensions
{
    internal static JsonSerializerOptions GetOptions()
    {
        return new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            MaxDepth = 256,
            IncludeFields = true,
            WriteIndented = true // for beautiful formatting of a json document
        };
    }

    /// <summary>
    /// RU: Сгенерировать метаданные
    /// <br/><br/>
    /// EN: Generate metadata
    /// </summary>
    /// <param name="_dbContext">
    ///     RU: сам контекст базы данных
    ///     <br/><br/>
    ///     EN: database context itself
    /// </param>
    public static void GenerateMetadata(this DbContext _dbContext)
    {
        var designTimeModel = _dbContext.GetService<IDesignTimeModel>();
        var database = $"{_dbContext.Database.GetDbConnection().Database}";
        var metadataSchemas = new Dictionary<string, HashSet<MetadataUnit>>();

        if (designTimeModel.Model.GetPostgresEnums().Count > 0)
        {
            metadataSchemas.Add("pg_enums", [new() {
                Schema = "pg_enums",
                EntityName = "PosgtresEnum",
                Enums = designTimeModel.Model.GetPostgresEnums().Select(e => new MetadataEnum { Schema = e.Schema ?? "public", Name = e.Name, Labels = [.. e.Labels] }).ToArray()
            }]);
        }

        foreach (var entityType in designTimeModel.Model.GetEntityTypes()) // entities/tables
        {
            var metadataUnit = new MetadataUnit
            {
                Schema = entityType.GetSchema() ?? "public",
                TableName = entityType.GetTableName(),
                EntityName = entityType.Name,
                Comment = entityType.GetComment(),
            };

            var storage = new HashSet<MetadataUnit>();

            if (!metadataSchemas.TryGetValue(metadataUnit.Schema, out storage))
            {
                storage = [];
                metadataSchemas.Add(metadataUnit.Schema, storage);
            }

            storage.Add(metadataUnit);

            foreach (var property in entityType.GetProperties()) // properties/fields
            {
                var clrTypeName = property.ClrType.Name;
                if (property.ClrType.IsGenericType && property.ClrType.Name.StartsWith("Nullable")) 
                {
                    clrTypeName = property.ClrType.GenericTypeArguments.Length > 0 ? property.ClrType.GenericTypeArguments[0].Name : string.Empty;
                }
                
                metadataUnit.Properties.Add(new MetadataProperty()
                {
                    Name = property.Name,
                    ColumnName = property.GetColumnName(),
                    Type = clrTypeName,
                    ColumnType = property.GetColumnType(),
                    LengthOrPrecision = property.GetMaxLength() ?? property.GetPrecision(),
                    Scale = property.GetScale(),
                    Comment = property.GetComment(),
                    ColumnOrder = property.GetColumnOrder(),
                    IdentityMinValue = property.GetIdentityMinValue(),
                    IdentityStartValue = property.GetIdentityStartValue(),
                    IdentityMaxValue = property.GetIdentityMaxValue(),
                    IsPrimaryKey = property.IsPrimaryKey(),
                    IsForeignKey = property.IsForeignKey(),
                    IsNullable = property.IsNullable,
                    IsIndex = property.IsIndex(),
                    IsUniqueIndex = property.IsUniqueIndex(),
                    IsStored = property.GetIsStored(),
                    ValueGenerated = property.ValueGenerated,
                    DefaultValue = property.GetDefaultValue(),
                    DefaultValueSql = property.GetDefaultValueSql(),
                    Collation = property.GetCollation(),
                    ComputedSQL = property.GetComputedColumnSql()
                });
            }

            metadataUnit.Properties = [.. metadataUnit.Properties.OrderBy(p => p.ColumnOrder)];

            foreach (var navigation in entityType.GetNavigations()) // entities navigations
            {
                var _navigation = new MetadataNavigation()
                {
                    FkSchema = navigation.ForeignKey.DeclaringEntityType.GetSchema() ?? "public",
                    FkTableName = navigation.ForeignKey.DeclaringEntityType.GetTableName(),
                    FkEntityName = navigation.ForeignKey.DeclaringEntityType.Name,
                    Type = $"{(navigation.ForeignKey.PrincipalKey.Properties.Count > 0 ? "1" : "N")}:{(navigation.ForeignKey.IsUnique ? "1" : "N")}",
                    PkSchema = navigation.ForeignKey.PrincipalEntityType.GetSchema() ?? "public",
                    PkTableName = navigation.ForeignKey.PrincipalEntityType.GetTableName(),
                    PkEntityName = navigation.ForeignKey.PrincipalEntityType.Name
                };
                if (metadataUnit.FullTableName == _navigation.FullFkTableName) // only for the one that is being analyzed, otherwise duplicates of navigations
                {
                    metadataUnit.Navigations.Add(_navigation);

                    foreach (var fkProp in navigation.ForeignKey.Properties)
                    {
                        _navigation.ForeignKeys.Add(new ValueTuple<string, string>(fkProp.Name, fkProp.GetColumnName()));
                    }

                    foreach (var pkProp in navigation.ForeignKey.PrincipalKey.Properties)
                    {
                        _navigation.PrincipalKeys.Add(new ValueTuple<string, string>(pkProp.Name, pkProp.GetColumnName()));
                    }
                }
            }
        }
        
        using var sw = new StreamWriter($"{database}.metadata.json", false, Encoding.UTF8);
        sw.Write(JsonSerializer.Serialize(metadataSchemas, GetOptions()));
    }
}
