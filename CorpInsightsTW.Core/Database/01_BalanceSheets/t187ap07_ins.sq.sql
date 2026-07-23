CREATE TABLE IF NOT EXISTS `t187ap07_ins` (
    -- 1. 核心識別與索引欄位
    `year`                                        SMALLINT        NOT NULL COMMENT '年度 ( e.g., 2026 )',
    `quarter`                                     TINYINT UNSIGNED NOT NULL COMMENT '季別 ( 1: 第一季, 2: 第二季, 3: 第三季, 4: 第四季 )',
    `listing_status`                              CHAR(1)         NOT NULL COMMENT '掛牌狀態 ( L: 上市, O: 上櫃, R: 興櫃, X: 公發 )',
    `company_code`                                VARCHAR(10)     NOT NULL COMMENT '公司代號',
    `company_name`                                VARCHAR(100)    NOT NULL COMMENT '公司名稱',

    -- 2. 保險業資產類項目
    `cash_and_cash_equivalents`                   DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '現金及約當現金',
    `receivables`                                 DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '應收款項',
    `current_tax_assets`                          DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '本期所得稅資產',
    `assets_held_for_sale`                        DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '待出售資產',
    `assets_for_distribution`                     DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '待分配予業主之資產（或處分群組）',
    `financial_assets_at_fvtpl`                   DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '透過損益按公允價值衡量之金融資產',
    `financial_assets_at_fvoci`                   DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '透過其他綜合損益按公允價值衡量之金融資產',
    `financial_assets_at_amortized_cost`          DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '按攤銷後成本衡量之金融資產',
    `derivative_financial_assets_for_hedging`     DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '避險之金融資產',
    `investments_using_equity_method`             DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '採用權益法之投資－淨額',
    `other_financial_assets`                      DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '其他金融資產－淨額',
    `investment_property`                         DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '投資性不動產',
    `loans`                                       DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '放款',
    `insurance_contract_assets`                   DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '保險合約資產',
    `reinsurance_contract_assets`                 DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '再保險合約資產',
    `property_and_equipment`                      DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '不動產及設備',
    `right_of_use_assets`                         DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '使用權資產',
    `intangible_assets`                           DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '無形資產',
    `deferred_tax_assets`                         DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '遞延所得稅資產',
    `other_assets`                                DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '其他資產',
    `assets_on_separate_accounts`                 DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '分離帳戶保險商品資產',
    `total_assets`                                DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '資產總計',

    -- 3. 保險業負債類項目
    `short_term_debts`                            DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '短期債務',
    `payables`                                    DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '應付款項',
    `current_tax_liabs`                           DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '本期所得稅負債',
    `liabs_related_to_assets_held_for_sale`       DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '與待出售資產直接相關之負債',
    `financial_liabs_at_fvtpl`                    DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '透過損益按公允價值衡量之金融負債',
    `derivative_financial_liabs_for_hedging`      DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '避險之金融負債',
    `bonds_payable`                               DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '應付債券',
    `preferred_stock_liabs`                       DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '特別股負債',
    `provisions`                                  DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '負債準備',
    `other_financial_liabs`                       DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '其他金融負債',
    `lease_liabs`                                 DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '租賃負債',
    `deferred_tax_liabs`                          DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '遞延所得稅負債',
    `other_liabs`                                 DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '其他負債',
    `total_liabs`                                 DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '負債總計',

    -- 4. 權益類項目
    `share_capital`                               DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '股本',
    `security_token_equity`                       DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '權益─具證券性質之虛擬通貨',
    `capital_surplus`                             DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '資本公積',
    `retained_earnings`                           DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '保留盈餘',
    `other_equity`                                DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '其他權益',
    `treasury_stock`                              DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '庫藏股票',
    `equity_attributable_to_owners_of_parent`     DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '歸屬於母公司業主之權益合計',
    `predecessor_interests`                       DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '共同控制下前手權益',
    `equity_not_under_common_control`             DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '合併前非屬共同控制股權',
    `non_controlling_interests`                   DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '非控制權益',
    `total_equity`                                DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '權益總計',

    -- 5. 保險業特有總計項目
    `total_liabs_and_equity`                      DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '負債及權益總計',

    -- 6. 股數與每股價值項目
    `pending_cancellation_shares`                 DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '待註銷股本股數（單位：股）',
    `pre_received_shares_equivalent`              DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '預收股款（權益項下）之約當發行股數（單位：股）',
    `parent_subsidiary_treasury_shares`           DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '母公司暨子公司所持有之母公司庫藏股股數（單位：股）',
    `net_value_per_share`                         DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '每股參考淨值',

    -- 7. 系統稽核欄位
    `updated_at`                                  DATETIME        NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新時間',

    PRIMARY KEY (`company_code`, `year`, `quarter`),
    KEY `idx_year_quarter` (`year`, `quarter`),
    KEY `idx_listing_status` (`listing_status`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='公司資產負債表-保險業';