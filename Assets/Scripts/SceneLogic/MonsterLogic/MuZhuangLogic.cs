using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Modules;
public class MuZhuangLogic : MonoBehaviour
{
    public ParticleSystem particle;
    private void OnEnable()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnAttacked()
    {
        particle.Play();
        LocalPlayer.Instance.TanQin();        
        //Instantiate<GameObject>(particle, transform.position, transform.rotation);
        //Handheld.Vibrate();
    }
}
