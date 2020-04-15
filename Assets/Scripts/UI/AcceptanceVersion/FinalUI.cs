using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using Modules;
using Game;
using NetWork;
using UnityEngine.SceneManagement;

public class FinalUI : MonoBehaviour
{
    //Login模块
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
    public GTextInput accout;
    public GTextInput passward;

    //Lobby模块
    public GComponent MainWnd;
    public GComponent ReturnWnd;
    public GComponent CheckPointWnd;
    public GComponent CheckPointPreviewWnd;
    public GComponent CheckPointPreviewWnd2;
    public GComponent CheckPointPreviewWnd3;
    public GComponent CheckPointPreviewWnd4;
    public GComponent LoadGameWnd;
    public GButton backCharacterBtn;
    public GButton successListBtn;
    public GButton rankListBtn;
    public GButton setBtn;
    public GButton returnGameBtn;
    public GButton startGameBtn;

    public GImage lobbyBk;
    public GImage cpwBk;

    //Game模块
    public GComponent GameWnd;
    public GComponent Lobby;

    public GButton upJump;
    public GButton upAttack;
    public GButton downAttack;

    public GButton gameStart;
    public GProgressBar blood;
    public float bloodValue;
    public new GTextField text;
    public GTextField jumpNum;

    GImage veryGood;
    GImage good;
    GImage ok;
    GImage bad;
    GImage veryBad;

    Transition transVeryGood;
    Transition transGood;
    Transition transOk;
    Transition transBad;
    Transition transVeryBad;

    //结算模块
    public GComponent FinishWnd;

    public GImage finishBk;
    public GImage winLogo;
    public GImage failureLogo;
    public GImage failureLogo2;

    public GButton returnLobby;
    public GButton continueAdventure;
    public GButton againChooseLevel;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        UIPackage.AddPackage("FGUI/Package1");
        GRoot.inst.SetContentScaleFactor(1600, 900, UIContentScaler.ScreenMatchMode.MatchWidthOrHeight);

        MyEventSystem.Instance.Subscribe(LoginEvent.id, OnLogin);
        //Login入口
        LoginWnd = UIPackage.CreateObject("Package1", "LoginWnd").asCom;
        EnterWnd = UIPackage.CreateObject("Package1", "EnterWnd").asCom;
        IntroduceWnd = UIPackage.CreateObject("Package1", "IntroduceWnd").asCom;
        CharacterWnd = UIPackage.CreateObject("Package1", "CharacterWnd").asCom;
        tran1 = CharacterWnd.GetTransition("t1");
        tran2 = CharacterWnd.GetTransition("t0");
        chooseMan = CharacterWnd.GetChild("img_man").asImage;
        chooseWomen = CharacterWnd.GetChild("img_women").asImage;

        background1 = LoginWnd.GetChild("n30").asImage;
        loginButton = LoginWnd.GetChild("n20").asButton;
        enterBackground1 = EnterWnd.GetChild("n9").asImage;
        introduceBk = IntroduceWnd.GetChild("n9").asImage;
        characterBk = CharacterWnd.GetChild("n30").asImage;
        characterBk2 = CharacterWnd.GetChild("n40").asImage;
        //获取账号密码 提供对应接口
        accout = LoginWnd.GetChild("n22").asTextInput;
        passward = LoginWnd.GetChild("n23").asTextInput;

        //Lobby入口
        MainWnd = UIPackage.CreateObject("Package1", "MainWnd").asCom;
        ReturnWnd = UIPackage.CreateObject("Package1", "ReturnWnd").asCom;
        CheckPointWnd = UIPackage.CreateObject("Package1", "CheckPointWnd").asCom;
        CheckPointPreviewWnd = UIPackage.CreateObject("Package1", "CheckPointPreviewWnd").asCom;
        CheckPointPreviewWnd2 = UIPackage.CreateObject("Package1", "CheckPointPreviewWnd2").asCom;
        CheckPointPreviewWnd3 = UIPackage.CreateObject("Package1", "CheckPointPreviewWnd3").asCom;
        CheckPointPreviewWnd4 = UIPackage.CreateObject("Package1", "CheckPointPreviewWnd4").asCom;
        LoadGameWnd = UIPackage.CreateObject("Package1", "LoadGameWnd").asCom;

