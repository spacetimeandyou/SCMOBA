/****************************************************
	文件：UIManager.cs
	作者：孙淳
	邮箱: 1832259562@qq.com
	日期：2024/04/25 16:37   	
	功能：UI管理类
*****************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;


class UIManager : Singleton<UIManager>
{
    /// <summary>
    /// 核心的管理所有UI的Dictionary
    /// </summary>
    private Dictionary<string, UIBase> m_uiDict;
    
    private GameObject m_uiRoot;
    public GameObject UIRoot { get { return m_uiRoot; } }
    #region Normal
    private Transform m_transNormal;
    public Transform TransNormal { get { return m_transNormal; } }
    private RectTransform m_rectTransNormal;
    public RectTransform RectTransNormal { get { return m_rectTransNormal; } }
    #endregion
    #region Top
    private Transform m_transTop;
    public Transform TransTop { get { return m_transTop; } }
    private RectTransform m_rectTransTop;
    public RectTransform RectTransTop { get { return m_rectTransTop; } }
    #endregion
    #region Bottom
    private Transform m_transBottom;
    private RectTransform m_rectTransBottom;
    public Transform TransBottom { get { return m_transBottom; } }
    public RectTransform RectTransBottom { get { return m_rectTransBottom; } }
    #endregion

    public bool Init()
    {
        m_uiDict = new Dictionary<string, UIBase>();
        m_uiRoot = ObjectPool.Instance.GetObj(Config.UIPrefabPath, "UIRoot");
        if (m_uiRoot == null)
        {

            return false;
        }
        m_uiRoot.name = "UIRoot";
        m_uiRoot.SetActive(true);
        m_transNormal = m_uiRoot.transform.Find("NormalLayer");
        m_rectTransNormal = m_transNormal.gameObject.GetComponent<RectTransform>();
        m_transTop = m_uiRoot.transform.Find("TopLayer");
        m_rectTransTop = m_transTop.gameObject.GetComponent<RectTransform>();
        m_transBottom = m_uiRoot.transform.Find("BottomLayer");
        m_rectTransBottom = m_transBottom.gameObject.GetComponent<RectTransform>();
        GameObject.DontDestroyOnLoad(m_uiRoot);
        return true;
    }

    public void UnInit()
    {
        if (m_uiRoot)
        {
            ObjectPool.Instance.RecycleObj(m_uiRoot);
            m_uiRoot = null;
            m_transNormal = null;
            m_rectTransNormal = null;
            m_transTop = null;
            m_rectTransTop = null;
            m_transBottom = null;
            m_rectTransBottom = null;
        }
    }

    /// <summary>
    /// 打开一个UI的接口
    /// </summary>
    /// <param name="uiName"></param>
    /// <returns></returns>
    public UIBase ActiveUI<T>(string uiName,params object[] para)where T:UIBase,new()
    {
        UIBase mUIBase = GetUI<T>(uiName);
        if (mUIBase == null)
        {
            Debug.LogError("UIDic里面没有这个UI信息 UIName：" + uiName);
            return null;
        }

        if (!mUIBase.IsInited)
        {
            mUIBase.Init(para);
        }

        return mUIBase;
    }

    /// <summary>
    /// 关闭一个UI的接口
    /// </summary>
    /// <param name="uiName"></param>
    public void DeActiveUI<T>(string uiName) where T : UIBase, new()
    {
        UIBase mUIBase = GetUI<T>(uiName);
        if (mUIBase == null)
        {
            Debug.LogError("UIDic里面没有这个UI信息 UIName：" + uiName);
            return;
        }

        if (mUIBase.IsInited)
        {
            mUIBase.Uninit();
        }
    }

    /// <summary>
    /// 获取一个UI的接口
    /// </summary>
    /// <param name="uiName"></param>
    /// <returns></returns>
    public UIBase GetUI<T>(string uiName) where T :UIBase,new()
    {
        if (!m_uiDict.ContainsKey(uiName))
        {
            m_uiDict.Add(uiName, new T());
        }
        m_uiDict.TryGetValue(uiName, out UIBase mUIBase);
        return mUIBase;
    }

    /// <summary>
    /// Update方法
    /// </summary>
    /// <param name="delta"></param>
    public void Update(float delta)
    {
        foreach (var mUIBase in m_uiDict.Values)
        {
            mUIBase.Update(delta);
        }
    }
}

