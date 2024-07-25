using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlopScript : MonoBehaviour
{
    public GameObject miniBlop_pf;


    public void KillBlop() 
    {
        SplitBlop();

        if (StageManagement.instance.currentEnemyList.Contains(this.gameObject))
        {
            StageManagement.instance.currentEnemyList.Remove(this.gameObject);
        }

        Destroy(gameObject);
    }

    public void SplitBlop() 
    {
        Vector3 pos_mini_Blop_1 = new Vector3((transform.position.x + 0.3f), transform.position.y, 0f); //Ajuster gap selon pos sur anim
        Vector3 pos_mini_Blop_2 = new Vector3((transform.position.x - 0.3f), transform.position.y, 0f); //Ajuster gap selon pos sur anim

        GameObject miniBlop_1 = Instantiate(miniBlop_pf, pos_mini_Blop_1, Quaternion.identity) as GameObject;
        GameObject miniBlop_2 = Instantiate(miniBlop_pf, pos_mini_Blop_2, Quaternion.identity) as GameObject;

        miniBlop_1.GetComponent<EnemyMeleAttackGen>().enemyActivated = true;
        miniBlop_2.GetComponent<EnemyMeleAttackGen>().enemyActivated = true;

        StageManagement.instance.currentEnemyList.Add(miniBlop_1);
        StageManagement.instance.currentEnemyList.Add(miniBlop_2);
    }
}
