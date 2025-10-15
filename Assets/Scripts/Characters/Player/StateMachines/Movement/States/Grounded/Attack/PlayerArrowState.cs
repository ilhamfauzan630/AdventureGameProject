using UnityEngine;

namespace AdventureGame
{
    public class PlayerArrowState : PlayerAttackState
    {
        private bool hasShot = false;
        private float postShotTimer = 0f;
        private readonly float postShotDelay = 0.1f;

        private float maxArrowStateTime = 1f;  // fallback
        private float stateTimer = 0f;

        public PlayerArrowState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log(">> Enter Arrow State");

            hasShot = false;
            postShotTimer = 0f;
            stateTimer = 0f;

            StartAnimation(stateMachine.Player.AnimationData.ShootParameterHash);
        }

        public void OnShootArrowEvent()
        {
            if (hasShot) return;

            Debug.Log("🏹 Animation Event: Shoot Arrow");
            hasShot = true;

            stateMachine.Player.Bow?.ShootAutoAim();
        }

        public override void Update()
        {
            base.Update();
            stateTimer += Time.deltaTime;

            if (hasShot)
            {
                postShotTimer += Time.deltaTime;

                if (postShotTimer >= postShotDelay)
                {
                    StopAnimation(stateMachine.Player.AnimationData.ShootParameterHash);
                    stateMachine.ChangeState(stateMachine.AttackState);
                }
            }
            else
            {
                // fallback kalau animation event gak terpanggil
                if (stateTimer >= maxArrowStateTime)
                {
                    Debug.LogWarning("⚠️ [ArrowState] Timeout - Event tidak terpanggil. Force reset.");
                    StopAnimation(stateMachine.Player.AnimationData.ShootParameterHash);
                    stateMachine.ChangeState(stateMachine.AttackState);
                }
            }
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("<< Exit Arrow State");
            StopAnimation(stateMachine.Player.AnimationData.ShootParameterHash);
        }
    }
}
