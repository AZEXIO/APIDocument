#AZEX 商户-API 

##请求

###1.创建充值地址

创建充值地址接口因为业务原因，不是一个实时返回结果的接口，当充值地址真正创建成功之后会调用商户的回调接口进行通知

**请求URL：** 
- Post ` https://api.azex.io/MerchantApi/Merchant/GenerateAddress `

  Content-Type 请指定为 application/x-www-form-urlencoded


**参数：** 

|参数名|类型|说明|必填|
|:-----  |:-----|----- |-------|
|merchantId |long   |商户id  |否 |
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


##回调
回调请求是由azex服务器调用客户端的http请求


### 1.钱包地址创建成功回调通知



**请求URL：** 
- Post ` 商户回调地址 `
  
  


**参数：** 

|参数名|类型|说明|必填|
|:-----  |:-----|----- |-------|
|id |long   |id  |否 |
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



###2.充值到账回调



**请求URL：** 
- Post ` 商户回调地址 `
  
  


**参数：** 

|参数名|类型|说明|必填|
|:-----  |:-----|----- |-------|
|id |long   |id  |否 |
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


##签名说明



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



