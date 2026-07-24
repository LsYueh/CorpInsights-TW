using CommandLine;

namespace CorpInsightsTW.DataFetcher;

public class CliOptions
{
    [Option('m', "market", Required = false, Default = "twse", 
        HelpText = "市場: twse: 上市, tpex: 上櫃")]
    public string Market { get; set; } = "tse";

    [Option('s', "status", Required = false, Default = "all", 
        HelpText = "上市狀態: 'all' (全部), L: 上市, X: 公發, O: 上櫃, U: 興櫃")]
    public string Status { get; set; } = "all";

    [Option('t', "taxonomy", Required = false, Default = "all", 
        HelpText = "申報分類: 'all' (全部), 'general' (一般行業), 'banking' (金融業), 'securities' (證券期貨業), 'holding' (金控業), 'insurance' (保險業), 'crossindustry' (異業別合併)")]
    public string Taxonomy { get; set; } = "all";

    [Option('r', "report", Required = false, Default = "all", 
        HelpText = "報表代號: 'all' (全部), 't187ap06' (綜合損益表), 't187ap07' (資產負債表)")]
    public string ApCode { get; set; } = "all";
}