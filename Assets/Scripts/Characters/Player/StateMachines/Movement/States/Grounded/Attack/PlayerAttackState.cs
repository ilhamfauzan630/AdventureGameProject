using UnityEngine;
using UnityEngine.InputSystem;

namespace AdventureGame
{
    public class PlayerAttackState : PlayerGroundedState
    {
        public PlayerAttackState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log(">> Enter Attack State");

            StartAnimation(stateMachine.Player.AnimationData.AttackingParameterHash);

            // Kurangi kecepatan saat mode attack
            stateMachine.ReusableData.MovementSpeedModifier = 2f;
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("<< Exit Attack State");

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
            // Klik kiri mouse → masuk ke ArrowState
            stateMachine.ChangeState(stateMachine.ArrowState);
        }
    }
}
