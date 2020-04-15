using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using Modules;
using System.Threading;
using System.IO;

namespace NetWork
{
    public class NetBase : Singleton<NetBase>
    {
        static private Socket ClientSocket;

        /// <summary>
        /// 是否正在处理接收到的数据
        /// </summary>
        public bool isProcessingReceive = false;
        /// <summary>
        /// 数据暂存区
        /// </summary>
        public byte[] receiveBuffer = new byte[1024];
        /// <summary>
        /// 数据缓存
        /// </summary>
        public List<byte> receiveCache = new List<byte>();

        public bool Connect(string ip, int port)
        {
            ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            if (ClientSocket != null && !ClientSocket.Connected)
            {
                try
                {
                    IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(ip), port);
                    ClientSocket.Connect(ipEnd);

                    Debug.Log("链接目标服务器：" + ip + ":" + port + "成功");
                    return true;
                }
                catch (Exception e)
                {                  
                    Debug.LogError("连接失败" + e.ToString());
                    return false;
                }
                
            }
            else
            {
                Debug.LogError("套接字为空或者已经连接");
                return false;
            }
        }
        public void SendMsg(byte[] content)
        {
            if (ClientSocket != null && ClientSocket.Connected)
            {
                if (content != null)
                {
                    try
                    {
                        int n = ClientSocket.Send(content);
                        Debug.Log("发送数据成功");
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("发送失败：" + e.ToString());
                    }
                }
                else
                {
                    Debug.LogError("发送内容为空");
                    return;
                }
            }
            else
            {
                Debug.LogError("套接字为空或者未连接");
            }
        }
        public void StartReceiveMsg()
        {
            if (ClientSocket.Connected == false && ClientSocket == null)
            {
                Debug.LogError("套接字为空或者未连接");
                return;
            }
            ClientSocket.BeginReceive(receiveBuffer, 0, 1024, SocketFlags.None, ReceiveCallback, ClientSocket);

        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            int length = ClientSocket.EndReceive(ar);
            byte[] data = new byte[length];
            
            Buffer.BlockCopy(receiveBuffer, 0, data, 0, length);

            //添加包长度存到receiveCache中
            byte[] lengthbytes = BitConverter.GetBytes(length);
            byte[] resArr = new byte[lengthbytes.Length + length];
            lengthbytes.CopyTo(resArr, 0);
            data.CopyTo(resArr, lengthbytes.Length);

            receiveCache.AddRange(resArr);
            if (isProcessingReceive == false)
                NetWorkModule.ProcessReceive();

            StartReceiveMsg();
        }

        public void DisConnect()
        {
            if (ClientSocket != null && ClientSocket.Connected)
            {
                try
                {
                    ClientSocket.Shutdown(SocketShutdown.Both);
                    ClientSocket.Close();
                    Debug.Log("断开连接");
                }
                catch (Exception e)
                {
                    Debug.LogError("关闭套接字失败" + e.ToString());
                }
            }
            else
            {
                Debug.LogError("套接字不存在或者已经断开连接");
            }
        }


        /// <summary>
        /// 解析包，从缓冲区里取出一个完整的包
        /// </summary>
        /// <param name="cache"></param>
        /// <returns></returns>
        public static byte[] DecodePacket(ref List<byte> cache)
        {
            if (cache.Count < 4)
            {
                return null;
            }
            using (MemoryStream ms = new MemoryStream(cache.ToArray()))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    int length = br.ReadInt32();
                    int remainLength = (int)(ms.Length - ms.Position);
                    if (length > remainLength)
                    {
                        return null;
                    }
                    byte[] data = br.ReadBytes(length);
                    //更新数据缓存
                    cache.Clear();
                    cache.AddRange(br.ReadBytes(remainLength));
                    return data;
                }
            }
        }

    }
}
