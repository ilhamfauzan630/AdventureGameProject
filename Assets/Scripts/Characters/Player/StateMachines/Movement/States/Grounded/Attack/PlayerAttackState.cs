using UnityEngine;
using UnityEngine.InputSystem;

namespace AdventureGame
{
    public class PlayerAttackState : PlayerGroundedState
    {
        private float attackTimer;
        private readonly float maxAttackDuration = 2f; // ⏱️ Waktu maksimum dalam Attack State

        public PlayerAudio playerAudio;
        public PlayerAttackState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
            playerAudio = stateMachine.Player.GetComponentInChildren<PlayerAudio>();
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log(">> Enter Attack State");

            playerAudio.StartRunFast();
            StartAnimation(stateMachine.Player.AnimationData.AttackingParameterHash);

            stateMachine.ReusableData.MovementSpeedModifier = 3f;

            // Reset timer setiap masuk Attack State
            attackTimer = 0f;
        }

        public override void Update()
        {
            base.Update();

            // Jalanin timer
            attackTimer += Time.deltaTime;

            if (attackTimer >= maxAttackDuration)
            {
                Debug.Log("[AttackState] Timer habis → Kembali ke RunningState");
                stateMachine.ChangeState(stateMachine.RunningState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("<< Exit Attack State");
            
            if (stateMachine.CurrentState != stateMachine.ArrowState)
            {
                playerAudio.StopRunFast();
            }

            StopAnimation(stateMachine.Player.AnimationData.AttackingParameterHash);
        }

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();
            stateMachine.Player.Input.PlayerActions.Fire.started += OnShootStarted;
        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
            stateMachine.Player.Input.PlayerActions.Fire.started -= OnShootStarted;
        }

        private void OnShootStarted(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.ArrowState);
        }
    }
}
