using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 4f;
    public bool _isMoving = false;
    public GameObject current_Weapon;
    
    Animator animator;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Vector2 dir;

    public GameObject playerArm;

    Camera cam;


    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        current_Weapon = GetComponent<PlayerEquipment>().currentWeapon;
    }

    void Update()
    {

        //On récupère les inputs

        //Axe Horizontal
        if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1f;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            dir.x = -1f;
        }
        else dir.x = 0f;

        //Axe Vertical
        if (Input.GetKey(KeyCode.Z))
        {
            dir.y = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dir.y = -1f;
        }
        else dir.y = 0f;

        if ((dir.x == 0) && (dir.y == 0)) 
        {
            _isMoving = false;
        }
        else _isMoving = true;

        animator.SetBool("IsMoving", _isMoving);
    }
    void FixedUpdate()
    {
        //Déplacement + flip
        MovePlayer(dir);    
        Flip();
             
    }

    void MovePlayer(Vector2 _dir) 
    {
        rb.MovePosition(rb.position + _dir * speed * Time.fixedDeltaTime);
    }

    
    void Flip()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        float gap = 0.2f;

        if ((mousePos.x >= transform.position.x + gap) && (transform.eulerAngles.y != 0f))
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0f, transform.eulerAngles.z);       
        }
        else if((mousePos.x <= transform.position.x - gap) && (transform.eulerAngles.y != 180f))
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 180f, transform.eulerAngles.z);
        }

    }



}
