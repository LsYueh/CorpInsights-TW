using CommandLine;

namespace CorpInsightsTW.Etl;

public class CliOptions
{
    [Option('s', "status", Required = false, Default = "all",
        HelpText = "目標上市狀態: 'all' (全部), 'listed' (上市公司), 'publicoffering' (公開發行公司)")]
    public string Status { get; set; } = "all";

    [Option('t', "taxonomy", Required = false, Default = "all",
        HelpText = "目標申報分類法: 'all' (全部), 'general' (一般行業), 'banking' (金融業), 'securities' (證券期貨業), 'holding' (金控業), 'insurance' (保險業), 'crossindustry' (異業別合併)")]
    public string Taxonomy { get; set; } = "all";

    [Option('r', "report", Required = false, Default = "all",
        HelpText = "目標報表代號: 'all' (全部), 't187ap06' (綜合損益表), 't187ap07' (資產負債表)")]
    public string ApCode { get; set; } = "all";

    [Option('d', "date", Required = false, Default = null,
        HelpText = "指定匯入資料日期 (格式: yyyyMMdd)，預設為今日")]
    public string? Date { get; set; }
}