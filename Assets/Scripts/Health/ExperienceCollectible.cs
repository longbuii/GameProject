using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceCollectible : MonoBehaviour
{
    [SerializeField] private float experienceValue; // Giá trị kinh nghiệm mà item này cung cấp
    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            SoundManager.instance.Playsound(pickupSound);
            XpManager xpManager = col.GetComponent<XpManager>();

            // Gọi hàm GainExperience trong XpManager và truyền giá trị kinh nghiệm
            xpManager.GainExperience(experienceValue);

            // Tắt item sau khi người chơi tương tác
            gameObject.SetActive(false);
        }
    }
}
