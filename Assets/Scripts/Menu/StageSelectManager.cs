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
            [TextArea] public string description;
            public Sprite stageImage;
        }

        [Header("Stage Data")]
        public StageInfo[] stages;

        [Header("UI References")]
        public GameObject stageInfoPanel;     
        public Image stageImageUI;
        public TextMeshProUGUI titleText;  
        public TextMeshProUGUI descText; 
        public Button playButton;  
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
        }

        public void ShowStageInfo(int index)
        {
            if (index < 0 || index >= stages.Length)
            {
                Debug.LogWarning("Stage index out of range: " + index);
                return;
            }

            StageInfo stage = stages[index];

            titleText.text = stage.actTitle;
            descText.text = stage.description;

            if (stage.stageImage != null && stageImageUI != null)
            {
                stageImageUI.sprite = stage.stageImage;
                stageImageUI.gameObject.SetActive(true);
            }
            else if (stageImageUI != null)
            {
                stageImageUI.gameObject.SetActive(false);
            }

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
