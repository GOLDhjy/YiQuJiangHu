using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using YiQuJiangHuNetData;
using Google.Protobuf;
using UnityEngine;

namespace NetWork
{
    public class NetWorkModule : Singleton<NetWorkModule>
    {
        private static byte[] ReceiveBuff = new byte[1024];//接收缓存区
        private static byte[] SendBuff = new byte[1024];//发生缓存区

        static private bool pingponging = false;

        /// <summary>
        /// 网络模块启动接口，整体网络库对外的启动
        /// </summary>
        /// <returns>bool</returns>
        public bool NetMoudleStart()
        {
            try
            {
                //目前没有设置超时断连等操作
                Debug.Log("Start Connect");
                if(this.Connect())
                {
                    NetBase.Instance.StartReceiveMsg();
                }
            }
            catch (Exception e)
            {
                Debug.Log("Start Connect is failed!");
            }

            return true;
        }

        public bool Connect()
        {
            return NetBase.Instance.Connect(NetConfig.IpAddress, NetConfig.Port);
        }
        public void DisConnect()
        {
            NetBase.Instance.DisConnect();
        }

        //****************************协议 start***************************************//

        public void Client_ping_req(string msg)
        {
            Client_ping_req Client_ping_req_Test = new Client_ping_req();
      
            this.SendMsg("YiQuJiangHuNetData.Client_ping_req", Client_ping_req_Test.ToByteString());
        }

        public void Client_login_req(string Name, string Password)
        {
            Client_login_req cData = new Client_login_req();
            cData.Name = Name;
            cData.Password = Password;

            this.SendMsg("YiQuJiangHuNetData.Client_login_req", cData.ToByteString());
        }

        public void Client_regist_req(string Name, string Password)
        {
            Client_login_req cData = new Client_login_req();
            cData.Name = Name;
            cData.Password = Password;

            this.SendMsg("YiQuJiangHuNetData.Client_regist_req", cData.ToByteString());
        }

        //****************************协议 end***************************************//

        //根据消息类型发送
        public void SendMsg(string pbname, Google.Protobuf.ByteString data)
        {
            //封装协议头
            MsgHead head = new MsgHead();
            head.Proto = pbname;
            head.Data = data;
            byte[] senddata = head.ToByteArray();
            int len = senddata.Length;
            if (len > 0xffffff)
            {
                Debug.LogError("error");
            }

            byte[] headTCP = System.BitConverter.GetBytes(len);
            headTCP[3] = 0x00;

            byte[] msgTCP = new byte[4 + len];

            Buffer.BlockCopy(headTCP, 0, msgTCP, 0, 4);
            Buffer.BlockCopy(senddata, 0, msgTCP, 4, len);

            NetBase.Instance.SendMsg(msgTCP);

        }

        //收到服务器消息后分发消息到处理中心处理。
        public static void ProcessReceive()
        {
            NetBase.Instance.isProcessingReceive = true;

            byte[] RecveiveHead = NetBase.DecodePacket(ref NetBase.Instance.receiveCache);

            if (RecveiveHead == null)
            {
                NetBase.Instance.isProcessingReceive = false;
                return;
            }

            //解析头部
            byte[] headTCP = RecveiveHead.Skip(0).Take(4).ToArray();
            int msglen = (headTCP[0] & 0xff) | ((headTCP[1] & 0xff) << 8) | ((headTCP[2] & 0xff) << 16);
            int ctl = (headTCP[3] >> 6) & 0x03;

            switch (ctl)
            {
                case 0x00:
                    { //消息
                        IMessage IMperson = new MsgHead();   //解析协议头
                        MsgHead msg = new MsgHead();
                        msg = (MsgHead)IMperson.Descriptor.Parser.ParseFrom(RecveiveHead.Skip(4).Take(msglen).ToArray());//获取协议名


                        string pbname = msg.Proto.Substring(msg.Proto.IndexOf('.') + 1); //执行特定的协议

                        var handler = MsgHandler.Instance.GetCallback(pbname);

                        Debug.Log(pbname);

                      
                        MagCache.Instance.EnQueueMessage(() => { handler.Invoke( msg.Data); });

                        break;
                    }
                case 0x01:
                    {//ping
                        Debug.Log("Received ping");
                        // Ping();
                        break;
                    }
                case 0x02:
                    {//pong
                        //Debug.Log("Received pong");
                        pingponging = false;
                        break;
                    }
                case 0x03:
                    {//close
                        NetBase.Instance.DisConnect();
                        Debug.Log("Received close");
                        break;
                    }
                default:
                    {//error
                        Debug.LogError("error");
                        break;
                    }
            }
            ProcessReceive();
        }

    }
}
