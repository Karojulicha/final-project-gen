using UnityEngine;

public class ManagementData : MonoBehaviour
{
    public static ManagementData Instance {get; private set;}
    void Awake()
    {
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
