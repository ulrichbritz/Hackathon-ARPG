using Unity.Cinemachine;
using UnityEngine;

namespace UB
{
    public class PlayerManager : CharacterManager
    {
        public static PlayerManager Instance { get; private set; }
        public PlayerLocomotionManager PlayerLocomotionManager { get; private set; }
        public PlayerAnimatorManager PlayerAnimatorManager { get; private set; }
        public PlayerNetworkManager PlayerNetworkManager { get; private set; }
        public PlayerStatsManager PlayerStatsManager { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            PlayerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            PlayerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            PlayerNetworkManager = GetComponent<PlayerNetworkManager>();
            PlayerStatsManager = GetComponent<PlayerStatsManager>();
        }

        protected override void Update()
        {
            base.Update();

            if (!IsOwner) {
                return;
            }

            // Handle all player movement
            PlayerLocomotionManager.HandleAllMovement();

            // Regenerate mana over time
            PlayerStatsManager.RegenerateMana();
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

                PlayerNetworkManager.CurrentMana.OnValueChanged += PlayerUIManager.Instance.PlayerUIHudManager.SetNewManaValue;
                PlayerNetworkManager.CurrentMana.OnValueChanged += PlayerStatsManager.ResetManaRegenerationTimer;

                // This will be moved when saving and loading is added
                // filling mana based on wisdom stat
                PlayerNetworkManager.MaxMana.Value = PlayerStatsManager.CalculateManaBasedOnWisdom(PlayerNetworkManager.Wisdom.Value);
                PlayerNetworkManager.CurrentMana.Value = PlayerStatsManager.CalculateManaBasedOnWisdom(PlayerNetworkManager.Wisdom.Value);
                PlayerUIManager.Instance.PlayerUIHudManager.SetMaxManaValue(PlayerNetworkManager.MaxMana.Value);
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
            PlayerNetworkManager = null;
            PlayerStatsManager = null;
        }
    }
}
