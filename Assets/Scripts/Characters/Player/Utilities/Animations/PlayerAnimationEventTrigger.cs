using UnityEngine;

namespace AdventureGame
{
    public class PlayerAnimationEventTrigger : MonoBehaviour
    {
        private Player player;

        private void Awake()
        {
            player = transform.parent.GetComponent<Player>();
        }

        public void TriggerOnMovementStateAnimationEnterEvent()
        {
            if (IsInAnimationTransition())
            {
                return;
            }

            player.OnMovementStateAnimationEnterEvent();
        }

        public void TriggerOnMovementStateAnimationExitEvent()
        {
            if (IsInAnimationTransition())
            {
                return;
            }

            player.OnMovementStateAnimationExitEvent();
        }

        public void TriggerOnMovementStateAnimationTransitionEvent()
        {
            if (IsInAnimationTransition())
            {
                return;
            }

            player.OnMovementStateAnimationTransitionEvent();
        }

        private bool IsInAnimationTransition(int layerIndex = 0)
        {
            return player.Animator.IsInTransition(layerIndex);
        }

        public void TriggerOnShootArrowEvent()
        {
            // pastikan player selalu valid
            if (player == null)
            {
                player = GetComponentInParent<Player>();
            }

            // Hindari terlalu ketat pakai IsInAnimationTransition di sini
            // Bisa kamu hapus atau pindahkan logikanya ke state kalau perlu
            player.OnShootArrowEvent();
        }
    }
}