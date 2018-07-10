# AZEX HTTP-API

开发者可以通过该文档，了解如何通过HTTP调用的方式，实现获取行情数据和用户个人数据。

## 全局说明

本节所描述的内容属于下文的公共内容。开发者请先阅读该节所陈述内容。

### 请求参数

所有API均采用form表单提交的方式进行调用，仅支持 POST 和 GET 两种请求方式。

GET 请求参数在 QueryString 中进行传递。

POST 请求参数在 Body 中进行传递。

请求中必须包含以下两个Header

Name         | Value
------------ | ---------------------------------
Content-Type | application/x-www-form-urlencoded
Accept       | application/json

默认的，服务端返回的提示错误信息为英文提示，若需要改变提示内容，需要传递名为 Lang 的 Header。

相应的值与语言的对照关系如下：

值  | 语言
-- | ----
cn | 简体中文
en | 英文

### 响应结果

若相应服务端处理成功，将响应 2xx 系列响应码，并返回的响应正文中输出JSON，JSON 的格式一般如下所示：

```json
{
    "isOk": true,
    "value": []或{},
    "err": {
        "code": 0,
        "message": null
    }
}
```

其中各个属性的解释如下：

名称          | 类型           | 说明
----------- | ------------ | --------------------------------
isOk        | bool         | 该请求是否被处理成功。true:成功,false:失败
value       | object,array | 针对该请求处理结果，可能为数组、对象或空，需要参看具体请求的说明
err.code    | int          | 错误码，标识此错误的错误码
err.message | string       | 错误描述信息

### 市场Id

市场Id由两部分组成，基本币和目标币，中间采用下划线分隔。例如 btc_usdt 即表示 基本币为 usdt 目标币为 btc 。

### 时间格式

以下 API 中涉及到时间的作为传入或传出参数的位置，时间格式统一为unix时间戳秒数。

例如：北京时间 2018/7/9 15:25:13 对应的 unix时间戳秒数为 1531121113 。

开发者可以通过搜索引擎或右侧链接，查询相应开发语言的处理方法：<https://www.epochconverter.com/>

## 公共API

本节所陈述的API，属于公共API，无需用户进行身份认证便可调用。

### 获取平台所有币种

#### 请求URL

- GET `https://openapi.azex.io/Market/GetCurrencys`

#### 参数

无

#### 返回示例

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

#### 返回参数说明

参数名   | 类型       | 说明
----- | -------- | --
value | string[] | 币种

### 获取市场信息

#### 请求URL

- GET `https://openapi.azex.io/Market/GetMarketInfos`

#### 参数

参数名    | 类型     | 说明 | 必填
------ | ------ | -- | --
market | string | 市场 | 否

#### 返回示例

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

#### 返回参数说明

参数名                  | 类型     | 说明
-------------------- | ------ | -----------------------------------------
id                   | string | 市场id
basic                | string | 基本币
target               | string | 目标币
makerFeeRate         | double | 挂单手续费率。若返回0.05，则表示 5%。
takerFeeRate         | double | 吃单手续费率。若返回0.05，则表示 5%。
minOrderAmount       | double | 最小下单总额
pricePrecision       | double | 价格精度。若返回8，则表示，下单价格小数位不得超过8位
volumePrecision      | double | 数量精度。若返回8，则表示，下单价格小数位不得超过8位
depthVolumePrecision | double | 深度数量精度。若返回8，则表示，小数位不得超过8位小数
priceLimitPercent    | double | 下单价格涨幅限制。若返回0.25，则表示，下单价格不得大于当前市场价格的 25%。
status               | int    | 市场状态。2:已开放,4:已锁定,8:已关闭

### 获取市场k线

#### 请求URL

- GET `https://ws.azex.io/Market/GetKline`

#### 参数

参数名       | 类型     | 说明                                                             | 必填
--------- | ------ | -------------------------------------------------------------- | --
market    | string | 市场                                                             | 是
frequency | string | k线周期类别。可选值为：1，5，15，30，60，240，480，720，d。分别表示 1分钟、5分钟...720分钟、1天 | 是
startTime | long   | 开始时间                                                           | 是
limit     | int    | 数据条目数量，范围[1,200] 默认200                                         | 否

#### 返回示例

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

#### 返回参数说明

