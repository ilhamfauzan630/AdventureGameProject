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

        public static bool GameEnded = false;

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

        public void NextStage()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            SceneManager.LoadScene(currentSceneIndex + 1);
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
