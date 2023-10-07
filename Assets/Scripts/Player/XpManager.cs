using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class XpManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text_1;
    [SerializeField] private TextMeshProUGUI text_2;
    [SerializeField] private Image barFill;
    [SerializeField] private Image barOutline;
    [SerializeField] private Image circle_1;
    [SerializeField] private Image circle_2;
    [SerializeField] private GameObject xpObject;

    // References
    private PlayerStatus playerStatus; // Tham chiếu đến PlayerStatus.cs
    private PlayerUpgradeManager playerUpgrade;

    private float currentAmount = 0;
    private int currentLevel = 1; // Level hiện tại, bắt đầu từ level 1
    private int nextLevel = 2; // Level sau khi lên cấp, mặc định là level 2
    private float experienceRequired = 100f; // Số kinh nghiệm cần thiết để lên cấp
    private static bool hasExitedUnity = false;




    private void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        playerUpgrade = GetComponent<PlayerUpgradeManager>();

        if (PlayerPrefs.HasKey("UpgradePoints"))
        {
            playerStatus.upgradePoints = PlayerPrefs.GetInt("UpgradePoints");
        }
        if (hasExitedUnity)
        {
            ResetExperience();
            hasExitedUnity = false;
        }
        else
        {
            LoadExperience();
        }
    }

    private void Update()
    {
        // Kiểm tra nút G đã được nhấn (bạn có thể thay đổi key hoặc điều kiện kiểm tra)
        if (Input.GetKeyDown(KeyCode.G))
        {
            // Gọi hàm GainExperience với một giá trị XP cố định, ví dụ: 50 XP
            GainExperience(50);
        }
    }
    void OnApplicationQuit()
    {
        hasExitedUnity = true;
        ResetUpgradePoints();
        ResetExperience();
    }
    private void ResetExperience()
    {
        currentAmount = 0;
        currentLevel = 1;
        nextLevel = 2;
        experienceRequired = 100;
        SaveExperience();
    }
    private void ResetUpgradePoints()
    {
        playerStatus.upgradePoints = 0;
        PlayerPrefs.SetInt("UpgradePoints", 0);
        PlayerPrefs.Save();
    }

    // Gọi hàm này khi player nhận EXP
    public void GainExperience(float experience)
    {
        float experienceRatio = experience / experienceRequired;

        if (currentAmount + experienceRatio >= 1f)
        {
            LevelUp();
        }
        else
        {
            UpdateProgressBar(experienceRatio);
        }
    }

    // Hàm để cập nhật thanh bar
    private void UpdateProgressBar(float progress)
    {
        currentAmount += progress;

        // Cập nhật fill amount của thanh bar
        barFill.fillAmount = currentAmount;

        // Cập nhật Text hiển thị level hiện tại và level sau khi lên cấp
        text_1.text = currentLevel.ToString();
        text_2.text = nextLevel.ToString();
    }

    // Hàm thực hiện Level Up
    // Hàm LevelUp cập nhật
    private void LevelUp()
    {
        currentLevel++;
        nextLevel++;

        playerStatus.upgradePoints += playerUpgrade.upgradePointsOnLevelUp; // Tăng điểm nâng cấp

        text_1.text = currentLevel.ToString();
        text_2.text = nextLevel.ToString();

        currentAmount = 0f;
        barFill.fillAmount = currentAmount;

        // Gọi hàm cập nhật UI trong PlayerStatus để hiển thị số điểm nâng cấp
        playerStatus.UpdateUI();
        playerUpgrade.UpdateUI();

        // Cập nhật giá trị kinh nghiệm cần thiết cho level mới
        experienceRequired = CalculateExperienceRequired(currentLevel);
        SaveExperience();
        // Lưu giá trị kinh nghiệm và level mới vào PlayerPrefs
        PlayerPrefs.SetInt("UpgradePoints", playerStatus.upgradePoints);
        PlayerPrefs.Save();

    }
    private float CalculateExperienceRequired(int level)
    {
        // Viết mã ở đây để tính toán kinh nghiệm cần thiết dựa trên level
        return 100f * level; // Ví dụ: 100 XP cho level 1, 200 XP cho level 2, và cứ tiếp tục.
    }
    public void SaveExperience()
    {
        PlayerPrefs.SetFloat("PlayerExperience", currentAmount);
        PlayerPrefs.SetInt("PlayerLevel", currentLevel);
        PlayerPrefs.SetInt("PlayerNextLevel", nextLevel);
        PlayerPrefs.SetFloat("ExperienceRequired", experienceRequired);
        PlayerPrefs.Save();

    }
    public void LoadExperience()
    {
        if (PlayerPrefs.HasKey("PlayerExperience"))
        {
            currentAmount = PlayerPrefs.GetFloat("PlayerExperience");
            currentLevel = PlayerPrefs.GetInt("PlayerLevel");
            nextLevel = PlayerPrefs.GetInt("PlayerNextLevel");
            experienceRequired = PlayerPrefs.GetFloat("ExperienceRequired");
            barFill.fillAmount = currentAmount;
            text_1.text = currentLevel.ToString();
            text_2.text = nextLevel.ToString();
        }
    }
}

