using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AdventureGame
{
    public class PlayerSprintingState : PlayerGroundedState
    {
        private PlayerSprintData sprintData;

        private float startTime;

        private bool keepSprinting;
        public PlayerSprintingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            sprintData = movementData.SprintData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            stateMachine.ReusableData.MovementSpeedModifier = sprintData.SpeedModifier;

            startTime = Time.time;
        }

        public override void Exit()
        {
            base.Exit();

            keepSprinting = false;
        }

        public override void Update()
        {
            base.Update();

            if (keepSprinting)
            {
                return;
            }

            if (Time.time < startTime + sprintData.SprintToRunTime)
            {
                return;
            }

            StopSprinting();
        }

        #region Main Methods
        private void StopSprinting()
        {
            if (stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.IdlingState);

                return;
            }

            stateMachine.ChangeState(stateMachine.RunningState);
        }

        #endregion

        #endregion

        #region Reusable Methods
        protected override void AddInputActionsCallback()
        {
            base.AddInputActionsCallback();

            stateMachine.Player.Input.PlayerActions.Sprint.performed += OnSprintPerformed;
        }

        protected override void RemoveInputActionsCallback()
        {
            base.RemoveInputActionsCallback();

            stateMachine.Player.Input.PlayerActions.Sprint.performed -= OnSprintPerformed;
        }
        #endregion

        #region Input Methods
        private void OnSprintPerformed(InputAction.CallbackContext context)
        {
            keepSprinting = true;
        }
        #endregion
    }
}
