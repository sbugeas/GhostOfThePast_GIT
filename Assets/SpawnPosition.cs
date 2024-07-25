using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPosition : MonoBehaviour
{
    private void Awake()
    {
       GameObject player = GameObject.FindGameObjectWithTag("Player");
       player.transform.position = transform.position;
       Destroy(this.gameObject);
    }
}
