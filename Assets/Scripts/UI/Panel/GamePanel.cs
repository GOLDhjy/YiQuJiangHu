using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FairyGUI;
using Game;
using Modules;
using System;

public class GamePanel : UIBasePanel
{
    //这里是GamePanel
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

    public GamePanel(string packageName, UIWndType uIWndType, UIBase uIBaseManager) : base(packageName, uIWndType, uIBaseManager)
    {
    }
    protected override void OnAwake()
    {
        Debug.Log("进入GameAwake");
        GameWnd = UIPackage.CreateObject("Package1", "GameWnd").asCom;

        veryGood = contentPane.GetChild("n67").asImage;
        good = contentPane.GetChild("n73").asImage;
        ok = contentPane.GetChild("n72").asImage;
        bad = contentPane.GetChild("n69").asImage;
        veryBad = contentPane.GetChild("n70").asImage;

        blood = contentPane.GetChild("n6").asProgress;
        upJump = contentPane.GetChild("n7").asButton;
        upAttack = contentPane.GetChild("n10").asButton;
        downAttack = contentPane.GetChild("n9").asButton;
        gameStart = contentPane.GetChild("n29").asButton;
        text = contentPane.GetChild("n63").asTextField;
        jumpNum = contentPane.GetChild("n62").asTextField;


        transVeryGood = contentPane.GetTransition("t0");
        transGood = contentPane.GetTransition("t4");
        transOk = contentPane.GetTransition("t5");
        transBad = contentPane.GetTransition("t6");
        transVeryBad = contentPane.GetTransition("t7");

        MyEventSystem.Instance.Subscribe(ScoreChange.id, OnShowScore);
        MyEventSystem.Instance.Subscribe(AttackTypeChange.id, OnAttackTypeChange);
        MyEventSystem.Instance.Subscribe(HpChange.id, OnHpChange);
        blood.value = 100;
        Debug.Log("awake hp" + blood.value);
    }

   
    protected override void OnInitPanel()
    {  
        Debug.Log("进入GameInit");
        //veryGood.visible = false;
        //good.visible = false;
        //ok.visible = false;
        //bad.visible = false;
        //veryBad.visible = false;

        upJump.onClick.Add(() => { LocalPlayer.Instance.GetPlayController().JumpUp(15);});
        //downSquat.onClick.Add(() => { LocalPlayer.Instance.GetPlayController().JumpDown(); });

        upAttack.onTouchBegin.Add(() => { LocalPlayer.Instance.GetPlayController().AttackStart(AttackPath.UP); });
        upAttack.onTouchEnd.Add(() => { LocalPlayer.Instance.GetPlayController().AttackFinish(AttackPath.UP); });

        downAttack.onTouchBegin.Add(() => { LocalPlayer.Instance.GetPlayController().AttackStart(AttackPath.DOWM); });
        downAttack.onTouchEnd.Add(() => { LocalPlayer.Instance.GetPlayController().AttackFinish(AttackPath.DOWM); });
        gl = GameObject.Find("StartRoot").GetComponent<GameLogic>();

        //游戏正式开始按钮
        gameStart.onClick.Add(() =>
        {
            Debug.Log("游戏开始咯");
         
            count3++;
            if (gl != null)
            {
                gl.StartGameRun();
            }
            else
            {
                Debug.LogError("未找到Root");
            }
                
            if (count3<2)
            {
                //gameStart.visible = false;
                blood.value = 1;
                blood.TweenValue(100, 3);
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
                Debug.Log("游戏暂停");
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
                Debug.Log("游戏暂停恢复");
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
        contentPane.GetChild("n19").onClick.Add(() =>
        {
            GRoot.inst.RemoveChild(contentPane);
            SceneManager.LoadScene(1);
        });
    }
    private void OnHpChange(object sender, GameEventArgs e)
    {
        HpChange hp = e as HpChange;
        //bloodValue = (hp.Hp/LocalPlayer.Instance.Max_hp)/100f;
        blood.value = hp.Hp;
        //Debug.Log("blood" +bloodValue);
        //Debug.Log(hp.Hp);
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

    int count2 = 1;
    int count3 = 0;
    //GameLogin
    GameLogic gl;
    LocalPlayerController player;
    public float timer = 0;
    public void OnShutDown()
    {
        MyEventSystem.Instance.UnSubscribe(ScoreChange.id, OnShowScore);
        MyEventSystem.Instance.UnSubscribe(AttackTypeChange.id, OnAttackTypeChange);
        MyEventSystem.Instance.UnSubscribe(HpChange.id, OnHpChange);
    }
}
