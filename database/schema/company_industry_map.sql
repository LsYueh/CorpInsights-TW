CREATE TABLE IF NOT EXISTS company_industry_map (
    -- 1. 主鍵與核心識別
    company_code VARCHAR(10) NOT NULL COMMENT '公司代號 ( e.g., "2330", "2881" )',
    company_name VARCHAR(100) NOT NULL COMMENT '公司名稱 ( e.g., "台積電", "富邦金" )',
    
    -- 2. 路由核心欄位
    industry_type ENUM('ci', 'basi', 'bd', 'fh', 'ins', 'mim') NOT NULL 
        COMMENT '財報行業別分類 ( ci:一般業, basi:金融業, bd:證券期貨業, fh:金控業, ins:保險業, mim:異業 )',
    
    -- 3. 輔助管理欄位 (選填，方便追蹤與管理)
    is_active TINYINT(1) DEFAULT 1 COMMENT '是否啟用中 ( 1: 啟用, 0: 停用 )',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP COMMENT '資料建立時間',
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '最後更新時間',
    
    PRIMARY KEY (company_code),
    INDEX idx_industry_type (industry_type)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='核心主表 - 公司與財報行業別分類路由對應表';