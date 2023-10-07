using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip CheckpointSound;// Sound that we'll play when picking up a new checkpoint
    private Transform currentCheckpoint; // We'll store last checkpoint here
    private Health playerHealth;
    private UiManager uiManager;
    private Enemy_Side saw;


    void Awake()
    {
        playerHealth = GetComponent<Health>();
        saw = GetComponent<Enemy_Side>();
        uiManager = FindObjectOfType<UiManager>();

    }
    public void CheckRespawn()
    {
        // Check if checkpoint available
        if (currentCheckpoint == null)
        {
            uiManager.GameOver();
            // Show game over screen
            return;
        }
        transform.position = currentCheckpoint.position; // Move player to checkpoint position
        playerHealth.Respawn(); // Restore player health and reset animation
    }

    // Activate checkponts
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Checkpoint")
        {
            currentCheckpoint = col.transform; // Store the checkpoint that we activated as the current one
            SoundManager.instance.Playsound(CheckpointSound);
            col.GetComponent<Collider2D>().enabled = false; // Deactive checkpoint collider
            //col.GetComponent<Animator>().SetTrigger("apper");
        }
    }
}