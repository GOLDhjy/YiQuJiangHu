using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class FPSShow : MonoBehaviour
{
    private float m_LastUpdateShowTime = 0f;    //上一次更新帧率的时间;

    private float m_UpdateShowDeltaTime = 0.01f;//更新帧率的时间间隔;

    private int m_FrameUpdate = 0;//帧数;

    private float m_FPS = 0;
    void Awake()
    {
        //Application.targetFrameRate = 30;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_LastUpdateShowTime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        m_FrameUpdate++;
        if (Time.realtimeSinceStartup - m_LastUpdateShowTime >= m_UpdateShowDeltaTime)
        {
            m_FPS = Convert.ToInt16( m_FrameUpdate / (Time.realtimeSinceStartup - m_LastUpdateShowTime));
            m_FrameUpdate = 0;
            m_LastUpdateShowTime = Time.realtimeSinceStartup;
        }
    }
    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2, 0, 100, 100), "FPS: " + m_FPS);
    }
}
    





