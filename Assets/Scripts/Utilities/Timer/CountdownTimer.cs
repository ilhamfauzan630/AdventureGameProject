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

            if (winPanel != null)
                winPanel.SetActive(true);

            Time.timeScale = 0f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void NextStage()
        {
            Time.timeScale = 1f;

            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;

            // cek apakah masih ada scene berikutnya
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.Log("Sudah stage terakhir!");

                if (winPanel != null)
                {
                    winPanel.SetActive(true);
                }
            }
        }

        // fungsi tambah waktu
        public void AddTime(float extraTime)
        {
            timeLeft += extraTime;

            UpdateTimerUI();
        }
    }
}