using App.Attributes;
using App.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entities;

/// <summary>
/// Test 2
/// </summary>
[Table("tests_2"), PrimaryKey(nameof(TestId1), nameof(Number)), SummaryAsComment]
public class TestEntity2: IEntity
{
    /// <summary>
    /// Foreign key for Test entity 1
    /// </summary>
    [Column("test_id_1", Order = 0), SummaryAsComment,
     ForeignKeyFor(ToMany = false, ForeignKeyNavigationProperty = nameof(TestEntity1), RelatedNavigationProperty = nameof(TestEntity1.TestEntity2), RelatedKey = nameof(TestEntity1.Id), RelatedType = nameof(Entities.TestEntity1))]
    public int TestId1 { get; set; }

    /// <summary>
    /// Number
    /// </summary>
    [Column("number", Order = 1), SummaryAsComment, WithDefaultValue]
    public int Number { get; set; } = 1;

    /// <summary>
    /// Description
    /// </summary>
    [Column("desctiption", Order = 2), SummaryAsComment]
    public string? Description { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public TestEntity1 TestEntity1 { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public HashSet<TestEntity3> TestEntities3 { get; set; } = [];
}
