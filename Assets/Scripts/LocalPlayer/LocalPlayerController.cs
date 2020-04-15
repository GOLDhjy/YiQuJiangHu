using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Modules;
using System;

namespace Game
{
    public enum JudgeType
    {
        Perfect,
        Great,
        Good,
        Miss,
        None
    }
    public class LocalPlayerController : MonoBehaviour
    {
        [Header("上下攻击特效")]
        public ParticleSystem AttackPar;
        public ParticleSystem AttackParUp;
        [Header("弹琴声音")]
        public AudioSource TanQin;
        [Header("下降系数")]
        public float DownX;

        private LocalPlayer player = null;
        private GameObject m_CurPlane = null;
        private Dictionary<JudgeType, Color> m_JudgeColor;
        private Dictionary<JudgeType, float> m_JudgeScore;

        public KeyNode m_CurNode;
        private JudgeType m_CurType;
        private float m_NodeSocre = 0;

        private Camera m_Camera;
        private Vector3 m_DiffVec;

        private GameLogic m_GameLogic = null;
        private float m_TimeFormStart = 0.0f;
        private int m_MonsterId = 0;
        private List<KeyNode> m_KeyNodes;
        private float m_Speed = 0;
        private float HitParticlePlayTime;

        private Queue<Monster> PushMonsterQ = new Queue<Monster>();

        private float JugeTime;
        private float HalfJudgeTime;
        //分数系数
        private float ScoreNum;
        
        public void Start()
        {
            InitJudgeData();

            m_Camera = Camera.main;
            m_GameLogic = GameObject.Find("StartRoot").GetComponent<GameLogic>();
            if (m_GameLogic == null)
            {

                Debug.LogError("This is none");
            }

            HalfJudgeTime = m_GameLogic.MusicInfo.DeltaTime;
            JugeTime = 2 * HalfJudgeTime;

            HitParticlePlayTime = 0;
            ScoreNum = 1;
            m_TimeFormStart = 0.0f;
            m_MonsterId = 0;
            m_KeyNodes = m_GameLogic.NodeList;
            m_Speed = m_GameLogic.Speed;
            player = LocalPlayer.Instance;
            TanQin = GetComponent<AudioSource>();
            TanQin.loop = false;

            MyEventSystem.Instance.Subscribe(HpChange.id, OnHpChange);
        }

       

