using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UB
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Instance { get; private set; }

        public CinemachineCamera FollowCamera { get; private set; }
        public Camera MainCamera { get; private set; }

        private void Awake()
        {
            CreateInstance();

            SceneManager.activeSceneChanged += OnSceneChange;

            MainCamera = Camera.main;
            FollowCamera = GetComponentInChildren<CinemachineCamera>();
            FollowCamera.enabled = false;
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex == WorldSaveGameManager.Instance.WorldSceneIndex) {
                if (PlayerManager.Instance == null) {
                    Debug.Log("is null");
                }
                FollowCamera.Target = new CameraTarget { TrackingTarget = PlayerManager.Instance.transform };
                FollowCamera.enabled = true;
            }
            else {
                FollowCamera.enabled = false;
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
            }
            else {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }
    }
}
