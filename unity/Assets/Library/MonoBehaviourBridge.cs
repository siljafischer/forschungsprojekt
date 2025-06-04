using UnityEngine;

// unity-script: only can start coroutines from a class that derivates from MonoBehaviour (!= C#)
// this helper: allows to start coroutines from a normal C#-Script
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
                // cant destroy it in game mode
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }
}
