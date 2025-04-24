using App.Domain.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain;

/// <summary>
/// 
/// </summary>
public abstract class BaseEntityWithDescription: IEntity
{
    /// <summary>
    /// Description
    /// </summary>
    [Column("description"), WithComment, WithDefaultValue]
    public string Description { get; set; } = string.Empty;
}
