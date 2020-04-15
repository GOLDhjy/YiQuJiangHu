using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Game
{
    public class GameFinish : IGameBaseState
    {
        public GameStateEnum Type { get => GameStateEnum.Finish; }
        List<IGameSystem> systems;

        public void EndState()
        {
            Debug.Log("退出结算大厅");
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
            Debug.Log("完成游戏");
            systems = GameSystems.Instance.Get(new IGameFinishSystem());
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
