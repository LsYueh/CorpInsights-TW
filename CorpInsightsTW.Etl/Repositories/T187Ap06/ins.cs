using Dapper;
using MySqlConnector;
using CorpInsightsTW.Etl.Dtos.T187Ap06;

namespace CorpInsightsTW.Etl.Repositories.T187Ap06;

public class InsRepository(string connectionString) : IRepository<InsDto>
{
    private readonly string _connectionString = connectionString;

    /// <summary>
    /// 批次寫入或更新綜合損益表-保險業 (t187ap06_ins)
    /// </summary>
    public async Task UpsertAsync(IEnumerable<InsDto> dtos, CancellationToken cancellationToken = default)
    {
        if (dtos == null || !dtos.Any()) return;

        const string sql = @"
            INSERT INTO t187ap06_ins (
                year,
                quarter,
                listing_status,
                company_code,
                company_name,
                operating_revenue,
                operating_costs,
                operating_expenses,
                operating_income,
                non_operating_income,
                income_before_tax,
                income_tax,
                income_after_tax,
                discontinued_ops_income,
                pre_merger_non_control_income,
                net_income,
                other_comprehensive_income,
                pre_merger_non_control_oci,
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
                @OperatingRevenue,
                @OperatingCosts,
                @OperatingExpenses,
                @OperatingIncome,
                @NonOperatingIncome,
                @IncomeBeforeTax,
                @IncomeTax,
                @IncomeAfterTax,
                @DiscontinuedOpsIncome,
                @PreMergerNonControlIncome,
                @NetIncome,
                @OtherComprehensiveIncome,
                @PreMergerNonControlOci,
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
                listing_status                = VALUES(listing_status),
                company_name                  = VALUES(company_name),
                operating_revenue             = VALUES(operating_revenue),
                operating_costs               = VALUES(operating_costs),
                operating_expenses            = VALUES(operating_expenses),
                operating_income              = VALUES(operating_income),
                non_operating_income          = VALUES(non_operating_income),
                income_before_tax             = VALUES(income_before_tax),
                income_tax                    = VALUES(income_tax),
                income_after_tax              = VALUES(income_after_tax),
                discontinued_ops_income       = VALUES(discontinued_ops_income),
                pre_merger_non_control_income = VALUES(pre_merger_non_control_income),
                net_income                    = VALUES(net_income),
                other_comprehensive_income    = VALUES(other_comprehensive_income),
                pre_merger_non_control_oci    = VALUES(pre_merger_non_control_oci),
                total_comprehensive_income    = VALUES(total_comprehensive_income),
                net_income_parent             = VALUES(net_income_parent),
                net_income_predecessor        = VALUES(net_income_predecessor),
                net_income_nci                = VALUES(net_income_nci),
                comp_income_parent            = VALUES(comp_income_parent),
                comp_income_predecessor       = VALUES(comp_income_predecessor),
                comp_income_nci               = VALUES(comp_income_nci),
                basic_eps                     = VALUES(basic_eps),
                updated_at                    = NOW();";

        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        foreach (var chunk in dtos.Chunk(1000))
        {
            var command = new CommandDefinition(sql, chunk, cancellationToken: cancellationToken);
            await connection.ExecuteAsync(command);
        }
    }
}