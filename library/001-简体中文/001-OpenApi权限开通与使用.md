# OpenApi权限开通与使用

本文将阐述开发者在调用包含用户私人信息的API时所需要提前准备的工作和相关知识。

## 申请开通OpenAPI权限

1. 登录AZEX

  访问 <https://www.azex.io> 并登陆账号，进入"账号设置->API管理->添加API"，创建OpenApi。

2. 获得ApiKey和Secret

  成功通过二次认证之后，便可以获取ApiKey和Secret。

  **注意：ApiKey和Secret仅展现一次，请妥善保管。**

  **注意：ApiKey和Secret是关乎用户资产的重要数据，请妥善保管，谨防泄露。**

## 如何使用ApiKey和Secret

经过上节描述的过程，开发者已经获得了调用OpenApi所必要的两个参数 ApiKey 和 Secret。本节将阐述如何使用这两个值来调用OpenApi。

**再次强调：ApiKey和Secret是关乎用户资产的重要数据，请妥善保管，谨防泄露。**

### HTTP API

HTTP API 中有一类API与用户的个人资产有关，调用此类API时需要用到ApiKey和Secret对请求的数据进行签名。

#### 使用方法

##### ApiKey

ApiKey在请求时，直接在请求的Header中进行传递。假设 ApiKey 是 27783.xxxxxxxxxxx。则Header为

Name          | Value
------------- | -------------------------
Authorization | OPENAPI 27783.xxxxxxxxxxx

注意 OPENAPI 与 ApiKey之间存在一个空格。

##### Secret

Secret用于在请求时对请求的参数进行签名加密，用于确保AZEX在收到的请求确实是用户发出的，没有被篡改。详细的签名过程如下。

1).假设请求的参数名值对如下

Name | Value
---- | ---------------
b    | azex,is,perfect
a    | 1
as   | 3
ae   | 2
z    | 3.1415926

2).获取当前客户端的unix时间戳秒数，附加到请求参数中，则请求参数名值对变为如下

Name      | Value
--------- | ---------------
b         | azex,is,perfect
a         | 1
as        | 3
ae        | 2
z         | 3.1415926
timestamp | 1531137017

3).将以上的名值对按照Name进行升序排序，则排序结果如下，注意与第2步进行对比：

Name      | Value
--------- | ---------------
a         | 1
ae        | 2
as        | 3
b         | azex,is,perfect
timestamp | 1531137017
z         | 3.1415926

4).将上一步生成的名值对，使用`&`和`=`进行连接，连接出来的文本如下：

```bash
a=1&ae=2&as=3&b=azex,is,perfect&timestamp=1531137017&z=3.1415926
```

5).使用secret作为secret Key ，用第4步生成的字符串作为明文，进行`HmacSHA256`计算。便可以得出 sign 值。

假设 secret 是 `17184178f3334842a75c15c1d1d4e666`，则通过计算，第4步中字符串对应的 sign 为：

```bash
b72ba29328442e669851414cc0d894156dcee8c324b272b5819cc149ef877e58
```

6).将上一步生成的sign值和第3步中的所有数据，以form post的方式，进行提交，便可以完成调用。

7).至此完成了订阅用户个人交易信息所必须的认证过程，另外：

开发者可以使用本样例中的sign值与自己计算的sign值进行比对，以检测自己的处理方式是否得当，还可以通过右侧的在线工具，计算sign值：<https://www.freeformatter.com/hmac-generator.html>

开发者可以通过搜索引擎或右侧链接，查询unix时间戳秒数相应开发语言的处理方法：<https://www.epochconverter.com/>

### WebSocket

若需要通过WebSocket订阅用户个人相关的交易信息，同样需要使用到 ApiKey 和 secret。详细的步骤如下：

1). 假设用户的 ApiKey 是 `81.67AAA2F6041D408D9868387A8904431D` ，Secret 是 `2288987EFDB54F848D7BACCE1288FC9A` 。

2). 使用secret作为secret Key ，用 ApiKey 拼接为 `Authorization=81.67AAA2F6041D408D9868387A8904431D` ，进行`HmacSHA256`计算。便可以得出 sign 值。

```bash
057c4c6770d565aa236f87706053bd51512862443062e471bd3243a60ed8eef2
```

3).将 ApiKey 和 sign 拼接在 WebSocket 链接后作为参数，则最终生成的 WebSocket 链接为

```bash
wss://ws.azex.io?Authorization=81.67AAA2F6041D408D9868387A8904431D&sign=057c4c6770d565aa236f87706053bd51512862443062e471bd3243a60ed8eef2
```

4).至此完成了订阅用户个人交易信息所必须的认证过程。
