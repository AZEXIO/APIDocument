# AZEX HTTP-API 

##ʹ��˵��


API ������https://api.azex.io



###����API

------------


**��Ҫ������** 

- ��ȡƽ̨���б���

**����URL��** 
- Get ` /Market/GetCurrencys `
  


**������** 

��

 **����ʾ��**

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

 **���ز���˵��** 

|������|����|˵��|
|:-----  |:-----|-----                           |
|value | string[]   |����  |



------------



**��Ҫ������** 

- ��ȡ�г���Ϣ(���ȵ�����)

**����URL��** 
- Get ` /Market/GetMarketInfos `
  


**������** 

|������|����|˵��|����|
|:-----  |:-----|----- |-------|
|market |string   |�г����׶�  |�� |


 **����ʾ��**

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

 **���ز���˵��** 

|������|����|˵��|
|:-----  |:-----|-----                           |
|id | string   |�г�id  |
|basic | string   |������  |
|target | string   |Ŀ���  |
|makerFeeRate | float   |�ҵ�������  |
|takerFeeRate | float   |�Ե�������  |
|minOrderAmount | float   |��С�µ��ܶ�  |
|pricePrecision | float   |���۸񾫶�  |
|volumePrecision | float   |�����������  |
|depthVolumePrecision | float   |��Ⱦ���  |


------------



**��Ҫ������** 

- ��ȡ�г�k��

**����URL��** 
- Get ` /Market/GetKline `
  


**������** 

|������|����|˵��|����|
|:-----  |:-----|----- |-------|
|market |string   |�г����׶�  |�� |
|frequency |string   |k���������1��5��15��30��60��240��480��720��d  |�� |
|startTime |string   |��ʼʱ�� |�� |
|limit |int   |������Ŀ���� ��Χ1-200 Ĭ��200 |�� |


 **����ʾ��**

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

 **���ز���˵��** 

|������|����|˵��|
|:-----  |:-----|-----                           |
|id | int   |k��id  |
|openTime | long   |����ʱ��  |
|openPrice | string   |���̼�  |
|lowPrice | float   |��ͼ�  |
|highPrice | float   |��߼�  |
|closedPrice | float   |���̼�  |
|volume | float   |���׶�  |



------------


**��Ҫ������** 

- ��ȡ�г����

**����URL��** 
- Get ` /Market/GetDepth `
  


**������** 

|������|����|˵��|����|
|:-----  |:-----|----- |-------|
|market |string   |�г����׶�  |�� |
|precision |int   |�г�����  |�� |
|limit |int   |������Ŀ���� ��Χ1-40 Ĭ��40 |�� |


 **����ʾ��**

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

 **���ز���˵��** 

|������|����|˵��|
|:-----  |:-----|-----                           |
|market | int   |k��id  |
|precision | int   |�г�����  |
|askList | string   |����  |
|bidList | string   |����  |
|version | long   |��Ȱ汾��  |



------------

**��Ҫ������** 

- ��ȡ�г�24Сʱ��������

**����URL��** 
- Get ` /Market/GetTiker `
  


**������** 

|������|����|˵��|����|
|:-----  |:-----|----- |-------|
|market |string   |�г����׶�  |�� |



 **����ʾ��**

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

 **���ز���˵��** 

|������|����|˵��|
|:-----  |:-----|-----                           |
|Market | string   |�г�  |
|openPrice | string   |���̼�  |
|lowPrice | float   |��ͼ�  |
|highPrice | float   |��߼�  |
|closedPrice | float   |���̼�  |
|volume | float   |���׶�  |


------------

**��Ҫ������** 

- ��ȡ����ɽ���¼

**����URL��** 
- Get ` /Market/GetHistoryTrade `
  


**������** 

|������|����|˵��|����|
|:-----  |:-----|----- |-------|
|market |string   |�г����׶�  |�� |
|limit |int   |������ 1-200�� Ĭ��200 |�� |



 **����ʾ��**

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

 **���ز���˵��** 

|������|����|˵��|
|:-----  |:-----|-----                           |
|market | string   |�г�  |
|id | int   |id  |
|price | float   |�۸�  |
|volume | float   |����  |
|amount | float   |���  |
|trend | int   |�ǵ����ͣ�1����2�µ�3��ƽ��  |


------------


##˽��APIʹ��˵��

˽��apiÿ��������Ҫ����һ��ǩ������sign��һ��Authorization������ͷ

###ǩ��

��ȡapi�������˺�����->Api����->����api���������֮�������һ��Apikey��Secrect Key,�����ƹ��� ** ��Ҫ��ʾ����������Կ���˺Ű�ȫ������أ����ۺ�ʱ��������������͸¶��**

** Apikey������ʶ�û���ʹ�÷�������http����ͷ���һ��Authorization������ͷscheme=OPENAPI,value=apikey **

** Secrect Key��������sign���� **

** sign ���ɷ���: ** �ӿ�����������������õ����ַ�������ʹ��Secrect Key��Ϊ��Կ���� HmacSHA256 ����ó�

��

