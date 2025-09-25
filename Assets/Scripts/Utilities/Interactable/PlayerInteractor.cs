using UnityEngine;
using UnityEngine.InputSystem;

namespace AdventureGame
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInteractor : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float interactDistance = 3f;
        [SerializeField] private LayerMask interactableMask;

        private Interactable current;           // object yg sedang bisa di-interact
        private PlayerInputActions inputActions; // hasil generate dari Input System

        void Awake()
        {
            inputActions = new PlayerInputActions();
        }

        void OnEnable()
        {
            inputActions.Player.Enable();
            inputActions.Player.Interact.performed += OnInteract;
        }

        void OnDisable()
        {
            inputActions.Player.Interact.performed -= OnInteract;
            inputActions.Player.Disable();
        }

        void Update()
        {
            Detect();
        }

        void Detect()
        {
            // reset highlight
            if (current != null) current.SetHighlighted(false);
            current = null;

            // ray dari player, set tinggi kira-kira dada
            Vector3 origin = transform.position + Vector3.up * 1.5f;
            Vector3 direction = transform.forward;

            Debug.DrawRay(origin, direction * interactDistance, Color.green);

            if (Physics.Raycast(origin, direction, out RaycastHit hit, interactDistance, interactableMask))
            {
                current = hit.collider.GetComponentInParent<Interactable>();
                if (current != null)
                {
                    current.SetHighlighted(true);
                    UImanager.Instance?.ShowPressE("[E] " + current.promptLabel);
                    return;
                }
            }

            // kalau tidak ada interactable
            UImanager.Instance?.HidePressE();
        }

        private void OnInteract(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) return;

            Debug.Log(">>> Tombol Interact (E) ditekan!");

            if (current != null)
            {
                Debug.Log("Interacting dengan object: " + current.name);
                current.Interact();
            }
            else
            {
                Debug.Log("Tidak ada object di depan untuk di-interact.");
            }
        }
    }
}

