using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Modules;

public class MonsterPoolManager :Singleton<MonsterPoolManager>
{
    //[Header("MonsterRoot")]
    //public GameObject StartRoot;
    //private bool IsStart;
    //[Header("Speed")]
    //public float Speed;
    //[Header("Short")]
    //public GameObject ShortMon;
    //[Header("Long")]
    //public GameObject LongMon;
    //[Header("Boss")]
    //public GameObject Boss;
    //[Header("Trap")]
    //public GameObject Trap;
    //[Header("Err")]

    //static MonsterPool monsterPool;
    private Dictionary<ItemType, MonsterPool> monsterPool = new Dictionary<ItemType, MonsterPool>();
    // Start is called before the first frame update
    public MonsterPool GetMonsterPool(ItemType itemType)
    {
        if (monsterPool == null)
        {
            Debug.Log("创建新的对象池管理器");
            monsterPool = new Dictionary<ItemType, MonsterPool>();
        }
        MonsterPool pool = null;
        if (monsterPool.TryGetValue(itemType,out pool))
        {
        }
        else
        {
            monsterPool[itemType] = new MonsterPool();
            monsterPool.TryGetValue(itemType, out pool);
        }
        return pool;
    }
    public void ClearALLPool()
    {
        Debug.Log("删除对象池个数："+monsterPool.Values.Count);
        foreach (var item in monsterPool.Values)
        {
            item.ClearPool();
        }
        monsterPool = null;
    }
}
