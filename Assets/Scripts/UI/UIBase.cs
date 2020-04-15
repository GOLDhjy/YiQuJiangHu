using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIBase : MonoBehaviour
{
    
    public Dictionary<UIWndType, UIBasePanel> UIPanelDict;
    public UIWndType currentPanel;
    public UIBase()
    {
        UIPanelDict = new Dictionary<UIWndType, UIBasePanel>();
    }
    //解决UI切换场景报错无法清空的问题
    public void ClearDict()
    {
        foreach (var item in UIPanelDict)
        {
            item.Value.Dispose();
        }
        UIPanelDict.Clear();
    }
    //父类声明 子类实例化
    public void LoginUIManager()
    {
        if (UIPanelDict.Count != 0)
        {
            ClearDict();
        }
        UIPanelDict.Add(UIWndType.LoginWnd, new LoginPanel("LoginWnd", UIWndType.LoginWnd, this));
        UIPanelDict[UIWndType.LoginWnd].Show();
        Debug.Log("Login优化部分");
    }
    public void LobbyUIManger()
    {
        if (UIPanelDict.Count != 0)
        {
            ClearDict();
        }
        //每个场景的初始化窗口
        UIPanelDict.Add(UIWndType.MainWnd, new LobbyPanel("MainWnd", UIWndType.MainWnd, this));
        UIPanelDict[UIWndType.MainWnd].Show();
        Debug.Log("Lobby优化部分");
    }
    public void GameUIManager()
    {
        if (UIPanelDict.Count != 0)
        {
            ClearDict();
        }
        UIPanelDict.Add(UIWndType.GameWnd, new GamePanel("GameWnd", UIWndType.GameWnd, this));
        UIPanelDict[UIWndType.GameWnd].Show();
        Debug.Log("Game优化部分");
    }
    public void FinishUIManager()
    {
        if (UIPanelDict.Count != 0)
        {
            ClearDict();
        }
        UIPanelDict.Add(UIWndType.ReturnWnd, new FinishPanel("FinishWnd", UIWndType.ReturnWnd, this));
        UIPanelDict[UIWndType.ReturnWnd].Show();
        Debug.Log("Finish优化部分");
    }
}
