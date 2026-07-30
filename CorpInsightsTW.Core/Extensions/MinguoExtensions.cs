using System.Globalization;

namespace CorpInsightsTW.Core.Extensions;

public static class MinguoExtensions
{
    private static readonly Calendar TaiwanCal = new TaiwanCalendar();

    /// <summary>
    /// 將 DateOnly (Gregorian) 轉為民國年 (Minguo) 格式字串
    /// </summary>
    /// <param name="date">西元 DateOnly</param>
    /// <param name="format">
    /// 支援格式：
    /// "yyy/MM/dd" -> "115/07/30"
    /// "yyy年MM月dd日" -> "115年07月30日"
    /// "yyyMMdd" -> "1150730"
    /// "民國yyy年MM月dd日" -> "民國115年07月30日"
    /// </param>
    public static string ToMinguoDateString(this DateOnly date, string format = "yyy/MM/dd")
    {
        // 抓出民國年數字（例如 2024 - 1911 = 113）
        int taiwanYear = TaiwanCal.GetYear(date.ToDateTime(TimeOnly.MinValue));

        // 替換 yyy / yy 自訂格式
        string result = format
            .Replace("yyy", taiwanYear.ToString("D3")) // 補滿三碼，例如 099 年
            .Replace("yy", taiwanYear.ToString());

        // 剩餘的 MM, dd 等 standard datetime 格式交由 ToString 處理
        return date.ToString(result, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 將民國年字串解析為西元 DateOnly (Gregorian)
    /// </summary>
    /// <param name="minguoDateStr">例如 "115/07/30" 或 "1150730"</param>
    /// <param name="format">對應格式，例如 "yyy/MM/dd" 或 "yyyMMdd"</param>
    /// <param name="result">解析成功時傳出西元 DateOnly</param>
    public static bool TryParseMinguoDate(this string minguoDateStr, string format, out DateOnly result)
    {
        result = default;
        if (string.IsNullOrWhiteSpace(minguoDateStr)) return false;

        try
        {
            // 處理「民國」字樣前綴
            minguoDateStr = minguoDateStr.Replace("民國", "").Trim();
            format = format.Replace("民國", "").Trim();

            // 拆解 yyy 取得長度並替換成西元年計算
            int yearIndex = format.IndexOf("yyy");
            if (yearIndex == -1) return false;

            // 假設民國年份固定為 2~3 位數，擷取民國年並加 1911
            // 這裡採用簡易安全的邏輯：依據格式切出民國年
            string yearStr = minguoDateStr.Substring(yearIndex, 3);
            if (!int.TryParse(yearStr, out int taiwanYear)) return false;

            int westernYear = taiwanYear + 1911;

            // 將原字串中的民國年替換成西元年後，用標準 DateTime 解析
            string westernDateStr = minguoDateStr.Remove(yearIndex, 3).Insert(yearIndex, westernYear.ToString("D4"));
            string westernFormat = format.Replace("yyy", "yyyy");

            if (DateTime.TryParseExact(westernDateStr, westernFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDt))
            {
                result = DateOnly.FromDateTime(parsedDt);
                return true;
            }
        }
        catch
        {
            return false;
        }

        return false;
    }

    /// <summary>
    /// 取得 DateOnly 對應的民國年整數 (例如 2026 傳回 115)
    /// </summary>
    public static int GetMinguoYear(this DateOnly date)
    {
        return TaiwanCal.GetYear(date.ToDateTime(TimeOnly.MinValue));
    }

    /// <summary>
    /// 取得 DateOnly 對應的民國年字串 (可選擇是否補零，例如 "115" 或 "099")
    /// </summary>
    public static string GetMinguoYearString(this DateOnly date, bool padLeft = false)
    {
        int year = date.GetMinguoYear();
        return padLeft ? year.ToString("D3") : year.ToString();
    }
}