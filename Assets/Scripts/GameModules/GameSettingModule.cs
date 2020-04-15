using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Modules
{
    public class GameSettingModule : Singleton<GameSettingModule>
    {
        /// <summary>
        /// 时间偏移，节拍点，单位 秒
        /// </summary>
        private static float offset = 0;

        public enum QualityEnum
        {
            Low,
            Mid,
            High
        }
        bool EdgeTurn = false;

        public static float Offset { get => PlayerPrefs.GetFloat("Offset"); set => PlayerPrefs.SetFloat("Offset", value); }

        /// <summary>
        /// 设置画质等级
        /// </summary>
        /// <param name="quality">等级枚举</param>
        public void SetQualityLevel(QualityEnum quality)
        {
            if (quality != QualityEnum.High)
            {
                Camera.main.GetComponent<FogWithNoise>().enabled = false;
                
            }
            else
            {
                Camera.main.GetComponent<FogWithNoise>().enabled = true;
                
            }
            QualitySettings.SetQualityLevel(Convert.ToInt32(quality), true);

        }
        /// <summary>
        /// 开启/关闭风格化描边
        /// </summary>
        public void SetEdgeDetection()
        {
            if (EdgeTurn)
            {
                Camera.main.GetComponent<EdgeDetection>().enabled = false;
                EdgeTurn = false;
            }
            else
            {
                Camera.main.GetComponent<EdgeDetection>().enabled = true;
                EdgeTurn = true;
            }
        }
    }
}
