using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI myScore;
    private int scoreNum;
    [SerializeField] private GameObject scoreObject;
    void Start()
    {
        // Lấy điểm số từ PlayerPrefs
        if (!PlayerPrefs.HasKey("Coin"))
        {
            PlayerPrefs.SetInt("Coin", 0);
        }

        scoreNum = PlayerPrefs.GetInt("Coin");
        UpdateScoreUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            AddScore(10);
        }
    }
    void OnApplicationQuit()
    {
        // Khi ứng dụng được đóng, đặt giá trị coin về 0 và lưu vào PlayerPrefs
        ResetScore();
    }
    public void AddScore(int amount)
    {

        scoreNum += amount;
        UpdateScoreUI();

        PlayerPrefs.SetInt("Coin", scoreNum);
        PlayerPrefs.Save(); // Lưu thay đổi vào PlayerPrefs
    }

    public void ResetScore()
    {
        scoreNum = 0; // Đặt giá trị coin về 0
        PlayerPrefs.SetInt("Coin", scoreNum);
        PlayerPrefs.Save(); // Lưu thay đổi vào PlayerPrefs
        UpdateScoreUI();
    }

    public int CalculateLuckyCoin(int baseCoinValue, float luckModifier)
    {
        if (Random.Range(0f, 1f) <= luckModifier)
        {
            return baseCoinValue * 2; // Gấp đôi số coin nếu may mắn thành công
        }
        return baseCoinValue; // Trả về giá trị cơ bản nếu may mắn không thành công
    }

    public void UpdateScoreUI()
    {
        myScore.text = " " + scoreNum.ToString();
    }

}
