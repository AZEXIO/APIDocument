using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Websocket
{
    public class ProtobufHelper
    {
        public static Int16 GetCommand(byte[] bts)
        {
            return BitConverter.ToInt16(bts, 0);
        }


        public static byte[] Serializer<T>(T mesage, Int16 code)
        {
            byte[] result = null;
            using (MemoryStream stream = new MemoryStream())
            {
                byte[] controler = BitConverter.GetBytes(1);
                stream.Write(controler, 0, controler.Length);
                byte[] bts = BitConverter.GetBytes(code);
                stream.Write(bts, 0, bts.Length);
                ProtoBuf.Serializer.Serialize<T>(stream, mesage);
                result = stream.ToArray();
            }
            return result;
        }

        public static T Deserialize<T>(byte[] bts)
        {
            var messageBts = new byte[bts.Length - 2];
            Buffer.BlockCopy(bts, 2, messageBts, 0, messageBts.Length);
            using (MemoryStream stream = new MemoryStream(messageBts))
            {
                return ProtoBuf.Serializer.Deserialize<T>(stream);

            }
        }
    }
}
