using System;
using System.Threading;

namespace Game
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			if(!DbManager.Connect("game", "127.0.0.1", 3306, "root", "sc010709")){
				return;
			}

			NetManager.StartLoop(8888);
            
        }
	}
}
