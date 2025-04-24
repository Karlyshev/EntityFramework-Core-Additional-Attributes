using App.Domain;
using App.Domain.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

namespace App.Infrastructure.Helpers;
internal static class WithDefaultValueAttributeHelper
{
    public static void AppleWithDefaultValueAttribute<TEntity>(this EntityTypeBuilder<TEntity> builder, WithDefaultValueAttribute? defaultValueAttribute, ref TEntity? entityInstance, PropertyInfo targetProperty)
        where TEntity : class, IEntity
    {
        if (defaultValueAttribute is not null && targetProperty is not null) 
        {
            entityInstance ??= Activator.CreateInstance<TEntity>();

            if (defaultValueAttribute.SqlDefaultValue is not null)
            {
                builder.Property(targetProperty.Name).HasDefaultValueSql(defaultValueAttribute.SqlDefaultValue);
            }
            else 
            {
                var defaultValue = targetProperty.GetValue(entityInstance);
                if (defaultValue is not null)
                {
                    builder.Property(targetProperty.Name).HasDefaultValue(defaultValue);
                }
            }
        }
    }
}
