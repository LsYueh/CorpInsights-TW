using System.Reflection;
using CorpInsightsTW.Core.Attributes;

namespace CorpInsightsTW.Core.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// 取得 Enum 標註的 OpenAPI 網址代碼
    /// </summary>
    public static string ToCode<TEnum>(this TEnum value) where TEnum : Enum
    {
        FieldInfo? field = value.GetType().GetField(value.ToString());
        if (field == null) return value.ToString();

        var attribute = field.GetCustomAttribute<CodeAttribute>();
        return attribute?.Value ?? value.ToString();
    }

    /// <summary>
    /// 取得 Enum 標註的顯示名稱
    /// </summary>
    public static string ToDisplay<TEnum>(this TEnum value) where TEnum : Enum
    {
        FieldInfo? field = value.GetType().GetField(value.ToString());
        if (field == null) return value.ToString();

        // 🎯 改為尋找我們自訂的 Display 標籤
        var attribute = field.GetCustomAttribute<DisplayAttribute>();
        return attribute?.Name ?? value.ToString();
    }
}