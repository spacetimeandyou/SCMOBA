using Lockstep.Math;
using System;
using System.Collections.Generic;

public interface IEntityView
{
    public void OnTakeDamage(int amount, LVector3 hitPoint);
    public void OnDead();
    public void OnRollbackDestroy();
}

