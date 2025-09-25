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
            public string stageName;      // nama scene yang akan di-load, ex: "Stage_Jawa"
            public string actTitle;       // judul yang tampil, ex: "Act 1: Pulau Jawa"
            [TextArea] public string description; // deskripsi singkat
            public Sprite stageImage;     // thumbnail untuk panel info
        }

        [Header("Stage Data")]
        public StageInfo[] stages; // isi melalui Inspector

        [Header("UI References")]
        public GameObject stageInfoPanel;      // panel yang berisi info stage
        public Image stageImageUI;             // UI Image untuk thumbnail
        public TextMeshProUGUI titleText;      // judul
        public TextMeshProUGUI descText;       // deskripsi
        public Button playButton;              // tombol play di panel

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

        // Dipanggil saat tombol pulau diklik (pakai StageButton atau OnClick di Inspector)
        public void ShowStageInfo(int index)
        {
            if (index < 0 || index >= stages.Length)
            {
                Debug.LogWarning("Stage index out of range: " + index);
                return;
            }

            StageInfo stage = stages[index];

            // Update UI
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

            // Simpan nama scene untuk PlayStage
            currentStageScene = stage.stageName;

            // Aktifkan panel
            stageInfoPanel.SetActive(true);

            // Pastikan tombol play aktif jika scene di-set
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

            // Load scene gameplay
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
