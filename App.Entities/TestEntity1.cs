using App.Attributes;
using App.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entities;

/// <summary>
/// Test 1
/// </summary>
[Table("tests_1"), SummaryAsComment]
public class TestEntity1: IEntity
{
    /// <summary>
    /// Identifier
    /// </summary>
    [Column("id", Order = 0), SummaryAsComment]
    public int Id { get; set; }

    /// <summary>
    /// Foreign key for Main entity
    /// </summary>
    [Column("main_id", Order = 1), SummaryAsComment,
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
