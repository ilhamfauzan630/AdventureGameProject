using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdventureGame
{
    public class MainMenuUI : MonoBehaviour
    {
        public void StartGame()
    {
        SceneManager.LoadScene("StageSelect");
    }

    public void OpenOptions()
    {
        Debug.Log("Options menu belum dibuat.");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    }
}
