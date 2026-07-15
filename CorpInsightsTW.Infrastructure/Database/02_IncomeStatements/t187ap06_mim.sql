CREATE TABLE IF NOT EXISTS t187ap06_mim (
    -- 1. 核心識別與索引欄位
    year         SMALLINT         NOT NULL COMMENT '年度 ( e.g., "2026" )',
    quarter      TINYINT UNSIGNED NOT NULL COMMENT '季別 ( 1: 第一季, 2: 第二季, 3: 第三季, 4: 第四季 )',
    market_type  CHAR(1)          NOT NULL COMMENT '市場註記 ( L: 上市公司, X: 公發公司 )',
    company_code VARCHAR(10)      NOT NULL COMMENT '公司代號',
    company_name VARCHAR(100)     NOT NULL COMMENT '公司名稱',
    
    -- 2. 異業最精簡之收支項目
    revenues DECIMAL(20, 2) DEFAULT 0.00 COMMENT '收入',
    expenses DECIMAL(20, 2) DEFAULT 0.00 COMMENT '支出',
    
    -- 3. 稅前與稅後淨利項目
    income_before_tax       DECIMAL(20, 2) DEFAULT 0.00 COMMENT '繼續營業單位稅前淨利（淨損）',
    income_tax              DECIMAL(20, 2) DEFAULT 0.00 COMMENT '所得稅費用（利益）',
    income_after_tax        DECIMAL(20, 2) DEFAULT 0.00 COMMENT '繼續營業單位本期淨利（淨損）',
    discontinued_ops_income DECIMAL(20, 2) DEFAULT 0.00 COMMENT '停業單位損益',
    net_income              DECIMAL(20, 2) DEFAULT 0.00 COMMENT '本期淨利（淨損）',
    
    -- 4. 其他綜合損益項目
    other_comprehensive_income DECIMAL(20, 2) DEFAULT 0.00 COMMENT '其他綜合損益',
    total_comprehensive_income DECIMAL(20, 2) DEFAULT 0.00 COMMENT '本期綜合損益總額',
    
    -- 5. 損益歸屬項目
    net_income_parent      DECIMAL(20, 2) DEFAULT 0.00 COMMENT '淨利（淨損）歸屬於母公司業主',
    net_income_predecessor DECIMAL(20, 2) DEFAULT 0.00 COMMENT '淨利（淨損）歸屬於共同控制下前手權益',
    net_income_nci         DECIMAL(20, 2) DEFAULT 0.00 COMMENT '淨利（淨損）歸屬於非控制權益',
    
    -- 6. 綜合損益總額歸屬項目
    comp_income_parent      DECIMAL(20, 2) DEFAULT 0.00 COMMENT '綜合損益總額歸屬於母公司業主',
    comp_income_predecessor DECIMAL(20, 2) DEFAULT 0.00 COMMENT '綜合損益總額歸屬於共同控制下前手權益',
    comp_income_nci         DECIMAL(20, 2) DEFAULT 0.00 COMMENT '綜合損益總額歸屬於非控制權益',
    
    -- 7. 每股盈餘項目
    basic_eps DECIMAL(10, 2) DEFAULT 0.00 COMMENT '基本每股盈餘（元）',
    
    -- 8. 系統稽核欄位
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '資料最後更新時間',
    
    -- 設定主鍵與索引
    PRIMARY KEY (company_code, year, quarter),
    INDEX idx_year_quarter (year, quarter)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='公發公司綜合損益表-異業';