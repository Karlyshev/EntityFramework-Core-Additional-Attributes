namespace App.Attributes;

/// <summary>
/// RU: Атрибут маркирует свойство класса, имеющее значение по умолчанию
/// <br/><br/>
/// EN: The attribute marks a class property that has a default value.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class WithDefaultValueAttribute: Attribute
{

}
