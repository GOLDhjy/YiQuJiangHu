using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Modules;

public class JianQiLogic : MonoBehaviour
{
    Transform PlayerTransform;
    bool IsAttacked;
    //GameLogic gameLogic;
    //public GameObject JianQi;
    // Start is called before the first frame update
    void Start()
    {
        PlayerTransform = LocalPlayer.Instance.transform;
        //gameLogic = GameObject.Find("StartRoot").GetComponent<GameLogic>();
    }
    private void OnEnable()
    {
        IsAttacked = false;
    }
    // Update is called once per frame
    void Update()
    {
    if (transform.position.x <= PlayerTransform.position.x)
    {
	    if (!IsAttacked)
	        {
	            LocalPlayer.Instance.m_hp -= 3;
	            MyEventSystem.Instance.Invoke(HpChange.id, this, new HpChange() { Hp = LocalPlayer.Instance.m_hp });
	            IsAttacked = true;
	        }
    }
    }
}
