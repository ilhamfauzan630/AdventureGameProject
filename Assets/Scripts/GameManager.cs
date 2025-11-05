using TMPro;
using UnityEngine;
using UnityEngine.UI; // untuk akses Text UI

namespace AdventureGame
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance; // singleton agar mudah diakses

        [Header("Target Settings")]
        public int totalTargets = 10;
        private int hitCount = 0;

        [Header("UI")]
        public TextMeshProUGUI  counterText;
        public GameObject winPanel; // panel win (bisa popup atau text besar)

        private void Awake()
        {
            // pastikan hanya satu instance
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            UpdateCounterUI();
            if (winPanel != null)
                winPanel.SetActive(false);
        }

        public void RegisterHit()
        {
            hitCount++;
            UpdateCounterUI();

            if (hitCount >= totalTargets)
            {
                Win();
            }
        }

        private void UpdateCounterUI()
        {
            if (counterText != null)
                counterText.text = $"{hitCount}/{totalTargets}";
        }

        private void Win()
        {
            Debug.Log("You Win!");
            if (winPanel != null)
                winPanel.SetActive(true);
            // bisa tambahkan efek, suara, atau transisi ke level berikutnya
        }
    }
}
