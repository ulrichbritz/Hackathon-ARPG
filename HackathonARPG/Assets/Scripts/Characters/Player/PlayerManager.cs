using UnityEngine;

namespace UB
{
    public class PlayerManager : CharacterManager
    {
        private PlayerLocomotionManager playerLocomotionManager;
        private PlayerAnimatorManager playerAnimatorManager;

        protected override void Awake()
        {
            base.Awake();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        }

        protected override void Update()
        {
            base.Update();

            if (!IsOwner) {
                return;
            }

            // Handle all player movement
            playerLocomotionManager.HandleAllMovement();
        }
    }
}
