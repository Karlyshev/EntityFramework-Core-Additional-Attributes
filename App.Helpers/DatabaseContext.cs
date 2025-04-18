using App.Attributes;
using App.Common;
using App.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace App.Infrastructure;

public class DatabaseContext: DbContext
{
    private readonly string connectionString;

    public DatabaseContext(string connectionString) : base() 
    {
        this.connectionString = connectionString;

#if DEBUG
        Database.EnsureDeleted();
#endif
        Database.EnsureCreated();
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
        var entityTypes = Assembly.LoadFrom("App.Entities.dll").GetTypes().Where(t => t.IsClass && !t.IsAbstract && typeof(IEntity).IsAssignableFrom(t)).ToArray();

        foreach (var entityType in entityTypes)
        {
            var method = typeof(DatabaseContext).GetMethod(nameof(ConfigureEntity), BindingFlags.NonPublic | BindingFlags.Instance)?.MakeGenericMethod(entityType);
            method?.Invoke(this, [modelBuilder]);
        }
    }

    protected void ConfigureEntity<TEntity>(ModelBuilder modelBuilder) where TEntity : class, IEntity
    {
        var instanceInitialized = false;

        var entityType = typeof(TEntity);
        var entityXmlDoc = new Dictionary<string, string>();
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
                propertyType == typeof(Guid) || // Guid
                propertyType.IsEnum; // enum
        }).ToArray();

        var builder = modelBuilder.Entity<TEntity>();
        builder.ApplyForeignKeyAttribute(entityType.GetCustomAttribute<ForeignKeyForAttribute>());
        builder.ApplySummaryAsCommentAttribute(entityType.GetCustomAttribute<SummaryAsCommentAttribute>(), ref entityXmlDoc);

        if (properties.Length > 0)
        {
            foreach (var property in properties)
            {
                builder.ApplyForeignKeyAttribute(property.GetCustomAttribute<ForeignKeyForAttribute>(), property.Name);
                builder.ApplySummaryAsCommentAttribute(property.GetCustomAttribute<SummaryAsCommentAttribute>(), ref entityXmlDoc, property.Name);

                if (property.GetCustomAttribute<WithDefaultValueAttribute>() is not null)
                {
                    if (!instanceInitialized)
                    {
                        entityInstance = Activator.CreateInstance<TEntity>();
                        instanceInitialized = true;
                    }

                    var defaultValue = property.GetValue(entityInstance);
                    if (defaultValue is not null)
                    {
                        builder.Property(property.Name).HasDefaultValue(defaultValue);
                    }
                }
            }
        }
    }
}
