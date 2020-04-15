using UnityEngine;
using System.Collections;

public class WaterWaveEffect : PostEffectBase {
 
    public int count = 0;
    public int length = 5;
    private float[] waveStartTime = new float[5];
    private Vector4[] startPos = new Vector4[5];

    //距离系数
    public float distanceFactor = 90.0f;
    //时间系数
    public float timeFactor = -30.0f;
    //sin函数结果系数
    public float totalFactor = 2.0f;
 
    //波纹宽度
    public float waveWidth = 0.5f;
    //波纹扩散的速度
    public float waveSpeed = 0.3f;
 
    void OnRenderImage (RenderTexture source, RenderTexture destination)
    {
       
        float[] curWaveDistance = new float[length];
        _Material.SetFloat("_distanceFactor", distanceFactor);
        _Material.SetFloat("_timeFactor", timeFactor);
        _Material.SetFloat("_totalFactor", totalFactor);
        _Material.SetFloat("_waveWidth", waveWidth);
        for(int i = 0 ;i < length; i++)
        {   
            curWaveDistance[i] = (Time.time - waveStartTime[i]) * waveSpeed;
            _Material.SetFloat("_curWaveDis" + i, curWaveDistance[i]);
            _Material.SetVector("_startPos" + i, startPos[i]);
        }
		Graphics.Blit (source, destination, _Material);
	}
    
    void Update()
    {               
        if (Input.GetMouseButtonDown(0))
        {                                         
            //Debug.Log(count);                                      
            Vector2 mousePos = Input.mousePosition;
            startPos[count] = new Vector4(mousePos.x/ Screen.width, mousePos.y / Screen.height, 0, 0);
            waveStartTime[count] = Time.time;
            count++;
            if(count == length )
            {
                count = 0;
            }       

        }    
            
    }
}
