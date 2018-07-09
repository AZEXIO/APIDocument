# AZEX Websocket

## Instructions

Domain Name:wss://ws.azex.io

**Serialization**

**Message Request**: The message request consists of three parts: **Route, Command, and Message Body**

The first four bytes (int32) of the request message are **routes** The fifth and sixth bytes of the request message are **Command** (int16)

**Message receiving** : The message returned by the server consists of two parts: the ** command and the message body.

The first two bytes of the received message are **Command** (int16)

** Serialization of message bodies via [protobuf] (<https://developers.google.com/protocol-buffers/> "protobuf")

[websocket-protobuf](/protobuf.json "websocket-protobuf")

example

```
//Subscription market depth
Message SubMarketDepth{
    //market
    Required string Market= 1 ;
    // decimal precision
    Required int32 Precision= 2 ;
    //Get the number of rows, the optional value is: 10,20,40
    Required int32 Limit= 3 ;
}
Message MarketDepthList{
    // Market depth list
    Repeated MarketDepthDto List= 1 ;
}
// Depth snapshot of market
Message MarketDepthDto{
    //market
    Required string Market= 1 ;
    //Accuracy
    Required int32 Precision= 2 ;
    //Sell list
    Repeated MarketDepth AskList= 3 ;
    // Pay List
    Repeated MarketDepth BidList= 4 ;
    //version number
    Required int64 Version= 5 ;
}
```

Send code (pseudo code) as follows

```
Websocket ws;
Protobuf protbuf;
Int16 SendCommand = 906; // command: subscribe to market depth
Int32 route = 1; //route
SubMarketDepth subMarket;
subMarket.Market = "eth_btc";
subMarket.Precision = 8;
subMarket.Limit = 10;

Ws.send(byteof(route)+byteof(SendCommand)+protbuf.Serialize(subMarket));
```

Receive code (pseudo-code) as follows

```
OnWebsocketReceived(byte[] data){
The
Int16 receiveCommand = ConvertToInt16(data.sub(0,2))
Switch(receiveCommand)
Case 1003
MarketDepthList dto = protbuf.Deserialize(data.sub(2,data.length))
//Business code
...
}
```

--------------------------------------------------------------------------------

### function list

Function                                   | Submit Command | Receive Command | Use
------------------------------------------ | -------------- | --------------- | ----------------------------------------------------------------------------------------------
Sign up for auction.                       | 907            | 1005            |
Get bulk K line data (subscribe)           | 900            | 1000,1001, 1002 | K line chart                                                                                   | 1000 single newest k line, 1001 part history k line, 1002 batch k line data push completed
Subscribe to the latest single K-line data | 902            | 1000            | K-line chart / Frequencys Optional "1", "5", "15", "30", "60", "180", "360", "720" , "D", "7D"
Subscribe to 24-hour rolling Quotes        | 902            | 1006            | Currency Trading - Market Quotes, Home                                                         | Frequencys to SD1
Subscription Market Depth                  | 906            | 1003,1004       | Currency Transactions - Latest Price                                                           | 1003 Depth Snapshot, 1004 Depth Difference Data
Subscribe to Personal Order Messages       | 1000           | 1008,1009,1010  | Update Currency Transactions - Current Orders                                                  | 1008 Order Creation, 1009 Order Updates, 1010 Planned Order Triggering
Error message                              |                | 0               | error message                                                                                  |

## API Reference

### Quotes API

Subscribe transaction record

**Request message**

**Route: 1** **Command: 907** Request parameters:

```
Message GetTopTradeList{
    //market
    Required string Market= 1 ;
    //Quantity
    Required int32 Count= 2 ;
    // subscribed
    Required bool Subscribe= 3 ;
}
```

Parameter Description

Parameter Name | Type   | Description                            | Required | Protobuf Sequence
:------------- | :----- | -------------------------------------- | -------- | -----------------
market         | string | Market Trading Pair                    | Yes      | 1
Count          | string | The number of subscriptions (up to 99) | Yes      | 2
subscribe      | bool   | To Subscript?                          | Yes      | 3

**Receive message**

**Command: 1005**

```
Message TradeSimpleDtoList{
    //Transaction data
    Repeated TradeSimpleData List= 1 ;
}
// transaction information
Message TradeSimpleData{
    //market
    Required string Market= 1 ;
    // transaction Id
    Required int64 Id= 2 ;
    //final price
    Required double Price= 3 ;
    / Volume
    Required double Volume= 4 ;
    // turnover
    Required double Amount= 5 ;
    / / Change type
    Required int32 Trend= 6 ;
    // transaction time
    Required int64 CreateTime= 7 ;
}
```

Parameter Name | Type   | Description                          | Protobuf Sequence
:------------- | :----- | ------------------------------------ | -----------------
market         | string | Market Trading Pair                  | 1
Id             | string |                                      | 2
Price          | double | sale price                           | 3
Volume         | double | Volume                               | 4
Amount         | double | Trade value                          | 5
Trend          | int    | Types of Change (1 up 2 down 3 flat) | 6
CreateTime     | long   | time                                 | 7

--------------------------------------------------------------------------------

Request k line history **Request message**

**Route: 1** **Command: 900** Request parameters:

```
Message GetKLineList{
    //market
    Required string Market= 1 ;
    //K line type
    Required string Frequency= 2 ;
    //Starting time
    Required int64 Start= 3 ;
    //End Time
    Required int64 End= 4 ;
    // subscribed
    Required bool Subscribe= 5 [default = true];
}
```

Parameter Description

Parameter Name | Type   | Description                                                                             | Required | Protobuf Sequence
:------------- | :----- | --------------------------------------------------------------------------------------- | -------- | -----------------
market         | string | Market Trading Pair                                                                     | Yes      | 1
frequency      | string | Frequencys Optional values ​​"1", "5", "15", "30", "60", "180", "360", "720", "D", "7D" | yes      | 2
start          | long   | Query start time                                                                        | Yes      | 3
end            | long   | End of time                                                                             | Yes      | 4
subscribe      | bool   | Subscript k line after request                                                          | No       | 5

**Receive message**

**Command: 1001**

```
Message WsKLineList{
    //K line data
    Repeated WsKLine List= 1 ;
}

Message WsKLine{
    //market
    Required string Market= 1 ;
    //K line type
    Required string Frequency= 2 ;
    / Volume
    Required double Volume= 3 ;
    //Opening price
    Required double OpenPrice= 4 ;
    //Closing price
    Required double ClosedPrice= 5 ;
    //lowest price
    Required double LowPrice= 6 ;
    // highest price
    Required double HighPrice= 7 ;
    //Opening time
    Required int64 OpenTime= 8 ;
}
```

Parameter Description

Parameter Name | Type   | Description                                                                             | Protobuf Sequence
:------------- | :----- | --------------------------------------------------------------------------------------- | -----------------
market         | string | Market Trading Pair                                                                     | 1
frequency      | string | Frequencys Optional values ​​"1", "5", "15", "30", "60", "180", "360", "720", "D", "7D" | 2
volume         | double | Volume                                                                                  | 3
openPrice      | double | Opening price                                                                           | 4
closedPrice    | double | close price                                                                             | 5
lowPrice       | double | lowest                                                                                  | 6
highPrice      | double | highest price                                                                           | 7
openTime       | long   | Opening Time                                                                            | 8

**Command: 1002** //Data reception completed

```
Message BatchSendComplate{
    //market
    Required string Market= 1 ;
    //K line type
    Required string Frequency= 2 ;
    //
    Required int64 Start= 3 ;
    //
    Required int64 End= 4 ;
    // Whether to reach the starting position of the K line
    Required bool IsStart= 5 ;
}
```

Parameter Description

Parameter Name | Type   | Description                                                    | Protobuf Sequence
:------------- | :----- | -------------------------------------------------------------- | -----------------
market         | string | Market Trading Pair                                            | 1
frequency      | string | Frequencys Values ​​1, 5, 15, 15, 30, 60, 180, 360, 720, D, 7D | 2
start          | long   | Query start time                                               | 3
end            | long   | End of query                                                   | 4
isStart        | bool   | Has taken all historical k lines                               | 5

--------------------------------------------------------------------------------

Subscribe k-line

**Request message**

**Route: 1** **Command: 902** Request parameters:

```
Message SubKLine{
    //Subscription details
    Repeated SubKLineItem Items= 1 ;
}
//K line subscription parameters
Message SubKLineItem{
    //market
    Required string Market= 1 ;
    //K line type
    Repeated string Frequencys= 2 ;
}
```

Parameter Description

Parameter Name | Type   | Description                                                                             | Required | Protobuf Sequence
:------------- | :----- | --------------------------------------------------------------------------------------- | -------- | -----------------
market         | string | Market Trading Pairs                                                                    | Yes      | 1
frequency      | string | Frequencys Optional values ​​"1", "5", "15", "30", "60", "180", "360", "720", "D", "7D" | yes      | 2

**Receive message**

**Command: 1000**

```
message WsKLine{
    //market
    required string Market= 1 ;
    //Opening price
    required double OpenPrice= 2 ;
    //Closing price
    required double ClosedPrice= 3 ;
    //lowest price
    required double LowPrice= 4 ;
    // highest price
    required double HighPrice= 5 ;
    // Volume
    required double Volume= 6 ;
}
```

Parameter Name | Type   | Description          | Protobuf Sequence
:------------- | :----- | -------------------- | -----------------
market         | string | Market Trading Pairs | 1
openPrice      | double | Opening price        | 2
closedPrice    | double | close price          | 3
lowPrice       | double | lowest               | 4
highPrice      | double | highest price        | 5
volume         | double | Volume               | 6

--------------------------------------------------------------------------------

Subscribe 24-hour rolling market

**Request message**

**Route: 1** **Command: 902** Request parameters:

```
Message SubKLine{
    //Subscription details
    Repeated SubKLineItem Items= 1 ;
}
//K line subscription parameters
Message SubKLineItem{
    //market
    Required string Market= 1 ;
    //K line type
    Repeated string Frequencys= 2 ;
}
```

Parameter Description

Parameter Name | Type   | Description                                                     | Required | Protobuf Sequence
:------------- | :----- | --------------------------------------------------------------- | -------- | -----------------
market         | string | Market Trading Pair                                             | Yes      | 1
frequency      | string | Frequencys can only be SD1 (to distinguish from normal k-lines) | YES      | 2

**Receive message**

**Command: 1000**

```
Message WsKLine{
    //market
    Required string Market= 1 ;
    //K line type
    Required string Frequency= 2 ;
    / Volume
    Required double Volume= 3 ;
    //Opening price
    Required double OpenPrice= 4 ;
    //Closing price
    Required double ClosedPrice= 5 ;
    //lowest price
    Required double LowPrice= 6 ;
    // highest price
    Required double HighPrice= 7 ;
    //Opening time
    Required int64 OpenTime= 8 ;
}
```

Parameter Name | Type   | Description                                                                             | Protobuf Sequence
:------------- | :----- | --------------------------------------------------------------------------------------- | -----------------
market         | string | Market Trading Pairs                                                                    | 1
frequency      | string | Frequencys Optional values ​​"1", "5", "15", "30", "60", "180", "360", "720", "D", "7D" | 2
volume         | double | Volume                                                                                  | 3
openPrice      | double | Opening price                                                                           | 4
closedPrice    | double | close price                                                                             | 5
lowPrice       | double | lowest                                                                                  | 6
highPrice      | double | highest price                                                                           | 7
openTime       | long   | Opening Time                                                                            | 8

--------------------------------------------------------------------------------

Subscript market depth

Market depth subscription is differential data pushing. The first pushing will return full data as snapshot, and then only differential data will be pushed.

Difference comparison method: 1\. Newly price, add new depth to snapshot. 2\. The price existed in snapshot, the number of V is not 0, which means to update depth in snapshot. The number of v is 0, which means to delete depth in snapshot . =====If StartVersion is different from snapshot Version, you need to re-subscript market depth

**Request message**

**Route: 1** **Command: 906** Request parameters:

```
Message SubMarketDepth{
    //market
    Required string Market= 1 ;
    // decimal precision
    Required int32 Precision= 2 ;
    //Get the number of rows, the optional value is: 10,20,40
    Required int32 Limit= 3 ;
}
```

Parameter Description

Parameter Name | Type   | Description                                        | Required | Protobuf Sequence
:------------- | :----- | -------------------------------------------------- | -------- | -----------------
market         | string | Market Trading Pairs                               | Yes      | 1
precision      | int    | Decimal precision                                  | Yes      | 2
limit          | int    | Number of entries, the optional value is: 10,20,40 | Yes      | 3

**Receive message**

**Command: 1003** //depth snapshot, will be pushed once for the first subscription

```
Message MarketDepthList{
    // Market depth list
    Repeated MarketDepthDto List= 1 ;
}
// Depth snapshot of market
Message MarketDepthDto{
    //market
    Required string Market= 1 ;
    //Accuracy
    Required int32 Precision= 2 ;
    //Sell list
    Repeated MarketDepth AskList= 3 ;
    // Pay List
    Repeated MarketDepth BidList= 4 ;
    //version number
    Required int64 Version= 5 ;
}
Message MarketDepth{
    //unit price
    Required double P= 1 ;
    //Quantity
    Required double V= 2 ;
}
```

Parameter Name | Type   | Description          | Protobuf Sequence
:------------- | :----- | -------------------- | -----------------
market         | string | Market Trading Pairs | 1
precision      | string | Market accuracy      | 2
askList        | array  | unit price           | 3
bidList        | array  | Quantity             | 4
version        | long   | version number       | 5
p              | double | price                | 1
v              | double | Quantity             | 2

**Command: 1004** //depth difference

```
// Market depth difference information. Difference comparison method: 1\. Newly appeared price, indicating new depth. 2\. The price that has already appeared, the number of pending orders is not 0, which means that the updated price has appeared. The number of pending orders is 0, indicating that the depth is deleted. =====If the versions of StartVersion and Market Depth snapshot are different, you need to re-acquire a market depth snapshot
Message MarketDepthDiff{
    //market
    Required string Market= 1 ;
    //Accuracy
    Required int32 Precision= 2 ;
    // Selling Difference List
    Repeated MarketDepth AskList= 3 ;
    // Pay the difference list
    Repeated MarketDepth BidList= 4 ;
    //Start version number
    Required int64 StartVersion= 5 ;
    // deadline version number
    Required int64 EndVersion= 6 ;
}
Message MarketDepth{
    //unit price
    Required double P= 1 ;
    //Quantity
    Required double V= 2 ;
}
```

Parameter Name | Type   | Description            | Protobuf Sequence
:------------- | :----- | ---------------------- | -----------------
market         | string | Market Trading Pairs   | 1
precision      | string | Market Accuracy        | 2
askList        | array  | sell                   | 3
bidList        | array  | Buy                    | 4
startVersion   | double | Beginning Version      | 5
endVersion     | double | Current version number | 6
p              | double | price                  | 1
v              | double | Quantity               | 2

--------------------------------------------------------------------------------

### Private API

Each request requires the construction of a signature parameter sign and an Authorization parameter

### signature

Get api: Please create api in the Account Settings -> Api Management -> create an API, create an Apikey and Secret Key, please properly manage, **Important: These two keys and account security are closely related, whenever Do not disclose it to others.**

**Apikey is used to identify the user. Add the Authorization=Apikey parameter in the ws uri.**

**Secrect Key is used to generate sign parameters**

**sign generation method:** Use Secret Key as key, Authorization=Apikey as value to calculate HmacSHA256

example

APIKey=81.67AAA2F6041D408D9868387A8904431D, Authorization parameters are as follows

```
Authorization=81.67AAA2F6041D408D9868387A8904431D
```

If the Secret Key is 2288987EFDB54F848D7BACCE1288FC9A, the sign value is calculated.

```
57c4c6770d565aa236f87706053bd51512862443062e471bd3243a6ed8eef2
```

The final generated ws address: Wss://ws.azex.io?Authorization=81.67AAA2F6041D408D9868387A8904431D&sign=57c4c6770d565aa236f87706053bd51512862443062e471bd3243a6ed8eef2

--------------------------------------------------------------------------------

Subscribe to personal orders (order creation message, order update message, plan trigger message)

**Request message**

**Route: 1** **Command: 1000** Request parameters:

```
//log into the market
Message LoginToMarket{
    //market
    Required string Market= 1 ;
}
```

Parameter Description

Parameter Name | Type   | Description         | Required | Protobuf Sequence
:------------- | :----- | ------------------- | -------- | -----------------
market         | string | Market Trading Pair | Yes      | 1

**Receive message**

**Command: 1008** //Add order message

```
//order information
Message OrderInfoDto{
    //Order Id
    Required string Id= 1 ;
    //User Id
    Required int64 UserId= 2 ;
    //Currency
    Required string Currency= 3 ;
    //Currency Currency
    Required string FeeCurrency= 4 ;
    //market
    Required string Market= 5 ;
    //Order classification
    Required int32 Category= 6 ;
    //sales category
    Required int32 OrderType= 7 ;
    // Plan category
    Required int32 PlanType= 8 ;
    // Trigger price
    Required double TriggerPrice= 9 ;
    // Limit price unit price
    Required double Price= 10 ;
    // limit order quantity
    Required double Volume= 11 ;
    // Market Order Amount
    Required double Amount= 12 ;
    //Order Status
    Required int32 Status= 13 ;
    //Create time
    Required int64 CreateTime= 14 ;
}
```

**Parameter Description**

Parameter Name | Type   | Description                                                      | Protobuf Sequence
:------------- | :----- | ---------------------------------------------------------------- | -----------------
id             | string | Order id                                                         | 1
userId         | long   | user id                                                          | 2
currency       | string | Currency                                                         | 3
feeCurrency    | string | Fee Currency                                                     | 4
market         | string | Marketing                                                        | 5
category       | int    | Order Classification (1 unscheduled order, 2 planned order)      | 6
orderType      | int    | Order Type (1 Limit Buy 2 Market Buy 3 Limit Sell 4 Market Sell) | 7
planType       | int    | Plan Type (1 High Trigger 2 Low Trigger)                         | 8
triggerPrice   | double | trigger price                                                    | 9
price          | double | hanging price                                                    | 10
volume         | double | coin quantity                                                    | 11
amount         | double | Order total price                                                | 12
status         | int    | Order Status (3 orders completed in 2 transactions cancelled 4)  | 13
createTime     | long   | time                                                             | 14

**Command: 1009** // Update order message

```
//Update order information
Message UpdateOrderInfo{
    // highest price
    Required string Market= 1 ;
    //Order Id
    Required string OrderId= 2 ;
    //Trading volume
    Required double TxVolume= 3 ;
    //Transaction amount
    Required double TxAmount= 4 ;
    //Order Status
    Required int32 Status= 5 ;
    //Update time
    Required int64 UpdateTime= 6 ;
}
```

**Parameter Description**

Parameter Name | Type   | Description                                                     | Protobuf Sequence
:------------- | :----- | --------------------------------------------------------------- | -----------------
market         | string | Market Trading Pairs                                            | 1
orderId        | string | Order id                                                        | 2
txVolume       | double | Trade Volume                                                    | 3
txAmount       | double | Transaction Amount                                              | 4
status         | int    | Order Status (3 orders completed in 2 transactions cancelled 4) | 5
updateTime     | long   | Updated Time                                                    | 6

**Command: 1010** //Single-shot message

```
Message PlanOrderTrigger{
    //market
    Required string Market= 1 ;
    //planned order ID
    Required string Id= 2 ;
    // Trigger price
    Required double Price= 3 ;
    //User Id
    Required int64 UserId= 4 ;
}
```

**Parameter Description**

Parameter Name | Type   | Description          | Protobuf Sequence
:------------- | :----- | -------------------- | -----------------
market         | string | Market Trading Pairs | 1
id             | string | Plan ID id           | 2
price          | double | trigger price        | 3
userId         | double | user id              | 4

--------------------------------------------------------------------------------

Error message

**Receive message** **Command: 0**

```
Message WsError{
    //error code
    Required int32 Code= 1 ;
    //Error message
    Required string Message= 2 ;
}
```
