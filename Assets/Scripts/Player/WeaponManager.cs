using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public int weaponIndex = 1;
    public string weaponName = "Short Bow";
    public bool rangeWeapon = true;

    public float projSpeed = 5f;
    public float attackSpeed = 2f;
    public float range = 5f;
    public float baseDamage = 20f;
    public float powerPush = 2f;

    public static Collider2D playerCol;
    public bool isNear = false;
    public bool playerCarriesThisWeapon = false;

    public bool playerIsAttacking = false;

    public GameObject player;
    public GameObject panel;
    public GameObject projectile;
    public GameObject slashEffect;

    public Animator weaponAnim;
    

    void Update()
    {
        if (playerCarriesThisWeapon && (transform.localEulerAngles.z != 0)) 
        {
           transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        if((Input.GetKeyDown(KeyCode.R)) && (isNear == true) && (player.GetComponent<PlayerEquipment>().canAttack == true) && (playerCarriesThisWeapon == false)) 
        {
            TakeWeaponOnFloor();
        }

        if(playerCarriesThisWeapon) 
        {
            //Si le joueur tiens cette arme
            if (weaponIndex == (player.GetComponent<PlayerEquipment>().currentWeapon.GetComponent<WeaponManager>().weaponIndex))
            {
                playerIsAttacking = player.GetComponent<PlayerEquipment>().isAttacking;
            }
            else playerIsAttacking = false;
        }

        if (weaponAnim != null) 
        {
            weaponAnim.SetBool("IsAttacking", playerIsAttacking);
        }     
    }

    void TakeWeaponOnFloor()    
    {
        player.GetComponent<PlayerEquipment>().numberOfWeapons++;
        player.GetComponent<PlayerEquipment>().indexOfChild = (player.GetComponent<PlayerEquipment>().numberOfWeapons) - 1;

        if (player.GetComponent<PlayerEquipment>().currentWeapon != null) 
        {
            player.GetComponent<PlayerEquipment>().currentWeapon.SetActive(false);
        }
     
        transform.parent = player.GetComponent<PlayerEquipment>().weaponPosition.transform;

        Vector3 weapPos = player.GetComponent<PlayerEquipment>().weaponPosition.transform.position;
        transform.position = weapPos;

        player.GetComponent<PlayerEquipment>().currentWeapon = this.gameObject;
        player.GetComponent<PlayerEquipment>().armPivot.GetComponent<ArmRotation>().curr_weapon = this.gameObject;


        float rotZ = player.GetComponent<PlayerEquipment>().armPivot.transform.eulerAngles.z;
        
        if(player.transform.eulerAngles.y == 0)
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0, rotZ);
        }
        else 
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 180, rotZ);
        }

        playerCarriesThisWeapon = true;
        panel.SetActive(false);

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!playerCarriesThisWeapon)
        {
            if (col.gameObject.tag == "Player")
            {
                isNear = true;
                player = col.gameObject;
                panel.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!playerCarriesThisWeapon)
        {
            if (col.gameObject.tag == "Player")
            {
                isNear = false;
                player = null;
                panel.SetActive(false);
            }
        }     
    }



}
