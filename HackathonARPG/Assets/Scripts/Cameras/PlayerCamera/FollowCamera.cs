using UnityEngine;

namespace UB
{
    public class FollowCamera : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
