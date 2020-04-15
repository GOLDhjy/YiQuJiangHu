using Game;
using Modules;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(AudioSource))]
public class StartGuide : MonoBehaviour
{
    [Header("轨道值")]
    public float path1 = -2;
    public float path2 = -0;
    public float path3 = 2;
    public float path4 = 4;
    [Header("Root")]
    public GameObject StarRoot;
    [Header("Monster")]
    public GameObject Monster;
    [Header("Envirment")]
    public GameObject Envirment;
    [Header("Speed")]
    public float Speed;
    [Header("Short")]
    public GameObject ShortMon;
    [Header("Long")]
    public GameObject LongMon;
    [Header("Boss")]
    public GameObject Boss;
    [Header("Trap1")]
    public GameObject Trap1;
    [Header("Trap2")]
    public GameObject Trap2;
    [Header("Trap3")]
    public GameObject Trap3;
    [Header("Err")]
    public GameObject Err;
    [Header("Music")]
    public AudioClip Music;
    //[Header("玩家设备偏移值"), Range(-0.5f, 0.5f)]
    //public float DeviceOffset;
    public Slider Offset;
    //反序列化
    public MusicInfo MusicInfo;
    internal AudioSource AS;
    //保存每个节点
    public List<KeyNode> NodeList;
    //怪物的队列
    public Queue<Monster> monsters;

    [HideInInspector]
    public float TimeFromStart;
    [HideInInspector]
    public bool IsStart;
    internal int IndexOfCreat;

    //提前多少秒加载怪物
    [Header("提前多少秒加载怪物")]
    public float PreTime;
    // Start is called before the first frame update


    void Start()
    {
        monsters = new Queue<Monster>();
        IsStart = false;
        TimeFromStart = 0;
        IndexOfCreat = 0;
        PreTime = 5;
       // DeviceOffset = 0;
        #region
        //#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        //        string path = Directory.GetCurrentDirectory() + "/Assets/RawRes/OutputData/" + Music.name + ".json";
        //        using (FileStream fs = new FileStream(path, FileMode.Open))
        //        {
        //            byte[] bytes = new byte[10240];
        //            try
        //            {
        //                while (fs.Read(bytes, 0, bytes.Length) != 0)
        //                {
        //                    json += Encoding.UTF8.GetString(bytes);
        //                }
        //                Debug.Log("导出完成");
        //            }
        //            catch (Exception e)
        //            {
        //                Debug.LogError(e);
        //            }
        //        }
        //#endif
        #endregion
        InitNodeList();
        SetASAndClip(Music);
        CheckTimeToCreat();

        //实例化Boss
        Instantiate<GameObject>(Boss);

    }
    void Update()
    {
        //if (TimeFromStart > 5)
        //{
        //    SceneModule.Instance.LoadScene(3);
        //}
        CheckTimeToCreat();
        EnvirmentMove();
        CheckForDelete();
        CheckForFinish();
    }
    void InitNodeList()
    {
        string json = string.Empty;
        AssetBundle AB;
        //AB = AssetBundle.LoadFromFile(Application.dataPath + "/StreamingAssets/musicoutdata.unity3d");
        AB = AssetBundleModule.Instance.LoadFormFile(GlobalConifg.Instance.musicoutdata);
        TextAsset TA = AB.LoadAsset<TextAsset>(Music.name + ".json");
        json = TA.text;

        MusicInfo = JsonConvert.DeserializeObject<MusicInfo>(json);
        NodeList = MusicInfo.keyNodes;

        //给每一个节点做偏移
        //#region
        //for (int i = 0; i < NodeList.Count; i++)
        //{
        //    NodeList[i].StartTime += DeviceOffset;
        //    NodeList[i].EndTime += DeviceOffset;
        //}
        //#endregion
    }
    void SetASAndClip(AudioClip clip)
    {
        AS = GetComponent<AudioSource>();
        if (AS != null)
        {
            AS.clip = Music;
        }
        else
        {
            Debug.LogError("AS为空");
        }
    }

