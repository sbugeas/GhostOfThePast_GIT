using UnityEngine;
using System.Collections.Generic;

public class DontDestroyOnLoadScene : MonoBehaviour
{
    public GameObject[] objectsToSave;

    void Awake()
    {
        foreach(GameObject obj in objectsToSave) 
        {
            GameObject[] existingObj = GameObject.FindGameObjectsWithTag(obj.tag);

            //Si un même objet existe déjà dans la scène
            if (existingObj.Length > 1)
            {
                Destroy(obj);
            }
            else //Sinon(1ère instance)
            {
                DontDestroyOnLoad(obj);
            }
            

            
        }
    }
}
