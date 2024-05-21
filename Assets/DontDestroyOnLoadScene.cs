using UnityEngine;

public class DontDestroyOnLoadScene : MonoBehaviour
{
    public GameObject[] objectsToSave;

    void Awake()
    {
        foreach(var obj in objectsToSave) 
        {
            DontDestroyOnLoad(obj);
        }
    }
}
