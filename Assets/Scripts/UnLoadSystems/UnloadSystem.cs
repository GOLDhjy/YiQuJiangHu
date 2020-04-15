using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules;
using System.Reflection;

namespace Game
{
    public class UnloadSystem : Singleton<UnloadSystem>
    {
        Dictionary<Type, List<IUnloadSystemBase>> gameSystems = new Dictionary<Type, List<IUnloadSystemBase>>();

        //在这里初始化添加所有系统
        public void Init()
        {
            Add(new NetWorkUnloadSystem());

            InitSubSystem();
        }
        void InitSubSystem()
        {
            foreach (var system in gameSystems)
            {
                var SubTypeQuery = from t in Assembly.GetExecutingAssembly().GetTypes()
                                   where t.Equals(system.Key)
                                   select t;
                foreach (var item in SubTypeQuery)
                {
                    Add(system.Key, item);
                }
            }
        }
        //这里是筛选非基类的子类
        bool IsSubClassOf(Type type, Type baseType)
        {
            var b = type.BaseType;
            while (b != null)
            {
                if (b.Equals(baseType))
                {
                    return true;
                }
                b = b.BaseType;
            }
            return false;
        }


        public void Add(IUnloadSystemBase system)
        {
            gameSystems.Add(system.GetType(), new List<IUnloadSystemBase>());
        }
        public void Add(Type Fsystem, Type subsystem)
        {

            gameSystems[Fsystem].Add(Activator.CreateInstance(subsystem) as IUnloadSystemBase);
        }
        public List<IUnloadSystemBase> Get(IUnloadSystemBase system)
        {

            if (system != null && gameSystems.ContainsKey(system.GetType()))
            {
                List<IUnloadSystemBase> systems;
                gameSystems.TryGetValue(system.GetType(), out systems);
                return systems;
            }
            else
            {
                return null;
            }

        }
    }
}
