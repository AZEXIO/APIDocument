# AZEX HTTP-API

## Instructions


API domain name: https://openapi.azex.io



### Quotes API

------------


**A brief description:** 

- Get all currencies in the platform

**Request URL:**
- Get `/Market/GetCurrencys`
  


**Parameters:**

no

**Return example**

```
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

**Return parameter description**

|Parameter name|Type|Description|
|:----- |:-----|----- |
|value | string[] |Currency |



------------



**A brief description:** 

- Get market information (accuracy and other data)

**Request URL:**
- Get ` /Market/GetMarketInfos `
  


**Parameters:**

|Parameter name|Type|Description|Required|
|:----- |:-----|----- |-------|
|market |string |Market Trading Pairs | No |


**Return example**

```
{
    "isOk": true,
    "value": {
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
        "area": ​​1,
        "status": 2,
        "openTime": 1528367890,
        "lockEndTime": -62135596800,
        "createdAt": 1528367894,
        "sortIndex": 0,
        "topIndex": 4
    },
    "err": {
        "code": 0,
        "message": null
    }
}
```

**Return parameter description**

|Parameter name|Type|Description|
|:----- |:-----|----- |
|id | string | market id |
|basic | string | basic currency |
Target | string | target currency |
|makerFeeRate | float | Maker fee rate |
|takerFeeRate | float | Taker fee rate |
|minOrderAmount | float |Minimum total of order |
|pricePrecision | float |Maximum price precision |
|volumePrecision | float |Maximum amount precision |
|depthVolumePrecision | float |Deep precision |


------------



**A brief description:** 

- Get market k line

**Request URL:**
- Get `/Market/GetKline`
  


**Parameters:**

|Parameter name|Type|Description|Required|
|:----- |:-----|----- |-------|
|market |string |Market trading pair | Yes |
|frequency |string |k Line Period Category, 1,5,15,30,60,240,480,720,d |Yes |
|startTime |string |Start time |Yes |
|limit |int |The number of data entries (Range 1-200 Default 200) | No |


**Return example**

```
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

**Return parameter description**

|Parameter name|Type|Description|
|:----- |:-----|----- |
|id | int |k line id |
|openTime | long |Opening time |
|openPrice | string | opening price |
|lowPrice | float |lowest price |
|highPrice | float |highest price |
|closedPrice | float | closing price |
|volume | float | transaction amount |



------------


**A brief description:** 

- Get market depth

**Request URL:**
- Get ` /Market/GetDepth `
  


**Parameters:**

|Parameter name|Type|Description|Required|
|:----- |:-----|----- |-------|
|market |string |Market trading pair | Yes |
|precision |int |Market Accuracy |Yes |
|limit |int |The number of data entries (Range 1-40 Default 40) | No |


**Return example**

```
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

**Return parameter description**

|Parameter name|Type|Description|
|:----- |:-----|----- |
|market | int |k line id |
|precision | int | market accuracy |
|askList | string | sell |
|bidList | string | Buy |
|version | long |depth version number |



------------

**A brief description:** 

- Get market 24-hour rolling quotes

**Request URL:**
- Get `/Market/GetTiker`
  


**Parameters:**

|Parameter name|Type|Description|Required|
|:----- |:-----|----- |-------|
|market |string |Market trading pair | Yes |



**Return example**

```
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

**Return parameter description**

|Parameter name|Type|Description|
|:----- |:-----|----- |
|Market | string |Market |
|openPrice | string | opening price |
|lowPrice | float |lowest price |
|highPrice | float |highest price |
|closedPrice | float | closing price |
|volume | float | transaction amount |


------------

**A brief description:** 

- Get recent transactions

**Request URL:**
- Get ` /Market/GetHistoryTrade `
  


**Parameters:**

|Parameter name|Type|Description|Required|
|:----- |:-----|----- |-------|
|market |string |Market trading pair | Yes |
|limit |int |Data Volume 1-200, Default 200 | No |



**Return example**

