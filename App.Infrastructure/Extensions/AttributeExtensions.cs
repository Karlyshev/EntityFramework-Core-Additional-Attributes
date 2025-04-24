using System.Reflection;

namespace App.Infrastructure.Extensions;

public static class AttributeExtensions
{
    /// <summary>
    /// RU: Получает атрибут свойства, включая его объявления в интерфейсах (если его нет в классе).
    /// <br/><br/>
    /// EN: Get the property attribute, including its declarations in interfaces (if it is not in the class).
    /// </summary>
    public static TAttribute? GetCustomAttributeWithInterfaces<TAttribute>(this PropertyInfo property, bool inherit = true)
        where TAttribute : Attribute
    {
        ArgumentNullException.ThrowIfNull(property);

        // RU: 1. Сначала проверяем атрибут в самом классе (приоритет)
        // EN: 1. Check the attribute in the class itself (priority)
        var classAttribute = property.GetCustomAttribute<TAttribute>(inherit);
        if (classAttribute != null)
            return classAttribute;

        // RU: 2. Если в классе нет, ищем во всех интерфейсах
        // EN: 2. If there is no class then search in all interfaces.
        var interfacesWithProperty = property.DeclaringType?.GetInterfaces()
                                                            .Where(i => i.GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance) != null)
                                                            .ToArray();

        if (interfacesWithProperty == null || interfacesWithProperty.Length == 0)
            return null;

        // RU: 3. Возвращаем первый найденный атрибут из интерфейсов
        // EN: 3. Return the first attribute found from the interfaces
        return interfacesWithProperty.Select(i => i.GetProperty(property.Name)?.GetCustomAttribute<TAttribute>())
                                     .FirstOrDefault(attr => attr != null);
    }

    /// <summary>
    /// RU: Получает атрибут класса, включая его объявления в интерфейсах (если его нет в классе).
    /// <br/><br/>
    /// EN: Get the class attribute, including its declarations in interfaces (if it is not in the class).
    /// </summary>
    public static TAttribute? GetCustomAttributeWithInterfaces<TAttribute>(this Type type, bool inherit = true)
        where TAttribute : Attribute
    {
        ArgumentNullException.ThrowIfNull(type);

        // RU: 1. Сначала проверяем атрибут в самом классе (приоритет)
        // EN: 1. Check the attribute in the class itself (priority)
        var classAttribute = type.GetCustomAttribute<TAttribute>(inherit);
        if (classAttribute != null)
            return classAttribute;

        // RU: 2. Если в классе нет, ищем во всех интерфейсах и возвращаем первый найденный атрибут из интерфейса
        // EN: 2. If there is no class then search in all interfaces and return the first attribute found from the interfaces.
        return type.GetInterfaces()
                   .Select(i => i.GetCustomAttribute<TAttribute>())
                   .FirstOrDefault(attr => attr != null);
    }
}