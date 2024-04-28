using System;
using System.Collections.Generic;
using UnityEngine;

class ObjectPool : Singleton<ObjectPool>
{
    /// <summary>
    /// 对象池
    /// </summary>
    private Dictionary<string, List<GameObject>> pool;
    /// <summary>
    /// 预设体
    /// </summary>
    private Dictionary<string, GameObject> prefabs;
    /// <summary>
    /// 默认构造函数
    /// </summary>
    public ObjectPool()
    {
        pool = new Dictionary<string, List<GameObject>>();
        prefabs = new Dictionary<string, GameObject>();
    }

    /// <summary>
    /// 从对象池中获取对象
    /// </summary>
    /// <param name="objName"></param>
    /// <returns></returns>
    public GameObject GetObj(string path, string objName)
    {
        GameObject result = null;
        if (pool.ContainsKey(objName))
        {
            if (pool[objName].Count > 0)
            {
                result = pool[objName][0];
                result.SetActive(true);
                pool[objName].Remove(result);
                return result;
            }
        }
        GameObject prefab = null;
        if (prefabs.ContainsKey(objName))
        {
            prefab = prefabs[objName];
        }
        else
        {
            //待迭代，Resources文件加载prefab
            prefab = Resources.Load<GameObject>(path + '/' + objName);
            prefabs.Add(objName, prefab);
        }
        result = UnityEngine.Object.Instantiate(prefab);
        return result;
    }

    /// <summary>
    /// 回收对象到对象池
    /// </summary>
    /// <param name="objName"></param>
    public void RecycleObj(GameObject obj, bool isDelete = false)
    {
        obj.SetActive(false);
        if (isDelete) return;
        if (pool.ContainsKey(obj.name))
        {
            pool[obj.name].Add(obj);
        }
        else
        {
            pool.Add(obj.name, new List<GameObject>() { obj });
        }
    }

}

