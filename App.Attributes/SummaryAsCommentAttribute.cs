namespace App.Attributes;

/// <summary>
/// RU: Summary комментарий как комментарий EntityFramework
/// <br/><br/>
/// EN: Summary comment as comment of EntityFramework
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public sealed class SummaryAsCommentAttribute : Attribute
{
}
