# CorpInsights-TW (台灣公司洞察)

`CorpInsights-TW` 是一個專為台灣上市櫃及公發公司財務報表設計的資料倉儲與大數據分析專案。本專案將公開資訊觀測站的財報 JSON 數據進行清洗、結構化，為後續的量化分析與公司洞察提供堅實的數據底座。

<br>

```
依`證券交易法第36條`及證券期貨局相關函令規定，財務報告申報期限如下：
  1.一般行業申報期限：第一季為5月15日，第二季為8月14日，第三季為11月14日，年度為3月31日。
  2.金控業申報期限：第一季為5月30日，第二季為8月31日，第三季為11月29日，年度為3月31日。
  3.銀行及票券業申報期限：第一季為5月15日，第二季為8月31日，第三季為11月14日，年度為3月31日。
  4.保險業申報期限：第一季為5月15日，第二季為8月31日，第三季為11月14日，年度為3月31日。
  5.證券業申報期限：第一季為5月15日，第二季為8月31日，第三季為11月14日，年度為3月31日。
  6.申報期限如遇例假日，以證券期貨局公布者為準。
```

<br>

<details>
    <summary>專案內容</summary>

## 📁 專案目錄結構
```text
CorpInsightsTW/                             # 專案總根目錄
├── CorpInsightsTW.Core/
│   ├── Database/                           # DDL 腳本區
│   ├── Storage/                            # 實體資料管理
│   └── ... 
├── CorpInsightsTW.DataFetcher/             # 財報資料抓取工具
├── CorpInsightsTW.DbMigrator/              # 資料庫初始化/維運專用微型工具
├── CorpInsightsTW.Etl/                     # ETL
│   ├── Core/
│   ├── Dtos/                               # DTOs
│   ├── Pipeline/
│   │   ├── Extract/
│   │   ├── Transform/
│   │   ├── Load/
│   │   └── EtlPipeline.cs                  # 串接 Extract → Transform → Load
│   └── Repository/                         # 儲藏區
├── CorpInsightsTW.Tests
├── docker/
│   └── mariadb/                            # 資料庫服務
└── CorpInsightsTW.slnx                     # .NET 10 方案核心管理檔
```

## ⚠️ 相依套件
| 套件 | 來源 |
|---|---|
| `MySqlConnector` | 經由 `CorpInsightsTW.Core` 傳遞相依 |

</details>

<br>

---

<br><br>

# `綜合損益表` / `資產負債表`
資料來源：`臺灣證券交易所 (OpenAPI)` 與 `證券櫃檯買賣中心 (OpenAPI)` 。  

## 📡 資料請求 (DataFetcher)  

![Data Fetcher](/docs/DataFetcher.png)  

### 🚀 快速執行 (Quick Start)

預設執行將自動抓取**今日全市場（TWSE/TPEX）所有財務報表**：

```bash
dotnet run --project CorpInsightsTW.DataFetcher
```

<br>

📖 完整 CLI 選項與產業別代碼請參閱：[DataFetcher 參數說明文件](/docs/DataFetcher/README.md)

---

<br>

## ⚙️ 資料匯入 (ETL)

![ETL](/docs/ETL.png)  

### 🚀 快速執行 (Quick Start)

> ⚠️ 使用前請先確定資料庫服務已啟動且資料表皆建立完畢。 ([資料庫服務操作文件](/docs/docker/mariadb.md))

預設執行將自動轉換**今日全市場（TWSE/TPEX）所有財務報表**：

```bash
dotnet run --project CorpInsightsTW.Etl -- --dry
```

<br>

📖 完整 CLI 選項與產業別代碼請參閱：[ETL 參數說明文件](/docs/Etl/README.md)

<br>

<details>
    <summary>🛠️ 客製化 T187JsonConverter</summary>

    解決 `證交所 OpenAPI` 公開資料內跨業別或跨版本`欄位別名`不一致的問題  
    ![ETL](/docs/JsonPropertyNames.png)  

    交易所的公開資料在格式上常有一些資料品質痛點：  
    1. 跨業別命名不一致：`上市公司`使用 "避險之金融資產－淨額"，`公發公司`卻使用 "避險之衍生金融資產淨額"。  
    2. 新舊版本 API 變更：舊版 API 給 "公司代號"，新版 API 無預警改為 "公司代碼"。  

    <br>

    為了在不破壞既有 DTO 結構、不降低反序列化效能的前提下，讓同一 C# 的JSON屬性方便支援多個中文 Key，同時具備嚴謹的「欄位缺失防守（至少要出現其中一個別名）」機制。

    <br>

    可與 `JsonPropertyName` 混用 
    ``` csharp
    [JsonPropertyName("資產總額")]
    [JsonPropertyNames("資產總計", "資產總額", "TotalAssets")]
    public decimal TotalAssets { get; set; }
    ```
    Converter 會自動去重複，並產生單一別名陣列：`["資產總計", "資產總額", "TotalAssets"]`。  

</details>

<br>

---

<br><br>

# `現金流量表`
資料來源：`MOPS 公開資訊觀測站 (HTML)`

