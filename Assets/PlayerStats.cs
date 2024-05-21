using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public bool isInvincible = false;
    public float invincibilityTime = 2f;

    public HealthBar healthBar;

    public SpriteRenderer current_Weapon_sprite;
    SpriteRenderer spriteRenderer;
    public SpriteRenderer armSprite;

    Color baseColor;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseColor = spriteRenderer.color;

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void PlayerTakeDamage(float _damage) 
    {

        //Calcul nouveau point de vie et maj healthBar
        if ((currentHealth - _damage) < 0f)
        {
            currentHealth = 0f;
        }
        else currentHealth -= _damage;

        healthBar.SetHealth(currentHealth);

        foreach(GameObject enemy in StageManagement.instance.currentEnemyList) 
        {
            //ATTENTION LORSQUE ENNEMI RANGE AJOUTE
            Physics2D.IgnoreCollision(enemy.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
            enemy.GetComponent<EnemyMeleAttackGen>().isAttacking = false;
        }

        

        StartCoroutine(PlayerIsInvincible());
        StartCoroutine(PlayerFlash());

    }

    private IEnumerator PlayerIsInvincible() 
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;

        foreach (GameObject enemy in StageManagement.instance.currentEnemyList)
        {
            //ATTENTION LORSQUE ENNEMI RANGE AJOUTE
            Physics2D.IgnoreCollision(enemy.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
            enemy.GetComponent<EnemyMeleAttackGen>().isAttacking = true;
        }
    }

    private IEnumerator PlayerFlash() 
    {
        bool playerHaveWeapon = (GetComponent<PlayerEquipment>().currentWeapon != null);

        if (playerHaveWeapon) 
        {
            current_Weapon_sprite = GetComponent<PlayerEquipment>().currentWeapon.GetComponent<SpriteRenderer>();
        }
        

        while (isInvincible == true) 
        {
            spriteRenderer.color = new Color(0, 0, 0, 0);
            armSprite.color = new Color(0, 0, 0, 0);

            if (playerHaveWeapon)
            {
                current_Weapon_sprite.color = new Color(0, 0, 0, 0);
            }

            yield return new WaitForSeconds(0.2f);

            spriteRenderer.color = baseColor;
            armSprite.color = baseColor;
            

            if (playerHaveWeapon)
            {
                current_Weapon_sprite.color = baseColor;
            }

            yield return new WaitForSeconds(0.2f);
        }
        
    }


}
