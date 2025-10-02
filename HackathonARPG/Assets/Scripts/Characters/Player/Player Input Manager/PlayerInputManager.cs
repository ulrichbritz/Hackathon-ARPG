using UnityEngine;
using UnityEngine.SceneManagement;

namespace UB
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance { get; private set; }
        private PlayerControls playerControls;

        private Vector2 movementInput;
        public float verticalInput { get; private set; }
        public float horizontalInput { get; private set; }
        public float MovementAmount { get; private set; }

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
            }
            else {
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

        private void Update()
        {
            HandleMovementInput();
        }

        //Movement Input
        private void HandleMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            MovementAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

            // TODO: Maybe clamp e.g if moveamount <= 05f, then moveamount = 0.5f;
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

        private void OnApplicationFocus(bool focus)
        {
            if (enabled) {
                if (focus) {
                    playerControls.Enable();
                }
                else {
                    playerControls.Disable();
                }
            }
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }
    }
}
