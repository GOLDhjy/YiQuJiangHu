using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameLobby : IGameBaseState
    {
        public GameStateEnum Type { get=>GameStateEnum.Lobby; }
        List<IGameSystem> systems;
        //private GameStateEnum type = GameStateEnum.Lobby;
        //public GameStateEnum Type
        //{
        //    get
        //    {
        //        if (type == GameStateEnum.Ivalid)
        //        {
        //            Debug.LogError("当前状态为空");
        //        }
        //        return type;
        //    }
        //}

        public void EndState()
        {
            Debug.Log("离开大厅");
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
            Debug.Log("进入大厅");
            systems= GameSystems.Instance.Get(new IGameLobbySystem());
            if (systems!=null)
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
            //List<IGameLobbySystem> gameLobbySystems = GameSystems.Instance.Get(new IGameLobbySystem()) as List<IGameLobbySystem>;

        }
    }
}
