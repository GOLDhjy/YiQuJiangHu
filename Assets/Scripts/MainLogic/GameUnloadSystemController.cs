using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules;
namespace Game
{
    public class GameUnloadSystemController : Singleton<GameUnloadSystemController>
    {
        List<IUnloadSystemBase> systems = new List<IUnloadSystemBase>();
        public void Awake()
        {

            //在这里加入需要持久化的系统
            systems.AddRange( UnloadSystem.Instance.Get(new NetWorkUnloadSystem()));



            if (systems != null)
            {
                foreach (var item in systems)
                {
                    item.Awake();
                }
            }
        }
        public void Start()
        {
            if (systems != null)
            {
                foreach (var item in systems)
                {
                    item.Start();
                }
            }
        }

        public void Update()
        {
            if (systems != null)
            {
                foreach (var item in systems)
                {
                    item.Update();
                }
            }
        }
        public void LateUpdate()
        {
            if (systems != null)
            {
                foreach (var item in systems)
                {
                    item.LateUpdate();
                }
            }
        }
        public void FixedUpdate()
        {
            if (systems != null)
            {
                foreach (var item in systems)
                {
                    item.FixedUpdate();
                }
            }
        }
        public void OnDestroy()
        {
            if (systems != null)
            {
                foreach (var item in systems)
                {
                    item.OnDestroy();
                }
            }
        }
    }
}
