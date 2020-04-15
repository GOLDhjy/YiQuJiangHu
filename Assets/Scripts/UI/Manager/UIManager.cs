using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using Modules;
using Game;

public class UIManager : MonoBehaviour
{
    [HideInInspector]
    public bool isInGame;
    [HideInInspector]
    public bool isInLobby;

    [HideInInspector]
    public enum GameState
    {
        isInLogin,
        isInLobby,
        isInGame,
        isInFinish
    }

    private UIBase currentUIManager;
    private UIBasePanel basePanel;

    public GComponent ProgressWnd;
    public GProgressBar progressBar;

    //场景BGM以及场景音效
    [HideInInspector]
    public UIAudioSourceManager audioSourceManager;
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public AudioClip buttonSound;
    public GameObject LoadingBar;

    private static UIManager _instance;
    public static UIManager Instance { get => _instance; set => _instance = value; }
    private void Awake()
    {
        //UI初始化
        DontDestroyOnLoad(this);
        Instance = this;

        //GameObject LoadingBar = Resources.Load<GameObject>("Prefabs/LoadingBar");
        LoadingBar = Instantiate<GameObject>(LoadingBar);
        LoadingBar.SetActive(false);
        DontDestroyOnLoad(LoadingBar);


        UIPackage.AddPackage("FGUI/Package1");
        //默认按钮音效
        //UIConfig.buttonSound =  buttonSound;
        GRoot.inst.SetContentScaleFactor(1600, 900, UIContentScaler.ScreenMatchMode.MatchWidthOrHeight);
        MyEventSystem.Instance.Subscribe(StateChanged.Id, ChangeScene);
        //ProgressWnd= UIPackage.CreateObject("Package1", "IntroduceWnd").asCom;
        //progressBar = ProgressWnd.GetChild("n11").asProgress;
        if (currentUIManager == null)
        {
            currentUIManager = new UIBase();
            currentUIManager.LoginUIManager();
        }
        if (audioSourceManager == null)
        {
            audioSourceManager = new UIAudioSourceManager(this);
        }
    }
    protected void ChangeScene(object sender, GameEventArgs args)
    {
        StateChanged state = args as StateChanged;
        if (state.GameState.Type == Game.GameStateEnum.Finish)
        {
            currentUIManager.FinishUIManager();
        }
        else if (state.GameState.Type == Game.GameStateEnum.Lobby)
        {
            currentUIManager.LobbyUIManger();
        }
        else if (state.GameState.Type == GameStateEnum.Game)
        {
            currentUIManager.GameUIManager();
            
        }
        else if (state.GameState.Type == GameStateEnum.Login)
        {
            currentUIManager.LoginUIManager();
        }
        else
        {
            Debug.Log("黄金翼出来挨打");
        }
    }

    /*
        写给黄金翼的 
        依靠游戏状态切换不足以完成异步加载，只能判定单一场景
        依靠加载对应场景再进行添加四个场景未免太没必要要麻烦
        有其他解决办法吗
    */
   
    //GameEventArgs arg;
    //StateChanged state;
    //bool isCreateGameWnd=false;
    //float timer = 0;
    void Start()
    {
        //state = arg as StateChanged;
    }
    void Update()
    {
     
    }
}
