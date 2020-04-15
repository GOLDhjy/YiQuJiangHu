using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Modules;
public class XueBaoLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            AudioSource source = Camera.main.GetComponent<AudioSource>();
            source.clip = GetComponent<AudioSource>().clip;
            source.Play();

            LocalPlayer.Instance.m_hp += 50;
            if (LocalPlayer.Instance.m_hp > 100)
            {
                LocalPlayer.Instance.m_hp = 100;
            }
            MyEventSystem.Instance.Invoke(HpChange.id, this, new HpChange() { Hp = LocalPlayer.Instance.m_hp });
            
            
            this.gameObject.SetActive(false);
        }
    }
}
