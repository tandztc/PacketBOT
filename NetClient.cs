using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

/// <summary>
/// 封装Socket
/// </summary>
public class NetClient
{
    public static NetClient instance = new NetClient();
    public TcpClient client;
    private bool isHead;
    private int len;

    public bool isConnected()
    {
        return client.Client.Connected;
    }
    /// <summary>
    /// 初始化网络连接
    /// </summary>
    public void Init()
    {
        client = new TcpClient();
        client.Connect("127.0.0.1", 12580);
        isHead = true;
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    public void SendMsg(byte[] msg)
    {
        //消息体结构：消息体长度（int） + 消息类型（short） + 消息体
        byte[] data = new byte[6 + msg.Length];
        BitConverter.GetBytes(msg.Length+6).CopyTo(data, 0);
        BitConverter.GetBytes(0).CopyTo(data, 4);
        msg.CopyTo(data, 6);
        client.GetStream().Write(data, 0, data.Length);
        client.GetStream().Flush();
        Console.WriteLine("msg send!");
    }

    /// <summary>
    /// 接收消息
    /// </summary>
    public void ReceiveMsg()
    {
        NetworkStream stream = client.GetStream();
        if (!stream.CanRead)
        {
            return;
        }
        //读取消息体的长度
        if (isHead)
        {
            if (client.Available < 4)
            {
                //return;
            }
            byte[] lenByte = new byte[4];
            stream.Read(lenByte, 0, 4);
            len = BitConverter.ToInt32(lenByte, 0);
            isHead = false;
        }
        //读取消息体内容
        if (!isHead)
        {
            if (client.Available < len - 4)
            {
                return;
            }
            byte[] typeByte = new byte[2];
            stream.Read(typeByte, 0, 2);
            short eType = BitConverter.ToInt16(typeByte, 0);
            byte[] msgByte = new byte[len-6];
            stream.Read(msgByte, 0, len-6);
            isHead = true;
            len = 0;
            if (onRecMsg != null)
            {
                //处理消息
                onRecMsg(msgByte);
            }
        }
        Console.WriteLine("msg receive!");

    }

    /// <summary>
    /// bytes转int
    /// </summary>
    /// <param name="data"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public static int BytesToInt(byte[] data, int offset)
    {
        int num = 0;
        for (int i = offset; i < offset + 4; i++)
        {
            num <<= 8;
            num |= (data[i] & 0xff);
        }
        return num;
    }

    /// <summary>
    /// int 转 bytes
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static byte[] IntToBytes(int num)
    {
        byte[] bytes = new byte[4];
        for (int i = 0; i < 4; i++)
        {
            bytes[i] = (byte)(num >> (24 - i * 8));
        }
        return bytes;
    }

    public delegate void OnRevMsg(byte[] msg);

    public OnRevMsg onRecMsg = PacketBOT.MessageHandler.onRecMsg;


}

