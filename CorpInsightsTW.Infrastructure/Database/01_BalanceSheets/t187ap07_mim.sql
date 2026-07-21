CREATE TABLE IF NOT EXISTS `t187ap07_mim` (
    -- 1. 核心識別與索引欄位
    `year`                                        SMALLINT        NOT NULL COMMENT '年度 ( e.g., 2026 )',
    `quarter`                                     TINYINT UNSIGNED NOT NULL COMMENT '季別 ( 1: 第一季, 2: 第二季, 3: 第三季, 4: 第四季 )',
    `listing_status`                              CHAR(1)         NOT NULL COMMENT '掛牌狀態 ( L: 上市, O: 上櫃, R: 興櫃, X: 公發 )',
    `company_code`                                VARCHAR(10)     NOT NULL COMMENT '公司代號',
    `company_name`                                VARCHAR(100)    NOT NULL COMMENT '公司名稱',

    -- 2. 資產類項目
    `current_assets`                              DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '流動資產',
    `non_current_assets`                          DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '非流動資產',
    `total_assets`                                DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '資產總額 / 資產總計',

    -- 3. 負債類項目
    `current_liabs`                               DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '流動負債',
    `non_current_liabs`                           DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '非流動負債',
    `total_liabs`                                 DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '負債總額 / 負債總計',

    -- 4. 權益類項目
    `share_capital`                               DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '股本',
    `security_token_equity`                       DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '權益─具證券性質之虛擬通貨',
    `capital_surplus`                             DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '資本公積',
    `retained_earnings`                           DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '保留盈餘',
    `other_equity`                                DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '其他權益',
    `treasury_stock`                              DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '庫藏股票',
    `equity_attributable_to_owners_of_parent`     DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '歸屬於母公司業主之權益合計',
    `predecessor_interests`                       DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '共同控制下前手權益',
    `non_controlling_interests`                   DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '非控制權益',
    `total_equity_amount`                         DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '權益總額 / 權益總計',

    -- 5. 股數與每股價值項目
    `pending_cancellation_shares`                 DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '待註銷股本股數',
    `pre_received_shares_equivalent`              DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '預收股款（權益項下）之約當發行股數',
    `parent_subsidiary_treasury_shares`           DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '母公司暨子公司所持有之母公司庫藏股股數',
    `net_value_per_share`                         DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '每股參考淨值',

    -- 6. 系統稽核欄位
    `updated_at`                                  DATETIME        NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新時間',

    PRIMARY KEY (`company_code`, `year`, `quarter`),
    KEY `idx_year_quarter` (`year`, `quarter`),
    KEY `idx_listing_status` (`listing_status`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='公司資產負債表-異業';