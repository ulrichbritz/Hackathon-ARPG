using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UB
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Instance { get; private set; }

        private CinemachineCamera followCamera;

        private void Awake()
        {
            CreateInstance();

            SceneManager.activeSceneChanged += OnSceneChange;

            followCamera = GetComponentInChildren<CinemachineCamera>();
            followCamera.enabled = false;
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex == WorldSaveGameManager.Instance.WorldSceneIndex) {
                followCamera.enabled = true;
            } else {
                //followCamera.enabled = false;
            }
        }

        private void OnEnable()
        {
            // Get the player as target
        }

        private void CreateInstance()
        {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }
    }
}
