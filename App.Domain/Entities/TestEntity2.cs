using App.Domain.Attributes;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Entities;

/// <summary>
/// Test 2
/// </summary>
[Table("tests_2"), PrimaryKey(nameof(TestId1), nameof(Number)), WithComment]
public class TestEntity2: BaseEntityWithDescription, IEntity
{
    /// <summary>
    /// Foreign key for Test entity 1
    /// </summary>
    [WithComment,
     ForeignKeyFor(ToMany = false, ForeignKeyNavigationProperty = nameof(TestEntity1), RelatedNavigationProperty = nameof(TestEntity1.TestEntity2), RelatedKey = nameof(TestEntity1.Id), RelatedType = nameof(Entities.TestEntity1))]
    public int TestId1 { get; set; }

    /// <summary>
    /// Number
    /// </summary>
    [WithComment, WithDefaultValue]
    public int Number { get; set; } = 1;

    /// <summary>
    /// 
    /// </summary>
    public TestEntity1 TestEntity1 { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public HashSet<TestEntity3> TestEntities3 { get; set; } = [];
}
