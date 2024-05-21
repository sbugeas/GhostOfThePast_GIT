using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            if ((StageManagement.instance.stage_index) < (StageManagement.instance.maxStage) - 1)
            {
                StageManagement.instance.PassNextStage();
            }
            else Debug.Log("Fin du niveau");

            this.gameObject.SetActive(false);
        }
    }
}
