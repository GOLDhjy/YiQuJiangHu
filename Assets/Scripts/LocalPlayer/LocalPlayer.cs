using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules;
using UnityEngine;

namespace Game
{
    public enum AttackPath
    {
        DOWM,
        UP
    }
    public class LocalPlayer : Singleton<LocalPlayer>
    {
        public long m_exp = 0;
        public int m_level = 0;

        public float m_hp = 100;
        public float Max_hp = 100;
        public float m_score = 0;
        public int m_pathway = 1;
        public int AttackCount = 0;
        public int MaxAttackCount = 0;
        public Transform transform = null;
        public Rigidbody rigidbody = null;
        public Animator anim = null;
        public GameObject m_PlayerObj = null;
        LocalPlayerController localPlayerController = null;
        public Vector3 DesLocalPos;
        public LocalPlayer()
        {
         
        }
        public LocalPlayerController GetPlayController()
        {
            if (localPlayerController == null)
            {
                localPlayerController =  m_PlayerObj.GetComponent<LocalPlayerController>();
            }
            if (localPlayerController == null)
            {
                Debug.LogError("场景中Player为空，无LocalPlayerController");
            }
            return localPlayerController;
        }
        public void InitPlayer()
        {
            if (m_PlayerObj != null)
            {

                transform = m_PlayerObj.GetComponent<Transform>();
                rigidbody = m_PlayerObj.GetComponent<Rigidbody>();
                anim = m_PlayerObj.GetComponent<Animator>();
                var tran = GameObject.Find("PlayerAndCamera").GetComponent<Transform>();
                if (tran == null)
                {
                    Debug.LogError("找不到PlayerAndCamera");
                }
                else
                {
                    m_PlayerObj.transform.SetParent(tran);
                    transform.position = new Vector3(-3, -2, -0.3f);
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                    DesLocalPos = transform.localPosition;
                }
            }
            else
            {
                Debug.Log("场景不存在Player");
            }
            //初始化数据
            m_hp = 100;
            AttackCount = 0;
            m_pathway = 1;
            m_score = 0;
            MaxAttackCount = 0;



        }
        public void ClearPlayer()
        {
            GameObject.Destroy(m_PlayerObj);
            m_PlayerObj = null;
            transform = null;
            rigidbody = null;
            anim = null;
        }
        public void TanQin()
        {
            GetPlayController().TanQinFunc();
        }


    }
}
