using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FairyGUI;
using Game;
using Modules;
using NetWork;
using System;

public class LoginPanel : UIBasePanel
{
    public GComponent LoginWnd;
    public GComponent EnterWnd;
    public GComponent IntroduceWnd;
    public GComponent CharacterWnd;
    public GImage chooseMan;
    public GImage chooseWomen;
    Transition tran1;
    Transition tran2;

    public GImage background1;
    public GImage enterBackground1;
    public GImage enterBackground2;
    public GImage introduceBk;
    public GImage characterBk;
    public GImage characterBk2;

    public GButton loginButton;

    //账号密码
    public GTextInput accout;
    public GTextInput passward;
    public LoginPanel(string packageName, UIWndType uIWndType, UIBase uIBaseManager) : base(packageName, uIWndType, uIBaseManager)
    {
    }
    protected override void OnAwake()
    {
        MyEventSystem.Instance.Subscribe(LoginEvent.id, OnLogin);
        Debug.Log("这里是login入口");
        LoginWnd = UIPackage.CreateObject("Package1", "LoginWnd").asCom;
        EnterWnd = UIPackage.CreateObject("Package1", "EnterWnd").asCom;
        IntroduceWnd = UIPackage.CreateObject("Package1", "IntroduceWnd").asCom;
        CharacterWnd = UIPackage.CreateObject("Package1", "CharacterWnd").asCom;
        tran1 = CharacterWnd.GetTransition("t1");
        tran2 = CharacterWnd.GetTransition("t0");
        chooseMan = CharacterWnd.GetChild("img_man").asImage;
        chooseWomen = CharacterWnd.GetChild("img_women").asImage;

        background1 = contentPane.GetChild("n30").asImage;
        loginButton = contentPane.GetChild("n20").asButton;
        enterBackground1 = EnterWnd.GetChild("n9").asImage;
        introduceBk = IntroduceWnd.GetChild("n9").asImage;
        characterBk = CharacterWnd.GetChild("n30").asImage;
        characterBk2 = CharacterWnd.GetChild("n40").asImage;
        //获取账号密码 提供对应接口
        accout = contentPane.GetChild("n22").asTextInput;
        passward = contentPane.GetChild("n23").asTextInput;


    }

    private void OnLogin(object sender, GameEventArgs e)
    {
       
    }

    protected override void OnInitPanel()
    {
        background1.SetSize(GRoot.inst.width, GRoot.inst.height);
        enterBackground1.MakeFullScreen();
        //introduceBk.MakeFullScreen();
        introduceBk.SetSize(GRoot.inst.width + 50, GRoot.inst.height + 50);
        characterBk.MakeFullScreen();
        characterBk2.MakeFullScreen();
        //enterBackground2.MakeFullScreen();

        background1.AddRelation(GRoot.inst, RelationType.Size);
        enterBackground1.AddRelation(GRoot.inst, RelationType.Size);
        introduceBk.AddRelation(GRoot.inst, RelationType.Size);
        characterBk.AddRelation(GRoot.inst, RelationType.Size);
        characterBk2.AddRelation(GRoot.inst, RelationType.Size);

        chooseMan.visible = false;
        chooseWomen.visible = false;
        contentPane.GetChild("n20").onClick.Add(() =>
        {
            GRoot.inst.RemoveChild(LoginWnd);
            //GRoot.inst.AddChild(EnterWnd);
            contentPane.visible = false;
            SceneManager.LoadScene(1);
        });
        loginButton.onClick.Add(() => {
            NetWorkModule.Instance.Client_regist_req(accout.ToString(), passward.ToString());
            NetWorkModule.Instance.Client_login_req(accout.ToString(), passward.ToString());
        });
        EnterWnd.onClick.Add(() =>
        {
            GRoot.inst.RemoveChild(EnterWnd);
            GRoot.inst.AddChild(CharacterWnd);
        });
        //IntroduceWnd.onClick.Add(() =>
        //{
        //    GRoot.inst.RemoveChild(IntroduceWnd);
        //    GRoot.inst.AddChild(CharacterWnd);
        //});
        //女角色
        GButton n3 = CharacterWnd.GetChild("n3").asButton;
        //男角色
        GButton n4 = CharacterWnd.GetChild("n4").asButton;
        n3.onClick.Add(() =>
        {
            tran1.Play();
            tran2.PlayReverse();
            n4.selected = false;
            chooseWomen.visible = true;
            chooseMan.visible = false;
        });
        n4.onClick.Add(() =>
        {
            tran2.Play();
            tran1.PlayReverse();
            n3.selected = false;
            chooseMan.visible = true;
            chooseWomen.visible = false;
        });
        CharacterWnd.GetChild("n1").onClick.Add(() =>
        {
            GRoot.inst.RemoveChild(CharacterWnd);
            GRoot.inst.AddChild(EnterWnd);
        });
        //进入游戏场景
        CharacterWnd.GetChild("n10").onClick.Add(() =>
        {
            GRoot.inst.RemoveChild(CharacterWnd);
            //EnterOtherPanel(UIWndType.MainWnd);
            //有BGM的时候进行添加
            //UIManager.Instance.audioSourceManager.ChangeBGM(1);
            SceneManager.LoadScene(1);
        });
    }
    public void OnShutDown()
    {
        MyEventSystem.Instance.UnSubscribe(LoginEvent.id, OnLogin);
    }

}
