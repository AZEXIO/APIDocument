# AZEX Websocket 

##ʹ��˵��


������wss://ws.azex.io

**���л�**

**��Ϣ����**����Ϣ������ɰ���**·�ɣ������Ϣ��**��������

������Ϣ��ǰ�ĸ��ֽڣ�int32��Ϊ**·��** 
������Ϣ�ĵ�����������ֽ�Ϊ**����**��int16��


**��Ϣ����**�����������ص���Ϣ��**�����Ϣ��**�����������

���յ���Ϣǰ�����ֽ�Ϊ**����**��int16��

**��Ϣ��**�����л�ͨ��[protobuf](https://developers.google.com/protocol-buffers/ "protobuf")

websocket-protobuf[�ļ�����](https://developers.google.com/protocol-buffers/ "�ļ�����")

��
```
//�����г����
message SubMarketDepth{
    //�г�
    required string Market= 1 ;
    //С��λ����
    required int32 Precision= 2 ;
    //��ȡ��������ѡֵΪ��10,20,40
    required int32 Limit= 3 ;
}
message MarketDepthList{
    //�г�����б�
    repeated MarketDepthDto List= 1 ;
}
//�г���ȿ���
message MarketDepthDto{
    //�г�
    required string Market= 1 ;
    //����
    required int32 Precision= 2 ;
    //�����б�
    repeated MarketDepth AskList= 3 ;
    //���б�
    repeated MarketDepth BidList= 4 ;
    //�汾��
    required int64 Version= 5 ;
}
```
���ʹ��루α���룩����

```
websocket ws;
protobuf protbuf;
int16 SendCommand = 906; //��������г����
int32 route = 1; //·��
SubMarketDepth subMarket;
subMarket.Market = "eth_btc";
subMarket.Precision = 8;
subMarket.Limit = 10;

ws.send(byteof(route)+byteof(SendCommand)+protbuf.Serialize(subMarket));
```

���մ��루α���룩����
```
OnWebsocketReceived��byte[] data��{
	
	int16 receiveCommand = ConvertToInt16��data.sub(0,2))
	switch(receiveCommand)
	case 1003
		MarketDepthList dto = protbuf.Deserialize(data.sub(2,data.length))
	//ҵ�����
	...
}
```

----
###�����б�

����         | �ύ����                       | ��������                                     | �ô�           | ��ע
---------- | -------------------------- | ---------------------------------------- | ------------ | -------------------------------------------------------------------------
���ĳɽ���¼     | 907       | 1005                       | ����ɽ�         |
��ȡ����K������(�ɶ���)   | 900           | 1000,1001 , 1002      | K��ͼ          | 1000��������k�ߣ�1001������ʷk�ߣ�1002����k�������������
�������µ���K������   | 902 | 1000                              | K��ͼ          | Frequencys ��ѡֵ "1", "5", "15", "30", "60", "180", "360", "720", "D", "7D"
����24Сʱ�������� | 902 | 1006                           | �ұҽ���-�г����顢��ҳ | Frequencys Ϊ SD1
�����г����     | 906             | 1003��1004            | �ұҽ���-���¼۸�    |1003��ȿ��գ�1004��Ȳ�������
���ĸ��˶�����Ϣ     | 1000                          | 1008��1009��1010 | ���� �ұҽ���-��ǰί�� |1008����������1009�������£�1010�ƻ�������
������Ϣ| |0|������Ϣ|



##API�ο�
###����API

���ĳɽ���¼


** ������Ϣ **

** ·�ɣ�1**
** ���907 ** 
���������

```
message GetTopTradeList{
    //�г�
    required string Market= 1 ;
    //����
    required int32 Count= 2 ;
    //�Ѷ���
    required bool Subscribe= 3 ;
}
```
����˵��

|������|����|˵��|����|protobuf����|
|:-----  |:-----|----- |-------|----|
|market |string   |�г����׶�  |�� |1|
|Count |string   | �������������99���� |�� |2|
|subscribe |bool   |�Ƿ���  |�� |3|


** ������Ϣ **

** ���1005**
```
message TradeSimpleDtoList{
    //��������
    repeated TradeSimpleData List= 1 ;
}
//�ɽ���Ϣ
message TradeSimpleData{
    //�г�
    required string Market= 1 ;
    //�ɽ�Id
    required int64 Id= 2 ;
    //�ɽ���
    required double Price= 3 ;
    //�ɽ���
    required double Volume= 4 ;
    //�ɽ���
    required double Amount= 5 ;
    //�ǵ�����
    required int32 Trend= 6 ;
    //�ɽ�ʱ��
    required int64 CreateTime= 7 ;
}
```
|������|����|˵��|protobuf����|
|:-----  |:-----|-----|----|
|market |string   |�г����׶�  |1
|Id |string   | |2|
|Price |double   |�ɽ��� |3|
|Volume |double   |�ɽ���  |4|
|Amount |double   |�ɽ����  |5|
|Trend |int   |�ǵ����ͣ�1����2�µ�3��ƽ��  |6|
|CreateTime |long   |�ɽ�ʱ��  |7|


----
������ʷk��
** ������Ϣ **

** ·�ɣ�1**
** ���900 **
���������

```
message GetKLineList{
    //�г�
    required string Market= 1 ;
    //K������
    required string Frequency= 2 ;
    //��ʼʱ��
    required int64 Start= 3 ;
    //����ʱ��
    required int64 End= 4 ;
    //�Ѷ���
    required bool Subscribe= 5 [default = true];
}
```
����˵��

|������|����|˵��|����|protobuf����|
|:-----  |:-----|----- |-------|----|
|market |string   |�г����׶�  |�� |1
|frequency |string   |Frequencys ��ѡֵ ��1��, ��5��, ��15��, ��30��, ��60��, ��180��, ��360��, ��720��, ��D��, ��7D��  |�� |2|
|start |long   |��ѯ��ʼʱ��  |�� |3|
|end |long   |��ѯ����ʱ��  |�� |4|
|subscribe |bool   | ������֮���Ƿ���k��  |�� |5|

** ������Ϣ **

** ���1001**

```
message WsKLineList{
    //K������
    repeated WsKLine List= 1 ;
}

message WsKLine{
    //�г�
    required string Market= 1 ;
    //K������
    required string Frequency= 2 ;
    //�ɽ���
    required double Volume= 3 ;
    //���̼�
    required double OpenPrice= 4 ;
    //���̼�
    required double ClosedPrice= 5 ;
    //��ͼ�
    required double LowPrice= 6 ;
    //��߼�
    required double HighPrice= 7 ;
    //����ʱ��
    required int64 OpenTime= 8 ;
}
```
����˵��

|������|����|˵��|protobuf����|
|:-----  |:-----|-----|----|
|market |string   |�г����׶�  |1
|frequency |string   |Frequencys ��ѡֵ ��1��, ��5��, ��15��, ��30��, ��60��, ��180��, ��360��, ��720��, ��D��, ��7D��  |2|
|volume |double   |�ɽ��� |3|
|openPrice |double   |���̼�  |4|
|closedPrice |double   |���̼�  |5|
|lowPrice |double   |��ͼ�  |6|
|highPrice |double   |��߼�  |7|
|openTime |long   |����ʱ��  |8|

** ���1002** //���ݽ������
```
message BatchSendComplate{
    //�г�
    required string Market= 1 ;
    //K������
    required string Frequency= 2 ;
    //
    required int64 Start= 3 ;
    //
    required int64 End= 4 ;
    //�Ƿ񵽴�K�ߵ����λ��
    required bool IsStart= 5 ;
}
```
����˵��

|������|����|˵��|protobuf����|
|:-----  |:-----|-----|----|
|market |string   |�г����׶� |1
|frequency |string   |Frequencys ֵ ��1��, ��5��, ��15��, ��30��, ��60��, ��180��, ��360��, ��720��, ��D��, ��7D��  |2|
|start |long   |��ѯ��ʼʱ��  |3|
|end |long   |��ѯ����ʱ��  |4|
|isStart |bool   |�Ƿ���ȡ��������ʷk��  |5|


----
����k��

** ������Ϣ **

** ·�ɣ�1**
** ���902 ** 
���������

```
message SubKLine{
    //����ϸ��
    repeated SubKLineItem Items= 1 ;
}
//K�߶��Ĳ���
message SubKLineItem{
    //�г�
    required string Market= 1 ;
    //K������
    repeated string Frequencys= 2 ;
}
```
����˵��

|������|����|˵��|����|protobuf����|
|:-----  |:-----|----- |-------|----|
|market |string   |�г����׶�  |�� |1
|frequency |string   |Frequencys ��ѡֵ ��1��, ��5��, ��15��, ��30��, ��60��, ��180��, ��360��, ��720��, ��D��, ��7D��  |�� |2|


** ������Ϣ **

** ���1000**
```
message WsKLine{
    //�г�
    required string Market= 1 ;
    //K������
    required string Frequency= 2 ;
    //�ɽ���
    required double Volume= 3 ;
    //���̼�
    required double OpenPrice= 4 ;
    //���̼�
    required double ClosedPrice= 5 ;
    //��ͼ�
    required double LowPrice= 6 ;
    //��߼�
    required double HighPrice= 7 ;
    //����ʱ��
    required int64 OpenTime= 8 ;
}
```
|������|����|˵��|protobuf����|
|:-----  |:-----|-----|----|
|market |string   |�г����׶�  |1
|frequency |string   |Frequencys ��ѡֵ ��1��, ��5��, ��15��, ��30��, ��60��, ��180��, ��360��, ��720��, ��D��, ��7D��  |2|
|volume |double   |�ɽ��� |3|
|openPrice |double   |���̼�  |4|
|closedPrice |double   |���̼�  |5|
|lowPrice |double   |��ͼ�  |6|
|highPrice |double   |��߼�  |7|
|openTime |long   |����ʱ��  |8|

----

����24Сʱ��������

** ������Ϣ **

** ·�ɣ�1**
** ���902 **
���������

```
message SubKLine{
    //����ϸ��
    repeated SubKLineItem Items= 1 ;
}
//K�߶��Ĳ���
message SubKLineItem{
    //�г�
    required string Market= 1 ;
    //K������
    repeated string Frequencys= 2 ;
}
```
����˵��

|������|����|˵��|����|protobuf����|
|:-----  |:-----|----- |-------|----|
|market |string   |�г����׶�  |�� |1
|frequency |string   |Frequencys��ֵֻ��ΪSD1��Ϊ��������k�����֣� |�� |2|


** ������Ϣ **

** ���1000**
```
message WsKLine{
    //�г�
    required string Market= 1 ;
    //K������
    required string Frequency= 2 ;
    //�ɽ���
    required double Volume= 3 ;
    //���̼�
    required double OpenPrice= 4 ;
    //���̼�
    required double ClosedPrice= 5 ;
    //��ͼ�
    required double LowPrice= 6 ;
    //��߼�
    required double HighPrice= 7 ;
    //����ʱ��
    required int64 OpenTime= 8 ;
}
```
|������|����|˵��|protobuf����|
|:-----  |:-----|-----|----|
|market |string   |�г����׶�  |1
|frequency |string   |Frequencys ��ѡֵ ��1��, ��5��, ��15��, ��30��, ��60��, ��180��, ��360��, ��720��, ��D��, ��7D��  |2|
|volume |double   |�ɽ��� |3|
|openPrice |double   |���̼�  |4|
|closedPrice |double   |���̼�  |5|
|lowPrice |double   |��ͼ�  |6|
|highPrice |double   |��߼�  |7|
|openTime |long   |����ʱ��  |8|


----

�����г����

��ȶ���Ϊ�����������ͣ���һ�ζ��Ļ᷵��ȫ�����ݣ�֮��ֻ���Ͳ�������

����ȽϷ�����1.�³��ֵļ۸񣬱�ʾ������ȡ�2.�ѳ��ֵļ۸񣬹ҵ�����Ϊ0����ʾ����.3.�ѳ��ֵļ۸񣬹ҵ���Ϊ0����ʾɾ������ȡ�=====��StartVersion���г���ȿ��յİ汾��ͬ����Ҫ���»�ȡ�г���ȿ���

** ������Ϣ **

** ·�ɣ�1**
** ���906 **
���������

```
message SubMarketDepth{
    //�г�
    required string Market= 1 ;
    //С��λ����
    required int32 Precision= 2 ;
    //��ȡ��������ѡֵΪ��10,20,40
    required int32 Limit= 3 ;
}
```
����˵��

|������|����|˵��|����|protobuf����|
|:-----  |:-----|----- |-------|----|
|market |string   |�г����׶�  |�� |1
|precision |int   |С��λ���� |�� |2|
|limit |int   |��ȡ��������ѡֵΪ��10,20,40 |�� |3|


** ������Ϣ **

** ���1003** //��ȿ��գ���һ�ζ���ʱ������һ��
```
message MarketDepthList{
    //�г�����б�
    repeated MarketDepthDto List= 1 ;
}
//�г���ȿ���
message MarketDepthDto{
    //�г�
    required string Market= 1 ;
    //����
    required int32 Precision= 2 ;
    //�����б�
    repeated MarketDepth AskList= 3 ;
    //���б�
    repeated MarketDepth BidList= 4 ;
    //�汾��
    required int64 Version= 5 ;
}
message MarketDepth{
    //����
    required double P= 1 ;
    //����
    required double V= 2 ;
}
```
|������|����|˵��|protobuf����|
|:-----  |:-----|-----|---|
|market |string   |�г����׶�  |1
|precision |string   |�г�����  |2
|askList |array   |���� |3
|bidList |array   |����  |4
|version |long   |�汾��  |5
|p |double   |����  |1
|v |double   |����  |2


** ���1004** //��Ȳ���

```
//�г���Ȳ�����Ϣ������ȽϷ�����1.�³��ֵļ۸񣬱�ʾ������ȡ�2.�ѳ��ֵļ۸񣬹ҵ�����Ϊ0����ʾ����.3.�ѳ��ֵļ۸񣬹ҵ���Ϊ0����ʾɾ������ȡ�=====��StartVersion���г���ȿ��յİ汾��ͬ����Ҫ���»�ȡ�г���ȿ���
message MarketDepthDiff{
    //�г�
    required string Market= 1 ;
    //����
    required int32 Precision= 2 ;
    //���������б�
    repeated MarketDepth AskList= 3 ;
    //�򵥲����б�
    repeated MarketDepth BidList= 4 ;
    //��ʼ�汾��
    required int64 StartVersion= 5 ;
    //��ֹ�汾��
    required int64 EndVersion= 6 ;
}
message MarketDepth{
    //����
    required double P= 1 ;
    //����
    required double V= 2 ;
}
```


|������|����|˵��|protobuf����|
|:-----  |:-----|-----|----|
|market |string   |�г����׶�  |1
|precision |string   |�г�����  |2|
|askList |array   |���� |3|
|bidList |array   |����  |4|
|startVersion |double   |��ʼ�汾��  |5|
|endVersion |double   |��ǰ�汾��  |6|
|p |double   |�۸�  |1|
|v |double   |����  |2|

----

###����API

˽��apiÿ��������Ҫ����һ��ǩ������sign��һ��Authorization�Ĳ���

###ǩ��

��ȡapi�������˺�����->Api����->����api���������֮�������һ��Apikey��Secrect Key,�����ƹ��� ** ��Ҫ��ʾ����������Կ���˺Ű�ȫ������أ����ۺ�ʱ��������������͸¶��**

** Apikey������ʶ�û�����ws���Ӻ����Authorization=Apikey���� **


** Secrect Key��������sign���� **

** sign ���ɷ���: ** ʹ��Secrect Key��Ϊ��Կ��Authorization=ApikeyΪֵ���� HmacSHA256 ����ó�

��

APIKey=81.67AAA2F6041D408D9868387A8904431D��Authorization��������

```
Authorization=81.67AAA2F6041D408D9868387A8904431D
```

��Secrect KeyΪ 2288987EFDB54F848D7BACCE1288FC9A�������ó�signֵΪ
```
57c4c6770d565aa236f87706053bd51512862443062e471bd3243a6ed8eef2
```
�������ɵ�ws��ַ��
wss://api.azex.io?Authorization=81.67AAA2F6041D408D9868387A8904431D&sign=57c4c6770d565aa236f87706053bd51512862443062e471bd3243a6ed8eef2


----
���ĸ��˶���������������Ϣ������������Ϣ���ƻ���������Ϣ��

** ������Ϣ **

** ·�ɣ�1**
** ���1000 **
���������

```
//��¼�г�
message LoginToMarket{
    //�г�
    required string Market= 1 ;
}
```
����˵��

|������|����|˵��|����|protobuf����|
|:-----  |:-----|----- |-------|----|
|market |string   |�г����׶�  |�� |1



** ������Ϣ **

** ���1008** //����������Ϣ
```
//������Ϣ
message OrderInfoDto{
    //����Id
    required string Id= 1 ;
    //�û�Id
    required int64 UserId= 2 ;
    //����
    required string Currency= 3 ;
    //�����ѱ���
    required string FeeCurrency= 4 ;
    //�г�
    required string Market= 5 ;
    //��������
    required int32 Category= 6 ;
    //�������
    required int32 OrderType= 7 ;
    //�ƻ������
    required int32 PlanType= 8 ;
    //�����۸�
    required double TriggerPrice= 9 ;
    //�޼۵�����
    required double Price= 10 ;
    //�޼۵�����
    required double Volume= 11 ;
    //�м۵����
    required double Amount= 12 ;
    //����״̬
    required int32 Status= 13 ;
    //����ʱ��
    required int64 CreateTime= 14 ;
}

```
����˵��

|������|����|˵��|protobuf����|
|:-----  |:-----|-----|----|
|id |string   |����id  |1
|userId |long   |�û�id  |2|
|currency |string   |���� |3|
|feeCurrency |string   |�����ѱ���  |4|
|market |string   |�г�  |5|
|category |int   |�������ࣨ1�Ǽƻ�������2�ƻ�������  |6|
|orderType |int   |�������ͣ�1�޼���2�м���3�޼�����4�м�������  |7|
|planType |int   |�ƻ������1�߼۴���2�ͼ۴�����  |8|
|triggerPrice |double   |������  |9|
|price |double   |�ҵ���  |10|
|volume |double   |�ҵ�����  |11|
|amount |double   |�ҵ����  |12|
|status |int   |����״̬  |13|
|createTime |long   |����ʱ��  |14|

** ���1009** //���¶�����Ϣ
```
//���¶�����Ϣ
message UpdateOrderInfo{
    //��߼�
    required string Market= 1 ;
    //����Id
    required string OrderId= 2 ;
    //������
    required double TxVolume= 3 ;
    //���׶�
    required double TxAmount= 4 ;
    //����״̬
    required int32 Status= 5 ;
    //����ʱ��
    required int64 UpdateTime= 6 ;
}

```
����˵��

|������|����|˵��|protobuf����|
|:-----  |:-----|-----|----|
|market |string   |�г����׶�  |1
|orderId |string   |����id  |2|
|txVolume |double   |������ |3|
|txAmount |double   |���׽��  |4|
|status |int   |����״̬��2������3���������4��ȡ����  |5|
|updateTime |long   |����ʱ��  |6|

** ���1010** //�ƻ���������Ϣ
```
message PlanOrderTrigger{
    //�г�
    required string Market= 1 ;
    //�ƻ�����ID
    required string Id= 2 ;
    //�����۸�
    required double Price= 3 ;
    //�û�Id
    required int64 UserId= 4 ;
}

```
����˵��

|������|����|˵��|protobuf����|
|:-----  |:-----|-----|----|
|market |string   |�г����׶�  |1
|id |string   |�ƻ���id |2|
|price |double   |�����۸� |3|
|userId |double   |�û�id  |4|

----

������Ϣ��Ϣ

** ������Ϣ **
** ���0** 
```
message WsError{
    //������
    required int32 Code= 1 ;
    //������ʾ
    required string Message= 2 ;
}
```


