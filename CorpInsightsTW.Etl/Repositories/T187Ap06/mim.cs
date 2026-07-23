using Dapper;
using MySqlConnector;
using CorpInsightsTW.Etl.Dtos.T187Ap06;

namespace CorpInsightsTW.Etl.Repositories.T187Ap06;

public class MimRepository(string connectionString) : IRepository<MimDto>
{
    private readonly string _connectionString = connectionString;

    /// <summary>
    /// 批次寫入或更新綜合損益表-異業 (t187ap06_mim)
    /// </summary>
    public async Task UpsertAsync(IEnumerable<MimDto> dtos, CancellationToken cancellationToken = default)
    {
        if (dtos == null || !dtos.Any()) return;

        const string sql = @"
            INSERT INTO t187ap06_mim (
                year,
                quarter,
                listing_status,
                company_code,
                company_name,
                revenues,
                expenses,
                income_before_tax,
                income_tax,
                income_after_tax,
                discontinued_ops_income,
                net_income,
                other_comprehensive_income,
                total_comprehensive_income,
                net_income_parent,
                net_income_predecessor,
                net_income_nci,
                comp_income_parent,
                comp_income_predecessor,
                comp_income_nci,
                basic_eps,
                updated_at
            ) VALUES (
                @Year,
                @Quarter,
                @ListingStatus,
                @CompanyCode,
                @CompanyName,
                @Revenues,
                @Expenses,
                @IncomeBeforeTax,
                @IncomeTax,
                @IncomeAfterTax,
                @DiscontinuedOpsIncome,
                @NetIncome,
                @OtherComprehensiveIncome,
                @TotalComprehensiveIncome,
                @NetIncomeParent,
                @NetIncomePredecessor,
                @NetIncomeNci,
                @CompIncomeParent,
                @CompIncomePredecessor,
                @CompIncomeNci,
                @BasicEps,
                NOW()
            )
            ON DUPLICATE KEY UPDATE
                listing_status             = VALUES(listing_status),
                company_name               = VALUES(company_name),
                revenues                   = VALUES(revenues),
                expenses                   = VALUES(expenses),
                income_before_tax          = VALUES(income_before_tax),
                income_tax                 = VALUES(income_tax),
                income_after_tax           = VALUES(income_after_tax),
                discontinued_ops_income    = VALUES(discontinued_ops_income),
                net_income                 = VALUES(net_income),
                other_comprehensive_income = VALUES(other_comprehensive_income),
                total_comprehensive_income = VALUES(total_comprehensive_income),
                net_income_parent          = VALUES(net_income_parent),
                net_income_predecessor     = VALUES(net_income_predecessor),
                net_income_nci             = VALUES(net_income_nci),
                comp_income_parent         = VALUES(comp_income_parent),
                comp_income_predecessor    = VALUES(comp_income_predecessor),
                comp_income_nci            = VALUES(comp_income_nci),
                basic_eps                  = VALUES(basic_eps),
                updated_at                 = NOW();";

        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        foreach (var chunk in dtos.Chunk(1000))
        {
            var command = new CommandDefinition(sql, chunk, cancellationToken: cancellationToken);
            await connection.ExecuteAsync(command);
        }
    }
}