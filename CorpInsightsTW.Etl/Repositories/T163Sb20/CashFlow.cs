using CorpInsightsTW.Etl.Dtos.T163Sb20;

namespace CorpInsightsTW.Etl.Repositories.T163Sb20;

public class CashFlowRepository(string connectionString) : HtmlStatementRepository<CashFlowDto>(connectionString)
{
    protected override string MainTableUpsertSql => @"
        INSERT INTO `t163sb20` (
            `company_code`,
            `year`,
            `quarter`,
            `listing_status`,
            `company_name`,
            `operating_cash_flows`,
            `investing_cash_flows`,
            `financing_cash_flows`,
            `fx_effect`,
            `net_change_in_cash`,
            `beginning_cash_balance`,
            `ending_cash_balance`
        ) VALUES (
            @CompanyCode,
            @Year,
            @Quarter,
            @ListingStatus,
            @CompanyName,
            @OperatingCashFlows,
            @InvestingCashFlows,
            @FinancingCashFlows,
            @FxEffect,
            @NetChangeInCash,
            @BeginningCashBalance,
            @EndingCashBalance
        )
        ON DUPLICATE KEY UPDATE
            `listing_status`         = VALUES(`listing_status`),
            `company_name`           = VALUES(`company_name`),
            `operating_cash_flows`   = VALUES(`operating_cash_flows`),
            `investing_cash_flows`   = VALUES(`investing_cash_flows`),
            `financing_cash_flows`   = VALUES(`financing_cash_flows`),
            `fx_effect`              = VALUES(`fx_effect`),
            `net_change_in_cash`     = VALUES(`net_change_in_cash`),
            `beginning_cash_balance` = VALUES(`beginning_cash_balance`),
            `ending_cash_balance`    = VALUES(`ending_cash_balance`);"; // TODO: ...
}