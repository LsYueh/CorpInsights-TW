CREATE TABLE IF NOT EXISTS t187ap06_ci (
    -- 1. 核心識別與索引欄位
    year         SMALLINT         NOT NULL COMMENT '年度 ( e.g., "2026" )',
    quarter      TINYINT UNSIGNED NOT NULL COMMENT '季別 ( 1: 第一季, 2: 第二季, 3: 第三季, 4: 第四季 )',
    market_type  CHAR(1)          NOT NULL COMMENT '市場註記 ( L: 上市公司, X: 公發公司 )',
    company_code VARCHAR(10)      NOT NULL COMMENT '公司代號',
    company_name VARCHAR(100)     NOT NULL COMMENT '公司名稱',
    
    -- 2. 營業收入、成本與毛利項目
    operating_revenue                     DECIMAL(20, 2) DEFAULT 0.00 COMMENT '營業收入',
    operating_costs                       DECIMAL(20, 2) DEFAULT 0.00 COMMENT '營業成本',
    gains_losses_on_initial_recognition_of_biological_assets   DECIMAL(20, 2) DEFAULT 0.00 COMMENT '原始認列生物資產及農產品之利益（損失）',
    gains_losses_on_changes_in_fair_value_of_biological_assets DECIMAL(20, 2) DEFAULT 0.00 COMMENT '生物資產當期公允價值減出售成本之變動利益（損失）',
    gross_profit_loss_from_operations     DECIMAL(20, 2) DEFAULT 0.00 COMMENT '營業毛利（毛損）',
    unrealized_gains_losses_on_sales      DECIMAL(20, 2) DEFAULT 0.00 COMMENT '未實現銷貨（損）益',
    realized_gains_losses_on_sales        DECIMAL(20, 2) DEFAULT 0.00 COMMENT '已實現銷貨（損）益',
    net_gross_profit_loss_from_operations DECIMAL(20, 2) DEFAULT 0.00 COMMENT '營業毛利（毛損）淨額',
    
    -- 3. 營業費用與營業利益
    operating_expenses                DECIMAL(20, 2) DEFAULT 0.00 COMMENT '營業費用',
    net_other_revenues_and_gains_expenses_and_losses DECIMAL(20, 2) DEFAULT 0.00 COMMENT '其他收益及費損淨額',
    operating_income_loss             DECIMAL(20, 2) DEFAULT 0.00 COMMENT '營業利益（損失）',
    non_operating_income_and_expenses DECIMAL(20, 2) DEFAULT 0.00 COMMENT '營業外收入及支出',
    
    -- 4. 稅前與稅後淨利項目
    net_income_before_tax                              DECIMAL(20, 2) DEFAULT 0.00 COMMENT '稅前淨利（淨損）',
    income_tax_expense_or_benefit                      DECIMAL(20, 2) DEFAULT 0.00 COMMENT '所得稅費用（利益）',
    net_income_from_continuing_operations              DECIMAL(20, 2) DEFAULT 0.00 COMMENT '繼續營業單位本期淨利（淨損）',
    discontinued_operations_income_or_loss             DECIMAL(20, 2) DEFAULT 0.00 COMMENT '停業單位損益',
    pre_merger_non_joint_control_equity_income_or_loss DECIMAL(20, 2) DEFAULT 0.00 COMMENT '合併前非屬共同控制股權損益',
    net_income_or_loss                                 DECIMAL(20, 2) DEFAULT 0.00 COMMENT '本期淨利（淨損）',
    
    -- 5. 其他綜合損益項目
    other_comprehensive_income_net_after_tax DECIMAL(20, 2) DEFAULT 0.00 COMMENT '其他綜合損益（淨額）',
    pre_merger_non_joint_control_equity_other_comprehensive_income_net DECIMAL(20, 2) DEFAULT 0.00 COMMENT '合併前非屬共同控制股權綜合損益淨額',
    total_comprehensive_income               DECIMAL(20, 2) DEFAULT 0.00 COMMENT '本期綜合損益總額',
    
    -- 6. 損益歸屬項目
    net_income_attributable_to_owners_of_parent          DECIMAL(20, 2) DEFAULT 0.00 COMMENT '淨利（淨損）歸屬於母公司業主',
    net_income_attributable_to_predecessor_interests     DECIMAL(20, 2) DEFAULT 0.00 COMMENT '淨利（淨損）歸屬於共同控制下前手權益',
    net_income_attributable_to_non_controlling_interests DECIMAL(20, 2) DEFAULT 0.00 COMMENT '淨利（淨損）歸屬於非控制權益',
    
    -- 7. 綜合損益總額歸屬項目
    total_comprehensive_income_attributable_to_owners_of_parent          DECIMAL(20, 2) DEFAULT 0.00 COMMENT '綜合損益總額歸屬於母公司業主',
    total_comprehensive_income_attributable_to_predecessor_interests     DECIMAL(20, 2) DEFAULT 0.00 COMMENT '綜合損益總額歸屬於共同控制下前手權益',
    total_comprehensive_income_attributable_to_non_controlling_interests DECIMAL(20, 2) DEFAULT 0.00 COMMENT '綜合損益總額歸屬於非控制權益',
    
    -- 8. 每股盈餘項目
    basic_earnings_per_share DECIMAL(10, 2) DEFAULT 0.00 COMMENT '基本每股盈餘（元）',
    
    -- 9. 系統稽核欄位
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '資料最後更新時間',
    
    -- 設定主鍵與索引
    PRIMARY KEY (company_code, year, quarter),
    INDEX idx_year_quarter (year, quarter)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='公發公司綜合損益表-一般業';