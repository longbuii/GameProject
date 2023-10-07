using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpBossManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int xpValue = 10; // Số kinh nghiệm mà boss gửi cho người chơi

    // References
    [SerializeField] private XpManager xpManager;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GiveExperienceToPlayer()
    {
        XpManager xpManager = FindObjectOfType<XpManager>(); // Tìm XpManager trong cả Scene
        if (xpManager != null)
        {
            xpManager.GainExperience(xpValue);
            Debug.Log("Gained " + xpValue + " experience.");
        }
    }
}
