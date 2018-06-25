# AZEX HTTP-API 

## 使用说明


API 域名：https://openapi.azex.io



### 行情API

------------


**简要描述：** 

- 获取平台所有币种

**请求URL：** 
- Get ` /Market/GetCurrencys `
  


**参数：** 

无

 **返回示例**

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

 **返回参数说明** 

|参数名|类型|说明|
|:-----  |:-----|-----                           |
|value | string[]   |币种  |



------------



**简要描述：** 

- 获取市场信息(精度等数据)

**请求URL：** 
- Get ` /Market/GetMarketInfos `
  


**参数：** 

|参数名|类型|说明|必填|
|:-----  |:-----|----- |-------|
|market |string   |市场交易对  |否 |


 **返回示例**

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
        "area": 1,
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

 **返回参数说明** 

|参数名|类型|说明|
|:-----  |:-----|-----                           |
|id | string   |市场id  |
|basic | string   |基本币  |
|target | string   |目标币  |
|makerFeeRate | float   |挂单手续费  |
|takerFeeRate | float   |吃单手续费  |
|minOrderAmount | float   |最小下单总额  |
|pricePrecision | float   |最大价格精度  |
|volumePrecision | float   |最大数量精度  |
|depthVolumePrecision | float   |深度精度  |


------------



**简要描述：** 

- 获取市场k线

**请求URL：** 
- Get ` /Market/GetKline `
  


**参数：** 

|参数名|类型|说明|必填|
|:-----  |:-----|----- |-------|
|market |string   |市场交易对  |是 |
|frequency |string   |k线周期类别，1，5，15，30，60，240，480，720，d  |是 |
|startTime |string   |开始时间 |是 |
|limit |int   |数据条目数量 范围1-200 默认200 |否 |


 **返回示例**

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

 **返回参数说明** 

|参数名|类型|说明|
|:-----  |:-----|-----                           |
|id | int   |k线id  |
|openTime | long   |开盘时间  |
|openPrice | string   |开盘价  |
|lowPrice | float   |最低价  |
|highPrice | float   |最高价  |
|closedPrice | float   |收盘价  |
|volume | float   |交易额  |



------------


**简要描述：** 

- 获取市场深度

**请求URL：** 
- Get ` /Market/GetDepth `
  


**参数：** 

|参数名|类型|说明|必填|
|:-----  |:-----|----- |-------|
|market |string   |市场交易对  |是 |
|precision |int   |市场精度  |是 |
|limit |int   |数据条目数量 范围1-40 默认40 |否 |


 **返回示例**

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

 **返回参数说明** 

|参数名|类型|说明|
|:-----  |:-----|-----                           |
|market | int   |k线id  |
|precision | int   |市场精度  |
|askList | string   |卖盘  |
|bidList | string   |买盘  |
|version | long   |深度版本号  |



------------

**简要描述：** 

- 获取市场24小时滚动行情

**请求URL：** 
- Get ` /Market/GetTiker `
  


**参数：** 

|参数名|类型|说明|必填|
|:-----  |:-----|----- |-------|
|market |string   |市场交易对  |是 |



 **返回示例**

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

 **返回参数说明** 

|参数名|类型|说明|
|:-----  |:-----|-----                           |
|Market | string   |市场  |
|openPrice | string   |开盘价  |
|lowPrice | float   |最低价  |
|highPrice | float   |最高价  |
|closedPrice | float   |收盘价  |
|volume | float   |交易额  |


------------

**简要描述：** 

- 获取最近成交记录

**请求URL：** 
- Get ` /Market/GetHistoryTrade `
  


**参数：** 

|参数名|类型|说明|必填|
|:-----  |:-----|----- |-------|
|market |string   |市场交易对  |是 |
|limit |int   |数据量 1-200， 默认200 |否 |



 **返回示例**

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

 **返回参数说明** 

|参数名|类型|说明|
|:-----  |:-----|-----                           |
|market | string   |市场  |
|id | int   |id  |
|price | float   |价格  |
|volume | float   |数量  |
|amount | float   |金额  |
|trend | int   |涨跌类型（1上涨2下跌3持平）  |


------------


## 私人API使用说明

私人api每次请求都需要构建一个签名参数sign与一个Authorization的请求头


### 签名

获取api：请在账号设置->Api管理->创建api，创建完成之后会生成一个Apikey和Secrect Key,请妥善管理， ** 重要提示：这两个密钥与账号安全紧密相关，无论何时都请勿向其他人透露。**

** Apikey用来标识用户，使用方法是在http请求头添加一个Authorization的请求头scheme=OPENAPI,value=apikey **

** Secrect Key用来生成sign参数 **

** sign 生成方法: ** 接口请求参数升序排序后得到的字符串，再使用Secrect Key作为密钥进行 HmacSHA256 计算得出

例

账户资产API请求参数为currencies=btc，得出加密字符串如下

```
currencies=btc&timestamp=1500000000
```

如Secrect Key为 2288987EFDB54F848D7BACCE1288FC9A，则计算得出sign值为
```
9a9f368f1499e228fc2b4036183ef8446f518d3cd5ce1dd167caff5ae91cb885
```






### 请求


 Content-Type 请指定为 application/x-www-form-urlencoded 
 

