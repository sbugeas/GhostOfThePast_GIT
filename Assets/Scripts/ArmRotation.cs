using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour
{
    public GameObject player;
    public GameObject curr_weapon;
    public GameObject player_arm;

    public Transform slashPosition;

    public float zTargetAngle = 0f;

    float speedRotation = 1000f;
    public float speed = 0f;

    float dir_y = 0f;

    public bool weaponPosTop;
    public bool weaponPosBot;
    public bool weaponIsRotating = false;

    public bool playerCanFlip = true;

    private int arm_layer;
    private int weapon_layer;

    void Start() 
    {
        weaponPosTop = true;
        weaponPosBot = false;
    }

    void Update() 
    {
        //joueur regarde à droite
        if (player.transform.eulerAngles.y == 0) 
        {
            if ((transform.eulerAngles.z > 0) && (transform.eulerAngles.z < 145)) //Partie haute
            {
                arm_layer = 3;
                weapon_layer = 2;
            }
            else //Partie basse
            {
                arm_layer = 6;
                weapon_layer = 5;
            }
        }
        else //joueur regarde à gauche
        {
            if ((transform.eulerAngles.z > 0) && (transform.eulerAngles.z < 145))
            {
                arm_layer = 3;
                weapon_layer = 2;
            }
            else 
            {
                arm_layer = 6;
                weapon_layer = 5;     
            }
        }

        player_arm.GetComponent<SpriteRenderer>().sortingOrder = arm_layer;

        if(curr_weapon != null) 
        {
            curr_weapon.GetComponent<SpriteRenderer>().sortingOrder = weapon_layer;
        }

        

        //Flip arme pendant attaque(melée)
        if ((curr_weapon != null) && (player.GetComponent<PlayerEquipment>().isAttacking == false) && (curr_weapon.GetComponent<WeaponManager>().rangeWeapon == false)) 
        {
            if (weaponPosBot)
            {
                if (curr_weapon.GetComponent<SpriteRenderer>().flipY == false)
                {
                    curr_weapon.GetComponent<SpriteRenderer>().flipY = true;
                }
            }
            else if (weaponPosTop)
            {
                if (curr_weapon.GetComponent<SpriteRenderer>().flipY == true)
                {
                    curr_weapon.GetComponent<SpriteRenderer>().flipY = false;
                }
            }
        }
                
    }

    private void FixedUpdate()
    {
        if (weaponPosTop) 
        {
            slashPosition.localPosition = new Vector3(-0.2f,- 0.3f, 0f); 
        }
        else slashPosition.localPosition = new Vector3(-0.2f,0.3f, 0f);

        //Direction depuis le bras vers la souris
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();


        //Calcul rotation(degré)
        float rotationZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        //Si en train d'attaquer avec arme mélée
        if ((player.GetComponent<PlayerEquipment>().isAttacking == true) && (curr_weapon.GetComponent<WeaponManager>().rangeWeapon == false))
        {

            //Si l'arme n'est pas déjà en train de tourner -> calcul angle cible
            if ((weaponIsRotating == false) && (zTargetAngle == 0f)) 
            {
                dir_y = player.transform.eulerAngles.y;

                if ((weaponPosTop == true) && (player.transform.eulerAngles.y == 0))
                {
                    zTargetAngle = transform.eulerAngles.z + 180 ;
                }
                else if((weaponPosTop == true) && (player.transform.eulerAngles.y == 180)) //OK
                {
                    zTargetAngle = transform.eulerAngles.z + 180;                           
                }
                else if ((weaponPosBot == true) && (player.transform.eulerAngles.y == 0))
                {
                    zTargetAngle = transform.eulerAngles.z - 180;
                }
                else if ((weaponPosBot  == true) && (player.transform.eulerAngles.y == 180)) //OK
                {
                    zTargetAngle = transform.eulerAngles.z - 180;
                }

                //Verifs angle cible pas < 0 OU > 360
                if (zTargetAngle < 0)
                {
                    zTargetAngle = -zTargetAngle;
                }

                if (zTargetAngle >= 360) 
                {
                    zTargetAngle = zTargetAngle % 360;
                }
                
                //Direction rotation selon position arme
                if (weaponPosBot == true)
                {
                    speed = speedRotation;
                }
                else speed = -speedRotation;

                weaponIsRotating = true;

            }
              
            //Regarde si le joueur change d'orientation pendant rotation arme
            if(player.transform.eulerAngles.y != dir_y) 
            {
                speed = -speed;
                dir_y = player.transform.eulerAngles.y;
            }

        }
        else 
        {
            if (curr_weapon == null) //Si pas d'arme
            {
                
                if (player.transform.eulerAngles.y == 0) //Regarde à droite
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, rotationZ - 45);
                }
                else transform.rotation = Quaternion.Euler(0f, 0f, rotationZ + 45);

            }
            else if (curr_weapon.GetComponent<WeaponManager>().rangeWeapon == true)  //Si arme range
            {

                //On applique la rotation exacte
                transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

                if (rotationZ < -90 || rotationZ > 90)
                {
                    if (player.transform.eulerAngles.y == 0) //Regarde à droite
                    {
                        transform.localRotation = Quaternion.Euler(180, 0, -rotationZ);
                    }
                    else if (player.transform.eulerAngles.y == 180) //Regarde à gauche
                    {
                        transform.localRotation = Quaternion.Euler(180, 180, -rotationZ);
                    }

                }
            } //Si arme mélée équipée et n'attaque pas
            else if ((curr_weapon.GetComponent<WeaponManager>().rangeWeapon == false) && (player.GetComponent<PlayerEquipment>().isAttacking == false))
            {
                //On applique la rotation + marge (arme melée)

                if (weaponPosTop)
                {
                    if (player.transform.eulerAngles.y == 0)
                    {
                        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ + 90);
                    }
                    else
                    {
                        transform.rotation = Quaternion.Euler(0f, 180f, -rotationZ - 90);
                    }

                }
                else if (weaponPosBot)
                {
                    if (player.transform.eulerAngles.y == 0)
                    {
                        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ - 90);
                    }
                    else
                    {
                        transform.rotation = Quaternion.Euler(0f, 180f, -rotationZ + 90);
                    }
                }

            }
        }

        transform.Rotate(Vector3.forward * Time.deltaTime * speed);


        float gap = 20f;

        // Si angle cible atteint
        if ((transform.eulerAngles.z >= zTargetAngle - gap) && (transform.eulerAngles.z <= zTargetAngle + gap) && weaponIsRotating == true)
        {
            speed = 0f;
            zTargetAngle = 0.0f;
            player.GetComponent<PlayerEquipment>().isAttacking = false;

            weaponPosTop = !weaponPosTop;
            weaponPosBot = !weaponPosBot;

            weaponIsRotating = false;
        }
        
        
    }

}
