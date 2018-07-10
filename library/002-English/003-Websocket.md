# AZEX Websocket

> This article is translated by Bing and, if there is a problem, welcome feedback via the right link:<https://github.com/AZEXIO/APIDocument/issues>

Developers can use this document to learn how to implement subscription data and individual order changes through WebSocket.

## General Description

### Link Address

Websocket's link address is: **wss://ws.azex.io**

### Message Structure

#### Request Message

Request message consist of **router**,**command**,**body** three parts.

The first four bytes of the request is **router**, and the fifth to sixth byte of the request is **command**.

The remaining part is **body**.

#### Received Message

The message returned by AZEX is composed of the **command**,**body** two parts.

The first four bytes of the request is **command**, and the remaining part is **body**.

#### Message Body Serialization

Serialization of **body** is implement by [protobuf](https://developers.google.com/protocol-buffers/ "protobuf").

The protobuf definition file for this websocket can be obtained from the right link: [websocket-protobuf.json](protobuf_en.json "websocket-protobuf")

**The serialized byte order of the protobuf is a small-end byte order**

### Pseudo Code Examples

For example, "subscribe market depth ", **body** is defined as follows

```protobuf
//SendCommand,906| Subscribe market depth
message SubMarketDepth{
    //market
    required string Market= 1 ;
    // precision of price Example: 2 means 2 decimal digits。Need to refer to the price precision of the market. If the price precision is greater than 2, the value range is [priceprecision-4,priceprecision],no] and the range is [0,priceprecision]
    required int32 Precision= 2 ;
    // Get the number of bars, the optional value is: 10, 20, 40
    required int32 Limit= 3 ;
}

//ReceiveCommand,1003| Market depth snapshot
message MarketDepthDto{
    //market
    required string Market= 1 ;
    // precision of price
    required int32 Precision= 2 ;
    //list of ask orders
    repeated MarketDepth AskList= 3 ;
    //list of bid orders
    repeated MarketDepth BidList= 4 ;
    //version number
    required int64 Version= 5 ;
}
// Single market depth
message MarketDepth{
    //price
    required double P= 1 ;
    //volume
    required double V= 2 ;
}
//ReceiveCommand,1004| Market depth difference information.
message MarketDepthDiff{
    //market
    required string Market= 1 ;
    // precision of price
    required int32 Precision= 2 ;
    //ask order difference list
    repeated MarketDepth AskList= 3 ;
    //bid order difference list
    repeated MarketDepth BidList= 4 ;
    // starting version number
    required int64 StartVersion= 5 ;
    // end version number
    required int64 EndVersion= 6 ;
}
// Single market depth
message MarketDepth{
    // price
    required double P= 1 ;
    // volume
    required double V= 2 ;
}
```

Send method (pseudo code) as follows：

```javascript
websocket ws;
protobuf protbuf;
int16 SendCommand = 906; //Command: Subscribe to market depth
int32 route = 1; //Router
SubMarketDepth subMarket;
subMarket.Market = "eth_btc";
subMarket.Precision = 8;
subMarket.Limit = 10;

ws.send(byteof(route)+byteof(SendCommand)+protbuf.Serialize(subMarket));
```

Receive method (pseudo code) as follows：

```
function OnWebsocketReceived (byte[] data) {
    int16 receiveCommand = ConvertToInt16（data.sub(0,2))
    switch(receiveCommand)
    case 1003:
        MarketDepthList dto = protbuf.Deserialize(data.sub(2,data.length));
    // Business code
    break;
}
```

### Market ID

The market ID consists of two parts, a basic currency and a target currency, separated by an underscore in the middle.

For example, btc_usdt means that the basic currency is the usdt target currency btc.

### Time Format

the format of time as an incoming or outgoing parameter refers to the following API is Unix timestamp seconds.

For example:time 2018/7/9 15:25:13 in Beijing is 1531121113 as Unix timestamp seconds.

Developers can use this link on the right to find the method of getting Unix timestamp seconds in some program language:<https://www.epochconverter.com/>

## Features

Feature                                        | SendCommand | ReceiveCommand
---------------------------------------------- | ----------- | ------------------------------------------------------------------------------------------
Subscribe to the transaction record            | 907         | 1005
Get batch K line data (subscribe)              | 900         | 1000 single latest k line, 1001 part history k line, 1002 batch k line data push completed
Subscribe to the latest single K-line data     | 902         | 1000
Subscribe to the 24-hour rolling market quotes | 902         | 1006
Subscribe to Market Depth                      | 906         | 1003 Depth Snapshot, 1004 Depth difference
Subscribe to Individual Order Messages         | 1000        | 1008 Order Creation, 1009 Order Update, 1010 Stop-Limit Order Stopped
Error message                                  |             | 0

## Public API

### Subscribe to the Transaction Record

#### Request Message

**router：1** **command：907**

```protobuf
//SendCommand,907| Get the latest deal list
message GetTopTradeList{
    //market
    required string Market= 1 ;
    //quantity
    required int32 Count= 2 ;
    // subscribed
    required bool Subscribe= 3 ;
}
```

Name      | Type   | Description                                             | Required | protobuf tag
:-------- | :----- | ------------------------------------------------------- | -------- | ------------
market    | string | market                                                  | Yes      | 1
count     | string | number of list at first, range itn [10,99]              | Yes      | 2
subscribe | bool   | if true , you will receive latest deal list from now on | Yes      | 3

#### Received Message

**command：1005**

```protobuf
//ReceiveCommand,1005| Recent Deal List
message TradeSimpleDtoList{
    //Recent deal list
    repeated TradeSimpleData List= 1 ;
}
// Deal information
message TradeSimpleData{
    //market
    required string Market= 1 ;
    //Transaction Id
    required int64 Id= 2 ;
    // deal price
    required double Price= 3 ;
    // volume
    required double Volume= 4 ;
    // amount
    required double Amount= 5 ;
    // price trend
    required int32 Trend= 6 ;
    // deal time
    required int64 CreateTime= 7 ;
}
```

Name       | Type   | Description                     | protobuf tag
:--------- | :----- | ------------------------------- | ------------
market     | string | market id                       | 1
Id         | string | transaction Id                  | 2
Price      | double | deal price                      | 3
Volume     | double | deal volume                     | 4
Amount     | double | deal amount                     | 5
Trend      | int    | price trend（1:up,2:down,3:flat） | 6
CreateTime | long   | deal time                       | 7

### Get batch K line data (subscribe)

The history of the K-line through the bulk of the way to send, will be in order to send a number of times the K-line data, and sent in the final send the results.

#### Request Message

**router：1** **command：900**

```protobuf
//SendCommand,900| Subscribe to the market batch K line
message GetKLineList{
    //market
    required string Market= 1 ;
    //K line type
    required string Frequency= 2 ;
    //Starting time
    required int64 Start= 3 ;
    //End Time
    required int64 End= 4 ;
    //Do you subscribe?
    required bool Subscribe= 5 [default = true];
}
```

参数Description

Name      | Type   | Description                                                                                                                                        | Required | protobuf tag
:-------- | :----- | -------------------------------------------------------------------------------------------------------------------------------------------------- | -------- | ------------
market    | string | market id                                                                                                                                          | Yes      | 1
frequency | string | Frequency, Optional values: "1", "5", "15", "30", "60", "180", "360", "720", "D", "7D". It means 1 minute, 5 minutes...720 minutes, 1 day, 7 days. | Yes      | 2
start     | long   | start time                                                                                                                                         | Yes      | 3
end       | long   | end time                                                                                                                                           | Yes      | 4
subscribe | bool   | subscribe to lastest single K line from now on                                                                                                     | No       | 5

#### Received Message

**command：1001**

```protobuf
//ReceiveCommand,1001| Batch K line data
message WsKLineList{
    //K line data
    repeated WsKLine List= 1 ;
}
// K line data
message WsKLine{
    //market
    required string Market= 1 ;
    //K line type
    required string Frequency= 2 ;
    // volume
    required double Volume= 3 ;
    //Opening price
    required double OpenPrice= 4 ;
    //Closing price
    required double ClosedPrice= 5 ;
    //lowest price
    required double LowPrice= 6 ;
    //highest price
    required double HighPrice= 7 ;
    //Opening time
    required int64 OpenTime= 8 ;
}
```

Name        | Type   | Description                                                                                                                                        | protobuf tag
----------- | ------ | -------------------------------------------------------------------------------------------------------------------------------------------------- | ------------
market      | string | market id                                                                                                                                          | 1
frequency   | string | Frequency, Optional values: "1", "5", "15", "30", "60", "180", "360", "720", "D", "7D". It means 1 minute, 5 minutes...720 minutes, 1 day, 7 days. | 2
volume      | double | deal volume                                                                                                                                        | 3
openPrice   | double | open price                                                                                                                                         | 4
closedPrice | double | last price                                                                                                                                         | 5
lowPrice    | double | low price                                                                                                                                          | 6
highPrice   | double | high price                                                                                                                                         | 7
openTime    | long   | open time                                                                                                                                          | 8

**command：1002**

```protobuf
//ReceiveCommand, 1002| K line data batch transmission completed
message BatchSendComplate{
    //market
    required string Market= 1 ;
    //K line type
    required string Frequency= 2 ;
    //Starting time
    required int64 Start= 3 ;
    //deadline
    required int64 End= 4 ;
    // Whether to reach the starting point of the K line
    required bool IsStart= 5 ;
}
```

Name      | Type   | Description                                                                                                                                        | protobuf tag
--------- | ------ | -------------------------------------------------------------------------------------------------------------------------------------------------- | ------------
market    | string | market id                                                                                                                                          | 1
frequency | string | Frequency, Optional values: "1", "5", "15", "30", "60", "180", "360", "720", "D", "7D". It means 1 minute, 5 minutes...720 minutes, 1 day, 7 days. | 2
start     | long   | start time                                                                                                                                         | 3
end       | long   | end time                                                                                                                                           | 4
isStart   | bool   | You've reached the starting point of the K-line if true                                                                                            | 5

### Subscribe to the latest single K-line data

#### Request Message

**router：1** **command：902**

```protobuf
//SendCommand, 902| Subscribe to the K line
message SubKLine{
    //Subscription details
    repeated SubKLineItem Items= 1 ;
}
// K line subscription parameters
message SubKLineItem{
    //market
    required string Market= 1 ;
    //K line type
    repeated string Frequencys= 2 ;
}
```

Name       | Type     | Description                                                                                                                                        | Required | protobuf tag
:--------- | :------- | -------------------------------------------------------------------------------------------------------------------------------------------------- | -------- | ------------
market     | string   | market id                                                                                                                                          | Yes      | 1
frequencys | string[] | Frequency, Optional values: "1", "5", "15", "30", "60", "180", "360", "720", "D", "7D". It means 1 minute, 5 minutes...720 minutes, 1 day, 7 days. | Yes      | 2

#### Received Message

**command：1000**

```protobuf
//ReceiveCommand, 1000| K-line data
message WsKLine{
    //market
    required string Market= 1 ;
    //K line type
    required string Frequency= 2 ;
    // volume
    required double Volume= 3 ;
    //Opening price
    required double OpenPrice= 4 ;
    //Closing price
    required double ClosedPrice= 5 ;
    //lowest price
    required double LowPrice= 6 ;
    //highest price
    required double HighPrice= 7 ;
    //Opening time
    required int64 OpenTime= 8 ;
}
```

Name        | Type   | Description                                                                                                                                        | protobuf tag
----------- | ------ | -------------------------------------------------------------------------------------------------------------------------------------------------- | ------------
market      | string | market id                                                                                                                                          | 1
frequency   | string | Frequency, Optional values: "1", "5", "15", "30", "60", "180", "360", "720", "D", "7D". It means 1 minute, 5 minutes...720 minutes, 1 day, 7 days. | 2
volume      | double | deal volume                                                                                                                                        | 3
openPrice   | double | open price                                                                                                                                         | 4
closedPrice | double | last price                                                                                                                                         | 5
lowPrice    | double | low price                                                                                                                                          | 6
highPrice   | double | high price                                                                                                                                         | 7
openTime    | long   | open time                                                                                                                                          | 8

### Subscribe to the 24-hour rolling market quotes

#### Request Message

**router：1** **command：902**

```protobuf
//SendCommand, 902| Subscribe to the K line
message SubKLine{
    //Subscription details
    repeated SubKLineItem Items= 1 ;
}
// K line subscription parameters
message SubKLineItem{
    //market
    required string Market= 1 ;
    //K line type
    repeated string Frequencys= 2 ;
}
```

Name       | Type     | Description                        | Required | protobuf tag
:--------- | :------- | ---------------------------------- | -------- | ------------
market     | string   | market id                          | Yes      | 1
frequencys | string[] | Frequency, value can be 'SD1' only | Yes      | 2

#### Received Message

**command：1006**

```protobuf
//ReceiveCommand, 1000| K-line data
message WsKLine{
    //market
    required string Market= 1 ;
    //K line type
    required string Frequency= 2 ;
    // volume
    required double Volume= 3 ;
    //Opening price
    required double OpenPrice= 4 ;
    //Closing price
    required double ClosedPrice= 5 ;
    //lowest price
    required double LowPrice= 6 ;
    //highest price
    required double HighPrice= 7 ;
    //Opening time
    required int64 OpenTime= 8 ;
}
```

Name        | Type   | Description | protobuf tag
:---------- | :----- | ----------- | ------------
market      | string | market id   | 1
openPrice   | double | open price  | 2
closedPrice | double | last price  | 3
lowPrice    | double | low price   | 4
highPrice   | double | high price  | 5
volume      | double | deal volume | 6

### Subscribe to Market Depth

The push of the depth data is based on an incremental push.

After the subscription depth is pushed to the full amount of data, then the incremental data is pushed.

#### Method for Depth Incremental Update

Deep incremental updates are mainly divided into the "Data update" and "version comparison" two sections.

##### Data Update

To illustrate the convenience, it is assumed that the first push to obtain the market depth is as follows：

bid volume | bid price | ask price | ask volume
---------- | --------- | --------- | ----------
100        | 50        | 90        | 600
90         | 40        | 100       | 500
80         | 30        | 110       | 400

The following three cases of depth change are described below.

###### Add

If the depth increment data is

ask price | ask volume
--------- | ----------
80        | 600

This price is not in the original depth, it belongs to the new depth, therefore, the depth should be updated to：

bid volume | bid price | ask price | ask volume
---------- | --------- | --------- | ----------
100        | 50        | 80        | 600
90         | 40        | 90        | 600
80         | 30        | 100       | 500
           |           | 110       | 400

###### Update

If the depth increment data is

ask price | ask volume
--------- | ----------
90        | 233

This price exists at the original depth and is the data that updates the corresponding price, so the depth should be updated to：

bid volume | bid price | ask price | ask volume
---------- | --------- | --------- | ----------
100        | 50        | 90        | 233
90         | 40        | 100       | 500
80         | 30        | 110       | 400

###### Delete

If the depth increment data is

ask price | ask volume
--------- | ----------
90        | 0

This price exists at the original depth, and the quantity is already 0, the depth data is deleted, so the depth should be updated to：

bid volume | bid price | ask price | ask volume
---------- | --------- | --------- | ----------
100        | 50        | 100       | 500
90         | 40        | 110       | 400
80         | 30        |           |

##### Version Comparison

Both the depth snapshot and the depth increment will return the corresponding version information.

Developer needs to compare the versions after obtaining the version to ensure the correctness of the deep data update. Divided into two categories:

###### Comparison of snapshots and increments

The depth snapshot data contains the Version field.

The first increment of data contains startversion and endversion two fields.

Compare the Version field of the depth snapshot data to the Startversion field of the incremental data

If two fields are not equal, the increment is lost, requiring the developer to subscribe to the incremental data.

###### Comparison of increment and increment

The incremental data contains startversion and endversion two fields.

Compare the previous increment's Endversion field with the value of the last Startversion field.

If two fields are not equal, the increment is lost, requiring the developer to subscribe to the incremental data.

#### Request Message

**router：1** **command：906**

```protobuf
//SendCommand,906| Subscribe to market depth
message SubMarketDepth{
    //market
    required string Market= 1 ;
    // decimal place precision
    required int32 Precision= 2 ;
    // Get the number of bars, the optional value is: 10, 20, 40
    required int32 Limit= 3 ;
}
```

Name      | Type   | Description                             | Required | protobuf tag
--------- | ------ | --------------------------------------- | -------- | ------------
market    | string | market id                               | Yes      | 1
precision | int    | precision of price                      | Yes      | 2
limit     | int    | number of data，optional value ：10,20,40 | Yes      | 3

#### Received Message

**command：1003**

```protobuf
//ReceiveCommand,1003| Market depth snapshot
message MarketDepthDto{
    //market
    required string Market= 1 ;
    //Price accuracy
    required int32 Precision= 2 ;
    //list of sell orders
    repeated MarketDepth AskList= 3 ;
    //Buy list
    repeated MarketDepth BidList= 4 ;
    //version number
    required int64 Version= 5 ;
}
// Single market depth
message MarketDepth{
    //unit price
    required double P= 1 ;
    //quantity
    required double V= 2 ;
}
```

Name      | Type   | Description                         | protobuf tag
:-------- | :----- | ----------------------------------- | ------------
market    | string | market id                           | 1
precision | string | precision of price                  | 2
askList   | array  | ask list, price in ascending order  | 3
bidList   | array  | bid list, price in descending order | 4
version   | long   | version                             | 5
p         | double | price                               | 1
v         | double | volume                              | 2

**command：1004**

```protobuf
//ReceiveCommand,1004| Market depth difference information.
message MarketDepthDiff{
    //market
    required string Market= 1 ;
    // precision
    required int32 Precision= 2 ;
    //Sell order difference list
    repeated MarketDepth AskList= 3 ;
    //Buy order difference list
    repeated MarketDepth BidList= 4 ;
    // starting version number
    required int64 StartVersion= 5 ;
    //cutoff version number
    required int64 EndVersion= 6 ;
}
// Single market depth
message MarketDepth{
    //unit price
    required double P= 1 ;
    //quantity
    required double V= 2 ;
}
```

Name         | Type   | Description                         | protobuf tag
:----------- | :----- | ----------------------------------- | ------------
market       | string | market id                           | 1
precision    | int    | precision of price                  | 2
askList      | array  | ask list, price in ascending order  | 3
bidList      | array  | bid list, price in descending order | 4
startVersion | double | start version                       | 5
endVersion   | double | end version                         | 6
p            | double | price                               | 1
v            | double | volume                              | 2

### Error message

#### Request Message

none

#### Received Message

**command：0**

```protobuf
//ReceiveCommand,0| error message
message WsError{
    //error code
    required int32 Code= 1 ;
    //Error message
    required string message= 2 ;
}
```

## Personal APIs

Calling the user API requires authentication information to be invoked. Please refer [How To Get OpenApi And Use It](?file=002-English/001-How%20To%20Get%20OpenApi%20And%20Use "How To Get OpenApi And Use It") at first.

### Subscribe to Individual Order Messages

#### Request Message

**router：1** **command：1000**

```protobuf
//SendCommand, 1000| Login to the market
message LoginToMarket{
    //market
    required string Market= 1 ;
}
```

Name   | Type   | Description | Required | protobuf tag
:----- | :----- | ----------- | -------- | ------------
market | string | market id   | Yes      | 1

#### Received Message

**command：1008**

```protobuf
message OrderInfoDto{
    // Order Id
    required string Id= 1 ;
    //User ID
    required int64 UserId= 2 ;
    //Currency
    required string Currency= 3 ;
    //Handling strategy
    required int32 FeeStrategy= 4 ;
    //market
    required string Market= 5 ;
    // Order classification
    required int32 Category= 6 ;
    //Buy and sell category
    required int32 OrderType= 7 ;
    //plan list
    required int32 PlanType= 8 ;
    //trigger price
    required double TriggerPrice= 9 ;
    //Limited price unit price
    required double Price= 10 ;
    // limit order quantity
    required double Volume= 11 ;
    //Market price amount
    required double Amount= 12 ;
    //Order Status
    required int32 Status= 13 ;
    //Create time
    required int64 CreateTime= 14 ;
}
```

Name         | Type   | Description                                                                                  | protobuf tag
------------ | ------ | -------------------------------------------------------------------------------------------- | ------------
id           | string | order id                                                                                     | 1
userId       | long   | user id                                                                                      | 2
currency     | string | currency                                                                                     | 3
feeStrategy  | int    | strategy of exchange fee                                                                     | 4
market       | string | market                                                                                       | 5
category     | int    | order category. Optional: (1: no stop-limit, 2: stop-limit)                                  | 6
orderType    | int    | order type. Optional: (1: limit bid, 2: market price bid, 3: limit ask, 4: market price ask) | 7
planType     | int    | stop-limit order stop type （1:high price stop，2:low price stop）                              | 8
triggerPrice | double | stop price                                                                                   | 9
price        | double | limit price                                                                                  | 10
volume       | double | limit volume                                                                                 | 11
amount       | double | amount = price * volume                                                                      | 12
status       | int    | order status （2:pending,3:dealed,4:canceled）                                                 | 13
createTime   | long   | order creation time                                                                          | 14

**command：1009**

```protobuf
//ReceiveCommand,1009| Update order information
message UpdateOrderInfo{
    //highest price
    required string Market= 1 ;
    // Order Id
    required string OrderId= 2 ;
    //Trading volume
    required double TxVolume= 3 ;
    // transaction amount
    required double TxAmount= 4 ;
    //Order Status
    required int32 Status= 5 ;
    //Update time
    required int64 UpdateTime= 6 ;
}
```

Name       | Type   | Description                                  | protobuf tag
:--------- | :----- | -------------------------------------------- | ------------
market     | string | market id                                    | 1
orderId    | string | order id                                     | 2
txVolume   | double | deal volume                                  | 3
txAmount   | double | deal amount                                  | 4
status     | int    | order status （2:pending,3:dealed,4:canceled） | 5
updateTime | long   | order update time                            | 6

**command：1010**

```protobuf
//ReceiveCommand,1010| Plan to trigger
message PlanOrderTrigger{
     //market
     required string Market= 1 ;
     / / Plan order ID
     required string Id= 2 ;
     //trigger price
     required double Price= 3 ;
     //User ID
     required int64 UserId= 4 ;
}
```

Name   | Type   | Description      | protobuf tag
:----- | :----- | ---------------- | ------------
market | string | market id        | 1
id     | string | order id         | 2
price  | double | stop-limit price | 3
userId | double | user id          | 4
