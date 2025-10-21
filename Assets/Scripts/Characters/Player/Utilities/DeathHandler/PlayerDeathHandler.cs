using UnityEngine;

namespace AdventureGame
{
    [RequireComponent(typeof(PlayerHealthBridge))]
    public class PlayerDeathHandler : MonoBehaviour
    {
        private Animator animator;
        private PlayerHealthBridge healthBridge;

        private bool hasDied = false;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            healthBridge = GetComponent<PlayerHealthBridge>();
        }

        private void OnEnable()
        {
            healthBridge.OnDeath += OnPlayerDeath;
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

            // TODO: nanti tampilkan Game Over UI
            // GameOverUI.Instance.Show();

            var input = GetComponent<PlayerInput>();
            if (input != null)
                input.enabled = false;
        }
    }
}
