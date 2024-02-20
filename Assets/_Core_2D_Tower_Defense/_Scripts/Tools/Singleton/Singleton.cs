using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<T>(true);
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = (T) (MonoBehaviour) this;
        }
    }
}
