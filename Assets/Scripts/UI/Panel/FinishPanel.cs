using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FairyGUI;
using Modules;
using Game;
public class FinishPanel : UIBasePanel
{
    //结算场景
    public GComponent FinishWnd;

    public GImage finishBk;
    public GImage winLogo;
    public GImage failureLogo;
    public GImage failureLogo2;

    public GButton returnLobbyBtn;
    public GButton continueAdventure;
    public GButton againChooseLevel;

    int num;
    public FinishPanel(string packageName, UIWndType uIWndType, UIBase uIBaseManager) : base(packageName, uIWndType, uIBaseManager)
    {
    }
    protected override void OnAwake()
    {
        Debug.Log("进入FinishPanel");
        num = Random.Range(1, 10);
        //Debug.Log(num);
        FinishWnd = UIPackage.CreateObject("Package1", "GameWnd").asCom;

        finishBk = contentPane.GetChild("n1").asImage;
        winLogo = contentPane.GetChild("win").asImage;
        failureLogo = contentPane.GetChild("failure").asImage;
        failureLogo2 = contentPane.GetChild("failure2").asImage;

        returnLobbyBtn = contentPane.GetChild("n6").asButton;
        continueAdventure = contentPane.GetChild("n8").asButton;
        againChooseLevel = contentPane.GetChild("n9").asButton;

        MyEventSystem.Instance.Subscribe(StateChanged.Id, OnSC);
        //MyEventSystem.Instance.Subscribe(HpChange.id, HpJudge);

        winLogo.visible = false;
        failureLogo.visible = true;
        failureLogo2.visible = true;
    }
    protected override void OnInitPanel()
    {
        finishBk.MakeFullScreen();
        finishBk.AddRelation(GRoot.inst, RelationType.Size);
        returnLobbyBtn.onClick.Add(() =>
        {
            GRoot.inst.RemoveChild(contentPane);
            SceneManager.LoadScene(1);
        });
        continueAdventure.onClick.Add(() =>
        {
            GRoot.inst.RemoveChild(contentPane);
            SceneModule.Instance.LoadSceneAtAB(Random.Range(1,5) + "_Base_MusicScene");
        });
        againChooseLevel.onClick.Add(() =>
        {
            GRoot.inst.RemoveChild(contentPane);
            SceneManager.LoadScene(1);
        });
        WinOrF();
    }
    private void HpJudge(object sender, GameEventArgs e)
    {
        HpChange hp = e as HpChange;
        //bloodValue = (hp.Hp/LocalPlayer.Instance.Max_hp)/100f;
        Debug.Log(hp.Hp);
        
    }
    private void WinOrF()
    {
        Debug.Log("显示结算界面");
        if (LocalPlayer.Instance.m_hp > 0)
        {
            failureLogo.visible = false;
            failureLogo2.visible = false;
            winLogo.visible = true;
        }
        else
        {
            winLogo.visible = false;
            failureLogo.visible = true;
            failureLogo2.visible = true;
        }
    }
    private void OnSC(object sender, GameEventArgs args)
    {
        StateChanged state = args as StateChanged;
        if (state.GameState.Type == Game.GameStateEnum.Finish)
        {
            Debug.Log("进入Finish");
        }
        else if (state.GameState.Type == Game.GameStateEnum.Lobby)
        {
            Debug.Log("场景由finish跳转至lobby");
        }
    }
    public void OnShutDown()
    {
        MyEventSystem.Instance.UnSubscribe(StateChanged.Id, OnSC);
        //MyEventSystem.Instance.UnSubscribe(HpChange.id, HpJudge);
    }

}
