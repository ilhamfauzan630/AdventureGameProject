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

        [Header("Result UI")]
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI timeText;
        public TextMeshProUGUI totalScoreText;

        [Header("Stage")]
        public int nextStageToUnlock = 2;

        [Header("Final Stage")]
        public bool isFinalStage = false;

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

            Debug.Log("Waktu habis!");

            // ambil data
            int hitScore = GameManager.Instance.HitCount;
            int timeBonus = collectedBonusTime;

            // contoh total score
            int totalScore = (hitScore * 100) + (timeBonus * 10);

            // update UI result
            if (scoreText != null)
                scoreText.text = "Target Hit : " + hitScore;

            if (timeText != null)
                timeText.text = "Time Bonus : " + timeBonus;

            if (totalScoreText != null)
                totalScoreText.text = "Total Score : " + totalScore;

            // =========================
            // UNLOCK NEXT STAGE
            // =========================

            int currentUnlocked = PlayerPrefs.GetInt("UnlockedStage", 1);

            if (nextStageToUnlock > currentUnlocked)
            {
                PlayerPrefs.SetInt("UnlockedStage", nextStageToUnlock);
                PlayerPrefs.Save();

                Debug.Log("Stage baru terbuka: " + nextStageToUnlock);
            }

            // tampilkan panel
            if (winPanel != null)
                winPanel.SetActive(true);

            // jika stage terakhir selesai
            if (isFinalStage)
            {
                PlayerPrefs.SetInt("GameFinished", 1);
                PlayerPrefs.Save();
            }
            
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