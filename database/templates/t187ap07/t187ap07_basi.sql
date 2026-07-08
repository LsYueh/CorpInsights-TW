CREATE TABLE IF NOT EXISTS t187ap07_basi_{COMPANY_CODE} (
    -- 1. 核心識別與索引欄位
    year VARCHAR(4) NOT NULL COMMENT '年度 ( e.g., "2026" )',
    quarter VARCHAR(2) NOT NULL COMMENT '季別 ( e.g., "01", "02", "Q1" )',
    market_type VARCHAR(2) NOT NULL COMMENT '市場註記 ( L: 上市公司, X: 公發公司 )',
    company_code VARCHAR(10) NOT NULL COMMENT '公司代號',
    company_name VARCHAR(100) NOT NULL COMMENT '公司名稱',
    
    -- 2. 金融業資產類項目
    cash_and_cash_equivalents DECIMAL(20, 2) DEFAULT 0.00 COMMENT '現金及約當現金',
    due_from_central_bank_and_banks DECIMAL(20, 2) DEFAULT 0.00 COMMENT '存放央行及拆借銀行同業',
    financial_assets_at_fair_value_through_profit_or_loss DECIMAL(20, 2) DEFAULT 0.00 COMMENT '透過損益按公允價值衡量之金融資產',
    financial_assets_at_fair_value_through_other_comprehensive_income DECIMAL(20, 2) DEFAULT 0.00 COMMENT '透過其他綜合損益按公允價值衡量之金融資產',
    financial_assets_at_amortized_cost DECIMAL(20, 2) DEFAULT 0.00 COMMENT '按攤銷後成本衡量之債務工具投資',
    derivative_financial_assets_for_hedging DECIMAL(20, 2) DEFAULT 0.00 COMMENT '避險之衍生金融資產淨額',
    securities_purchased_under_resale_agreements DECIMAL(20, 2) DEFAULT 0.00 COMMENT '附賣回票券及債券投資淨額',
    receivables_net DECIMAL(20, 2) DEFAULT 0.00 COMMENT '應收款項－淨額',
    current_tax_assets DECIMAL(20, 2) DEFAULT 0.00 COMMENT '當期所得稅資產',
    assets_held_for_sale_net DECIMAL(20, 2) DEFAULT 0.00 COMMENT '待出售資產－淨額',
    assets_to_be_distributed_to_owners_net DECIMAL(20, 2) DEFAULT 0.00 COMMENT '待分配予業主之資產－淨額',
    discounts_and_loans_net DECIMAL(20, 2) DEFAULT 0.00 COMMENT '貼現及放款－淨額',
    investments_accounted_for_using_equity_method_net DECIMAL(20, 2) DEFAULT 0.00 COMMENT '採用權益法之投資－淨額',
    restricted_assets_net DECIMAL(20, 2) DEFAULT 0.00 COMMENT '受限制資產－淨額',
    other_financial_assets_net DECIMAL(20, 2) DEFAULT 0.00 COMMENT '其他金融資產－淨額',
    property_and_equipment_net DECIMAL(20, 2) DEFAULT 0.00 COMMENT '不動產及設備－淨額',
    right_of_use_assets_net DECIMAL(20, 2) DEFAULT 0.00 COMMENT '使用權資產－淨額',
    investment_property_net DECIMAL(20, 2) DEFAULT 0.00 COMMENT '投資性不動產投資－淨額',
    intangible_assets_net DECIMAL(20, 2) DEFAULT 0.00 COMMENT '無形資產－淨額',
    deferred_tax_assets DECIMAL(20, 2) DEFAULT 0.00 COMMENT '遞延所得稅資產',
    other_assets_net DECIMAL(20, 2) DEFAULT 0.00 COMMENT '其他資產－淨額',
    total_assets_amount DECIMAL(20, 2) DEFAULT 0.00 COMMENT '資產總額',
    
    -- 3. 金融業負債類項目
    deposits_from_central_bank_and_banks DECIMAL(20, 2) DEFAULT 0.00 COMMENT '央行及銀行同業存款',
    due_to_central_bank_and_banks DECIMAL(20, 2) DEFAULT 0.00 COMMENT '央行及同業融資',
    financial_liabilities_at_fair_value_through_profit_or_loss DECIMAL(20, 2) DEFAULT 0.00 COMMENT '透過損益按公允價值衡量之金融負債',
    derivative_financial_liabilities_for_hedging DECIMAL(20, 2) DEFAULT 0.00 COMMENT '避險之衍生金融負債－淨額',
    securities_sold_under_repurchase_agreements DECIMAL(20, 2) DEFAULT 0.00 COMMENT '附買回票券及債券負債',
    payables DECIMAL(20, 2) DEFAULT 0.00 COMMENT '應付款項',
    current_tax_liabilities DECIMAL(20, 2) DEFAULT 0.00 COMMENT '當期所得稅負債',
    liabilities_directly_associated_with_assets_held_for_sale DECIMAL(20, 2) DEFAULT 0.00 COMMENT '與待出售資產直接相關之負債',
    deposits_and_remittances DECIMAL(20, 2) DEFAULT 0.00 COMMENT '存款及匯款',
    financial_bonds_payable DECIMAL(20, 2) DEFAULT 0.00 COMMENT '應付金融債券',
    bonds_payable DECIMAL(20, 2) DEFAULT 0.00 COMMENT '應付公司債',
    preferred_stock_liabilities DECIMAL(20, 2) DEFAULT 0.00 COMMENT '特別股負債',
    other_financial_liabilities DECIMAL(20, 2) DEFAULT 0.00 COMMENT '其他金融負債',
    provisions_for_liabilities DECIMAL(20, 2) DEFAULT 0.00 COMMENT '負債準備',
    lease_liabilities DECIMAL(20, 2) DEFAULT 0.00 COMMENT '租賃負債',
    deferred_tax_liabilities DECIMAL(20, 2) DEFAULT 0.00 COMMENT '遞延所得稅負債',
    other_liabilities DECIMAL(20, 2) DEFAULT 0.00 COMMENT '其他負債',
    total_liabilities_amount DECIMAL(20, 2) DEFAULT 0.00 COMMENT '負債總額',
    
    -- 4. 權益類項目
    share_capital DECIMAL(20, 2) DEFAULT 0.00 COMMENT '股本',
    equity_virtual_currencies DECIMAL(20, 2) DEFAULT 0.00 COMMENT '權益─具證券性質之虛擬通貨',
    capital_surplus DECIMAL(20, 2) DEFAULT 0.00 COMMENT '資本公積',
    retained_earnings DECIMAL(20, 2) DEFAULT 0.00 COMMENT '保留盈餘',
    other_equity DECIMAL(20, 2) DEFAULT 0.00 COMMENT '其他權益',
    treasury_stock DECIMAL(20, 2) DEFAULT 0.00 COMMENT '庫藏股票',
    equity_attributable_to_owners DECIMAL(20, 2) DEFAULT 0.00 COMMENT '歸屬於母公司業主之權益合計',
    predecessor_interests DECIMAL(20, 2) DEFAULT 0.00 COMMENT '共同控制下前手權益',
    non_joint_control_equity DECIMAL(20, 2) DEFAULT 0.00 COMMENT '合併前非屬共同控制股權',
    non_controlling_interests DECIMAL(20, 2) DEFAULT 0.00 COMMENT '非控制權益',
    total_equity_amount DECIMAL(20, 2) DEFAULT 0.00 COMMENT '權益總額',
    
    -- 5. 股數與每股價值項目
    pending_cancellation_shares DECIMAL(20, 2) DEFAULT 0.00 COMMENT '待註銷股本股數（單位：股）',
    parent_subsidiary_treasury_shares DECIMAL(20, 2) DEFAULT 0.00 COMMENT '母公司暨子公司所持有之母公司庫藏股股數（單位：股）',
    pre_received_shares DECIMAL(20, 2) DEFAULT 0.00 COMMENT '預收股款（權益項下）之約當發行股數（單位：股）',
    net_value_per_share DECIMAL(20, 2) DEFAULT 0.00 COMMENT '每股參考淨值',
    
    -- 6. 系統稽核欄位
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '資料最後更新時間',
    
    -- 設定主鍵
    PRIMARY KEY (year, quarter)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='公發公司資產負債表-金融業 (公司 {COMPANY_CODE})';