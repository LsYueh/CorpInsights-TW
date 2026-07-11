# CorpInsights-TW (台灣公司洞察)

`CorpInsights-TW` 是一個專為台灣上市櫃及公發公司財務報表設計的資料倉儲與大數據分析專案。本專案將公開資訊觀測站的財報 JSON 數據進行清洗、結構化，為後續的量化分析與公司洞察提供堅實的數據底座。

---

## 📁 專案目錄結構 (暫)

```text
CorpInsightsTW/                             # 專案總根目錄
├── CorpInsightsTW.Core/                    # 🛡️ 核心領域層
├── CorpInsightsTW.DataFetcher/             # 🔨 財報資料抓取工具
├── CorpInsightsTW.DbMigrator/              # 🔨 資料庫初始化/維運專用微型工具
├── CorpInsightsTW.Infrastructure/          # ⚙️ 基礎建設層
│   ├── Database/                           # 💾 DDL 腳本區
│   │   ├── 00_Master_Data/                 # 💾 主資料
│   │   ├── 01_BalanceSheets/               # 📑 範本一：資產負債表 DDL 區 (t187ap07)
│   │   └── 02_IncomeStatements/            # 📊 範本二：綜合損益表 DDL 區 (t186ap06)
│   └── ... (Data, OpenApiClients, Mappers)
├── CorpInsightsTW.Tests
└── CorpInsightsTW.slnx                     # .NET 10 方案核心管理檔
```
