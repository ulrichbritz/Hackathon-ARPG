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

        public PlayerManager playerManager;

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
                if (playerManager != null) {
                    FollowCamera.Target.TrackingTarget = playerManager.transform;
                }
                FollowCamera.enabled = true;
            }
            else {
                FollowCamera.enabled = false;
            }
        }

        private void OnEnable()
        {
            if (playerManager != null && FollowCamera.Target.TrackingTarget == null) {
                FollowCamera.Target.TrackingTarget = playerManager.transform;
            }
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

        public void GetNewTarget(PlayerManager newTarget)
        {
            FollowCamera.Target.TrackingTarget = newTarget.transform;
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;

            if (Instance == this) {
                Instance = null;
            }
            FollowCamera = null;
            MainCamera = null;
            playerManager = null;
        }
    }
}
