# DataFetcher - CLI 參數使用說明

本工具提供命令列介面（CLI）排程或手動操作時帶入篩選條件。所有參數皆為選填，未指定時預設值皆為 `all`（全選）。

## 📋 參數總覽

| 短命令 | 長命令 | 說明 | 預設值 | 可選值 |
| --- | --- | --- | :---: | --- |
| `-m` | `--market` | **市場別** | `all` | `all`, `twse`, `tpex` |
| `-s` | `--status` | **上市/櫃狀態** | `all` | `all`, `L`, `X`, `O`, `U` |
| `-t` | `--taxonomy` | **XBRL 申報分類** | `all` | `all`, `ci`, `basi`, `bd`, `fh`, `ins`, `mim` |
| `-r` | `--report` | **財務報表代號** | `all` | `all`, `t187ap06`, `t187ap07` |

---

<br>

## 🔍 參數詳細選項說明

### 1. 市場別 (`-m`, `--market`)

指定抓取的目標證券交易所市場：

* `all`：（預設）包含所有市場 (`twse` + `tpex`)
* `twse`：臺灣證券交易所 (上市)
* `tpex`：證券櫃檯買賣中心 (上櫃)

---

### 2. 上市/櫃狀態 (`-s`, `--status`)

篩選目標公司的發行狀態代碼：

* `all`：（預設）包含所有上市狀態
* `L`：上市 (Listed)
* `X`：公開發行 (Public)
* `O`：上櫃 (OTC)
* `U`：興櫃 (Emerging)

---

### 3. 申報分類法 (`-t`, `--taxonomy`)

指定產業別 XBRL 申報代號：

* `all`：（預設）所有產業分類
* `ci`：一般行業 (Company Industry)
* `basi`：金融業 (Banking & Financial Services)
* `bd`：證券期貨業 (Brokerage & Dealers)
* `fh`：金融控股業 (Financial Holding)
* `ins`：保險業 (Insurance)
* `mim`：異業別合併 (Multiple Industry / Mixed)

---

### 4. 財務報表代號 (`-r`, `--report`)

指定要抓取/處理的財務報表項目：

* `all`：（預設）所有支援的報表
* `t187ap06`：綜合損益表 (Statement of Comprehensive Income)
* `t187ap07`：資產負債表 (Balance Sheet)

---

<br>

## 💡 常見使用範例 (Examples)

### 1. 完全預設（抓取全市場所有報表與產業）

```bash
dotnet run

```

### 2. 僅抓取「上市 (TWSE)」的「綜合損益表 (t187ap06)」

```bash
dotnet run -- -m twse -r t187ap06

```

### 3. 僅抓取「上櫃 (TPEX)」且屬於「一般行業 (ci)」的「資產負債表 (t187ap07)」

```bash
dotnet run -- -m tpex -t ci -r t187ap07

```

### 4. 僅針對「上市 (L)」類別的「金控業 (fh)」執行

```bash
dotnet run -- -s L -t fh

```
