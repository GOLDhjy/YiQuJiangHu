using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class FenGe : MonoBehaviour
{
    public static GameObject Root;
    public static GameObject[] GbList;
    public static List<List<GameObject>> ChunksGb;
    public static GameObject[] Chunks;
    public const float Distance = 100;
   // public static 
    public static int Num;

    private void Start()
    {
       
    }
    static void  Init()
    {
        Root = GameObject.Find("Root");
        GbList = new GameObject[Root.transform.childCount];
        
    }
    [MenuItem("Tool/FenGe")]
    public static void FenGeDiTu()
    {

        Init();

        for (int i = 0; i < Root.transform.childCount; i++)
        {
            GbList[i] = Root.transform.GetChild(i).gameObject;
            int tmp = Convert.ToInt32(GbList[i].transform.position.x / Distance);
            if (tmp > Num)
            {
                Num = tmp;
            }
        }
        Num++;
        ChunksGb = new List<List<GameObject>>(Num);
        for (int i = 0; i < Num; i++)
        {
            ChunksGb.Add(new List<GameObject>());
        }
        foreach (var item in GbList)
        {
            //if (ChunksGb[Convert.ToInt32(item.transform.position.x / 100)].Count==0)
            //{
            //    ChunksGb[Convert.ToInt32(item.transform.position.x / 100)] = new List<GameObject>();
            //}
            //else
            if (!item.name.Contains("Chunk"))
            {
                ChunksGb[((int)(item.transform.position.x / Distance))].Add(item);
            }
               
        }
        Chunks = new GameObject[Num];
        for (int i = 0; i < Num; i++)
        {
            Chunks[i] = new GameObject();
            Chunks[i].transform.SetParent(Root.transform);
            Chunks[i].name = "Chunk" + (i+1);
            Chunks[i].transform.position =new Vector3(i * Distance, 0,0);
            //Chunks[i].GetComponent<Mesh>();
            for (int j = 0; j < ChunksGb[i].Count; j++)
            {
                ChunksGb[i][j].transform.SetParent(Chunks[i].transform);
            }
            string path = Application.dataPath + "/Scenes/" + Chunks[i].name + ".prefab";
            PrefabUtility.SaveAsPrefabAsset(Chunks[i], path);
        }
        


    }

    //GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Envirment");
    //for (int i = 0; i < gameObjects.Length; i++)
    //{

    //}

}
