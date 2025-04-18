using App.Attributes;
using App.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entities;

/// <summary>
/// Test 3
/// </summary>
[Table("tests_3"), SummaryAsComment,
 ForeignKeyFor(ForeignKeys = [ nameof(TestId1), nameof(TestNumber) ], ForeignKeyNavigationProperty = nameof(TestEntity2), 
               RelatedKeys = [ nameof(TestEntity2.TestId1), nameof(TestEntity2.Number) ], RelatedNavigationProperty = nameof(TestEntity2.TestEntities3))]
public class TestEntity3: IEntity
{
    /// <summary>
    /// Identifier
    /// </summary>
    [Key, Column("id", Order = 0), SummaryAsComment]
    public int Id { get; set; }

    /// <summary>
    /// Foreign key for Test entity 2 (1/2)
    /// </summary>
    [Column("test_id_1", Order = 1), SummaryAsComment]
    public int? TestId1 { get; set; }

    /// <summary>
    /// Foreign key for Test entity 2 (2/2)
    /// </summary>
    [Column("test_number", Order = 2), SummaryAsComment]
    public int? TestNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public TestEntity2? TestEntity2 { get; set; }
}
