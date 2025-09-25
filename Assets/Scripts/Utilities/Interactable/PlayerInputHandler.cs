using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureGame
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public PlayerInputActions InputActions { get; private set; }
        public PlayerInputActions.PlayerActions PlayerActions { get; private set; }

        private void Awake()
        {
            // inisialisasi input actions
            InputActions = new PlayerInputActions();
            PlayerActions = InputActions.Player;
        }

        private void OnEnable() => InputActions.Enable();
        private void OnDisable() => InputActions.Disable();
    }
}
