using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class PlayerAndCamera : MonoBehaviour
{
    public Vector3 CameraPos;
    GameLogic gameLogic;
    Camera Maincamera;
    Vector3 PlayerDesLocalPos;
    Transform PlayerobjectTran;


    public GameObject CheckPoint;
    // Start is called before the first frame update
    void Start()
    {
        Maincamera = Camera.main;
        CameraPos = new Vector3(0, 0, -6);
        Maincamera.transform.position = CameraPos;
        gameLogic = GameObject.Find("StartRoot").GetComponent<GameLogic>();
        CheckPoint = transform.Find("JudgePoint").gameObject;
        if (gameLogic == null)
        {
            Debug.LogError("未找到场景中StartRoot");
        }
        if (CheckPoint!=null)
        {
            CheckPoint.transform.SetParent(PlayerobjectTran.transform);
            //CheckPoint.transform.position = new Vector3(-1.96f, -0.92f, -0.3f);
            CheckPoint.transform.position = new Vector3(0f, 0f, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameLogic.IsStart)
        {
            Vector3 pos = transform.position;
            pos.x += gameLogic.Speed * Time.deltaTime;
            transform.position = pos;

            if (PlayerDesLocalPos.x>LocalPlayer.Instance.transform.localPosition.x)
            {
                float jiasu = Mathf.Lerp(LocalPlayer.Instance.transform.localPosition.x, PlayerDesLocalPos.x, 0.5f * Time.deltaTime);
                Vector3 vector3 = new Vector3(jiasu, LocalPlayer.Instance.transform.localPosition.y, LocalPlayer.Instance.transform.localPosition.z);
                LocalPlayer.Instance.transform.localPosition = vector3;

                //CheckPoint.SetActive(false);
            }
            else
            {
                //if (!CheckPoint.activeSelf)
                //{
                //    CheckPoint.SetActive(true);
                //}
            }
                
        }
    }
    public void SetPlayerLocalPos(Vector3 vector)
    {
        PlayerDesLocalPos = vector;
        PlayerobjectTran = transform.Find("Player");
    }
}
