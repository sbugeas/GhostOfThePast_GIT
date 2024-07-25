using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public string projectileName = "Wood Arrow";

    public Rigidbody2D rb;
    public Collider2D col;

    public float powerShoot = 0f;
    public float projRange = 0f;
    
    public bool isShooted = false;
    private bool onCourse = false;
    public bool projectIsBroken = false;

    public Vector2 target;
    public Vector2 final_target;
    public Vector2 dir = Vector2.zero;
    public Vector2 ArmPivotPos;

    public GameObject weaponOfThisProjectile;

    Animator animator;
    TrailRenderer trailRend;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        trailRend = GetComponent<TrailRenderer>();
        animator.SetBool("IsBroken", false);
        final_target = new Vector2(transform.position.x, transform.position.y);
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {      
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);

        if (isShooted) 
        {
            dir = target - ArmPivotPos;
            dir.Normalize();

            final_target = pos + (dir * projRange);

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = rotation;

            GetComponent<SpriteRenderer>().enabled = true;

            isShooted = false;
            onCourse = true;
        }
           
        if (onCourse) 
        {
            float dist = Vector3.Distance(transform.position, new Vector3(final_target.x, final_target.y, transform.position.z));

            if (dist <= 0.3f) 
            {
                if (!projectIsBroken)
                {
                    DestroyProject();
                }
            }
        }
    }

    void FixedUpdate() 
    {
        if (!projectIsBroken) 
        {
            rb.MovePosition(rb.position + dir * powerShoot * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)  
    {
        if (col.gameObject.tag == "Fondation")
        {
            if (!projectIsBroken) 
            {
                DestroyProject();
            }
        }
        
    }

    public void DestroyProject() 
    {
        projectIsBroken = true;
        trailRend.enabled = false;
        col.enabled = false;
        animator.SetBool("IsBroken", true);
        Vector3 force = new Vector3(dir.x, dir.y, 0f) * 100f;
        rb.AddForce(force);
        Destroy(gameObject, 0.4f);
    }
}
