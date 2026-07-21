using System.Text.Json;
using System.Text.Json.Serialization;

using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.Core.Extensions;
using CorpInsightsTW.Etl.Core.Common;
using CorpInsightsTW.Etl.Core.Json;

namespace CorpInsightsTW.Etl.Dtos;

/// <summary>
/// 統一的 DTO 介面，用於 Transformer 與 Loader 之間的管線傳遞
/// </summary>
public interface IT187RawDto
{
    // ====== ETL 自行加工欄位 (反序列化時會忽略) ======
    // 💡 允許後續手動賦值 (set)，且給予預設空字串避免 Null 異常
    string MarketType { get; set; }
}

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
    /// 根據 EtlContext 將單一 JsonElement 解析為正確的強型別 DTO
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static IT187RawDto? MapToStrongTypeDto(EtlContext context, JsonElement row)
    {
        return context.ApCode switch
        {
            T187ApCode.T187AP06 => MapToAp06Dto(context, row),
            T187ApCode.T187AP07 => MapToAp07Dto(context, row),
            _ => throw new NotSupportedException($"未知的財務報表: {context.ApCode}")
        };
    }

    /// <summary>
    /// T187Ap06 (綜合損益表) 各業別的解析
    /// </summary>
    private static IT187RawDto? MapToAp06Dto(EtlContext context, JsonElement row)
    {
        return context.Taxonomy switch
        {
            XbrlTaxonomy.CI   => row.Deserialize<T187Ap06.CiDto  >(_jsonOptions),
            XbrlTaxonomy.BASI => row.Deserialize<T187Ap06.BasiDto>(_jsonOptions),
            XbrlTaxonomy.BD   => row.Deserialize<T187Ap06.BdDto  >(_jsonOptions),
            XbrlTaxonomy.FH   => row.Deserialize<T187Ap06.FhDto  >(_jsonOptions),
            XbrlTaxonomy.INS  => row.Deserialize<T187Ap06.InsDto >(_jsonOptions),
            XbrlTaxonomy.MIM  => row.Deserialize<T187Ap06.MimDto >(_jsonOptions),
            _ => throw new NotSupportedException($"未知的 T187Ap06 分類: {context.Taxonomy.ToCode()}")
        };

        throw new NotImplementedException("T187AP06 損益表 DTO 轉換尚未實作。");
    }

    /// <summary>
    /// T187Ap07 (資產負債表) 各業別的解析
    /// </summary>
    private static IT187RawDto? MapToAp07Dto(EtlContext context, JsonElement row)
    {
        return context.Taxonomy switch
        {
            XbrlTaxonomy.CI   => row.Deserialize<T187Ap07.CiDto  >(_jsonOptions),
            XbrlTaxonomy.BASI => row.Deserialize<T187Ap07.BasiDto>(_jsonOptions),
            XbrlTaxonomy.BD   => row.Deserialize<T187Ap07.BdDto  >(_jsonOptions),
            XbrlTaxonomy.FH   => row.Deserialize<T187Ap07.FhDto  >(_jsonOptions),
            XbrlTaxonomy.INS  => row.Deserialize<T187Ap07.InsDto >(_jsonOptions),
            XbrlTaxonomy.MIM  => row.Deserialize<T187Ap07.MimDto >(_jsonOptions),
            _ => throw new NotSupportedException($"未知的 T187Ap07 分類: {context.Taxonomy.ToCode()}")
        };
    }
}
