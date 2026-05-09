using System.Collections;
using UnityEngine;

namespace AdventureGame
{
    [RequireComponent(typeof(PlayerHealthBridge))]
    public class PlayerDeathHandler : MonoBehaviour
    {
        private Animator animator;
        private PlayerHealthBridge healthBridge;

        private bool hasDied = false;
        public GameObject winPanel;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            healthBridge = GetComponent<PlayerHealthBridge>();
        }

        private void OnEnable()
        {
            healthBridge.OnDeath += OnPlayerDeath;
        }

        private void Start()
        {
            if (winPanel != null)
                winPanel.SetActive(false);
        }

        private void OnDisable()
        {
            healthBridge.OnDeath -= OnPlayerDeath;
        }

        private void OnPlayerDeath()
        {
            if (hasDied) return;
            hasDied = true;

            Debug.Log("💀 PlayerDeathHandler: Player mati!");

            animator.SetTrigger("Die");

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            var input = GetComponent<PlayerInput>();
            if (input != null)
                input.enabled = false;

            StartCoroutine(HandleDeath());
        }

        private IEnumerator HandleDeath()
        {
            yield return new WaitForSeconds(1.5f);

            // freeze game
            Time.timeScale = 0f;

            if (winPanel != null)
                winPanel.SetActive(true);
        }
    }
}