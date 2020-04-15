using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Game
{
    public class Monster
    {
        public Transform transform;
        public int Index;
        public int PathWay;
        public float StartTime;
        public float EndTime;
        public ItemType itemType;

        public Monster(Transform transform, int index ,int pathWay, float startTime, float endTime, ItemType itemType)
        {
            this.transform = transform;
            Index = index;
            PathWay = pathWay;
            StartTime = startTime;
            EndTime = endTime;
            this.itemType = itemType;
        }
        public void Reset(Transform transform, int index, int pathWay, float startTime, float endTime, ItemType itemType)
        {
            this.transform = transform;
            Index = index;
            PathWay = pathWay;
            StartTime = startTime;
            EndTime = endTime;
            this.itemType = itemType;
        }

    }
}
