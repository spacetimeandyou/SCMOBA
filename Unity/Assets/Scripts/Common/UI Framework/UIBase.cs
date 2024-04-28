/****************************************************
	文件：UIBase.cs
	作者：孙淳
	邮箱: 1832259562@qq.com
	日期：2024/04/24 19:39   	
	功能：所有UI基类
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBase
{
    #region 字段和属性
    /// <summary>
    /// 是否加载完成
    /// </summary>
    private bool m_isInited;

    //UILayer
    private UIType.Layer m_uiLayerType;

    //UILoad
    private UIType.Load m_uiLoadType;

    /// <summary>
    /// UI的资源路径
    /// </summary>
    private string m_uiPath = "";
    public string m_UIPath
    {
        get { return m_uiPath; }
        set
        {
            m_uiPath = value;
        }
    }
    /// <summary>
    /// UI名字
    /// </summary>
    private string m_uiName;
    public string m_UIName
    {
        get { return m_uiName; }
        set
        {
            m_uiName = value;
        }
    }

    /// <summary>
    /// 关闭时是否缓存UI
    /// </summary>
    private bool m_isCacheUI = false;
    public bool m_IsCacheUI
    {
        get { return m_isCacheUI; }
        set
        {
            m_isCacheUI = value;
        }
    }

    /// <summary>
    /// UI实例
    /// </summary>
    private GameObject m_uiGameObject;
    public GameObject m_UIGameObject
    {
        get { return m_uiGameObject; }
        set { m_uiGameObject = value; }
    }

    /// <summary>
    /// UI是否可见
    /// </summary>
    private bool m_uiActive = false;
    public bool m_UIActive
    {
        get { return m_uiActive; }
        set
        {
            m_uiActive = value;
            if (m_uiGameObject != null)
            {
                m_uiGameObject.SetActive(value);
                if (m_uiGameObject.activeSelf)
                {
                    OnActive();
                }
                else
                {
                    OnDeActive();
                }
            }
        }
    }

    public bool IsInited { get { return m_isInited; } }
    #endregion

    protected UIBase(string uiName, string uiPath, UIType.Layer layerType, UIType.Load loadType = UIType.Load.sync)
    {
        m_UIName = uiName;
        m_UIPath = uiPath;
        m_uiLayerType = layerType;
        m_uiLoadType = loadType;
    }

    public virtual void Init()
    {
        if (m_uiLoadType == UIType.Load.sync)
        {
            GameObject go = ObjectPool.Instance.GetObj(m_uiPath, m_uiName);
            OnGameObjectLoaded(go);
        }
        else
        {
            //TODO 异步加载
            Debug.LogError("异步加载:" + m_uiName);

        }
    }
    private void OnGameObjectLoaded(GameObject uiObj)
    {
        if (uiObj == null)
        {
            Debug.LogError("UI加载失败" + uiObj.name);
            return;
        }
        m_uiGameObject = uiObj;
        m_isInited = true;
        SetPanetByLayerType(m_uiLayerType);
        m_uiGameObject.transform.localPosition = Vector3.zero;
        m_uiGameObject.transform.localScale = Vector3.one;
    }
    protected void SetPanetByLayerType(UIType.Layer layerType)
    {
        switch (m_uiLayerType)
        {
            case UIType.Layer.Top:
                m_uiGameObject.transform.SetParent(UIManager.Instance.RectTransTop);
                break;
            case UIType.Layer.Normal:
                m_uiGameObject.transform.SetParent(UIManager.Instance.RectTransNormal);
                break;
            case UIType.Layer.Bottom:
                m_uiGameObject.transform.SetParent(UIManager.Instance.RectTransBottom);
                break;
            default:
                Debug.LogError("层级出错");
                break;
        }
    }
    public virtual void Uninit()
    {
        m_isInited = false;
        m_UIActive = false;
        if (m_isCacheUI)
        {
            ObjectPool.Instance.RecycleObj(m_uiGameObject);
        }
        else
        {
            ObjectPool.Instance.RecycleObj(m_uiGameObject, true);
        }
    }

    #region 抽象方法
    protected abstract void OnActive();
    protected abstract void OnDeActive();
    public virtual void Update(float deltaTime) { }
    public virtual void LateUpdate(float deltaTime) { }
    public virtual void OnLogOut() { }
    #endregion
}


