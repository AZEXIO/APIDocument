
using SuperSocket.ClientEngine;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocket4Net;

namespace Websocket
{
    public class AzexWebsocket
    {
        #region 私有属性
        private WebSocket websocket;
        private Dictionary<string, byte[]> topicDic = new Dictionary<string, byte[]>();
        private const string WEBSOCKET_API = "wss://ws.azex.io";
        #endregion

        /// <summary>
        /// 接收WebScoket消息事件
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs> OnMessage;

        /// <summary>
        /// 初始化WebSocket
        /// </summary>
        /// <returns></returns>
        public bool Init(string apiKey = "",string sign="")
        {
            try
            {
                var uri = WEBSOCKET_API;
                if (!string.IsNullOrEmpty(apiKey))
                    uri += "?Authorization=" + System.Web.HttpUtility.UrlEncode(apiKey) + "&sign="+sign;
                websocket = new WebSocket(uri);
                websocket.Error += (sender, e) =>
                {
                    Console.WriteLine("Error:" + e.Exception.Message.ToString());
                };
                websocket.DataReceived += ReceivedMsg;
                websocket.Opened += OnOpened;
                websocket.Open();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:" + ex.Message);
            }
            return true;
        }


        public void Close()
        {
            this.websocket.Close();
        }


        public void SubUserOrderInfo(string[] markets)
        {
            foreach (var market in markets)
            {
                var topicObj = new LoginToMarket()
                {
                    Market = market
                };
                topicDic.Add(market + "loginM", ProtobufHelper.Serializer(topicObj, 1000));
            }
        }


        public void SubMarketOrder(string market)
        {

            var topicObj = new SubMarketOrder()
            {
                Market = market,
                Count = 10,
                Subscribe = true
            };
            topicDic.Add("Order",ProtobufHelper.Serializer(topicObj, 907));
            
        }

        public void SubMarketDepth(string market)
        {
            
            var topicObj = new SubMarketDepth()
            {
                Market = market,
                Limit = 10,
                Precision = 5
            };
            topicDic.Add("Depth", ProtobufHelper.Serializer(topicObj, 906));
            
        }

        public void SendMarketDepth(string market)
        {
            var topicObj = new SubMarketDepth()
            {
                Market = market,
                Limit = 10,
                Precision = 5
            };
            SendSubscribeTopic(ProtobufHelper.Serializer(topicObj, 906));
        }

        /// <summary>
        /// 连通WebSocket，发送订阅消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnOpened(object sender, EventArgs e)
        {
            foreach (var item in topicDic)
            {
                SendSubscribeTopic(item.Value);
            }

        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void ReceivedMsg(object sender, WebSocket4Net.DataReceivedEventArgs args)
        {
            var msg = args.Data;
            var codeArray = new byte[2] { msg[0], msg[1] };
            var code = (int)ProtobufHelper.GetCommand(codeArray);

            OnMessage?.Invoke(null, new MessageReceivedEventArgs(msg, (CommandType)Enum.Parse(typeof(CommandType), code.ToString())));
        }



        #region 订阅相关
        private void SendSubscribeTopic(byte[] byts)
        {
            websocket.Send(byts, 0, byts.Length);
        }
        #endregion
    }

    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(byte[] message, CommandType command)
        {
            this.Command = command;
            this.Message = message;
        }
        public CommandType Command { get; set; }
        public byte[] Message { get; set; }
    }

    public enum CommandType
    {
        SetMarketDepth = 906,
        SetReceiveTradeOrder = 907,
        /// <summary>
        /// 深度快照数据
        /// </summary>
        MarketDepthData = 1003,
        /// <summary>
        /// 深度差异数据
        /// </summary>
        MarketDepthDiff = 1004,
        UpdateOrder = 1009
    }

    [ProtoBuf.ProtoContract]
    public class SubMarketDepth
    {
        /// <summary>
        /// 市场
        /// </summary>
        [ProtoBuf.ProtoMember(1)]
        public string Market { get; set; }

        /// <summary>
        /// 小数位精度
        /// </summary>
        [ProtoBuf.ProtoMember(2)]
        public int Precision { get; set; }
        /// <summary>
        /// 获取条数
        /// </summary>
        [ProtoBuf.ProtoMember(3)]
        public int Limit { get; set; }
        /// <summary>
        /// 是否是深度图
        /// </summary>
        //[ProtoBuf.ProtoMember(4)]
        //public bool IsChart { get; set; }
    }
    [ProtoBuf.ProtoContract]
    public class SubMarketOrder
    {
        /// <summary>
        /// 市场
        /// </summary>
        [ProtoBuf.ProtoMember(1)]
        public string Market { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [ProtoBuf.ProtoMember(2)]
        public int Count { get; set; }
        /// <summary>
        /// 是否订阅
        /// </summary>
        [ProtoBuf.ProtoMember(3)]
        public bool Subscribe { get; set; }
    }
    [ProtoBuf.ProtoContract]
    public class GetTopTradeList
    {
        [ProtoBuf.ProtoMember(1)]
        public string Market { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public int Count { get; set; }
        [ProtoBuf.ProtoMember(3)]
        public bool Subscribe { get; set; }
    }
    [ProtoBuf.ProtoContract(ImplicitFields = ProtoBuf.ImplicitFields.AllFields)]
    public class LoginToMarket
    {
        public string Market { get; set; }
    }
}
