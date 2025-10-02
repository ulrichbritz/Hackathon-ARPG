using UnityEngine;

namespace UB
{
    public class WorldPlayerUIManager : MonoBehaviour
    {
        public static WorldPlayerUIManager Instance { get; private set; }

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
