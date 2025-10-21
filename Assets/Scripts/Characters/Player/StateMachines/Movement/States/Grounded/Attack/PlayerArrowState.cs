using UnityEngine;

namespace AdventureGame
{
    public class PlayerArrowState : PlayerAttackState
    {
        private bool hasShot = false;

        public PlayerArrowState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Debug.Log(">> Enter Arrow State");
            StartAnimation(stateMachine.Player.AnimationData.ShootParameterHash);
            hasShot = false;
        }

        public void OnShootArrowEvent()
        {
            if (hasShot) return;

            Debug.Log("🏹 Animation Event: Shoot Arrow");
            hasShot = true;

            if (stateMachine.Player.Bow != null)
            {
                stateMachine.Player.Bow.ShootAutoAim();
                stateMachine.ChangeState(stateMachine.AttackState);
            }
        }

        public override void OnAnimationExitEvent()
        {
            // Tidak perlu ganti state di sini
        }

        public override void Exit()
        {
            base.Exit();

            Debug.Log("<< Exit Arrow State");
            StopAnimation(stateMachine.Player.AnimationData.ShootParameterHash);
        }
    }
}
