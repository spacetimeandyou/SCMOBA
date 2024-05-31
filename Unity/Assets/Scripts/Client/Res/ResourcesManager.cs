/****************************************************
	文件：ResourcesManager.cs
	作者：孙淳
	邮箱: 1832259562@qq.com
	日期：2024/05/04 20:00   	
	功能：资源加载管理器
*****************************************************/
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

class ResourcesManager : Singleton<ResourcesManager>
{
    //异步加载场景,加载Loading界面
    private Action prgCB = null;
    public void AsyncLoadScene(string SceneName, Action loaded)
    {
        Form_Loading loading =(Form_Loading)UIManager.Instance.ActiveUI<Form_Loading>(UIPanel.Form_Loading);

        AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(SceneName);
        prgCB = () =>
        {
            float val = sceneAsync.progress;
            loading.SetProgress(val);

            if (val == 1)
            {
                if (loaded != null)
                {
                    loaded();
                }
                prgCB = null;
                sceneAsync = null;
                UIManager.Instance.DeActiveUI<Form_Loading>(UIPanel.Form_Loading);
            }
        };
    }
    public void Update()
    {
        if (prgCB != null)
        {
            prgCB();
        }
    }
}

