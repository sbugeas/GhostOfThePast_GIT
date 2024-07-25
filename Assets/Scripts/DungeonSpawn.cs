
using UnityEngine;

public class DungeonSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = transform.position;
        GameObject.Find("GameManager").GetComponent<StageManagement>().InitDungeon();
    }

}