        lobbyBk = MainWnd.GetChild("n38").asImage;
        cpwBk = CheckPointWnd.GetChild("n15").asImage;

        //Game入口
        GameWnd = UIPackage.CreateObject("Package1", "GameWnd").asCom;
        Lobby = UIPackage.CreateObject("Package1", "MainWnd").asCom;

        veryGood = GameWnd.GetChild("n67").asImage;
        good = GameWnd.GetChild("n73").asImage;
        ok = GameWnd.GetChild("n72").asImage;
        bad = GameWnd.GetChild("n69").asImage;
        veryBad = GameWnd.GetChild("n70").asImage;

        blood = GameWnd.GetChild("n6").asProgress;
        upJump = GameWnd.GetChild("n7").asButton;
        upAttack = GameWnd.GetChild("n10").asButton;
        downAttack = GameWnd.GetChild("n9").asButton;
        gameStart = GameWnd.GetChild("n29").asButton;
        text = GameWnd.GetChild("n63").asTextField;
        jumpNum = GameWnd.GetChild("n62").asTextField;

        transVeryGood = GameWnd.GetTransition("t0");
        transGood = GameWnd.GetTransition("t4");
        transOk = GameWnd.GetTransition("t5");
        transBad = GameWnd.GetTransition("t6");
        transVeryBad = GameWnd.GetTransition("t7");

        MyEventSystem.Instance.Subscribe(ScoreChange.id, OnShowScore);
        MyEventSystem.Instance.Subscribe(AttackTypeChange.id, OnAttackTypeChange);
        MyEventSystem.Instance.Subscribe(HpChange.id, OnHpChange);
        Debug.Log("awake hp" + blood.value);

        //Finish入口
        //FinishWnd = UIPackage.CreateObject("Package1", "FinishWnd").asCom;

        //finishBk = FinishWnd.GetChild("n1").asImage;
        //winLogo = FinishWnd.GetChild("win").asImage;
        //failureLogo = FinishWnd.GetChild("failure").asImage;
        //failureLogo2 = FinishWnd.GetChild("failure2").asImage;

        //returnLobby = FinishWnd.GetChild("n6").asButton;
        //continueAdventure = FinishWnd.GetChild("n8").asButton;
        //againChooseLevel = FinishWnd.GetChild("n9").asButton;

