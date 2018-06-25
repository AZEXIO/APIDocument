using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Websocket
{
    class Program
    {
        static AzexWebsocket azWebsocket = new AzexWebsocket();
        static ConcurrentDictionary<string, DepthDto> Depth = new ConcurrentDictionary<string, DepthDto>();
        static void Main(string[] args)
        {
            azWebsocket.SubMarketDepth("eth_btc");
            //azWebsocket.SubMarketOrder("eos_btc");
            azWebsocket.OnMessage += OnAzMessageReciver;
            var sign = EncryptHMACSHA256("api_secret", "api_key");
            azWebsocket.Init();
            
            Console.ReadKey();
        }


        public static void OnAzMessageReciver(object sender, MessageReceivedEventArgs args)
        {
            //获取快照数据
            if (args.Command == CommandType.MarketDepthData)
            {
                var model = ProtobufHelper.Deserialize<MarketDepthDto>(args.Message);
                DepthDto depthResult = ConvertDepth(model);
                Depth.AddOrUpdate(model.Market, depthResult, (key,v)=> { return depthResult; });
            }
            else if (args.Command == CommandType.MarketDepthDiff)
            {
                var diffModel = ProtobufHelper.Deserialize<MarketDepthDiff>(args.Message);
                //深度无快照，或者快照数据的版本与差异数据起始版本不一致时，重新订阅获取快照
                if (!Depth.ContainsKey(diffModel.Market) || Depth[diffModel.Market].Version != diffModel.StartVersion)
                    azWebsocket.SendMarketDepth("eth_btc");
                else
                    UpdateDepth(diffModel);
            }

            Console.WriteLine("数据更新");
        }

        public static void UpdateDepth(MarketDepthDiff depthDiff)
        {
            var depth = Depth[depthDiff.Market];
            foreach (var item in depthDiff.AskList)
            {
                var key = Convert.ToDecimal(item.Price);
                if (item.Volume == 0)
                {
                    if (depth.Asks.ContainsKey(key))
                        depth.Asks.Remove(key);
                }
                else//增，改
                {
                    if (!depth.Asks.ContainsKey(key))
                    {
                        depth.Asks.Add(key, Convert.ToDecimal(item.Volume));
                    }
                    else
                    {
                        depth.Asks[key] = Convert.ToDecimal(item.Volume);
                    }
                }
            }

            foreach (var item in depthDiff.BidList)
            {
                var key = Convert.ToDecimal(item.Price);
                if (item.Volume == 0)
                {
                    if (depth.Bids.ContainsKey(key))
                        depth.Bids.Remove(key);
                }
                else//增，改
                {
                    if (!depth.Bids.ContainsKey(key))
                    {
                        depth.Bids.Add(key, Convert.ToDecimal(item.Volume));
                    }
                    else
                    {
                        depth.Bids[key] = Convert.ToDecimal(item.Volume);
                    }
                }
            }
            depth.Version = depthDiff.EndVersion;
        }

        public static DepthDto ConvertDepth(MarketDepthDto source)
        {
            var resultDto = new DepthDto();
            resultDto.Version = source.Version;
            foreach (var item in source.AskList)
            {
                var key = Convert.ToDecimal(item.Price);
                if (!resultDto.Asks.ContainsKey(key))
                    resultDto.Asks.Add(key, Convert.ToDecimal(item.Volume));
                else
                    resultDto.Asks[key] = Convert.ToDecimal(item.Volume);
            }
            foreach (var item in source.BidList)
            {
                var key = Convert.ToDecimal(item.Price);
                if (!resultDto.Bids.ContainsKey(key))
                    resultDto.Bids.Add(key, Convert.ToDecimal(item.Volume));
                else
                    resultDto.Bids[key] = Convert.ToDecimal(item.Volume);
            }

            return resultDto;
        }

        public static string EncryptHMACSHA256(string key, string value)
        {
            using (var sha = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(value));
                string result = string.Empty;
                foreach (var item in bytes)
                {
                    result += item.ToString("x");
                }
                return result;
            }
        }
    }
    public class DepthDto
    {
        public DepthDto()
        {
            Bids = new SortedDictionary<decimal, decimal>();
            Asks = new SortedDictionary<decimal, decimal>();
        }
        public long Version { get; set; }
        public SortedDictionary<decimal, decimal> Bids { get; set; }
        public SortedDictionary<decimal, decimal> Asks { get; set; }

    }

    [ProtoBuf.ProtoContract]
    public class MarketDepthDto
    {
        public MarketDepthDto()
        {
            AskList = new List<DepthItem>();
            BidList = new List<DepthItem>();
        }
        //市场
        [ProtoBuf.ProtoMember(1)]
        public string Market { get; set; }
        //百分比
        [ProtoBuf.ProtoMember(2)]
        public int Precision { get; set; }
        //卖单列表
        [ProtoBuf.ProtoMember(3)]
        public List<DepthItem> AskList { get; set; }
        //买单列表
        [ProtoBuf.ProtoMember(4)]
        public List<DepthItem> BidList { get; set; }
        [ProtoBuf.ProtoMember(5)]
        public long Version { get; set; }
    }
    [ProtoBuf.ProtoContract]
    public class MarketDepthDiff
    {
        public MarketDepthDiff()
        {
            AskList = new List<DepthItem>();
            BidList = new List<DepthItem>();
        }
        [ProtoBuf.ProtoMember(1)]
        public string Market { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public int Precision { get; set; }
        [ProtoBuf.ProtoMember(3)]
        public List<DepthItem> AskList { get; set; }
        [ProtoBuf.ProtoMember(4)]
        public List<DepthItem> BidList { get; set; }
        [ProtoBuf.ProtoMember(5)]
        public long StartVersion { get; set; }
        [ProtoBuf.ProtoMember(6)]
        public long EndVersion { get; set; }
    }
    [ProtoBuf.ProtoContract]
    public class DepthItem
    {
        //单价
        [ProtoBuf.ProtoMember(1)]
        public double Price { get; set; }
        //数量
        [ProtoBuf.ProtoMember(2)]
        public double Volume { get; set; }
        //订单数量
    }
}
