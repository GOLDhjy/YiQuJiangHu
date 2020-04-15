using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Game
{
    public class GameLogin : IGameBaseState
    {
        public GameStateEnum Type { get => GameStateEnum.Login; }
        List<IGameSystem> systems;
        public void EndState()
        {
            Debug.Log("退出登录界面");
            if (systems != null)
            {
                foreach (var item in systems)
                {
                    item.Destroy();
                }
            }
        }

        public void EntryState()
        {
            Debug.Log("欢迎来到一曲江湖");
            systems = GameSystems.Instance.Get(new IGameLobbySystem());
            if (systems != null)
            {
                foreach (var item in systems)
                {
                    item.Start();
                }
            }
        }

        public void OnState()
        {
            if (systems != null)
            {
                foreach (var item in systems)
                {
                    item.Update();
                }
            }
        }
    }
}
