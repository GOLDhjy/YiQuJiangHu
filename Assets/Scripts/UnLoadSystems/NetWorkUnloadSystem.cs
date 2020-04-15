using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NetWork;
namespace Game
{
    public class NetWorkUnloadSystem : IUnloadSystemBase
    {
        public void Awake()
        {
            NetWorkModule.Instance.NetMoudleStart();
            Debug.Log("开始联网了");

            //test
            NetWorkModule.Instance.Client_regist_req("wfw", "xa87321578");

            //close
           // NetWorkModule.Instance.DisConnect();
        }

        public void FixedUpdate()
        {
            if (MagCache.Instance.Messages.Count>0)
            {
                var msg = MagCache.Instance.Messages.Dequeue();
                msg.Invoke();            
            }     
        }

        public void LateUpdate()
        {

        }

        public void OnDestroy()
        {
            
        }

        public void Start()
        {

        }

        public void Update()
        {

        }
    }
}
