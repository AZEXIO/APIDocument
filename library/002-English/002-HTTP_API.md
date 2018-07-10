# AZEX HTTP-API

> This article is translated by Bing and, if there is a problem, welcome feedback via the right link:<https://github.com/AZEXIO/APIDocument/issues>

Through this document, developers can learn how to get quotes and user personal data through HTTP invocation.

## Global Description

The content described in this section is important. Developers are requested to read the contents of this section at first.

### Request Parameters

All APIs are invoked by Form submission, supporting only POST and get two request methods.

The request parameter with GET method is passed in the querystring.

The request parameter with POST method is passed in the body.

Must contain the following two headers in the request

Name         | Value
------------ | ---------------------------------
Content-Type | application/x-www-form-urlencoded
Accept       | application/json

By default, the message returned by the server is prompted in English, and if you need to change language of message, you need to pass the header named 'Lang'.

The corresponding values are compared with the language as follows:

Value | Language
----- | ------------------
cn    | Chinese Simplified
en    | English

### Response Result

If the corresponding service-side processing succeeds, it responds to the 2XX series response code and returns the format of the output Json.

json in the response body generally as follows:

```json
{
    "isOk": true,
    "value": []or{},
    "err": {
        "code": 0,
        "message": null
    }
}
```

The explanations for each of these attributes are as follows:

Name        | Type         | Description
----------- | ------------ | ----------------------------------------------------------------------------------------------------------
isOk        | bool         | Whether the request was processed successfully. True: Success, false: Failed
value       | object,array | Processing results for this request may be an array, object, or null, as described in the specific request
err.code    | int          | Error code that identifies the error code for this error
err.message | string       | Error Information

### Market ID

The market ID consists of two parts, a basic currency and a target currency, separated by an underscore in the middle.

For example, btc_usdt means that the basic currency is the usdt target currency btc.

### Time Format

the format of time as an incoming or outgoing parameter refers to the following API is Unix timestamp seconds.

For example:time 2018/7/9 15:25:13 in Beijing is 1531121113 as Unix timestamp seconds.

Developers can use this link on the right to find the method of getting Unix timestamp seconds in some program language:<https://www.epochconverter.com/>

## Public APIs

The APIs stated in this section are public APIs that can be invoked without user authentication.

### Get All currencies

#### Request URL

- GET `https://openapi.azex.io/Market/GetCurrencys`

#### Parameters

None

#### Responce Sample

```json
{
    "isOk": true,
    "value": [
        "btc",
        "bts",
        "eth",
        "hlc",
        "usdt",
        "xrp"
    ],
    "err": {
        "code": 0,
        "message": null
    }
}
```

#### Responce Description

Name  | Type     | Description
----- | -------- | -----------
value | string[] | Currency

### Get Market Info

#### Request URL

- GET `https://openapi.azex.io/Market/GetMarketInfos`

#### Parameters

Name   | Type   | Description | Required
------ | ------ | ----------- | --------
market | string | market      | No

#### Responce Sample

```json
{
    "isOk": true,
    "value": [{
        "id": "eth_btc",
        "basic": "btc",
        "target": "eth",
        "makerFeeRate": 0.0015,
        "takerFeeRate": 0.002,
        "minOrderAmount": 0.001,
        "pricePrecision": 8,
        "volumePrecision": 8,
        "depthVolumePrecision": 8,
        "priceLimitPercent": 0.25,
        "area": 1,
        "status": 2,
        "openTime": 1528367890,
        "lockEndTime": -62135596800,
        "createdAt": 1528367894,
        "sortIndex": 0,
        "topIndex": 4
    }],
    "err": {
        "code": 0,
        "message": null
    }
}
```

#### Responce Description

Name                 | Type   | Description
-------------------- | ------ | ------------------------------------------------------------------------------------------------------------------------------------
id                   | string | market id
basic                | string | basic currency
target               | string | target currency
makerFeeRate         | double | fee rate of maker.Example: 0.05 as 5%。
takerFeeRate         | double | fee rate of tacker.Example: 0.05 as 5%。
minOrderAmount       | double | min amount of order,order amount must greater than this
pricePrecision       | double | price precision. Example: 8,The price of the order must not exceed 8 digits
volumePrecision      | double | volume precision. Example: 8,The volume of the order must not exceed 8 digits
depthVolumePrecision | double | precision of volume in market depth. Example: 8,The volume in market depth must not exceed 8 digits
priceLimitPercent    | double | price increase limit in order. Example: If 0.25 is returned, the order price cannot be greater than 25% of the current market price.
status               | int    | market status. 2:open,4:locked,8:closed