------------


**简要描述：** 

- 获取账户资产

**请求URL：** 
- Post ` /Account/Balance `
  


**参数：** 

|参数名|类型|说明|必填|
|:-----  |:-----|----- |-------|
|currencies |string   |币种，多个币种英文逗号隔开，为空查询所有币种  |否 |
|timestamp |long   |时间戳  |是|
|sign |string   |加密签名  |是|

 **返回示例**

``` 
{
  "isOk": true,
  "value": {
    "items": [
      {
        "id": "string",
        "userId": 0,
        "currency": "string",
        "address": "string",
        "balance": 0,
        "locked": 0,
        "mortgaged": 0,
        "totalAmount": 0,
        "status": 0,
        "createdAt": "2018-06-11T09:24:24.629Z"
      }
    ]
  },
  "err": {
    "code": 0,
    "message": "string"
  }
}
```

 **返回参数说明** 

|参数名|类型|说明|
|:-----  |:-----|-----                           |
|id |string   |币种id  |
|userId |string   |用户id  |
|currency |string   |币种代码  |
|address |string   |充值地址  |
|balance |double   |可用余额  |
|locked |double   |冻结余额  |
|mortgaged |double   |抵押金额  |
|totalAmount |double   |余额总额  |
|status |int   |账户状态 （0未激活1激活2锁定3禁用） |



------------


**简要描述：** 

- 市价与限价单下单

**请求URL：** 
- Post` /Order/Trade `


**参数：** 

|参数名|类型|说明|必填|
|:-----  |:-----|----- |-------|
|marketId |string   |市场 例 eth_btc  |是 |
|orderType |int   |订单类型（1限价买单2市价买单3限价卖单4市价卖单）  |是|
|price |string   |限价单价格  |是|
|volume |string   |限价单数量，市价卖单数量  |是|
|amount |string   |市价买单金额  |是|
|sign |string   |加密签名  |是|
|timestamp |long   |时间戳  |是|

 **返回示例**

``` 
{
    "isOk": true,
    "value": {
        "OrderId":"5b207fb09c1a0d82d80799dd",
        "Balance":123
    },
    "err": {
        "code": 0,
        "message": ""
    }
}
```

 **返回参数说明** 

|参数名|类型|说明|
|:-----  |:-----|-----                           |
|orderId |string   |订单id  |
|Balance |double   |下单币种余额  |


-----

**简要描述：** 

- 计划单下单

**请求URL：** 
- Post` /Order/SubmitPlanOrder `


**参数：** 

|参数名|类型|说明|必填|
|:-----  |:-----|----- |-------|
|marketId |string   |市场 例 eth_btc  |是 |
|orderType |int   |订单类型（1限价买单3限价卖单）  |是|
|TriggerPrice |float   |触发价格  |是|
|price |string   |挂单价格  |是|
|volume |string   |挂单数量 |是|
|sign |string   |加密签名  |是|
|timestamp |long   |时间戳  |是|

 **返回示例**

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

 **返回参数说明** 

|参数名|类型|说明|
|:-----  |:-----|-----                           |
|value |string   |订单id  |



----
**简要描述：** 

- 撤销订单

**请求URL：** 
- Post ` /Order/CancelOrder `


**参数：** 

|参数名|类型|说明|必填|
|:-----  |:-----|----- |-------|
|marketId |string   |市场 例 eth_btc  |是 |
|orderType |int   |订单类型（1限价买单2市价买单3限价卖单4市价卖单）  |是|
|orderCategory |int   |订单类型（1非计划单2计划单）  |是|
|orderId |string   |订单id  |是|
|sign |string   |加密签名  |是|
|timestamp |long   |时间戳  |是|

 **返回示例**

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

**简要描述：** 

- 批量撤销订单

**请求URL：** 
- Post ` /Order/BatchCancelOrder `
  


**参数：** 

|参数名|类型|说明|必填|
|:-----  |:-----|----- |-------|
|marketId |string   |市场 例 eth_btc  |是 |
|orderCategory |int   |订单类型（1非计划单2计划单）  |是|
|sign |string   |加密签名  |是|
|timestamp |long   |时间戳  |是|

 **返回示例**

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

**简要描述：** 

- 查询订单

**请求URL：** 
- Post` /Order/QueryOrder `
  


**参数：** 

|参数名|类型|说明|必填|
|:-----  |:-----|----- |-------|
|orderIds |string   |订单id，多个订单用英文逗号(,)隔开  |是 |
|sign |string   |加密签名  |是|
|timestamp |long   |时间戳  |是|

 **返回示例**

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

 **返回参数说明** 

|参数名|类型|说明|
|:-----  |:-----|-----                           |
|id |string   |订单id  |
|market |string   |市场  |
|userId |long   |用户id  |
|currency |string   |基本币  |
|orderType |int   |订单类型（1限价买单2市价买单3限价卖单4市价卖单）  |
|price |double   |价格  |
|volume |double   |数量  |
|amount |double   |订单总额  |
|tradedVolume |double   |已交易数量  |
|tradeAmount |double   |已交易总金额  |
|returnAmount |double   |回退金额  |
|status |int   |订单状态 （2交易中3订单已完成4已取消） |