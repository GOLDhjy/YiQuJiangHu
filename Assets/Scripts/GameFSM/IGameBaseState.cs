using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Game
{
    public enum GameStateEnum
    {
        Ivalid,
        Login,
        Lobby,
        Game,
        Finish
    }
    public interface IGameBaseState
    {

        GameStateEnum Type { get; }

        void EntryState();
        void OnState();
        void EndState();
    }
}
