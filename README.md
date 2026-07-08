# CorpInsights-TW (台灣公司洞察)

`CorpInsights-TW` 是一個專為台灣上市櫃及公發公司財務報表設計的資料倉儲與大數據分析專案。本專案將公開資訊觀測站的財報 JSON 數據進行清洗、結構化，為後續的量化分析與公司洞察提供堅實的數據底座。

---

## 📁 專案目錄結構

```text
CorpInsights-TW/
├── config/                  # 專案設定檔 (如資料庫連線設定)
├── database/                # 資料庫相關的所有檔案
│   ├── schema/              # 存放全域固定表的 DDL
│   └── templates/           # 💡 存放動態建表的 DDL 模板
│       ├── t187ap06/        # 綜合損益
│       └── t187ap07/        # 資產負債
├── src/                     # 程式原始碼 (ETL、爬蟲、資料處理邏輯)
│   ├── crawlers/            # 負責抓取財報資料的腳本
│   └── processors/          # 負責解析 DDL 模板並寫入資料庫的核心邏輯
├── tests/                   # 單元測試與整合測試
└── README.md                # 專案說明文件 (本檔案)
```
