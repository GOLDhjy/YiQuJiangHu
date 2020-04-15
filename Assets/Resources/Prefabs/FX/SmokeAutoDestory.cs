using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeAutoDestory : MonoBehaviour
{
    ParticleSystem particleSystem;
    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Play();
        Destroy(gameObject, 2);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
