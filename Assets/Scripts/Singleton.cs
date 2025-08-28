using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    protected virtual void Awake() 
    {
        Instance = this as T;
        OnRegistration();
    }

    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }
    protected virtual void OnRegistration()
    {
    }
}

public abstract class SingletonPersistent<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
      
        if (Instance != null) {
            Destroy(gameObject);
            return;
        };
        base.Awake();
        DontDestroyOnLoad(gameObject);
       
    }
}
