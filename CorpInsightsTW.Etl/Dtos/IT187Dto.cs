namespace CorpInsightsTW.Etl.Dtos;

public interface IT187Dto
{
    /// <summary>
    /// 掛牌狀態: 'L' (上市公司), 'X' (公開發行公司)
    /// </summary>
    string ListingStatus { get; set; }

    /// <summary>
    /// 檢查此 DTO 的 Key 欄位是否合法有效
    /// </summary>
    bool IsValidKey();
}