### Get Market K Line

#### Request URL

- GET `https://ws.azex.io/Market/GetKline`

#### Parameters

Name      | Type   | Description                                                                                                            | Required
--------- | ------ | ---------------------------------------------------------------------------------------------------------------------- | --------
market    | string | market id                                                                                                              | Yes
frequency | string | frequency of kline .The optional value is: 1,5,15,30,60,240,480,720,d. as 1 minutes, 5 minutes ... 720 minutes, 1 days | Yes
startTime | long   | start time                                                                                                             | Yes
limit     | int    | number of data，range in [1,200] default is 200                                                                         | No

#### Responce Sample

```json
{
    "isOk": true,
    "value": [
        {
            "id": 3144,
            "openTime": 1528787640,
            "openPrice": 0.003303,
            "lowPrice": 0.000048,
            "highPrice": 0.011193,
            "closedPrice": 0.00235,
            "volume": 23529.582076
        },
        {
            "id": 3145,
            "openTime": 1528787700,
            "openPrice": 0.00235,
            "lowPrice": 0.000357,
            "highPrice": 0.010713,
            "closedPrice": 0.006908,
            "volume": 2171.671503
        }
    ],
    "err": {
        "code": 0,
        "message": null
    }
}
```

#### Responce Description

Name        | Type   | Description
----------- | ------ | -----------
id          | int    | kline id
openTime    | long   | open time
openPrice   | double | open price
lowPrice    | double | low price
highPrice   | double | high price
closedPrice | double | last price
volume      | double | deal volume

### Get Market Depth

#### Request URL

- GET `https://ws.azex.io/Market/GetDepth`

#### Parameters

Name      | Type   | Description                                                                                                                                                                                                                                      | Required
--------- | ------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | --------
market    | string | market id                                                                                                                                                                                                                                        | Yes
precision | int    | precision of price. Example: 2 means 2 decimal digits。Need to refer to the price precision of the market. If the price precision is greater than 2, the value range is [priceprecision-4,priceprecision],no] and the range is [0,priceprecision] | Yes
limit     | int    | number of depth,range [1,40] default is 40                                                                                                                                                                                                       | No

#### Responce Sample

```json
{
    "isOk": true,
    "value": {
        "market": "eth_btc",
        "precision": 8,
        "askList": [
            {
                "p": 0.013113,
                "v": 400.38968775
            },
            {
                "p": 0.013114,
                "v": 1842.064724
            }
        ],
        "bidList": [
            {
                "p": 0,
                "v": 790685791395.61292
            },
            {
                "p": 0.013108,
                "v": 54.454594
            }
        ],
        "version": 14093621
    },
    "err": {
        "code": 0,
        "message": null
    }
}
```

#### Responce Description

Name      | Type    | Description
--------- | ------- | -----------------------------------
market    | int     | kline id
precision | int     | precision of price
askList   | array   | ask list, price in ascending order
bidList   | array   | bid list, price in descending order
p         | decimal | price
v         | decimal | volume
version   | long    | version

### Get Market 24-Hour Rolling Quotes

#### Request URL

- GET `https://ws.azex.io/Market/GetTiker`

#### Parameters

Name   | Type   | Description | Required
------ | ------ | ----------- | --------
market | string | market id   | Yes

#### Responce Sample

```json
{
    "isOk": true,
    "value": {
        "market": "eth_btc",
        "openPrice": 0.010755,
        "closedPrice": 0.018027,
        "lowPrice": 0.000001,
        "highPrice": 0.043969,
        "volume": 7213330617.52480125
    },
    "err": {
        "code": 0,
        "message": null
    }
}
```

#### Responce Description

Name        | Type   | Description
----------- | ------ | -----------
market      | string | market id
openPrice   | string | open price
lowPrice    | double | low price
highPrice   | double | high price
closedPrice | double | last price
volume      | double | deal amount

### Get Recent Closing Records

#### Request URL

- GET `https://ws.azex.io/Market/GetHistoryTrade`

#### Parameters

