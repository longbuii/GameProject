
using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    [SerializeField] protected float damage;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Trap")
            collision.GetComponent<Health>().TakeDamage(damage);
    }
}
