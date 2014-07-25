using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace PacketBOT
{
    class Program
    {
        static void Main(string[] args)
        {
            NetClient.instance.Init();
            while (NetClient.instance.isConnected())
            {
                ChatMsg msg = new ChatMsg();
                msg.sender = "sender process ID = " + Process.GetCurrentProcess().Id.ToString();
                msg.msg = DateTime.Now.ToString();
                MemoryStream dest = new MemoryStream();
                Serializer.Serialize(dest, msg);
                byte[] array = new byte[dest.Length];
                System.Array.Copy(dest.GetBuffer(), array, dest.Length);
                NetClient.instance.SendMsg(array);
                NetClient.instance.ReceiveMsg();
                Thread.Sleep(100);
            }
        }

    }
}
