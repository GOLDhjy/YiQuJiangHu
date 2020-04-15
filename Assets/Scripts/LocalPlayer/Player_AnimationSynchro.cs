using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AnimationSynchro : MonoBehaviour
{
    private Animator Anim;
    public float N_Time;
    // Start is called before the first frame update
    void Start()
    {
        Anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        N_Time = Anim.GetCurrentAnimatorStateInfo(1).normalizedTime;
        Anim.SetFloat("LegTime",N_Time);
    }
}
