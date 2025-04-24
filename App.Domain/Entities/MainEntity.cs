using App.Domain.Attributes;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Entities;

/// <summary>
/// Main entity
/// </summary>
[Table("mains"), Comment("Main entities")]
public class MainEntity: BaseEntityWithDescription
{
    /// <summary>
    /// Identifier
    /// </summary>
    [Key, Column("id"), WithComment]
    public int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public HashSet<TestEntity1> TestEntities1 { get; set; } = [];
}
