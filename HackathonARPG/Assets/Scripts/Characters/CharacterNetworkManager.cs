using UnityEngine;
using Unity.Netcode;

namespace UB
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
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

        //TODO not sure if we even need move amount with our movement
        /*
        public NetworkVariable<float> networkMoveAmountParameter = new NetworkVariable<float>(
        0f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
        );
        */

    }
}