        private void FixedUpdate()
        {
            //下降
            GetComponent<Rigidbody>().AddForce(Vector3.down * DownX);
        }
        private void Update()
        {
            

            if (LocalPlayer.Instance.anim == null)
            {
                Debug.LogError("This");
            }
            m_TimeFormStart = m_GameLogic.TimeFromStart;
            if (m_GameLogic.IsStart)
            {
                // m_TimeSpeed += Time.deltaTime;

                //Vector3 pos = m_Camera.transform.position;
                //pos.x += m_Speed * Time.deltaTime;
                ////pos.y = LocalPlayer.Instance.transform.position.y + m_DiffVec.y;
                //m_Camera.transform.position = pos;

                //Vector3 lp_pos = LocalPlayer.Instance.transform.position;
                //lp_pos.x = pos.x - m_DiffVec.x;
                //LocalPlayer.Instance.transform.position = lp_pos;

                //动态处理角色轨道
                if (m_MonsterId < m_KeyNodes.Count)
                {
                    KeyNode node = m_KeyNodes[m_MonsterId];
                    if (m_TimeFormStart > node.EndTime + HalfJudgeTime)
                    {
                       // Debug.Log("+++");
                        m_MonsterId++;
                    }
                }
                if (transform.position.y > (m_GameLogic.path2 + m_GameLogic.path3)/2)
                {
                    player.m_pathway = 3;
                }
                else if (transform.position.y > (m_GameLogic.path1 + m_GameLogic.path2) / 2)
                {
                    player.m_pathway = 2;
                }
                else if (transform.position.y > (m_GameLogic.path3 + m_GameLogic.path4) / 2)
                {
                    player.m_pathway = 4;
                }
                else
                {
                    player.m_pathway = 1;
                }
                //处理长按怪物队列
                if (PushMonsterQ.Count>0)
                {
                    var mon = PushMonsterQ.Dequeue();
                    Transform left = mon.transform.Find("Left");
                    var vec3 = left.transform.position;
                    vec3.x = m_TimeFormStart * m_Speed;

                    //就不继续向前进了,这个物体也不会入队列了，注意外部访问的时候，队列有可能已经为空了 ->现在改为都要入队列了
                    if (vec3.x> mon.EndTime*m_Speed)
                    {
                        PushMonsterQ.Enqueue(mon);
                    }
                    //继续前进
                    else
                    {
                        if (left == null)
                        {
                            Debug.LogError("erro");
                        }
                        else
                        {
                            //Transform tmp = mon.transform.Find("Left");
                            left.position = vec3;
                            //播放特效
                            if ((m_TimeFormStart- mon.StartTime)/(m_GameLogic.MusicInfo.DeltaTime*4) >= HitParticlePlayTime)
                            {
                                left.GetComponent<MuZhuangLogic>().OnAttacked();
                                HitParticlePlayTime++;
                            }
                            //if (mon.transform.GetComponent<ParticleSystem>()!=null && mon.transform.GetComponent<ParticleSystem>().isPlaying)
                            //{
                            //    mon.transform.GetComponent<ParticleSystem>().Stop();
                            //    //Debug.Log("---");
                            //}

                        }
                        PushMonsterQ.Enqueue(mon);
                    }
                }
                else
                {
                    HitParticlePlayTime = 0;
                }

            }
        }

        private void InitJudgeData()
        {
            m_JudgeColor = new Dictionary<JudgeType, Color>();
            m_JudgeColor.Add(JudgeType.Perfect, new Color(1, 1, 0));
            m_JudgeColor.Add(JudgeType.Great, new Color(1, 0.5f, 0));
            m_JudgeColor.Add(JudgeType.Good, new Color(0, 1, 0));
           // m_JudgeColor.Add(JudgeType.Bad, new Color(0, 0, 1));
            m_JudgeColor.Add(JudgeType.Miss, new Color(0.5f, 0.5f, 0.5f));


            m_JudgeScore = new Dictionary<JudgeType, float>();
            m_JudgeScore.Add(JudgeType.Perfect, 30);
            m_JudgeScore.Add(JudgeType.Great, 20);
            m_JudgeScore.Add(JudgeType.Good, 10);
            //m_JudgeScore.Add(JudgeType.Bad, 5);
            m_JudgeScore.Add(JudgeType.Miss, 0);
        }

        public void StarRun()
        {
            LocalPlayer.Instance.anim.SetBool("Run", true);
        }


        public void JumpUp(float velocity)
        {
            if (!player.anim.GetBool("Jump"))
            {
                player.anim.SetBool("Jump", true);
                player.rigidbody.velocity = new Vector3(0, velocity, 0);
                Debug.Log(player.anim.GetBool("Jump"));
            }           
        }
        public void ResetCurNode()
        {
            m_CurNode = null;
            m_CurType = JudgeType.None;
            m_NodeSocre = 0;
        }

