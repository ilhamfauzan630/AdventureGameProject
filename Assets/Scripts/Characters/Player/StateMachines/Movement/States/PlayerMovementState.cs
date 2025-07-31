using System;
using UnityEngine;

namespace AdventureGame
{
    public class PlayerMovementState : IState
    {
        protected PlayerMovementStateMachine stateMachine;

        protected Vector2 movementInput;

        protected float baseSpeed = 5f;
        protected float speedModifier = 1f;

        public PlayerMovementState(PlayerMovementStateMachine playerMovementStateMachine)
        {
            stateMachine = playerMovementStateMachine;
        }

        #region Istate Methods
        public void Enter()
        {
            Debug.Log("State : " + GetType().Name);
        }

        public void Exit()
        {

        }

        public void HandleInput()
        {
            ReadMovementInput();
        }

        public void PhysicsUpdate()
        {
            Move();
        }


        public void Update()
        {

        }
        #endregion

        #region Main Methods
        private void ReadMovementInput()
        {
            movementInput = stateMachine.Player.Input.PlayerActions.Movement.ReadValue<Vector2>();
        }
        private void Move()
        {
            if (movementInput == Vector2.zero || speedModifier == 0f)
            {
                return;
            }

            Vector3 movementDirection = GetMovementDirection();

            float movementSpeed = getMovementSpeed();

            Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

            stateMachine.Player.Rigidbody.AddForce(movementDirection * movementSpeed - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
        }



        #endregion

        #region Reusable Methods
        protected Vector3 GetMovementDirection()
        {
            return new Vector3(movementInput.x, 0f, movementInput.y);
        }
        
        protected float getMovementSpeed()
        {
            return baseSpeed * speedModifier;
        }

        protected Vector3 GetPlayerHorizontalVelocity()
        {
            Vector3 PlayerHorizontalVelocity = stateMachine.Player.Rigidbody.velocity;

            PlayerHorizontalVelocity.y = 0f;

            return PlayerHorizontalVelocity;
        }
        #endregion
    }
}
