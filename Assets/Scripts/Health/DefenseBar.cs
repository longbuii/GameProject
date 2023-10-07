using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class DefenseBar : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PlayerStatus playerStatus; // Đối tượng chứa defense và các thông số khác
    [SerializeField] private Image totalDefenseBar;
    [SerializeField] private Image currentDefenseBar;
    [SerializeField] private TextMeshProUGUI defenseText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float currentDefense = playerStatus.Defense;
        float maxDefense = playerStatus.MaxDefense;

        currentDefenseBar.fillAmount = currentDefense / maxDefense;
        defenseText.text = currentDefense.ToString();
    }
}
