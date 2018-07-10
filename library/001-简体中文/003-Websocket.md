# AZEX Websocket

开发者可以通过该文档，了解如何通过Websocket，实现订阅行情数据和用户个人订单变化。

## 总体说明

### 链接地址

Websocket 的链接地址为：**wss://ws.azex.io**

### 序列化

#### 消息请求

消息请求组成包括**路由，命令，消息体**三个部分。

请求消息的前四个字节（int32）为**路由** 请求消息的第五与第六个字节为**命令**（int16）。

#### 消息接收

服务器返回的消息由**命令，消息体**两个部分组成

接收的消息前两个字节为**命令**（int16）

**消息体** 的序列化通过[protobuf](https://developers.google.com/protocol-buffers/ "protobuf")进行实现。关于本Websocket的protobuf定义文件，可以通过右侧链接获取：[websocket-protobuf.json](protobuf.json "websocket-protobuf")

**protobuf的序列化字节序为小端字节序**

### 伪代码样例

以"订阅市场深度"为例，**消息体** 的 protobuf 定义如下：

```protobuf
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

发送代码（伪代码）如下：

```javascript
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

接收代码（伪代码）如下：

```
function OnWebsocketReceived (byte[] data) {
    int16 receiveCommand = ConvertToInt16（data.sub(0,2))
    switch(receiveCommand)
    case 1003:
        MarketDepthList dto = protbuf.Deserialize(data.sub(2,data.length));
    // 业务代码
    break;
}
```

### 市场Id

市场Id由两部分组成，基本币和目标币，中间采用下划线分隔。例如 btc_usdt 即表示 基本币为 usdt 目标币为 btc 。

### 时间格式

以下 API 中涉及到时间的作为传入或传出参数的位置，时间格式统一为unix时间戳秒数。

例如：北京时间 2018/7/9 15:25:13 对应的 unix时间戳秒数为 1531121113 。

开发者可以通过搜索引擎或右侧链接，查询相应开发语言的处理方法：<https://www.epochconverter.com/>

## 功能列表

功能            | 提交命令 | 接收命令             | 用处           | 备注
------------- | ---- | ---------------- | ------------ | -----------------------------------------------------------------------------------------------------
订阅成交记录        | 907  | 1005             | 最近成交         |
获取批量K线数据(可订阅) | 900  | 1000,1001 , 1002 | K线图          | 1000单条最新k线，1001部分历史k线，1002批量k线数据推送完成
订阅最新单条K线数据    | 902  | 1000             | K线图          | Frequencys 可选值 "1", "5", "15", "30", "60", "180", "360", "720", "D", "7D"。分别表示 1分钟、5分钟...720分钟、1天、7天。
订阅24小时滚动行情    | 902  | 1006             | 币币交易-市场行情、首页 | Frequencys 为 SD1
订阅市场深度        | 906  | 1003，1004        | 币币交易-最新价格    | 1003深度快照，1004深度差异数据
订阅个人订单消息      | 1000 | 1008，1009，1010   | 更新 币币交易-当前委托 | 1008订单创建，1009订单更新，1010计划单触发
错误信息          |      | 0                | 错误信息         |

## 公共API

### 订阅成交记录

#### 请求消息

**路由：1** **命令：907**

```protobuf
message GetTopTradeList{
    //市场
    required string Market= 1 ;
    //数量
    required int32 Count= 2 ;
    //已订阅
    required bool Subscribe= 3 ;
}
```

参数名       | 类型     | 说明          | 必填 | protobuf序列
:-------- | :----- | ----------- | -- | ----------
market    | string | 市场交易对       | 是  | 1
Count     | string | 订阅数量（最大99条） | 是  | 2
subscribe | bool   | 是否订阅        | 是  | 3

#### 接收消息

**命令：1005**

```protobuf
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

参数名        | 类型     | 说明              | protobuf序列
:--------- | :----- | --------------- | ----------
market     | string | 市场交易对           | 1
Id         | string |                 | 2
Price      | double | 成交价             | 3
Volume     | double | 成交量             | 4
Amount     | double | 成交金额            | 5
Trend      | int    | 涨跌类型（1上涨2下跌3持平） | 6
CreateTime | long   | 成交时间            | 7

### 请求历史k线

历史K线通过批量的方式发送，会依次批量发送若干次K线数据，并在最后发送依次发送完成的结果。

#### 请求消息

**路由：1** **命令：900**

```protobuf
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

参数名       | 类型     | 说明                                                                                                    | 必填 | protobuf序列
:-------- | :----- | ----------------------------------------------------------------------------------------------------- | -- | ----------
market    | string | 市场交易对                                                                                                 | 是  | 1
frequency | string | Frequencys 可选值 "1", "5", "15", "30", "60", "180", "360", "720", "D", "7D"。分别表示 1分钟、5分钟...720分钟、1天、7天。 | 是  | 2
start     | long   | 查询开始时间                                                                                                | 是  | 3
end       | long   | 查询结束时间                                                                                                | 是  | 4
subscribe | bool   | 请求完之后是否订阅k线                                                                                           | 否  | 5

#### 接收消息

**命令：1001** //批量K线数据

```protobuf
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

参数名         | 类型     | 说明                                                                                                    | protobuf序列
:---------- | :----- | ----------------------------------------------------------------------------------------------------- | ----------
market      | string | 市场交易对                                                                                                 | 1
frequency   | string | Frequencys 可选值 "1", "5", "15", "30", "60", "180", "360", "720", "D", "7D"。分别表示 1分钟、5分钟...720分钟、1天、7天。 | 2
volume      | double | 成交量                                                                                                   | 3
openPrice   | double | 开盘价                                                                                                   | 4
closedPrice | double | 收盘价                                                                                                   | 5
lowPrice    | double | 最低价                                                                                                   | 6
highPrice   | double | 最高价                                                                                                   | 7
openTime    | long   | 开盘时间                                                                                                  | 8

**命令：1002** //数据接收完成

```protobuf
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

参数名       | 类型     | 说明                                                                                                  | protobuf序列
:-------- | :----- | --------------------------------------------------------------------------------------------------- | ----------
market    | string | 市场交易对                                                                                               | 1
frequency | string | Frequencys 值 "1", "5", "15", "30", "60", "180", "360", "720", "D", "7D"。分别表示 1分钟、5分钟...720分钟、1天、7天。 | 2
start     | long   | 查询开始时间                                                                                              | 3
end       | long   | 查询结束时间                                                                                              | 4
isStart   | bool   | 是否已取完所有历史k线                                                                                         | 5

### 订阅k线

#### 请求消息

**路由：1** **命令：902**

```protobuf
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

参数名        | 类型     | 说明                                                                                                    | 必填 | protobuf序列
:--------- | :----- | ----------------------------------------------------------------------------------------------------- | -- | ----------
market     | string | 市场交易对                                                                                                 | 是  | 1
frequencys | string | Frequencys 可选值 "1", "5", "15", "30", "60", "180", "360", "720", "D", "7D"。分别表示 1分钟、5分钟...720分钟、1天、7天。 | 是  | 2

#### 接收消息

**命令：1000**

```protobuf
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

参数名         | 类型     | 说明                                                                                                    | protobuf序列
:---------- | :----- | ----------------------------------------------------------------------------------------------------- | ----------
market      | string | 市场交易对                                                                                                 | 1
frequency   | string | Frequencys 可选值 "1", "5", "15", "30", "60", "180", "360", "720", "D", "7D"。分别表示 1分钟、5分钟...720分钟、1天、7天。 | 2
volume      | double | 成交量                                                                                                   | 3
openPrice   | double | 开盘价                                                                                                   | 4
closedPrice | double | 收盘价                                                                                                   | 5
lowPrice    | double | 最低价                                                                                                   | 6
highPrice   | double | 最高价                                                                                                   | 7
openTime    | long   | 开盘时间                                                                                                  | 8

### 订阅24小时滚动行情

#### 请求消息

**路由：1** **命令：902**

```protobuf
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

参数名        | 类型     | 说明                            | 必填 | protobuf序列
:--------- | :----- | ----------------------------- | -- | ----------
market     | string | 市场交易对                         | 是  | 1
frequencys | string | Frequencys的值只能为SD1（为了与正常k线区分） | 是  | 2

#### 接收消息

**命令：1006**

```protobuf
message WsKLine{
    //市场
    required string Market= 1 ;
    //开盘价
    required double OpenPrice= 2 ;
    //收盘价
    required double ClosedPrice= 3 ;
    //最低价
    required double LowPrice= 4 ;
    //最高价
    required double HighPrice= 5 ;
    //成交量
    required double Volume= 6 ;
}
```

参数名         | 类型     | 说明    | protobuf序列
:---------- | :----- | ----- | ----------
market      | string | 市场交易对 | 1
openPrice   | double | 开盘价   | 2
closedPrice | double | 收盘价   | 3
lowPrice    | double | 最低价   | 4
highPrice   | double | 最高价   | 5
volume      | double | 成交量   | 6

### 订阅市场深度

深度数据的推送采用的是增量式的推送。

订阅深度之后会推送依次全量的数据，之后便会推送增量的数据。

#### 深度增量更新方法

深度增量更新主要分为"数据更新"和"版本比对"两个部分。

##### 数据更新

为了说明方便，现假设第一次推送获得的市场深度如下：

买量  | 买价 | 卖价  | 卖量
--- | -- | --- | ---
100 | 50 | 90  | 600
90  | 40 | 100 | 500
80  | 30 | 110 | 400

以下分别说明深度变化的三种情况

###### 新增

若深度增量数据为

卖价 | 卖量
-- | ---
80 | 600

此价格在原来的深度之中没有，则属于新增的深度，因此，深度应更新为：

买量  | 买价 | 卖价  | 卖量
--- | -- | --- | ---
100 | 50 | 80  | 600
90  | 40 | 90  | 600
80  | 30 | 100 | 500
    |    | 110 | 400

###### 更新

若深度增量数据为

卖价 | 卖量
-- | ---
90 | 233

此价格在原来的深度存在，则属于更新对应价格的数据，因此，深度应更新为：

买量  | 买价 | 卖价  | 卖量
--- | -- | --- | ---
100 | 50 | 90  | 233
90  | 40 | 100 | 500
80  | 30 | 110 | 400

###### 删除

若深度增量数据为

卖价 | 卖量
-- | --
90 | 0

此价格在原来的深度存在，并且数量已经为0，则删除此深度数据，因此，深度应更新为：

买量  | 买价 | 卖价  | 卖量
--- | -- | --- | ---
100 | 50 | 100 | 500
90  | 40 | 110 | 400
80  | 30 |     |

##### 版本比对

深度快照和深度增量都会返回相应的版本信息，开发者需要在获取版本之后进行版本比对，确保深度的数据更新的正确性。分为两类情况：

###### 快照与增量的对比

深度快照数据中包含 Version 字段。

第一条增量的数据中包含 StartVersion 和 EndVersion 两个字段。

需要比对深度快照数据的 Version 和 StartVersion 字段。

若两个字段不相等，则说明增量存在丢失情况，需要开发者重新订阅增量数据。

###### 增量与增量的对比

增量数据中包含 StartVersion 和 EndVersion 两个字段。

前一次增量的 EndVersion 字段需要与后一次的 StartVersion 字段的值相同。

若两个字段不相等，则说明增量存在丢失情况，需要开发者重新订阅增量数据。

#### 请求消息

**路由：1** **命令：906**

```protobuf
message SubMarketDepth{
    //市场
    required string Market= 1 ;
    //小数位精度
    required int32 Precision= 2 ;
    //获取条数，可选值为：10,20,40
    required int32 Limit= 3 ;
}
```

参数名       | 类型     | 说明                 | 必填 | protobuf序列
:-------- | :----- | ------------------ | -- | ----------
market    | string | 市场交易对              | 是  | 1
precision | int    | 价格小数位精度            | 是  | 2
limit     | int    | 获取条数，可选值为：10,20,40 | 是  | 3

#### 接收消息

**命令：1003** //深度快照，第一次订阅时会推送一次

```protobuf
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

参数名       | 类型     | 说明    | protobuf序列
:-------- | :----- | ----- | ----------
market    | string | 市场交易对 | 1
precision | string | 市场精度  | 2
askList   | array  | 单价    | 3
bidList   | array  | 数量    | 4
version   | long   | 版本号   | 5
p         | double | 单价    | 1
v         | double | 数量    | 2

**命令：1004** //深度差异

```protobuf
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

参数名          | 类型     | 说明    | protobuf序列
:----------- | :----- | ----- | ----------
market       | string | 市场交易对 | 1
precision    | string | 市场精度  | 2
askList      | array  | 卖盘    | 3
bidList      | array  | 买盘    | 4
startVersion | double | 起始版本号 | 5
endVersion   | double | 当前版本号 | 6
p            | double | 价格    | 1
v            | double | 数量    | 2

### 错误信息消息

#### 请求消息

无

#### 接收消息

**命令：0**

```protobuf
message WsError{
    //错误码
    required int32 Code= 1 ;
    //错误提示
    required string Message= 2 ;
}
```

## 个人API

调用用户API需要身份认证信息才能调用。请先参看[OpenApi权限开通与令牌使用](?file=001-简体中文/001-OpenApi权限开通与使用 "OpenApi权限开通与使用")

### 订阅个人订单变更

#### 请求消息

**路由：1** **命令：1000**

```protobuf
//登录市场
message LoginToMarket{
    //市场
    required string Market= 1 ;
}
```

参数名    | 类型     | 说明    | 必填 | protobuf序列
:----- | :----- | ----- | -- | ----------
market | string | 市场交易对 | 是  | 1

#### 接收消息

**命令：1008** //新增订单消息

```protobuf
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

参数名          | 类型     | 说明                         | protobuf序列
:----------- | :----- | -------------------------- | ----------
id           | string | 订单id                       | 1
userId       | long   | 用户id                       | 2
currency     | string | 币种                         | 3
feeCurrency  | string | 手续费币种                      | 4
market       | string | 市场                         | 5
category     | int    | 订单分类（1非计划订单，2计划订单）         | 6
orderType    | int    | 订单类型（1限价买单2市价买单3限价卖单4市价卖单） | 7
planType     | int    | 计划单类别（1高价触发2低价触发）          | 8
triggerPrice | double | 触发价                        | 9
price        | double | 挂单价                        | 10
volume       | double | 挂单数量                       | 11
amount       | double | 挂单金额                       | 12
status       | int    | 订单状态                       | 13
createTime   | long   | 创建时间                       | 14

**命令：1009** //更新订单消息

```protobuf
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

参数名        | 类型     | 说明                   | protobuf序列
:--------- | :----- | -------------------- | ----------
market     | string | 市场交易对                | 1
orderId    | string | 订单id                 | 2
txVolume   | double | 交易量                  | 3
txAmount   | double | 交易金额                 | 4
status     | int    | 订单状态（2交易中3订单已完成4已取消） | 5
updateTime | long   | 更新时间                 | 6

**命令：1010** //计划单触发消息

```protobuf
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

参数名    | 类型     | 说明    | protobuf序列
:----- | :----- | ----- | ----------
market | string | 市场交易对 | 1
id     | string | 计划单id | 2
price  | double | 触发价格  | 3
userId | double | 用户id  | 4
