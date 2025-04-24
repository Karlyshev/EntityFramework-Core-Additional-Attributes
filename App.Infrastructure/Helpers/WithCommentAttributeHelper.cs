using App.Domain;
using App.Domain.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

namespace App.Infrastructure.Helpers;

internal static class WithCommentAttributeHelper
{
    public static void ApplySummaryAsCommentAttribute<TEntity>(this EntityTypeBuilder<TEntity> builder, WithCommentAttribute? summaryAsCommentAttr, Dictionary<string, string>? summaries = null, MemberInfo? targetProperty = null)
        where TEntity : class, IEntity
    {
        if (summaryAsCommentAttr is not null) 
        {
            var commentMemberName = targetProperty is not null ? $"P:{(targetProperty?.DeclaringType?.FullName ?? string.Empty)}.{targetProperty!.Name}" 
                                                               : $"T:{builder.GetType().GenericTypeArguments[0].FullName}";
            
            if (commentMemberName is not null && summaries?.Count > 0 && summaries.TryGetValue(commentMemberName, out string? comment) && !string.IsNullOrEmpty(comment)) 
            {
                if (targetProperty is not null)
                {
                    builder.Property(targetProperty.Name).HasComment(comment);
                }
                else 
                {
                    builder.ToTable(t => t.HasComment(comment));
                }
            }
        }
    }
}
