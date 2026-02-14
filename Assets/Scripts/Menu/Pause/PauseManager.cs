using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdventureGame
{
    public class PauseManager : MonoBehaviour
    {
        public GameObject pauseMenu;
        private PlayerInputActions controls;

        private bool isPaused = false;

        void Awake()
        {
            controls = new PlayerInputActions();
            controls.UI.Pause.performed += ctx => TogglePause();
        }

        void OnEnable()
        {
            controls.UI.Enable();
        }

        void OnDisable()
        {
            controls.UI.Disable();
        }

        void TogglePause()
        {
            if (GameManager.GameEnded) return;

            isPaused = !isPaused;

            pauseMenu.SetActive(isPaused);

            if (isPaused)
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        // For button
        public void Resume()
        {
            TogglePause();
        }

        public void QuitGame()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu"); 
        }
    }
}
