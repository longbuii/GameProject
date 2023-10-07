using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager Instance; // Singleton pattern

    public float Defense { get; set; }
    public float Luck { get; set; }
    public float Crit { get; set; }
    public float Health { get; set; }
    public float Maxhealth { get; set; }
    public int Upgradepoint { get; set; }
    public float Speed { get; set; }
    public float InitialHealth { get; set; }
    public string Inventory { get; set; }

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        // Load trạng thái từ PlayerPrefs
        LoadGameState();
    }
    public void SaveGameState()
    {
        // Lưu trạng thái vào PlayerPrefs
        PlayerPrefs.SetFloat("PlayerDefense", Defense);
        PlayerPrefs.SetFloat("PlayerLuck", Luck);
        PlayerPrefs.SetFloat("PlayerCrit", Crit);
        PlayerPrefs.SetFloat("PlayerHealth", Health);
        PlayerPrefs.SetFloat("PlayerMaxHealth", Maxhealth);
        PlayerPrefs.SetInt("UpgradePoints", Upgradepoint);
        PlayerPrefs.SetFloat("PlayerSpeed", Speed);
        PlayerPrefs.SetFloat("PlayerInitialHealth", InitialHealth);
        PlayerPrefs.SetString("PlayerInventory", Inventory);
        PlayerPrefs.Save();
    }
    public void LoadGameState()
    {
        // Nạp trạng thái từ PlayerPrefs
        Defense = PlayerPrefs.GetFloat("PlayerDefense");
        Luck = PlayerPrefs.GetFloat("PlayerLuck");
        Crit = PlayerPrefs.GetFloat("PlayerCrit");
        Health = PlayerPrefs.GetFloat("PlayerHealth");
        Maxhealth = PlayerPrefs.GetFloat("PlayerMaxHealth");
        Upgradepoint = PlayerPrefs.GetInt("UpgradePoints");
        Speed = PlayerPrefs.GetFloat("PlayerSpeed");
        InitialHealth = PlayerPrefs.GetFloat("PlayerInitialHealth");
        Inventory = PlayerPrefs.GetString("PlayerInventory");
    }
}
