using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWater : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] private int takedamage;
    [SerializeField] private int enraged;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Enemy Move")]
    [SerializeField] private float agroRange;
    [SerializeField] private float moveSpeed;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private float knockbackForce;

    public Animator anim;
    private Health playerHealth;

    public bool isAttacking = false;
    public bool isInvulnerable = false;
    public bool isFlipped = false;

    public Transform player;
    public Rigidbody2D rb2d;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb2d = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        float distToPlayer = Vector2.Distance(transform.position, player.position);

        if (distToPlayer <= agroRange)
        {
            ChasePlayer();

            if (PlayerInSight())
            {
                if (cooldownTimer >= attackCooldown)
                {
                    cooldownTimer = 0;
                    anim.SetTrigger("Attack");
                }
                return;
            }

        }
        else
        {
            StopChasePlayer();
        }


    }
    void ChasePlayer()
    {
        if (transform.position.x < player.position.x )
        {
            rb2d.velocity = new Vector2(moveSpeed, 0);
            transform.localScale = new Vector3(1, 1, 1);
            isAttacking = false;
        }
        else if (transform.position.x > player.position.x)
        {
            rb2d.velocity = new Vector2(-moveSpeed, 0);
            transform.localScale = new Vector3(-1, 1, 1);
            isAttacking = true;
        }
        anim.SetBool("Moving", true);
    }

    void StopChasePlayer()
    {
        rb2d.velocity = new Vector2(0, 0);
        anim.SetBool("Moving", false);
    }

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
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, agroRange);
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            playerHealth.TakeDamage(damage);

            Vector2 knockbackDirection = (player.position - transform.position).normalized;
            rb2d.AddForce(-knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }


    private void EnragedAttack()
    {
        if (PlayerInSight())
            playerHealth.TakeDamage(enraged);
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

