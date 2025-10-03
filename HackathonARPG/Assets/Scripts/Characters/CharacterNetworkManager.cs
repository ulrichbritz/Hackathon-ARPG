using UnityEngine;
using Unity.Netcode;

namespace UB
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
        private CharacterManager character;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        [Header("Position")]
        public NetworkVariable<Vector3> NetworkPosition = new NetworkVariable<Vector3>(
        Vector3.zero,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
        );

        public Vector3 NetworkPositionVelocity;
        public float NetworkPositionSmoothTime = 0.1f;

        [Header("Rotation")]
        public NetworkVariable<Quaternion> NetworkRotation = new NetworkVariable<Quaternion>(
        Quaternion.identity,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
        );

        public float NetworkRotationSmoothTime = 0.1f;

        [Header("Animator")]
        public NetworkVariable<float> NetworkAnimatorHorizontalParameter = new NetworkVariable<float>(
        0f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
        );

        public NetworkVariable<float> NetworkAnimatorVerticalParameter = new NetworkVariable<float>(
        0f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
        );


        // Called from client to server
        [ServerRpc]
        public void NotifyServerOfActionAnimationServerRPC(ulong clientID, string animationID, bool applyRootMotion)
        {
            // If this character is the SERVER/HOST then activate the client rpc
            if (IsServer) {
                PlayActionAnimationForAllClientsClientRpc(clientID, animationID, applyRootMotion);
            }
        }

        // Called from server to all clients
        [ClientRpc]
        public void PlayActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            // Making sure we don't play the animation twice on the character who sent it
            if (clientID != NetworkManager.Singleton.LocalClientId) {
                PerformActionAnimationFromServer(animationID, applyRootMotion);
            }
        }

        [Header("Stats")]
        public NetworkVariable<int> Strength = new NetworkVariable<int>(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner
        );

        public NetworkVariable<int> Constitution = new NetworkVariable<int>(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner
        );

        public NetworkVariable<int> Dexterity = new NetworkVariable<int>(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner
        );

        public NetworkVariable<int> Intelligence = new NetworkVariable<int>(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner
        );

        public NetworkVariable<int> Wisdom = new NetworkVariable<int>(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner
        );

        public NetworkVariable<int> Charisma = new NetworkVariable<int>(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner
        );

        public NetworkVariable<int> CurrentMana = new NetworkVariable<int>(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner
        );

        public NetworkVariable<int> MaxMana = new NetworkVariable<int>(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner
        );

        private void PerformActionAnimationFromServer(string animationID, bool applyRootMotion)
        {
            character.animator.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(animationID, 0.2f);
        }

    }
}
