using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Modules;

public class JinBiLogic : MonoBehaviour
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

            LocalPlayer.Instance.m_score += 500;
            MyEventSystem.Instance.Invoke(ScoreChange.id, this, new ScoreChange() { Score = LocalPlayer.Instance.m_score, AttackNum = LocalPlayer.Instance.AttackCount});
            
            this.gameObject.SetActive(false);
        }
    }

}
