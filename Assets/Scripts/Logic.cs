using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

//public class KeyNode
//{
//    public bool Do;
//    public float Time;
//    public int PathWay;
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
public class Logic : MonoBehaviour
{
    public float BPM;
    public float CurrentTime;
    public float OffsetTime;
    public int bar;
    public float BarTime;
    public float len;
    public float DeltaTime;
    public AudioClip Music;

    public Dropdown[] List1;
    public Dropdown[] List2;
    public Dropdown[] List3;
    public Dropdown[] List4;
    GUIStyle gs;


    public GameObject Root;
    public Dropdown dropdown;
    public Button button;

    Dropdown tmp;
    Button tmpb;

    private void Start()
    {
        gs = new GUIStyle();
        List<string> ls = AddOptions();
        dropdown.ClearOptions();
        dropdown.AddOptions(ls);
        dropdown.captionText.text = "None";
        //Debug.Log(dropdown.captionText.text);
        for (int i = 0; i < 1028; i++)
        {
            tmp = Instantiate<Dropdown>(dropdown);
            tmp.transform.SetParent(Root.transform);
            tmp.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            Vector3 vt = tmp.gameObject.GetComponent<RectTransform>().localPosition;
            vt.z = 0;
            tmp.gameObject.GetComponent<RectTransform>().localPosition = vt;
            //Debug.Log(tmp.gameObject.GetComponent<RectTransform>().localScale);
            //tmp.
        }
        
       
    }
    private void Update()
    {
       // Debug.Log(tmp.captionText.text);
    }
    private List<string> AddOptions()
    {
        List<string> ls = new List<string>();
        //ls.Add(ItemType.None.ToString());
        //ls.Add(ItemType.ShortBeat.ToString());
        //ls.Add(ItemType.LongBeat.ToString());
        //ls.Add(ItemType.Trap1.ToString());
        //ls.Add(ItemType.Trap2.ToString());
        //ls.Add(ItemType.Trap3.ToString());
        //ls.Add(ItemType.Boss.ToString());
        return ls;
    }

    public void Refresh()
    {
        len = Music.length;
        BarTime = 240 / BPM;
        bar = Convert.ToInt32(len / BarTime);
        DeltaTime = BarTime / 32;
        CurrentTime = OffsetTime;

        float TimeCounter = OffsetTime;
        List1 = new Dropdown[bar * 32];
        List2 = new Dropdown[bar * 32];
        List3 = new Dropdown[bar * 32];
        List4 = new Dropdown[bar * 32];
        for (int i = 0; i < bar * 32; i++)
        {
            List1[i] = Instantiate<Dropdown>(dropdown);
            List2[i] = Instantiate<Dropdown>(dropdown);
            List3[i] = Instantiate<Dropdown>(dropdown);
            List4[i] = Instantiate<Dropdown>(dropdown);
            //List1[i].Time = TimeCounter;
            //List2[i].Time = TimeCounter;
            //List3[i].Time = TimeCounter;
            //List4[i].Time = TimeCounter;
            //TimeCounter += DeltaTime;
        }
    }
}
