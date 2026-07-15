CREATE TABLE IF NOT EXISTS t187ap07_fh (
    -- 1. 核心識別與索引欄位
    year         SMALLINT         NOT NULL COMMENT '年度 ( e.g., "2026" )',
    quarter      TINYINT UNSIGNED NOT NULL COMMENT '季別 ( 1: 第一季, 2: 第二季, 3: 第三季, 4: 第四季 )',
    market_type  CHAR(1)          NOT NULL COMMENT '市場註記 ( L: 上市公司, X: 公發公司 )',
    company_code VARCHAR(10)      NOT NULL COMMENT '公司代號',
    company_name VARCHAR(100)     NOT NULL COMMENT '公司名稱',
    
    -- 2. 金控業資產類項目
    cash_and_cash_equivalents                 DECIMAL(20, 2) DEFAULT 0.00 COMMENT '現金及約當現金',
    due_from_central_bank_and_financial_peers DECIMAL(20, 2) DEFAULT 0.00 COMMENT '存放央行及拆借金融同業',
    financial_assets_at_fvtpl                 DECIMAL(20, 2) DEFAULT 0.00 COMMENT '透過損益按公允價值衡量之金融資產',
    financial_assets_at_fvoci                 DECIMAL(20, 2) DEFAULT 0.00 COMMENT '透過其他綜合損益按公允價值衡量之金融資產',
    financial_assets_at_amortized_cost        DECIMAL(20, 2) DEFAULT 0.00 COMMENT '按攤銷後成本衡量之債務工具投資',
    derivative_financial_assets_for_hedging   DECIMAL(20, 2) DEFAULT 0.00 COMMENT '避險之衍生金融資產淨額',
    reverse_repo                DECIMAL(20, 2) DEFAULT 0.00 COMMENT '附賣回票券及債券投資淨額',
    receivables                 DECIMAL(20, 2) DEFAULT 0.00 COMMENT '應收款項－淨額',
    current_tax_assets          DECIMAL(20, 2) DEFAULT 0.00 COMMENT '當期所得稅資產',
    assets_held_for_sale        DECIMAL(20, 2) DEFAULT 0.00 COMMENT '待出售資產－淨額',
    assets_for_distribution     DECIMAL(20, 2) DEFAULT 0.00 COMMENT '待分配予業主之資產－淨額',
    discounts_and_loans         DECIMAL(20, 2) DEFAULT 0.00 COMMENT '貼現及放款－淨額',
    reinsurance_contract_assets DECIMAL(20, 2) DEFAULT 0.00 COMMENT '再保險合約資產－淨額',
    investments_using_equity_method DECIMAL(20, 2) DEFAULT 0.00 COMMENT '採用權益法之投資－淨額',
    restricted_assets           DECIMAL(20, 2) DEFAULT 0.00 COMMENT '受限制資產－淨額',
    other_financial_assets      DECIMAL(20, 2) DEFAULT 0.00 COMMENT '其他金融資產－淨額',
    investment_property         DECIMAL(20, 2) DEFAULT 0.00 COMMENT '投資性不動產－淨額',
    property_and_equipment      DECIMAL(20, 2) DEFAULT 0.00 COMMENT '不動產及設備－淨額',
    right_of_use_assets         DECIMAL(20, 2) DEFAULT 0.00 COMMENT '使用權資產－淨額',
    intangible_assets           DECIMAL(20, 2) DEFAULT 0.00 COMMENT '無形資產－淨額',
    deferred_tax_assets         DECIMAL(20, 2) DEFAULT 0.00 COMMENT '遞延所得稅資產',
    other_assets                DECIMAL(20, 2) DEFAULT 0.00 COMMENT '其他資產－淨額',
    total_assets_amount         DECIMAL(20, 2) DEFAULT 0.00 COMMENT '資產總額',
    
    -- 3. 金控業負債類項目
    deposits_from_central_bank_and_financial_peers DECIMAL(20, 2) DEFAULT 0.00 COMMENT '央行及金融同業存款',
    due_to_central_bank_and_financial_peers        DECIMAL(20, 2) DEFAULT 0.00 COMMENT '央行及同業融資',
    financial_liabs_at_fvtpl DECIMAL(20, 2) DEFAULT 0.00 COMMENT '透過損益按公允價值衡量之金融負債',
    derivative_financial_liabs_for_hedging_net DECIMAL(20, 2) DEFAULT 0.00 COMMENT '避險之衍生金融負債',
    repo_liabs               DECIMAL(20, 2) DEFAULT 0.00 COMMENT '附買回票券及債券負債',
    commercial_paper_payable_net DECIMAL(20, 2) DEFAULT 0.00 COMMENT '應付商業本票－淨額',
    payables                 DECIMAL(20, 2) DEFAULT 0.00 COMMENT '應付款項',
    current_tax_liabs        DECIMAL(20, 2) DEFAULT 0.00 COMMENT '當期所得稅負債',
    liabs_related_to_assets_held_for_sale DECIMAL(20, 2) DEFAULT 0.00 COMMENT '與待出售資產直接相關之負債',
    deposits_and_remittances DECIMAL(20, 2) DEFAULT 0.00 COMMENT '存款及匯款',
    bonds_payable            DECIMAL(20, 2) DEFAULT 0.00 COMMENT '應付債券',
    other_borrowings         DECIMAL(20, 2) DEFAULT 0.00 COMMENT '其他借款',
    preferred_stock_liabs    DECIMAL(20, 2) DEFAULT 0.00 COMMENT '特別股負債',
    provisions_for_liabs     DECIMAL(20, 2) DEFAULT 0.00 COMMENT '負債準備',
    other_financial_liabs    DECIMAL(20, 2) DEFAULT 0.00 COMMENT '其他金融負債',
    lease_liabs              DECIMAL(20, 2) DEFAULT 0.00 COMMENT '租賃負債',
    deferred_tax_liabs       DECIMAL(20, 2) DEFAULT 0.00 COMMENT '遞延所得稅負債',
    other_liabs              DECIMAL(20, 2) DEFAULT 0.00 COMMENT '其他負債',
    total_liabs_amount       DECIMAL(20, 2) DEFAULT 0.00 COMMENT '負債總額',
    
    -- 4. 權益類項目
    share_capital             DECIMAL(20, 2) DEFAULT 0.00 COMMENT '股本',
    capital_surplus           DECIMAL(20, 2) DEFAULT 0.00 COMMENT '資本公積',
    retained_earnings         DECIMAL(20, 2) DEFAULT 0.00 COMMENT '保留盈餘（累積虧損)',
    other_equity              DECIMAL(20, 2) DEFAULT 0.00 COMMENT '其他權益',
    treasury_stock            DECIMAL(20, 2) DEFAULT 0.00 COMMENT '庫藏股票',
    equity_attributable_to_owners_of_parent DECIMAL(20, 2) DEFAULT 0.00 COMMENT '歸屬於母公司業主之權益',
    predecessor_interests     DECIMAL(20, 2) DEFAULT 0.00 COMMENT '共同控制下前手權益',
    non_controlling_interests DECIMAL(20, 2) DEFAULT 0.00 COMMENT '非控制權益',
    total_equity_amount       DECIMAL(20, 2) DEFAULT 0.00 COMMENT '權益總額',
    
    -- 5. 股數與每股價值項目
    pending_cancellation_shares       DECIMAL(20, 2) DEFAULT 0.00 COMMENT '待註銷股本股數（單位：股）',
    pre_received_shares_equivalent    DECIMAL(20, 2) DEFAULT 0.00 COMMENT '預收股款（權益項下）之約當發行股數（單位：股）',
    parent_subsidiary_treasury_shares DECIMAL(20, 2) DEFAULT 0.00 COMMENT '母公司暨子公司所持有之母公司庫藏股股數（單位：股）',
    net_value_per_share               DECIMAL(20, 2) DEFAULT 0.00 COMMENT '每股參考淨值',
    
    -- 6. 系統稽核欄位
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '資料最後更新時間',
    
    -- 設定主鍵與索引
    PRIMARY KEY (company_code, year, quarter),
    INDEX idx_year_quarter (year, quarter)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='公發公司資產負債表-金控業';