using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleAttackGen : MonoBehaviour
{
    public int pointsCount = 0;
    public int curIndexPoint;

    public float moveSpeed = 3f;
    public float damageCac = 20f;

    //Etat
    public bool isAttacking = true;
    public bool playerIsNear = false;
    public bool targetCanChange = true;

    public bool canMove = true;
    public bool enemyActivated = false;

    public GameObject pointsAroundPlayer;
    public Transform target;
    public GameObject player;

    Rigidbody2D rb;
    


    // Start is called before the first frame update
    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        pointsAroundPlayer = player.transform.Find("pointsAroundPlayer").gameObject;
        pointsCount = pointsAroundPlayer.transform.childCount;
        curIndexPoint = 0;
        target = pointsAroundPlayer.transform.GetChild(0).transform;
    }

    void Update() 
    {
        //Si joueur proche ET non invincible
        if (playerIsNear && (player.GetComponent<PlayerStats>().isInvincible == false))
        {
             player.GetComponent<PlayerStats>().PlayerTakeDamage(damageCac);
        }     
    }


    void FixedUpdate() 
    {
        if (enemyActivated) 
        {
            //Etat de contournement (player invincible)
            if ((isAttacking == false) && (canMove == true))
            {
                //dir
                Vector2 dir = new Vector2((target.position.x - transform.position.x), (target.position.y - transform.position.y));
                //déplacement
                rb.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);


                //TIR RAYCAST ET VERIF DEST ACCESSIBLE
                RaycastHit2D[] hits;
                float dist = Vector3.Distance(transform.position, target.position);

                dir.Normalize();

                hits = Physics2D.RaycastAll(new Vector2(transform.position.x, transform.position.y), dir, dist);

                if(checkDestAccepted(hits) == false) 
                {
                    PassNextPointRand();
                }

                Debug.DrawRay(transform.position, dir * dist, Color.red);


                //verif si arrivé destination
                if ((Vector3.Distance(transform.position, target.position) < 0.3f))
                {
                    PassNextPointRand();
                }
            }
            else if (canMove && isAttacking) //Deplacement pour attaquer Player
            {
                Vector2 targetDir = new Vector2((player.transform.position.x - transform.position.x), (player.transform.position.y - transform.position.y));
                targetDir.Normalize();
                MoveEnemy(targetDir);
            }
        }
              
    }

    void MoveEnemy(Vector2 _dir) 
    {
        rb.MovePosition(rb.position + _dir * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D col) 
    {
        if (col.gameObject.tag == "Player") 
        {
            playerIsNear = true;
        }

        if((col.gameObject.tag == "Enemy") && targetCanChange && !isAttacking) 
        {
            PassNextPointRand();
            StartCoroutine(WaitBeforeChangePoint(col.gameObject));
            targetCanChange = false;
            Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
        }
    }

    private void OnCollisionExit2D(Collision2D col) 
    {
        if (col.gameObject.tag == "Player") 
        {
            playerIsNear = false;
        }
    }

    public bool checkDestAccepted(RaycastHit2D[] _hits)
    {
        bool result = true;

        foreach (RaycastHit2D elmt in _hits)
        {
            if (elmt.collider.tag == "Fondation")  
            {
                Debug.Log("Raycast détecte Fondation");
                result = false;
            }
        }

        return result;
    }

    private IEnumerator WaitBeforeChangePoint(GameObject otherEnemy) 
    {
        yield return new WaitForSeconds(0.15f);
        targetCanChange = true;
        Physics2D.IgnoreCollision(otherEnemy.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
    }

    public void PassNextPointRand() 
    {
        int rand_int = Random.Range(1, 4);
        curIndexPoint = (curIndexPoint + rand_int) % pointsCount;
        target = pointsAroundPlayer.transform.GetChild(curIndexPoint).transform;
    }

    /*
    void OnDrawGizmos()
    {
        if (target == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, target.position);
    }
    */
}
