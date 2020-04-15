using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using Modules;
//public class KeyNode
//{
//    public int PathWay;
//    public float Time;
//    public int Bar;
//    public int Beat;
//    public int Part;
//    public int index;
//    public ItemType itemType;
//}
//public enum ItemType
//{
//    None,
//    ShortBeat,
//    LongBeat,
//    Trap1,
//    Trap2,
//    Trap3,
//    Boss,
//}
public class MusicPlugin : EditorWindow
{
    public static float BPM;//BPM是多少
    public float CurrentTime;//当前时间
    public static float OffsetTime;//第一拍开始的偏移时间
    public int bar;//一共多少个bar
    public float BarTime;//1个bar的时长
    public float len;//歌曲长度
    public float DeltaTime;//1个part的时长
    public static AudioClip Music;
    public AudioSource Asource;//播放器在哪
    public GameObject Root;
    //预览的物体
    public GameObject Prefab;
    public GameObject Prefab2;
    public float Speed;//预览时场景移动的速度

    float CurrentValue = 0;
    bool Jump = false;
    List<KeyNode> FinalList = new List<KeyNode>();//导出时保存节点信息的数组
    int Index = 0;
    int BarShow;
    int BeatShow;
    int pages;//页数

    
    Rect rect;
    Vector2 ViewPos;
    //4个轨道
    public KeyNode[] List1;
    public KeyNode[] List2;
    public KeyNode[] List3;
    public KeyNode[] List4;
    private List<KeyNode> ImportNodeList;//导入时存的数组
    private List<GameObject> GBList;//预览时存怪物的
    bool IsInit = false;
    GUIStyle gs = new GUIStyle();
    GUIStyle WhiteStyle = new GUIStyle();
    GUIStyle Style1 = new GUIStyle();
    GUIStyle Style2 = new GUIStyle();
    GUIStyle Style3 = new GUIStyle();
    GUIStyle BlackStyle = new GUIStyle();

