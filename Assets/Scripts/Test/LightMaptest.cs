using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMaptest : MonoBehaviour
{
    public GameObject Root;
    public GameObject game1;
    public GameObject game2;
    // Start is called before the first frame update
    void Start()
    {
        GameObject gameins1 = Instantiate<GameObject>(game1);
        GameObject gameins2 = Instantiate<GameObject>(game2);
        gameins1.transform.SetParent(Root.transform);
        gameins2.transform.SetParent(Root.transform);
        gameins1.transform.position = new Vector3(0, 0, 0);
        gameins2.transform.position = new Vector3(100, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
