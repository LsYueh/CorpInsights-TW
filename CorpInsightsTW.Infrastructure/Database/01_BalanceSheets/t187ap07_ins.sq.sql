CREATE TABLE IF NOT EXISTS t187ap07_ins (
    -- 1. 核心識別與索引欄位 (完美保留原始相對順序)
    year         SMALLINT         NOT NULL COMMENT '年度 ( e.g., "2026" )',
    quarter      TINYINT UNSIGNED NOT NULL COMMENT '季別 ( 1: 第一季, 2: 第二季, 3: 第三季, 4: 第四季 )',
    market_type  CHAR(1)          NOT NULL COMMENT '市場註記 ( L: 上市公司, X: 公發公司 )',
    company_code VARCHAR(10)      NOT NULL COMMENT '公司代號',
    company_name VARCHAR(100)     NOT NULL COMMENT '公司名稱',
    
    -- 2. 保險業資產類項目
    cash_and_cash_equivalents   DECIMAL(20, 2) DEFAULT 0.00 COMMENT '現金及約當現金',
    receivables                 DECIMAL(20, 2) DEFAULT 0.00 COMMENT '應收款項',
    current_tax_assets          DECIMAL(20, 2) DEFAULT 0.00 COMMENT '本期所得稅資產',
    assets_held_for_sale        DECIMAL(20, 2) DEFAULT 0.00 COMMENT '待出售資產',
    assets_for_distribution     DECIMAL(20, 2) DEFAULT 0.00 COMMENT '待分配予業主之資產 (處分群組)',
    investments                 DECIMAL(20, 2) DEFAULT 0.00 COMMENT '投資',
    reinsurance_contract_assets DECIMAL(20, 2) DEFAULT 0.00 COMMENT '再保險合約資產',
    property_and_equipment      DECIMAL(20, 2) DEFAULT 0.00 COMMENT '不動產及設備',
    right_of_use_assets         DECIMAL(20, 2) DEFAULT 0.00 COMMENT '使用權資產',
    intangible_assets           DECIMAL(20, 2) DEFAULT 0.00 COMMENT '無形資產',
    deferred_tax_assets         DECIMAL(20, 2) DEFAULT 0.00 COMMENT '遞延所得稅資產',
    other_assets                DECIMAL(20, 2) DEFAULT 0.00 COMMENT '其他資產',
    assets_on_separate_accounts DECIMAL(20, 2) DEFAULT 0.00 COMMENT '分離帳戶保險商品資產',
    total_assets                DECIMAL(20, 2) DEFAULT 0.00 COMMENT '資產總計',
    
    -- 3. 保險業負債類項目
    short_term_debts            DECIMAL(20, 2) DEFAULT 0.00 COMMENT '短期債務',
    payables                    DECIMAL(20, 2) DEFAULT 0.00 COMMENT '應付款項',
    current_tax_liabilities     DECIMAL(20, 2) DEFAULT 0.00 COMMENT '本期所得稅負債',
    liabilities_related_to_assets_held_for_sale  DECIMAL(20, 2) DEFAULT 0.00 COMMENT '與待出售資產直接相關之負債',
    financial_liabilities_at_fvtpl               DECIMAL(20, 2) DEFAULT 0.00 COMMENT '透過損益按公允價值衡量之金融負債',
    derivative_financial_liabilities_for_hedging DECIMAL(20, 2) DEFAULT 0.00 COMMENT '避險之衍生金融負債',
    bonds_payable               DECIMAL(20, 2) DEFAULT 0.00 COMMENT '應付債券',
    preferred_stock_liabilities DECIMAL(20, 2) DEFAULT 0.00 COMMENT '特別股負債',
    other_financial_liabilities DECIMAL(20, 2) DEFAULT 0.00 COMMENT '其他金融負債',
    lease_liabilities           DECIMAL(20, 2) DEFAULT 0.00 COMMENT '租賃負債',
    insurance_liabilities       DECIMAL(20, 2) DEFAULT 0.00 COMMENT '保險負債',
    financial_insurance_reserves        DECIMAL(20, 2) DEFAULT 0.00 COMMENT '具金融商品性質之保險契約準備',
    foreign_exchange_valuation_reserves DECIMAL(20, 2) DEFAULT 0.00 COMMENT '外匯價格變動準備',
    provisions                  DECIMAL(20, 2) DEFAULT 0.00 COMMENT '負債準備',
    deferred_tax_liabilities    DECIMAL(20, 2) DEFAULT 0.00 COMMENT '遞延所得稅負債',
    other_liabilities           DECIMAL(20, 2) DEFAULT 0.00 COMMENT '其他負債',
    liabilities_on_segregated_accounts  DECIMAL(20, 2) DEFAULT 0.00 COMMENT '分離帳戶保險商品負債',
    total_liabilities           DECIMAL(20, 2) DEFAULT 0.00 COMMENT '負債總計',
    
    -- 4. 權益類項目
    share_capital             DECIMAL(20, 2) DEFAULT 0.00 COMMENT '股本',
    security_token_equity     DECIMAL(20, 2) DEFAULT 0.00 COMMENT '權益 (具證券性質之虛擬通貨)',
    capital_surplus           DECIMAL(20, 2) DEFAULT 0.00 COMMENT '資本公積',
    retained_earnings         DECIMAL(20, 2) DEFAULT 0.00 COMMENT '保留盈餘（累積虧損)',
    other_equity              DECIMAL(20, 2) DEFAULT 0.00 COMMENT '其他權益',
    treasury_stock            DECIMAL(20, 2) DEFAULT 0.00 COMMENT '庫藏股票',
    equity_attributable_to_owners_of_parent DECIMAL(20, 2) DEFAULT 0.00 COMMENT '歸屬於母公司業主之權益合計',
    predecessor_interests     DECIMAL(20, 2) DEFAULT 0.00 COMMENT '共同控制下前手權益',
    equity_not_under_common_control DECIMAL(20, 2) DEFAULT 0.00 COMMENT '合併前非屬共同控制股權',
    non_controlling_interests DECIMAL(20, 2) DEFAULT 0.00 COMMENT '非控制權益',
    total_equity              DECIMAL(20, 2) DEFAULT 0.00 COMMENT '權益總計',
    
    -- 5. 保險業特有總計項目
    total_liabilities_and_equity DECIMAL(20, 2) DEFAULT 0.00 COMMENT '負債及權益總計',
    
    -- 6. 股數與每股價值項目
    pending_cancellation_shares       DECIMAL(20, 2) DEFAULT 0.00 COMMENT '待註銷股本股數（單位：股）',
    pre_received_shares_equivalent    DECIMAL(20, 2) DEFAULT 0.00 COMMENT '預收股款（權益項下）之約當發行股數（單位：股）',
    parent_subsidiary_treasury_shares DECIMAL(20, 2) DEFAULT 0.00 COMMENT '母公司暨子公司所持有之母公司庫藏股股數（單位：股）',
    net_value_per_share               DECIMAL(20, 2) DEFAULT 0.00 COMMENT '每股參考淨值',
    
    -- 7. 系統稽核欄位 (不影響原始欄位順序，放最後)
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '資料最後更新時間',
    
    -- 設定主鍵與索引
    PRIMARY KEY (company_code, year, quarter),
    INDEX idx_year_quarter (year, quarter)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='公發公司資產負債表-保險業';