using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FairyGUI;
using Modules;
using UnityEditor;
using System;

public class SceneSwitch : MonoBehaviour
{
    public GComponent mainUI;
    public GComponent LoginWnd;
    public GComponent EnterWnd;
    public GComponent IntroduceWnd;
    public GComponent CharacterWnd;

    public GComponent MainWnd;
    public GComponent ReturnWnd;
    public GComponent CheckPointWnd;
    public GComponent CheckPointPreviewWnd;
    public GComponent LoadGameWnd;

    public GComponent GameWnd;
    Transition tran1;
    Transition tran2;

    public GProgressBar load;
    public float loadValue;
    private void Awake()
    {
        MyEventSystem.Instance.Subscribe(StateChanged.Id, OnSC);

        UIPackage.AddPackage("FGUI/Package1");
        mainUI = GetComponent<UIPanel>().ui;

        LoginWnd = UIPackage.CreateObject("Package1", "LoginWnd").asCom;
        EnterWnd = UIPackage.CreateObject("Package1", "EnterWnd").asCom;
        IntroduceWnd = UIPackage.CreateObject("Package1", "IntroduceWnd").asCom;
        CharacterWnd = UIPackage.CreateObject("Package1", "CharacterWnd").asCom;
        MainWnd = UIPackage.CreateObject("Package1", "MainWnd").asCom;
        ReturnWnd = UIPackage.CreateObject("Package1", "ReturnWnd").asCom;
        CheckPointWnd = UIPackage.CreateObject("Package1", "CheckPointWnd").asCom;
        CheckPointPreviewWnd = UIPackage.CreateObject("Package1", "CheckPointPreviewWnd").asCom;
        LoadGameWnd = UIPackage.CreateObject("Package1", "LoadGameWnd").asCom;
        //Game
        GameWnd = UIPackage.CreateObject("Package1", "GameWnd").asCom;

        //DontDestroyOnLoad(this.gameObject);


        tran1 = CharacterWnd.GetTransition("t1");
        tran2 = CharacterWnd.GetTransition("t0");
        //LoginWnd.visible = true;
        GRoot.inst.AddChild(LoginWnd);
        LoginWnd.GetChild("n20").onClick.Add(() => {
            print("登陆成功");
            GRoot.inst.RemoveChild(LoginWnd);
            GRoot.inst.AddChild(EnterWnd);
        });
        EnterWnd.onClick.Add(() =>
        {
            GRoot.inst.RemoveChild(EnterWnd);
            GRoot.inst.AddChild(IntroduceWnd);
        });
        IntroduceWnd.onClick.Add(() =>
        {
            GRoot.inst.RemoveChild(IntroduceWnd);
            GRoot.inst.AddChild(CharacterWnd);
        });
        GButton n3 = CharacterWnd.GetChild("n3").asButton;
        GButton n4 = CharacterWnd.GetChild("n4").asButton;
        n3.onClick.Add(() =>
        {
            tran1.Play();
            tran2.PlayReverse();
            n4.selected = false;
        });
        n4.onClick.Add(() =>
        {
            tran2.Play();
            tran1.PlayReverse();
            n3.selected = false;
        });
        CharacterWnd.GetChild("n1").onClick.Add(() =>
        {
            
            GRoot.inst.RemoveChild(CharacterWnd);
            GRoot.inst.AddChild(IntroduceWnd);
        });
        CharacterWnd.GetChild("n10").onClick.Add(() =>
        {
            GRoot.inst.RemoveChild(CharacterWnd);
            //SceneModule.Instance.LoadScene(2);
            //GRoot.inst.AddChild(GameWnd);

            //Game.GameContrller.Instance.ChangeState(Game.GameStateEnum.Game);
            SceneManager.LoadScene(1);
            GRoot.inst.AddChild(MainWnd);

        });
        //GameWnd.GetChild("n7").onClick.Add(() => UpJumpMethod());
        MainWnd.GetChild("n9").onClick.Add(() =>
        {
            GRoot.inst.RemoveChild(MainWnd);
            GRoot.inst.AddChild(CheckPointWnd);
        });
        CheckPointWnd.GetChild("n4").onClick.Add(() =>
        {
            GRoot.inst.ShowPopup(CheckPointPreviewWnd);
            CheckPointPreviewWnd.SetXY(800, 100);
        });
        GameWnd.GetChild("n19").onClick.Add(() =>
        {
            //GRoot.inst.RemoveChild(GameWnd);
            //GRoot.inst.AddChild(MainWnd);
            //SceneManager.LoadScene("Lobby");
        });
        GameWnd.GetChild("n20").onClick.Add(() =>
        {
            //GRoot.inst.RemoveChild(GameWnd);
            //GRoot.inst.AddChild(CharacterWnd);
            //SceneManager.LoadScene("Login");
        });
        CheckPointPreviewWnd.GetChild("n4").onClick.Add(() =>
        {
            GRoot.inst.RemoveChild(CheckPointPreviewWnd);
            GRoot.inst.RemoveChild(CheckPointWnd);

            
            StartCoroutine("LoadGameScene");
            //GRoot.inst.AddChild(GameWnd);
            //SceneManager.LoadScene(2);
            //GRoot.inst.AddChild(LoadGameWnd);
            //print("场景加载时间为3s");
        });
        //LoadGameWnd.onClick.Add(() =>
        //{
        //    GRoot.inst.RemoveChild(LoadGameWnd);
        //    GRoot.inst.AddChild(GameWnd);
        //});
        MainWnd.GetChild("n11").onClick.Add(() =>
        {
            GRoot.inst.ShowPopup(ReturnWnd);
            ReturnWnd.Center();
        });
        ReturnWnd.GetChild("n2").onClick.Add(() =>
        {
            Application.Quit();
        });
        ReturnWnd.GetChild("n3").onClick.Add(() =>
        {
            GRoot.inst.RemoveChild(ReturnWnd);
        });
        Init();
    }

    private void OnSC(object sender, GameEventArgs args)
    {
        StateChanged state = args as StateChanged;
        if (state.GameState.Type == Game.GameStateEnum.Finish)
        {
            //SceneManager.LoadScene("Finish");
            //GRoot.inst.RemoveChild(GameWnd);
            //GRoot.inst.AddChild(MainWnd);
        }
        else if (state.GameState.Type == Game.GameStateEnum.Lobby)
        {
            Debug.Log("场景转至Lobby");
            //SceneManager.LoadScene(1);
        }
    }

    virtual protected void Init()
    {

    }
    //virtual protected void UpJumpMethod()
    //{
    //    print("上跳1");
    //}
    IEnumerator LoadGameScene()
    {
        GRoot.inst.AddChild(LoadGameWnd);
        AsyncOperation async = SceneModule.Instance.LoadSceneAsync(2);
        async.allowSceneActivation = false;
        //while (!async.isDone)
        while (async.progress<=0.88)
        {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(3);
        GRoot.inst.RemoveChild(LoadGameWnd);
        async.allowSceneActivation = true;
        async = null;
        GRoot.inst.AddChild(GameWnd);
    }
}
