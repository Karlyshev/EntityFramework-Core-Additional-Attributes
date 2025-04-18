using App.Attributes;
using App.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infrastructure.Helpers;

internal static class ForeignKeyAttributeHelper
{
    public static void ApplyForeignKeyAttribute<TEntity>(this EntityTypeBuilder<TEntity> builder, ForeignKeyForAttribute? foreignKeyAttr, string? targetPropertyName = null) 
        where TEntity : class, IEntity
    {
        if (foreignKeyAttr is not null && foreignKeyAttr.ForeignKeyNavigationProperty is not null)
        {
            var entityType = typeof(TEntity);
            if ((targetPropertyName is not null || foreignKeyAttr.ForeignKeys?.Length > 0)
                && (targetPropertyName is not null && foreignKeyAttr.RelatedKey is not null || foreignKeyAttr.ForeignKeys?.Length > 0 && foreignKeyAttr.RelatedKeys?.Length > 0))
            {
                if (foreignKeyAttr.ToMany)
                {
                    builder.HasOne(foreignKeyAttr.ForeignKeyNavigationProperty).WithMany(foreignKeyAttr.RelatedNavigationProperty)
                           .HasForeignKey(targetPropertyName is not null ? [targetPropertyName] : foreignKeyAttr.ForeignKeys!)
                           .HasPrincipalKey(foreignKeyAttr.RelatedKey is not null ? [ foreignKeyAttr.RelatedKey ] : foreignKeyAttr.RelatedKeys!);
                }
                else if (foreignKeyAttr.RelatedType is not null && foreignKeyAttr.RelatedNavigationProperty is not null)
                {
                    builder.HasOne(foreignKeyAttr.ForeignKeyNavigationProperty).WithOne(foreignKeyAttr.RelatedNavigationProperty)
                           .HasForeignKey(entityType.Name, targetPropertyName is not null ? [targetPropertyName] : foreignKeyAttr.ForeignKeys!)
                           .HasPrincipalKey(foreignKeyAttr.RelatedType, foreignKeyAttr.RelatedKey is not null ? [foreignKeyAttr.RelatedKey] : foreignKeyAttr.RelatedKeys!);
                }
            }
        }
    }
}