        public void AttackStart(AttackPath path)
        {
            //播放特效
            TanQin.Play();
            if (!CheckCanAttack())
            {
                return;
            }
            if (path == AttackPath.UP)
            {            
                AttackParUp.Play();
                player.anim.SetBool("AttackUP", true);
            }
            else
            {
                AttackPar.Play();
                player.anim.SetBool("AttackDown", true);
            }
            if (m_MonsterId > m_KeyNodes.Count - 1)
            {
                MyEventSystem.Instance.Invoke(ScoreChange.id, this, new ScoreChange() { Score = player.m_score, AttackNum = 0 });
                return;
            }

            KeyNode node = m_KeyNodes[m_MonsterId];
            float len = Mathf.Abs(m_TimeFormStart - node.StartTime);
           // Debug.Log(node.StartTime + "  " + m_TimeFormStart);
            JudgeType type = Judge(len, node.PathWay, path);
            //Debug.Log(type);

            MyEventSystem.Instance.Invoke(AttackTypeChange.id, this, new AttackTypeChange() { judgeType = type });

            if (type == JudgeType.None)
            {
                ResetCurNode();
                if (player.MaxAttackCount < player.AttackCount)
                {
                    player.MaxAttackCount = player.AttackCount;
                }
                player.AttackCount = 0;
                MyEventSystem.Instance.Invoke(ScoreChange.id, this, new ScoreChange() { Score = player.m_score, AttackNum = 0 }) ;
                //Debug.Log("000");
                return;
            }
            if (type == JudgeType.Miss)
            {
                ResetCurNode();
                if (player.MaxAttackCount < player.AttackCount)
                {
                    player.MaxAttackCount = player.AttackCount;
                }
                player.AttackCount = 0;
                MyEventSystem.Instance.Invoke(ScoreChange.id, this, new ScoreChange() { Score = player.m_score, AttackNum = 0 }) ;
                return;
            }

            player.AttackCount++;
            m_CurNode = node;
           // Debug.Log("111");
            m_CurType = type;
            float score;
            m_JudgeScore.TryGetValue(type, out score);
            m_NodeSocre = score;
            //Debug.Log(ScoreNum + (player.AttackCount / 10)/10f);
            //Debug.Log(score);
            player.m_score +=  Convert.ToInt32( score*(ScoreNum+ (player.AttackCount / 10) / 10f));


            Monster mon = null;
            foreach (var item in m_GameLogic.monsters)
            {
                if (item.Index == m_CurNode.index)
                {
                    mon = item;
                    //mon.transform.gameObject.SetActive(false);
                    break;
                }
            }
            if (node.itemType != ItemType.LongBeatStart)
            {
                AttackFly(mon);
            }
            else
            {
                PushMon(mon);
            }

            MyEventSystem.Instance.Invoke(ScoreChange.id, this, new ScoreChange() { Score = player.m_score,AttackNum = player.AttackCount });


            Debug.Log("Score:"+player.m_score);
            Color color;
            m_JudgeColor.TryGetValue(m_CurType, out color);
            string strColor = "#" + ColorUtility.ToHtmlStringRGB(color);
            Debug.Log("<color=" + strColor + ">" + " 判定：" + m_CurType + "  index: " + m_CurNode.index + "</color>");

        }

 

