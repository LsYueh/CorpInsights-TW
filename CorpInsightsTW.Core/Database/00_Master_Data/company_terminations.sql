CREATE TABLE IF NOT EXISTS company_terminations (
    `company_code`      VARCHAR(10)  NOT NULL COMMENT '公司代號',
    
    `reason`            ENUM('delisted', 'merged') NOT NULL COMMENT '終止原因 ( delisted:下市, merged:合併 )',
    `effective_date`    DATE         NOT NULL COMMENT '生效日期',
    `merged_into_code`  VARCHAR(10)  NULL     COMMENT '存續公司代號',
    `created_at`        TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '建立時間',
    
    PRIMARY KEY (`company_code`),
    INDEX `idx_merged_into` (`merged_into_code`),
    
    -- 外鍵約束：確保公司必須存在於主表
    CONSTRAINT `fk_term_company` FOREIGN KEY (`company_code`) REFERENCES `companies` (`company_code`) ON DELETE CASCADE,
    CONSTRAINT `fk_term_merged_into` FOREIGN KEY (`merged_into_code`) REFERENCES `companies` (`company_code`) ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='公司下市/合併事件紀錄表';

-- 查詢時： 使用 LEFT JOIN company_terminations 即可知道該公司是否已經下市或被合併。