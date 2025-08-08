using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AdventureGame
{
    public class PlayerDashingState : PlayerGroundedState
    {
        private PlayerDashData dashData;
        private float startTime;
        private int ConsecutiveDashesUsed;
        private bool shouldKeepRotating;

        public PlayerDashingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            dashData = movementData.DashData;
        }


        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            stateMachine.ReusableData.MovementSpeedModifier = dashData.SpeedModifier;

            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StrongForce;

            stateMachine.ReusableData.RotationData = dashData.RotationData;

            AddForceOnTransitionFromStationaryState();

            shouldKeepRotating = stateMachine.ReusableData.MovementInput != Vector2.zero;

            UpdateConsecutiveDashes();

            startTime = Time.time;
        }

        public override void Exit()
        {
            base.Exit();

            SetBaseRotationData();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (!shouldKeepRotating)
            {
                return;
            }

            RotateTowardsTargetRotation();
        }

        public override void OnAnimationTransitionEvent()
        {
            if (stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.HardStoppingState);

                return;
            }

            stateMachine.ChangeState(stateMachine.SprintingState);
        }
        #endregion

        #region Main Methods
        private void AddForceOnTransitionFromStationaryState()
        {
            if (stateMachine.ReusableData.MovementInput != Vector2.zero)
            {
                return;
            }

            Vector3 characterRotationDirection = stateMachine.Player.transform.forward;

            characterRotationDirection.y = 0f;

            UpdateTargetRotation(characterRotationDirection, false);

            stateMachine.Player.Rigidbody.velocity = characterRotationDirection * getMovementSpeed();
        }

        private void UpdateConsecutiveDashes()
        {
            if (IsConsecutive())
            {
                ConsecutiveDashesUsed = 0;
            }

            ++ConsecutiveDashesUsed;

            if (ConsecutiveDashesUsed == dashData.ConsecutiveDashedLimitAmound)
            {
                ConsecutiveDashesUsed = 0;

                stateMachine.Player.Input.DisableActionFor(stateMachine.Player.Input.PlayerActions.Dash, dashData.DashLimitReachedCooldown);
            }
        }

        private bool IsConsecutive()
        {
            return Time.time < startTime + dashData.TimeToBeConsideredConsecutive;
        }
        #endregion

        #region Reusable Methods
        protected override void AddInputActionsCallback()
        {
            base.AddInputActionsCallback();

            stateMachine.Player.Input.PlayerActions.Movement.performed += OnMovementPerformed;
        }

        protected override void RemoveInputActionsCallback()
        {
            base.RemoveInputActionsCallback();

            stateMachine.Player.Input.PlayerActions.Movement.performed -= OnMovementPerformed;
        }
        #endregion

        #region Input Methods

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
        }
        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            shouldKeepRotating = true;
        }
        protected override void OnDashStarted(InputAction.CallbackContext context)
        {
        }
        #endregion
    }
}
