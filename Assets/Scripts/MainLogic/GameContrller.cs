using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Modules;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace Game
{
    /// <summary>
    /// 状态机控制器
    /// </summary>
    public class GameContrller : Singleton<GameContrller> 
    {
        Dictionary<GameStateEnum,IGameBaseState> StateList = new Dictionary<GameStateEnum, IGameBaseState>();
        IGameBaseState m_CurrentState;

        public IGameBaseState CurrentState { get => m_CurrentState; set => m_CurrentState = value; }

        //在这里添加状态机的状态
        public GameContrller()
        {
            AddState(new GameLogin());
            AddState(new GameLobby());
            AddState(new GameRun());
            AddState(new GameFinish());

            
        }
        public void AddState<T>(T gamestate)where T: IGameBaseState
        {
            if (StateList.ContainsKey(gamestate.Type))
            {
                Debug.Log("已经添加此状态,不用重复添加");
                return;
                
            }
            StateList.Add(gamestate.Type,gamestate);
            Debug.Log("场景添加状态成功:" + gamestate.Type.ToString());
        }
        public void RemoveState(GameStateEnum gameState)
        {
            if (StateList.ContainsKey(gameState))
            {
                StateList.Remove(gameState);
            }
            else
            {
                Debug.Log("删除失败，不包含状态:"+gameState);
            }
        }
        public bool ChangeState(GameStateEnum gameState)
        {
            if (CheckCondition())
            {
                if (!StateList.ContainsKey(gameState))
                {
                    Debug.LogError("Has Not Such State  state " + gameState);
                    return false;
                }
                if (CurrentState != null && CurrentState.Type == gameState)
                {
                    Debug.Log("与当前状态相同，无须改变");
                    return false;
                }
                if (CurrentState != null)
                {
                    //退出状态
                    CurrentState.EndState();
                }
                CurrentState = StateList[gameState];
                Debug.Log("修改游戏State为:" + gameState.ToString());
                //修改后执行一遍
                CurrentState.EntryState();
                CurrentState.OnState();
                return true;
            }
            else
                return false;
        }
        public bool ChangeStateForce(GameStateEnum gameState)
        {
            if (CheckCondition())
            {
                if (!StateList.ContainsKey(gameState))
                {
                    Debug.LogError("Has Not Such State  state " + gameState);
                    return false;
                }
                if (CurrentState != null)
                {
                    CurrentState.EndState();
                }
                CurrentState = StateList[gameState];
                Debug.Log("修改游戏State为:" + gameState.ToString());
                CurrentState.EntryState();
                CurrentState.OnState();
                return true;
            }
            else
                return false;
        }
        //加转换条件
        public bool CheckCondition()
        {

            return true;
        }

        //初始化游戏所需要的东西
        public void Initialize()
        {
            //ChangeState(GameStateEnum.Login);
            //给加载场景增加回调事件，不用手动换状态。
            SceneManager.sceneLoaded += ChangeSelfState();


        }

        //根据场景转换回调自动转换场景状态
        private UnityAction<Scene, LoadSceneMode> ChangeSelfState()
        {
            
            
            return new UnityAction<Scene, LoadSceneMode>(SceneChangeEvent);
        }

        private void SceneChangeEvent(Scene arg0, LoadSceneMode arg1)
        {
            IGameBaseState state;
            
            if (arg1 == LoadSceneMode.Additive)
            {
                return;
            }
            if (arg0.name == "Login")
            {
                state = new GameLogin();
                GameContrller.Instance.ChangeState(GameStateEnum.Login);
                
            }
            else if (arg0.name == "Lobby")
            {
                
                state = new GameLobby();
                GameContrller.Instance.ChangeState(GameStateEnum.Lobby);
            }
            else if (arg0.name == "Finish")
            {
                //清除怪物的对象池
                MonsterPoolManager.Instance.ClearALLPool();
                state = new GameFinish();
                GameContrller.Instance.ChangeState(GameStateEnum.Finish);
            }
            else
            {
                state = new GameRun();
                GameContrller.Instance.ChangeState(GameStateEnum.Game);
            }
            Modules.MyEventSystem.Instance.Invoke(StateChanged.Id, this, new StateChanged { GameState = state });
        }

        public void Start()
        {

        }
        
        
        public void Update()
        {
            CurrentState.OnState();
        }
        public void LateUpdate()
        {

        }
        public void FixUpdate()
        {

        }
        public void Destroy()
        {

        }
    }
}
