using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMlee : MonoBehaviour
{

    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    [SerializeField] private float range;
    [SerializeField] private int takedamage;

    private bool isDead = false;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Enemy Move")]
    const string LEFT = "left";
    const string RIGHT = "right";


    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;
    //References
    public Animator anim;
    private Health playerHealth;
    private Rigidbody2D rb2d;
    Vector3 baseScale;

    public bool isInvulnerable = false;

    public Transform player;
    private EnemyPatrol enemyPatrol;


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        baseScale = transform.localScale;
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb2d = GetComponent<Rigidbody2D>();;
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            cooldownTimer = 0;
            anim.SetTrigger("Attack");
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
    }


    #region Move
    //private void Update()
    //{
    //    float vX = moveSpeed;

    //    if(facingDirection == LEFT)
    //    {
    //        vX = -moveSpeed;
    //    }

    //    rb2d.velocity = new Vector2(vX, rb2d.velocity.y);

    //    anim.SetBool("Moving", true);

    //    if (IsWall() || IsNear())
    //    {
    //        if (facingDirection == LEFT)
    //        {

    //            ChangeFacingDirection(RIGHT);
    //        }
    //        else if (facingDirection == RIGHT)
    //        {
    //            ChangeFacingDirection(LEFT);
    //        }
    //    }


    //}

    //void   ChangeFacingDirection(string newDirection)
    //{
    //    Vector3 newScale = baseScale;

    //    if (newDirection == LEFT)
    //    {
    //        newScale.x = -baseScale.x;
    //    }
    //    else if (newDirection == RIGHT)
    //    {
    //        newScale.x = baseScale.x;  
    //    }

    //    transform.localScale = newScale;

    //    facingDirection = newDirection;
    //}

    //bool IsWall()
    //{
    //    bool val = false;
    //    float castDist = baseCastDist;
    //    if(facingDirection == LEFT)
    //    {
    //        castDist = -baseCastDist;
    //    }
    //    else
    //    {
    //        castDist = baseCastDist;    
    //    }

    //    Vector3 targetPos = castPos.position;
    //    targetPos.x += castDist;

    //    Debug.DrawLine(castPos.position, targetPos, Color.blue);

    //    if(Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground"))) 
    //    {
    //        val = true;
    //    }
    //    else
    //    {
    //        val = false;
    //    }

    //    return val;

    //}
    //    bool IsNear()
    //    {
    //        bool val = true;
    //        float castDist = baseCastDist;

    //        Vector3 targetPos = castPos.position;
    //        targetPos.y -= castDist;

    //        Debug.DrawLine(castPos.position, targetPos, Color.blue);

    //        if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground")))
    //        {
    //            val = false;
    //        }
    //        else
    //        {
    //            val = true;
    //        }

    //        return val;

    //    }
    #endregion



    private bool PlayerInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
            playerHealth.TakeDamage(damage);
    }
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(takedamage);
            }
        }
    }
}