using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules;
using UnityEngine;

namespace Game
{
    public class MonsterFactory : Singleton<MonsterFactory>
    {

        public void CreatMonster(KeyNode node,GameObject gameObject)
        {
            GameObject MonObj = GameObject.Instantiate<GameObject>(gameObject);
            Monster monster = new Monster(MonObj.transform, node.index,node.PathWay, node.StartTime, node.EndTime, node.itemType);
            MonsterPoolManager.Instance.GetMonsterPool(node.itemType).AddToPool(monster);
            // monsterPool.AddToPool(monster);
        }
        public Monster GetMonster(KeyNode node, GameObject gameObject)
        {
            Monster mon = MonsterPoolManager.Instance.GetMonsterPool(node.itemType).Next();
            if (mon == null)
            {
                CreatMonster(node, gameObject);
                mon = MonsterPoolManager.Instance.GetMonsterPool(node.itemType).Next();
            }
            else
            {
                //GameObject MonObj = GameObject.Instantiate<GameObject>(gameObject);
                mon.Reset(mon.transform, node.index, node.PathWay, node.StartTime, node.EndTime, node.itemType);          
            }
            return mon;
        }



    }
}
