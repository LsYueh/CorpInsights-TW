CREATE TABLE fact_reinvestment_quarterly (
    symbol VARCHAR(10) NOT NULL,          -- 股票代號 (例: '2330')
    year INT NOT NULL,                    -- 財報年份 (例: 2026)
    quarter INT NOT NULL,                 -- 財報季度 (1, 2, 3, 4)
    
    -- 【核心數據欄位】
    non_current_assets DECIMAL(20, 2),    -- 期末資產 (一般業：非流動資產 / 金融業：長投+固定資產)
    net_income_single DECIMAL(20, 2),     -- 當季單季稅後淨利 (已完成累計轉單季)
    
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (symbol, year, quarter)
);