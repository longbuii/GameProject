using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgradeManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int upgradePointsOnLevelUp; // Số điểm nâng cấp khi lên cấp
    public int damage;
    private PlayerStatus playerStatus; // Tham chiếu đến PlayerStatus.cs
    private PlayerMove PlayerMove;
    private PlayerAttack playerAttack;
    public Image defenseUpgradeButton;
    public Image luckUpgradeButton;
    public Image critRateUpgradeButton;
    public Image speedUpgradeButton;
    public Image damageUpgradeButton;



    void Start()
    {
        playerStatus = GetComponent<PlayerStatus>(); // Lấy tham chiếu đến PlayerStatus.cs
        PlayerMove = GetComponent<PlayerMove>();
        playerAttack = GetComponent<PlayerAttack>();
        damage = playerAttack.damage;

        if (PlayerPrefs.HasKey("UpgradePoints"))
        {
            playerStatus.upgradePoints = PlayerPrefs.GetInt("UpgradePoints");
        }
        UpdateUI();

    }
    public void UpgradeDefense()
    {
        if (playerStatus.upgradePoints > 0)
        {
            playerStatus.Defense += 0.5f; // Tăng điểm nâng cấp cho Defense
            playerStatus.upgradePoints--;
            UpdateUI();
            playerStatus.UpdateUI();
            PlayerPrefs.SetInt("UpgradePoints", playerStatus.upgradePoints);
            PlayerPrefs.Save();
        }
    }

    public void UpgradeCritRate()
    {
        if (playerStatus.upgradePoints > 0)
        {
            playerStatus.CritRate += 0.5f; // Tăng điểm nâng cấp cho CritRate
            playerStatus.upgradePoints--;
            UpdateUI();
            playerStatus.UpdateUI();
            PlayerPrefs.SetInt("UpgradePoints", playerStatus.upgradePoints);
            PlayerPrefs.Save();
        }
    }

    public void UpgradeLuck()
    {
        if (playerStatus.upgradePoints > 0)
        {
            playerStatus.Luck += 0.5f; // Tăng điểm nâng cấp cho Luck
            playerStatus.upgradePoints--;
            UpdateUI();
            playerStatus.UpdateUI();
            PlayerPrefs.SetInt("UpgradePoints", playerStatus.upgradePoints);
            PlayerPrefs.Save();
        }
    }

    public void UpgradeDamage()
    {
        if (playerStatus.upgradePoints > 0)
        {
            // Tính toán tăng chỉ số damage kiểu số nguyên (int) lên 1
            int damageIncrease = 1;

            // Tăng damage của PlayerAttack theo kiểu số nguyên
            playerAttack.damage += damageIncrease;

            playerStatus.upgradePoints--;
            UpdateUI();
            playerStatus.UpdateUI();
            PlayerPrefs.SetInt("UpgradePoints", playerStatus.upgradePoints);
            PlayerPrefs.Save();
        }
    }

    public void UpgradeSpeed()
    {
        if (playerStatus.upgradePoints > 0)
        {
            PlayerMove.speed += 0.5f; // Tăng điểm nâng cấp cho Defense
            PlayerPrefs.SetFloat("PlayerSpeed", PlayerMove.speed);

            playerStatus.upgradePoints--;
            UpdateUI();
            playerStatus.UpdateUI();
            PlayerPrefs.SetInt("UpgradePoints", playerStatus.upgradePoints);
            PlayerPrefs.Save();
        }
    }

    public void UpdateUI()
    {
        // Cập nhật UI để ẩn/hiện các nút tăng điểm nâng cấp
        if (defenseUpgradeButton != null)
        {
            defenseUpgradeButton.enabled = playerStatus.upgradePoints > 0;
        }

        if (critRateUpgradeButton != null)
        {
            critRateUpgradeButton.enabled = playerStatus.upgradePoints > 0;
        }

        if (luckUpgradeButton != null)
        {
            luckUpgradeButton.enabled = playerStatus.upgradePoints > 0;
        }

        if (damageUpgradeButton != null)
        {
            damageUpgradeButton.enabled = playerStatus.upgradePoints > 0;
        }

        if (speedUpgradeButton != null)
        {
            speedUpgradeButton.enabled = playerStatus.upgradePoints > 0;
        }

        playerStatus.UpdateUI();
    }
}
