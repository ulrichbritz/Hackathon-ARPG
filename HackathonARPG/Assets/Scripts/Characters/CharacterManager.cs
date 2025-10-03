using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

namespace UB
{
    public class CharacterManager : NetworkBehaviour
    {
        public CharacterController characterController { get; private set; }
        public Animator animator { get; private set; }

        public CharacterNetworkManager characterNetworkManager { get; private set; }

        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);

            characterController = GetComponent<CharacterController>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();

            animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            if (IsOwner) {
                characterNetworkManager.NetworkPosition.Value = transform.position;
                characterNetworkManager.NetworkRotation.Value = transform.rotation;
            }
            else {
                //---Position---//
                // Temporarily disable CharacterController to avoid conflicts with direct position assignment
                bool wasEnabled = characterController.enabled;
                characterController.enabled = false;

                // direct assignment for perfect sync
                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    characterNetworkManager.NetworkPosition.Value,
                    ref characterNetworkManager.NetworkPositionVelocity,
                    characterNetworkManager.NetworkPositionSmoothTime
                    );

                // Re-enable CharacterController
                characterController.enabled = wasEnabled;

                /* Original way that had conflicts due to CharacterController:
                // Position
                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    characterNetworkManager.NetworkPosition.Value,
                    ref characterNetworkManager.NetworkPositionVelocity,
                    characterNetworkManager.NetworkPositionSmoothTime
                    );
                */

                //--- Rotation ---//
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    characterNetworkManager.NetworkRotation.Value,
                    characterNetworkManager.NetworkRotationSmoothTime
                    );
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            characterController = null;
            animator = null;
            characterNetworkManager = null;
        }
    }
}
