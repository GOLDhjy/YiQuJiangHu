using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTest : MonoBehaviour
{
    Animator animator;
    GameLogic gameLogic;
    // Start is called before the first frame update
    void Start()
    {
        gameLogic = GameObject.Find("StartRoot").GetComponent<GameLogic>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (gameLogic.IsStart )
        //{
        //    if (!animator.GetBool("Run"))
        //    {
        //        animator.SetBool("Run", true);
        //    }
        //    transform.position = new Vector3(transform.position.x + Time.deltaTime * gameLogic.Speed, transform.position.y, transform.position.z);
            
        //}
        //if (Input.GetMouseButtonDown(0))
        //{
        //    animator.SetTrigger("Attack");

        //}
        //{

        //}
    }
}
