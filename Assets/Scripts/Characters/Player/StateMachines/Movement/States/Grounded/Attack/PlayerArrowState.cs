using UnityEngine;

namespace AdventureGame
{
    public class PlayerArrowState : PlayerAttackState
    {
        private bool hasShot = false;
        private float postShotTimer = 0f;
        private readonly float postShotDelay = 0.1f; // jeda kecil agar animator sempat reset

        public PlayerArrowState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log(">> [ArrowState] ENTER");

            hasShot = false;
            postShotTimer = 0f;

            // Aktifkan animasi shoot
            Debug.Log("[ArrowState] Start Shoot Animation");
            StartAnimation(stateMachine.Player.AnimationData.ShootParameterHash);
        }

        // Dipanggil dari Animation Event saat panah dilepaskan
        public void OnShootArrowEvent()
        {
            Debug.Log($"[ArrowState] OnShootArrowEvent called | hasShot = {hasShot}");

            if (hasShot)
            {
                Debug.LogWarning("[ArrowState] OnShootArrowEvent IGNORED (hasShot already true)");
                return;
            }

            Debug.Log("🏹 [ArrowState] PANAH DITEMBAK!");
            hasShot = true;

            if (stateMachine.Player.Bow != null)
            {
                Debug.Log("[ArrowState] Bow found → ShootAutoAim()");
                stateMachine.Player.Bow.ShootAutoAim();
            }
            else
            {
                Debug.LogWarning("[ArrowState] Bow is NULL!");
            }
        }

        public override void Update()
        {
            base.Update();

            if (hasShot)
            {
                postShotTimer += Time.deltaTime;
                Debug.Log($"[ArrowState] hasShot=true | postShotTimer={postShotTimer:F3}/{postShotDelay}");

                if (postShotTimer >= postShotDelay)
                {
                    Debug.Log("[ArrowState] postShotTimer selesai → StopAnimation + ChangeState(Attack)");
                    StopAnimation(stateMachine.Player.AnimationData.ShootParameterHash);

                    // Kembali ke AttackState setelah animasi selesai
                    stateMachine.ChangeState(stateMachine.AttackState);
                }
            }
            else
            {
                // Debug kalau belum nembak
                Debug.Log("[ArrowState] Menunggu Animation Event (OnShootArrowEvent)...");
            }
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("<< [ArrowState] EXIT");

            // Pastikan animasi shoot dimatikan saat keluar state
            Debug.Log("[ArrowState] Stop Shoot Animation (on Exit)");
            StopAnimation(stateMachine.Player.AnimationData.ShootParameterHash);
        }
    }
}