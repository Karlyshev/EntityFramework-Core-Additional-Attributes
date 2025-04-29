using App.Domain.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Entities;

/// <summary>
/// Test 1
/// </summary>
[Table("tests_1"), WithComment]
public class TestEntity1: BaseEntityWithDescription, IEntity
{
    /// <summary>
    /// Identifier
    /// </summary>
    [Key, WithComment]
    public int Id { get; set; }

    /// <summary>
    /// Foreign key for Main entity
    /// </summary>
    [WithComment,
     ForeignKeyFor(ForeignKeyNavigationProperty = nameof(MainEntity), RelatedNavigationProperty = nameof(MainEntity.TestEntities1), RelatedKey = nameof(MainEntity.Id))]
    public int MainId { get; set; }

    /// <summary>
    /// Main entity
    /// </summary>
    public MainEntity MainEntity { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public TestEntity2 TestEntity2 { get; set; } = null!;
}
