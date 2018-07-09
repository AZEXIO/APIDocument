# AZEX 商户-API

## 请求

### 1.创建充值地址

创建充值地址接口因为业务原因，不是一个实时返回结果的接口，当充值地址真正创建成功之后会调用商户的回调接口进行通知

**请求URL：**
- Post ` https://api.azex.io/MerchantApi/Merchant/GenerateAddress `

  Content-Type 请指定为 application/x-www-form-urlencoded


**参数：**

|参数名|类型|说明|必填|
|:-----  |:-----|----- |-------|
|merchantId |int   |商户id  |是 |
|currency |string   |币种  |是|
|timestamp |long   |时间戳，**UTC时间**  |是|
|sign |string   |[加密签名说明](/#签名说明)  |是|

**请求示例**
```
merchantid=1&currency=btc&timestamp=100000&sign=xxx
```

 **返回示例**

```
{
  "isOk": true,
  "err": {
    "code": 0,
    "message": "string"
  }
}
```
---
### 2.提现地址校验

当校验请求成功之后会调用商户的回调接口通知校验结果

**请求URL：**
- Post ` https://api.azex.io/MerchantApi/Merchant/WithdrawAddressValidation `

  Content-Type 请指定为 application/x-www-form-urlencoded


**请求参数：**

|参数名|类型|说明|必填|
|:-----  |:-----|----- |-------|
|merchantId |int   |商户id  |是 |
|address |string   |提现地址  |是|
|memo |string   |提现memo  |是|
|timestamp |long   |时间戳，**UTC时间**  |是|
|sign |string   |[加密签名说明](/#签名说明)  |是|

**请求示例**
```
address=xxx&memo=xxx&merchantid=1&timestamp=100000&sign=xxx
```

 **返回示例**

```
{
  "isOk": true,
  "err": {
    "code": 0,
    "message": "string"
  }
}
```


---
### 3.提现

当提现请求成功之后会调用商户的回调接口进行通知

**请求URL：**
- Post ` https://api.azex.io/MerchantApi/Merchant/Withdraw `

  Content-Type 请指定为 application/x-www-form-urlencoded


**请求参数：**

|参数名|类型|说明|必填|
|:-----  |:-----|----- |-------|
|merchantId |int   |商户id  |是 |
|currency |string   |币种  |是|
|volume |string   |数额  |是|
|address |string   |提现地址  |是|
|memo |string   |提现memo  |是|
|timestamp |long   |时间戳，**UTC时间**  |是|
|sign |string   |[加密签名说明](/#签名说明)  |是|

**请求示例**
```
address=xxx&currency=btc&memo=xxx&merchantid=1&volume=666&timestamp=100000&sign=xxx
```

 **返回示例**

```
{
  "isOk": true,
  "value":{
  	"WithdrawId":"56809_btc/56809_btc_2/btc",
	"fee":0.2
  },
  "err": {
    "code": 0,
    "message": "string"
  }
}
```
**返回参数**

|参数名|类型|说明|必填|
|:-----  |:-----|----- |-------|
|WithdrawId |string   |提现记录id  |是 |
|fee |double   |提现手续费  |是|

---

### 4.提现状态查询


**请求URL：**
- Post ` https://api.azex.io/MerchantApi/Merchant/Withdraw `

  Content-Type 请指定为 application/x-www-form-urlencoded


**请求参数：**

|参数名|类型|说明|必填|
|:-----  |:-----|----- |-------|
|merchantId |int   |商户id  |是 |
|withdrawId |string   |币种  |是|
|timestamp |long   |时间戳，**UTC时间**  |是|
|sign |string   |[加密签名说明](/#签名说明)  |是|

**请求示例**
```
merchantid=1&withdrawId=xxx&timestamp=100000&sign=xxx
```

 **返回示例**

```
{
  "isOk": true,
  "value":{
  	"id": "string",
	"currency": "string",
	"address": "string",
	"volume": 0,
	"fee": 0,
	"feeCurrency": "string",
	"memo": "string",
	"tag": "string",
	"txNo": "string",
	"validResult": 0,
	"status": 0,
	"createdAt": "2018-07-02T07:49:53.416Z",
	"doneAt": "2018-07-02T07:49:53.416Z"
  },
  "err": {
    "code": 0,
    "message": "string"
  }
}
```
**返回参数**

|参数名|类型|说明|必填|
|:-----  |:-----|----- |-------|
|id |string   | WithdrawId  |是 |
|currency |long   |提现币种 id  |是|
|address |string   |提现地址  |是|
|volume |double   |提现数额  |是|
|fee |double   |提现手续费  |是|
|feeCurrency |string   |提现手续费币种  |是|
|memo |string   |备注码  |是|
|tag |string   |提币地址别名  |是|
|txNo |string   |交易流水号  |是|
|validResult |int   |审核结果（1超过日限额，2余额不足，3需要人工处理）  |是|
|status |int   |提现状态（1开始2等待处理3正在处理4完成7驳回） |是|
|createdAt |long   |创建时间  |是|
|doneAt |long   |完成时间  |是|

---


## 回调
回调请求是由azex服务器调用客户端的http请求


### 1.钱包地址创建成功回调通知



**请求URL：**
- Post ` 商户回调地址 `




**参数：**

|参数名|类型|说明|必填|
|:-----  |:-----|----- |-------|
|id |long   |id  |是 |
|currency |string   |币种  |是|
|address |string   |钱包地址  |是|
|memo |string   |memo  |是|
|timestamp |long   |时间戳，**UTC时间**  |是|
|sign |string   |[加密签名说明](/#签名说明) |是|

**请求示例**
```
id=123&address=xxxx&memo=xxxx&currency=btc&timestamp=100000&sign=xxx
```

---



### 2.充值到账回调



**请求URL：**
- Post ` 商户回调地址 `




**参数：**

|参数名|类型|说明|必填|
|:-----  |:-----|----- |-------|
|id |long   |id  |是 |
|currency |string   |币种  |是|
|address |string   |钱包地址  |是|
|memo |string   |memo  |是|
|volume |string   |到账额  |是|
|fee |string   |手续费  |是|
|timestamp |long   |时间戳，**UTC时间**  |是|
|sign |string   |[加密签名说明](/#签名说明) |是|

**请求示例**
```
id=123&address=xxxx&memo=xxxx&currency=btc&volume=10&fee=0.01&timestamp=100000&sign=xxx
```

---

### 3.提现地址验证回调



**请求URL：**
- Post ` 商户回调地址 `




**参数：**

|参数名|类型|说明|必填|
|:-----  |:-----|----- |-------|
|isvalid |bool   |验证是否通过  |是 |
|address |string   |钱包地址  |是|
|memo |string   |memo  |是|
|timestamp |long   |时间戳，**UTC时间**  |是|
|sign |string   |[加密签名说明](/#签名说明) |是|

**请求示例**
```
isvalid=true&address=xxxx&memo=xxxx&timestamp=100000&sign=xxx
```

---
### 4.提现状态变更回调

当提现申请被驳回或者通过时回调通知商户

**请求URL：**
- Post ` 商户回调地址 `




**参数：**

|参数名|类型|说明|必填|
|:-----  |:-----|----- |-------|
|withdrawId |bool   |验证是否通过  |是 |
|status |string   |提现状态(1通过3驳回)  |是|
|timestamp |long   |时间戳，**UTC时间**  |是|
|sign |string   |[加密签名说明](/#签名说明) |是|

**请求示例**
```
withdrawId=xxx&status=1&timestamp=100000&sign=xxx
```

---


## 签名说明



每个商户有一个商户的标识**merchantid**,密钥**secret**

** sign 生成方法: ** 接口请求参数升序排序后得到的字符串，再使用secret作为密钥进行 HmacSHA256 计算得出

例

创建充值地址API请求参数为merchantId=1&currency=btc，得出加密字符串如下

```
currencies=btc&merchantId=1&timestamp=1500000000
```

如Secrect Key为 2288987EFDB54F848D7BACCE1288FC9A，则计算得出sign值为
```
74abca47f563b04bcd987c396d5667df876e2a649a9a5615dea0f7783afb9923
```





------------



