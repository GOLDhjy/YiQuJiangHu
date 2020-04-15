using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using UnityEngine.SceneManagement;
using Game;

public class LobbyApplication : SceneSwitch
{
    //游戏窗口在Lobby中显示
    public GButton upJump;
    public GButton downSquat;
    public GButton upAttack;
    public GButton downAttack;

    public GButton gameStart;
    public GProgressBar blood;
    public float bloodValue;
    public GTextField text;
    override protected void Init()
    {
        blood = GameWnd.GetChild("n6").asProgress;

        upJump = GameWnd.GetChild("n7").asButton;
        downSquat = GameWnd.GetChild("n8").asButton;
        upAttack = GameWnd.GetChild("n10").asButton;
        downAttack = GameWnd.GetChild("n9").asButton;
        gameStart = GameWnd.GetChild("n29").asButton;

        text = GameWnd.GetChild("n17").asTextField;

        upJump.onClick.Add(() => { LocalPlayer.Instance.GetPlayController().JumpUp(15); });
        //downSquat.onClick.Add(() => { LocalPlayer.Instance.GetPlayController().JumpDown(8); });

        upAttack.onTouchBegin.Add(() => { LocalPlayer.Instance.GetPlayController().AttackStart( AttackPath.UP); });
        upAttack.onTouchEnd.Add(() => { LocalPlayer.Instance.GetPlayController().AttackFinish( AttackPath.UP); });

        downAttack.onTouchBegin.Add(() => { LocalPlayer.Instance.GetPlayController().AttackStart( AttackPath.DOWM); });
        downAttack.onTouchEnd.Add(() => { LocalPlayer.Instance.GetPlayController().AttackFinish( AttackPath.DOWM); } ) ;

        
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        //进入game场景后动态加载生命值
        CheckPointPreviewWnd.GetChild("n4").onClick.Add(() =>
        {
            //if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1))
            //{
            blood.value = 1;
            blood.TweenValue(100, 3);
            //}
        });
        //游戏正式开始按钮
        gameStart.onClick.Add(() =>
        {
            //黄金翼查看一下下面代码，引用会报错，暂时用OnGUI按钮开始游戏 gl在90行
            //gl.StarRoot.transform.position = Vector3.zero;
            //gl.AS.time = 0;
            //gl.IndexOfCreat = 0;
            //gl.AS.Play();
            //gl.IsStart = true;
            //LocalPlayerController.StarRun();
            count3++;
            if (gl == null)
            {
                gl = GameObject.Find("StartRoot").GetComponent<GameLogic>();
            }
            else
                gl.StartGameRun();
            if (count3%3 == 0)
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
        //这里需要通过player获取到分数值
    }
    int count2 = 1;
    int count3 = 1;
    //GameLogin
    GameLogic gl;
    LocalPlayerController player;
    public float timer = 0;
    void Update()
    {
        //CheckPointPreviewWnd.GetChild("n4").onClick.Add(() =>

        //if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(2))
        //{
        //    timer += Time.deltaTime;
        //    if (timer > 3f)
        //    {
        //        GRoot.inst.AddChild(GameWnd);
        //        GRoot.inst.RemoveChild(LoadGameWnd);
        //    }
        //    else
        //    {
        //        GRoot.inst.AddChild(LoadGameWnd);
        //        //GRoot.inst.RemoveChild(LoadGameWnd);
        //    }
        //}

        //text.text = LocalPlayer.Instance.m_score.ToString();
    }

}

