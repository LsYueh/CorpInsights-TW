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
| 套件 | 說明 |
|---|---|
| `AngleSharp` | 由 `CorpInsightsTW.Etl` 的 `HtmlDataExtractor` 來使用 |
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

<br><br>

# `現金流量表`
資料來源：`MOPS 公開資訊觀測站 (HTML)`

##

請先至**公開資訊觀測站**下載[**現金流量表**](https://mops.twse.com.tw/mops/#/web/t163sb20)，並按照以下結構存放：
```text
(DATA_ROOT)
├── yyyyMMdd/                 # 執行日期 (如：20260729)
│   ├── tpex/                 # 櫃買中心
│   │   ├── t163sb20_O.htm    # 上櫃
│   │   └── t163sb20_U.htm    # 興櫃
│   └── twse/                 # 證券交易所
│       ├── t163sb20_L.htm    # 上市
│       └── t163sb20_X.htm    # 公發
└── ...
```

<br>

> ⚠️ **資料匯入操作注意事項**
> 
> * **自動壓碼機制**：系統執行資料匯入時，會**自動根據當下執行日期（`context.Date`）** 帶入對應的「年度」與「季別」。
> * **歷史補單/重跑注意**：若是**補跑歷史資料**或**跨期補抓**，請特別留意傳入的日期參數，避免匯入的資料被壓成錯誤的當期季度。

<details>
<summary><b>💡 上市櫃財報申報截止時間對照</b></summary>

<br>

| 執行日期區間 | 目前申報狀態 | 最新確定可取得之財報期別 |
| :--- | :--- | :--- |
| **01/01 ~ 03/31** | 前一年度 Q4 申報中（未全數出爐） | **前一年 Q3** |
| **04/01 ~ 05/15** | 前一年度 Q4 申報完畢 (截止日 03/31) | **前一年 Q4** |
| **05/16 ~ 08/14** | 當年度 Q1 申報完畢 (截止日 05/15) | **當年度 Q1** |
| **08/15 ~ 11/14** | 當年度 Q2 申報完畢 (截止日 08/14) | **當年度 Q2** |
| **11/15 ~ 12/31** | 當年度 Q3 申報完畢 (截止日 11/14) | **當年度 Q3** |

</details>

<details>
<summary><b>💡 字尾代碼對照</b></summary>

<br>

| 字尾 | 對應市場 |
| --- | --- |
| `_L` | **上市** |
| `_O` | **上櫃** |
| `_U` | **興櫃** |
| `_X` | **公發** |

</details>

<br>

---

<br><br>

# ⚙️ 資料匯入 (ETL)

![ETL](/docs/ETL.png)  

## 🚀 快速執行 (Quick Start)

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

