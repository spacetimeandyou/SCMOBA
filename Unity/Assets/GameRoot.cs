using UnityEngine;

public class GameRoot : MonoBehaviour
{
    public static GameRoot Instance = null;

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        Init();
    }

    private void Init()
    {
        UIManager.Instance.Init();
    }
}
