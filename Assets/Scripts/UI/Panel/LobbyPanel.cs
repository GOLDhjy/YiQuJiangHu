using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FairyGUI;
using Modules;

public class LobbyPanel : UIBasePanel
{

    public GComponent MainWnd;
    public GComponent ReturnWnd;
    public GComponent CheckPointWnd;
    public GComponent CheckPointPreviewWnd;
    public GComponent CheckPointPreviewWnd2;
    public GComponent CheckPointPreviewWnd3;
    public GComponent CheckPointPreviewWnd4;
    public GComponent LoadGameWnd;
    
    //角色选择
    public GButton backCharacterBtn;
    //成就
    public GButton successListBtn;
    //排行榜
    public GButton rankListBtn;
    //设置
    public GButton setBtn;
    //退出游戏
    public GButton returnGameBtn;
    //开始游戏
    public GButton startGameBtn;
    //返回大厅
    public GButton checkBtnBack;

    public GImage lobbyBk;
    public GImage cpwBk;
    public GProgressBar loadBar;
    public LobbyPanel(string packageName, UIWndType uIWndType, UIBase uIBaseManager) : base(packageName, uIWndType, uIBaseManager)
    {
    }
    protected override void OnAwake()
    {
        Debug.Log("Lobby场景加包入口");
        MainWnd = UIPackage.CreateObject("Package1", "MainWnd").asCom;
        ReturnWnd = UIPackage.CreateObject("Package1", "ReturnWnd").asCom;
        CheckPointWnd = UIPackage.CreateObject("Package1", "CheckPointWnd").asCom;
        CheckPointPreviewWnd = UIPackage.CreateObject("Package1", "CheckPointPreviewWnd").asCom;
        CheckPointPreviewWnd2 = UIPackage.CreateObject("Package1", "CheckPointPreviewWnd2").asCom;
        CheckPointPreviewWnd3 = UIPackage.CreateObject("Package1", "CheckPointPreviewWnd3").asCom;
        CheckPointPreviewWnd4 = UIPackage.CreateObject("Package1", "CheckPointPreviewWnd4").asCom;
        LoadGameWnd = UIPackage.CreateObject("Package1", "IntroduceWnd").asCom;

        checkBtnBack = CheckPointWnd.GetChild("n1").asButton;
        lobbyBk = contentPane.GetChild("n38").asImage;
        cpwBk = CheckPointWnd.GetChild("n15").asImage;
        loadBar = LoadGameWnd.GetChild("n11").asProgress;
    }
    //MonoBehaviour mono;
    protected override void OnInitPanel()
    {
        lobbyBk.SetSize(GRoot.inst.width + 50, GRoot.inst.height + 50);
        cpwBk.SetSize(GRoot.inst.width + 50, GRoot.inst.height + 50);

        lobbyBk.AddRelation(GRoot.inst, RelationType.Size);
        cpwBk.AddRelation(GRoot.inst, RelationType.Size);

        MyEventSystem.Instance.Subscribe(StateChanged.Id, OnScene);
        contentPane.GetChild("n9").onClick.Add(() =>
        {
            GRoot.inst.RemoveChild(MainWnd);
            GRoot.inst.AddChild(CheckPointWnd);
            contentPane.visible = false;
        });
        contentPane.GetChild("n11").onClick.Add(() =>
        {
            GRoot.inst.ShowPopup(ReturnWnd);
            ReturnWnd.SetXY(610,180);
        });
        ReturnWnd.GetChild("n2").onClick.Add(() =>
        {
            Application.Quit();
        });
        ReturnWnd.GetChild("n3").onClick.Add(() =>
        {
            GRoot.inst.RemoveChild(ReturnWnd);
        });
        checkBtnBack.onClick.Add(() =>
        {
            GRoot.inst.RemoveChild(CheckPointWnd);
            GRoot.inst.AddChild(MainWnd);
        });
        MainWnd.GetChild("n9").onClick.Add(() =>
        {
            GRoot.inst.RemoveChild(MainWnd);
            GRoot.inst.AddChild(CheckPointWnd);
            contentPane.visible = false;
        });
        CheckPointWnd.GetChild("n18").onClick.Add(() =>
        {
            Debug.Log("进入荒漠关卡");
            GRoot.inst.ShowPopup(CheckPointPreviewWnd);
            CheckPointPreviewWnd.SetXY(1000, 100);
        });
        CheckPointWnd.GetChild("n19").onClick.Add(() =>
        {
            Debug.Log("进入师门关卡");
            GRoot.inst.ShowPopup(CheckPointPreviewWnd2);
            CheckPointPreviewWnd2.SetXY(1000, 100);
        });
        CheckPointWnd.GetChild("n20").onClick.Add(() =>
        {
            Debug.Log("进入城镇关卡");
            GRoot.inst.ShowPopup(CheckPointPreviewWnd3);
            CheckPointPreviewWnd3.SetXY(1000, 100);
        });
        CheckPointWnd.GetChild("n21").onClick.Add(() =>
        {
            Debug.Log("进入竹林关卡");
            GRoot.inst.ShowPopup(CheckPointPreviewWnd4);
            CheckPointPreviewWnd4.SetXY(1000, 100);
        });
        CheckPointPreviewWnd.GetChild("n4").onClick.Add(() =>
        {
            UIManager.Instance.LoadingBar.SetActive(true);
            GRoot.inst.RemoveChild(CheckPointPreviewWnd);
            GRoot.inst.RemoveChild(CheckPointWnd);
            //mono.StartCoroutine("LoadGameScene");
            // AsyncOperation async = SceneModule.Instance.LoadSceneAtABAsync(1+"_Base_MusicScene");
            //SceneModule.Instance.LoadScene(1 + "_Base_MusicScene");
            SceneModule.Instance.LoadSceneAtAB(1 + "_Base_MusicScene");
            //LoadGameScene();
           // GRoot.inst.RemoveChild(LoadGameWnd);
            //EnterOtherPanel(UIWndType.GameWnd);
        });
        CheckPointPreviewWnd2.GetChild("n4").onClick.Add(() =>
        {
            UIManager.Instance.LoadingBar.SetActive(true);
            GRoot.inst.RemoveChild(CheckPointPreviewWnd2);
            GRoot.inst.RemoveChild(CheckPointWnd);
            SceneModule.Instance.LoadSceneAtAB(2 + "_Base_MusicScene");
            //GRoot.inst.RemoveChild(LoadGameWnd);
            //LoadGameScene();
        });
        CheckPointPreviewWnd3.GetChild("n4").onClick.Add(() =>
        {
            UIManager.Instance.LoadingBar.SetActive(true);
            GRoot.inst.RemoveChild(CheckPointPreviewWnd3);
            GRoot.inst.RemoveChild(CheckPointWnd);
            SceneModule.Instance.LoadSceneAtAB(3 + "_Base_MusicScene");
        });
        CheckPointPreviewWnd4.GetChild("n4").onClick.Add(() =>
        {
            UIManager.Instance.LoadingBar.SetActive(true);
            Debug.Log("正式进入竹林关卡");
            GRoot.inst.RemoveChild(CheckPointPreviewWnd4);
            GRoot.inst.RemoveChild(CheckPointWnd);
            
            //gameObject.SetActive(true);
            Debug.Log("打开加载条");
            SceneModule.Instance.LoadSceneAtAB(4 + "_Base_MusicScene");
        });
    }
    private void OnScene(object sender, GameEventArgs args)
    {
        StateChanged state = args as StateChanged;
        if (state.GameState.Type == Game.GameStateEnum.Finish) { }
        else if (state.GameState.Type == Game.GameStateEnum.Lobby) { }
    }
    float timer;
    IEnumerator LoadGameScene()
    {
        Debug.Log("进入携程");
        //GRoot.inst.RemoveChild(CheckPointPreviewWnd);
        //GRoot.inst.RemoveChild(CheckPointWnd);

        GRoot.inst.AddChild(LoadGameWnd);
        loadBar.value = 1;
        loadBar.TweenValue(100, 5);
        //AsyncOperation async = SceneModule.Instance.LoadSceneAtABAsync(1 + "_Base_MusicScene");
        //async.allowSceneActivation = false;
        ////while (!async.isDone)
        //while (async.progress <= 0.88)
        //{
        //yield return new WaitForEndOfFrame();
        //}
        timer += Time.deltaTime;
        yield return new WaitForSeconds(5);
       
        
        //async.allowSceneActivation = true;
        //async = null;
        //EnterOtherPanel(UIWndType.GameWnd);
        //GRoot.inst.AddChild(GameWnd);
    }



}
