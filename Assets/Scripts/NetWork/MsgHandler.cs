using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using YiQuJiangHuNetData;
using Modules;
using UnityEngine;

namespace NetWork
{
    public class MsgHandler : Singleton<MsgHandler>
    {
        public delegate void NetWorkCallback(ByteString bs);

        private void Login(ByteString message)
        {
            //Account account = Account.Parser.ParseFrom(message);
           
        }

        private void Server_pong_rsp(ByteString message)
        {

            Server_pong_rsp server_Login_Rsp = YiQuJiangHuNetData.Server_pong_rsp.Parser.ParseFrom(message);


            Debug.Log("收到服务器消息---Server_New_Turn---" + message);
        }

        public void Server_login_rsp(ByteString message)
        {
            Server_login_rsp server_Login_Rsp = YiQuJiangHuNetData.Server_login_rsp.Parser.ParseFrom(message);

            if (server_Login_Rsp.Msg)
            {
                MyEventSystem.Instance.Invoke(LoginEvent.id, this, new LoginEvent() { Loginfor = true });
            }



            Debug.Log("收到服务器消息---Server_New_Turn---" + server_Login_Rsp.Msg);
        }

       public void Server_regist_rsp(ByteString message)
        {
            Server_regist_rsp server_regist_Rsp = YiQuJiangHuNetData.Server_regist_rsp.Parser.ParseFrom(message);




            Debug.Log("收到服务器消息---Server_New_Turn---" + server_regist_Rsp.Msg);

        }

        public static void Ping()
        {
            MsgHead head = new MsgHead();
            byte[] msgTCP = new byte[4];
            msgTCP[3] = 0x01 << 6;
           // pingponging = true;

            NetBase.Instance.SendMsg(msgTCP);
        }

        public NetWorkCallback GetCallback(string methodName)
        {
            switch (methodName)
            {
                case "Server_pong_rsp":
                    return new NetWorkCallback(Server_pong_rsp);
                case "Server_login_rsp":
                    return new NetWorkCallback(Server_login_rsp);
                //case ProtoType.Ranklist:
                //    return new NetWorkCallback(Login);
                //case ProtoType.Battle:
                //    return new NetWorkCallback(Login);
                default:
                    return null;
            }
        }
    }
}
