using Unity.Cinemachine;
using UnityEngine;

namespace UB
{
    public class PlayerManager : CharacterManager
    {
        public static PlayerManager Instance { get; private set; }
        public PlayerLocomotionManager playerLocomotionManager { get; private set; }
        public PlayerAnimatorManager playerAnimatorManager { get; private set; }

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
            Instance = this;
        }

        protected override void OnNetworkPostSpawn()
        {
            base.OnNetworkPostSpawn();

            if (IsOwner) {
                CreateInstance();
                PlayerCamera.Instance.playerManager = this;
                PlayerCamera.Instance.GetNewTarget(this);
                PlayerInputManager.Instance.player = this;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (Instance == this) {
                Instance = null;
            }

            playerLocomotionManager = null;
            playerAnimatorManager = null;
        }
    }
}
