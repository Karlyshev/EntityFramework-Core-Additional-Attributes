using App.Domain;
using App.Domain.Attributes;
using App.Infrastructure.Extensions;
using App.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace App.Infrastructure;

public abstract class ExtendedDbContext: DbContext
{
    private readonly string connectionString;
    private readonly string? entityDllName;
    protected static Dictionary<string, string>? xmlDocs;

    public ExtendedDbContext(string connectionString, string? entityDllName = null, bool isCodeFirstModel = true, bool codeFirstModel_deleteBeforeCreate = false) : base()
    {
        this.connectionString = connectionString;
        this.entityDllName = entityDllName;

        if (isCodeFirstModel)
        {
            if (codeFirstModel_deleteBeforeCreate)
            {
                Database.EnsureDeleted();
            }
            Database.EnsureCreated();
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

#if DEBUG
        optionsBuilder.LogTo(Console.WriteLine);
#endif

        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        List<Type>? entityTypes = [];
        if (entityDllName is not null)
        {
            entityTypes = [.. Assembly.LoadFrom(entityDllName).GetTypes().Where(t => t.IsClass && !t.IsAbstract && typeof(IEntity).IsAssignableFrom(t))];
            xmlDocs = XmlDocumentationHelper.GetSummaries(entityDllName);
        }
        else
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<string> xmlDocFileNames = [];
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && typeof(IEntity).IsAssignableFrom(t)).ToArray();
                entityTypes.AddRange(types);
                if (types.Length > 0 && assembly.FullName is not null)
                {
                    var assemblyName = assembly.GetName().Name;
                    if (assemblyName is not null)
                    {
                        xmlDocFileNames.AddRange($"{assemblyName}.dll");
                    }
                }
            }
            if (xmlDocFileNames.Count > 0)
            {
                foreach (var xmlDocFileName in xmlDocFileNames)
                {
                    var localXmlDoc = XmlDocumentationHelper.GetSummaries(xmlDocFileName);
                    xmlDocs = [];
                    foreach (var locXmlDocLine in localXmlDoc)
                    {
                        xmlDocs.Add(locXmlDocLine.Key, locXmlDocLine.Value);
                    }
                }
            }
        }

        var type = GetType().BaseType ?? GetType();

        foreach (var entityType in entityTypes)
        {
            var method = type.GetMethod(nameof(ConfigureEntity), BindingFlags.Static | BindingFlags.NonPublic)?.MakeGenericMethod(entityType);
            method?.Invoke(this, [modelBuilder]);
        }
    }

    private static void ConfigureEntity<TEntity>(ModelBuilder modelBuilder) where TEntity : class, IEntity
    {
        var entityType = typeof(TEntity);
        TEntity? entityInstance = null;
        var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p =>
        {
            var propertyType = p.PropertyType;

            if (Nullable.GetUnderlyingType(propertyType) is not null)
                propertyType = Nullable.GetUnderlyingType(propertyType)!;

            return
                propertyType.IsPrimitive || // int, bool, double и т.д.
                propertyType == typeof(string) || // string
                propertyType == typeof(decimal) || // decimal
                propertyType == typeof(DateTime) || // DateTime
                propertyType == typeof(DateOnly) || //DateOnly
                propertyType == typeof(Guid) || // Guid
                propertyType.IsEnum; // enum
        }).ToArray();

        var builder = modelBuilder.Entity<TEntity>();
        builder.ApplyForeignKeyAttribute(entityType.GetCustomAttribute<ForeignKeyForAttribute>());
        builder.ApplySummaryAsCommentAttribute(entityType.GetCustomAttribute<WithCommentAttribute>(), xmlDocs);

        if (properties.Length > 0)
        {
            for (var i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                builder.ApplyForeignKeyAttribute(property.GetCustomAttributeWithInterfaces<ForeignKeyForAttribute>(), property.Name);
                builder.ApplySummaryAsCommentAttribute(property.GetCustomAttributeWithInterfaces<WithCommentAttribute>(), xmlDocs, property);
                builder.AppleWithDefaultValueAttribute(property.GetCustomAttributeWithInterfaces<WithDefaultValueAttribute>(), ref entityInstance, property);
                builder.Property(property.Name).HasColumnOrder(i);
            }
        }
    }
}