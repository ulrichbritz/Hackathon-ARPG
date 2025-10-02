using UnityEngine;
using UnityEngine.SceneManagement;

namespace UB
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance { get; private set; }
        private PlayerControls playerControls;

        private Vector2 movementInput;

        private void Awake()
        {
            CreateInstance();

            SceneManager.activeSceneChanged += OnSceneChange;

            Instance.enabled = false;
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex == WorldSaveGameManager.Instance.WorldSceneIndex) {
                Instance.enabled = true;
            } else {
                Instance.enabled = false;
            }
        }

        private void OnEnable()
        {
            if (playerControls == null) {
                playerControls = new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
            }

            playerControls.Enable();
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
