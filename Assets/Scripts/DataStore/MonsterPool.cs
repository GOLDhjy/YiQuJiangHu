using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Game
{
    public class MonsterPool
    {
        public Transform Root;
        private Queue<Monster> monstersPool = new Queue<Monster>();
        //private Queue<GameObject> monsterGameObjectPool = new Queue<GameObject>();

        public MonsterPool()
        {
            monstersPool = new Queue<Monster>();
           // Root = transform;
        }
        public Monster Next()
        {
            Monster mon = null;
            //Debug.Log(monstersPool.Count);
            if (monstersPool.Count > 0)
                mon = monstersPool.Dequeue();
            else
            {
                return null;
            }
            //Debug.Log(mon.itemType);
            if (mon.transform == null)
            {
                
            }
            else
            {
                mon.transform.gameObject.SetActive(true);
            }
            return mon;
        }
        public void ReturnToPoll(Monster monster)
        {
            //Debug.Log("回退到对象池");
            AddToPool(monster);

        }
        public void AddToPool(Monster monster)
        {
            monster.transform.gameObject.SetActive(false);
            monstersPool.Enqueue(monster);
            //monster.transform.SetParent();
        }
        public void ClearPool()
        {
            //int len = monstersPool.Count;
            //for (int i = 0; i < len; i++)
            //{
            //    var item = monstersPool.Dequeue();
            //    //GameObject.Destroy(item.transform.gameObject);
            //    item = null;
            //}
            monstersPool.Clear();
            monstersPool = null;
        }
    }
}
