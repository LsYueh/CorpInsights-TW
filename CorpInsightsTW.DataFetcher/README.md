# CorpInsightsTW.DbMigrator

台灣上市櫃公司財報 ETL 系統的資料庫維運與初始化工具。

## 說明

本工具為一次性執行的 CLI 工具，負責對目標資料庫執行 DDL 腳本，建立或重置所需的資料表與索引結構。

> ⚠️ **注意：本專案預設資料庫為 MySQL / MariaDB**，與解決方案中其他子專案的預設資料庫不同，請確認連線字串指向正確的資料庫引擎。

## 設定

`appsettings.json` / `appsettings.Development.json`：

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=corpinsightstw;User=root;Password=yourpassword;"
  },
  "MigrationSettings": {
    "DatabaseScriptsPath": "Scripts",
    "DropDatabaseFirst": false
  }
}
```

| 設定項目 | 說明 |
|---|---|
| `DefaultConnection` | MySQL / MariaDB 連線字串 |
| `DatabaseScriptsPath` | DDL 腳本根目錄路徑 |
| `DropDatabaseFirst` | `true` 會先摧毀並重建資料庫，**不可逆** |

## 腳本目錄結構

工具會依照目錄前綴數字順序執行：

```
Scripts/
├── 00_init/
├── 01_tables/
└── 02_indexes/
```

## 執行

```bash
# Development 環境
DOTNET_ENVIRONMENT=Development dotnet run

# Production 環境
dotnet run
```

## 相依套件

| 套件 | 來源 |
|---|---|
| `MySqlConnector` | 經由 `CorpInsightsTW.Infrastructure` 傳遞相依 |
| `Microsoft.Extensions.Hosting` | Generic Host / 組態載入 |
