using UnityEngine;

namespace UB
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager Instance { get; private set; }

        public PlayerUIHudManager PlayerUIHudManager { get; private set; }

        private void Awake()
        {
            CreateInstance();

            PlayerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
        }

        private void CreateInstance()
        {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else {
                Destroy(gameObject);
            }
        }
    }
}
