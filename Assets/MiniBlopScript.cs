using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBlopScript : MonoBehaviour
{
    void KillMiniBlop() 
    {
        if (StageManagement.instance.currentEnemyList.Contains(this.gameObject))
        {
            StageManagement.instance.currentEnemyList.Remove(this.gameObject);
        }

        Destroy(gameObject);
    }
}
