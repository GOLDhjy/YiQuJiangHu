using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Game
{
    public class Main : MonoBehaviour
    {

        private void Awake()
        {
            UnloadSystem.Instance.Init();
            GameContrller.Instance.Initialize();
            GameUnloadSystemController.Instance.Awake();

            GameObject.DontDestroyOnLoad(this.gameObject);
        }
        // Start is called before the first frame update
        void Start()
        {
            GameUnloadSystemController.Instance.Start();
            GameContrller.Instance.Start();
        }

        // Update is called once per frame
        void Update()
        {
            GameUnloadSystemController.Instance.Update();
            GameContrller.Instance.Update();
        }
        private void LateUpdate()
        {
            GameUnloadSystemController.Instance.LateUpdate();
            GameContrller.Instance.LateUpdate();
        }
        private void FixedUpdate()
        {
            GameUnloadSystemController.Instance.FixedUpdate();
            GameContrller.Instance.FixUpdate();
        }
        private void OnDestroy()
        {
            GameUnloadSystemController.Instance.OnDestroy();
            GameContrller.Instance.Destroy();
        }
        private void OnGUI()
        {
            if (GameContrller.Instance.CurrentState.Type != GameStateEnum.Game)
            {
                if (GUILayout.Button("引导关卡", GUILayout.MinWidth(100), GUILayout.MinHeight(60)))
                {
                    SceneManager.LoadScene("Practice");

                }
            }
            
        }
    }

}
