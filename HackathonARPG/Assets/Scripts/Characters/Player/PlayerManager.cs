using Unity.Cinemachine;
using UnityEngine;

namespace UB
{
    public class PlayerManager : CharacterManager
    {
        public static PlayerManager Instance { get; private set; }
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

        private void CreateInstance()
        {
            if (Instance == null) {
                Instance = this;
            }
        }

        protected override void OnNetworkPostSpawn()
        {
            base.OnNetworkPostSpawn();

            if (IsOwner) {
                CreateInstance();

                PlayerCamera.Instance.GetNewTarget(this);
            }
        }
    }
}
