/****************************************************
	�ļ���UIBase.cs
	���ߣ��﴾
	����: 1832259562@qq.com
	���ڣ�2024/04/24 19:39   	
	���ܣ�����UI����
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBase
{
    #region �ֶκ�����
    /// <summary>
    /// �Ƿ�������
    /// </summary>
    private bool m_isInited;

    //UILayer
    private UIType.Layer m_uiLayerType;

    //UILoad
    private UIType.Load m_uiLoadType;

    /// <summary>
    /// UI����Դ·��
    /// </summary>
    private string m_uiPath = "";
    public string UIPath
    {
        get { return m_uiPath; }
        set
        {
            m_uiPath = value;
        }
    }
    /// <summary>
    /// UI����
    /// </summary>
    private string m_uiName;
    public string UIName
    {
        get { return m_uiName; }
        set
        {
            m_uiName = value;
        }
    }

    /// <summary>
    /// �ر�ʱ�Ƿ񻺴�UI
    /// </summary>
    private bool m_isCacheUI = true;
    public bool IsCacheUI
    {
        get { return m_isCacheUI; }
        set
        {
            m_isCacheUI = value;
        }
    }

    /// <summary>
    /// UIʵ��
    /// </summary>
    private GameObject m_uiGameObject;
    public GameObject UIGameObject
    {
        get { return m_uiGameObject; }
        set { m_uiGameObject = value; }
    }

    /// <summary>
    /// UI�Ƿ�ɼ�
    /// </summary>
    private bool m_uiActive = false;
    public bool UIActive
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

    protected UIBase(string uiName, UIType.Layer layerType, string uiPath = Config.UIPrefabPath, UIType.Load loadType = UIType.Load.sync)
    {
        UIName = uiName;
        UIPath = uiPath;
        m_uiLayerType = layerType;
        m_uiLoadType = loadType;
    }

    public virtual void Init(params object[] para)
    {
        if (m_uiLoadType == UIType.Load.sync)
        {
            GameObject go = ObjectPool.Instance.GetObj(m_uiPath, m_uiName);
            OnPimitive(para);
            OnGameObjectLoaded(go);
        }
        else
        {
            //TODO �첽����
            Debug.LogError("�첽����:" + m_uiName);

        }
    }
    private void OnGameObjectLoaded(GameObject uiObj)
    {
        if (uiObj == null)
        {
            Debug.LogError("UI����ʧ��" + uiObj.name);
            return;
        }
        m_uiGameObject = uiObj;
        m_isInited = true;
        SetPanetByLayerType(m_uiLayerType);
        RectTransform uiRectTransform = m_uiGameObject.GetComponent<RectTransform>();
        SetRectTransAsFullScreen(uiRectTransform);
        UIActive = true;
    }
    protected void SetPanetByLayerType(UIType.Layer layerType)
    {
        switch (layerType)
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
                Debug.LogError("�㼶����");
                break;
        }
    }
    /// <summary>
    /// ����UIȫ��
    /// </summary>
    /// <param name="rectTrans"></param>
    private void SetRectTransAsFullScreen(RectTransform rectTrans)
    {
        rectTrans.localPosition = Vector3.zero;
        rectTrans.sizeDelta = Vector2.zero;
        rectTrans.localScale = Vector3.one;
        rectTrans.anchorMin = Vector2.zero;
        rectTrans.anchorMax = Vector2.one;
    }

    public virtual void Uninit()
    {
        m_isInited = false;
        UIActive = false;
        ObjectPool.Instance.RecycleObj(m_uiGameObject,!m_isCacheUI);
    }

    #region ���󷽷�
    //��ʼ�����ε���
    public abstract void OnPimitive(params object[] para);
    protected abstract void OnActive();
    protected abstract void OnDeActive();
    public virtual void Update(float deltaTime) { }
    public virtual void LateUpdate(float deltaTime) { }
    public virtual void OnLogOut() { }
    #endregion
}


