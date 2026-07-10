# CorpInsights-TW (台灣公司洞察)

`CorpInsights-TW` 是一個專為台灣上市櫃及公發公司財務報表設計的資料倉儲與大數據分析專案。本專案將公開資訊觀測站的財報 JSON 數據進行清洗、結構化，為後續的量化分析與公司洞察提供堅實的數據底座。

---

## 📁 專案目錄結構 (暫)

```text
CorpInsightsTW/                             # 專案總根目錄
├── .gitignore
├── CorpInsightsTW.slnx                     # .NET 10 方案核心管理檔
│
├── 1. CorpInsightsTW.Core/                 # 🛡️ 核心領域層 (純 C# 邏輯，不依賴任何外部套件)
│   ├── Enums/
│   │   └── IndustryType.cs                 # 定義 ci, basi, bd, fh, ins, mim 的強型態列舉
│   └── Entities/                           # 📊 資料庫實體模型 (對應我們的 1 + 6 + 6 架構)
│       ├── CompanyIndustryMap.cs           # 路由主表實體
│       ├── BalanceSheets/                  # 資產負債表實體區
│       │   ├── FinancialStatementBase.cs   # 抽象基底類別 (內含 company_code, year, quarter, market_type)
│       │   ├── ComponentGeneralIndustry.cs # t187ap07_ci (一般業)
│       │   └── ComponentFinancialHolding.cs# t187ap07_fh (金控業，其餘行業別依此類推)
│       └── IncomeStatements/               # 綜合損益表實體區
│           ├── ComponentGeneralIndustryIncome.cs # t187ap06_ci
│           └── ...
│
├── 2. CorpInsightsTW.Infrastructure/       # ⚙️ 基礎建設層 (負責與外部 OpenAPI 和資料庫溝通)
│   ├── Database/                           # 💾 依 Schema Templates 物理切分的 DDL 腳本區
│   │   ├── 00_Master_Data/                 # 主資料
│   │   ├── 01_BalanceSheets/               # 📑 範本一：資產負債表 DDL 區 (t187ap07)
│   │   └── 02_IncomeStatements/            # 📊 範本二：綜合損益表 DDL 區 (t186ap06)
│   └── ... (Data, OpenApiClients, Mappers)
│
├── 3. CorpInsightsTW.DbMigrator/           # 🔨 資料庫初始化/維運專用微型工具
│
└── 4. CorpInsightsTW.Tests (🟢 使用 MS Test 框架)
    ├── Mappers/
    │   └── FinancialDataMapperTests.cs     # 測試 OpenAPI 轉大寬表
    └── Routing/
        └── IndustryRouterTests.cs          # 測試行業別路由
```
