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
            } else {
                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    characterNetworkManager.networkPosition.Value,
                    ref characterNetworkManager.NetworkPositionVelocity,
                    characterNetworkManager.NetworkPositionSmoothTime
                    );
            }
        }
    }
}
