using System.Reflection;
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
    /// <param name="dllName"></param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    public static Dictionary<string, string> GetSummaries(string dllName) 
    {
        var result = new Dictionary<string, string>();
        if (File.Exists(dllName))
        {
            var assembly = Assembly.LoadFrom(dllName);
            var xmlPath = assembly.Location.Replace(".dll", ".xml");

            if (File.Exists(xmlPath))
            {
                foreach (var member in XDocument.Load(xmlPath).Descendants("member"))
                {
                    var nameAttr = member.Attribute("name");
                    if (nameAttr is not null)
                    {
                        result.Add(nameAttr.Value.Trim(), member.Element("summary")?.Value.Trim() ?? string.Empty);
                    }
                }
            }
        }
        return result;
    }
}
