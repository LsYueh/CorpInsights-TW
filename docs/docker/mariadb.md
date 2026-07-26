# 啟動 MariaDB 11.8. 
``` bash
docker compose up -d
```

<br>

# 啟動後測試 (驗證時區設定)
容器啟動後，執行以下指令驗證使`用者`、`密碼`、`時區`是否成功設定完成

``` bash
docker compose exec mariadb mariadb -u cis_user -pcis_password cis_db -e "SELECT NOW(), @@system_time_zone, @@time_zone;"
```

<br>

應輸出顯示：
``` txt
+---------------------+--------------------+-------------+
| NOW()               | @@system_time_zone | @@time_zone |
+---------------------+--------------------+-------------+
| 20XX-XX-XX hh:MM:dd | CST                | SYSTEM      |
+---------------------+--------------------+-------------+
```

請檢查 MariaDB 時間是否與台灣時間一致。
