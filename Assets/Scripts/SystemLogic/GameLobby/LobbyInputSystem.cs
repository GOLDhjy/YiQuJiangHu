using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [GameSystem(typeof(IGameLobbySystem),typeof(LobbyInputSystem))]
    public class LobbyInputSystem : IGameLobbySystem
    {
        public override void Start()
        {
            
        }

        public override void Update()
        {
            //if (Input.GetMouseButton(0))
            //{
            //    Debug.Log("大厅中...");
            //}
        }
        public override void Destroy()
        {

        }
    }
}