参数名         | 类型     | 说明
----------- | ------ | ----
id          | int    | k线id
openTime    | long   | 开盘时间
openPrice   | double | 开盘价
lowPrice    | double | 最低价
highPrice   | double | 最高价
closedPrice | double | 收盘价
volume      | double | 交易额

### 获取市场深度

#### 请求URL

- GET `https://ws.azex.io/Market/GetDepth`

#### 参数

参数名       | 类型     | 说明                                                                                                                        | 必填
--------- | ------ | ------------------------------------------------------------------------------------------------------------------------- | --
market    | string | 市场                                                                                                                        | 是
precision | int    | 价格精度。2表示2位小数。需要参照市场的pricePrecision。若pricePrecision大于2，则取值范围为 [pricePrecision-4,pricePrecision]，否则 取值范围为[0,pricePrecision] | 是
limit     | int    | 数据条目数量 范围[1,40] 默认40                                                                                                      | 否

#### 返回示例

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

#### 返回参数说明

参数名       | 类型      | 说明
--------- | ------- | ---------------------
market    | int     | k线id
precision | int     | 价格精度
askList   | object  | 卖盘列表，价低在前
bidList   | object  | 买盘列表，价高在前
p         | decimal | 价格
v         | decimal | 数量
version   | long    | 深度版本号，标识当前深度的版本号，逐渐增加

### 获取市场24小时滚动行情

#### 请求URL

- GET `https://ws.azex.io/Market/GetTiker`

#### 参数

参数名    | 类型     | 说明 | 必填
------ | ------ | -- | --
market | string | 市场 | 是

#### 返回示例

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

#### 返回参数说明

参数名         | 类型     | 说明
----------- | ------ | ---
Market      | string | 市场
openPrice   | string | 开盘价
lowPrice    | double | 最低价
highPrice   | double | 最高价
closedPrice | double | 收盘价
volume      | double | 交易额

### 获取最近成交记录

#### 请求URL

- GET `https://ws.azex.io/Market/GetHistoryTrade`

#### 参数

参数名    | 类型     | 说明                 | 必填
------ | ------ | ------------------ | --
market | string | 市场                 | 是
limit  | int    | 数据量 [1,200]， 默认200 | 否

#### 返回示例

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

#### 返回参数说明

参数名    | 类型     | 说明
------ | ------ | --------------------
id     | int    | 成交Id
market | string | 市场
price  | double | 成交价
volume | double | 成交量
amount | double | 成交额
trend  | int    | 涨跌类型（1:上涨,2:下跌,3:持平）

## 用户API

调用用户API需要身份认证信息才能调用。请先参看[OpenApi权限开通与令牌使用](?file=001-简体中文/001-OpenApi权限开通与使用 "OpenApi权限开通与使用")

### 获取账户资产

#### 请求URL

- POST `https://openapi.azex.io/Account/Balance`

#### 参数

参数名        | 类型     | 说明            | 必填
---------- | ------ | ------------- | --
currencies | string | 币种，多个币种英文逗号隔开 | 否
timestamp  | long   | unix时间戳       | 是
sign       | string | 加密签名          | 是

#### 返回示例

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

#### 返回参数说明

参数名              | 类型     | 说明
---------------- | ------ | ------------------------
currency         | string | 币种代码
lockedBalance    | double | 冻结余额
availableBalance | double | 可用余额
mortgagedBalance | double | 抵押余额
totalBalance     | double | 总余额 = 冻结余额 + 可用余额 + 抵押余额

### 市价与限价单下单

#### 请求URL

- POST `https://openapi.azex.io/Order/Trade`

#### 参数

参数名       | 类型     | 说明                                    | 必填
--------- | ------ | ------------------------------------- | --
marketId  | string | 市场                                    | 是
orderType | int    | 订单类型。可选值：1:限价买单,2:市价买单,3:限价卖单,4:市价卖单） | 是
price     | double | 限价单价格                                 | 是
volume    | double | 限价单数量，市价卖单数量                          | 是
amount    | double | 市价买单金额                                | 是
sign      | string | 加密签名                                  | 是
timestamp | long   | unix时间戳                               | 是

#### 返回示例

```json
{
    "isOk": true,
    "value": {
        "OrderId":"5b207fb09c1a0d82d80799dd",
        "balance":123
    },
    "err": {
        "code": 0,
        "message": ""
    }
}
```

#### 返回参数说明

参数名     | 类型     | 说明
------- | ------ | ------
orderId | string | 订单id
balance | double | 下单币种余额

