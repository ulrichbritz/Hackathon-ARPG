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

            // Move direction based on camera facing perspective and player input
            moveDirection = PlayerCamera.Instance.transform.forward * verticalMovement;
            moveDirection = moveDirection + PlayerCamera.Instance.transform.right * horizontalMovement;
            moveDirection.Normalize();
            // dont want to move up and down only backwards left and right
            moveDirection.y = 0;

            if (PlayerInputManager.Instance.MovementAmount > 0.2f) {
                // move at regular speed (run)
                player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
            }

            // TODO: Maybe add an else for other speeds
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
    }
}
