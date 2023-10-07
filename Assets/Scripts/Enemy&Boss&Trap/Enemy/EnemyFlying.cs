using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlying : MonoBehaviour
{
    [Header("Moving")]  
    [SerializeField] private float speed;
    public Transform startingPoint;
    [Header("Enemy Move")]
    [SerializeField] private float agroRange;
    [SerializeField] private float moveSpeed;

    public Animator anim;
    public Transform player;
    public bool chase=false;
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
        float distToPlayer = Vector2.Distance(transform.position, player.position);


        if(distToPlayer <= agroRange)
        {
            ChasePlayer();
        }
        else
        {
            StopChasePlayer();
        }
    }

    void ChasePlayer()
    {
        if (transform.position.x < player.position.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0,0,0);
        }
        else if (transform.position.x > player.position.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0,180, 0);
        }
    }

    void StopChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, startingPoint.position, speed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, agroRange);

    }

}
