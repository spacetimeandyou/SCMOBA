/****************************************************
	文件：ResourcesType.cs
	作者：孙淳
	邮箱: 1832259562@qq.com
	日期：2024/05/04 20:01   	
	功能：资源加载类型和名字
*****************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class ResourcesType
{
    //材质
    //图片
    //场景
    //预制体
}

class SceneName
{
    public const string GameBattle = "GameBattle";
}

class func
{
    private void init()
    {
        ResourcesManager.Instance.AsyncLoadScene(SceneName.GameBattle, () =>
        {
            //加载战斗场景
            //产生回调发送加载完毕消息，开始Tick
        });
    }
}

