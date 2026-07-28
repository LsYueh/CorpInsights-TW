CREATE TABLE `t163sb20` (
    -- 1. 核心識別與索引欄位
    `company_code`            VARCHAR(10)     NOT NULL COMMENT '公司代號',
    `year`                    SMALLINT        NOT NULL COMMENT '年度 ( e.g., 2026 )',
    `quarter`                 TINYINT UNSIGNED NOT NULL COMMENT '季別 ( 1: 第一季, 2: 第二季, 3: 第三季, 4: 第四季 )',
    `listing_status`          CHAR(1)         NOT NULL COMMENT '掛牌狀態 ( L: 上市, O: 上櫃, U: 興櫃, X: 公發 )',
    `company_name`            VARCHAR(100)    NOT NULL COMMENT '公司名稱',
    
    -- 2. 現金流量表數值明細 (Cash Flow Amounts)
    `operating_cash_flows`    DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '營業活動之淨現金流入（流出）',
    `investing_cash_flows`    DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '投資活動之淨現金流入（流出）',
    `financing_cash_flows`    DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '籌資活動之淨現金流入（流出）',
    `fx_effect`               DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '匯率變動對現金及約當現金之影響',
    `net_change_in_cash`      DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '本期現金及約當現金增加（減少）數',
    `beginning_cash_balance`  DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '期初現金及約當現金餘額',
    `ending_cash_balance`     DECIMAL(20, 2)  NOT NULL DEFAULT 0.00 COMMENT '期末現金及約當現金餘額',
    
    -- 3. 系統稽核欄位
    `updated_at`              DATETIME        NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新時間',

    PRIMARY KEY (`company_code`, `year`, `quarter`),
    KEY `idx_year_quarter` (`year`, `quarter`),
    KEY `idx_listing_status` (`listing_status`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='現金流量表';