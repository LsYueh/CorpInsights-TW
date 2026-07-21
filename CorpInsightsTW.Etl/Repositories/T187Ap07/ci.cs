using Dapper;
using MySqlConnector;
using CorpInsightsTW.Etl.Dtos.T187Ap07;

namespace CorpInsightsTW.Etl.Repositories.T187Ap07;

public class CiRepository(string connectionString)
{
    private readonly string _connectionString = connectionString;

    /// <summary>
    /// 批次寫入或更新資產負債表-一般業 (t187ap07_ci)
    /// </summary>
    public async Task UpsertAsync(IEnumerable<CiDto> dtos, CancellationToken cancellationToken = default)
    {
        if (dtos == null || !dtos.Any()) return;

        const string sql = @"
            INSERT INTO t187ap07_ci (
                year,
                quarter,
                listing_status,
                company_code,
                company_name,
                current_assets,
                non_current_assets,
                total_assets,
                current_liabs,
                non_current_liabs,
                total_liabs,
                share_capital,
                security_token_equity,
                capital_surplus,
                retained_earnings,
                other_equity,
                treasury_stock,
                equity_attributable_to_owners_of_parent,
                predecessor_interests,
                equity_not_under_common_control,
                non_controlling_interests,
                total_equity,
                pending_cancellation_shares,
                pre_received_shares_equivalent,
                parent_subsidiary_treasury_shares,
                net_value_per_share,
                updated_at
            ) VALUES (
                @Year,
                @Quarter,
                @ListingStatus,
                @CompanyCode,
                @CompanyName,
                @CurrentAssets,
                @NonCurrentAssets,
                @TotalAssets,
                @CurrentLiabs,
                @NonCurrentLiabs,
                @TotalLiabs,
                @ShareCapital,
                @SecurityTokenEquity,
                @CapitalSurplus,
                @RetainedEarnings,
                @OtherEquity,
                @TreasuryStock,
                @EquityAttributableToOwnersOfParent,
                @PredecessorInterests,
                @EquityNotUnderCommonControl,
                @NonControllingInterests,
                @TotalEquity,
                @PendingCancellationShares,
                @PreReceivedSharesEquivalent,
                @ParentSubsidiaryTreasuryShares,
                @NetValuePerShare,
                NOW()
            )
            ON DUPLICATE KEY UPDATE
                listing_status                          = VALUES(listing_status),
                company_name                            = VALUES(company_name),
                current_assets                          = VALUES(current_assets),
                non_current_assets                      = VALUES(non_current_assets),
                total_assets                            = VALUES(total_assets),
                current_liabs                           = VALUES(current_liabs),
                non_current_liabs                       = VALUES(non_current_liabs),
                total_liabs                             = VALUES(total_liabs),
                share_capital                           = VALUES(share_capital),
                security_token_equity                   = VALUES(security_token_equity),
                capital_surplus                         = VALUES(capital_surplus),
                retained_earnings                       = VALUES(retained_earnings),
                other_equity                            = VALUES(other_equity),
                treasury_stock                          = VALUES(treasury_stock),
                equity_attributable_to_owners_of_parent = VALUES(equity_attributable_to_owners_of_parent),
                predecessor_interests                   = VALUES(predecessor_interests),
                equity_not_under_common_control         = VALUES(equity_not_under_common_control),
                non_controlling_interests               = VALUES(non_controlling_interests),
                total_equity                            = VALUES(total_equity),
                pending_cancellation_shares             = VALUES(pending_cancellation_shares),
                pre_received_shares_equivalent          = VALUES(pre_received_shares_equivalent),
                parent_subsidiary_treasury_shares       = VALUES(parent_subsidiary_treasury_shares),
                net_value_per_share                     = VALUES(net_value_per_share),
                updated_at                              = NOW();";

        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        foreach (var chunk in dtos.Chunk(1000))
        {
            var command = new CommandDefinition(sql, chunk, cancellationToken: cancellationToken);
            await connection.ExecuteAsync(command);
        }
    }
}