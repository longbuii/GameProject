    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class Portal : MonoBehaviour
    {
        // Start is called before the first frame update
        public string sceneToLoad; // Tên của scene bạn muốn chuyển đến.
        public HealthEnemyBoss healthEnemyBoss;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && healthEnemyBoss.isDead)
            {
                // Xác định nếu người chơi là đối tượng va chạm và đã đánh bại boss.

                // Chuyển đến scene mới (màn chơi khác).
                SceneManager.LoadScene(sceneToLoad);    
            }
        }
    }
