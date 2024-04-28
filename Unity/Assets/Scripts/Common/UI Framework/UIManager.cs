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

    /// <summary>
    /// 摄像机UICamera
    /// </summary>
    private Camera m_uiCamera;
    public Camera UICamera
    {
        get
        {
            return m_uiCamera;
        }
    }

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
        return InitUIInfo() && UIRegister();
    }

    /// <summary>
    /// 初始化UI信息
    /// </summary>
    /// <returns></returns>
    public bool InitUIInfo()
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

        m_uiCamera = m_uiRoot.transform.Find("Camera").GetComponent<Camera>();
        GameObject.DontDestroyOnLoad(m_uiRoot);
        return true;
    }
    /// <summary>
    /// UI注册,挂载第一个UI
    /// </summary>
    private bool UIRegister()
    {
        Form_Loading form_Loading = new Form_Loading();
        m_uiDict.Add(UIPanel.Form_Loading, form_Loading);
        form_Loading.Init();
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
            m_uiCamera = null;
        }
    }

    /// <summary>
    /// 打开一个UI的接口
    /// </summary>
    /// <param name="uiName"></param>
    /// <returns></returns>
    public UIBase ActiveUI(string uiName)
    {
        UIBase mUIBase = GetUI(uiName);
        if (mUIBase == null)
        {
            Debug.LogError("UIDic里面没有这个UI信息 UIName：" + uiName);
            return null;
        }

        if (!mUIBase.IsInited)
        {
            mUIBase.Init();
        }

        return mUIBase;
    }

    /// <summary>
    /// 关闭一个UI的接口
    /// </summary>
    /// <param name="uiName"></param>
    public void DeActiveUI(string uiName)
    {
        UIBase mUIBase = GetUI(uiName);
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
    public UIBase GetUI(string uiName)
    {
        UIBase mUIBase = null;
        m_uiDict.TryGetValue(uiName, out mUIBase);
        return mUIBase;
    }

    public T GetUI<T>(string uiName) where T : UIBase
    {
        UIBase mUIBase = null;
        if (m_uiDict.TryGetValue(uiName, out mUIBase))
        {
            if (mUIBase is T)
            {
                return (T)mUIBase;
            }
        }
        return null;
    }

    /// <summary>
    /// 关闭所有UI的接口
    /// </summary>
    public void DeActiveAll()
    {
        foreach (KeyValuePair<string, UIBase> pair in m_uiDict)
        {
            DeActiveUI(pair.Key);
        }
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

    /// <summary>
    /// LateUpdate方法
    /// </summary>
    /// <param name="delta"></param>
    public void LateUpdate(float delta)
    {
        foreach (var mUIBase in m_uiDict.Values)
        {
            mUIBase.LateUpdate(delta);
        }
    }

    /// <summary>
    /// 注销方法
    /// </summary>
    public void OnLogout()
    {
        foreach (var mUIBase in m_uiDict.Values)
        {
            mUIBase.OnLogOut();
        }
        if (m_uiCamera)
        {
            m_uiCamera.enabled = false;
        }
    }
}

