using UnityEngine;

namespace UB
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        private PlayerManager player;

        [Header("Movement Settings")]
        [HideInInspector]
        public float verticalMovement;
        [HideInInspector]
        public float horizontalMovement;
        [HideInInspector]
        public float moveAmount;
        private Vector3 moveDirection;
        [SerializeField]
        private float runningSpeed = 5f;

        [Header("Animation Parameters")]
        // Relative movement values for animations and networking animations
        [HideInInspector]
        public float RelativeHorizontalMovement;
        [HideInInspector]
        public float RelativeVerticalMovement;

        [Header("Roll Settings")]
        private Vector3 rollDirection;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Update()
        {
            base.Update();

            if (player.IsOwner) {
                player.characterNetworkManager.NetworkAnimatorHorizontalParameter.Value = RelativeHorizontalMovement;
                player.characterNetworkManager.NetworkAnimatorVerticalParameter.Value = RelativeVerticalMovement;
            }
            else {
                //---Movement Animation---//
                RelativeHorizontalMovement = player.characterNetworkManager.NetworkAnimatorHorizontalParameter.Value;
                RelativeVerticalMovement = player.characterNetworkManager.NetworkAnimatorVerticalParameter.Value;

                player.PlayerAnimatorManager.UpdateAnimatorMovementParameters(RelativeHorizontalMovement, RelativeVerticalMovement);
            }
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
            if (player.isPerformingAction) {
                return;
            }

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
            if (player.isPerformingAction) {
                return;
            }

            // Get the movement amount (0.5 for walk, 1.0 for run)
            float movementAmount = PlayerInputManager.Instance.MovementAmount;

            // Calculate movement direction relative to where the player is facing
            Vector3 worldMovement = new Vector3(horizontalMovement, 0, verticalMovement);

            if (worldMovement.magnitude > 0.1f) {
                // Transform world movement to local space relative to player's facing direction
                Vector3 localMovement = transform.InverseTransformDirection(worldMovement);

                // Clamp to either 0 or movementAmount based on direction
                RelativeHorizontalMovement = Mathf.Abs(localMovement.x) > 0.1f ? (localMovement.x > 0 ? movementAmount : -movementAmount) : 0;
                RelativeVerticalMovement = Mathf.Abs(localMovement.z) > 0.1f ? (localMovement.z > 0 ? movementAmount : -movementAmount) : 0;
            }
            else {
                RelativeHorizontalMovement = 0;
                RelativeVerticalMovement = 0;
            }

            player.PlayerAnimatorManager.UpdateAnimatorMovementParameters(RelativeHorizontalMovement, RelativeVerticalMovement);
        }

        private void HandleRotation()
        {
            if (player.isPerformingAction) {
                return;
            }

            Vector3 mouseDirection = PlayerInputManager.Instance.MouseDirection;

            if (mouseDirection != Vector3.zero) {
                // Create rotation towards mouse direction
                Quaternion targetRotation = Quaternion.LookRotation(mouseDirection);

                // Smoothly rotate towards target
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        public void AttemptPerformToRoll()
        {
            if (player.isPerformingAction) {
                return;
            }

            if (moveAmount > 0) {
                // roll in movement direction
                rollDirection = moveDirection;

                rollDirection.y = 0;
                rollDirection.Normalize();

                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                player.transform.rotation = playerRotation;

                player.PlayerAnimatorManager.PlayTargetAnimation("roll_front", true, true, false, 0.2f);
            }
            else {
                // Roll in current facing direction
                player.PlayerAnimatorManager.PlayTargetAnimation("roll_front", true, true, false, 0.2f);
            }


        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            player = null;
        }
    }
}
