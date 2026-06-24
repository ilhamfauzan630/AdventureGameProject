using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdventureGame
{
    public class CountdownTimer : MonoBehaviour
    {
        public static CountdownTimer Instance;
        
        [Header("Timer")]
        public float timeLeft = 60f;

        [Header("UI")]
        public TextMeshProUGUI timerText;
        public GameObject winPanel;
        public GameObject failedPanel;

        [Header("Result UI")]
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI timeText;
        public TextMeshProUGUI totalScoreText;

        [Header("Stage")]
        public int nextStageToUnlock = 2;

        [Header("Final Stage")]
        public bool isFinalStage = false;

        [Header("Stage Objective")]
        public int requiredHits = 1;

        private int collectedBonusTime = 0;

        public bool GameEnded { get; private set; }

        private bool timerRunning = true;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
        
        private void Start()
        {
            GameEnded = false;

            UpdateTimerUI();

            if (winPanel != null)
                winPanel.SetActive(false);
        }

        private void Update()
        {
            if (!timerRunning)
                return;

            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;

                if (timeLeft < 0)
                    timeLeft = 0;

                UpdateTimerUI();
            }
            else
            {
                Win();
            }
        }

        private void UpdateTimerUI()
        {
            int minutes = Mathf.FloorToInt(timeLeft / 60);
            int seconds = Mathf.FloorToInt(timeLeft % 60);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            // warna merah saat hampir habis
            if (timeLeft <= 10)
            {
                timerText.color = Color.red;
            }
        }

        private void Win()
        {
            GameEnded = true;
            timerRunning = false;

            int hitScore = GameManager.Instance.HitCount;

            Debug.Log("Hit Score = " + hitScore);
            Debug.Log("Required Hits = " + requiredHits);
            if (hitScore < requiredHits)
            {
                if (failedPanel != null)
                    failedPanel.SetActive(true);

                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                return;
            }

            // =====================
            // WIN
            // =====================

            int timeBonus = collectedBonusTime;
            int totalScore = (hitScore * 100) + (timeBonus * 10);

            if (scoreText != null)
                scoreText.text = "Target Hit : " + hitScore;

            if (timeText != null)
                timeText.text = "Time Bonus : " + timeBonus;

            if (totalScoreText != null)
                totalScoreText.text = "Total Score : " + totalScore;

            int currentUnlocked = PlayerPrefs.GetInt("UnlockedStage", 1);

            if (nextStageToUnlock > currentUnlocked)
            {
                PlayerPrefs.SetInt("UnlockedStage", nextStageToUnlock);
                PlayerPrefs.Save();

                Debug.Log("Stage baru terbuka: " + nextStageToUnlock);
            }

            if (isFinalStage)
            {
                PlayerPrefs.SetInt("GameFinished", 1);
                PlayerPrefs.Save();

                Debug.Log("Game Finished!");
            }
            
            if (winPanel != null)
                winPanel.SetActive(true);

            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void NextStage()
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene("StageSelect");
        }

        // fungsi tambah waktu
        public void AddTime(float extraTime)
        {
            timeLeft += extraTime;

            // simpan total bonus yang dikumpulkan
            collectedBonusTime += Mathf.FloorToInt(extraTime);

            UpdateTimerUI();
        }
    }
}