/****************************************************
	文件：EventArgsType.cs
	作者：孙淳
	邮箱: 1832259562@qq.com
	日期：2024/05/04 19:58   	
	功能：消息参数定义
*****************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 网络消息参数
/// </summary>
class MsgEventArgs : EventArgs
{
    public MsgBase msgBase;
}

class StringEventArgs : EventArgs 
{
	public string str;
}


