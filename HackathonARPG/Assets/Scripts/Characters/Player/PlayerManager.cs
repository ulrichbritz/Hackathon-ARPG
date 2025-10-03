using Unity.Cinemachine;
using UnityEngine;

namespace UB
{
    public class PlayerManager : CharacterManager
    {
        public static PlayerManager Instance { get; private set; }
        public PlayerLocomotionManager PlayerLocomotionManager { get; private set; }
        public PlayerAnimatorManager PlayerAnimatorManager { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            PlayerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            PlayerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        }

        protected override void Update()
        {
            base.Update();

            if (!IsOwner) {
                return;
            }

            // Handle all player movement
            PlayerLocomotionManager.HandleAllMovement();
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

        public override void OnDestroy()
        {
            base.OnDestroy();

            if (Instance == this) {
                Instance = null;
            }

            PlayerLocomotionManager = null;
            PlayerAnimatorManager = null;
        }
    }
}
