using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Modules;
[RequireComponent(typeof(Animator)), RequireComponent(typeof(Rigidbody))]
public class MonsterLogic : MonoBehaviour
{
    Animator animator;
    Transform PlayerTransform;
    GameLogic gameLogic;
    float Speed;
    bool IsAttacke;
    public bool IsDied;
    [Header("提前多少秒放攻击动画")]
    public float Sec;
    public ParticleSystem DeathSmoke;
    public ParticleSystem HitPar;
    [SerializeField]
    private Vector3 FlyToMax = new Vector3(1, 1, -1);
    [SerializeField]
    private Vector3 FlyToMin = new Vector3(1, 1, 0.4f);
    [SerializeField]
    private float FlyForce = 10f;
    //public Rigidbody[] ragdollparts;
    // Start is called before the first frame update
    private void OnEnable()
    {
        IsAttacke = false;
        IsDied = false;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        PlayerTransform = LocalPlayer.Instance.transform;
        gameLogic = GameObject.Find("StartRoot").GetComponent<GameLogic>();
        Speed = gameLogic.Speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x-PlayerTransform.position.x<Speed* Sec && transform.position.x> PlayerTransform.position.x)
        {
            if (!IsAttacke && !IsDied)
            {
                animator.SetTrigger("Attack");
                LocalPlayer.Instance.m_hp -= 3.5f;
                MyEventSystem.Instance.Invoke(HpChange.id, this, new HpChange() { Hp = LocalPlayer.Instance.m_hp });
                IsAttacke = true;
            }
        }


    }
    //被攻击的时候的表现
    public void OnAttacked()
    {
        IsDied = true;

        //播放特效
        HitPar.Play();
       
        //Instantiate(HitPar, transform.position, transform.rotation);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        Vector3 NV = new Vector3(Random.Range(FlyToMin.x, FlyToMax.x), Random.Range(FlyToMin.y, FlyToMax.y), Random.Range(FlyToMin.z, FlyToMax.z));
        GetComponent<Rigidbody>().AddRelativeForce(NV * Random.Range(FlyForce + 3, FlyForce - 3), ForceMode.Impulse);

        animator.SetTrigger("Die");
        


    }
    public void OnDied()
    {
        //播放特效
        //Instantiate(DeathSmoke, transform.position, Quaternion.Euler(0, 90, 0));
        //Debug.Log("beidale");
        DeathSmoke.Play();
        //DeathSmoke.GetComponent<ParticleSystem>().Play();
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        StartCoroutine(DieCorutine());
    }
    IEnumerator DieCorutine()
    {
        yield return new WaitForSeconds(0.35f);
        gameObject.SetActive(false);
    }
    //开始执行布料娃娃
   // public void StartRagDoll()
    //{
        
    //    foreach (Rigidbody rig in ragdollparts)
    //    {
    //        rig.isKinematic = false;
    //    }
    //    animator.enabled = false;
   // }
    
    private void OnBecameVisible()
    {
        animator.SetBool("IsVis", true);
    }
}
