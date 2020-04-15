using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modules;
using System;
using System.Reflection;
using System.Linq;
namespace Game
{
    public class GameSystems : Singleton<GameSystems>
    {
        Dictionary<Type, List<IGameSystem>> gameSystems = new Dictionary<Type, List<IGameSystem>>();

        //在这里初始化添加所有系统
        public GameSystems()
        {
            Add(new IGameLobbySystem());
            Add(new IGameRunSystem());
            Add(new IGameFinishSystem());
            Add(new IGameLoginSystem());

            InitSubSystem();
        }
        void InitSubSystem()
        {
            foreach (var system in gameSystems)
            {
                var SubTypeQuery = from t in Assembly.GetExecutingAssembly().GetTypes()
                                   where IsSubClassOf(t, system.Key)
                                   select t;
                foreach (var item in SubTypeQuery)
                {   
                    Add(system.Key, item); 
                }
            }
        }
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

        
        public void Add(IGameSystem system)
        {
            gameSystems.Add(system.GetType(), new List<IGameSystem>());
        }
        public void Add(Type Fsystem,Type subsystem)
        {

            gameSystems[Fsystem].Add(Activator.CreateInstance(subsystem) as IGameSystem);
        }
        public List<IGameSystem> Get(IGameSystem system)
        {

            if (system != null && gameSystems.ContainsKey(system.GetType()))
            {
                List<IGameSystem> systems;
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
