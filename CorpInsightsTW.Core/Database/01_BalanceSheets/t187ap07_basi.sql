CREATE TABLE IF NOT EXISTS `t187ap07_basi` (
    -- 1. 核心識別與索引欄位
    `year`                                        SMALLINT        NOT NULL COMMENT '年度 ( e.g., 2026 )',
    `quarter`                                     TINYINT UNSIGNED NOT NULL COMMENT '季別 ( 1: 第一季, 2: 第二季, 3: 第三季, 4: 第四季 )',
    `listing_status`                              CHAR(1)         NOT NULL COMMENT '掛牌狀態 ( L: 上市, O: 上櫃, R: 興櫃, X: 公發 )',
    `company_code`                                VARCHAR(10)     NOT NULL COMMENT '公司代號',
    `company_name`                                VARCHAR(100)    NOT NULL COMMENT '公司名稱',

    -- 2. 金融業資產類項目
    `cash_and_cash_equivalents`                   DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '現金及約當現金',
    `due_from_central_bank_and_banks`             DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '存放央行及拆借銀行同業',
    `financial_assets_at_fvtpl`                   DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '透過損益按公允價值衡量之金融資產',
    `financial_assets_at_fvoci`                   DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '透過其他綜合損益按公允價值衡量之金融資產',
    `financial_assets_at_amortized_cost`          DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '按攤銷後成本衡量之債務工具投資',
    `derivative_financial_assets_for_hedging`     DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '避險之金融資產－淨額 / 避險之衍生金融資產淨額',
    `reverse_repo`                                DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '附賣回票券及債券投資淨額',
    `receivables`                                 DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '應收款項－淨額',
    `current_tax_assets`                          DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '本期所得稅資產 / 當期所得稅資產',
    `assets_held_for_sale`                        DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '待出售資產－淨額',
    `assets_for_distribution`                     DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '待分配予業主之資產－淨額',
    `discounts_and_loans`                         DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '貼現及放款－淨額',
    `investments_using_equity_method`             DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '採用權益法之投資－淨額',
    `restricted_assets`                           DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '受限制資產－淨額',
    `other_financial_assets`                      DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '其他金融資產－淨額',
    `property_and_equipment`                      DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '不動產及設備－淨額',
    `right_of_use_assets`                         DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '使用權資產－淨額',
    `investment_property`                         DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '投資性不動產－淨額 / 投資性不動產投資－淨額',
    `intangible_assets`                           DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '無形資產－淨額',
    `deferred_tax_assets`                         DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '遞延所得稅資產',
    `other_assets`                                DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '其他資產－淨額',
    `total_assets_amount`                         DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '資產總計',

    -- 3. 金融業負債類項目
    `deposits_from_central_bank_and_banks`        DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '央行及銀行同業存款',
    `due_to_central_bank_and_banks`               DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '央行及同業融資',
    `financial_liabs_at_fvtpl`                    DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '透過損益按公允價值衡量之金融負債',
    `derivative_financial_liabs_for_hedging`      DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '避險之金融負債 / 避險之衍生金融負債－淨額',
    `repo_liabs`                                  DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '附買回票券及債券負債',
    `payables`                                    DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '應付款項',
    `current_tax_liabs`                           DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '本期所得稅負債',
    `liabs_related_to_assets_held_for_sale`       DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '與待出售資產直接相關之負債',
    `deposits_and_remittances`                    DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '存款及匯款',
    `financial_bonds_payable`                     DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '應付金融債券',
    `bonds_payable`                               DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '應付公司債',
    `preferred_stock_liabs`                       DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '特別股負債',
    `other_financial_liabs`                       DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '其他金融負債',
    `provisions_for_liabs`                        DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '負債準備',
    `lease_liabs`                                 DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '租賃負債',
    `deferred_tax_liabs`                          DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '遞延所得稅負債',
    `other_liabs`                                 DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '其他負債',
    `total_liabs_amount`                          DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '負債總計',

    -- 4. 權益類項目
    `share_capital`                               DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '股本',
    `security_token_equity`                       DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '權益─具證券性質之虛擬通貨',
    `capital_surplus`                             DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '資本公積',
    `retained_earnings`                           DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '保留盈餘',
    `other_equity`                                DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '其他權益',
    `treasury_stock`                              DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '庫藏股票',
    `equity_attributable_to_owners_of_parent`     DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '歸屬於母公司業主之權益合計',
    `predecessor_interests`                       DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '共同控制下前手權益',
    `equity_not_under_common_control`             DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '合併前非屬共同控制股權',
    `non_controlling_interests`                   DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '非控制權益',
    `total_equity_amount`                         DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '權益總計 / 權益總額',

    -- 5. 股數與每股價值項目
    `pending_cancellation_shares`                 DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '待註銷股本股數（單位：股）',
    `pre_received_shares_equivalent`              DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '預收股款（權益項下）之約當發行股數（單位：股）',
    `parent_subsidiary_treasury_shares`           DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '母公司暨子公司所持有之母公司庫藏股股數（單位：股）',
    `net_value_per_share`                         DECIMAL(18, 2)  NOT NULL DEFAULT 0.00 COMMENT '每股參考淨值',

    -- 6. 稽核欄位
    `updated_at`                                  DATETIME        NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新時間',

    PRIMARY KEY (`company_code`, `year`, `quarter`),
    KEY `idx_year_quarter` (`year`, `quarter`),
    KEY `idx_listing_status` (`listing_status`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='公司資產負債表-金融業';
-- ----------------------------------------------------
-- 💡 未來擴充備案：若未來加入日頻率大數據，再啟用年份分區
-- PARTITION BY RANGE COLUMNS(year) (
--     PARTITION p_old VALUES LESS THAN ('2020'),
--     PARTITION p2020 VALUES LESS THAN ('2021'),
--     PARTITION p2021 VALUES LESS THAN ('2022'),
--     PARTITION p2022 VALUES LESS THAN ('2023'),
--     PARTITION p2023 VALUES LESS THAN ('2024'),
--     PARTITION p2024 VALUES LESS THAN ('2025'),
--     PARTITION p2025 VALUES LESS THAN ('2026'),
--     PARTITION p2026 VALUES LESS THAN ('2027'),
--     PARTITION p2027 VALUES LESS THAN ('2028'),
--     PARTITION p2028 VALUES LESS THAN ('2029'),
--     PARTITION p_future VALUES LESS THAN (MAXVALUE) -- 💡 所有超過的通通先塞這裡，防止系統死當
-- );