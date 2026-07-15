CREATE TABLE IF NOT EXISTS t187ap06_fh (
    -- 1. 核心識別與索引欄位
    year         SMALLINT         NOT NULL COMMENT '年度 ( e.g., "2026" )',
    quarter      TINYINT UNSIGNED NOT NULL COMMENT '季別 ( 1: 第一季, 2: 第二季, 3: 第三季, 4: 第四季 )',
    market_type  CHAR(1)          NOT NULL COMMENT '市場註記 ( L: 上市公司, X: 公發公司 )',
    company_code VARCHAR(10)      NOT NULL COMMENT '公司代號',
    company_name VARCHAR(100)     NOT NULL COMMENT '公司名稱',
    
    -- 2. 金控業特有營業淨收益項目
    net_interest_income DECIMAL(20, 2) DEFAULT 0.00 COMMENT '利息淨收益',
    non_interest_income DECIMAL(20, 2) DEFAULT 0.00 COMMENT '利息以外淨收益',
    net_revenue         DECIMAL(20, 2) DEFAULT 0.00 COMMENT '淨收益',
    
    -- 3. 金控與保險特有成本準備與費用項目
    bad_debt_and_provision    DECIMAL(20, 2) DEFAULT 0.00 COMMENT '呆帳費用、承諾及保證責任準備提存',
    net_change_insurance_liab DECIMAL(20, 2) DEFAULT 0.00 COMMENT '保險負債準備淨變動',
    operating_expenses        DECIMAL(20, 2) DEFAULT 0.00 COMMENT '營業費用',
    
    -- 4. 稅前與稅後淨利項目
    income_before_tax       DECIMAL(20, 2) DEFAULT 0.00 COMMENT '繼續營業單位稅前損益',
    income_tax              DECIMAL(20, 2) DEFAULT 0.00 COMMENT '所得稅（費用）利益',
    income_after_tax        DECIMAL(20, 2) DEFAULT 0.00 COMMENT '繼續營業單位本期淨利（淨損）',
    discontinued_ops_income DECIMAL(20, 2) DEFAULT 0.00 COMMENT '停業單位損益',
    net_income              DECIMAL(20, 2) DEFAULT 0.00 COMMENT '本期稅後淨利（淨損）',
    
    -- 5. 其他綜合損益項目
    other_comprehensive_income DECIMAL(20, 2) DEFAULT 0.00 COMMENT '本期其他綜合損益（稅後淨額）',
    total_comprehensive_income DECIMAL(20, 2) DEFAULT 0.00 COMMENT '本期綜合損益總額',
    
    -- 6. 損益歸屬項目
    net_income_parent      DECIMAL(20, 2) DEFAULT 0.00 COMMENT '淨利（淨損）歸屬於母公司業主',
    net_income_predecessor DECIMAL(20, 2) DEFAULT 0.00 COMMENT '淨利（淨損）歸屬於共同控制下前手權益',
    net_income_nci         DECIMAL(20, 2) DEFAULT 0.00 COMMENT '淨利（淨損）歸屬於非控制權益',
    
    -- 7. 綜合損益總額歸屬項目
    comp_income_parent      DECIMAL(20, 2) DEFAULT 0.00 COMMENT '綜合損益總額歸屬於母公司業主',
    comp_income_predecessor DECIMAL(20, 2) DEFAULT 0.00 COMMENT '綜合損益總額歸屬於共同控制下前手權益',
    comp_income_nci         DECIMAL(20, 2) DEFAULT 0.00 COMMENT '綜合損益總額歸屬於非控制權益',
    
    -- 8. 每股盈餘項目
    basic_eps DECIMAL(10, 2) DEFAULT 0.00 COMMENT '基本每股盈餘（元）',
    
    -- 9. 系統稽核欄位
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '資料最後更新時間',
    
    -- 設定主鍵與索引
    PRIMARY KEY (company_code, year, quarter),
    INDEX idx_year_quarter (year, quarter)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='公發公司綜合損益表-金控業';