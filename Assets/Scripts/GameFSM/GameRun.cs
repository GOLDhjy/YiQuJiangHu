using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class GameRun : IGameBaseState
    {
        public GameStateEnum Type { get => GameStateEnum.Game; }
        List<IGameSystem> systems;

        public void EntryState()
        {
            Debug.Log("开始游戏");
            systems = GameSystems.Instance.Get(new IGameRunSystem());
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
        public void EndState()
        {
            if (systems != null)
            {
                foreach (var item in systems)
                {
                    item.Destroy();
                }
            }
        }
    }
}
