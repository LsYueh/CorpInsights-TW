CREATE TABLE IF NOT EXISTS companies (
    -- 1. 主鍵與識別
    `company_code`        VARCHAR(10)  NOT NULL COMMENT '公司代號 ( e.g., "2330", "2881" )',
    `company_name`        VARCHAR(100) NOT NULL COMMENT '公司簡稱 ( e.g., "台積電", "富邦金" )',
    
    -- 2. 原始屬性
    `listing_status`      CHAR(1)      NOT NULL COMMENT '掛牌狀態 ( L: 上市, O: 上櫃, U: 興櫃, X: 公發 )',
    `xbrl_taxonomy`       ENUM('ci', 'basi', 'bd', 'fh', 'ins', 'mim') NOT NULL 
        COMMENT 'XBRL行業別分類 ( ci:一般業, basi:金融業, bd:證券期貨業, fh:金控業, ins:保險業, mim:異業 )',
    
    -- 3. 系統管理欄位
    `is_active`           TINYINT(1)   NOT NULL DEFAULT 1 COMMENT '是否啟用中 ( 1: 啟用, 0: 停用 )',
    `created_at`          TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '資料建立時間',
    `updated_at`          TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '最後更新時間',
    
    PRIMARY KEY (`company_code`),
    INDEX `idx_listing_status` (`listing_status`),
    INDEX `idx_xbrl_taxonomy`  (`xbrl_taxonomy`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='公司基本資料表';