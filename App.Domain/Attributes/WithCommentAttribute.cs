namespace App.Domain.Attributes;

/// <summary>
/// RU: Summary комментарий вместо комментария EntityFramework
/// <br/><br/>
/// EN: Summary comment instead of comment of EntityFramework
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public sealed class WithCommentAttribute : Attribute
{
}
