using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float max_hp = 100f;
    public float cur_hp;
    public bool isPushed = false;

    public HealthBar healthBarScript;
    public GameObject healhBarCanv;

    Rigidbody2D rb;
    public bool enemyIsAttacked = false;
    public Animator animator;

    SpriteRenderer spriteRenderer;
    Collider2D col;
    Color baseColor;

    void Start() 
    {
        cur_hp = max_hp;
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        baseColor = spriteRenderer.color;
        healthBarScript.SetMaxHealth(max_hp);
    }

   

    private void OnTriggerEnter2D(Collider2D col)
    {
        //CAS DU PROJECTILE
        if (col.gameObject.tag == "Projectile")
        {
            if (enemyIsAttacked == true)
            {
                StopCoroutine(EnemyIsAttacked());  
                spriteRenderer.color = baseColor;
            }

            enemyIsAttacked = true;

            float damage = col.gameObject.GetComponent<ProjectileManager>().weaponOfThisProjectile.GetComponent<WeaponManager>().baseDamage;                  
            float _powerPush = col.gameObject.GetComponent<ProjectileManager>().weaponOfThisProjectile.GetComponent<WeaponManager>().powerPush;
            Vector2 _dir = col.gameObject.GetComponent<ProjectileManager>().dir;

            PushEnemy(_dir, _powerPush);
            TakeDamage(damage);


            if (col.gameObject.GetComponent<ProjectileManager>().projectIsBroken == false)
            {
                col.gameObject.GetComponent<ProjectileManager>().DestroyProject();
            }

        }
        else if ((col.gameObject.tag == "Fondation") && (isPushed == true)) 
        {
            StopCoroutine(WaitPushTime());
            rb.velocity = new Vector3(0, 0, 0);
            isPushed = false;
        }
    }

    public void TakeDamage(float _damage) 
    {
        cur_hp -= _damage;
        healthBarScript.SetHealth(cur_hp);

        StartCoroutine(EnemyIsAttacked());
        
        if (cur_hp <= 0) 
        {
            EnemyDie();
        }
        
    }

    public void PushEnemy(Vector2 dir, float _power_push) //dir doit être normalisé
    {
        Vector2 force_2D = dir * _power_push;
        Vector3 force_3D = new Vector3(force_2D.x, force_2D.y, 0f);

        rb.AddForce(force_3D);
        isPushed = true;

        GetComponent<EnemyMeleAttackGen>().canMove = false;
        GetComponent<EnemyMeleAttackGen>().isAttacking = false;

        StartCoroutine(WaitPushTime());
    }

    private IEnumerator EnemyIsAttacked()
    {
        spriteRenderer.color = new Color(1, 0, 0, 1);
        yield return new WaitForSeconds(0.4f);
        spriteRenderer.color = baseColor;

        enemyIsAttacked = false;

             
    }

    private IEnumerator WaitPushTime()
    {
        //On gère quand la poussée se stop
        yield return new WaitForSeconds(0.3f);
        rb.velocity = new Vector3(0, 0, 0);
        isPushed = false;
        GetComponent<EnemyMeleAttackGen>().canMove = true;
        GetComponent<EnemyMeleAttackGen>().isAttacking = true;
    }

    public void EnemyDie() 
    {
        GetComponent<EnemyMeleAttackGen>().enemyActivated = false;
        col.enabled = false;
        healhBarCanv.SetActive(false);
        animator.SetTrigger("isDead");
    }
}
