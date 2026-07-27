using CorpInsightsTW.Etl.Dtos;
using Dapper;
using MySqlConnector;

namespace CorpInsightsTW.Etl.Repositories;

public abstract class T187Repository<TDto>(string connectionString) : IT187Repository<TDto> where TDto : T187Dto
{
    protected readonly string ConnectionString = connectionString;

    /// <summary>
    /// 子類別需提供主要報表的 Upsert SQL 指令
    /// </summary>
    protected abstract string MainTableUpsertSql { get; }

    /// <summary>
    /// 行業別分類 (e.g., "basi", "bd", "ci")
    /// </summary>
    protected abstract string Taxonomy { get; }

    public virtual async Task UpsertAsync(IEnumerable<TDto> dtos, CancellationToken cancellationToken = default)
    {
        var dtoList = dtos.ToList();
        if (dtoList.Count == 0) return;

        await using var conn = new MySqlConnection(ConnectionString);
        await conn.OpenAsync(cancellationToken);

        await using var transaction = await conn.BeginTransactionAsync(cancellationToken);

        try
        {
            await conn.ExecuteAsync(
                new CommandDefinition(MainTableUpsertSql, dtoList, transaction: transaction, cancellationToken: cancellationToken));

            await UpsertCompanyAsync(conn, transaction, dtoList, Taxonomy, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    /// <summary>
    /// 同 Transaction 內更新公司基本資料
    /// </summary>
    protected static async Task UpsertCompanyAsync(
        MySqlConnection conn,
        MySqlTransaction transaction,
        IEnumerable<T187Dto> dtos,
        string taxonomy,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(taxonomy))
        {
            throw new ArgumentException("Taxonomy 參數不可為空或空字串", nameof(taxonomy));
        }

        var cleanTaxonomy = taxonomy.Trim().ToLower();
        
        // 記憶體內去重 + 補上 Taxonomy 參數
        var mapItems = dtos
            .Where(x => !string.IsNullOrWhiteSpace(x.CompanyCode))
            .DistinctBy(x => x.CompanyCode)
            .Select(x => new
            {
                x.CompanyCode,
                x.CompanyName,
                x.ListingStatus,
                Taxonomy = cleanTaxonomy
            })
            .ToList();

        if (mapItems.Count == 0) return;

        const string mapSql = @"
            INSERT INTO companies (company_code, company_name, listing_status, xbrl_taxonomy)
            VALUES (@CompanyCode, @CompanyName, @ListingStatus, @Taxonomy)
            ON DUPLICATE KEY UPDATE
                company_name = VALUES(company_name),
                listing_status = VALUES(listing_status),
                xbrl_taxonomy = VALUES(xbrl_taxonomy),
                updated_at = NOW();";

        await conn.ExecuteAsync(
            new CommandDefinition(mapSql, mapItems, transaction: transaction, cancellationToken: cancellationToken));
    }
} 