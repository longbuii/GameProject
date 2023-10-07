using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCoolDown;
    [SerializeField] public int damage;
    [SerializeField] private float range;
    [SerializeField] private LayerMask enemyLayer;
    private int originalDamage; // The original damage before applying the buff
    private bool damageBuffActive = false;
    private float damageBuffEndTime;


    [Header("Knockback")]
    [SerializeField] private float knockbackForce;

    [Header("Attack Sound")]
    [SerializeField] private AudioClip attackSound;


    private Animator aim;
    private PlayerStatus playerStatus;
    private PlayerMove playerMove;  
    private float coolDownTimer = Mathf.Infinity;
    private bool meleeAttack;

    private void Start()
    {
        aim = GetComponent<Animator>();
        originalDamage = damage; // Store the original damage when the game starts
        playerMove = GetComponent<PlayerMove>();
        playerStatus = GetComponent<PlayerStatus>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && coolDownTimer > attackCoolDown && playerMove.CanAttack())
        {
            float horizontalValue = playerMove.GetHorizontalValue();

            if (Mathf.Abs(horizontalValue) < 0.1f)
            {
                Attack();
            }
        }

        coolDownTimer += Time.deltaTime;

        if (damageBuffActive && Time.time >= damageBuffEndTime)
        {
            RemoveDamageBuff(originalDamage);
        }
    }

    private void Attack()
    {
        SoundManager.instance.Playsound(attackSound);
        aim.SetTrigger("attack");
        playerMove.DisableMovement(); // Vô hiệu hóa di chuyển của player trong thời gian tấn công
        StartCoroutine(EnableMovementAfterAttack()); // Kích hoạt lại di chuyển sau khi tấn công hoàn thành
        coolDownTimer = 0;

   
        float baseDamage = damage;
        float finalDamage = ApplyCritRate(baseDamage);

    }

    float ApplyCritRate(float damage)
    {
        float randomValue = Random.Range(0f, 1f);
        if (randomValue <= playerStatus.CritRate)
            return damage * 2; 

        return damage;
    }

    private IEnumerator EnableMovementAfterAttack()
    {
        yield return new WaitForSeconds(1.1f); 

        playerMove.EnableMovement(); 
    }

    public void DamageEnemy()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, range, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {

            float randomValue = Random.Range(0f, 100f); 
            int finalDamage = randomValue <= playerStatus.CritRate ? (int)(damage * 2) : damage;

            Debug.Log("Damage: " + finalDamage + (randomValue <= playerStatus.CritRate ? " (Critical!)" : ""));
            enemy.GetComponent<HealthEnemyBoss>().TakeDamage(finalDamage);

            ApplyKnockback(enemy.GetComponent<Rigidbody2D>());
        }
    }

    private void ApplyKnockback(Rigidbody2D enemyRigidbody)
    {
        Vector2 knockbackDirection = (enemyRigidbody.transform.position - transform.position).normalized;
        enemyRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void ApplyDamageBuff(int damageIncreaseAmount, float duration)
    {
        damageBuffActive = true;
        damage = originalDamage + damageIncreaseAmount; 
        damageBuffEndTime = Time.time + duration;
    }

    public void RemoveDamageBuff(int previousDamage)
    {
        damageBuffActive = false;
        damage = previousDamage; 
    }
}
