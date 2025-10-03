using UnityEngine;

namespace UB
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        private PlayerManager player;

        [HideInInspector]
        public float verticalMovement;
        [HideInInspector]
        public float horizontalMovement;
        [HideInInspector]
        public float moveAmount;

        private Vector3 moveDirection;

        [SerializeField]
        private float runningSpeed = 5f;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public void HandleAllMovement()
        {
            // Handle Grounded Movement
            HandleGroundedMovement();


            HandleAnimationParameters();

            // Rotation
            HandleRotation();

            // Ground Check
        }

        private void GetVerticalAndHorizontalInputs()
        {
            verticalMovement = PlayerInputManager.Instance.verticalInput;
            horizontalMovement = PlayerInputManager.Instance.horizontalInput;

            // TODO maybe clamp the movement amount
        }

        private void HandleGroundedMovement()
        {
            GetVerticalAndHorizontalInputs();

            // Early exit if no movement
            float movementAmount = PlayerInputManager.Instance.MovementAmount;
            if (movementAmount <= 0)
                return;

            moveDirection.x = horizontalMovement;
            moveDirection.y = 0; // Keep on ground plane
            moveDirection.z = verticalMovement;

            // Normalize only if needed
            if (moveDirection.sqrMagnitude > 1f) {
                moveDirection.Normalize();
            }

            if (movementAmount > 0.5f) {
                // move at regular speed (run)
                player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
            }
            else if (movementAmount > 0) {
                // move at walk speed
                player.characterController.Move(moveDirection * (runningSpeed * 0.5f) * Time.deltaTime);
            }
        }

        private void HandleAnimationParameters()
        {
            // Calculate movement direction relative to where the player is facing
            Vector3 worldMovement = new Vector3(horizontalMovement, 0, verticalMovement);

            if (worldMovement.magnitude > 0.1f) {
                // Transform world movement to local space relative to player's facing direction
                Vector3 localMovement = transform.InverseTransformDirection(worldMovement);

                // Update animator with relative movement values
                // localMovement.x = strafe (left/right relative to facing direction)
                // localMovement.z = forward/back relative to facing direction
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(localMovement.x, localMovement.z);
            }
            else {
                // No movement - idle
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, 0);
            }
        }

        private void HandleRotation()
        {
            Vector3 mouseDirection = PlayerInputManager.Instance.MouseDirection;

            if (mouseDirection != Vector3.zero) {
                // Create rotation towards mouse direction
                Quaternion targetRotation = Quaternion.LookRotation(mouseDirection);

                // Smoothly rotate towards target
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            player = null;
        }
    }
}
