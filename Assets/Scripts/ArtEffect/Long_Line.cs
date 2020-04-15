using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Long_Line : MonoBehaviour
{
    private LineRenderer LRD;
    public Transform RightT;
    public float LineHeightNum;
    // Start is called before the first frame update
    void Start()
    {
        LRD = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        LRD.SetPosition(1, transform.position + Vector3.up * LineHeightNum);
        LRD.SetPosition(0, RightT.position + Vector3.up * LineHeightNum);

    }
}
