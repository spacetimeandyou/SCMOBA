using System;
using System.Collections.Generic;

class SystemManager: Singleton<SystemManager>
{
    private List<BaseSystem> _systems = new List<BaseSystem>();
    public void RegisterSystem(BaseSystem system)
    {
        _systems.Add(system);
    }
    public void ClearSystem()
    {
        _systems.Clear();
    }
    public void RemoveSystem(BaseSystem system)
    {
        _systems.Remove(system);
    }
    public List<BaseSystem> GetAllSystems()
    {
        return _systems;
    }
}

