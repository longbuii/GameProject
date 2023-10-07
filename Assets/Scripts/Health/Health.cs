using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Collections.AllocatorManager;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Health")]
    public float startingHealth;
    public float maxHealth;
    public float initialHealth; // Biến tạm thời để lưu trạng thái máu ban đầu

    public float MaxHealth => maxHealth;
    public float currentHealth { get; private set; }
    private bool dead;
    private Rigidbody2D rb;
    [SerializeField] private float damage;
    public event Action<float> OnHealthChanged;

    // Reference
    private Animator anim;
    private PlayerStatus playerStatus;


    [Header("Health Status")]
    public TextMeshProUGUI healthtxt;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int iFrameCount;
    private SpriteRenderer spriteRend;

    [Header("Component")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;

    [Header("Death Sound")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        playerStatus = GetComponent<PlayerStatus>();

        currentHealth = startingHealth;

        // Kiểm tra xem khóa "PlayerHealth" đã tồn tại trong PlayerPrefs
        if (PlayerPrefs.HasKey("PlayerHealth"))
        {
            currentHealth = PlayerPrefs.GetFloat("PlayerHealth", startingHealth);
            maxHealth = PlayerPrefs.GetFloat("PlayerMaxHealth", 10f);
            initialHealth = PlayerPrefs.GetFloat("PlayerInitialHealth", 10f);
        }
        playerStatus.UpdateUI();
    }

    void OnApplicationQuit()
    {
        // Khi ứng dụng được đóng, lưu giá trị `currentHealth` vào PlayerPrefs.
        ResetHealth();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            // Giảm máu khi nút Y được nhấn
            TakeDamage(5f); // Điều chỉnh số lượng máu bị giảm tùy theo nhu cầu của bạn.
        }
    }
    public void ResetHealth()
    {
        PlayerPrefs.SetFloat("PlayerInitialHealth", initialHealth);
        currentHealth = initialHealth;
        PlayerPrefs.SetFloat("PlayerHealth", currentHealth);
        PlayerPrefs.Save();
        playerStatus.UpdateUI();

    }
    public void TakeDamage(float _damage)
    {
        if (invulnerable) return;

        if (playerStatus.Defense > 0)
        {
            // Trừ sát thương từ Defense
            float actualDamage = CalculateActualDamage(_damage);

            playerStatus.Defense -= actualDamage;

            // Nếu giáp dưới 0, đặt nó thành 0 thông qua thuộc tính Defense
            playerStatus.Defense = Mathf.Max(playerStatus.Defense, 0);

            // Thông báo cho PlayerStatus biết rằng giáp đã bị trừ
            playerStatus.UpdateUI();
        }
        else
        {
            // Trừ sát thương vào máu
            currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
            OnHealthChanged?.Invoke(currentHealth);
        }

        // Kiểm tra xem player còn máu hay không
        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Inuvunerability());
            SoundManager.instance.Playsound(hurtSound);
        }
        else
        {
            if (!dead)
            {
                rb.bodyType = RigidbodyType2D.Static;

                // Deactivate all attached component classes
                foreach (Behaviour component in components)
                    component.enabled = false;
                anim.SetBool("Jump", false);
                anim.SetTrigger("die");
                dead = true;
                SoundManager.instance.Playsound(deathSound);
            }
        }
        if (currentHealth > 4)
        {
            PlayerPrefs.SetFloat("PlayerHealth", currentHealth);
            PlayerPrefs.Save();
        }
        // Gọi sự kiện OnHealthChanged để cập nhật UI
        OnHealthChanged?.Invoke(currentHealth);
    }

    float CalculateActualDamage(float rawDamage)
    {
        if (playerStatus != null)
        {
            float actualDamage = rawDamage;
            return actualDamage;
        }
        else
        {
            // Handle the case where playerStatus is null (e.g., log an error or use a default value).
            return rawDamage;
        }
    }

    public void Addheal(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
        if (OnHealthChanged != null)
        {
            OnHealthChanged(currentHealth);
        }
    }

    public void IncreaseMaxHealth(float _value)
    {
        float previousMaxHealth = currentHealth - startingHealth;
        startingHealth += _value;
        currentHealth = startingHealth + previousMaxHealth;
    }

    public void Setheal(float _value)
    {
        startingHealth = _value;

        initialHealth = _value;
    }

    public void Respawn()
    {
        Setheal(initialHealth); // Đặt lại máu về giá trị ban đầu
        rb.bodyType = RigidbodyType2D.Dynamic;
        anim.ResetTrigger("die");
        anim.Play("Moving");
        StartCoroutine(Inuvunerability());

        // Activate all attached component classes
        foreach (Behaviour component in components)
            component.enabled = true;
        TakeDamage(damage);
    }
    private IEnumerator Inuvunerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < iFrameCount; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (iFrameCount * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (iFrameCount * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
        invulnerable = false;
    }
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

}


