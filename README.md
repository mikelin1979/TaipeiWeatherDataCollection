# TaipeiWeatherDataCollection
基於PWEB的天氣資料收集程式

由於中央氣象局PWEB API沒有歷史資料查詢，每次啟動將會檢查是否有7天後的新資料

如果資料庫已有新資料，則不會向PWEB查詢以免拖慢效能
