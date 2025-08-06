using UnityEngine.InputSystem;

namespace AdventureGame
{
    public class PlayerWalkingState : PlayerMovingState
    {
        public PlayerWalkingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            stateMachine.ReusableData.MovementSpeedModifier = movementData.WalkData.SpeedModifier;
        }
        #endregion

        #region Input Methods
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.LightStoppingState);
        }
        protected override void OnwalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnwalkToggleStarted(context);

            stateMachine.ChangeState(stateMachine.RunningState);
        }    
        #endregion
        }
}
