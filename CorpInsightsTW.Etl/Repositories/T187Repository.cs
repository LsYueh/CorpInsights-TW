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
    /// 子類別需提供專屬的 Taxonomy 名稱 (e.g., "basi", "bd", "ci")
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

            await UpsertTaxonomyMapAsync(conn, transaction, dtoList, Taxonomy, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    /// <summary>
    /// 靜態輔助方法：同 Transaction 內更新公司 Taxonomy 對應表
    /// </summary>
    protected static async Task UpsertTaxonomyMapAsync(
        MySqlConnection conn,
        MySqlTransaction transaction,
        IEnumerable<T187Dto> dtos,
        string taxonomy,
        CancellationToken cancellationToken)
    {
        // 記憶體內去重 + 補上 Taxonomy 參數
        var mapItems = dtos
            .Where(x => !string.IsNullOrWhiteSpace(x.CompanyCode))
            .DistinctBy(x => x.CompanyCode)
            .Select(x => new
            {
                x.CompanyCode,
                x.CompanyName,
                Taxonomy = taxonomy.ToLower()
            })
            .ToList();

        if (mapItems.Count == 0) return;

        const string mapSql = @"
            INSERT INTO company_taxonomy_map (company_code, company_name, xbrl_taxonomy)
            VALUES (@CompanyCode, @CompanyName, @Taxonomy)
            ON DUPLICATE KEY UPDATE
                company_name = VALUES(company_name),
                xbrl_taxonomy = VALUES(xbrl_taxonomy);";

        await conn.ExecuteAsync(
            new CommandDefinition(mapSql, mapItems, transaction: transaction, cancellationToken: cancellationToken));
    }
} 