Name   | Type   | Description                                     | Required
------ | ------ | ----------------------------------------------- | --------
market | string | market id                                       | Yes
limit  | int    | number of data, rang in [1,200]， default is 200 | No

#### Responce Sample

```json
{
    "isOk": true,
    "value": [
        {
            "market": "eth_btc",
            "id": 7573609,
            "price": 0.002476,
            "volume": 34.554212,
            "amount": 0.08555622,
            "trend": 2,
            "createTime": 1528792488
        }
    ],
    "err": {
        "code": 0,
        "message": null
    }
}
```

#### Responce Description

Name   | Type   | Description
------ | ------ | -------------------------------
id     | int    | deal record id
market | string | market id
price  | double | deal price
volume | double | deal volume
amount | double | deal amount
trend  | int    | price trend（1:up,2:down,3:flat）

## User APIs

Calling the user API requires authentication information to be invoked. Please refer [How To Get OpenApi And Use It](?file=002-English/001-How%20To%20Get%20OpenApi%20And%20Use "How To Get OpenApi And Use It") at first.

### Get Assets of User

#### Request URL

- POST `https://openapi.azex.io/Account/Balance`

#### Parameters

Name       | Type   | Description                                              | Required
---------- | ------ | -------------------------------------------------------- | --------
currencies | string | Currency, multiple currencies in English comma separated | No
timestamp  | long   | unix timestamp                                           | Yes
sign       | string | sign                                                     | Yes

#### Responce Sample

```json
{
    "isOk": true,
    "value": [{
        "currency": "usdt",
        "lockedBalance": 0.00000000,
        "availableBalance": 100000.00000000,
        "mortgagedBalance": 0.00000000,
        "totalBalance": 100000.00000000
    }, {
        "currency": "btc",
        "lockedBalance": 0.00000000,
        "availableBalance": 10.08500000,
        "mortgagedBalance": 0.00000000,
        "totalBalance": 10.08500000
    }],
    "err": {
        "code": 0,
        "message": "string"
    }
}
```

#### Responce Description

Name             | Type   | Description
---------------- | ------ | ----------------------------------------------
currency         | string | currency
lockedBalance    | double | locked balance
availableBalance | double | available balance
mortgagedBalance | double | mortgaged balance
totalBalance     | double | totalb alance = locked + available + mortgaged

### Submit Limit Price or Market Price Order

#### Request URL

- POST `https://openapi.azex.io/Order/Trade`

#### Parameters

Name      | Type   | Description                                                                                  | Required
--------- | ------ | -------------------------------------------------------------------------------------------- | --------
marketId  | string | market id                                                                                    | Yes
orderType | int    | order type. Optional: (1: limit bid, 2: market price bid, 3: limit ask, 4: market price ask) | Yes
price     | double | limit price                                                                                  | Yes
volume    | double | limit volume or volume of market price ask                                                   | Yes
amount    | double | amount of market price bid                                                                   | Yes
sign      | string | sign                                                                                         | Yes
timestamp | long   | unix timestamp                                                                               | Yes

#### Responce Sample

```json
{
    "isOk": true,
    "value": {
        "orderId":"5b207fb09c1a0d82d80799dd",
        "balance":123
    },
    "err": {
        "code": 0,
        "message": ""
    }
}
```

#### Responce Description

Name    | Type   | Description
------- | ------ | ---------------------------------------------------
orderId | string | order id
balance | double | available balance of currency after order submitted

### Submit Stop-Limit Order

#### Request URL

- POST `https://openapi.azex.io/Order/SubmitPlanOrder`

#### Parameters

Name         | Type   | Description                         | Required
------------ | ------ | ----------------------------------- | --------
marketId     | string | market id                           | Yes
orderType    | int    | order type（1:limit bid，3:limit ask） | Yes
triggerPrice | double | stop price                          | Yes
price        | double | limit price                         | Yes
volume       | double | volume                              | Yes
sign         | string | sign                                | Yes
timestamp    | long   | unix timestamp                      | Yes

#### Responce Sample

```json
{
    "isOk": true,
    "value": {
        "orderId":"5b207fb09c1a0d82d80799dd",
        "balance":123
    },
    "err": {
        "code": 0,
        "message": ""
    }
}
```

#### Responce Description

Name    | Type   | Description
------- | ------ | ---------------------------------------------------
orderId | string | order id
balance | double | available balance of currency after order submitted

