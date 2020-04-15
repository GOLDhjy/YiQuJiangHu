using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumbackgroundLoader : MonoBehaviour
{
    [Header("中景模块长度")]
    public float len;
    [Header("随机数种子")]
    public int RandSeedNum;
    [Header("中景离玩家距离")]
    public float Distance;
    [Header("Y轴高度")]
    public float DistanceY;
    [Header("同时加载个数")]
    public int NumOnScene;
    [Header("中景模块预制体")]
    public List<GameObject> BackgroundPrefabs;
    [Header("摄像机速度")]
    public float Speed;

    private float TimeFormStart;
    private Transform GameLogic;
    private float CurrentPosition;
    public List<GameObject> GBsOnScene;
    // Start is called before the first frame update
    void Start()
    {
        GameLogic = GameObject.Find("StartRoot").transform;
        Speed = GameLogic.GetComponent<GameLogic>().Speed;
        TimeFormStart = GameLogic.GetComponent<GameLogic>().TimeFromStart;

        Init();
    }

    private void Init()
    {
        UnityEngine.Random.InitState(RandSeedNum);
        
        if (BackgroundPrefabs.Count>0)
        {
            for (int i = 0; i < NumOnScene; i++)
            {
                int index = GetPrefabsRandNum();
                GameObject gameObject = Instantiate<GameObject>(BackgroundPrefabs[index]);
                gameObject.transform.SetParent(this.gameObject.transform);
                GBsOnScene.Add(gameObject);
            }
            int mid = NumOnScene / 2;
            for (int i = 0; i < NumOnScene; i++)
            {
                
                GBsOnScene[i].transform.position = new Vector3((i - mid) * len, DistanceY, Distance);
                
            }
            CurrentPosition = GBsOnScene[NumOnScene - 1].transform.position.x + len;
        }
    }

    private int GetPrefabsRandNum()
    {
        int index = UnityEngine.Random.Range(0, 100);
        index = index % BackgroundPrefabs.Count;
        return index;
    }

    // Update is called once per frame
    void Update()
    {

        //根据距离重新创建新的中景
        CheckDistanceForCreatMid();

    }

    private void CheckDistanceForCreatMid()
    {
        if (GBsOnScene[0] != null && (GBsOnScene[0].transform.position.x - Camera.main.transform.position.x) < -(len * 1.5))
        {
            Destroy(GBsOnScene[0]);
            GBsOnScene.RemoveAt(0);
            int index = GetPrefabsRandNum();
            GameObject gameObject = Instantiate<GameObject>(BackgroundPrefabs[index]);
            gameObject.transform.SetParent(this.gameObject.transform);
            gameObject.transform.position = new Vector3(CurrentPosition, DistanceY, Distance);
            GBsOnScene.Add(gameObject);
            CurrentPosition += len;
            //GameObject tmp = GBsOnScene[0];




        }
    }
}
