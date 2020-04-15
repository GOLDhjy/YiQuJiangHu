using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
public class BossLogic : MonoBehaviour
{
    Animator animator;
    Transform PlayerTransform;
    Transform CameraTransform;
    GameLogic gameLogic;
    float Speed;
    float TimeLen;
    float DT;
    Vector3 OrinPosition;
    //这里需要在初始化位置时往前面多移动backsec*speed，在目标位置会丢下一个东西。这里需要在GameLogic的里面设置位置时特殊一下
    [Header("提前多少秒开跑")]
    public float BackSec;
    [Header("剑气物体")]
    public GameObject TeXiao;
    [Header("拉开BOSS时间")]
    public float MoveBOSSTime;

    private Queue<KeyNode> BossNodes = new Queue<KeyNode>();
    private Queue<Monster> BossTeXiao = new Queue<Monster>();
    private KeyNode firstnode;
    private KeyNode lastnode;
    private bool HasBoss;
    private float TimeFromStart;
    private Queue<Monster> monsters;
    
    //[Header("提前多少秒播放攻击动画")]
    //public int AttackSec;
    // Start is called before the first frame update
    void Start()
    {
        MoveBOSSTime = 5;
        gameObject.name = "BOSS";
        animator = GetComponent<Animator>();
        PlayerTransform = LocalPlayer.Instance.transform;
        CameraTransform = Camera.main.transform;
        gameLogic = GameObject.Find("StartRoot").GetComponent<GameLogic>();
        Speed = gameLogic.Speed;
        monsters = gameLogic.monsters;

        foreach (var item in gameLogic.NodeList)
        {

            if (item.itemType == ItemType.Boss)
            {
                BossNodes.Enqueue(item);
            }
        }
        KeyNode[] nodes = BossNodes.ToArray();
        if (nodes.Length>0)
        {
            firstnode = nodes[0];
            lastnode = nodes[nodes.Length - 1];
            //设置boss位置
            SetPositionToPre();
            HasBoss = true;
        }
        else
        {
            transform.position = new Vector3(-100, 0, 0);
        }
        //transform.position = new Vector3((firstnode.StartTime - BackSec) * Speed, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (HasBoss)
        {
            //同步时间
            TimeFromStart = gameLogic.TimeFromStart;
            //提前开跑
            if (TimeFromStart > firstnode.StartTime - BackSec)
            {
                //animator.SetBool("Run", true);
                transform.localPosition = new Vector3(transform.localPosition.x + Speed * Time.deltaTime, transform.localPosition.y, transform.localPosition.z);
                transform.rotation = Quaternion.Euler(0, -90, 0);
                //TimeLen -= Time.deltaTime;
            }
            
            //消失
            if (TimeFromStart > lastnode.StartTime + BackSec)
            {
                if (transform.localPosition.y<=20)
                {
                    Vector3 vector = transform.localPosition;
                    vector.y += Time.deltaTime * Speed;
                    transform.localPosition = vector;
                } 
            }
            //发射剑气
            if (BossNodes.Count > 0)
            {
                KeyNode node = BossNodes.Peek();
                //暂时消失
                if (node.StartTime - BackSec - MoveBOSSTime>= TimeFromStart)
                {
                    Vector3 vector = transform.localPosition;
                    vector.y += Time.deltaTime * Speed;
                    if (vector.y>= gameLogic.path4)
                    {
                        vector.y = gameLogic.path4;
                    }
                    transform.localPosition = vector;
                }
                else
                {
                    Vector3 vector = transform.localPosition;
                    vector.y -= Time.deltaTime * Speed;
                    if (vector.y <= gameLogic.path1)
                    {
                        vector.y = gameLogic.path1;
                    }
                    transform.localPosition = vector;
                }

                //提前多少秒发出剑气
                if (TimeFromStart >= node.StartTime - BackSec)
                {
                    


                    animator.SetTrigger("Attack");
                    //KeyNode node = BossNodes.Dequeue();
                    Vector3 vector3 = Vector3.zero;

                    Monster monster = MonsterFactory.Instance.GetMonster(node, TeXiao);
                    //GameObject TeXiao = Instantiate(this.TeXiao);
                    //剑气的位置会往前移动BackSec*speed的距离
                    if (node.PathWay == 1)
                    {
                        vector3 = new Vector3((node.StartTime + BackSec) * Speed, gameLogic.path1 + 1.5f, 0);
                    }
                    else if (node.PathWay == 2)
                    {
                        vector3 = new Vector3((node.StartTime + BackSec) * Speed, gameLogic.path2 + 1.5f, 0);
                    }
                    else if (node.PathWay == 3)
                    {
                        vector3 = new Vector3((node.StartTime + BackSec) * Speed, gameLogic.path3 + 1.5f, 0);
                    }
                    else if (node.PathWay == 4)
                    {
                        vector3 = new Vector3((node.StartTime + BackSec) * Speed, gameLogic.path4 + 1.5f, 0);
                    }
                    monster.transform.position = vector3;
                    monsters.Enqueue(monster);
                    //加入特效队列，特效如果需要前移就加入。
                    BossTeXiao.Enqueue(monster);
                    //在这里释放剑气
                    animator.SetTrigger("Attack");
                    BossNodes.Dequeue();

                    //Debug.Log(BossNodes.Count);
                }
            }
            //特效前移
            if (BossTeXiao.Count > 0)
            {
                for (int i = 0; i < BossTeXiao.Count; i++)
                {

                    Monster monster = BossTeXiao.Dequeue();
                    if (monster.transform.gameObject.activeSelf)
                    {
                        monster.transform.position = new Vector3(monster.transform.position.x - Speed * Time.deltaTime, monster.transform.position.y, monster.transform.position.z);
                        BossTeXiao.Enqueue(monster);
                    }

                }


            }


            //if (OrinPosition.x == CameraTransform.position.x)
            //{
            //    animator.SetTrigger("Attack");
            //}
            //if (transform.localPosition.x - CameraTransform.position.x < Speed * BackSec)
            //{
            //    //放特效，然后消失
            //    if (TimeLen<=0)
            //    {


            //        transform.localPosition = new Vector3(0, 0, 0);
            //    }
            //    animator.SetBool("Run",true);
            //    transform.localPosition = transform.position + new Vector3(transform.localPosition.x + Speed * Time.deltaTime, transform.localPosition.y, transform.localPosition.z);
            //    transform.rotation = Quaternion.Euler(0, 90, 0);
            //    TimeLen -= Time.deltaTime;
            //}
        }
    }
    
    public void BeginAttack(float starttime,float endtime,float deltatime)
    {
        TimeLen = endtime - starttime;
        DT = deltatime;
        OrinPosition = transform.localPosition;
    }
    public void SetPositionToPre()
    {
        Vector3 vector3 = Vector3.zero;
        if (firstnode.PathWay == 1)
        {
            vector3 = new Vector3((firstnode.StartTime+BackSec) * Speed, gameLogic.path1, 0);
        }
        else if (firstnode.PathWay == 2)
        {
            vector3 = new Vector3((firstnode.StartTime - BackSec) * Speed, gameLogic.path2, 0);
        }
        else if (firstnode.PathWay == 3)
        {
            vector3 = new Vector3((firstnode.StartTime - BackSec) * Speed, gameLogic.path3, 0);
        }
        else if (firstnode.PathWay == 4)
        {
            vector3 = new Vector3((firstnode.StartTime - BackSec) * Speed, gameLogic.path4, 0);
        }
        transform.position = vector3;
        transform.rotation = Quaternion.Euler(0, -90, 0);
    }
}
