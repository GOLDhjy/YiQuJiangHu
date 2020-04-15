using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FairyGUI;

public class LoadGameBar : MonoBehaviour
{
    public GComponent LoadGameWnd;
    public GComponent GameWnd;

    public GProgressBar load;
    public float loadValue;

    void Start()
    {
        LoadGameWnd = UIPackage.CreateObject("Package1", "LoadGameWnd").asCom;
        GameWnd = UIPackage.CreateObject("Package1", "GameWnd").asCom;
        load = LoadGameWnd.GetChild("n3").asProgress;
        StartCoroutine("LoadGameScene");

        
    }
    AsyncOperation asyneGameScene=null;
    
    IEnumerator LoadGameScene()
    {
        asyneGameScene = SceneManager.LoadSceneAsync(2);
        asyneGameScene.allowSceneActivation = false;
        while (!asyneGameScene.isDone)
        {
            if (asyneGameScene.progress < 0.85f)
            {
                loadValue = asyneGameScene.progress;
            }
            else
            {
                loadValue = 1f;
            }
            //Debug.Log("1"+load.value);
            //Debug.Log("2"+loadValue);
            load.value = loadValue;
            load.text = (int)(load.value * 100) + " %";
            if (loadValue >= 0.85f)
            {
                load.text = "触摸屏幕继续";
                Debug.Log("加载完成");
                //asyneGameScene.allowSceneActivation = true;
                LoadGameWnd.onClick.Add(() =>
                {
                    asyneGameScene.allowSceneActivation = true;
                    GRoot.inst.RemoveChild(LoadGameWnd);
                    GRoot.inst.AddChild(GameWnd);
                });
            }
            yield return new WaitForEndOfFrame();
        }
    }
    void Update()
    {
        
    }
}
