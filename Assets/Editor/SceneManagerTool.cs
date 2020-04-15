using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor;
using System.IO;
using System;

public class MultiScene : EditorWindow
{
    static float Maxx = -100;
    static GameObject[] gameObjects;
    public static int DistanceScene = 500;
    static Scene[] FenGeScenes;
    static Scene Basescene;
    static List<GameObject> CloneGB = new List<GameObject>();

    [MenuItem("Framework/StartGame")]
    static void StartGame()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Login/Login.unity");
        EditorApplication.ExecuteMenuItem("Edit/Play");
    }
    [MenuItem("Framework/GoTestGame")]
    static void TestGame()
    {
        if (EditorApplication.isPlaying)
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            EditorUtility.DisplayDialog("错误", "请先启动游戏", "确定");
        }
    }
    [MenuItem("Framework/GoAnyGame")]
    static void AnyGame()
    {
        CreateInstance<MultiScene>().ShowUtility();
    }

    int JumpLevel;
    private void OnGUI()
    {
        JumpLevel = EditorGUILayout.IntField("跳转关数",JumpLevel);
        if (GUILayout.Button("跳转"))
        {
            if (EditorApplication.isPlaying)
            {
                Modules.SceneModule.Instance.LoadSceneAtAB(JumpLevel + "_Base_MusicScene");
            }
            else
            {
                EditorUtility.DisplayDialog("错误", "请先启动游戏", "确定");
            }
        }
    }


    /// <summary>
    /// 分割场景
    /// </summary>
    [MenuItem("SceneManagerTool/FenGe")]
    static void Creat()
    {
        //找到所有需要转移的物体
        gameObjects = GameObject.FindGameObjectsWithTag("Terrain");
        Basescene = gameObjects[0].scene;
        
        for (int i = 0; i < gameObjects.Length; i++)
        {
            
            if (gameObjects[i].transform.position.x>Maxx)
            {
                Maxx = gameObjects[i].transform.position.x;
            }
        }
        //找到需要的场景数
        int NumScene;
        if ((Maxx % DistanceScene)!=0)
        {
            NumScene = (int)(Maxx / DistanceScene) + 1;

        }
        else
        {
            NumScene = (int)(Maxx / DistanceScene);
        }
        FenGeScenes = new Scene[NumScene];
        //分配物体，并且重新创建场景
        for (int i = 0; i < gameObjects.Length; i++)
        {
            //如果物体有父节点就不需要复制
            if (gameObjects[i].transform.parent != null)
            {
                continue;
            }

            int index;
            if ((gameObjects[i].transform.position.x / DistanceScene)<0)
            {
                index = 0;
            }
            else
                index = (int)(gameObjects[i].transform.position.x / DistanceScene);
            //为什么使用复制，是因为如果直接把原物体移动到另一个场景，如果另一个场景资源都还不存在，是不能移动过去的。
            GameObject clone = GameObject.Instantiate(gameObjects[i]);
            CloneGB.Add(gameObjects[i]);
            if (!FenGeScenes[index].isLoaded)
            {
                FenGeScenes[index] = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
                FenGeScenes[index].name = index + "_FenGe_From_" + Basescene.name;
                string path = "Assets/Scenes/GameScene/" + "Level_" + Basescene.name[0]; 
                //AssetDatabase.GetAssetOrScenePath(FenGeScenes[i].);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                EditorSceneManager.SaveScene(FenGeScenes[index], "Assets/Scenes/GameScene/" + "Level_" + Basescene.name[0] + "/" + FenGeScenes[index].name + ".unity");
            }
            EditorSceneManager.MoveGameObjectToScene(clone, FenGeScenes[index]);
            clone.name = gameObjects[i].name;
        }
        //在分配物体给新场景后，还需要保存，不然卸载场景后，新添加的场景是没有的
        for (int i = 0; i < FenGeScenes.Length; i++)
        {
            EditorSceneManager.SaveScene(FenGeScenes[i], "Assets/Scenes/GameScene/" + "Level_" + Basescene.name[0] + "/" + FenGeScenes[i].name + ".unity");
        }
        //先清除原本的物体，在保存Base场景
        Clear();
        EditorSceneManager.SaveScene(Basescene, "Assets/Scenes/GameScene/" + "Level_" + Basescene.name[0] + "/" + Basescene.name + ".unity");
       
    }
    /// <summary>
    /// 将几个场景合并在Base场景里面
    /// </summary>
    [MenuItem("SceneManagerTool / Merge")]
    public static void Merge()
    {
        Scene[] scenes = new Scene[SceneManager.sceneCount];
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            scenes[i] = SceneManager.GetSceneAt(i);
            
        }
        int SceneNum = SceneManager.sceneCount;
        for (int i = 1; i < SceneNum; i++)
        {
            EditorSceneManager.MergeScenes(scenes[i], scenes[0]);
        }
        GameObject Root = GameObject.Find("Envirment");
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Terrain");
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i].transform.parent!=null)
            {
                continue;
            }
            gameObjects[i].transform.SetParent(Root.transform);
        }
        EditorSceneManager.SaveScene(scenes[0], scenes[0].path);
    }

    /// <summary>
    /// 清除原场景的物体，并且释放堆内存
    /// </summary>
    private static void Clear()
    {
        Maxx = -100;
        gameObjects = null;
        FenGeScenes = null;
        for (int i = 0; i < CloneGB.Count; i++)
        {
            DestroyImmediate(CloneGB[i]);
        }
}
}
