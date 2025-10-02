using UnityEngine;
using Unity.Netcode;

namespace UB
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
        [Header("Position")]
        public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(
        Vector3.zero,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
        );

        public Vector3 NetworkPositionVelocity;
        public float NetworkPositionSmoothTime = 0.1f;
    }
}
