using UnityEngine;

public abstract class SingleTon<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance { get => instance; }

    protected virtual void Awake()
    {
        instance = this as T;
        var objs = Object.FindObjectsOfType<T>();
        if (objs.Length > 1)
        {
            foreach (var obj in objs)
            {
                if(obj != instance)
                {
                    Object.Destroy(obj);
                }
            }
        }
    }
}