�˻��ʲ�API�������Ϊcurrencies=btc���ó������ַ�������

```
currencies=btc&timestamp=1500000000
```

��Secrect KeyΪ 2288987EFDB54F848D7BACCE1288FC9A�������ó�signֵΪ
```
9a9f368f1499e228fc2b4036183ef8446f518d3cd5ce1dd167caff5ae91cb885
```






###����


 Content-Type ��ָ��Ϊ application/x-www-form-urlencoded 
 

------------


**��Ҫ������** 

- ��ȡ�˻��ʲ�

**����URL��** 
- Post ` /Account/Balance `
  


**������** 

|������|����|˵��|����|
|:-----  |:-----|----- |-------|
|currencies |string   |���֣��������Ӣ�Ķ��Ÿ�����Ϊ�ղ�ѯ���б���  |�� |
|timestamp |long   |ʱ���  |��|
|sign |string   |����ǩ��  |��|

 **����ʾ��**

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

 **���ز���˵��** 

|������|����|˵��|
|:-----  |:-----|-----                           |
|id |string   |����id  |
|userId |string   |�û�id  |
|currency |string   |���ִ���  |
|address |string   |��ֵ��ַ  |
|balance |double   |�������  |
|locked |double   |�������  |
|mortgaged |double   |��Ѻ���  |
|totalAmount |double   |����ܶ�  |
|status |int   |�˻�״̬ ��0δ����1����2����3���ã� |



------------


**��Ҫ������** 

- �м����޼۵��µ�

**����URL��** 
- Post` /Order/Trade `


**������** 

|������|����|˵��|����|
|:-----  |:-----|----- |-------|
|marketId |string   |�г� �� eth_btc  |�� |
|orderType |int   |�������ͣ�1�޼���2�м���3�޼�����4�м�������  |��|
|price |string   |�޼۵��۸�  |��|
|volume |string   |�޼۵��������м���������  |��|
|amount |string   |�м��򵥽��  |��|
|sign |string   |����ǩ��  |��|
|timestamp |long   |ʱ���  |��|

 **����ʾ��**

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

 **���ز���˵��** 

|������|����|˵��|
|:-----  |:-----|-----                           |
|value |string   |����id  |


-----

**��Ҫ������** 

- �ƻ����µ�

**����URL��** 
- Post` /Order/SubmitPlanOrder `


**������** 

|������|����|˵��|����|
|:-----  |:-----|----- |-------|
|marketId |string   |�г� �� eth_btc  |�� |
|orderType |int   |�������ͣ�1�޼���3�޼�������  |��|
|TriggerPrice |float   |�����۸�  |��|
|price |string   |�ҵ��۸�  |��|
|volume |string   |�ҵ����� |��|
|sign |string   |����ǩ��  |��|
|timestamp |long   |ʱ���  |��|

 **����ʾ��**

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

 **���ز���˵��** 

|������|����|˵��|
|:-----  |:-----|-----                           |
|value |string   |����id  |



----
**��Ҫ������** 

- ��������

**����URL��** 
- Post ` /Order/CancelOrder `


**������** 

|������|����|˵��|����|
|:-----  |:-----|----- |-------|
|marketId |string   |�г� �� eth_btc  |�� |
|orderType |int   |�������ͣ�1�޼���2�м���3�޼�����4�м�������  |��|
|orderCategory |int   |�������ͣ�1�Ǽƻ���2�ƻ�����  |��|
|orderId |string   |����id  |��|
|sign |string   |����ǩ��  |��|
|timestamp |long   |ʱ���  |��|

 **����ʾ��**

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

**��Ҫ������** 

- ������������

**����URL��** 
- Post ` /Order/BatchCancelOrder `
  


**������** 

|������|����|˵��|����|
|:-----  |:-----|----- |-------|
|marketId |string   |�г� �� eth_btc  |�� |
|orderCategory |int   |�������ͣ�1�Ǽƻ���2�ƻ�����  |��|
|sign |string   |����ǩ��  |��|
|timestamp |long   |ʱ���  |��|

 **����ʾ��**

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

**��Ҫ������** 

- ��ѯ����

**����URL��** 
- Post` /Order/QueryOrder `
  


**������** 

|������|����|˵��|����|
|:-----  |:-----|----- |-------|
|OrderIds |string   |����id�����������Ӣ�Ķ���(,)����  |�� |
|sign |string   |����ǩ��  |��|
|timestamp |long   |ʱ���  |��|

 **����ʾ��**

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

 **���ز���˵��** 

|������|����|˵��|
|:-----  |:-----|-----                           |
|id |string   |����id  |
|market |string   |�г�  |
|userId |long   |�û�id  |
|currency |string   |������  |
|orderType |int   |�������ͣ�1�޼���2�м���3�޼�����4�м�������  |
|price |double   |�۸�  |
|volume |double   |����  |
|amount |double   |�����ܶ�  |
|tradedVolume |double   |�ѽ�������  |
|tradeAmount |double   |�ѽ����ܽ��  |
|returnAmount |double   |���˽��  |
|status |int   |����״̬ ��2������3���������4��ȡ���� |