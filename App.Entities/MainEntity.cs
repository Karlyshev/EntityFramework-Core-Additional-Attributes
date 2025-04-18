using App.Attributes;
using App.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entities;

/// <summary>
/// Main entity
/// </summary>
[Table("mains"), Comment("Main entities")]
public class MainEntity: IEntity
{
    /// <summary>
    /// Identifier
    /// </summary>
    [Key, Column("id", Order = 0), SummaryAsComment]
    public int Id { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    [Column("description", Order = 1), SummaryAsComment, WithDefaultValue]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public HashSet<TestEntity1> TestEntities1 { get; set; } = [];
}
