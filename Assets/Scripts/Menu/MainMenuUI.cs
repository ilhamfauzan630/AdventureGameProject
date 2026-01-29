using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdventureGame
{
    public class MainMenuUI : MonoBehaviour
    {
        public GameObject optionsPanel;

        public void StartGame()
        {
            SceneManager.LoadScene("StageSelect");
        }

        public void OpenOptions()
        {
            optionsPanel.SetActive(true);
        }

        public void CloseOptions()
        {
            optionsPanel.SetActive(false);
        }

        public void QuitGame()
        {
            Application.Quit();

            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}
