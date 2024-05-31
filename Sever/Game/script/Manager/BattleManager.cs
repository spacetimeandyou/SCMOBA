using System;
using System.Collections.Generic;

class BattleManager
{
    public static void Update()
    {
        if (RoomManager.rooms.Count > 0)
        {
            foreach (var item in RoomManager.rooms.Values)
            {
                if (item.status == Room.Status.FIGHT)
                {
                    item.Update();
                }
            }
        }
    }
}