```
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

**Return parameter description**

|Parameter name|Type|Description|
|:----- |:-----|----- |
|market | string |market |
|id | int |id |
|price | float |price |
|volume | float |Quantity |
|amount | float |amount |
|trend | int | Change Type (1 up 2 down 3 flat) |


## Private API Instructions

Each request requires the construction of sign, an signature parameter,an timestamp parameter (**UTC Time**) and an request header-Authorization


### signature

Get api: Please create api in Account Settings -> Api Management -> Create API. After the creation is completed, an Apikey and Secret Key will be generated. Please manage it properly. **Important: These two keys are closely related to account security. Do not disclose it to others.**

**Apikey is used to identify the user , applied to create Authorization in http request header  eg,scheme=OPENAPI,value=Apikey**

**Secrect Key is used to generate sign parameters**

**Sign generation method:** The encrypted string obtained by ascending order of the interface request parameters and the Secret Key as the key are used to calculate HmacSHA256 

example

The account asset API request parameter is currencies=btc, and the encrypted string is as follows

```
currencies=btc&timestamp=1500000000
```

If the Secret Key is 2288987EFDB54F848D7BACCE1288FC9A, the sign value is calculated.
```
9a9f368f1499e228fc2b4036183ef8446f518d3cd5ce1dd167caff5ae91cb885
```






### Request


 Content-Type Please specify application/x-www-form-urlencoded
 

------------


**A brief description:** 

- Get account assets

**Request URL:**
- Post ` /Account/Balance `
  


**Parameters:**

|Parameter name|Type|Description|Required|
|:----- |:-----|----- |-------|
|currencies |string| Coin type, multiple coins is separated by commas, empty value means all balances | No |
|timestamp |long | timestamp **UTC Time** | yes |
|sign |string | Signature |Yes|

**Return example**

```
{
  "isOk": true,
  "value": {
    "items": [
      {
        "currency": "string",
        "lockedBalance": 0,
        "availableBalance": 0,
        "mortgagedBalance": 0,
        "totalBalance": 0
      }
    ]
  },
  "err": {
    "code": 0,
    "message": "string"
  }
}
```

**Return parameter description**

|Parameter name|Type|Description|
|:----- |:-----|----- |
|currency |string |coin code |
|availableBalance |double |available balance |
|lockedBalance |double |Freeze balance |
|mortgagedBalance |double |Mortgage Amount |
|totalBalance |double |Total Balance |




------------


**A brief description:** 

- Order

**Request URL:**
- Post` /Order/Trade `


**Parameters:**

|Parameter name|Type|Description|Required|
|:----- |:-----|----- |-------|
|marketId |string |Marketing Example eth_btc |Yes |
|orderType |int |Order Type (1 Buy at limit 2 Buy at Market 3 Sell at limit 4 Sell at Market) |Yes|
|price |string |price ,limit price | yes|
|volume |string | coin quantity | Yes|
|amount |string | Order total price |Yes|
|sign |string | Signature |Yes|
|timestamp |long | timestamp **UTC Time** | yes |

**Return example**

```
{
    "isOk": true,
    "value": "5b207fb09c1a0d82d80799dd",
    "err": {
        "code": 0,
        "message": ""
    }
}
```

**Return parameter description**

|Parameter name|Type|Description|
|:----- |:-----|----- |
|value |string |Order id |


-----

**A brief description:** 

- Plan orders

**Request URL:**
- Post` /Order/SubmitPlanOrder `


**Parameters:**

|Parameter name|Type|Description|Required|
|:----- |:-----|----- |-------|
|marketId |string |Marketing Example eth_btc |Yes |
|orderType |int |Order Type (1 Limit Buy 3 Limit Sell) |Yes|
|TriggerPrice |float |Trigger price |Yes|
|price |string |Order price |Yes|
|volume |string |coin volume  |Yes|
|sign |string | Signature |Yes|
|timestamp |long | timestamp **UTC Time** | yes |

**Return example**

```
{
    "isOk": true,
    "value": "5b207fb09c1a0d82d80799dd",
    "err": {
        "code": 0,
        "message": ""
    }
}
```

**Return parameter description**

|Parameter name|Type|Description|
|:----- |:-----|----- |
|value |string |Order id |



----
**A brief description:** 

- Cancellation of order

**Request URL:**
- Post ` /Order/CancelOrder `


**Parameters:**

|Parameter name|Type|Description|Required|
|:----- |:-----|----- |-------|
|marketId |string |Marketing Example eth_btc |Yes |
|orderType |int |Order Type (1 Buy at limit 2 Buy at Market 3 Sell at limit 4 Sell at Market) |Yes|
|orderCategory |int |Order Type (1 non-plan 2 plan sheet) |Yes|
|orderId |string |Order id |Yes|
|sign |string | Signature |Yes|
|timestamp |long | timestamp **UTC Time** | yes |

**Return example**

```
{
    "isOk": true,
    "err": {
        "code": 0,
        "message": ""
    }
}
```



----

**A brief description:** 

- Mass cancellation of orders

**Request URL:**
- Post ` /Order/BatchCancelOrder `
  


**Parameters:**

|Parameter name|Type|Description|Required|
|:----- |:-----|----- |-------|
|marketId |string |Marketing Example eth_btc (cancel all orders of eht_btc)|Yes |
|orderCategory |int |Order Type (1 non-plan 2 plan sheet) |Yes|
|sign |string | Signature |Yes|
|timestamp |long | timestamp **UTC Time** | yes |

**Return example**

```
{
    "isOk": true,
    "err": {
        "code": 0,
        "message": ""
    }
}
```

----

**A brief description:** 

- checking order

**Request URL:**
- Post` /Order/QueryOrder `
  


**Parameters:**

|Parameter name|Type|Description|Required|
|:----- |:-----|----- |-------|
|OrderIds |string |Order id, multiple orders are separated by commas (,) |Yes |
|sign |string | Signature |Yes|
|timestamp |long | timestamp **UTC Time** | yes |

**Return example**

```
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

**Return parameter description**

|Parameter name|Type|Description|
|:----- |:-----|----- |
|id |string |Order id |
|market |string |Market |
|userId |long |user id |
|currency |string |basic currency |
|orderType |int |Order Type (1 Buy at limit 2 Buy at Market 3 Sell at limit 4 Sell at Market) |
|price |double |price |
|volume |double |Quantity |
|amount |double |Order total |
|tradedVolume |double |Traded Quantity |
|tradeAmount |double |Total Transaction Amount |
|returnAmount |double |rollback amount |