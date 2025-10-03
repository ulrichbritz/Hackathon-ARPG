using UnityEngine;

namespace UB
{
    public class WorldSoundFXManager : MonoBehaviour
    {
        public static WorldSoundFXManager Instance { get; private set; }

        [Header("Action Sound FX")]
        public AudioClip rollSoundFX;

        private void Awake()
        {
            CreateInstance();
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