    [MenuItem("BeatMaker/MusicPlugin")]
    public static void ShowWindow()
    {
        EditorWindow.CreateInstance<MusicPlugin>().ShowUtility();
    }
    private void OnEnable()
    {
        ImportNodeList = new List<KeyNode>();
        GBList = new List<GameObject>();
        gs.fontSize = 16;
        string RedTexturePath =@"Assets\RawRes\APPIcon\red.png";
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(RedTexturePath);
        Style2.normal.background = texture;
        Root = GameObject.Find("StartRoot");
        Speed = 5;
       // BPM = 120;
        Prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/RawRes/Prefabs/Sphere.prefab");
        Prefab2 = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/RawRes/Prefabs/Cylinder.prefab");
        
        // Init();
    }
    private void OnDisable()
    {
        DestroyGB();
        //if (Root != null)
        //{
        //    Root.transform.position = Vector3.zero;
        //}
        if (Asource!=null && Asource.clip!=null)
        {
            Asource.Stop();
        }
        
    }
    //输入歌曲数据后刷新界面，初始化这首歌的节点
    public void Init()
    {
        len = Music.length;
        BarTime =  240 / BPM;
        bar = Convert.ToInt32(len / BarTime);
        DeltaTime = BarTime/32;
        CurrentTime = OffsetTime;
        CurrentValue = 0;//把滑动条归零
        FinalList.Clear();

        //临时时间变量，用来给4轨道初始化，初始化时间
        float TimeCounter = OffsetTime;
        List1 = new KeyNode[bar * 32];
        List2 = new KeyNode[bar * 32];
        List3 = new KeyNode[bar * 32];
        List4 = new KeyNode[bar * 32];
        for (int i = 0; i < bar*32; i++)
        {
            List1[i] = new KeyNode();
            List2[i] = new KeyNode();
            List3[i] = new KeyNode();
            List4[i] = new KeyNode();

            List1[i].StartTime = TimeCounter;
            List2[i].StartTime = TimeCounter;
            List3[i].StartTime = TimeCounter;
            List4[i].StartTime = TimeCounter;
            List1[i].EndTime = TimeCounter;
            List2[i].EndTime = TimeCounter;
            List3[i].EndTime = TimeCounter;
            List4[i].EndTime = TimeCounter;
            TimeCounter  += DeltaTime;
        }
        IsInit = true;
        //初始化纹理颜色
        BlackStyle.normal.background = Texture2D.blackTexture;
        Style1.normal.background = BlackStyle.normal.background;
       
        WhiteStyle.normal.background = EditorGUIUtility.whiteTexture;
        pages = 1;//页数
        //设置音频
        Asource = Root.GetComponent<AudioSource>();
        Asource.clip = Music;
    }
    Vector3 ViewRootPos;
    private void Update()
    {
        //更新预览中的root位置
        if (Asource != null && Asource.isPlaying)
        {
            CurrentValue = Asource.time;
            CurrentTime = Asource.time;
            ViewRootPos.x = -CurrentValue * Speed;
            Root.transform.position = ViewRootPos;
        }
        //让红条跟着当前时间移动
        if (Asource != null && Asource.isPlaying)
        {
            int TmpBar = ((int)((CurrentTime-OffsetTime) / BarTime));
            pages = (TmpBar / 8) + 1;
            if (rect.yMax != 0)
            {
                ViewPos.y = rect.yMax / 8 * ((TmpBar % 8));
            }
            else
            {
                ViewPos.y = 5000 / 8 * ((TmpBar % 8));
            }
            Jump = false;
        }
        
    }
    //插件的界面
    public void OnGUI()
    {
        BarShow = 1;
        BeatShow = 1;
        //if (GUILayout.Button("关闭"))
        //{
        //    this.Close();
        //}
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("生成轨道"))
        {
            Init();
        }
        if (GUILayout.Button("导入"))
        {
            Import();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        OffsetTime = EditorGUILayout.FloatField("Offset", OffsetTime);
        BPM = EditorGUILayout.FloatField("BPM", BPM);
        Speed = EditorGUILayout.FloatField("Speed", Speed);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        Music = (AudioClip)EditorGUILayout.ObjectField("歌曲",Music, typeof(AudioClip),false);
        Root = (GameObject)EditorGUILayout.ObjectField("场景Root",Root, typeof(GameObject),true);
        Prefab = (GameObject)EditorGUILayout.ObjectField("预览物体",Prefab, typeof(GameObject),true);
        Prefab2 = (GameObject)EditorGUILayout.ObjectField("长按预览物体", Prefab2, typeof(GameObject),false);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        
        //CurrentTime = EditorGUILayout.FloatField("当前时间", CurrentTime);
        EditorGUILayout.LabelField("当前时间:" + CurrentTime);
        EditorGUILayout.LabelField("歌曲长度:" + len);
        EditorGUILayout.EndHorizontal();
        CurrentValue = EditorGUILayout.Slider(CurrentValue, 0, len);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("播放/暂停"))
        {
            if (Asource.isPlaying)
            {
                Asource.Pause();
            }
            else
            {
                Root.transform.position = new Vector3(-CurrentValue * Speed, 0, 0);
                if (Asource == null)
                {
                    EditorUtility.DisplayDialog("警告", "找不到声音源", "确定");
                }
                Asource.time = CurrentValue;
                Asource.Play();
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("上一页"))
        {
            if (pages>1)
            {
                pages--;
            }
        }
        if (GUILayout.Button("下一页"))
        {
            if (pages*256< bar * 32)
            {
                pages++;
            }
        }
        if (GUILayout.Button("跳转到当前播放"))
        {
            Jump = true;
            ////找到第几bar
            //int TmpBar = ((int)(CurrentTime / BarTime));
            //pages = (TmpBar / 8) + 1;
            //if (rect.yMax!=0)
            //{
            //    ViewPos.y = rect.yMax / 8 * ((TmpBar % 8));
            //}
            //else
            //{
            //    ViewPos.y = 4400f / 8 * ((TmpBar % 8));
            //}

        }
        EditorGUILayout.EndHorizontal();

        

        Rect tittlerect = EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("轨道:", gs);
        EditorGUILayout.LabelField("轨道1", gs);
        EditorGUILayout.LabelField("轨道2", gs);
        EditorGUILayout.LabelField("轨道3", gs);
        EditorGUILayout.LabelField("轨道4", gs);
        EditorGUILayout.EndHorizontal();

        //开始显示轨道
        ViewPos = EditorGUILayout.BeginScrollView(ViewPos);
        rect = EditorGUILayout.BeginVertical();
        
        //else
        //{
        //    ViewPos.y = 4400f / 8 * ((TmpBar % 8));
        //}
        if (IsInit)
        {
            //每一页显示256*4个
            for (int i = (pages-1)*256 ;i<pages*256 && i < bar * 32; i += 1)
            {
                if ((i) % 8 == 0 || i == 0)
                {
                    //EditorGUILayout.Space();
                    EditorGUILayout.LabelField(BarShow+(pages-1)*8 + ":" + (((i + 1) / 8) % 4 + 1), EditorStyles.whiteLargeLabel);
                    BeatShow++;
                }
                Rect tmprect = EditorGUILayout.BeginHorizontal();
                //EditorGUILayout.LabelField();
                //GUILayout.Button("Time: " + List1[i].Time,GUILayout.MaxWidth(200), GUILayout.MaxHeight(20));
                //GUILayout.Button("Time: " + List2[i].Time,GUILayout.MaxWidth(200),GUILayout.MaxHeight(20));
                //GUILayout.Button("Time: " + List3[i].Time,GUILayout.MaxWidth(200), GUILayout.MaxHeight(20));
                //GUILayout.Button("Time: " + List4[i].Time,GUILayout.MaxWidth(200), GUILayout.MaxHeight(20));
                EditorGUILayout.LabelField(((i % 8) + 1).ToString());
                //if (List1[i].itemType == ItemType.LongBeat)
                //{
                //    List1[i].itemType = (ItemType)EditorGUILayout.EnumPopup( List1[i].itemType,GUILayout.MaxWidth(200), GUILayout.MinWidth(200));
                //    List2[i].itemType = (ItemType)EditorGUILayout.EnumPopup( List2[i].itemType,GUILayout.MaxWidth(200), GUILayout.MinWidth(200));
                //    List3[i].itemType = (ItemType)EditorGUILayout.EnumPopup( List3[i].itemType,GUILayout.MaxWidth(200), GUILayout.MinWidth(200));
                //    List4[i].itemType = (ItemType)EditorGUILayout.EnumPopup( List4[i].itemType, GUILayout.MaxWidth(200), GUILayout.MinWidth(200));
                //}
                //else if (List1[i].itemType == ItemType.ShortBeat)
                //{
                //    List1[i].itemType = (ItemType)EditorGUILayout.EnumPopup( List1[i].itemType, GUILayout.MaxWidth(200), GUILayout.MinWidth(200));
                //    List2[i].itemType = (ItemType)EditorGUILayout.EnumPopup( List2[i].itemType, GUILayout.MaxWidth(200), GUILayout.MinWidth(200));
                //    List3[i].itemType = (ItemType)EditorGUILayout.EnumPopup( List3[i].itemType, GUILayout.MaxWidth(200), GUILayout.MinWidth(200));
                //    List4[i].itemType = (ItemType)EditorGUILayout.EnumPopup( List4[i].itemType, GUILayout.MaxWidth(200), GUILayout.MinWidth(200));
                //}
                //else if (List1[i].itemType == ItemType.Boss)
                //{
                //    List1[i].itemType = (ItemType)EditorGUILayout.EnumPopup( List1[i].itemType, GUILayout.MaxWidth(200), GUILayout.MinWidth(200));
                //    List2[i].itemType = (ItemType)EditorGUILayout.EnumPopup( List2[i].itemType, GUILayout.MaxWidth(200), GUILayout.MinWidth(200));
                //    List3[i].itemType = (ItemType)EditorGUILayout.EnumPopup( List3[i].itemType, GUILayout.MaxWidth(200), GUILayout.MinWidth(200));
                //    List4[i].itemType = (ItemType)EditorGUILayout.EnumPopup( List4[i].itemType, GUILayout.MaxWidth(200), GUILayout.MinWidth(200));
                //}
                //else

                //让红色条跟着移动
                if ((Convert.ToInt32((CurrentTime-OffsetTime)/DeltaTime)) == i)
                {
                    List1[i].itemType = (ItemType)EditorGUILayout.EnumPopup(List1[i].itemType, Style2, GUILayout.MaxWidth(200), GUILayout.MinWidth(200));
                    List2[i].itemType = (ItemType)EditorGUILayout.EnumPopup(List2[i].itemType, Style2, GUILayout.MaxWidth(200), GUILayout.MinWidth(200));
                    List3[i].itemType = (ItemType)EditorGUILayout.EnumPopup(List3[i].itemType, Style2, GUILayout.MaxWidth(200), GUILayout.MinWidth(200));
                    List4[i].itemType = (ItemType)EditorGUILayout.EnumPopup(List4[i].itemType, Style2, GUILayout.MaxWidth(200), GUILayout.MinWidth(200));
                }
                else
                {
                    List1[i].itemType = (ItemType)EditorGUILayout.EnumPopup(List1[i].itemType, GUILayout.MaxWidth(200), GUILayout.MinWidth(200));
                    List2[i].itemType = (ItemType)EditorGUILayout.EnumPopup(List2[i].itemType, GUILayout.MaxWidth(200), GUILayout.MinWidth(200));
                    List3[i].itemType = (ItemType)EditorGUILayout.EnumPopup(List3[i].itemType, GUILayout.MaxWidth(200), GUILayout.MinWidth(200));
                    List4[i].itemType = (ItemType)EditorGUILayout.EnumPopup(List4[i].itemType, GUILayout.MaxWidth(200), GUILayout.MinWidth(200));
                }
                    

                // List1[i].Do = EditorGUILayout.Toggle("是否放置", List1[i].Do);
                //if (List1[i].Do)
                //{
                //    EditorGUILayout.TextField("Name", List1[i].Name);
                //}
                //EditorGUILayout.LabelField("Time:" + List2[i].Time);
                // EditorGUILayout.LabelField("Time:" + List3[i].Time);
                //EditorGUILayout.LabelField("Time:" + List4[i].Time);
                EditorGUILayout.EndHorizontal();
                if (BeatShow == 5)
                {
                    BarShow++;
                    BeatShow = 1;
                }
            }
        }
        

        
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("导出"))
        {
            Exp();
        }
        if (GUILayout.Button("预览"))
        {
            string json = string.Empty;
            string path = Directory.GetCurrentDirectory() + "/Assets/RawRes/OutputData/" + Music.name + ".json";
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                byte[] bytes = new byte[10240];
                try
                {
                    while (fs.Read(bytes, 0, bytes.Length) != 0)
                    {
                        json += Encoding.UTF8.GetString(bytes);
                        Array.Clear(bytes, 0, bytes.Length);
                    }
                    Debug.Log("导出完成");
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            
            MusicInfo mi = JsonConvert.DeserializeObject<MusicInfo>(json);
            ImportNodeList = mi.keyNodes;
            Refresh();
        } 
        EditorGUILayout.EndHorizontal();

    }
    //在场景界面生成预览物体
    void Refresh()
    {
        DestroyGB();
        for (int i = 0; i < ImportNodeList.Count; i++)
        {
            Vector3 vector3 = Vector3.zero;
            if (ImportNodeList[i].PathWay == 1)
            {
                vector3 = new Vector3(ImportNodeList[i].StartTime * Speed, -2, 0);
            }
            else if (ImportNodeList[i].PathWay == 2)
            {
                vector3 = new Vector3(ImportNodeList[i].StartTime * Speed, 0, 0);
            }
            else if (ImportNodeList[i].PathWay == 3)
            {
                vector3 = new Vector3(ImportNodeList[i].StartTime * Speed, 2, 0);
            }
            else if (ImportNodeList[i].PathWay == 4)
            {
                vector3 = new Vector3(ImportNodeList[i].StartTime * Speed, 4, 0);
            }
            if (ImportNodeList[i].itemType == ItemType.LongBeatStart)
            {
                GameObject g2 = Instantiate<GameObject>(Prefab2);
                GameObject g3 = Instantiate<GameObject>(Prefab2);
                g2.transform.SetParent(Root.transform);
                g3.transform.SetParent(Root.transform);
                g2.transform.position = vector3;
                GBList.Add(g2);
                vector3.x = ImportNodeList[i].EndTime * Speed;
                g3.transform.position = vector3;
                GBList.Add(g3);
            }
            else
            {
                GameObject g = Instantiate<GameObject>(Prefab);
                g.transform.SetParent(Root.transform);
                g.transform.position = vector3;
                GBList.Add(g);
            }
        }
    }
    //关闭时销毁之前预览生成的物体
    void DestroyGB()
    {
        if (Root != null)
        {
            Root.transform.position = Vector3.zero;
        }
        for (int i = 0; i < GBList.Count; i++)
        {
            GameObject.DestroyImmediate(GBList[i]);
        }
    }
    //导入
    void Import()
    {
        string json = string.Empty;
        string path = Directory.GetCurrentDirectory() + "/Assets/RawRes/OutputData/" + Music.name + ".json";
        using (FileStream fs = new FileStream(path, FileMode.Open))
        {
            byte[] bytes = new byte[10240];
            try
            {
                while (fs.Read(bytes, 0, bytes.Length) !=0)
                {
                    json += Encoding.UTF8.GetString(bytes);
                    Array.Clear(bytes, 0, bytes.Length);
                }
                Debug.Log("导入完成");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        //反序列化
        MusicInfo mi  = JsonConvert.DeserializeObject<MusicInfo>(json);
        ImportNodeList = mi.keyNodes;
        //DeltaTime = mi.BarTime;
        //Speed = mi.Speed;
        //len = mi.Len;
        //OffsetTime = mi.OffsetTime;
        //BarTime = mi.BarTime;
        Init();
        for (int i = 0; i < ImportNodeList.Count; i++)
        {
            switch(ImportNodeList[i].PathWay)
            {
                case 1:
                    DealImportLongBeat(ref List1, ImportNodeList[i]);
                    ImportNodeList[i].StartTime = ImportNodeList[i].index  * DeltaTime + OffsetTime;
                    ImportNodeList[i].EndTime = ImportNodeList[i].index  * DeltaTime + OffsetTime;
                    List1[ImportNodeList[i].index] = ImportNodeList[i];
                    break;
                case 2:
                    DealImportLongBeat(ref List2, ImportNodeList[i]);
                    ImportNodeList[i].StartTime = ImportNodeList[i].index  * DeltaTime + OffsetTime;
                    ImportNodeList[i].EndTime = (ImportNodeList[i].index + 1) * DeltaTime + OffsetTime;
                    List2[ImportNodeList[i].index] = ImportNodeList[i];
                    break;
                case 3:
                    DealImportLongBeat(ref List3, ImportNodeList[i]);
                    ImportNodeList[i].StartTime = ImportNodeList[i].index  * DeltaTime + OffsetTime;
                    ImportNodeList[i].EndTime = ImportNodeList[i].index * DeltaTime + OffsetTime;
                    List3[ImportNodeList[i].index] = ImportNodeList[i];
                    break;
                case 4:
                    DealImportLongBeat(ref List4, ImportNodeList[i]);
                    ImportNodeList[i].StartTime = ImportNodeList[i].index  * DeltaTime + OffsetTime;
                    ImportNodeList[i].EndTime = ImportNodeList[i].index  * DeltaTime + OffsetTime;
                    List4[ImportNodeList[i].index] = ImportNodeList[i];
                    break;
            }
        }
        IsInit = true;

    }
    //导入时的对longbeat的处理
    public void DealImportLongBeat(ref KeyNode[] TMP, KeyNode key)
    {
        if (key.itemType == ItemType.LongBeatStart)
        {
            int num = (int)((key.EndTime - key.StartTime) / DeltaTime);
            KeyNode LongBeatEnd = new KeyNode();
            LongBeatEnd.StartTime = key.EndTime;
            LongBeatEnd.EndTime = key.EndTime;
            LongBeatEnd.PathWay = key.PathWay;
            //LongBeat.Part = key.Part + num;
            LongBeatEnd.itemType = ItemType.LongBeatEnd;
            LongBeatEnd.index = key.index + num;
            TMP[key.index + num] = LongBeatEnd;
        }
    }


    //导出
    void Exp()
    {
        KeyNode Tmpkeynode = new KeyNode();
        Tmpkeynode.HaveNext = false;
        for (int i = 0; i < bar * 32; i++)
        {
            //对每个轨道做longbeat检查，然后存入final
            if (List1[i].itemType != ItemType.None)
            {
                if ( DealLongBeat(ref Tmpkeynode, List1, i))
                {
                    List1[i].PathWay = 1;
                    List1[i].Bar = (i / 32) + 1;
                    List1[i].Beat = (i % 32) / 8 + 1;
                    List1[i].Part = (i % 32) % 8 + 1;
                    List1[i].index = i;
                    FinalList.Add(List1[i]);
                } 
            }
            if (List2[i].itemType != ItemType.None)
            {
                if (DealLongBeat(ref Tmpkeynode , List2, i))
                {
                    //DealLongBeat(ref Tmpkeynode, i);
                    List2[i].PathWay = 2;
                    List2[i].Bar = (i / 32) + 1;
                    List2[i].Beat = (i % 32) / 8 + 1;
                    List2[i].Part = (i % 32) % 8 + 1;
                    List2[i].index = i;
                    FinalList.Add(List2[i]);
                }
            }
            if (List3[i].itemType != ItemType.None)
            {
                if (DealLongBeat(ref Tmpkeynode, List3, i))
                {
                    //DealLongBeat(ref Tmpkeynode, i);
                    List3[i].PathWay = 3;
                    List3[i].Bar = (i / 32) + 1;
                    List3[i].Beat = (i % 32) / 8 + 1;
                    List3[i].Part = (i % 32) % 8 + 1;
                    List3[i].index = i;
                    FinalList.Add(List3[i]);
                }
            }
            if (List4[i].itemType != ItemType.None)
            {
                if (DealLongBeat(ref Tmpkeynode, List4, i))
                {
                    //DealLongBeat(ref Tmpkeynode, i);
                    List4[i].PathWay = 4;
                    List4[i].Bar = (i / 32) + 1;
                    List4[i].Beat = (i % 32) / 8 + 1;
                    List4[i].Part = (i % 32) % 8 + 1;
                    List4[i].index = i;
                    FinalList.Add(List4[i]);
                }
            }

        }
        if (Tmpkeynode.HaveNext == true)
        {
            if (EditorUtility.DisplayDialog("错误", "LongBeatStart与LongBeatEnd必须配合使用", "确定", "清除"))
            {
                Debug.LogError("LongBeatStart与End必须配合使用");

                return;
            }
            else
            {
                FinalList.Clear();
            }
        }
        else
        {
            //序列化歌曲信息
            MusicInfo music = new MusicInfo();
            music.keyNodes = FinalList;
            music.DeltaTime = DeltaTime;
            music.Speed = Speed;
            music.Len = len;
            music.OffsetTime = OffsetTime;
            music.BarTime = BarTime;
            string json = JsonConvert.SerializeObject(music);
            string path = Directory.GetCurrentDirectory() + "/" + "Assets/RawRes/OutputData/" + Music.name + ".json";
            if (Directory.Exists(path))
            {
                Directory.Delete(path);
            } 
            FileModule.Instance.WriteText(path, json);
            AssetDatabase.Refresh();
        }
        FinalList.Clear();
    }
    /// <summary>
    /// 利用存储一个临时变量来判断是不是start后面会有end
    /// </summary>
    /// <param name="Tmp"></param>
    /// <param name="ListX"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool DealLongBeat(ref KeyNode Tmp,KeyNode[] ListX ,int index)
    {
        if (ListX[index].itemType == ItemType.LongBeatStart)
        {
            if (Tmp.HaveNext == true)
            {
                if (EditorUtility.DisplayDialog("错误", "请仔细检查，前一个LongBeatStart与没有对应的End，必须配合使用", "确定","取消"))
                {
                    Debug.LogError("LongBeatStart与End必须配合使用");
                    return false;
                }
            }
            else
            {
                Tmp = ListX[index];
                Tmp.HaveNext = true;
                
            }
            return true;
        }
        else if (ListX[index].itemType == ItemType.LongBeatEnd)
        {
            if (Tmp != null && Tmp.HaveNext == false)
            {
                if (EditorUtility.DisplayDialog("错误", "请仔细检查，前面必须有LongBeatStart才能有End，不能重复两个End必须配合使用", "OK"))
                {
                    Debug.LogError("请仔细检查，LongBeatStart与End必须配合使用");
                    return false;
                }
            }
            else
            {
                //long类型的end是在这里设置时间
                Tmp.EndTime = index * DeltaTime+OffsetTime;
                Tmp.HaveNext = false;
                
            }
            return false;

        }
        else
            return true;
    }

}