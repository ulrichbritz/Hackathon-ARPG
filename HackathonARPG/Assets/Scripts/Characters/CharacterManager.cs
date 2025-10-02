using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

namespace UB
{
    public class CharacterManager : NetworkBehaviour
    {
        [HideInInspector]
        public CharacterController characterController { get; private set; }

        private CharacterNetworkManager characterNetworkManager;

        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);

            characterController = GetComponent<CharacterController>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
        }

        protected virtual void Update()
        {
            if (IsOwner) {
                characterNetworkManager.networkPosition.Value = transform.position;
                characterNetworkManager.networkRotation.Value = transform.rotation;
            }
            else {
                // Position
                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    characterNetworkManager.networkPosition.Value,
                    ref characterNetworkManager.NetworkPositionVelocity,
                    characterNetworkManager.NetworkPositionSmoothTime
                    );
                // Rotation
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    characterNetworkManager.networkRotation.Value,
                    characterNetworkManager.NetworkRotationSmoothTime
                    );
            }
        }
    }
}
