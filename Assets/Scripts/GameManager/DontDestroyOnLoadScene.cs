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

            //Si un m�me objet existe d�j� dans la sc�ne
            if (existingObj.Length > 1)
            {
                Destroy(obj);
            }
            else //Sinon(1�re instance)
            {
                DontDestroyOnLoad(obj);
            }
            

            
        }
    }
}
