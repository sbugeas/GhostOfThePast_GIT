using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    float healthAmount = 30f;

    private void OnTriggerEnter2D(Collider2D col)
    {
        float player_health;
        float max_health;

        if (col.gameObject.tag == "Player")
        {
            max_health = col.gameObject.GetComponent<PlayerStats>().maxHealth;
            player_health = col.gameObject.GetComponent<PlayerStats>().currentHealth + healthAmount;

            if(player_health > max_health) 
            {
                player_health = max_health;
            }

            col.gameObject.GetComponent<PlayerStats>().currentHealth = player_health;
            col.gameObject.GetComponent<PlayerStats>().healthBar.SetHealth(player_health);

            Destroy(gameObject);
        }

    }


}
