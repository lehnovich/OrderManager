using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

public static class EnumHelper
{
    /// <summary>
    /// Отображает значение Name из атрибута Display над енамкой
    /// </summary>
    /// <typeparam name="TEnum">Тип енамки</typeparam>
    /// <param name="value">Значение енамки</param>
    public static string? GetDisplayName<TEnum>(this TEnum value)
        where TEnum : struct, IConvertible
    {
        var attr = value.GetAttributeOfType<TEnum, DisplayAttribute>();
        return attr == null ? value.ToString() : attr.Name;
    }

    /// <summary>
    /// Получить атрибуты енамки
    /// </summary>
    /// <typeparam name="TEnum">Тип енамки</typeparam>
    /// <typeparam name="T">Тип аттрибута для отображения</typeparam>
    /// <param name="value">Значение енамки</param>
    private static T? GetAttributeOfType<TEnum, T>(this TEnum value)
        where TEnum : struct, IConvertible
        where T : Attribute
    {
        return value.GetType()
                    .GetMember(value.ToString())
                    .First()
                    .GetCustomAttributes(false)
                    .OfType<T>()
                    .LastOrDefault();
    }
}