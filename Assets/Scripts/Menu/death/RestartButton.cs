using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdventureGame
{
    public class RestartButton : MonoBehaviour
    {
        public void RestartGame()
        {
            Time.timeScale = 1f; 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
