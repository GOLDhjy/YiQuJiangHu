using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules;
using Google.Protobuf;
using YiQuJiangHuNetData;

namespace NetWork
{
    public class MagCache : Singleton<MagCache>
    {
        public Queue<Action> Messages = new Queue<Action>();

        private object lockObj = new object();                              //加锁对象

        //入队
        public void EnQueueMessage(Action msg)
        {
            lock (lockObj)
            {
                Messages.Enqueue(msg);
            }
        }

        //出队
        public Action DeQueueMessage()
        {
            lock (lockObj)
            {
                if (Messages.Count > 0)
                    return Messages.Dequeue();
                else
                    return null;
            }
        }

    }
}