        public void AttackFinish(AttackPath path)
        {
            if (!CheckCanAttack())
            {
                return;
            }
            if (path == AttackPath.UP)
            {
                player.anim.SetBool("AttackUP", false);
            }
            else
            {
                player.anim.SetBool("AttackDown", false);
            }
            if ( m_CurNode == null)
                return;
            if (m_MonsterId > m_KeyNodes.Count - 1)
            {
                MyEventSystem.Instance.Invoke(ScoreChange.id, this, new ScoreChange() { Score = player.m_score, AttackNum = 0 });
                return;
            }

            if (m_CurNode.itemType == ItemType.LongBeatStart)
            {
                KeyNode node = m_KeyNodes[m_MonsterId];
                //判断上一个长按是否已经结束，但是现在才放开按键
                if (m_CurNode.index == node.index)
                {
                    node = m_KeyNodes[m_MonsterId];
                    float len = Mathf.Abs(m_TimeFormStart - node.EndTime);
                    JudgeType type = Judge(len,node.PathWay,path);
                    MyEventSystem.Instance.Invoke(AttackTypeChange.id, this, new AttackTypeChange() { judgeType = type });

                    if (type == JudgeType.Miss)
                    {
                        ResetCurNode();
                        Monster mon = PushMonsterQ.Dequeue();
                        m_MonsterId++;
                        if (player.MaxAttackCount < player.AttackCount)
                        {
                            player.MaxAttackCount = player.AttackCount;
                        }
                        player.AttackCount = 0;
                        MyEventSystem.Instance.Invoke(ScoreChange.id, this, new ScoreChange() { Score = player.m_score, AttackNum = 0 });
                        return;
                    }

                    if (type == JudgeType.None)
                    {
                        ResetCurNode();
                        Monster mon = PushMonsterQ.Dequeue();
                        m_MonsterId++;

                        player.AttackCount = 0;
                        MyEventSystem.Instance.Invoke(ScoreChange.id, this, new ScoreChange() { Score = player.m_score, AttackNum = 0 });
                        return;
                    }

                    player.m_score += 60 * Convert.ToInt32((ScoreNum + (player.AttackCount / 10) / 10f));
                    m_CurType = type;

                    if (PushMonsterQ.Count>0)
                    {
                        Monster monster = PushMonsterQ.Dequeue();
                        AttackFly(monster);
                    }
                    
                    m_MonsterId++;
                    MyEventSystem.Instance.Invoke(ScoreChange.id, this, new ScoreChange() { Score = player.m_score,AttackNum = player.AttackCount });
                    Debug.Log("Score:" + player.m_score);
                    Color color;
                    m_JudgeColor.TryGetValue(m_CurType, out color);
                    string strColor = "#" + ColorUtility.ToHtmlStringRGB(color);
                    Debug.Log("<color=" + strColor + ">" + " 判定：" + m_CurType + "  index: " + m_CurNode.index + "</color>");
                }
                //如果已经过了就归零
                else
                {
                    player.AttackCount = 0;
                    MyEventSystem.Instance.Invoke(ScoreChange.id, this, new ScoreChange() { Score = player.m_score, AttackNum = 0 });
                }
            }
            PushMonsterQ.Clear();
            ResetCurNode();
        }
        private JudgeType Judge(float time_gap, int path_way,AttackPath attackpath)
        {
            JudgeType type;

            if (path_way != player.m_pathway + (int)attackpath)
            {
                return JudgeType.None;
            }
            if (time_gap > JugeTime)
                return JudgeType.Miss;
            else if (time_gap > 0.8 * JugeTime)//good
                type = JudgeType.Good;
            else if (time_gap > 0.5 * JugeTime)//great
                type = JudgeType.Great;
            else //preface
                type = JudgeType.Perfect;
            return type;
        }

        public bool CheckCanAttack()
        {
            if (player.DesLocalPos.x>transform.localPosition.x+0.5)
            {
                return false;
            }
            return true;
        }

        private void PushMon(Monster monster)
        {
            PushMonsterQ.Enqueue(monster);
        }
        public void AttackFly(Monster monster)
        {
            
            StartCoroutine(DMon(monster));
        }

        IEnumerator DMon(Monster m)
        {
            if (m.transform.GetComponent<Rigidbody>() == null)
            {

                m.transform.gameObject.SetActive(false);
            }
            else
            {

                MonsterLogic logic = m.transform.GetComponent<MonsterLogic>();
                logic.IsDied = true;
                logic.OnAttacked();
                yield return new WaitForSeconds( UnityEngine.Random.Range(0.8f, 1.5f));
                logic.OnDied();
            }
           
        }
        public void TanQinFunc()
        {
            TanQin.Play();
        }


        private void OnHpChange(object sender, GameEventArgs e)
        {
            if (LocalPlayer.Instance.m_hp <= 0)
            {
                SceneModule.Instance.LoadScene("Finish");
            }
        }
        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                player.anim.SetBool("Jump", true);
            }
        }
        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                player.anim.SetBool("Jump", false);
            }
            //Debug.Log(player.anim.GetBool("Jump"));
        }
        private void OnDestroy()
        {
            MyEventSystem.Instance.UnSubscribe(HpChange.id, OnHpChange);
        }

    }
}
