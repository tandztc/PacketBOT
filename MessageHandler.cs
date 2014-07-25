/********************************************************************************
** auth： Tan Yong
** date： 7/25/2014 4:16:01 PM
** desc： 消息处理器
** Ver.:  V1.0.0
** Feedback: mailto:tanyong@cyou-inc.com
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ProtoBuf;

namespace PacketBOT
{
    public class MessageHandler
    {
        public static void onRecMsg(byte[] msgBuff)
        {
            MemoryStream kStream = new MemoryStream(msgBuff);
            ChatMsg msg = Serializer.Deserialize<ChatMsg>(kStream);
            kStream.Dispose();
            System.Console.WriteLine("server thread No.: " + msg.sender);
            System.Console.WriteLine("server received amount: " + msg.msg);
            //System.Console.ReadLine();
        }
    }
}
