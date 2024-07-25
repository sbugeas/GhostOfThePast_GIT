using System.Collections;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public GameObject currentWeapon;
    public GameObject armPivot;

    public float meleAttackRange = 0.5f;

    public GameObject weaponPosition;

    public bool canAttack;
    public bool isAttacking;
    public bool haveWeapon;

    public int numberOfWeapons = 0; 

    public int indexOfChild;

    public LayerMask enemyLayers;

    WeaponManager currentWeaponScript;


    void Start() 
    {
        isAttacking = false;
        canAttack = true;
        currentWeapon = null;
        armPivot.GetComponent<ArmRotation>().curr_weapon = null;
    }

    void Update()
    {
        if (currentWeapon == null) 
        {
            haveWeapon = false;
        }
        else haveWeapon = true;

        //Attaque
        if (Input.GetMouseButton(0) && canAttack && haveWeapon)  
        {
            isAttacking = true;
            currentWeaponScript = currentWeapon.GetComponent<WeaponManager>();

            if (currentWeaponScript.rangeWeapon == true) 
            {
                // Attaque range
                PlayerShoot();
            }
            else // Attaque mélée
            { 
                Vector3 slashPos = armPivot.GetComponent<ArmRotation>().slashPosition.position;

                float rotX;
                float rotY;
                float rotZ;

                // SLASH EFFECT
                if (transform.eulerAngles.y == 0f)
                {       
                    rotY = 0f;
                }
                else
                {
                    rotY = -180f;
                }

                if (armPivot.GetComponent<ArmRotation>().weaponPosTop)
                {
                    rotZ = (armPivot.transform.eulerAngles.z - 90f);
                    rotX = 0f;
                }
                else
                {
                    rotZ = (armPivot.transform.eulerAngles.z + 90f);
                    rotX = 180f; 
                }

                if (armPivot.GetComponent<ArmRotation>().weaponPosBot) 
                {
                    rotZ = -rotZ;
                }
                    
                Quaternion rot = Quaternion.Euler(rotX, rotY, rotZ);

                GameObject slash_effect = Instantiate(currentWeaponScript.slashEffect, slashPos, rot) as GameObject;

                //Récupère enemis dans la range(selon layermask)
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(armPivot.GetComponent<ArmRotation>().slashPosition.position, meleAttackRange, enemyLayers);

                float weaponDamage = currentWeaponScript.baseDamage;

                //Leur applique les dégats + maj de leur healthBar
                foreach (Collider2D enemy in hitEnemies)
                {
                    enemy.GetComponent<EnemyHealth>().TakeDamage(weaponDamage);

                    Vector2 enemyPos = new Vector2(enemy.transform.position.x, enemy.transform.position.y);
                    Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
                    Vector2 push_dir = enemyPos - playerPos;
                    push_dir.Normalize();

                    enemy.GetComponent<EnemyHealth>().PushEnemy(push_dir, currentWeaponScript.powerPush);

                }
            }

            float coolDownAttack = 1 / (currentWeaponScript.attackSpeed);
            canAttack = false;
            StartCoroutine(WaitAttackCoolDown(coolDownAttack));
        }


        //Changement arme
        if ((Input.GetKeyDown(KeyCode.A)) && (canAttack == true)) 
        {
            if (numberOfWeapons >= 2)
            {
                ChangeWeapon('A');
            }
        }
        else if (Input.GetKeyDown(KeyCode.E)) 
        {
            if ((numberOfWeapons >= 2) && (canAttack == true))
            {
                ChangeWeapon('E');
            }
            
        }
    }

    public void PlayerShoot() 
    {
        Vector3 projectilePos = currentWeapon.transform.GetChild(0).transform.position;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject projectile = Instantiate(currentWeaponScript.projectile, projectilePos, Quaternion.identity) as GameObject;

        projectile.GetComponent<SpriteRenderer>().enabled = false;

        ProjectileManager projManager = projectile.GetComponent<ProjectileManager>();

        projManager.target = mousePos;
        projManager.powerShoot = currentWeaponScript.projSpeed;
        projManager.projRange = currentWeaponScript.range;
        projManager.ArmPivotPos = new Vector2(armPivot.transform.position.x, armPivot.transform.position.y);
        projManager.weaponOfThisProjectile = currentWeapon;
        projManager.isShooted = true;

    }

    public void ChangeWeapon(char c) 
    {
        currentWeapon.SetActive(false);

        //Parcours des arme vers la droite
        if(c == 'E') 
        {
            //index max atteint
            if(indexOfChild == (numberOfWeapons - 1)) 
            {
                indexOfChild = 0;
            }
            else indexOfChild++;

        }
        else //Parcours des arme vers la gauche
        {
            //index min atteint
            if (indexOfChild == 0)
            {
                indexOfChild = numberOfWeapons - 1;
            }
            else indexOfChild--;
        }

        //On sélectionne l'arme selon index (parmi les enfants de weaponPosition)
        currentWeapon = weaponPosition.gameObject.transform.GetChild(indexOfChild).gameObject; 
        armPivot.GetComponent<ArmRotation>().curr_weapon = currentWeapon;

       currentWeapon.SetActive(true);

    }

    private IEnumerator WaitAttackCoolDown(float _time) 
    {
        yield return new WaitForSeconds(_time);
        canAttack = true;
        isAttacking = false;
    }

    void OnDrawGizmosSelected() 
    {
        Transform slash_pos = armPivot.GetComponent<ArmRotation>().slashPosition;

        if (slash_pos == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(slash_pos.position, meleAttackRange);
    }
}
