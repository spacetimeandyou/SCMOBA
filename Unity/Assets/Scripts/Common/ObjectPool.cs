/****************************************************
	文件：ObjectPool.cs
	作者：孙淳
	邮箱: 1832259562@qq.com
	日期：2024/05/04 20:02   	
	功能：对象池
*****************************************************/
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
    /// 默认构造函数
    /// </summary>
    public ObjectPool()
    {
        pool = new Dictionary<string, List<GameObject>>();
    }
    /// <summary>
    /// 从对象池中获取对象
    /// </summary>
    /// <param name="objName"></param>
    /// <returns></returns>
    public GameObject GetObj(string path, string prefabName)
    {
        GameObject result = null;
        string cloneName = prefabName + "(Clone)";
        if (pool.ContainsKey(cloneName) && pool[cloneName].Count > 0)
        {
            result = pool[cloneName][0];
            result.SetActive(true);
            pool[cloneName].Remove(result);
            //Debug.LogError("对象池获取");
        }
        if (result) return result;
        //对象池内没有对象，实例化并加载
        result = LoadAndInstantiate(path, prefabName);
        return result;
    }
    /// <summary>
    /// 加载并实例化
    /// </summary>
    /// <param name="path"></param>
    /// <param name="prefabName"></param>
    /// <returns></returns>
    private GameObject LoadAndInstantiate(string path, string prefabName)
    {
        GameObject result = null;
        //同步加载，需要迭代
        GameObject prefab = Resources.Load<GameObject>(path + '/' + prefabName);
        if (prefab)
        {
            result = UnityEngine.Object.Instantiate(prefab);
        }
        else
        {
            Debug.LogError("资源加载失败，请检查路径是否正确");
        }
        return result;
    }

    /// <summary>
    /// 回收对象到对象池
    /// </summary>
    /// <param name="objName"></param>
    public void RecycleObj(GameObject obj, bool isDelete = false)
    {
        obj.SetActive(false);
        if (isDelete)
        {
            UnityEngine.Object.Destroy(obj);
            return;
        }
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

