using App.Attributes;
using App.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infrastructure.Helpers;

internal static class SummaryAsCommentAttributeHelper
{
    public static void ApplySummaryAsCommentAttribute<TEntity>(this EntityTypeBuilder<TEntity> builder, SummaryAsCommentAttribute? summaryAsCommentAttr, ref Dictionary<string, string> summaries, string? targetPropertyName = null)
        where TEntity : class, IEntity
    {
        if (summaryAsCommentAttr is not null) 
        {
            if (summaries.Count == 0) 
            {
                var entityType = typeof(TEntity);
                summaries = entityType.GetSummaries();
            }

            if (summaries.TryGetValue(targetPropertyName ?? "{Type}", out string? comment) && !string.IsNullOrEmpty(comment)) 
            {
                if (targetPropertyName is not null)
                {
                    builder.Property(targetPropertyName).HasComment(comment);
                }
                else 
                {
                    builder.ToTable(t => t.HasComment(comment));
                }
            }
        }
    }
}