        MyEventSystem.Instance.Subscribe(StateChanged.Id, OnSC);
    }
    private void OnLogin(object sender, GameEventArgs e)
    {

    }
    public void OnShutDown()
    {
        MyEventSystem.Instance.UnSubscribe(LoginEvent.id, OnLogin);
        MyEventSystem.Instance.UnSubscribe(ScoreChange.id, OnShowScore);
        MyEventSystem.Instance.UnSubscribe(AttackTypeChange.id, OnAttackTypeChange);
        MyEventSystem.Instance.UnSubscribe(HpChange.id, OnHpChange);
    }
    private void OnHpChange(object sender, GameEventArgs e)
    {
        HpChange hp = e as HpChange;
        //bloodValue = (hp.Hp/LocalPlayer.Instance.Max_hp)/100f;
        blood.value = hp.Hp;
        //Debug.Log("blood" +bloodValue);
        Debug.Log(hp.Hp);
    }

    private void OnAttackTypeChange(object sender, GameEventArgs e)
    {
        AttackTypeChange attackType = e as AttackTypeChange;

        if (attackType.judgeType == JudgeType.Perfect)
        {
            Debug.Log(attackType.judgeType);
            transVeryGood.Play();
        }
        else if (attackType.judgeType == JudgeType.Great)
        {
            Debug.Log(attackType.judgeType);
            transGood.Play();
        }
        else if (attackType.judgeType == JudgeType.Good)
        {
            Debug.Log(attackType.judgeType);
            transOk.Play();
        }
        else if (attackType.judgeType == JudgeType.Miss)
        {
            Debug.Log(attackType.judgeType);
            transBad.Play();
        }
        else if (attackType.judgeType == JudgeType.None)
        {
            Debug.Log(attackType.judgeType);
            transVeryBad.Play();
        }
        else
        {
            Debug.Log("不存在的判定条件，黄金翼检查");
        }

    }

    private void OnShowScore(object sender, GameEventArgs e)
    {
        ScoreChange score = e as ScoreChange;
        text.text = score.Score.ToString();
        jumpNum.text = score.AttackNum.ToString();
    }
    void Start()
    {
        Debug.Log("场景已经初始化");
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
        GRoot.inst.AddChild(LoginWnd);
        LoginWnd.GetChild("n20").onClick.Add(() =>
        {
            GRoot.inst.RemoveChild(LoginWnd);
            GRoot.inst.AddChild(EnterWnd);
            LoginWnd.visible = false;
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
            GRoot.inst.AddChild(IntroduceWnd);
        });
        //进入主场景
        CharacterWnd.GetChild("n10").onClick.Add(() =>
        {
            GRoot.inst.RemoveChild(CharacterWnd);
            //临时取消进入Lobby场景
            //SceneModule.Instance.LoadScene(1);
            GRoot.inst.AddChild(MainWnd);
        });
        lobbyBk.SetSize(GRoot.inst.width + 50, GRoot.inst.height + 50);
        cpwBk.SetSize(GRoot.inst.width + 50, GRoot.inst.height + 50);

        lobbyBk.AddRelation(GRoot.inst, RelationType.Size);
        cpwBk.AddRelation(GRoot.inst, RelationType.Size);

        MyEventSystem.Instance.Subscribe(StateChanged.Id, OnScene);
        MainWnd.GetChild("n9").onClick.Add(() =>
        {
            GRoot.inst.RemoveChild(MainWnd);
            GRoot.inst.AddChild(CheckPointWnd);
            MainWnd.visible = false;
        });
        MainWnd.GetChild("n11").onClick.Add(() =>
        {
            GRoot.inst.ShowPopup(ReturnWnd);
        });
        ReturnWnd.GetChild("n2").onClick.Add(() =>
        {
            Application.Quit();
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
            GRoot.inst.RemoveChild(CheckPointPreviewWnd);
            GRoot.inst.RemoveChild(CheckPointWnd);
            SceneModule.Instance.LoadSceneAtAB(1 + "_Base_MusicScene");
            GRoot.inst.AddChild(GameWnd);
           
        });
        CheckPointPreviewWnd2.GetChild("n4").onClick.Add(() =>
        {
            GRoot.inst.RemoveChild(CheckPointPreviewWnd2);
            GRoot.inst.RemoveChild(CheckPointWnd);
            SceneModule.Instance.LoadSceneAtAB(2 + "_Base_MusicScene");
            GRoot.inst.AddChild(GameWnd);
            
        });
        CheckPointPreviewWnd3.GetChild("n4").onClick.Add(() =>
        {
            GRoot.inst.RemoveChild(CheckPointPreviewWnd3);
            GRoot.inst.RemoveChild(CheckPointWnd);
            SceneModule.Instance.LoadSceneAtAB(3 + "_Base_MusicScene");
            GRoot.inst.AddChild(GameWnd);
        });
        CheckPointPreviewWnd4.GetChild("n4").onClick.Add(() =>
        {
            GRoot.inst.RemoveChild(CheckPointPreviewWnd4);
            GRoot.inst.RemoveChild(CheckPointWnd);
            SceneModule.Instance.LoadSceneAtAB(4 + "_Base_MusicScene");
            GRoot.inst.AddChild(GameWnd);
        });
       
        //Game相关
        upJump.onClick.Add(() => { LocalPlayer.Instance.GetPlayController().JumpUp(15); });
        //downSquat.onClick.Add(() => { LocalPlayer.Instance.GetPlayController().JumpDown(); });

        upAttack.onTouchBegin.Add(() => { LocalPlayer.Instance.GetPlayController().AttackStart(AttackPath.UP); });
        upAttack.onTouchEnd.Add(() => { LocalPlayer.Instance.GetPlayController().AttackFinish(AttackPath.UP); });

        downAttack.onTouchBegin.Add(() => { LocalPlayer.Instance.GetPlayController().AttackStart(AttackPath.DOWM); });
        downAttack.onTouchEnd.Add(() => { LocalPlayer.Instance.GetPlayController().AttackFinish(AttackPath.DOWM); });

        //游戏正式开始按钮
        gameStart.onClick.Add(() =>
        {
            gl = GameObject.Find("StartRoot").GetComponent<GameLogic>();
            Debug.Log("游戏开始咯");
            blood.value = 1;
            blood.TweenValue(100, 3);
            count3++;
            if (gl != null)
            {
                gl.StartGameRun();
            }
            else
            {
                Debug.LogError("未找到Root");
            }

            if (count3 % 2 == 1)
            {
                gameStart.visible = false;
            }
        });
        //游戏暂停按钮
        GameWnd.GetChild("n30").onClick.Add(() =>
        {
            count2++;
            if (count2 % 2 == 0)
            {
                if (gl == null)
                {
                    gl = GameObject.Find("StartRoot").GetComponent<GameLogic>();
                }
                else
                    gl.PauseGameRun();
                Time.timeScale = 0;
            }
            else
            {
                if (gl == null)
                {
                    gl = GameObject.Find("StartRoot").GetComponent<GameLogic>();
                }
                else
                    gl.ResumeGameRun();
                Time.timeScale = 1;
            }
        });
        //游戏恢复暂停按钮
        GameWnd.GetChild("n31").onClick.Add(() =>
        {
            if (gl == null)
            {
                gl = GameObject.Find("StartRoot").GetComponent<GameLogic>();
            }
            gl.ResumeGameRun();
            Time.timeScale = 1;
        });
        GameWnd.GetChild("n19").onClick.Add(() =>
        {
            GRoot.inst.RemoveChild(GameWnd);
            SceneManager.LoadScene("Login");
            GRoot.inst.AddChild(MainWnd);
        });
    }
    GameLogic gl;
    AsyncOperation asy;
    int count2 = 1;
    int count3 = 0;
    //GameLogin
    //GameLogic gl;
    LocalPlayerController player;
    public float timer = 0;
    //public void OnShutDown()
    //{
    //    MyEventSystem.Instance.UnSubscribe(ScoreChange.id, OnShowScore);
    //    MyEventSystem.Instance.UnSubscribe(AttackTypeChange.id, OnAttackTypeChange);
    //    MyEventSystem.Instance.UnSubscribe(HpChange.id, OnHpChange);
    //}
    private void OnScene(object sender, GameEventArgs args)
    {
        StateChanged state = args as StateChanged;
        if (state.GameState.Type == Game.GameStateEnum.Finish) { }
        else if (state.GameState.Type == Game.GameStateEnum.Lobby) { }
    }
    IEnumerator LoadGameScene()
    {
        Debug.Log("进入携程");
        GRoot.inst.RemoveChild(CheckPointPreviewWnd);
        GRoot.inst.RemoveChild(CheckPointWnd);

        GRoot.inst.AddChild(LoadGameWnd);
        AsyncOperation async = SceneModule.Instance.LoadSceneAtABAsync(1 + "_Base_MusicScene");
        async.allowSceneActivation = false;
        //while (!async.isDone)
        while (async.progress <= 0.88)
        {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(3);
        GRoot.inst.RemoveChild(LoadGameWnd);
        async.allowSceneActivation = true;
        async = null;
        //EnterOtherPanel(UIWndType.GameWnd);
        //GRoot.inst.AddChild(GameWnd);
    }
    void Update()
    {
        
    }
    public void EnterOtherPanel(UIWndType otherType)
    {
        
    }
    private void OnSC(object sender, GameEventArgs args)
    {
        StateChanged state = args as StateChanged;
        if (state.GameState.Type == Game.GameStateEnum.Finish)
        {
            GRoot.inst.RemoveChild(GameWnd);
            GRoot.inst.AddChild(FinishWnd);
        }
        else if (state.GameState.Type == Game.GameStateEnum.Lobby)
        {
            SceneManager.LoadScene("Lobby");
        }
        else if (state.GameState.Type == GameStateEnum.Game)
        {
            Debug.Log("进入游戏场景");
        }
        else if (state.GameState.Type == GameStateEnum.Login)
        {
            SceneManager.LoadScene("Login");
        }
        else
        {
            Debug.Log("暴打");
        }
    }
}
