using CorpInsightsTW.Etl.Dtos.T163Sb20;

namespace CorpInsightsTW.Etl.Repositories.T163Sb20;

public class CashFlowRepository(string connectionString, string taxonomy) : T187Repository<CashFlowDto>(connectionString)
{
    protected override string Taxonomy => taxonomy;

    protected override string MainTableUpsertSql => @"";
}