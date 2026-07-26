using System.Text.Json;
using System.Text.Json.Serialization;
using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.Core.Extensions;
using CorpInsightsTW.Etl.Core.Common;
using CorpInsightsTW.Etl.Core.Json;

namespace CorpInsightsTW.Etl.Dtos;
public static class T187DtoFactory
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        Converters = 
        {
            new T187JsonConverterFactory(),
        }
    };

    /// <summary>
    /// 輕量反序列化出 T187 的 Header
    /// </summary>
    public static T187HeaderDto? ExtractHeader(JsonElement row)
    {
         return DeserializeRow<T187HeaderDto>(row);
    }

    /// <summary>
    /// 根據 EtlContext 將 JsonElement 解析為對應的 T187AP06 或 T187AP07
    /// </summary>
    public static IT187Dto? ToDto(EtlContext context, JsonElement row)
    {
        return context.ApCode switch
        {
            T187ApCode.T187AP06 => MapToAp06Dto(context, row),
            T187ApCode.T187AP07 => MapToAp07Dto(context, row),
            _ => throw new NotSupportedException($"未知的財務報表: {context.ApCode}")
        };
    }

    private static T? DeserializeRow<T>(JsonElement row) where T : IT187Dto
    {
        return row.Deserialize<T>(_jsonOptions);
    }

    /// <summary>
    /// T187Ap06 (綜合損益表) 各業別的解析
    /// </summary>
    private static IT187Dto? MapToAp06Dto(EtlContext context, JsonElement row)
    {
        return context.Taxonomy switch
        {
            XbrlTaxonomy.CI   => DeserializeRow<T187Ap06.CiDto  >(row),
            XbrlTaxonomy.BASI => DeserializeRow<T187Ap06.BasiDto>(row),
            XbrlTaxonomy.BD   => DeserializeRow<T187Ap06.BdDto  >(row),
            XbrlTaxonomy.FH   => DeserializeRow<T187Ap06.FhDto  >(row),
            XbrlTaxonomy.INS  => DeserializeRow<T187Ap06.InsDto >(row),
            XbrlTaxonomy.MIM  => DeserializeRow<T187Ap06.MimDto >(row),
            _ => throw new NotSupportedException($"未知的 T187Ap06 分類: {context.Taxonomy.ToCode()}")
        };
    }

    /// <summary>
    /// T187Ap07 (資產負債表) 各業別的解析
    /// </summary>
    private static IT187Dto? MapToAp07Dto(EtlContext context, JsonElement row)
    {
        return context.Taxonomy switch
        {
            XbrlTaxonomy.CI   => DeserializeRow<T187Ap07.CiDto  >(row),
            XbrlTaxonomy.BASI => DeserializeRow<T187Ap07.BasiDto>(row),
            XbrlTaxonomy.BD   => DeserializeRow<T187Ap07.BdDto  >(row),
            XbrlTaxonomy.FH   => DeserializeRow<T187Ap07.FhDto  >(row),
            XbrlTaxonomy.INS  => DeserializeRow<T187Ap07.InsDto >(row),
            XbrlTaxonomy.MIM  => DeserializeRow<T187Ap07.MimDto >(row),
            _ => throw new NotSupportedException($"未知的 T187Ap07 分類: {context.Taxonomy.ToCode()}")
        };
    }
}
