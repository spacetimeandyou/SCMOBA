using Lockstep.Math;
using System;
using System.Collections.Generic;

class BaseLifeCycle
{
    public virtual void DoAwake() { }
    public virtual void DoUpdate() { }
    public virtual void DoDestroy() { }
}

