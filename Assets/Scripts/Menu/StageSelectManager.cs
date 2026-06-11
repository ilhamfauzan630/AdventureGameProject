using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace AdventureGame
{
    public class StageSelectManager : MonoBehaviour
    {
        [System.Serializable]
        public class StageInfo
        {
            public string stageName;
            public string actTitle;

            [TextArea]
            public string description;

            [Header("Unlock")]
            public int stageID;

            [Header("UI")]
            public Button stageButton;
            public Image stageImage;

            [Header("Sprites")]
            public Sprite unlockedSprite;
            public Sprite lockedSprite;
        }

        [Header("Stage Data")]
        public StageInfo[] stages;

        [Header("UI References")]
        public GameObject stageInfoPanel;     
        // public Image stageImageUI;
        public TextMeshProUGUI titleText;  
        public TextMeshProUGUI descText; 
        public Button playButton;  

        [Header("Ending Popup")]
        public GameObject endingPopup;
        private string currentStageScene;

        public void LoadStage(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void BackToMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        void Start()
        {
            if (stageInfoPanel != null)
                stageInfoPanel.SetActive(false);

            SetupStages();

            // popup ending
            if (endingPopup != null)
            {
                endingPopup.SetActive(false);

                // cek apakah game tamat
                if (PlayerPrefs.GetInt("GameFinished", 0) == 1)
                {
                    endingPopup.SetActive(true);

                    // reset agar popup tidak muncul terus
                    PlayerPrefs.SetInt("GameFinished", 0);
                    PlayerPrefs.Save();
                }
            }
        }

        void SetupStages()
        {
            int unlockedStage = PlayerPrefs.GetInt("UnlockedStage", 1);

            foreach (StageInfo stage in stages)
            {
                bool isUnlocked = unlockedStage >= stage.stageID;

                // Tombol bisa diklik atau tidak
                if (stage.stageButton != null)
                    stage.stageButton.interactable = isUnlocked;

                // Ganti gambar
                if (stage.stageImage != null)
                {
                    stage.stageImage.sprite =
                        isUnlocked ? stage.unlockedSprite : stage.lockedSprite;
                }
            }
        }

        public void ShowStageInfo(int index)
        {
            if (index < 0 || index >= stages.Length)
            {
                Debug.LogWarning("Stage index out of range: " + index);
                return;
            }

            StageInfo stage = stages[index];

            int unlockedStage = PlayerPrefs.GetInt("UnlockedStage", 1);

            // Cek lock
            if (unlockedStage < stage.stageID)
            {
                Debug.Log("Stage masih terkunci!");
                return;
            }

            titleText.text = stage.actTitle;
            descText.text = stage.description;

            currentStageScene = stage.stageName;

            stageInfoPanel.SetActive(true);

            if (playButton != null)
                playButton.interactable = !string.IsNullOrEmpty(currentStageScene);
        }

        public void PlayStage()
        {
            if (string.IsNullOrEmpty(currentStageScene))
            {
                Debug.LogWarning("No stage selected to play.");
                return;
            }

            SceneManager.LoadScene(currentStageScene);
        }

        public void ClosePanel()
        {
            if (stageInfoPanel != null)
                stageInfoPanel.SetActive(false);

            currentStageScene = null;
        }
    }
}
