using UnityEngine.InputSystem;

namespace AdventureGame
{
    public class PlayerRunningState : PlayerMovingState
    {
        public PlayerRunningState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            stateMachine.ReusableData.MovementSpeedModifier = movementData.RunData.SpeedModifier;
        }

        #endregion

        #region Input Methods
        protected override void OnwalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnwalkToggleStarted(context);

            stateMachine.ChangeState(stateMachine.WalkingState);
        }
    
        #endregion
    }
}
