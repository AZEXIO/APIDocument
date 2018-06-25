
var ws = (function (protoJsonUrl) {
    var exports = {};
    var sendCommands = {};
    var receiveCommands = {};

    var wsConnector = {};
    function StartWS(root, callbacks, url,onopencallbacks) {

        console.log(url);
        sendCommands = {
            SetKLineFequency: { code: 900, model: root.lookup("GetKLineList") },//批量获取k线数据
            SetReceiveOtherMarketKLine: { code: 902, model: root.lookup("SubKLine") },//设置接受其它市场的K线数据
            SetMarketDepth: { code: 906, model: root.lookup("SubMarketDepth") },//订阅市场深度
            SetReceiveTradeOrder: { code: 907, model: root.lookup("GetTopTradeList") },//设置接受交易订单数据
            Login: { code: 1000, model: root.lookup("LoginToMarket") },//用于接收个人订单变更数据
        };
        receiveCommands = {
            Error: { code: 0, model: root.lookup("WsError") },//错误信息
            SingleKLine: { code: 1000, model: root.lookup("WsKLine") },//单条K线数据
            BatchKLine: { code: 1001, model: root.lookup("WsKLineList") },//多条K线数据
            BatchKLineSendComplate: { code: 1002, model: root.lookup("BatchSendComplate") },//批量K线发送完成，可以渲染
            MarketDepthData: { code: 1003, model: root.lookup("MarketDepthDto") },//市场深度数据
            MarketDepthDiff: { code: 1004, model: root.lookup("MarketDepthDiff") },//市场深度差异数据
            TradeSimpleDtoList: { code: 1005, model: root.lookup("TradeSimpleDtoList") },//成交订单列表
            ScrollDayKLine: { code: 1006, model: root.lookup("ScrollDayKLine") },//滑动24小时K线数据
            CreateOrder: { code: 1008, model: root.lookup("OrderInfoDto") },//新增订单信息
            UpdateOrder: { code: 1009, model: root.lookup("UpdateOrderInfo") },//更新订单信息
            PlanOrderTrigger: { code: 1010, model: root.lookup("PlanOrderTrigger") },//计划订单触发
        };

        wsConnector = new WebSocket(url);
        wsConnector.onopen = function (e) {
            console.log("Connection open...");
            onopencallbacks();
        };
        wsConnector.binaryType = "arraybuffer";
        wsConnector.onmessage = function (e) {
            function ByteToUnShort(b) {
                return (b[0] & 0xff) | ((b[1] & 0xff) << 8);
            }
            if (e.data instanceof ArrayBuffer) {
                var cmdArray = new Uint8Array(e.data, 0, 2);
                var receiveBuffer = new Uint8Array(e.data, 2);
                var cmd = ByteToUnShort(cmdArray);
                for (const key in receiveCommands) {
                    if (receiveCommands.hasOwnProperty(key) && cmd == receiveCommands[key].code) {
                        const element = receiveCommands[key];
                        var data = element.model.decode(receiveBuffer);
                        callbacks[key](data, key);
                        break;
                    }
                }
            } else {
                console.log('string:', e.data);
            }
        };
        wsConnector.onerror = function (e) {
            console.log('ws', 'websocked error');
        }
        wsConnector.onclose = function (e) {
            console.log('ws', "Connection closed", e);
            setTimeout(function () { StartWS(root, url); }, 2000);
        };
        // wsConnector.onopen = function () {
        //     console.log('ws', 'opened');
        // }

    };
    exports.start = function (callbacks, wsUrl, urlencodedToken,onopencallbacks) {
        protobuf.load(protoJsonUrl, function (err, root) {
            if (err) {
                console.log('protobuf', 'error', err);
            }
            if (urlencodedToken) {
                wsUrl = wsUrl + "?Authorization=" + urlencodedToken;
            }

            StartWS(root, callbacks, wsUrl,onopencallbacks);
        });
    }
    exports.send = function (commandName, command, success, error) {
        function GenerateCmdBuffer(controller, command, dataBuffer) {
            var controllerLittleEndian = new dcodeIO.ByteBuffer(4).writeUint32(controller, 0).flip();
            var controllerBigEndian = new Uint8Array(4);
            controllerBigEndian[0] = controllerLittleEndian.view[3];
            controllerBigEndian[1] = controllerLittleEndian.view[2];
            controllerBigEndian[2] = controllerLittleEndian.view[1];
            controllerBigEndian[3] = controllerLittleEndian.view[0];
            var commandLittleEndian = new dcodeIO.ByteBuffer(2).writeUint16(command, 0).flip();
            var commandBigEndian = new Uint8Array(2);
            commandBigEndian[0] = commandLittleEndian.view[1];
            commandBigEndian[1] = commandLittleEndian.view[0];
            var allBuffer = dcodeIO.ByteBuffer.concat([controllerBigEndian, commandBigEndian, dataBuffer], "binary");
            return allBuffer.view;
        }

        var cmd = sendCommands[commandName];
        var data = cmd.model.create(command);
        var dataBuffer = cmd.model.encode(data).finish();
        var buffer = GenerateCmdBuffer(1, cmd.code, dataBuffer);
        if (wsConnector.readyState == WebSocket.OPEN) {
            console.log('ws', 'send', commandName, data, buffer);
            wsConnector.send(buffer);
        }
    }

    return exports;
})("Protobuffer/proto_market.json")
