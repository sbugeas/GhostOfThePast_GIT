using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadSpecificScene : MonoBehaviour
{
    public string sceneName;

    private void OnTriggerEnter2D(Collider2D col) 
    {
        if (col.CompareTag("Player")) 
        {
            StageManagement.instance.sceneName = sceneName;
            SceneManager.LoadScene(sceneName);
        }
    }
}
