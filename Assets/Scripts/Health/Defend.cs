
using UnityEngine;

public class Defend : MonoBehaviour
{
    [SerializeField] private float defenseValue;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            // Apply the defense value to the player's status
            PlayerStatus playerStatus = col.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                playerStatus.Defense += defenseValue;
            }

            // Deactivate the item
            gameObject.SetActive(false);
        }
    }
}
