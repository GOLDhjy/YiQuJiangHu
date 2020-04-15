using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using UnityEngine.SceneManagement;
using Modules;
using Game;

public class UIBasePanel : Window
{
    //基础属性 用则取
    protected string packageName;
    protected Transition transition;
    protected Controller controller;
    protected string wndName;
    //遮罩遮挡场景
    protected GComponent panelMask;
    //当前窗口
    protected UIWndType currentWndType;
    protected UIBase uIBaseManager;
    protected Scene scene;
    protected GComponent currentWnd;

    //packageName 为窗口名！
    public UIBasePanel(string packageName,UIWndType uIWndType,UIBase uIBaseManager)
    {
        this.packageName = packageName;
        currentWndType = uIWndType;
        this.uIBaseManager = uIBaseManager;
        //UIPackage.AddPackage("FGUI/" + packageName);
        currentWnd= UIPackage.CreateObject("Package1", packageName).asCom;
    }
    protected override void OnInit()
    {
        MyEventSystem.Instance.Subscribe(StateChanged.Id, OnSC);
        Debug.Log("进入加包初始化");
        contentPane = UIPackage.CreateObject("Package1", packageName).asCom;
        OnAwake();
        OnInitPanel();
    }
    protected virtual void OnInitPanel()
    {
        Debug.Log("属性初始化");
    }
    protected virtual void OnAwake()
    {

    }
    //进入页面
    public void EnterPanel()
    {

    }
    //进入其他界面
    public void EnterOtherPanel(UIWndType otherType)
    {
        //ExitPanel(() =>
        //{
            ChangePanel(otherType);
        //});
    }
    //退出界面
    protected void ExitPanel(PlayCompleteCallback playCompleteCallback)
    {
        //在添加界面特效时使用
    }
    protected void ChangePanel(UIWndType otherType)
    {
        //uIBaseManager.UIPanelDict[currentWndType].Hide();
        uIBaseManager.UIPanelDict[otherType].Show();
        //uIBaseManager.UIPanelDict[otherType].EnterPanel();
    }
    private void OnSC(object sender, GameEventArgs args)
    {
        StateChanged state = args as StateChanged;
        if (state.GameState.Type == Game.GameStateEnum.Finish)
        {
            EnterOtherPanel(UIWndType.ReturnWnd);
        }
        else if (state.GameState.Type == Game.GameStateEnum.Lobby)
        {
            EnterOtherPanel(UIWndType.MainWnd);
        }
        else if (state.GameState.Type == GameStateEnum.Game)
        {
            EnterOtherPanel(UIWndType.GameWnd);
            UIManager.Instance.LoadingBar.SetActive(false);
            //GameObject gameObject = GameObject.Find("LoadingBar");
            //Debug.Log("关闭加载条");
            //gameObject.SetActive(false);
        }
        else if (state.GameState.Type == GameStateEnum.Login)
        {
            EnterOtherPanel(UIWndType.LoginWnd);
        }
        else
        {
            Debug.Log("暴打");
        }
    }
}
