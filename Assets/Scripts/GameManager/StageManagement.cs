using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class StageManagement : MonoBehaviour
{
    public GameObject stages;
    public GameObject current_stage;
    public int stage_index;
    public Transform cur_stage_spawn;
    public GameObject blackScreen;
    public string sceneName;    

    public List<GameObject> currentEnemyList;
    public List<GameObject> currentDoorList;

    public int maxStage; 

    public Camera cam;

    public bool playerOnStage = true;

    public GameObject playerSpawn;
    public GameObject player;

    public Vector3 focusPoint;
    
    public float smoothTime = 0.5f;
    public float spawnTime = 1.25f;

    public bool doorSpawned = false;
    public bool playerInDungeon = false;

    private Vector3 velocity = Vector3.zero;

    public static StageManagement instance;

    private void Awake()
    {
        //Vérification et récupération instance
        if (instance != null)
        {
            Debug.LogWarning("Attention : Il y a plus d'une instance de StageManagement dans la scène !");
            Destroy(this.gameObject);
            //return;
        }

        instance = this;
    }

    void Start() 
    {
        sceneName = SceneManager.GetActiveScene().name;
    }

    void Update()
    {  
        if (playerOnStage) 
        {
            //focusPoint = player
            focusPoint = new Vector3(player.transform.position.x, player.transform.position.y, cam.transform.position.z);
        }
        else 
        {
            //focusPoint = cur_stage_spawn
            focusPoint = new Vector3(cur_stage_spawn.position.x, cur_stage_spawn.position.y, cam.transform.position.z);
        }

        //Deplacement fondu cam sur focusPoint
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, focusPoint, ref velocity, smoothTime);

        if (playerInDungeon) 
        {
            // PLUS D'ENNEMIS -> SPAWN PORTES
            if ((currentEnemyList.Count == 0) && (doorSpawned == false))
            {
                int numberOfDoor = currentDoorList.Count;

                for (int i = 0; i < numberOfDoor; i++)
                {
                    currentDoorList[i].gameObject.SetActive(true);
                }

                doorSpawned = true;
            }
        }    
    }



    public void InitDungeon()
    {
        stages = GameObject.FindGameObjectWithTag("Stages");

        stage_index = 0;
        maxStage = stages.transform.childCount;
        current_stage = stages.transform.GetChild(stage_index).gameObject;

        int numberOfChild = current_stage.transform.childCount;

        //Parcours stage actuel et maj liste portes
        for (int i = 0; i < numberOfChild; i++)
        {
            GameObject current_child = current_stage.transform.GetChild(i).gameObject;

            if (current_child.gameObject.tag == "Door")
            {
                currentDoorList.Add(current_child);
            }
        }

        playerOnStage = true;
        playerInDungeon = true;

        sceneName = SceneManager.GetActiveScene().name;
    }



    //Sur stage doors
    public void PassNextStage()
    {
        //Passe au stage suivant
        if (stage_index < maxStage)
        {
            stage_index++;
        }
        
        //Clear liste ennemies
        if(currentEnemyList.Count != 0) 
        {
            currentEnemyList.Clear();
        }

        //Clear liste portes
        if (currentDoorList.Count != 0) 
        {
            currentDoorList.Clear();
        }
        
        current_stage = stages.gameObject.transform.GetChild(stage_index).gameObject;
        int numberOfChild = current_stage.transform.childCount;

        //Parcours stage actuel et maj (liste enemy et portes)
        for (int i = 0; i < numberOfChild; i++)
        {
            GameObject current_child = current_stage.gameObject.transform.GetChild(i).gameObject;

            if (current_child.gameObject.tag == "Enemy")
            {
                currentEnemyList.Add(current_child);
            }
            else if (current_child.gameObject.tag == "Door")
            {
                currentDoorList.Add(current_child);
            }
            else if (current_child.gameObject.tag == "SpawnPoint")
            {
                cur_stage_spawn = current_child.transform;
            }
        }

        doorSpawned = false;

        playerOnStage = false;
        StartCoroutine(WaitAndSpawn());

    }

    IEnumerator WaitAndSpawn()
    {
        player.SetActive(false);

        blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, 255);

        //Apparition point spawn (joueur)
        player.transform.position = cur_stage_spawn.position;

        yield return new WaitForSeconds(spawnTime);

        blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, 0);

        player.SetActive(true);
        playerOnStage = true;
        player.GetComponent<PlayerEquipment>().canAttack = true;

        //Si liste ennemies non vide, on les active
        if (currentEnemyList.Count != 0) 
        {
            foreach (GameObject enemy in currentEnemyList)
            {
                enemy.SetActive(true);
                enemy.GetComponent<EnemyMeleAttackGen>().enemyActivated = true;                    
            }
        }
    }




}
