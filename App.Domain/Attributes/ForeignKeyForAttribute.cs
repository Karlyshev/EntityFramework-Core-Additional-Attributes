namespace App.Domain.Attributes;

/// <summary>
/// RU: 
/// Настройка внешнего ключа. Само свойство (целевое свойство), для которого настраивается этот атрибут является самим внешним ключом (аналогично тому если бы указывали в HasForeignKey).
/// <br/> Если атрибут настраивается на класс, то необходимо указать <see cref="ForeignKeys"/> вместо целевого свойства и <see cref="RelatedKeys"/> вместо <see cref="RelatedKey"/>
/// <br/><br/>
/// EN: Setting for Foreign key. The property itself (the target property) that this attribute is configuring for is the foreign key itself (similar to how it would be specified in HasForeignKey).
/// <br/> If the attribute is configuring for a class, then must specify <see cref="Foreign Keys"/> instead of the target property and <see cref="RelatedKeys"/> instead of <see cref="RelatedKey"/>
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
public sealed class ForeignKeyForAttribute: Attribute
{
    /// <summary>
    /// RU: Тип внешнего ключа "Один-ко-многим" (<see langword="true"/>) или "Один-к-одному" (<see langword="false"/>)
    /// <br/><br/>
    /// EN: Foreign key type "One-to-many" (<see langword="true"/>) или "One-to-one" (<see langword="false"/>)
    /// </summary>
    public bool ToMany { get; set; } = true;

    /// <summary>
    /// RU: Само свойство ключа, т.е. сущность, к которой ассоциируется внешний ключ (аналогично тому если бы указывали в HasOne)
    /// <br/><br/>
    /// EN: The property of foreign key itself, i.e. the entity to which the foreign key is associated (similar to how it would be specified in hasOne)
    /// </summary>
    public string? ForeignKeyNavigationProperty { get; set; }

    /// <summary>
    /// RU: Свойство, к которому ссылается сущность по внешнему ключу (WithOne/WithMany)
    /// <br/><br/>
    /// EN: The property that the entity refers to by a foreign key (With One/With Many)
    /// </summary>
    public string? RelatedNavigationProperty { get; set; }

    /// <summary>
    /// RU: Свойство-ключ, на который ссылается внешний ключ (аналогично тому если бы указывали в HasPrincipalKey)
    /// <br/><br/>
    /// EN: Property-Key is referenced by the foreign key (similar to how it would be specified in HasPrincipalKey)
    /// </summary>
    public string? RelatedKey { get; set; }

    /// <summary>
    /// RU: Тип сущности (для отношения "Один-ко-одному")
    /// <br/><br/>
    /// EN: Type of entity (for relation "One-to-many")
    /// </summary>
    public string? RelatedType { get; set; }

    /// <summary>
    /// RU: Свойства-ключи являющиеся внешними ключами (составной ключ, аналогично тому если бы указывали в HasForeignKey)
    /// <br/><br/>
    /// EN: Properties-Keys are foreign keys (composite key, similar to how it would be specified in HasForeignKey)
    /// </summary>
    public string[]? ForeignKeys { get; set; }

    /// <summary>
    /// RU: Свойства-ключи, на которые ссылаются внешние ключи <see cref="ForeignKeys"/> в строго таком же порядке как они заданы в <see cref="ForeignKeys"/>
    /// <br/><br/>
    /// EN: Properties-Keys are referenced by foreign keys <see cref="ForeignKeys"/> in exactly the same order as they are set in <see cref="ForeignKeys"/>
    /// </summary>
    public string[]? RelatedKeys { get; set; }
}