### 计划单下单

#### 请求URL

- POST `https://openapi.azex.io/Order/SubmitPlanOrder`

#### 参数

参数名          | 类型     | 说明                  | 必填
------------ | ------ | ------------------- | --
marketId     | string | 市场 例 eth_btc        | 是
orderType    | int    | 订单类型（1:限价买单，3:限价卖单） | 是
triggerPrice | double | 触发价格                | 是
price        | double | 挂单价格                | 是
volume       | double | 挂单数量                | 是
sign         | string | 加密签名                | 是
timestamp    | long   | unix时间戳             | 是

#### 返回示例

```json
{
    "isOk": true,
    "value": "5b207fb09c1a0d82d80799dd",
    "err": {
        "code": 0,
        "message": ""
    }
}
```

#### 返回参数说明

参数名   | 类型     | 说明
----- | ------ | ----
value | string | 订单id

### 撤销订单

#### 请求URL

- POST `https://openapi.azex.io/Order/CancelOrder`

#### 参数

参数名           | 类型     | 说明                         | 必填
------------- | ------ | -------------------------- | --
marketId      | string | 市场 例 eth_btc               | 是
orderType     | int    | 订单类型（1限价买单2市价买单3限价卖单4市价卖单） | 是
orderCategory | int    | 订单类型（1非计划单2计划单）            | 是
orderId       | string | 订单id                       | 是
sign          | string | 加密签名                       | 是
timestamp     | long   | unix时间戳                    | 是

#### 返回示例

```json
{
    "isOk": true,
    "err": {
        "code": 0,
        "message": ""
    }
}
```

### 批量撤销订单

#### 请求URL

- POST `https://openapi.azex.io/Order/BatchCancelOrder`

#### 参数

参数名           | 类型     | 说明                     | 必填
------------- | ------ | ---------------------- | --
marketId      | string | 市场 例 eth_btc           | 是
orderCategory | int    | 订单类型，可选值：1:非计划单,2:计划单） | 是
sign          | string | 加密签名                   | 是
timestamp     | long   | unix时间戳                | 是

#### 返回示例

```json
{
    "isOk": true,
    "err": {
        "code": 0,
        "message": ""
    }
}
```

### 查询订单

#### 请求URL

- POST `https://openapi.azex.io/Order/QueryOrder`

#### 参数

参数名       | 类型     | 说明                  | 必填
--------- | ------ | ------------------- | --
orderIds  | string | 订单id，多个订单用英文逗号(,)隔开 | 是
sign      | string | 加密签名                | 是
timestamp | long   | unix时间戳             | 是

#### 返回示例

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

#### 返回参数说明

参数名          | 类型     | 说明
------------ | ------ | ---------------------------------
id           | string | 订单id
market       | string | 市场
userId       | long   | 用户id
currency     | string | 币种
category     | int    | 订单分类（1:非计划订单，2:计划订单）
orderType    | int    | 订单类型（1:限价买单，2:市价买单，3:限价卖单，4:市价卖单）
planType     | int    | 计划单触发类型（1:高价触发，2:低价触发）
triggerPrice | double | 计划单触发价
price        | double | 下单价
volume       | double | 下单量
amount       | double | 下单额
tradedVolume | double | 成交量
tradeAmount  | double | 成交额
returnAmount | double | 回退金额
status       | int    | 订单状态 （2:交易中,3:订单已完成,4:已取消）
createTime   | long   | 订单创建时间

以下对订单状态进行罗列说明：

订单分类    | 订单类型   | 计划单触发类型 | 订单功能
------- | ------ | ------- | ----------------------------
1:非计划订单 | 1:限价买单 | 无       | 限价买单
1:非计划订单 | 2:市价买单 | 无       | 市价买单
1:非计划订单 | 3:限价卖单 | 无       | 限价卖单
1:非计划订单 | 4:市价卖单 | 无       | 市价卖单
2:计划订单  | 1:限价买单 | 1:高价触发  | 市场价高于triggerPrice时，自动下一个限价买单
2:计划订单  | 1:限价买单 | 2:低价触发  | 市场价低于triggerPrice时，自动下一个限价买单
2:计划订单  | 3:限价卖单 | 1:高价触发  | 市场价高于triggerPrice时，自动下一个限价卖单
2:计划订单  | 3:限价卖单 | 2:低价触发  | 市场价低于triggerPrice时，自动下一个限价卖单
