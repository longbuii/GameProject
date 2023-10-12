using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Events;

public class Chest : MonoBehaviour
{
    public enum ChestType
    {
        InstantOpen, // Chest mở ngay khi gặp và có thể nhận coin
        BossDefeatOpen, // Chest chỉ mở sau khi đánh bại boss
        EmptyChest
    }
    public ChestType chestType;

    public int baseCoinValue = 10; // Số coin cơ bản
    public float luckModifier = 0.1f; // Số liệu đo về sự may mắn
    public GameObject openedChestPrefab; // Prefab của rương sau khi mở  
    private bool isOpened = false; // Biến xác định xem rương đã mở hay chưa
    private bool isChestOpened = false;
    private float openedTime;


    // References
    public HealthEnemyBoss healthEnemyBoss;
    private ScoreSystem scoreSystem;
    private Animator anim;


    private void Start()
    {
        // Tìm ScoreSystem trong cùng GameObject chứa script Chest
        scoreSystem = GetComponent<ScoreSystem>();
        anim = GetComponent<Animator>();

        // Kiểm tra trạng thái ban đầu của chest và thực hiện hành vi tương ứng
        switch (chestType)
        {
            case ChestType.InstantOpen:
                // Chest mở ngay khi bắt đầu
                if (isOpened)
                {
                    gameObject.SetActive(false);
                    if (openedChestPrefab != null)
                    {
                        Instantiate(openedChestPrefab, transform.position, transform.rotation);
                    }
                }
                break;

            case ChestType.BossDefeatOpen:
                // Chest chỉ mở sau khi đánh bại boss, không hiển thị ban đầu
                gameObject.SetActive(false);
                break;
        }
    }
    private void Update()
    {
        if (isChestOpened && Time.time - openedTime > 5f)
        {
            // Nếu rương đã mở và đã qua 5 giây, hãy ẩn nó
            gameObject.SetActive(false);
        }
    }

    public void Interact()
    {
        if (isChestOpened)
        {
            return;
        }
        switch (chestType)
        {
            case ChestType.InstantOpen:
                OpenInstantChest();
                break;

            case ChestType.BossDefeatOpen:
                OpenAfterBossDefeat();
                break;

            case ChestType.EmptyChest:
                // Đây là loại rương trống, không cần thực hiện bất kỳ hành động gì.
                // Tắt rương rỗng
                isOpened = true;
                StartCoroutine(DisappearAfterDelay(5f));
                break;
        }
    }
    private IEnumerator DisappearAfterDelay(float delay)
    {
        // Đợi trong khoảng thời gian delay trước khi chạy animation
        yield return new WaitForSeconds(delay);

        // Kích hoạt animation khi mở
        if (anim != null)
        {
            anim.SetBool("open", true); // "OpenChest" là tên của Animation Clip.
        }
        isChestOpened = true;

        // Ghi lại thời điểm rương được mở
        openedTime = Time.time;
    }


    private void OpenInstantChest()
    {
        PlayerStatus playerStatus = FindObjectOfType<PlayerStatus>();
        float chance = playerStatus.Luck / 100f;

        int receivedCoins;

        // Tạo một giá trị ngẫu nhiên trong khoảng 0-1 và so sánh với cơ hội
        if (Random.Range(0f, 1f) <= chance)
        {
            receivedCoins = baseCoinValue * 2; // Gấp đôi số coin nếu may mắn thành công
        }
        else
        {
            receivedCoins = baseCoinValue; // Số coin cơ bản nếu may mắn không thành công
        }

        // Thêm số coin này vào điểm số của người chơi
        scoreSystem.AddScore(receivedCoins);
        Debug.Log("Received " + receivedCoins + " coins!");

        if (openedChestPrefab != null)
        {
            Instantiate(openedChestPrefab, transform.position, transform.rotation);
        }

        // Tắt rương và hiển thị rương đã mở
        isOpened = true;
        if (anim != null)
        {
            anim.SetBool("open", true); // "OpenChest" là tên của Animation Clip.
        }
        isChestOpened = true;
    }

    public void OpenAfterBossDefeat()
    {
        Debug.Log("Is boss dead? " + healthEnemyBoss.isDead);
        if (healthEnemyBoss.isDead)
        {
            gameObject.SetActive(true);

            // Thêm code để xử lý mở chest sau khi đánh bại boss ở đây
            int receivedCoins = baseCoinValue; // Số coin cơ bản, không cần kiểm tra cơ hội
            Debug.Log("Received " + receivedCoins + " coins!");

            // Thêm số coin này vào điểm số của người chơi
            scoreSystem.AddScore(receivedCoins);

            // Tắt rương và hiển thị rương đã mở (nếu có Animation)
            isOpened = true;
            if (anim != null)
            {
                anim.SetBool("open", true); // "OpenChest" là tên của Animation Clip.
            }
            isChestOpened = true;
        }
    }
}
