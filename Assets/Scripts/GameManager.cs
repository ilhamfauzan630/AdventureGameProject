using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

        public bool GameEnded { get; private set; }

        private void Awake()
        {
            // pastikan hanya satu instance            
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            GameEnded = false;
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

                // opsi: tampilkan panel tamat game
                if (winPanel != null)
                {
                    winPanel.SetActive(true);
                }
            }
        }


        private void UpdateCounterUI()
        {
            if (counterText != null)
                counterText.text = $"{hitCount}/{totalTargets}";
        }

        private void Win()
        {
            GameEnded = true;

            if (winPanel != null)
                winPanel.SetActive(true);

            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }
}
