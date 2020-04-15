using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEntrance : MonoBehaviour
{
    //这里是游戏初始化入口
    //public GameObject stageCamera;
    private void Awake()
    {
        if (UIManager.Instance == null)
        {
            GameObject go = Resources.Load<GameObject>("Prefabs/UIManager");
            Instantiate(go, transform.position, transform.rotation);

            
        }
    }
    void Start()
    {
        //DontDestroyOnLoad(stageCamera);
    }

    void Update()
    {
        
    }
}