    //根据怪物的时间来提前生成
    void CheckTimeToCreat()
    {
        while (IndexOfCreat < NodeList.Count)
        {
            if (NodeList[IndexOfCreat].StartTime < TimeFromStart + PreTime)
            {
                CreatMon(IndexOfCreat);
                IndexOfCreat++;
            }
            else
                break;
        }
    }
    void CreatMon(int i)
    {
        //跳过Boss的检测
        if (NodeList[i].itemType == ItemType.Boss)
        {
            return;
        }

        Vector3 vector3 = Vector3.zero;
        Monster monster;

        switch (NodeList[i].itemType)
        {
            case ItemType.ShortBeat:
                monster = MonsterFactory.Instance.GetMonster(NodeList[i], ShortMon);
                //g = Instantiate<GameObject>(ShortMon);
                if (monster == null)
                {
                    Debug.LogError("xxxxxx");
                }
                break;
            case ItemType.LongBeatStart:
                monster = MonsterFactory.Instance.GetMonster(NodeList[i], LongMon);
                //if (monster.transform.GetComponent<ParticleSystem>()!=null)
                //{
                //    monster.transform.GetComponent<ParticleSystem>().startSpeed = Speed;
                //    monster.transform.GetComponent<ParticleSystem>().startLifetime = monster.EndTime - monster.StartTime;
                //}


                //g = Instantiate<GameObject>(LongMon);
                break;
            case ItemType.Trap1:
                monster = MonsterFactory.Instance.GetMonster(NodeList[i], Trap1);
                //g = Instantiate<GameObject>(Trap);
                break;
            case ItemType.Trap2:
                monster = MonsterFactory.Instance.GetMonster(NodeList[i], Trap2);
                //g = Instantiate<GameObject>(Trap);
                break;
            case ItemType.Trap3:
                monster = MonsterFactory.Instance.GetMonster(NodeList[i], Trap3);
                //g = Instantiate<GameObject>(Trap);
                break;
            case ItemType.Boss:
                monster = MonsterFactory.Instance.GetMonster(NodeList[i], Trap1);
                // g = Instantiate<GameObject>(Boss);
                break;
            default:
                monster = MonsterFactory.Instance.GetMonster(NodeList[i], Err);
                //g = Instantiate<GameObject>(Err);
                break;
        }

        monster.transform.SetParent(Monster.transform);

        //下面是处理节点位置

        //特殊处理Long按
        if (monster.itemType == ItemType.LongBeatStart)
        {
            //monster.transform.Find("Left").localPosition = Vector3.zero;
            Vector3 tmppos = monster.transform.Find("Left").localPosition;
            tmppos.x = 0;
            monster.transform.Find("Left").localPosition = tmppos;

            tmppos = monster.transform.Find("Right").localPosition;
            tmppos.x = (monster.EndTime - monster.StartTime) * Speed;
            monster.transform.Find("Right").localPosition = tmppos;

        }


        if (NodeList[i].PathWay == 1)
        {
            vector3 = new Vector3(NodeList[i].StartTime * Speed, path1, 0);
        }
        else if (NodeList[i].PathWay == 2)
        {
            vector3 = new Vector3(NodeList[i].StartTime * Speed, path2, 0);
        }
        else if (NodeList[i].PathWay == 3)
        {
            vector3 = new Vector3(NodeList[i].StartTime * Speed, path3, 0);
        }
        else if (NodeList[i].PathWay == 4)
        {
            vector3 = new Vector3(NodeList[i].StartTime * Speed, path4, 0);
        }
        //这里做一个即时调整偏移
        vector3.x += GameSettingModule.Offset * Speed;
        monster.transform.localPosition = vector3;

        //以下处理旋转
        if (monster.itemType != ItemType.LongBeatStart && monster.itemType != ItemType.Trap2 && monster.itemType != ItemType.Trap3)
        {
            monster.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        //给金币和血包前移
        if (monster.itemType == ItemType.Trap2 && monster.itemType == ItemType.Trap3)
        {
            monster.transform.localPosition -= new Vector3(3.2f, 0, 0);
        }

        monsters.Enqueue(monster);
    }
    // Update is called once per frame




    void EnvirmentMove()
    {
        if (IsStart)
        {
            TimeFromStart += Time.deltaTime;
        }
    }
    //将怪物退回到对象池
    void CheckForDelete()
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            Monster monster = monsters.Dequeue();

            if (monster.transform.position.x <= LocalPlayer.Instance.transform.position.x - 10f && monster.EndTime < TimeFromStart - 3f)
            {
                MonsterPoolManager.Instance.GetMonsterPool(monster.itemType).ReturnToPoll(monster);
            }
            else
            {
                monsters.Enqueue(monster);
            }
        }
    }
    private void CheckForFinish()
    {
        if (TimeFromStart > Music.length + 2)
        {

            SceneModule.Instance.LoadScene("Finish");
        }
    }


    public void SetOffset()
    {
        float value = Offset.value;
        GameSettingModule.Offset = value;
        Debug.Log(this + "  GameSetingModule.Value Change"+value);
    }
    public void ConfirmReLoad()
    {
        
    }

    private void OnGUI()
    {
        #region
        //GUIStyle gs = new GUIStyle();
        //gs.normal.background = Texture2D.whiteTexture;
        //gs.normal.textColor = Color.green;

        //if (!IsStart)
        //{
        //    if (GUILayout.Button("开始", GUILayout.MinWidth(100), GUILayout.MinHeight(60)))
        //    {
        //        StarRoot.transform.position = Vector3.zero;
        //        AS.time = 0;
        //        IndexOfCreat = 0;
        //        AS.Play();
        //        IsStart = true;
        //        LocalPlayerController.StarRun();
        //    }
        //}
        #endregion
        //if (GUILayout.Button("返回", GUILayout.MinWidth(100), GUILayout.MinHeight(60)))
        //{
        //    SceneModule.Instance.LoadScene(1);
        //}
        //if (GUILayout.Button("Start", GUILayout.MinWidth(100), GUILayout.MinHeight(60)))
        //{
        //    StartGameRun();
        //}
        //风格化和高低端按钮
        #region
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("低配", GUILayout.MinWidth(100), GUILayout.MinHeight(60)))
        {
            GameSettingModule.Instance.SetQualityLevel(GameSettingModule.QualityEnum.Low);
        }
        if (GUILayout.Button("高配", GUILayout.MinWidth(100), GUILayout.MinHeight(60)))
        {
            GameSettingModule.Instance.SetQualityLevel(GameSettingModule.QualityEnum.High);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("描边风格变化", GUILayout.MinWidth(100), GUILayout.MinHeight(60)))
        {
            GameSettingModule.Instance.SetEdgeDetection();
        }
        GUILayout.EndHorizontal();
        #endregion
    }

    public void StartGameRun()
    {
        StarRoot.transform.position = Vector3.zero;
        AS.time = 0;
        //IndexOfCreat = 0;
        AS.Play();
        IsStart = true;
        LocalPlayerController player = LocalPlayer.Instance.GetPlayController();
        if (player != null) player.StarRun();
    }
    public void PauseGameRun()
    {
        AS.Pause();
        IsStart = false;
    }
    public void ResumeGameRun()
    {
        AS.UnPause();
        IsStart = true;
    }
    public void StopGameRun()
    {
        IsStart = false;

    }
}
