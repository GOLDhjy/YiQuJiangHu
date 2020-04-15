using UnityEngine;
using UnityEditor;


namespace Game
{
    [GameSystem(typeof(IGameRunSystem), typeof(RunInputSystem))]
    public class RunInputSystem : IGameRunSystem
    {
        GameObject playerObj;
        LocalPlayerController player;
        GameObject playerandcamera;
        public override void Start()
        {
            Debug.Log("初始化角色");
            playerandcamera = GameObject.Find("PlayerAndCamera");
            if (playerandcamera == null)
            {
                Debug.LogError("没找到PlayerAndCamera");
            }
            UnityEngine.Object obj = Resources.Load("Prefabs/Players/Player");
            playerObj = GameObject.Instantiate<GameObject>(obj as GameObject);
            playerObj.name = "Player";
            LocalPlayer.Instance.m_PlayerObj = playerObj;
            


            LocalPlayer.Instance.InitPlayer();
            player = LocalPlayer.Instance.GetPlayController();

            playerandcamera.GetComponent<PlayerAndCamera>().SetPlayerLocalPos(player.transform.position);
        }

        public override void Update()
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.JumpUp(15);
            }
            //if (Input.GetKeyDown(KeyCode.DownArrow))
            //{
            // LocalPlayerController.JumpDown();
            //}

            if (Input.GetKeyDown(KeyCode.D))
            {
                player.AttackStart( AttackPath.UP);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                player.AttackStart( AttackPath.DOWM);
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                player.AttackFinish( AttackPath.DOWM);
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                player.AttackFinish(AttackPath.UP);
            }
        }
        public override void Destroy()
        {
            playerObj = null;
            LocalPlayer.Instance.ClearPlayer();
        }
    }
}