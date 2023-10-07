using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus instance;
    [SerializeField] private float defense;
    [SerializeField] private float luck;
    [SerializeField] private float critRate;
    [SerializeField] public float MaxDefense; // Thêm biến MaxDefense
    [SerializeField] private GameObject status; // Thêm tham chiếu đến game object "infor"



    public TextMeshProUGUI defensetxt;
    public TextMeshProUGUI lucktxt;
    public TextMeshProUGUI critratetxt;
    public TextMeshProUGUI speedtxt;
    public TextMeshProUGUI damagetxt;
    public TextMeshProUGUI healthtxt;

    // References
    private PlayerMove playerMove; // Thêm biến tham chiếu đến PlayerMove.cs
    private PlayerAttack playerAttack;
    private Health health; // Thay thế currentHealth và maxHealth




    public int upgradePoints; // Số điểm nâng cấp
    public TextMeshProUGUI upgradePointstxt; // Thêm UI để hiển thị upgradePoints
    private bool isStatusOpen = false; // Biến để kiểm tra xem trạng thái status có đang mở hay không
    public bool canlevelup = false;


    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        playerAttack = GetComponent<PlayerAttack>();
        health = GetComponent<Health>();
        UpdateUI();
        health.OnHealthChanged += UpdateHealthInStatus;
        status.SetActive(false);
        canlevelup = false;

        if (instance == null)
        {
            instance = this;
        }

        if (PlayerPrefs.HasKey("PlayerDefense"))
        {
            defense = PlayerPrefs.GetFloat("PlayerDefense", 2f);
            MaxDefense = PlayerPrefs.GetFloat("PlayerMaxDefense", 10f);
        }
        if (PlayerPrefs.HasKey("PlayerLuck"))
        {
            luck = PlayerPrefs.GetFloat("PlayerLuck", 2f);
        }
        if (PlayerPrefs.HasKey("PlayerCrit"))
        {
            critRate = PlayerPrefs.GetFloat("PlayerCrit", 2f);
        }
        if (PlayerPrefs.HasKey("PlayerHealth"))
        {
            health.startingHealth = PlayerPrefs.GetFloat("PlayerHealth");
        }
        if (PlayerPrefs.HasKey("UpgradePoints"))
        {
            upgradePoints = PlayerPrefs.GetInt("UpgradePoints");
        }

        UpdateUI();


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ToggleStatus(); // Khi nhấn nút K, chuyển đổi trạng thái status
            UpdateUI(); // Đảm bảo rằng bạn gọi UpdateUI() khi bạn mở trạng thái
        }
    }

    private void ToggleStatus()
    {
        isStatusOpen = !isStatusOpen;

        // Vô hiệu hóa tấn công nếu trạng thái status đang mở
        if (isStatusOpen)
        {
            if (playerAttack != null)
            {
                playerAttack.enabled = false;
            }

            // Kích hoạt game object "infor" khi mở trạng thái
            status.SetActive(true);
        }
        else
        {
            if (playerAttack != null)
            {
                playerAttack.enabled = true;
            }

            // Vô hiệu hóa game object "infor" khi đóng trạng thái
            status.SetActive(false);
        }

        // Cập nhật giao diện người dùng sau khi chuyển đổi trạng thái
        UpdateUI();
    }

    void OnApplicationQuit()
    {
        // Khi ứng dụng được đóng, đặt giá trị coin về 0 và lưu vào PlayerPrefs        
        Reset();

    }

    public void Reset()
    {
        defense = 11;
        PlayerPrefs.SetFloat("PlayerDefense", defense);
        luck = 0;
        PlayerPrefs.SetFloat("PlayerLuck", luck);
        critRate = 0;
        PlayerPrefs.SetFloat("PlayerCrit", critRate);
        PlayerPrefs.SetInt("UpgradePoints", upgradePoints);
        PlayerPrefs.Save(); // Lưu thay đổi vào PlayerPrefs
        UpdateUI();
    }



    public void SetCanLevelUp(bool value)
    {
        canlevelup = value;
    }
    public void UpdateUI()
    {
        defensetxt.text = "Defense: " + defense.ToString();
        lucktxt.text = "Luck: " + luck.ToString("F0") + "%";
        critratetxt.text = "Crit Rate: " + critRate.ToString("F0") + "%";

        if (upgradePointstxt != null)
        {
            upgradePointstxt.text = "Your Points: " + upgradePoints.ToString();

        }
        // Hiển thị giá trị speed
        if (playerMove != null && speedtxt != null)
        {
            speedtxt.text = "Speed: " + playerMove.speed.ToString(); // Sử dụng playerMove.speed để lấy giá trị speed từ PlayerMove.cs
        }

        if (playerAttack != null && damagetxt != null)
        {
            damagetxt.text = "Damage: " + playerAttack.damage.ToString();
        }

        if (health != null && healthtxt != null)
        {
            healthtxt.text = "Health: " + health.currentHealth.ToString();
        }
    }

    void UpdateHealthInStatus(float newHealth)
    {
        healthtxt.text = "Health: " + newHealth.ToString();
    }

    public float Defense
    {
        get { return defense; }
        set
        {
            defense = value;
            PlayerPrefs.SetFloat("PlayerDefense", defense);
            PlayerPrefs.Save();
            UpdateUI();
        }
    }

    public float Luck
    {
        get { return luck; }
        set
        {
            luck = value;
            PlayerPrefs.SetFloat("PlayerLuck", luck);
            PlayerPrefs.Save();
            UpdateUI();
        }
    }

    public float CritRate
    {
        get { return critRate; }
        set
        {
            critRate = value;
            PlayerPrefs.SetFloat("PlayerCrit", critRate);
            PlayerPrefs.Save();
            UpdateUI();
        }
    }
}
