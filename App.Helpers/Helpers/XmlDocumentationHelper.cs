using System.Xml.Linq;

namespace App.Infrastructure.Helpers;

/// <summary>
/// 
/// </summary>
public static class XmlDocumentationHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Dictionary<string, string> GetSummaries(this Type type) 
    {
        // Загружаем XML-документацию для сборки
        var assembly = type.Assembly;
        var xmlPath = assembly.Location.Replace(".dll", ".xml");

        if (!File.Exists(xmlPath))
        {
            throw new FileNotFoundException("File XML documentation not found");
        }

        var result = new Dictionary<string, string>();

        foreach (var member in XDocument.Load(xmlPath).Descendants("member"))
        {
            var nameAttr = member.Attribute("name");
            if (nameAttr is not null) 
            {
                var PName = $"P:{type.FullName}.";
                var TName = $"T:{type.FullName}";

                if (nameAttr.Value.StartsWith(PName))
                {
                    result.Add(nameAttr.Value.Replace(PName, string.Empty).Trim(), member.Element("summary")?.Value.Trim() ?? string.Empty);
                }
                else if (nameAttr.Value.StartsWith(TName))
                {
                    result.Add(nameAttr.Value.Replace(TName, "{Type}").Trim(), member.Element("summary")?.Value.Trim() ?? string.Empty);
                }
            }
        }

        return result;
    }
}
