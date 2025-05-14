// Datei: Assets/Library/MonoBehaviourBridge.cs
using UnityEngine;

public class MonoBehaviourBridge : MonoBehaviour
{
    private static MonoBehaviourBridge _instance;

    public static MonoBehaviourBridge Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("MonoBehaviourBridge");
                _instance = go.AddComponent<MonoBehaviourBridge>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }
}
