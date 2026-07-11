using System.ComponentModel;
using System.Reflection;

namespace CorpInsightsTW.Core.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// 取得 Enum 標註的 Description 文字（若無則回傳 ToString()）
    /// </summary>
    public static string ToCode(this Enum value)
    {
        FieldInfo? field = value.GetType().GetField(value.ToString());
        if (field == null) return value.ToString();

        var attribute = field.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? value.ToString();
    }
}