### Cancel Order

#### Request URL

- POST `https://openapi.azex.io/Order/CancelOrder`

#### Parameters

Name          | Type   | Description                                                                                  | Required
------------- | ------ | -------------------------------------------------------------------------------------------- | --------
marketId      | string | market id                                                                                    | Yes
orderType     | int    | order type. Optional: (1: limit bid, 2: market price bid, 3: limit ask, 4: market price ask) | Yes
orderCategory | int    | order category. Optional: (1: no stop-limit, 2: stop-limit)                                  | Yes
orderId       | string | order id                                                                                     | Yes
sign          | string | sign                                                                                         | Yes
timestamp     | long   | unix timestamp                                                                               | Yes

#### Responce Sample

```json
{
    "isOk": true,
    "err": {
        "code": 0,
        "message": ""
    }
}
```

### Bulk Cancel Orders

#### Request URL

- POST `https://openapi.azex.io/Order/BatchCancelOrder`

#### Parameters

Name          | Type   | Description                                                 | Required
------------- | ------ | ----------------------------------------------------------- | --------
marketId      | string | market id                                                   | Yes
orderCategory | int    | order category. Optional: (1: no stop-limit, 2: stop-limit) | Yes
sign          | string | sign                                                        | Yes
timestamp     | long   | unix timestamp                                              | Yes

#### Responce Sample

```json
{
    "isOk": true,
    "err": {
        "code": 0,
        "message": ""
    }
}
```

### Qeury Orders

#### Request URL

- POST `https://openapi.azex.io/Order/QueryOrder`

#### Parameters

Name      | Type   | Description                                                  | Required
--------- | ------ | ------------------------------------------------------------ | --------
orderIds  | string | order id, multiple orders separated by commas (,) in English | Yes
sign      | string | sign                                                         | Yes
timestamp | long   | unix timestamp                                               | Yes

#### Responce Sample

```json
{
    "isOk": true,
    "value": [
        {
            "id": "5b207fb09c1a0d82d80799dd",
            "market": "xrp_btc",
            "userId": 10,
            "currency": "btc",
            "category": 1,
            "orderType": 1,
            "planType": 0,
            "triggerPrice": 0,
            "price": 0.005,
            "volume": 2,
            "amount": 0.01,
            "tradedVolume": 0,
            "tradeAmount": 0,
            "returnAmount": 0,
            "status": 4,
            "createTime": 1528856496
        }
    ],
    "err": {
        "code": 0,
        "message": null
    }
}
```

#### Responce Description

Name         | Type   | Description
------------ | ------ | --------------------------------------------------------------------------------------------
id           | string | order id
market       | string | market id
userId       | long   | user id
currency     | string | currency
category     | int    | order category. Optional: (1: no stop-limit, 2: stop-limit)
orderType    | int    | order type. Optional: (1: limit bid, 2: market price bid, 3: limit ask, 4: market price ask)
planType     | int    | stop-limit order stop type （1:high price stop，2:low price stop）
triggerPrice | double | stop price
price        | double | limit price
volume       | double | limit volume
amount       | double | amount = price * volume
tradedVolume | double | deal volume
tradeAmount  | double | deal amount
returnAmount | double | return amount
status       | int    | order status （2:pending,3:dealed,4:canceled）
createTime   | long   | order creation time

The following lists the status of the order：

order category  | order type         | plan order trigge type | Order function
--------------- | ------------------ | ---------------------- | ------------------------------------------------------------------------------------
1:no stop-limit | 1:limit bid        | none                   | limit bid order
1:no stop-limit | 2:market price bid | none                   | market price bid order
1:no stop-limit | 3:limit ask        | none                   | limit ask order
1:no stop-limit | 4:market price ask | none                   | market price ask order
2:stop-limit    | 1:limit bid        | 1:high price stop      | When the market price is higher than Triggerprice, a limit bid order will be execute
2:stop-limit    | 1:limit bid        | 2:low price stop       | When the market price is lower than Triggerprice, a limit bid order will be execute
2:stop-limit    | 3:limit ask        | 1:high price stop      | When the market price is higher than Triggerprice, a limit ask order will be execute
2:stop-limit    | 3:limit ask        | 2:low price stop       | When the market price is lower than Triggerprice, a limit ask order will be execute
