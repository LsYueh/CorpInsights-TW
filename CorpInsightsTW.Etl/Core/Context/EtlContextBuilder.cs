using CorpInsightsTW.Core.Enums;

namespace CorpInsightsTW.Etl.Core.Context;

public static class EtlContextBuilder
{
    /// <summary>
    /// 扁平化所有組合
    /// </summary>
    public static IEnumerable<EtlContext> BuildContexts(
        StockMarket market,
        IEnumerable<StatementType> reportList,
        IEnumerable<ListingStatus> statusList,
        IEnumerable<XbrlTaxonomy> taxonomyList,
        DateOnly targetDate)
    {
        var taxonomies = taxonomyList.DefaultIfEmpty().ToList();

        foreach (var type in reportList)
        {
            foreach (var status in statusList)
            {
                // 根據報表類型動態決定採用的 taxonomy 集合
                var currentTaxonomies = GetEffectiveTaxonomies(type, taxonomies);

                foreach (var taxonomy in currentTaxonomies)
                {
                    yield return new EtlContext(market, type, status, taxonomy, targetDate);
                }
            }
        }
    }

    private static IEnumerable<XbrlTaxonomy> GetEffectiveTaxonomies(StatementType type, List<XbrlTaxonomy> taxonomies)
    {
        // T163SB20 不看 taxonomy，只回傳預設/第一個
        if (type == StatementType.T163SB20)
        {
            return taxonomies.Take(1);
        }

        // Note: 未來有其他特例可以在這裡擴充...

        return taxonomies;
    }
}