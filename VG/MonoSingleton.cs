using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == false)
                _instance = FindObjectOfType<T> ();

            return _instance;
        }
    }

    public bool DestroyOnLoad = true;

    #region Unity methods

    protected virtual void Awake ()
    {
        if(_instance && _instance != this)
        {
            Destroy (gameObject);
            return;
        }

        if (DestroyOnLoad == false)
        {
            DontDestroyOnLoad (gameObject);
        }

    }



    #endregion
}