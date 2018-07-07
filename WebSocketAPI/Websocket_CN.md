# AZEX Websocket

## 使用说明


域名：wss://ws.azex.io

**序列化**

**消息请求**：消息请求组成包括**路由，命令，消息体**三个部分

请求消息的前四个字节（int32）为**路由**
请求消息的第五与第六个字节为**命令**（int16）


**消息接收**：服务器返回的消息由**命令，消息体**两个部分组成

接收的消息前两个字节为**命令**（int16）

**消息体**的序列化通过[protobuf](https://developers.google.com/protocol-buffers/ "protobuf")

[websocket-protobuf](/WebSocketAPI/protobuf.json "websocket-protobuf")

**protobuf的序列化字节序为小端字节序**

例
```
//订阅市场深度
message SubMarketDepth{
    //市场
    required string Market= 1 ;
    //小数位精度
    required int32 Precision= 2 ;
    //获取条数，可选值为：10,20,40
    required int32 Limit= 3 ;
}
message MarketDepthList{
    //市场深度列表
    repeated MarketDepthDto List= 1 ;
}
//市场深度快照
message MarketDepthDto{
    //市场
    required string Market= 1 ;
    //精度
    required int32 Precision= 2 ;
    //卖单列表
    repeated MarketDepth AskList= 3 ;
    //买单列表
    repeated MarketDepth BidList= 4 ;
    //版本号
    required int64 Version= 5 ;
}
```
发送代码（伪代码）如下

```
websocket ws;
protobuf protbuf;
int16 SendCommand = 906; //命令：订阅市场深度
int32 route = 1; //路由
SubMarketDepth subMarket;
subMarket.Market = "eth_btc";
subMarket.Precision = 8;
subMarket.Limit = 10;

ws.send(byteof(route)+byteof(SendCommand)+protbuf.Serialize(subMarket));
```

接收代码（伪代码）如下
```
OnWebsocketReceived（byte[] data）{

	int16 receiveCommand = ConvertToInt16（data.sub(0,2))
	switch(receiveCommand)
	case 1003
		MarketDepthList dto = protbuf.Deserialize(data.sub(2,data.length))
	//业务代码
	...
}
```

----
### 功能列表

功能         | 提交命令                       | 接收命令                                     | 用处           | 备注
---------- | -------------------------- | ---------------------------------------- | ------------ | -------------------------------------------------------------------------
订阅成交记录     | 907       | 1005                       | 最近成交         |
获取批量K线数据(可订阅)   | 900           | 1000,1001 , 1002      | K线图          | 1000单条最新k线，1001部分历史k线，1002批量k线数据推送完成
订阅最新单条K线数据   | 902 | 1000                              | K线图          | Frequencys 可选值 "1", "5", "15", "30", "60", "180", "360", "720", "D", "7D"
订阅24小时滚动行情 | 902 | 1006                           | 币币交易-市场行情、首页 | Frequencys 为 SD1
订阅市场深度     | 906             | 1003，1004            | 币币交易-最新价格    |1003深度快照，1004深度差异数据
订阅个人订单消息     | 1000                          | 1008，1009，1010 | 更新 币币交易-当前委托 |1008订单创建，1009订单更新，1010计划单触发
错误信息| |0|错误信息|



## API参考
### 行情API

订阅成交记录


**请求消息**

**路由：1**
**命令：907**
请求参数：

```
message GetTopTradeList{
    //市场
    required string Market= 1 ;
    //数量
    required int32 Count= 2 ;
    //已订阅
    required bool Subscribe= 3 ;
}
```
参数说明

|参数名|类型|说明|必填|protobuf序列|
|:-----  |:-----|----- |-------|----|
|market |string   |市场交易对  |是 |1|
|Count |string   | 订阅数量（最大99条） |是 |2|
|subscribe |bool   |是否订阅  |是 |3|


**接收消息**

**命令：1005**
```
message TradeSimpleDtoList{
    //交易数据
    repeated TradeSimpleData List= 1 ;
}
//成交信息
message TradeSimpleData{
    //市场
    required string Market= 1 ;
    //成交Id
    required int64 Id= 2 ;
    //成交价
    required double Price= 3 ;
    //成交量
    required double Volume= 4 ;
    //成交额
    required double Amount= 5 ;
    //涨跌类型
    required int32 Trend= 6 ;
    //成交时间
    required int64 CreateTime= 7 ;
}
```
|参数名|类型|说明|protobuf序列|
|:-----  |:-----|-----|----|
|market |string   |市场交易对  |1
|Id |string   | |2|
|Price |double   |成交价 |3|
|Volume |double   |成交量  |4|
|Amount |double   |成交金额  |5|
|Trend |int   |涨跌类型（1上涨2下跌3持平）  |6|
|CreateTime |long   |成交时间  |7|


----
请求历史k线
**请求消息**

**路由：1**
**命令：900**
请求参数：

```
message GetKLineList{
    //市场
    required string Market= 1 ;
    //K线类型
    required string Frequency= 2 ;
    //开始时间
    required int64 Start= 3 ;
    //结束时间
    required int64 End= 4 ;
    //已订阅
    required bool Subscribe= 5 [default = true];
}
```
参数说明

|参数名|类型|说明|必填|protobuf序列|
|:-----  |:-----|----- |-------|----|
|market |string   |市场交易对  |是 |1
|frequency |string   |Frequencys 可选值 “1”, “5”, “15”, “30”, “60”, “180”, “360”, “720”, “D”, “7D”  |是 |2|
|start |long   |查询开始时间  |是 |3|
|end |long   |查询结束时间  |是 |4|
|subscribe |bool   | 请求完之后是否订阅k线  |否 |5|

**接收消息**

**命令：1001**

```
message WsKLineList{
    //K线数据
    repeated WsKLine List= 1 ;
}

message WsKLine{
    //市场
    required string Market= 1 ;
    //K线类型
    required string Frequency= 2 ;
    //成交量
    required double Volume= 3 ;
    //开盘价
    required double OpenPrice= 4 ;
    //收盘价
    required double ClosedPrice= 5 ;
    //最低价
    required double LowPrice= 6 ;
    //最高价
    required double HighPrice= 7 ;
    //开盘时间
    required int64 OpenTime= 8 ;
}
```
参数说明

|参数名|类型|说明|protobuf序列|
|:-----  |:-----|-----|----|
|market |string   |市场交易对  |1
|frequency |string   |Frequencys 可选值 “1”, “5”, “15”, “30”, “60”, “180”, “360”, “720”, “D”, “7D”  |2|
|volume |double   |成交量 |3|
|openPrice |double   |开盘价  |4|
|closedPrice |double   |收盘价  |5|
|lowPrice |double   |最低价  |6|
|highPrice |double   |最高价  |7|
|openTime |long   |开盘时间  |8|

**命令：1002** //数据接收完成
```
message BatchSendComplate{
    //市场
    required string Market= 1 ;
    //K线类型
    required string Frequency= 2 ;
    //
    required int64 Start= 3 ;
    //
    required int64 End= 4 ;
    //是否到达K线的起点位置
    required bool IsStart= 5 ;
}
```
参数说明

|参数名|类型|说明|protobuf序列|
|:-----  |:-----|-----|----|
|market |string   |市场交易对 |1
|frequency |string   |Frequencys 值 “1”, “5”, “15”, “30”, “60”, “180”, “360”, “720”, “D”, “7D”  |2|
|start |long   |查询开始时间  |3|
|end |long   |查询结束时间  |4|
|isStart |bool   |是否已取完所有历史k线  |5|


----
订阅k线

**请求消息**

**路由：1**
**命令：902**
请求参数：

```
message SubKLine{
    //订阅细节
    repeated SubKLineItem Items= 1 ;
}
//K线订阅参数
message SubKLineItem{
    //市场
    required string Market= 1 ;
    //K线类型
    repeated string Frequencys= 2 ;
}
```
参数说明

|参数名|类型|说明|必填|protobuf序列|
|:-----  |:-----|----- |-------|----|
|market |string   |市场交易对  |是 |1
|frequency |string   |Frequencys 可选值 “1”, “5”, “15”, “30”, “60”, “180”, “360”, “720”, “D”, “7D”  |是 |2|


**接收消息**

**命令：1000**
```
message WsKLine{
    //市场
    required string Market= 1 ;
    //K线类型
    required string Frequency= 2 ;
    //成交量
    required double Volume= 3 ;
    //开盘价
    required double OpenPrice= 4 ;
    //收盘价
    required double ClosedPrice= 5 ;
    //最低价
    required double LowPrice= 6 ;
    //最高价
    required double HighPrice= 7 ;
    //开盘时间
    required int64 OpenTime= 8 ;
}
```
|参数名|类型|说明|protobuf序列|
|:-----  |:-----|-----|----|
|market |string   |市场交易对  |1
|frequency |string   |Frequencys 可选值 “1”, “5”, “15”, “30”, “60”, “180”, “360”, “720”, “D”, “7D”  |2|
|volume |double   |成交量 |3|
|openPrice |double   |开盘价  |4|
|closedPrice |double   |收盘价  |5|
|lowPrice |double   |最低价  |6|
|highPrice |double   |最高价  |7|
|openTime |long   |开盘时间  |8|

----

订阅24小时滚动行情

**请求消息**

**路由：1**
**命令：902**
请求参数：

```
message SubKLine{
    //订阅细节
    repeated SubKLineItem Items= 1 ;
}
//K线订阅参数
message SubKLineItem{
    //市场
    required string Market= 1 ;
    //K线类型
    repeated string Frequencys= 2 ;
}
```
参数说明

|参数名|类型|说明|必填|protobuf序列|
|:-----  |:-----|----- |-------|----|
|market |string   |市场交易对  |是 |1
|frequency |string   |Frequencys的值只能为SD1（为了与正常k线区分） |是 |2|


**接收消息**

**命令：1000**
```
message WsKLine{
    //市场
    required string Market= 1 ;
    //K线类型
    required string Frequency= 2 ;
    //成交量
    required double Volume= 3 ;
    //开盘价
    required double OpenPrice= 4 ;
    //收盘价
    required double ClosedPrice= 5 ;
    //最低价
    required double LowPrice= 6 ;
    //最高价
    required double HighPrice= 7 ;
    //开盘时间
    required int64 OpenTime= 8 ;
}
```
|参数名|类型|说明|protobuf序列|
|:-----  |:-----|-----|----|
|market |string   |市场交易对  |1
|frequency |string   |Frequencys 可选值 “1”, “5”, “15”, “30”, “60”, “180”, “360”, “720”, “D”, “7D”  |2|
|volume |double   |成交量 |3|
|openPrice |double   |开盘价  |4|
|closedPrice |double   |收盘价  |5|
|lowPrice |double   |最低价  |6|
|highPrice |double   |最高价  |7|
|openTime |long   |开盘时间  |8|


----

订阅市场深度

深度订阅为差异数据推送，第一次订阅会返回全量数据，之后只推送差异数据

差异比较方法：1.新出现的价格，表示新增深度。2.已出现的价格，挂单数不为0，表示更新.3.已出现的价格，挂单数为0，表示删除此深度。=====若StartVersion和市场深度快照的版本不同，需要重新获取市场深度快照

**请求消息**

**路由：1**
**命令：906**
请求参数：

```
message SubMarketDepth{
    //市场
    required string Market= 1 ;
    //小数位精度
    required int32 Precision= 2 ;
    //获取条数，可选值为：10,20,40
    required int32 Limit= 3 ;
}
```
参数说明

|参数名|类型|说明|必填|protobuf序列|
|:-----  |:-----|----- |-------|----|
|market |string   |市场交易对  |是 |1
|precision |int   |小数位精度 |是 |2|
|limit |int   |获取条数，可选值为：10,20,40 |是 |3|


**接收消息**

**命令：1003** //深度快照，第一次订阅时会推送一次
```
message MarketDepthList{
    //市场深度列表
    repeated MarketDepthDto List= 1 ;
}
//市场深度快照
message MarketDepthDto{
    //市场
    required string Market= 1 ;
    //精度
    required int32 Precision= 2 ;
    //卖单列表
    repeated MarketDepth AskList= 3 ;
    //买单列表
    repeated MarketDepth BidList= 4 ;
    //版本号
    required int64 Version= 5 ;
}
message MarketDepth{
    //单价
    required double P= 1 ;
    //数量
    required double V= 2 ;
}
```
|参数名|类型|说明|protobuf序列|
|:-----  |:-----|-----|---|
|market |string   |市场交易对  |1
|precision |string   |市场精度  |2
|askList |array   |单价 |3
|bidList |array   |数量  |4
|version |long   |版本号  |5
|p |double   |单价  |1
|v |double   |数量  |2


**命令：1004** //深度差异

```
//市场深度差异信息。差异比较方法：1.新出现的价格，表示新增深度。2.已出现的价格，挂单数不为0，表示更新.3.已出现的价格，挂单数为0，表示删除此深度。=====若StartVersion和市场深度快照的版本不同，需要重新获取市场深度快照
message MarketDepthDiff{
    //市场
    required string Market= 1 ;
    //精度
    required int32 Precision= 2 ;
    //卖单差异列表
    repeated MarketDepth AskList= 3 ;
    //买单差异列表
    repeated MarketDepth BidList= 4 ;
    //起始版本号
    required int64 StartVersion= 5 ;
    //截止版本号
    required int64 EndVersion= 6 ;
}
message MarketDepth{
    //单价
    required double P= 1 ;
    //数量
    required double V= 2 ;
}
```


|参数名|类型|说明|protobuf序列|
|:-----  |:-----|-----|----|
|market |string   |市场交易对  |1
|precision |string   |市场精度  |2|
|askList |array   |卖盘 |3|
|bidList |array   |买盘  |4|
|startVersion |double   |起始版本号  |5|
|endVersion |double   |当前版本号  |6|
|p |double   |价格  |1|
|v |double   |数量  |2|

----

### 个人API

私人api每次请求都需要构建一个签名参数sign与一个Authorization的参数


### 签名

获取api：请在账号设置->Api管理->创建api，创建完成之后会生成一个Apikey和Secrect Key,请妥善管理， **重要提示：这两个密钥与账号安全紧密相关，无论何时都请勿向其他人透露。**

**Apikey用来标识用户，在ws连接后加上Authorization=Apikey参数**


**Secrect Key用来生成sign参数**

**sign 生成方法:** 使用Secrect Key作为密钥，Authorization=Apikey为值进行 HmacSHA256 计算得出

例

APIKey=81.67AAA2F6041D408D9868387A8904431D，Authorization参数如下

```
Authorization=81.67AAA2F6041D408D9868387A8904431D
```

如Secrect Key为 2288987EFDB54F848D7BACCE1288FC9A，则计算得出sign值为
```
57c4c6770d565aa236f87706053bd51512862443062e471bd3243a6ed8eef2
```
最终生成的ws地址：
wss://ws.azex.io?Authorization=81.67AAA2F6041D408D9868387A8904431D&sign=57c4c6770d565aa236f87706053bd51512862443062e471bd3243a6ed8eef2


----
订阅个人订单（订单创建消息，订单更新消息，计划单触发消息）

**请求消息**

**路由：1**
**命令：1000**
请求参数：

```
//登录市场
message LoginToMarket{
    //市场
    required string Market= 1 ;
}
```
参数说明

|参数名|类型|说明|必填|protobuf序列|
|:-----  |:-----|----- |-------|----|
|market |string   |市场交易对  |是 |1



**接收消息**

**命令：1008** //新增订单消息
```
//订单信息
message OrderInfoDto{
    //订单Id
    required string Id= 1 ;
    //用户Id
    required int64 UserId= 2 ;
    //币种
    required string Currency= 3 ;
    //手续费币种
    required string FeeCurrency= 4 ;
    //市场
    required string Market= 5 ;
    //订单分类
    required int32 Category= 6 ;
    //买卖类别
    required int32 OrderType= 7 ;
    //计划单类别
    required int32 PlanType= 8 ;
    //触发价格
    required double TriggerPrice= 9 ;
    //限价单单价
    required double Price= 10 ;
    //限价单数量
    required double Volume= 11 ;
    //市价单金额
    required double Amount= 12 ;
    //订单状态
    required int32 Status= 13 ;
    //创建时间
    required int64 CreateTime= 14 ;
}

```
参数说明

|参数名|类型|说明|protobuf序列|
|:-----  |:-----|-----|----|
|id |string   |订单id  |1
|userId |long   |用户id  |2|
|currency |string   |币种 |3|
|feeCurrency |string   |手续费币种  |4|
|market |string   |市场  |5|
|category |int   |订单分类（1非计划订单，2计划订单）  |6|
|orderType |int   |订单类型（1限价买单2市价买单3限价卖单4市价卖单）  |7|
|planType |int   |计划单类别（1高价触发2低价触发）  |8|
|triggerPrice |double   |触发价  |9|
|price |double   |挂单价  |10|
|volume |double   |挂单数量  |11|
|amount |double   |挂单金额  |12|
|status |int   |订单状态  |13|
|createTime |long   |创建时间  |14|

**命令：1009** //更新订单消息
```
//更新订单信息
message UpdateOrderInfo{
    //最高价
    required string Market= 1 ;
    //订单Id
    required string OrderId= 2 ;
    //交易量
    required double TxVolume= 3 ;
    //交易额
    required double TxAmount= 4 ;
    //订单状态
    required int32 Status= 5 ;
    //更新时间
    required int64 UpdateTime= 6 ;
}

```
参数说明

|参数名|类型|说明|protobuf序列|
|:-----  |:-----|-----|----|
|market |string   |市场交易对  |1
|orderId |string   |订单id  |2|
|txVolume |double   |交易量 |3|
|txAmount |double   |交易金额  |4|
|status |int   |订单状态（2交易中3订单已完成4已取消）  |5|
|updateTime |long   |更新时间  |6|

**命令：1010** //计划单触发消息
```
message PlanOrderTrigger{
    //市场
    required string Market= 1 ;
    //计划订单ID
    required string Id= 2 ;
    //触发价格
    required double Price= 3 ;
    //用户Id
    required int64 UserId= 4 ;
}

```
参数说明

|参数名|类型|说明|protobuf序列|
|:-----  |:-----|-----|----|
|market |string   |市场交易对  |1
|id |string   |计划单id |2|
|price |double   |触发价格 |3|
|userId |double   |用户id  |4|

----

错误信息消息

**接收消息**
**命令：0**
```
message WsError{
    //错误码
    required int32 Code= 1 ;
    //错误提示
    required string Message= 2 ;
}
```


