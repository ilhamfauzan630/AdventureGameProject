using TMPro;
using UnityEngine;

namespace AdventureGame
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [Header("Target Settings")]
        public int totalTargets = 10;
        private int hitCount = 0;

        [Header("UI")]
        public TextMeshProUGUI counterText;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            UpdateCounterUI();
        }

        public void RegisterHit()
        {
            hitCount++;
            UpdateCounterUI();
        }

        private void UpdateCounterUI()
        {
            if (counterText != null)
            {
                counterText.text = $"{hitCount}/{totalTargets}";
            }
        }
    }
}